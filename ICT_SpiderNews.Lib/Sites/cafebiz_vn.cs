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
    public class cafebiz_vn : ISite
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
                    var regex = new System.Text.RegularExpressions.Regex(@"/(.*)-([0-9]+).chn$", System.Text.RegularExpressions.RegexOptions.Singleline);
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
            articleElement.Excerpt = Utilities._GetNode(doc, "//h2[@class=\"sapo\"]");
            //author
            articleElement.Author = null;// Utilities._GetNode(doc, "//p[@class=\"p-author\"]");
            //content
            articleElement.Content = Utilities._GetNode(doc, "//div[@class=\"detail-content\"]");
            if (articleElement.Content != null)
            {
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//div[@type=\"link-content-footer\"]");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//p[@class=\"p-author\"]");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//p[@class=\"p-source\"]");
                
                //get large image as thumbnail                    
                articleElement.Image = Utilities._GetNode(doc, "//img[@class=\"img\"]");
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
