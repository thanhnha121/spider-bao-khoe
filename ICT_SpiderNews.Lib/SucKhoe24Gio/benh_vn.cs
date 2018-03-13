using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using ICT_SpiderNews.Lib;

namespace SpiderNews.Lib.SucKhoe24Gio
{
    public class benh_vn : ISite
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

            var links = Utilities._GetNodes(doc, "//div[@class=\"detail\"]//a");
            getLinks(ref ret, links);
            return ret;
        }

        private void getLinks(ref List<LinkInfo> ret, HtmlNodeCollection links)
        {
            string regexCol = "http://benh.vn";
            if (links != null)
            {
                foreach (HtmlNode item in links)
                {
                    if (item.Attributes["href"] == null)
                        continue;
                    var href = item.Attributes["href"].Value.Replace("..", "");
                    if (!href.StartsWith("http"))
                        href = "http://benh.vn" + href;
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
            articleElement.Title = Utilities._GetNode(doc, "//h1[@class=\"title-content\"]", string.Empty);
            //excerpt
            articleElement.Excerpt = Utilities._GetNode(doc, "//div[@itemtype=\"http://schema.org/WebPageElement\"]//div//strong//p", string.Empty);
            //author
            articleElement.Author = Utilities._GetNode(doc, "//p[@class=\"undefined\"]", string.Empty);
            //content
            articleElement.Content = Utilities._GetNode(doc, "//div[@itemprop=\"description\"]", string.Empty);
            //publishTime
            articleElement.PublishedTime = Utilities._GetNode(doc, "//time[@class=\"clearfix\"]", string.Empty);
            //keyword
            articleElement.Keyword = Utilities._GetNodes(doc, "//ul[@class=\"td-tags td-post-small-box clearfix\"]//li//a", string.Empty);
            //relation article

            if (articleElement.Content != null)
            {
                //get large image as thumbnail                    
                articleElement.Image = articleElement.Content.SelectSingleNode("//meta[@property=\"og:image\"]");
                if (!articleElement.Image.Attributes["content"].Value.Contains("http"))
                {
                    if (articleElement.Image.Attributes["content"].Value.StartsWith("/"))
                    {
                        articleElement.Image.Attributes["content"].Value =
                            "http://benh.vn" + articleElement.Image.Attributes["content"].Value;
                    }
                    else
                    {
                        articleElement.Image.Attributes["content"].Value =
                            "http://benh.vn/" + articleElement.Image.Attributes["content"].Value;
                    }
                    
                }
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
