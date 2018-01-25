namespace HLib.View
{
    
    using System.Windows.Controls;


    public class HMenuItem : MenuItem
    {

        public IActionLogic ClickAction { get; set; }


        protected override void OnClick()
        {
            base.OnClick();

            if (ClickAction != null) ClickAction.Run();
        }
    }
}
