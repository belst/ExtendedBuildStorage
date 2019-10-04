using Blish_HUD;
using Blish_HUD.Controls;

namespace ExtendedBuildStorage
{
    class TemplateDetails : Panel
    {
        private Template _tpl;
        private TextBox _txtName;
        private TextBox _txtCode;
        public TemplateDetails(string name, string code) : base()
        {
            _tpl = new Template(name);
            if (_tpl.Value != code)
                _tpl.Value = code;
            buildLayout();
        }

        public TemplateDetails(Template tpl) : base()
        {
            _tpl = tpl;
            buildLayout();
        }

        public Template Template
        {
            get => _tpl;
            set => SetProperty(ref _tpl, value);
        }

        private void buildLayout()
        {
            GameService.Overlay.QueueMainThreadUpdate((gameTime) =>
            {
                _txtName = new TextBox
                {
                    Text = _tpl.Name,
                    Parent = this
                };
                
                _txtCode = new TextBox
                {
                    Text = _tpl.Value,
                    Parent = this,
                    Top = _txtName.Bottom + Control.ControlStandard.ControlOffset.Y,
                };
                PropertyChanged += (sender, args) => {
                    if (args.PropertyName == nameof(Template))
                    {
                        _txtName.Text = Template.Name;
                        _txtCode.Text = Template.Value;
                    }
                };
                var btnCopy = new StandardButton
                {
                    Text = "Copy to Clipboard",
                    Left = _txtCode.Right + Control.ControlStandard.ControlOffset.X,
                    Top = _txtCode.Top,
                    Parent = this,
                };
                btnCopy.Click += (sender, args) => System.Windows.Forms.Clipboard.SetText(_tpl.Value);

                var btnRename = new StandardButton
                {
                    Text = "Rename",
                    Left = _txtName.Right + Control.ControlStandard.ControlOffset.X,
                    Top = _txtName.Top,
                    Parent = this,
                };
                btnRename.Click += (sender, args) => _tpl.Name = _txtName.Text;

            });
        }
    }
}
