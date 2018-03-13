using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using ICT_SpiderNews.Lib;

namespace SpiderNews.Lib.SucKhoe24Gio
{
    public class www_yeutretho_vn : ISite
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

            var links = Utilities._GetNodes(doc, "//div[@class=\"col55per fl mar_bottom20\"]//a[1]");
            var links1 = Utilities._GetNodes(doc, "//div[@class=\"col48per fl\"]//a[1]");
            var links2 = Utilities._GetNodes(doc, "//ul[@class=\"list_news_two\"]//li[@class=\"pkg\"]//a");
            var links3 = Utilities._GetNodes(doc, "//div[@class=\"info_cate\"]//a");
            getLinks(ref ret, links);
            getLinks(ref ret, links1);
            getLinks(ref ret, links2);
            getLinks(ref ret, links3);
            return ret;
        }

        private void getLinks(ref List<LinkInfo> ret, HtmlNodeCollection links)
        {
            string regexCol = "http://www.yeutretho.vn";
            if (links != null)
            {
                foreach (HtmlNode item in links)
                {
                    if (item.Attributes["href"] == null)
                        continue;
                    var href = item.Attributes["href"].Value.Replace("..", "");
                    if (!href.StartsWith("http"))
                        href = "http://www.yeutretho.vn" + href;
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
            articleElement.Title = Utilities._GetNode(doc, "//h1//div[@class=\"title_detail_news\"]", string.Empty);
            //excerpt
            articleElement.Excerpt = Utilities._GetNode(doc, "//div[@class=\"sapo_detail fr fontSlabB\"]", string.Empty);
            articleElement.Excerpt = Utilities._RemoveNodeForNode(articleElement.Excerpt, "//div[@class=\"sapo_detail fr fontSlabB\"]//div");
            //author
            articleElement.Author = Utilities._GetNode(doc, "//div[@class=\"author_undefined\"]", string.Empty);
            //content
            articleElement.Content = Utilities._GetNode(doc, "//div[@id=\"cotent_detail\"]", string.Empty);
            //publishTime
            articleElement.PublishedTime = Utilities._GetNode(doc, "//div[@class=\"time_detail_news f11\"]", string.Empty);
            //keyword
            articleElement.Keyword = Utilities._GetNodes(doc, "//div[@class=\"tag_detail mar_bottom15\"]//a", string.Empty);
            //relation article

            if (articleElement.Content != null)
            {
                //get large image as thumbnail                    
                articleElement.Image = articleElement.Content.SelectSingleNode("//meta[@property=\"og:image\"]");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//div[@id=\"cotent_detail\"]//ins");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//div[@id=\"cotent_detail\"]//script");
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
