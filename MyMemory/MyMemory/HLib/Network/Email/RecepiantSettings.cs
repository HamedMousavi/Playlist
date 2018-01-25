// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecepiantSettings.cs" company="www.OrderedSoft.com">
//   Author: Hamed Mousavi: HamedMosavi[at] Yahoo (dot) com
//   License agreement: Please read License.txt provided in solution directory
// </copyright>
// <summary>
//   Defines the RecepiantSettings type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace HLib.Network.Email
{

    using System.Collections.Generic;
    using System.Collections.ObjectModel;


    public class RecepiantSettings : IRecepiantSettings
    {
        
        public RecepiantSettings()
        {
            RecepiantList = new ObservableCollection<IRecepiant>();
        }


        public ObservableCollection<IRecepiant> RecepiantList { get; set; }
    }
}
