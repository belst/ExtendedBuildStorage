using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace ExtendedBuildStorage
{
    class TemplateDetails : Panel
    {
        private Template _tpl;
        private TextBox _txtName;
        private TextBox _txtCode;
        private Panel _skillPanel;
        private Panel _traitPanel;
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
            _skillPanel = new SkillsPanel
            {
                Title = _tpl.Name,
                ShowBorder = true,
                Width = Width,
                //Height = (int)(220f * 0.9), // 0.9 = blish panel height / ingame panel height at scale Large
                Location = Location,
                Parent = this
            };
            Adhesive.Binding.CreateOneWayBinding(() => _skillPanel.Width, () => _skillPanel.Parent.Width);
            // TODO: Fix
            Adhesive.Binding.CreateOneWayBinding(() => _skillPanel.Title, () => ((TemplateDetails)_skillPanel.Parent).Template, td => td.Name);

            _traitPanel = new TraitPanel
            {
                Title = "Specializations",
                Width = Width,
                ShowBorder = true,
                Location = new Point(Location.X, _skillPanel.Bottom),
                Parent = this,
            };
            Adhesive.Binding.CreateOneWayBinding(() => _traitPanel.Width, () => _traitPanel.Parent.Width);
            GameService.Overlay.QueueMainThreadUpdate((gameTime) =>
            {
                _txtName = new TextBox
                {
                    Text = _tpl.Name,
                    Parent = _skillPanel,
                    Left = 20,
                };

                _txtCode = new TextBox
                {
                    Text = _tpl.Value,
                    Parent = _skillPanel,
                    Top = _txtName.Bottom + Control.ControlStandard.ControlOffset.Y,
                    Left = 20,
                };
                PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == nameof(Template))
                    {
                        _txtName.Text = Template.Name;
                        _skillPanel.Title = Template.Name;
                        _txtCode.Text = Template.Value;
                    }
                    if (args.PropertyName == nameof(ContentRegion))
                    {
                        _skillPanel.Width = ContentRegion.Width;
                    }
                    if (args.PropertyName == nameof(Location))
                    {
                        _skillPanel.Location = Location;
                    }
                };
                var btnCopy = new StandardButton
                {
                    Text = "Copy to Clipboard",
                    Left = _txtCode.Right + Control.ControlStandard.ControlOffset.X,
                    Top = _txtCode.Top,
                    Parent = _skillPanel,
                };
                btnCopy.Click += (sender, args) => System.Windows.Forms.Clipboard.SetText(_tpl.Value);

                var btnRename = new StandardButton
                {
                    Text = "Rename",
                    Left = _txtName.Right + Control.ControlStandard.ControlOffset.X,
                    Top = _txtName.Top,
                    Parent = _skillPanel,
                };
                btnRename.Click += (sender, args) => _tpl.Name = _txtName.Text;

            });
        }
    }
}
