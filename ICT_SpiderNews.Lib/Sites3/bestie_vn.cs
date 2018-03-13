using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using SpiderNews.Lib;

namespace ICT_SpiderNews.Lib
{
    public class bestie_vn : ISite
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

            var links3 = Utilities._GetNodes(doc, "//h2//a");
            var links4 = Utilities._GetNodes(doc, "//h3//a");
            getLinks(ref ret, links3);
            getLinks(ref ret, links4);
            return ret;
        }

        private void getLinks(ref List<LinkInfo> ret, HtmlNodeCollection links)
        {
            string regexCol = "http://bestie.vn/";
            if (links != null)
            {
                foreach (HtmlNode item in links)
                {
                    if (item.Attributes["href"] == null)
                        continue;
                    var href = item.Attributes["href"].Value;
                    if (!href.StartsWith("http"))
                        href = "http://bestie.vn" + href;
                    var regex = new System.Text.RegularExpressions.Regex(@regexCol, System.Text.RegularExpressions.RegexOptions.Singleline);
                    if (regex.IsMatch(href))
                    {
                        if (!ret.Any(c => c.Url == href))
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
            articleElement.Title = Utilities._GetNode(doc, "//h1", string.Empty);
            //excerpt
            articleElement.Excerpt = Utilities._GetNode(doc, "//h2", string.Empty);
            //author
            articleElement.Author = Utilities._GetNode(doc, "//span[@itemprop=\"author\"]", string.Empty);
            //content
            articleElement.Content = Utilities._GetNode(doc, "//div[@class=\"post-content fs15content pb10 pt10\"]", string.Empty);
            //publishTime
            articleElement.PublishedTime = Utilities._GetNode(doc, "//span[@class=\"time f-elle-futura-book hidden-sm hidden-xs\"]", string.Empty);
            //keyword
            articleElement.Keyword = Utilities._GetNodes(doc, "//div[@class=\"col-md-10\"]//a[@class=\"tarhome fs10\"]", string.Empty);
            //relation article

            if (articleElement.Content != null)
            {
                //get large image as thumbnail                    
                articleElement.Image = articleElement.Content.SelectSingleNode(".//img[1]");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, ".//p//small");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, ".//div[@class=\"mb10 mt10\"]");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, ".//div[@class=\"row related-post-detail hidden-sm hidden-xs\"]");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, ".//div[@style=\"height: 1px; width: 1px; display: none;\"]");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, ".//p//iframe");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, ".//iframe");
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
