// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HToolbarButton.cs" company="www.OrderedSoft.com">
//   Author: Hamed Mousavi: HamedMosavi[at] Yahoo (dot) com
//   License agreement: Please read License.txt provided in solution directory
// </copyright>
// <summary>
//   Defines the HToolbarButton type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace HLib.View
{

    using System.Windows.Controls;

    using Button = System.Windows.Controls.Button;


    public class HToolbarButton : Button
    {

        public IActionLogic ClickAction { get; set; }


        public HToolbarButton()
        {
            SetResourceReference(StyleProperty, ToolBar.ButtonStyleKey);
        }


        protected override void OnClick()
        {
            base.OnClick();

            if (ClickAction != null) ClickAction.Run();
        }
    }
}
