using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using VenoMpie.Common.Interfaces.Layer13.SearchResults;

namespace VenoMpie.Common.Interfaces.Layer13
{
    /// <summary>
    /// Class for Interfacing with Layer13
    /// </summary>
    /// <remarks>
    /// Seems like a really cool Pre-DB, only found them recently, thanks SRRDB!
    /// </remarks>
    /// <see cref="https://layer13.net/"/>
    public class Layer13 : JSONAPISearchBase
    {
        public override string APIPath { get { return "http://api.layer13.net/v1/"; } }
        public string API_Key { get; set; } = "YOUR KEY HERE";

        public Layer13() { }
        public Layer13(string apikey)
        {
            API_Key = apikey;
        }

        #region Specific Searches
        public GetPre GetPre(string releaseName)
        {
            AddSearchTermArgument("getpre", releaseName);
            return SearchWithKey<GetPre>();
        }
        public GetFileSize GetFileSize(string id)
        {
            AddSearchTermArgument("getfilessize", id);
            return SearchWithKey<GetFileSize>();
        }
        public ListFiles ListFiles(string id)
        {
            AddSearchTermArgument("listfiles", id);
            return SearchWithKey<ListFiles>();
        }
        #endregion

        private T SearchWithKey<T>() where T : class, new()
        {
            AddAPIKey();
            return Search<T>();
        }
        private void AddAPIKey()
        {
            AddSearchTermArgument("key", API_Key);
        }
    }
}
