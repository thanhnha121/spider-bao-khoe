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
    public class khampha_vn : ISite
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
                    var regex = new System.Text.RegularExpressions.Regex(@"/(.+)/(.+)-c(.+).html$", System.Text.RegularExpressions.RegexOptions.Singleline);
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
            articleElement.Title = Utilities._GetNode(doc, "//h1[@class=\"baiviet-title\"]");
            //excerpt
            articleElement.Excerpt = Utilities._GetNode(doc, "//p[@class=\"baiviet-sapo\"]");
            //author
            articleElement.Author = null;// Utilities._GetNode(doc, "//div[@class=\"nguontin\"]");
            //content
            articleElement.Content = Utilities._GetNode(doc, "//div[@class=\" text-conent\"]");
            if (articleElement.Content != null)
            {                

                //get large image as thumbnail                    
                articleElement.Image = Utilities._GetNode(doc, "//img[@class=\"news-image\"]");
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
