// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NotifyPropertyChanged.cs" company="www.OrderedSoft.com">
//   Author: Hamed Mousavi: HamedMosavi[at] Yahoo (dot) com
//   License agreement: Please read License.txt provided in solution directory
// </copyright>
// <summary>
//   Defines the NotifyPropertyChanged type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace HLib.Data
{

    using System.ComponentModel;


    public class NotifyPropertyChanged : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;


        protected void FirePropertyChanged(string propertyName)
        {
            FirePropertyChanged(this, propertyName);
        }


        protected void FirePropertyChanged(object sender, string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(sender, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
