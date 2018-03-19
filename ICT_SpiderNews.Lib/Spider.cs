using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using BaoKhoe;
using HtmlAgilityPack;
using ICT_SpiderNews.Lib;
using SpiderNews.Lib.Sites3;
using SpiderNews.Lib.SucKhoe24Gio;

namespace SpiderNews.Lib
{
    public class Spider
    {
        private ISite Site { get; set; }
        private bool _isConsole;
        private readonly Logging _logging;

        private readonly AppDBContext _appDbContext;

        public Spider(AppDBContext appDbContext, Logging logging, bool isConsole = true)
        {
            _logging = logging;
            _isConsole = isConsole;
            _appDbContext = _appDbContext == null ? new AppDBContext() : appDbContext;

        }

        /// <summary>
        /// get channel article
        /// </summary>
        /// <param name="channelUrl"></param>
        public List<RemoteArticleInfo> GetArticles(string channelUrl)
        {
            _logging.Log("spider", "GET: " + channelUrl);

            var lstArticles = new List<RemoteArticleInfo>();

            SiteDetect(channelUrl);
            if (Site == null)
            {
                return lstArticles;
            }

            var lstLinks = Site.GetLinkElements(channelUrl);
            var linkInfos = lstLinks as LinkInfo[] ?? lstLinks.ToArray();
            if (linkInfos.Any())
            {
                Uri myUri = new Uri(channelUrl);
                foreach (LinkInfo item in linkInfos)
                {
                    string errorStr = string.Empty;
                    string articleUrl = item.Url;
                    try
                    {
                        if (!articleUrl.StartsWith("http"))
                        {
                            articleUrl = "http://" + myUri.Host + item.Url;
                        }

                        _logging.Log("spider", "GET: " + articleUrl, false);
                        if (_appDbContext.Articles.Count(x => x.SourceUrl.Equals(articleUrl)) == 0)
                        {
                            _logging.Log("spider", "GET: " + articleUrl);
                            var article = GetRemoteArticleInfo(articleUrl);

                            if (article != null)
                            {

                                lstArticles.Add(article);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logging.Log("spider_error", ex.Message);
                    }
                }
            }
            return lstArticles;
        }

        /// <summary>
        /// get detail article
        /// </summary>
        /// <param name="articleUrl"></param>
        /// <returns></returns>
        public RemoteArticleInfo GetRemoteArticleInfo(string articleUrl)
        {
            SiteDetect(articleUrl);

            // get article htmlnode
            ArticleElementInfo articleElement =  Site?.GetArticleElement(articleUrl);

            if (articleElement == null)
                return null;

            if (articleElement.Content != null)
                articleElement.Content = Utilities._ClearHtmlTag(articleElement.Content);

            var remoteArticleInfo = new RemoteArticleInfo();
            //set content
            //domain
            Uri uri = new Uri(articleUrl);
            remoteArticleInfo.domain = uri.Host;
            //url
            remoteArticleInfo.url = articleUrl;
            //title
            remoteArticleInfo.title = articleElement.Title != null ? HttpUtility.HtmlDecode(articleElement.Title.InnerText.Trim()) : string.Empty;
            //_excerpt
            if (articleElement.Excerpt != null)
            {
                remoteArticleInfo.excerpt = string.IsNullOrEmpty(HttpUtility.HtmlDecode(articleElement.Excerpt.InnerText.Trim())) ? articleElement.Excerpt.Attributes["content"].Value.Trim() : HttpUtility.HtmlDecode(articleElement.Excerpt.InnerText.Trim());
            }
            else
            {
                remoteArticleInfo.excerpt = string.Empty;
            }
            //content
            remoteArticleInfo.content = articleElement.Content != null ? HttpUtility.HtmlDecode(articleElement.Content.InnerHtml) : string.Empty;

            //author
            remoteArticleInfo.author = articleElement.Author != null ? HttpUtility.HtmlDecode(articleElement.Author.InnerText.Trim()) : string.Empty;
            //_image
            if (articleElement.Image != null)
            {
                remoteArticleInfo.lead_image_url = articleElement.Image.Attributes["src"]?.Value.Trim() ?? articleElement.Image.Attributes["content"].Value.Trim();
                if (remoteArticleInfo.lead_image_url == null)
                {
                    return null;
                }
            }

            //publish time
            remoteArticleInfo.date_published = articleElement.PublishedTime != null ? HttpUtility.HtmlDecode(articleElement.PublishedTime.InnerText.Trim()) : string.Empty;

            //keyword
            if (articleElement.Keyword != null)
            {
                for (int i = 0; i < articleElement.Keyword.Count 
                    && (remoteArticleInfo.keyword + articleElement.Keyword[i].InnerText.Trim().Replace("#", "")).Length < 255; i++)
                {
                    string xxx = HttpUtility.HtmlDecode(articleElement.Keyword[i].InnerText.Trim().Replace("#", "").Replace("&2", "&#2"));
                    remoteArticleInfo.keyword += xxx;
                    if (i != articleElement.Keyword.Count - 1)
                    {
                        remoteArticleInfo.keyword += ',';
                    }
                }
            }
            else
            {
                remoteArticleInfo.keyword = string.Empty;
            }

            //replace
            remoteArticleInfo.content = Utilities._ClearContent(remoteArticleInfo.content);
            var replaceObj = Site.GetStringToReplace();
            if (replaceObj != null)
            {
                foreach (var obj in replaceObj)
                {
                    remoteArticleInfo.content = remoteArticleInfo.content.Replace(obj.ToString(), string.Empty);
                }
            }

            return remoteArticleInfo;

        }


        #region private

        private Sources GetSource(string reqUrl)
        {
            Uri myUri = new Uri(reqUrl);
            string reqSource = myUri.Host;
            if (!string.IsNullOrEmpty(reqSource))
            {
                reqSource = reqSource.Replace(".", "_").ToLower().Trim();
            }

            Sources mysite;
            try
            {
                mysite = Utilities.ParseEnum<Sources>(reqSource);
            }
            catch
            {
                mysite = Sources.none;
            }
            return mysite;
        }

        private enum Sources
        {
            none,
            autopro_com_vn,
            infogame_vn,
            khampha_vn,
            vietnamnet_vn,
            cafebiz_vn,
            giaoducthoidai_vn,
            kenh14_vn,
            baodatviet_vn,
            giadinh_net_vn,
            vnreview_vn,
            baoquocte_vn,
            baonghean_vn,
            congannghean_vn,
            laodong_com_vn,
            nghean_gov_vn,
            plo_vn,
            truyenhinhnghean_vn,
            suckhoedoisong_vn,
            soha_vn,
            ttvn_vn,
            thegioitre_vn,
            infonet_vn,
            bestie_vn,
            news_zing_vn,

            /* SucKhoe24Gio */
            www_suckhoegiadinh_com_vn,
            www_thuocbietduoc_com_vn,
            giadinhcuocsong_net,
            benh_vn,
            khoeplus_vn,
            phunuonline_com_vn,
            www_yeutretho_vn
        }
        private void SiteDetect(string Url)
        {
            Site = null;
            Sources source = GetSource(Url);
            switch (source)
            {
                // site 1
                case Sources.cafebiz_vn:
                    Site = new cafebiz_vn();
                    break;
                case Sources.infogame_vn:
                    Site = new infogame_vn();
                    break;
                case Sources.autopro_com_vn:
                    Site = new autopro_com_vn();
                    break;
                case Sources.khampha_vn:
                    Site = new khampha_vn();
                    break;
                case Sources.vietnamnet_vn:
                    Site = new vietnamnet_vn();
                    break;
                case Sources.kenh14_vn:
                    Site = new kenh14_vn();
                    break;
                case Sources.baodatviet_vn:
                    Site = new baodatviet_vn();
                    break;
                case Sources.vnreview_vn:
                    Site = new vnreview_vn();
                    break;

                //sites 2
                case Sources.baonghean_vn:
                    Site = new baonghean_vn();
                    break;
                case Sources.congannghean_vn:
                    Site = new congannghean_vn();
                    break;
                case Sources.laodong_com_vn:
                    Site = new laodong_com_vn();
                    break;
                case Sources.nghean_gov_vn:
                    Site = new nghean_gov_vn();
                    break;
                case Sources.plo_vn:
                    Site = new plo_vn();
                    break;
                case Sources.truyenhinhnghean_vn:
                    Site = new truyenhinhnghean_vn();
                    break;

                //sites 3
                case Sources.suckhoedoisong_vn:
                    Site = new suckhoedoisong_vn();
                    break;
                case Sources.soha_vn:
                    Site = new SohaVn();
                    break;
                case Sources.ttvn_vn:
                    Site = new ttvn_vn();
                    break;
                case Sources.thegioitre_vn:
                    Site = new thegioitre_vn();
                    break;
                case Sources.infonet_vn:
                    Site = new infonet_vn();
                    break;
                case Sources.bestie_vn:
                    Site = new bestie_vn();
                    break;
                case Sources.news_zing_vn:
                    Site = new news_zing_vn();
                    break;

                // SucKhoe24Gio
                case Sources.www_suckhoegiadinh_com_vn:
                    Site = new suckhoegiadinh_com_vn();
                    break;

                case Sources.www_thuocbietduoc_com_vn:
                    Site = new www_thuocbietduoc_com_vn();
                    break;

                case Sources.giadinhcuocsong_net:
                    Site = new giadinhcuocsong_net();
                    break;

                case Sources.benh_vn:
                    Site = new benh_vn();
                    break;

                case Sources.khoeplus_vn:
                    Site = new khoeplus_vn();
                    break;

                case Sources.phunuonline_com_vn:
                    Site = new phunuonline_com_vn();
                    break;

                case Sources.www_yeutretho_vn:
                    Site = new www_yeutretho_vn();
                    break;
            }
        }

        #endregion
    }

}
