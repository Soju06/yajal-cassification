namespace yajal_data_post_processing {
    internal class InfoLabel : Label {
        object? _value;
        string _template = "info: {0}";

        public string Template { 
            get => _template;
            set { 
                _template = value;
                SetText();
            } 
        }

        public new string Text {
            get => _value?.ToString() ?? "";
            set {
                _value = value;
                SetText();
            }
        }

        public object? Value {
            get => _value;
            set {
                _value = value;
                SetText();
            }
        }

        void SetText() {
            base.Text = string.Format(_template, _value);
        }
    }
}
