using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using SpiderNews.Lib;

namespace ICT_SpiderNews.Lib
{
    public class news_zing_vn : ISite
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

            var links3 = Utilities._GetNodes(doc, "//p[@class=\"title\"]//a");
            getLinks(ref ret, links3);
            return ret;
        }

        private void getLinks(ref List<LinkInfo> ret, HtmlNodeCollection links)
        {
            string regexCol = "http://news.zing.vn/";
            if (links != null)
            {
                foreach (HtmlNode item in links)
                {
                    if (item.Attributes["href"] == null)
                        continue;
                    var href = item.Attributes["href"].Value;
                    if (!href.StartsWith("http"))
                        href = "http://news.zing.vn" + href;
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
            articleElement.Excerpt = Utilities._GetNode(doc, "//p[@class=\"the-article-summary cms-desc\"]", string.Empty);
            //author
            articleElement.Author = Utilities._GetNode(doc, "//div[@class=\"the-article-credit\"]//p[@class=\"author\"]", string.Empty);
            //content
            articleElement.Content = Utilities._GetNode(doc, "//div[@class=\"the-article-body cms-body\"]", string.Empty);
            //publishTime
            articleElement.PublishedTime = Utilities._GetNode(doc, "//ul[@class=\"the-article-meta\"]//li[@class=\"the-article-publish cms-date\"]", string.Empty);
            //keyword
            articleElement.Keyword = Utilities._GetNodes(doc, "//p[@class=\"the-article-tags\"]//a", string.Empty);
            //relation article

            if (articleElement.Content != null)
            {
                //get large image as thumbnail                    
                articleElement.Image = articleElement.Content.SelectSingleNode("//meta[@property=\"og:image\"]");
                //articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, ".//p//small");
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
