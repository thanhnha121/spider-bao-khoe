using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using HtmlAgilityPack;
using SpiderNews.Lib;

namespace ICT_SpiderNews.Lib
{
    public class infogame_vn : ISite
    {       
        public IEnumerable<LinkInfo> GetLinkElements(string channelUrl)
        {
            var ret = new List<LinkInfo>();
            var doc = Utilities.GetHtmlDocument(channelUrl);
            var links = Utilities._GetNodes(doc, "//a");
            if (links != null)
            {
                foreach(HtmlNode item in links)
                {
                    var href = item.Attributes["href"].Value;
                    if (href.StartsWith("http://"))
                        continue;
                    if (href.StartsWith("/") == false)
                    {
                        href = "/" + href;
                    }

                    var regex = new System.Text.RegularExpressions.Regex(@"/(.+)/(.+)-([0-9]+).html", System.Text.RegularExpressions.RegexOptions.Singleline);
                    if (regex.IsMatch(href))
                    {
                        if(!ret.Any(c=>c.Url== href))
                        {
                            ret.Add(new LinkInfo
                            {
                                Url = href
                            });
                        }                        
                    }                    
                }
            }
            return ret;

        }

        public ArticleElementInfo GetArticleElement(string articleUrl)
        {
            var articleElement = new ArticleElementInfo();

            var doc = Utilities.GetHtmlDocument(articleUrl);

            //title
            articleElement.Title = Utilities._GetNode(doc, "//div[@class=\"chitiettin\"]", ".//h1");
            //excerpt
            articleElement.Excerpt = Utilities._GetNode(doc, "//div[@class=\"noidungchitiet\"]", ".//h2");
            //author
            articleElement.Author = null;// Utilities._GetNode(doc, "//p[@class=\"tacgia\"]", ".//span");
            //content
            articleElement.Content = Utilities._GetNode(doc, "//div[@class=\"noidungchitiet\"]");

            if (articleElement.Content != null)
            {
                string className = articleElement.Excerpt.Attributes["class"].Value;
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//h2[@class=\""+ className + "\"]");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//div[@class=\"thongtingame\"]");
                
                //get large image as thumbnail                    
                articleElement.Image = Utilities._GetNode(doc, "//div[@class=\"chitiettin\"]", ".//img");
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
