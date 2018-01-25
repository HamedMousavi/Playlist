// -----------------------------------------------------------------------
// <copyright file="ServerSettings.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------


namespace HLib.Network.Email
{

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ServerSettings : IServerSettings
    {
        public string Address { get; set; }

        public int Port { get; set; }

        public bool SupportsSsl { get; set; }
    }
}
