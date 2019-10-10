using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blish_HUD.Controls;

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
    class TraitPanel : Panel
    {
        public TraitPanel() : base()
        {
            Height = 477; // 530 * 0.9
            buildLayout();
        }

        private void buildLayout()
        {
            var lblTmp = new Label
            {
                Text = "HERE BE TRAITS",
                Parent = this,
                Size = ContentRegion.Size,
            };
            Adhesive.Binding.CreateOneWayBinding(() => lblTmp.Width, () => lblTmp.Parent.Width);
        }
    }
}
