using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace ExtendedBuildStorage
{
    /*
     * 
     * 720x750 Whole Panel

        720x220 Skill Bar

        720x530 Traits

        150 height for single trait line
        740x680
     * 
     */
    class SkillsPanel : Panel
    {

        private int _paddingBottom = 18; // 20 * 0.9
        private int _paddingLeft = 20;

        public SkillsPanel(): base()
        {
            LoadStaticResources();
            Height = 198; // 220 * 0.9
            buildLayout();
        }

        private static void LoadStaticResources()
        {
            
        }

        private void buildLayout()
        {
            buildWeaponSlots();
            buildUtilitySlots();
        }

        private void buildWeaponSlots()
        {
            var weaponSlotPanel = new Panel
            {
                Width = 64 * 5 + 4,
                Height = 64,
                Parent = this,
                Location = new Point(_paddingLeft, 70),
            };
            var skill1 = new Panel
            {
                Size = new Point(64, 64),
                Parent = weaponSlotPanel,
                Left = 0,
                BackgroundColor = new Color(0, 0, 1),
            };
            var skill2 = new Panel
            {
                Size = new Point(64, 64),
                BackgroundColor = new Color(0, 1, 0),
                Parent = weaponSlotPanel,
                Left = skill1.Right + 1,
            };
            var skill3 = new Panel
            {
                Size = new Point(64, 64),
                BackgroundColor = new Color(0, 1, 1),
                Parent = weaponSlotPanel,
                Left = skill2.Right + 1,
            };
            var skill4 = new Panel
            {
                Size = new Point(64, 64),
                BackgroundColor = new Color(1, 0, 0),
                Parent = weaponSlotPanel,
                Left = skill3.Right + 1,
            };
            var skill5 = new Panel
            {
                Size = new Point(64, 64),
                BackgroundColor = new Color(1, 0, 1),
                Parent = weaponSlotPanel,
                Left = skill4.Right + 1,
            };
        }

        private void buildUtilitySlots()
        {
            var utilitySkillPanel = new Panel
            {
                Width = 64 * 5 + 4,
                Height = 64,
                Parent = this,
                Location = new Point(_paddingLeft * 2 + 64 * 5 + 4, 70),
            };
            var skill1 = new Panel
            {
                Size = new Point(64, 64),
                Parent = utilitySkillPanel,
                Left = 0,
                BackgroundColor = new Color(0, 0, 1),
            };
            var skill2 = new Panel
            {
                Size = new Point(64, 64),
                BackgroundColor = new Color(0, 1, 0),
                Parent = utilitySkillPanel,
                Left = skill1.Right + 1,
            };
            var skill3 = new Panel
            {
                Size = new Point(64, 64),
                BackgroundColor = new Color(0, 1, 1),
                Parent = utilitySkillPanel,
                Left = skill2.Right + 1,
            };
            var skill4 = new Panel
            {
                Size = new Point(64, 64),
                BackgroundColor = new Color(1, 0, 0),
                Parent = utilitySkillPanel,
                Left = skill3.Right + 1,
            };
            var skill5 = new Panel
            {
                Size = new Point(64, 64),
                BackgroundColor = new Color(1, 0, 1),
                Parent = utilitySkillPanel,
                Left = skill4.Right + 1,
            };
        }

    }
}
