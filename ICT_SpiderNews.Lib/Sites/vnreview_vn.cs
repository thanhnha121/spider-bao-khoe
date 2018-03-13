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
    public class vnreview_vn : ISite
    {
        public IEnumerable<LinkInfo> GetLinkElements(string channelUrl)
        {
            var ret = new List<LinkInfo>();
            var doc = Utilities.GetHtmlDocument(channelUrl);
            var links = Utilities._GetNodes(doc, "//div[@class=\"asset_title\"]");
            if (links != null)
            {
                foreach (HtmlNode item in links)
                {
                    var a = item.SelectSingleNode(".//a[1]");
                    var href = a.Attributes["href"].Value;
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
            articleElement.Title = Utilities._GetNode(doc, "//h1[@itemprop=\"name\"]");
            //excerpt
            articleElement.Excerpt = Utilities._GetNode(doc, "//div[@class=\"journal-content-article\"]", ".//strong[1]");
            //author
            articleElement.Author = null;// Utilities._GetNode(doc, "//p[@class=\"author\"]");
            //content
            articleElement.Content = Utilities._GetNode(doc, "//div[@class=\"journal-content-article\"]");
            if (articleElement.Content != null)
            {
                //get large image as thumbnail                    
                articleElement.Image = articleElement.Content.SelectSingleNode(".//img[1]");
                                
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, ".//strong[1]");                
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
