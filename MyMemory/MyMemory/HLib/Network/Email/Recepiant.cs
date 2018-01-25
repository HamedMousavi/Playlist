// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Recepiant.cs" company="www.OrderedSoft.com">
//   Author: Hamed Mousavi: HamedMosavi[at] Yahoo (dot) com
//   License agreement: Please read License.txt provided in solution directory
// </copyright>
// <summary>
//   Defines the Recepiant type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace HLib.Network.Email
{

    using HLib.Data;


    public class Recepiant : NotifyPropertyChanged, IRecepiant
    {

        private TransmitState transmitState;

        private string address;


        public string Address
        {
            get
            {
                return address;
            }

            set
            {
                address = value;
                FirePropertyChanged(this, "Address");
            }
        }


        public TransmitState TransmitState
        {
            get
            {
                return transmitState;
            }

            set
            {
                transmitState = value;
                FirePropertyChanged(this, "TransmitState");
            }
        }
    }
}
