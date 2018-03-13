using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using ICT_SpiderNews.Lib;

namespace SpiderNews.Lib.SucKhoe24Gio
{
    public class suckhoegiadinh_com_vn : ISite
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

            var links = Utilities._GetNodes(doc, "//p[@class=\"title_list\"]//a");
            getLinks(ref ret, links);
            return ret;
        }

        private void getLinks(ref List<LinkInfo> ret, HtmlNodeCollection links)
        {
            string regexCol = "http://www.suckhoegiadinh.com.vn";
            if (links != null)
            {
                foreach (HtmlNode item in links)
                {
                    if (item.Attributes["href"] == null)
                        continue;
                    var href = item.Attributes["href"].Value;
                    if (!href.StartsWith("http"))
                        href = "http://www.suckhoegiadinh.com.vn" + href;
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
            articleElement.Title = Utilities._GetNode(doc, "//h1[@class=\"title\"]", string.Empty);
            //excerpt
            articleElement.Excerpt = Utilities._GetNode(doc, "//div[@id=\"detail_content\"]//p[1]//span//b", string.Empty);
            //author
            articleElement.Author = Utilities._GetNode(doc, "//div[@id=\"detail_content\"]//p[@style=\"TEXT-ALIGN: right\"]", string.Empty);
            //content
            articleElement.Content = Utilities._GetNode(doc, "//div[@id=\"detail_content\"]", string.Empty);
            //publishTime
            articleElement.PublishedTime = Utilities._GetNode(doc, "//div[@class=\"bar_video width_common detail_page\"]//p[@class=\"p_time\"]", string.Empty);
            //keyword
            articleElement.Keyword = Utilities._GetNodes(doc, "//div[@class=\"tag_detail width_common\"]//a[@class=\"item_tag\"]", string.Empty);
            //relation article

            if (articleElement.Content != null)
            {
                //get large image as thumbnail                    
                articleElement.Image = articleElement.Content.SelectSingleNode("//meta[@property=\"og:image\"]");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//div[@id=\"detail_content\"]//p[@style=\"margin:0px 0px;font-family: Helvetica Neue, Helvetica, Arial, sans-serif;font-weight: 500;\"]");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//div[@id=\"detail_content\"]//p[@style=\"TEXT-ALIGN: right\"][2]");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//div[@id=\"detail_content\"]//div[@style=\"text-align:center\"]");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//div[@id=\"detail_content\"]//div[@class=\"like_detail_bottom social-skgd\"]");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//div[@id=\"detail_content\"]//div[@class=\"tag_detail width_common\"]");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//div[@id=\"detail_content\"]//div[@class=\"width_common noborder\"]");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//div[@id=\"detail_content\"]//div[@class=\"width_common phantrang\"]");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//div[@id=\"detail_content\"]//ins[@class=\"adsbygoogle\"]");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//div[@id=\"detail_content\"]//script");
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
