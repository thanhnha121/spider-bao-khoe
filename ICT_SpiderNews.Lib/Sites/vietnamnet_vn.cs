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
    public class vietnamnet_vn : ISite
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
                    if (!href.StartsWith("/"))
                        continue;
                    var regex = new System.Text.RegularExpressions.Regex(@"/vn/(.+)/([\d]+)/(.+).html$", System.Text.RegularExpressions.RegexOptions.Singleline);
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
            articleElement.Title = Utilities._GetNode(doc, "//h1[@class=\"title\"]");
            //excerpt
            articleElement.Excerpt = Utilities._GetNode(doc, "//div[@id=\"ArticleContent\"]", ".//strong");
            //author
            articleElement.Author = null;
            //content
            articleElement.Content = Utilities._GetNode(doc, "//div[@id=\"ArticleContent\"]");
            if (articleElement.Content != null)
            {                
                //get large image as thumbnail                    
                articleElement.Image = articleElement.Content.SelectSingleNode(".//img[1]");

                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//*[@id=\"ArticleContent\"]/p[1]");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, ".//table[@class=\"quote center\"]");
                
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
