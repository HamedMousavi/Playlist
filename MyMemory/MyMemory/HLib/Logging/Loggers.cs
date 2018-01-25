// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Loggers.cs" company="www.OrderedSoft.com">
//   Author: Hamed Mousavi: HamedMosavi[at] Yahoo (dot) com
//   License agreement: Please read License.txt provided in solution directory
// </copyright>
// <summary>
//   Defines the Loggers type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace HLib.Logging
{

    public static class Loggers
    {

        private static ILoggable _null = new NullLogger();


        public static ILoggable Null
        {
            get
            {
                return _null;
            }

            set
            {
                _null = value;
            }
        }
    }
}
