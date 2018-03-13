using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using ICT_SpiderNews.Lib;

namespace SpiderNews.Lib.SucKhoe24Gio
{
    public class phunuonline_com_vn : ISite
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

            var links = Utilities._GetNodes(doc, "//h2[@class=\"article-title\"]//a");
            var links1 = Utilities._GetNodes(doc, "//h3[@itemprop=\"name\"]//a");
            getLinks(ref ret, links);
            getLinks(ref ret, links1);
            return ret;
        }

        private void getLinks(ref List<LinkInfo> ret, HtmlNodeCollection links)
        {
            string regexCol = "http://phunuonline.com.vn";
            if (links != null)
            {
                foreach (HtmlNode item in links)
                {
                    if (item.Attributes["href"] == null)
                        continue;
                    var href = item.Attributes["href"].Value.Replace("..", "");
                    if (!href.StartsWith("http"))
                        href = "http://phunuonline.com.vn" + href;
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
            articleElement.Title = Utilities._GetNode(doc, "//h1[@class=\"article-title\"]", string.Empty);
            //excerpt
            articleElement.Excerpt = Utilities._GetNode(doc, "//blockquote[@class=\"article-intro\"]", string.Empty);
            articleElement.Excerpt = Utilities._RemoveNodeForNode(articleElement.Excerpt, "//blockquote[@class=\"article-intro\"]//div");
            //author
            articleElement.Author = Utilities._GetNode(doc, "//p[@style=\"text-align:right\"]//strong", string.Empty);
            //content
            articleElement.Content = Utilities._GetNode(doc, "//section[@class=\"article-content\"]", string.Empty);
            //publishTime
            articleElement.PublishedTime = Utilities._GetNode(doc, "//p[@class=\"time \"]", string.Empty);
            //keyword
            articleElement.Keyword = Utilities._GetNodes(doc, "//div[@class=\"tags\"]//span//a", string.Empty);
            //relation article

            if (articleElement.Content != null)
            {
                //get large image as thumbnail                    
                articleElement.Image = articleElement.Content.SelectSingleNode("//meta[@property=\"og:image\"]");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//section[@class=\"article-content\"]//ul[@class=\"ul_relate\"]");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//section[@class=\"article-content\"]//div[@id=\"bs-inread-container\"]");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//section[@class=\"article-content\"]//div[@id=\"bsinread\"]");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//section[@class=\"article-content\"]//div[@class=\"bysource\"]");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//section[@class=\"article-content\"]//div[@class=\"tags\"]");
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
