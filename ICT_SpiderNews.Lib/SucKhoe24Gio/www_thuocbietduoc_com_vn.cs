using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using ICT_SpiderNews.Lib;

namespace SpiderNews.Lib.SucKhoe24Gio
{
    public class www_thuocbietduoc_com_vn : ISite
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

            var links = Utilities._GetNodes(doc, "//a[@class=\"textnewhead_home1\"]");
            getLinks(ref ret, links);
            return ret;
        }

        private void getLinks(ref List<LinkInfo> ret, HtmlNodeCollection links)
        {
            string regexCol = "https://www.thuocbietduoc.com.vn";
            if (links != null)
            {
                foreach (HtmlNode item in links)
                {
                    if (item.Attributes["href"] == null)
                        continue;
                    var href = item.Attributes["href"].Value.Replace("..", "");
                    if (!href.StartsWith("http"))
                        href = "https://www.thuocbietduoc.com.vn" + href;
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
            articleElement.Title = Utilities._GetNode(doc, "//h1[@class=\"newstitle\"]", string.Empty);
            //excerpt
            articleElement.Excerpt = Utilities._GetNode(doc, "//h2[@class=\"newsAbtract\"]", string.Empty);
            //author
            articleElement.Author = Utilities._GetNode(doc, "//div[@class=\"author\"]", string.Empty);
            //content
            articleElement.Content = Utilities._GetNode(doc, "//div[@class=\"content\"]", string.Empty);
            //publishTime
            articleElement.PublishedTime = Utilities._GetNode(doc, "//span[@class=\"newsdate\"]", string.Empty);
            //keyword
            articleElement.Keyword = Utilities._GetNodes(doc, "//h3[@class=\"keyword\"]//a", string.Empty);
            //relation article

            if (articleElement.Content != null)
            {
                //get large image as thumbnail                    
                articleElement.Image = articleElement.Content.SelectSingleNode("//meta[@property=\"og:image\"]");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//div[@class=\"content\"]//h1");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//div[@class=\"content\"]//h2");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//div[@class=\"content\"]//div[@style=\"padding-top:10px; padding-bottom: 10px; background-color: #fff;border-bottom: solid 1px silver; border-top: solid 1px silver;\"]");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//div[@class=\"content\"]//iframe");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//div[@class=\"content\"]//h3");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//div[@class=\"content\"]//div[@class=\"faq-listnext10\"]");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//div[@class=\"content\"]//div[@class=\"mtp10\"]");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//div[@class=\"content\"]//table");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//div[@class=\"content\"]//div[@class=\"news-listnext10\"]");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//div[@class=\"content\"]//script");
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
