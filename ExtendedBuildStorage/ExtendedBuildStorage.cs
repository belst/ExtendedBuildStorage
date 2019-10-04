using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Modules;
using Blish_HUD.Settings;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace ExtendedBuildStorage {


    [Export(typeof(Module))]
    public class ExtendedBuildStorage : Module {

        /// <summary>
        /// This is your logger for writing to the log.  Ensure the type of of your module class.
        /// Other classes can have their own logger.  Instance those loggers the same as you have
        /// here, but with their type as the argument.
        /// </summary>
        private static readonly Logger Logger = Logger.GetLogger(typeof(ExtendedBuildStorage));

        internal static ExtendedBuildStorage ModuleInstance;

        // Service Managers
        internal SettingsManager    SettingsManager    => this.ModuleParameters.SettingsManager;
        internal ContentsManager    ContentsManager    => this.ModuleParameters.ContentsManager;
        internal DirectoriesManager DirectoriesManager => this.ModuleParameters.DirectoriesManager;
        internal Gw2ApiManager      Gw2ApiManager      => this.ModuleParameters.Gw2ApiManager;

        private Texture2D     _mugTexture;

        // Controls (be sure to dispose of these in Unload()

        private WindowTab _templateTab;
        private Panel _tabPanel;
        private string _templatePath;
        private TemplateDetails _tplPanel;

        /// <summary>
        /// Ideally you should keep the constructor as is.
        /// Use <see cref="Initialize"/> to handle initializing the module.
        /// </summary>
        [ImportingConstructor]
        public ExtendedBuildStorage([Import("ModuleParameters")] ModuleParameters moduleParameters) : base(moduleParameters) {
            ModuleInstance = this;
        }

        /// <summary>
        /// Define the settings you would like to use in your module.  Settings are persistent
        /// between updates to both Blish HUD and your module.
        /// </summary>
        protected override void DefineSettings(SettingCollection settings) {
        }

        /// <summary>
        /// Allows your module to perform any initialization it needs before starting to run.
        /// Please note that Initialize is NOT asynchronous and will block Blish HUD's update
        /// and render loop, so be sure to not do anything here that takes too long.
        /// </summary>
        protected override void Initialize() {
            
        }

        /// <summary>
        /// Load content and more here. This call is asynchronous, so it is a good time to
        /// run any long running steps for your module. Be careful when instancing
        /// <see cref="Blish_HUD.Entities.Entity"/> and <see cref="Blish_HUD.Controls.Control"/>.
        /// Setting their parent is not thread-safe and can cause the application to crash.
        /// You will want to queue them to add later while on the main thread or in a delegate queued
        /// with <see cref="Blish_HUD.OverlayService.QueueMainThreadUpdate(Action{GameTime})"/>.
        /// </summary>
        protected override async Task LoadAsync() {

            // Load content from the ref directory automatically with the ContentsManager
            _mugTexture = ContentsManager.GetTexture("603447.png");

            // Get your manifest registered directories with the DirectoriesManager
            foreach (string directoryName in this.DirectoriesManager.RegisteredDirectories) {
                string fullDirectoryPath = DirectoriesManager.GetFullDirectoryPath(directoryName);

                var allFiles = Directory.EnumerateFiles(fullDirectoryPath, "*", SearchOption.AllDirectories).ToList();
                Logger.Info($"'{directoryName}' can be found at '{fullDirectoryPath}' and has {allFiles.Count} total files within it.");
            }

            _templatePath = DirectoriesManager.GetFullDirectoryPath("build-templates");

            _tabPanel = BuildTemplatePanel(GameService.Overlay.BlishHudWindow.ContentRegion);
        }

        /// <summary>
        /// Allows you to perform an action once your module has finished loading (once
        /// <see cref="LoadAsync"/> has completed).  You must call "base.OnModuleLoaded(e)" at the
        /// end for the <see cref="Module.ModuleLoaded"/> event to fire and for
        /// <see cref="Module.Loaded" /> to update correctly.
        /// </summary>
        protected override void OnModuleLoaded(EventArgs e) {

            _templateTab = GameService.Overlay.BlishHudWindow.AddTab("Build Templates", this.ContentsManager.GetTexture(@"textures\1466345.png"), _tabPanel);

            base.OnModuleLoaded(e);
        }

        /// <summary>
        /// Allows your module to run logic such as updating UI elements,
        /// checking for conditions, playing audio, calculating changes, etc.
        /// This method will block the primary Blish HUD loop, so any long
        /// running tasks should be executed on a separate thread to prevent
        /// slowing down the overlay.
        /// </summary>
        protected override void Update(GameTime gameTime) {

        }

        /// <summary>
        /// For a good module experience, your module should clean up ANY and ALL entities
        /// and controls that were created and added to either the World or SpriteScreen.
        /// Be sure to remove any tabs added to the Director window, CornerIcons, etc.
        /// </summary>
        protected override void Unload() {
            _tabPanel.Dispose();

            // Static members are not automatically cleared and will keep a reference to your,
            // module unless manually unset.
            ModuleInstance = null;
        }

        private Panel BuildTemplatePanel(Rectangle rect)
        {
            var btPanel = new Panel()
            {
                CanScroll = false,
                Size = rect.Size,
            };

            int topOffset = Panel.MenuStandard.ControlOffset.Y;

            var menuSection = new Panel
            {
                Title = "Build Templates",
                ShowBorder = true,
                Size = Panel.MenuStandard.Size - new Point(0, topOffset + Panel.MenuStandard.ControlOffset.Y),
                Parent = btPanel
            };

            var buildTemplates = new FlowMenu
            {
                Size = menuSection.ContentRegion.Size - new Point(0, 40),
                //Height = menuSection.ContentRegion.Size.Y - 40,
                MenuItemHeight = 40,
                Parent = menuSection,
                CanSelect = true,
                CanScroll = true,
            };

            var newButton = new StandardButton
            {
                Parent = menuSection,
                Text = "New Template",
                Width = buildTemplates.ContentRegion.Width - 10,
                Top = buildTemplates.Bottom + 3,
            };


            var _templates = Directory.GetFiles(_templatePath, "*.txt", SearchOption.TopDirectoryOnly).Select(f => new Template(Path.GetFileNameWithoutExtension(f))).ToList();
            Func<string, Template> findTplByName = name => _templates.Find(t => t.Name == name);

            foreach (var t in _templates)
            {
                var bt = buildTemplates.AddMenuItem(t.Name, t.Icon);
                bt.Click += delegate
                {
                    _tplPanel.Template = t;
                };
                Adhesive.Binding.CreateOneWayBinding(() => bt.Text, () => t.Name);
            }

            buildTemplates.Select((MenuItem)buildTemplates.Children.First());

            newButton.Click += delegate
            {
                var toAdd = findTplByName(buildTemplates.SelectedMenuItem.Text).Copy();
                _templates.Add(toAdd);
                var bt = buildTemplates.AddMenuItem(toAdd.Name, toAdd.Icon);
                bt.Click += delegate
                {
                    _tplPanel.Template = toAdd;
                };
                Adhesive.Binding.CreateOneWayBinding(() => bt.Text, () => toAdd.Name);
                bt.Select();
                _tplPanel.Template = toAdd; // poor mans click trigger
            };

            GameService.Overlay.QueueMainThreadUpdate((gameTime) => {
                var searchBox = new TextBox()
                {
                    PlaceholderText = "Search",
                    Width = menuSection.Width,
                    Location = new Point(TextBox.Standard.ControlOffset.Y, menuSection.Left),
                    Parent = btPanel
                };
                menuSection.Location = new Point(Panel.MenuStandard.PanelOffset.X, topOffset + searchBox.Bottom);

                searchBox.TextChanged += delegate (object sender, EventArgs args) {
                    buildTemplates.FilterChildren<MenuItem>(mi => mi.Text.ToLower().Contains(searchBox.Text.ToLower()));
                };
            });

            var tmp = findTplByName(buildTemplates.SelectedMenuItem.Text);
            // Main panel
            _tplPanel = new TemplateDetails(tmp)
            {
                Location = new Point(menuSection.Right + Panel.ControlStandard.ControlOffset.X, menuSection.Top),
                Size = new Point(btPanel.ContentRegion.Width - menuSection.Right - Control.ControlStandard.ControlOffset.X, menuSection.Height),
                Parent = btPanel,
            };


            return btPanel;
        }

        

    }

    

}
