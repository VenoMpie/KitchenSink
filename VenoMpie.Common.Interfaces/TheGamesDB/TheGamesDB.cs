using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using VenoMpie.Common.Interfaces.TheGamesDB.SearchResults;

namespace VenoMpie.Common.Interfaces.TheGamesDB
{
    public enum SearchParameters
    {
        name,
        exactname,
        id,
        platform,
        genre,
        time
    }
    public class TheGamesDB
    {
        System.Net.WebClient client = new System.Net.WebClient() { Encoding = System.Text.Encoding.UTF8 }; //TheGamesDB uses UTF8
        private const string TheGamesDBAPIPath = "http://thegamesdb.net/api/";
        private string BuildAPIPath<T>(Dictionary<SearchParameters, object> parameters)
        {
            bool isFirst = true;
            StringBuilder sb = new StringBuilder(TheGamesDBAPIPath + typeof(T).Name + ".php?");
            foreach (var searchKey in parameters)
            {
                if (!isFirst) sb.Append("&");
                sb.AppendFormat("{0}={1}", Enum.GetName(typeof(SearchParameters), searchKey.Key), (searchKey.Value.GetType() == typeof(string)) ? searchKey.Value.ToString().Replace(" ", "%20") : searchKey.Value);
                isFirst = false;
            }
            return sb.ToString();
        }
        public T MakeWebCall<T>(SearchParameters searchType, object parameter)
        {
            string results = client.DownloadString(BuildAPIPath<T>(new Dictionary<SearchParameters, object>() { { searchType, parameter } }));
            return DeserializeOnly<T>(results);
        }
        public T MakeWebCall<T>(Dictionary<SearchParameters, object> searchParameters)
        {
            string results = client.DownloadString(BuildAPIPath<T>(searchParameters));
            return DeserializeOnly<T>(results);
        }
        public T MakeWebCall<T>()
        {
            string results = client.DownloadString(TheGamesDBAPIPath + typeof(T).Name + ".php");
            return DeserializeOnly<T>(results);
        }
        public T DeserializeOnly<T>(string xml)
        {
            StringReader sr = new StringReader(xml);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            T retValue = (T)serializer.Deserialize(sr);
            sr.Close();
            //TODO: Fix this, it only goes 1 level deep, must be recursive
            foreach (var prop in retValue.GetType().GetProperties().Where(a => a.PropertyType.IsPublic && a.PropertyType == typeof(string)))
            {
                var value = prop.GetValue(retValue);
                if (value != null) prop.SetValue(retValue, System.Net.WebUtility.HtmlDecode(value.ToString()));
            }
            foreach (var field in retValue.GetType().GetFields().Where(a => a.IsPublic && a.ReflectedType != null))
            {
                var fieldInstance = field.GetValue(retValue);
                foreach (var prop in fieldInstance.GetType().GetProperties().Where(a => a.PropertyType.IsPublic && a.PropertyType == typeof(string)))
                {
                    var value = prop.GetValue(fieldInstance);
                    if (value != null) prop.SetValue(fieldInstance, System.Net.WebUtility.HtmlDecode(value.ToString()));
                }
                field.SetValue(retValue, fieldInstance);
            }
            return retValue;
        }
    }
}
