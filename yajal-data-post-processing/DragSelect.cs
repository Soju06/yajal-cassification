using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;

namespace yajal_data_post_processing {
    internal class DragSelect : Control {
        Image? _image;
        Bitmap? _resized_image;
        string disabledText = "Disabled";
        List<RectangleF> SelectedRectangles = new();
        BufferedGraphics bufferedGraphics;
        Rectangle _r_rect;

        public event DragSelectedEventHandler DragSelected;

        public DragSelect() {
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);
            using var g = CreateGraphics();
            bufferedGraphics = BufferedGraphicsManager.Current.Allocate(g,
                new Rectangle(0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height));
        }

        public List<RectangleF> ClientSelected => SelectedRectangles;


        public string DisabledText {
            get => disabledText;
            set {
                disabledText = value;
                Refresh();
            }
        }

        public Image? Image {
            get => _image;
            set {
                _image = value;
                var o_img = _resized_image;
                o_img?.Dispose();
                _resized_image = null;
                Refresh();
            }
        }

        public Bitmap? GetClippingImage(Rectangle rectangle) {
            if (_image == null) return null;
            return ClippingImage((Bitmap)_image, rectangle);
        }

        public Bitmap? GetClippingImage(RectangleF rect) =>
            GetClippingImage(new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height));

        public Rectangle GetSourceRectangle(RectangleF clientRectangle) {
            if (_image == null) return new((int)clientRectangle.X, (int)clientRectangle.Y, (int)clientRectangle.Width, (int)clientRectangle.Height);
            var w_s = _image.Width / (double)_r_rect.Width;
            var h_s = _image.Height / (double)_r_rect.Height;
            return new((int)((clientRectangle.X - _r_rect.X) * w_s), 
                       (int)((clientRectangle.Y - _r_rect.Y) * h_s),
                       (int)(clientRectangle.Width * w_s),
                       (int)(clientRectangle.Height * h_s));
        }

        protected override void OnPaint(PaintEventArgs e) {
            var graphics = bufferedGraphics.Graphics;
            graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            var rect = ClientRectangle;

            using (var brush = new SolidBrush(BackColor))
                graphics.FillRectangle(brush, rect);

            if (_image != null) {
                ResizeProcessing();
                if (_resized_image != null)
                    using (var i = HighlightProcessing(_resized_image))
                        graphics.DrawImageUnscaled(i, 0, 0);
            }

            if(!Enabled)
                DisabledProcessing(graphics, rect);

            bufferedGraphics.Render(e.Graphics);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent) {

        }

        bool mouse_down;
        int start_x, start_y, start_r, start_b;

        protected override void OnMouseDown(MouseEventArgs e) {
            if (!mouse_down && e.Button == MouseButtons.Left) {
                mouse_down = true;
                start_x = e.X;
                start_y = e.Y;
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e) {
            if (mouse_down) {
                lock (SelectedRectangles) {
                    mouse_down = false;
                    var rect = new RectangleF(start_x, start_y, start_r - start_x, start_b - start_y);

                    if (rect.Width > 1 && rect.Height > 1) {
                        SelectedRectangles.Add(rect);
                        if (DragSelected != null)
                            DragSelected.Invoke(this, new(rect, GetSourceRectangle(rect)));
                        Refresh();
                    }
                }
            }
            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            if (e.Button == MouseButtons.Left && mouse_down) {
                start_r = e.X;
                start_b = e.Y;
                Refresh();
            }
            base.OnMouseMove(e);
        }

        protected override void OnEnabledChanged(EventArgs e) {
            Refresh();
            base.OnEnabledChanged(e);
        }

        Size _old_size;

        protected override void OnResize(EventArgs e) {
            var size = Size;
            if (_old_size != default && (_old_size.Width != size.Width || _old_size.Height != size.Height))
                ResizeRectangle(size.Width / (double)_old_size.Width, size.Height / (double)_old_size.Height);
            _old_size = size;
            Refresh();
            base.OnResize(e);
        }

        void ResizeRectangle(double scaleX, double scaleY) {
            lock (SelectedRectangles) {
                for (int i = 0; i < SelectedRectangles.Count; i++)
                    SelectedRectangles[i] = ResizeRectangle(SelectedRectangles[i], scaleX, scaleY);
            }
        }

        void ResizeProcessing() {
            if (_resized_image == null || _resized_image.Width != Size.Width || _resized_image.Height != Size.Height)
                _resized_image = ResizeImage(_image, Size, out _r_rect);
        }

        void DisabledProcessing(Graphics graphics, Rectangle rectangle) {
            using (var sb = new SolidBrush(Color.FromArgb(200, 0, 0, 0)))
                graphics.FillRectangle(sb, rectangle);

            using (var sb = new SolidBrush(Color.White))
            using (var sf = new StringFormat() {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            })  graphics.DrawString(disabledText, Font, sb, rectangle, sf);
        }

        Bitmap HighlightProcessing(Bitmap image) {
            IEnumerable<RectangleF> rectangles = SelectedRectangles;
            if (mouse_down)
                rectangles = rectangles.Append(new (start_x, start_y, start_r - start_x, start_b - start_y));

            return MaskImage(image, rectangles.ToArray(), 255 / 4, 255, Color.PaleVioletRed, 3);
        }

        static RectangleF ResizeRectangle(RectangleF rect, double scaleX, double scaleY) =>
            new ((float)(rect.X * scaleX), (float)(rect.Y * scaleY), (float)(rect.Width * scaleX), (float)(rect.Height * scaleY));

        // ############# IMAGE PROCESSING #############

        static Bitmap ClippingImage(Bitmap image, Rectangle rect) {
            var _rect = new Rectangle(0, 0, image.Width, image.Height);
            var size = rect.Size;

            var out_img = new Bitmap(size.Width, size.Height);

            var u_img = image.LockBits(_rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            var o_img = out_img.LockBits(new (new(0, 0), size), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            unsafe {
                var _h = u_img.Height;
                var _w = u_img.Width;
                var o_h = o_img.Height;
                var o_w = o_img.Width;
                var r_x = rect.X;
                var r_y = rect.Y;

                for (int y = 0; y < o_h; y++) {
                    var a_y = r_y + y;
                    if (a_y < 0 || a_y > _h - 1) continue;

                    for (int x = 0; x < o_w; x++) {
                        var a_x = r_x + x;
                        if (a_x < 0 || a_x > _w) continue;

                        byte* ptr = (byte*)u_img.Scan0 + a_y * u_img.Stride;
                        byte* o_ptr = (byte*)o_img.Scan0 + y * o_img.Stride;

                        o_ptr[4 * x] = ptr[4 * a_x];           // blue
                        o_ptr[4 * x + 1] = ptr[4 * a_x + 1];   // green
                        o_ptr[4 * x + 2] = ptr[4 * a_x + 2];   // red
                        o_ptr[4 * x + 3] = ptr[4 * a_x + 3];   // alpha
                    }
                }
            }

            image.UnlockBits(u_img);
            out_img.UnlockBits(o_img);
            return out_img;
        }

        static Bitmap MaskImage(Bitmap image, RectangleF[] rectangles, byte n_alpha, byte m_alpha, Color adjacent_border, int adjacency) {
            var rect = new Rectangle(0, 0, image.Width, image.Height);
            var out_img = new Bitmap(image.Width, image.Height);
            var u_img = image.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            var o_img = out_img.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            unsafe {
                var _h = u_img.Height;
                var _w = u_img.Width;

                var a_r = adjacent_border.A;
                var a_g = adjacent_border.G;
                var a_b = adjacent_border.B;

                for (int y = 0; y < _h; y++) {
                    for (int x = 0; x < _w; x++) {
                        byte* ptr = (byte*)u_img.Scan0 + y * u_img.Stride;
                        byte* o_ptr = (byte*)o_img.Scan0 + y * o_img.Stride;

                        o_ptr[4 * x] = ptr[4 * x];           // blue
                        o_ptr[4 * x + 1] = ptr[4 * x + 1];   // green
                        o_ptr[4 * x + 2] = ptr[4 * x + 2];   // red
                        o_ptr[4 * x + 3] = n_alpha;
                    }
                }

                for (int i = 0; i < rectangles.Length; i++) {
                    var _rt = rectangles[i];
                    var r_x = _rt.X;
                    var r_y = _rt.Y;
                    var r_r = _rt.Right;
                    var r_b = _rt.Bottom;
                    var u_h = Math.Min(u_img.Height, _rt.Bottom + adjacency);
                    var u_w = Math.Min(u_img.Width, _rt.Right + adjacency);

                    for (int y = 0; y < u_h; y++) {
                        var c_y = y > r_y && y < r_b;
                        var c_y_a = (y >= r_y - adjacency && y <= r_y) || (y >= r_b && y <= r_b + adjacency);

                        for (int x = 0; x < u_w; x++) {
                            byte* ptr = (byte*)o_img.Scan0 + y * o_img.Stride;
                            byte* o_ptr = (byte*)o_img.Scan0 + y * o_img.Stride;

                            if (adjacency > 0 && 
                                ((c_y_a && x >= r_x - adjacency) || 
                                (((x >= r_x - adjacency && x <= r_x) || (x >= r_r && x <= r_r + adjacency)) && y >= r_y - adjacency))) {

                                o_ptr[4 * x] = Average(ptr[4 * x], a_b);           // blue
                                o_ptr[4 * x + 1] = Average(ptr[4 * x + 1], a_g);   // green
                                o_ptr[4 * x + 2] = Average(ptr[4 * x + 2], a_r);   // red
                                o_ptr[4 * x + 3] = 255;
                            } else if (c_y && x > r_x && x < r_r)
                                o_ptr[4 * x + 3] = m_alpha;
                        }
                    }
                }
            }

            image.UnlockBits(u_img);
            out_img.UnlockBits(o_img);
            return out_img;
        }

        static byte Average(byte a, byte b) =>
            (byte)((a + b) / 2);

        static Bitmap? ResizeImage(Image? image, Size size, out Rectangle rect) {
            if (image == null) {
                rect = default;
                return null;
            }

            var o_size = image.Size;
            int w = size.Width, h = size.Height;
            Bitmap b = new(w, h);

            double ratioX = size.Width / (double)o_size.Width;
            double ratioY = size.Height / (double)o_size.Height;

            var scale = Math.Min(ratioX, ratioY);

            int newWidth = (int)(o_size.Width * scale);
            int newHeight = (int)(o_size.Height * scale);

            using (Graphics g = Graphics.FromImage(b)) {
                g.FillRectangle(Brushes.Black, 0, 0, b.Width, b.Height);
                rect = new ((size.Width - newWidth) / 2, (size.Height - newHeight) / 2, newWidth, newHeight);
                g.DrawImage(image, rect);
            }

            return b;
        }
    }

    public delegate void DragSelectedEventHandler(object sender, DragSelectedEventArgs e);

    public class DragSelectedEventArgs {
        public RectangleF ClientRectangle { get; }
        public RectangleF SourceRectangle { get; }

        public DragSelectedEventArgs(RectangleF clientRectangle, RectangleF sourceRectangle) {
            ClientRectangle = clientRectangle;
            SourceRectangle = sourceRectangle;
        }
    }
}
