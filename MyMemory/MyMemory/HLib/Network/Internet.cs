// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Internet.cs" company="www.OrderedSoft.com">
//   Author: Hamed Mousavi: HamedMosavi[at] Yahoo (dot) com
//   License agreement: Please read License.txt provided in solution directory
// </copyright>
// <summary>
//   Defines the Navigation type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace HLib.Network
{

    public class Internet
    {

        public static void NavigateTo(string url)
        {
            System.Diagnostics.Process.Start(url);
        }


        public static string GetDomainName(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return string.Empty;

            if (url.StartsWith(".")) url = url.Remove(0, 1);
            if (!url.Contains("www.")) url = "www." + url;
            if (!url.Contains("://")) url = "http://" + url;
            var host = new System.Uri(url).Host;
            return host.Substring(host.LastIndexOf('.', host.LastIndexOf('.') - 1) + 1);            
        }
    }
}
