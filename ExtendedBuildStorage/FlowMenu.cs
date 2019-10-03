using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework.Graphics;

namespace ExtendedBuildStorage
{
    class FlowMenu : FlowPanel, IMenuItem
    {

        public event EventHandler<ControlActivatedEventArgs> ItemSelected;
        protected virtual void OnItemSelected(ControlActivatedEventArgs e)
        {
            this.ItemSelected?.Invoke(this, e);
        }

        private int _menuItemHeight = 32;

        public int MenuItemHeight
        {
            get => _menuItemHeight;
            set => SetProperty(ref _menuItemHeight, value, true);
        }

        protected bool _shouldShift = false;
        public bool ShouldShift
        {
            get => _shouldShift;
            set => SetProperty(ref _shouldShift, value, true);
        }

        private bool _canSelect;
        public bool CanSelect
        {
            get => _canSelect;
            set => SetProperty(ref _canSelect, value);
        }

        void IMenuItem.Select()
        {
            throw new InvalidOperationException($"The root {nameof(Menu)} instance can not be selected.");
        }

        bool IMenuItem.Selected => false;

        

        public MenuItem AddMenuItem(string text, Texture2D icon = null)
        {
            return new MenuItem(text)
            {
                Icon = icon,
                Parent = this,
                Width = this.Width,
                MenuItemHeight = _menuItemHeight
            };
        }

        private MenuItem _selectedMenuItem;
        public MenuItem SelectedMenuItem => _selectedMenuItem;
        public void Select(MenuItem menuItem, List<IMenuItem> itemPath)
        {
            if (!_canSelect)
            {
                itemPath.ForEach(i => i.Deselect());
                return;
            }

            foreach (var item in this.GetDescendants().Cast<IMenuItem>().Except(itemPath))
            {
                item.Deselect();
            }

            _selectedMenuItem = menuItem;

            OnItemSelected(new ControlActivatedEventArgs(menuItem));
        }

        public void Select(MenuItem menuItem)
        {
            menuItem.Select();
        }

        void IMenuItem.Deselect()
        {
            Select(null, null);
        }

    }
}
