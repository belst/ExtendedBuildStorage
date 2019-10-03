using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blish_HUD.Common;
using Microsoft.Xna.Framework.Graphics;

namespace ExtendedBuildStorage
{
    class Template
    {
        public Template(string name)
        {
            Name = name;
            if (File.Exists(FullPath))
            {
                _value = File.ReadAllText(FullPath);
            }
            else
            {
                Value = "AAAAAAAAAAAAA";
            }
        }

        private string _name;
        private string _value;
        private readonly string _path = ExtendedBuildStorage.ModuleInstance.DirectoriesManager.GetFullDirectoryPath("build-templates");

        private Texture2D _icon;

        public Texture2D Icon
        {
            get => _icon;
            set => _icon = value;
        }

        public string Filename
        {
            get
            {
                return _name + ".txt";
            }
        }

        public string FullPath
        {
            get
            {
                return Path.Combine(_path, Filename);
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                var oldpath = FullPath;
                if (_name == value)
                    return;

                _name = value;
                if (File.Exists(oldpath))
                {
                    File.Move(oldpath, FullPath);
                }
            }
        }

        public string Value
        {
            get
            {
                // TODO: parse Value
                return _value;
            }
            set
            {
                // TODO: verify chatcode
                _value = value;
                File.WriteAllText(FullPath, _value);
            }
        }
    }
}
