using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;

namespace yajal_data_post_processing {
    internal class DragSelect : Control {
        Image? _image;
        Bitmap? _resized_image;
        string disabledText = "Disabled";
        List<Rectangle> SelectedRectangles = new();
        BufferedGraphics bufferedGraphics;

        public DragSelect() {
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);
            using var g = CreateGraphics();
            bufferedGraphics = BufferedGraphicsManager.Current.Allocate(g,
                new Rectangle(0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height));
        }

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
                Refresh();
            }
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
                mouse_down = false;
                SelectedRectangles.Add(new(start_x, start_y, start_r - start_x, start_b - start_y));
                Refresh();
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

        protected override void OnResize(EventArgs e) {
            Refresh();
            base.OnResize(e);
        }

        Bitmap HighlightProcessing(Bitmap image) {
            IEnumerable<Rectangle> rectangles = SelectedRectangles;
            if (mouse_down)
                rectangles = rectangles.Append(new (start_x, start_y, start_r - start_x, start_b - start_y));

            return MaskImage(image, rectangles.ToArray(), 255 / 4, 255, Color.Orange, 2);
        }

        static Bitmap MaskImage(Bitmap image, Rectangle[] rectangles, byte n_alpha, byte m_alpha, Color adjacent_border, int adjacency) {
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
                    var r_r = _rt.X;
                    var r_y = _rt.Y;
                    var r_r = _rt.Right;
                    var r_b = _rt.Bottom;
                    var u_h = Math.Min(u_img.Height, _rt.Bottom);
                    var u_w = Math.Min(u_img.Width, _rt.Right);

                    for (int y = 0; y < u_h; y++) {
                        var c_y = y > r_y && y < r_b - 1;
                        var c_y_a = y >= r_y - adjacency && y <= r_y && y <= r_b + adjacency && y >= r_b;

                        for (int x = 0; x < u_w; x++) {
                            byte* ptr = (byte*)o_img.Scan0 + y * o_img.Stride;
                            byte* o_ptr = (byte*)o_img.Scan0 + y * o_img.Stride;

                            if (adjacency > 0 && c_y_a && x >= r_r - adjacency && x <= r_r && x <= r_r + adjacency && x >= r_r) {
                                o_ptr[4 * x] = Average(ptr[4 * x], a_b);           // blue
                                o_ptr[4 * x + 1] = Average(ptr[4 * x + 1], a_g);   // green
                                o_ptr[4 * x + 2] = Average(ptr[4 * x + 2], a_r);   // red
                            }

                            if (c_y && x > r_r && x < r_r - 1)
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

        void ResizeProcessing() {
            if (_resized_image == null || _resized_image.Width != Size.Width || _resized_image.Height != Size.Height)
                ResizeImage(Size);
        }

        void ResizeImage(Size size) {
            if (_image == null) return;

            var o_size = _image.Size;
            int w = size.Width, h = size.Height;
            var scale = Math.Min(w / o_size.Width, h / o_size.Height);
            Bitmap b = new(w, h);

            var scaleWidth = o_size.Width * scale;
            var scaleHeight = o_size.Height * scale;

            using (Graphics g = Graphics.FromImage(b))
                g.DrawImage(_image, (w - scaleWidth) / 2, (h - scaleHeight) / 2, scaleWidth, scaleHeight);

            var o_b = _resized_image;
            _resized_image = b;
            o_b?.Dispose();
        }
    }
}
