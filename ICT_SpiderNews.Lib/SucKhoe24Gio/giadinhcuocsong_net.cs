using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using ICT_SpiderNews.Lib;

namespace SpiderNews.Lib.SucKhoe24Gio
{
    public class giadinhcuocsong_net : ISite
    {
        public IEnumerable<LinkInfo> GetLinkElements(string channelUrl)
        {
            Uri myUri = new Uri(channelUrl);
            string reqSource = myUri.Host;
            if (!string.IsNullOrEmpty(reqSource))
            {
                reqSource = reqSource.Replace(".", "_").ToLower().Trim();
            }

            var ret = new List<LinkInfo>();
            var doc = Utilities.GetHtmlDocument(channelUrl);
            if (doc == null)
                return ret;

            var links = Utilities._GetNodes(doc, "//div[@class=\"td-module-thumb\"]//a");
            var links1 = Utilities._GetNodes(doc, "//h3[@class=\"entry-title td-module-title\"]//a");
            getLinks(ref ret, links);
            getLinks(ref ret, links1);
            return ret;
        }

        private void getLinks(ref List<LinkInfo> ret, HtmlNodeCollection links)
        {
            string regexCol = "http://giadinhcuocsong.net";
            if (links != null)
            {
                foreach (HtmlNode item in links)
                {
                    if (item.Attributes["href"] == null)
                        continue;
                    var href = item.Attributes["href"].Value.Replace("..", "");
                    if (!href.StartsWith("http"))
                        href = "http://giadinhcuocsong.net" + href;
                    var regex = new System.Text.RegularExpressions.Regex(@regexCol, System.Text.RegularExpressions.RegexOptions.Singleline);
                    if (regex.IsMatch(href))
                    {
                        if (ret.All(c => c.Url != href))
                        {
                            ret.Add(new LinkInfo
                            {
                                Url = href
                            });
                        }
                    }
                }
            }
        }

        public ArticleElementInfo GetArticleElement(string articleUrl)
        {
            Uri myUri = new Uri(articleUrl);
            string reqSource = myUri.Host;
            if (!string.IsNullOrEmpty(reqSource))
            {
                reqSource = reqSource.Replace(".", "_").ToLower().Trim();
            }

            var articleElement = new ArticleElementInfo();
            var doc = Utilities.GetHtmlDocument(articleUrl);

            //title
            articleElement.Title = Utilities._GetNode(doc, "//h1[@class=\"entry-title\"]", string.Empty);
            //excerpt
            articleElement.Excerpt = Utilities._GetNode(doc, "//h4[@style=\"text-align: justify;\"]//strong", string.Empty);
            //author
            articleElement.Author = Utilities._GetNode(doc, "//p[@style=\"text-align: right;\"]//em", string.Empty);
            //content
            articleElement.Content = Utilities._GetNode(doc, "//div[@class=\"td-post-content td-pb-padding-side\"]", string.Empty);
            //publishTime
            articleElement.PublishedTime = Utilities._GetNode(doc, "//div[@class=\"time_detail_news\"]", string.Empty);
            //keyword
            articleElement.Keyword = Utilities._GetNodes(doc, "//ul[@class=\"td-tags td-post-small-box clearfix\"]//li//a", string.Empty);
            //relation article

            if (articleElement.Content != null)
            {
                //get large image as thumbnail                    
                articleElement.Image = articleElement.Content.SelectSingleNode("//meta[@property=\"og:image\"]");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//div[@class=\"td-post-content td-pb-padding-side\"]//div");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//div[@class=\"td-post-content td-pb-padding-side\"]//h4");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//div[@class=\"td-post-content td-pb-padding-side\"]//p[@style=\"text-align: right;\"]//em");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//div[@class=\"td-post-content td-pb-padding-side\"]//iframe");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//div[@class=\"td-post-content td-pb-padding-side\"]//script");
            }

            return articleElement;
        }

        public string[] GetStringToReplace()
        {
            return new string[]
            {
                " xmlns=\"http://www.w3.org/1999/xhtml\"",
            };
        }

    }
}
