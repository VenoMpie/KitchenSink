using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace VenoMpie.Common.Interfaces.SRRDB
{
    public class Release
    {
        public string release { get; set; }
        public DateTime date { get; set; }
        public string hasNFO { get; set; }
        public string hasSRS { get; set; }

        public Release(string Release, DateTime ReleaseDate, string HasNFO, string HasSRS)
        {
            release = Release;
            date = ReleaseDate;
            hasNFO = HasNFO;
            hasSRS = HasSRS;
        }
    }
    public class SearchResult
    {
        public List<Release> results { get; set; }
        public long resultsCount { get; set; }
        public string[] query { get; set; }

        public SearchResult()
        {
            results = new List<Release>();
        }
        
    }
    public class SRRDB : JSONAPISearchBase
    {
        public override string APIPath { get { return "http://www.srrdb.com/api/search/"; } }

        public enum SRRSearchKeyWordsEnum
        {
            r,
            group,
            date,
            nfo,
            srs,
            foreign,
            confirmed,
            rarhash,
            category,
            imdb,
            genre,
            language,
            country,
            archivecrc,
            osohash,
            lower,
            firstupper,
            compressed,
            word,
            order,
            skip,
            skipr,
            storerealfilename,
            storerealcrc,
            userid,
            user
        }
        public SRRDB SearchBy(SRRSearchKeyWordsEnum search, string searchString)
        {
            switch (search)
            {
                case SRRSearchKeyWordsEnum.archivecrc:
                    AddSearchTerm("archive-crc:" + searchString);
                    break;
                default:
                    AddSearchTerm(Enum.GetName(typeof(SRRSearchKeyWordsEnum), search) + ":" + searchString);
                    break;
            }
            return this;
        }
        public Task DownloadSRR(Release releaseName, string toFile)
        {
            return DownloadSRR(releaseName.release, toFile);
        }
        public Task DownloadSRR(string releaseName, string toFile)
        {
            return Task.Run(() =>
            {
                string downloadUrl = "http://www.srrdb.com/download/srr/" + releaseName;
                WebClient client = new WebClient();
                client.DownloadFile(downloadUrl, toFile);
            });
        }
        public SearchResult Search()
        {
            return Search<SearchResult>();
        }
        public SearchResult Deserialize(string deserializeString)
        {
            return Deserialize<SearchResult>(deserializeString);
        }
    }
}
