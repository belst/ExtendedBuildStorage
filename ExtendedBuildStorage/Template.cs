using System.IO;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework.Graphics;

namespace ExtendedBuildStorage
{
    class Template: INotifyPropertyChanged
    {
        public Template() {}
        public Template(string name)
        {
            _name = name;
            if (File.Exists(FullPath))
            {
                _value = File.ReadAllText(FullPath);
                Dirty = false;
            }
        }

        public Template(string name, string value)
        {
            _name = name;
            _value = value;
        }

        public Template Copy()
        {
            return new Template(Name + "(Copy)", Value);
        }

        public void Save()
        {
            if (!_dirty && _rename == "") return;
            if (_rename != "")
            {
                if (File.Exists(_rename))
                    File.Move(_rename, FullPath);
            }
            File.WriteAllText(FullPath, _value);
            _rename = "";
            Dirty = false;
        }

        private string _name;
        private string _value;
        private bool _dirty = true;
        private string _rename = "";
        private string _origvalue;
        private readonly string _path = ExtendedBuildStorage.ModuleInstance.DirectoriesManager.GetFullDirectoryPath("build-templates");

        private Texture2D _icon;

        protected bool SetProperty<T>(ref T property, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (Equals(property, newValue) || propertyName == null) return false;

            property = newValue;

            OnPropertyChanged(propertyName);

            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)) return;

            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Texture2D Icon
        {
            get => _icon;
            set => SetProperty(ref _icon, value);
        }

        public string Filename
        {
            get => _name + ".txt";
        }

        public string FullPath
        {
            get => Path.Combine(_path, Filename);
        }

        public string Name
        {
            get
            {
                if (_dirty || _rename != "")
                    return "*" + _name;

                return _name;
            }
            set
            {
                if (_name == value)
                    return;

                if (_rename == "")
                    _rename = FullPath;

                _name = value;
                if (_rename == FullPath)
                    _rename = "";

                OnPropertyChanged(nameof(Name));
            }
        }

        private bool Dirty
        {
            set
            {
                SetProperty(ref _dirty, value);
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Value
        {
            get => _value;
            set
            {
                if (_value == value)
                    return;

                if (_origvalue == "")
                    _origvalue = _value;

                // TODO: verify chatcode
                SetProperty(ref _value, value);

                if (_origvalue == value)
                {
                    Dirty = false;
                    _origvalue = "";
                }
                else
                {
                    Dirty = true;
                }
            }
        }
    }
}
