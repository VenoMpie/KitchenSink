using System.Net;
using System.Web;

namespace VenoMpie.Common.Interfaces
{
    public abstract class JSONAPISearchBase
    {
        WebClient client = new WebClient();
        public abstract string APIPath { get; }
        protected string SearchUrl = "";

        public JSONAPISearchBase()
        {
            SearchUrl = APIPath;
        }
        public void AddSearchTermArgument(string key, string searchTerm)
        {
            AddSearchTerm((SearchUrl.IndexOf("?") > 0 ? "&" : "?") + key + "=" + HttpUtility.UrlEncode(searchTerm));
        }
        public void AddSearchTerm(string searchTerm)
        {
            SearchUrl += searchTerm;
        }
        public T Search<T>() where T : class, new()
        {
            if (SearchUrl == "") return default(T);
            string results = client.DownloadString(SearchUrl);
            SearchUrl = APIPath;
            return Deserialize<T>(results);
        }
        public T Deserialize<T>(string deserializeString) where T : class, new()
        {
            if (!deserializeString.StartsWith("{")) deserializeString = deserializeString.Substring(deserializeString.IndexOf("{"));
            if (!deserializeString.EndsWith("}")) deserializeString = deserializeString.Substring(0, deserializeString.LastIndexOf("}") + 1);
            T resultSet = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(deserializeString);
            return resultSet;
        }
        public string Serialize(object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }
    }
}
