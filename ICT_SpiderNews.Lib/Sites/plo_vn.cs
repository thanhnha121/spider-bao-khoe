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
    public class plo_vn :  ISite
    {
        public IEnumerable<LinkInfo> GetLinkElements(string channelUrl)
        {
            var ret = new List<LinkInfo>();
            var doc = Utilities.GetHtmlDocument(channelUrl);
            var links = Utilities._GetNodes(doc, "//div[@id=\"content-left\"]", "//a[@class=\"cms-link\"]");
            if (links != null)
            {
                foreach (HtmlNode item in links)
                {
                    var href = item.Attributes["href"].Value;                    
                    if (!ret.Any(c => c.Url == href))
                    {
                        ret.Add(new LinkInfo
                        {
                            Url = href
                        });
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
            articleElement.Title = Utilities._GetNode(doc, "//h1[@class=\"main-title cms-title\"]");
            //excerpt
            articleElement.Excerpt = Utilities._GetNode(doc, "//div[@class=\"sapo cms-desc\"]");
            //author
            articleElement.Author = null;// Utilities._GetNode(doc, "//p[@class=\"author\"]");
            //content
            articleElement.Content = Utilities._GetNode(doc, "//div[@class=\"cms-body\"]");
            if (articleElement.Content != null)
            {
                //get large image as thumbnail                    
                articleElement.Image = articleElement.Content.SelectSingleNode(".//img[1]");
                
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
