using System.IO;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework.Graphics;

namespace ExtendedBuildStorage
{
    class Template: INotifyPropertyChanged
    {
        public Template(string name)
        {
            _name = name;
            if (File.Exists(FullPath))
            {
                _value = File.ReadAllText(FullPath);
            }
            else
            {
                Value = "AAAAAAAAAAAAA";
            }
        }

        public Template(string name, string value)
        {
            // Will overwrite already existing builds
            _name = name;
            Value = value;
        }

        public Template Copy()
        {
            return new Template(Name + "(Copy)", Value);
        }

        private string _name;
        private string _value;
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
            get => _name;
            set
            {
                var oldpath = FullPath;
                if (_name == value)
                    return;

                SetProperty(ref _name, value);
                if (File.Exists(oldpath))
                {
                    File.Move(oldpath, FullPath);
                }
            }
        }

        public string Value
        {
            get => _value;
            set
            {
                // TODO: verify chatcode
                SetProperty(ref _value, value);
                File.WriteAllText(FullPath, _value);
            }
        }
    }
}
