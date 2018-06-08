using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using VenoMpie.Common.Interfaces.GGn.SearchResults;

namespace VenoMpie.Common.Interfaces.GGn
{
    /// <summary>
    /// Class for Interfacing with GazelleGames
    /// </summary>
    public class GGn : JSONAPISearchBase
    {
        public override string APIPath { get { return "https://gazellegames.net/api.php"; } }
        public string API_Key { get; set; } = "YOUR KEY HERE";

        public GGn() { }
        public GGn(string apikey)
        {
            API_Key = apikey;
        }

        #region Specific Searches
        public GGnResult<QuickUserInfo> GetQuickUserInfo()
        {
            AddSearchTermArgument("request", "quick_user");
            return SearchWithKey<GGnResult<QuickUserInfo>>();
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
