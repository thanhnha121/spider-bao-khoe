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
    public class autopro_com_vn : ISite
    {       
        public IEnumerable<LinkInfo> GetLinkElements(string channelUrl)
        {
            var ret = new List<LinkInfo>();
            var doc = Utilities.GetHtmlDocument(channelUrl);
            var links = Utilities._GetNodes(doc, "//a[@data-linktype=\"newsdetail\"]");
            if (links != null)
            {
                foreach(HtmlNode item in links)
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
            articleElement.Title = Utilities._GetNode(doc, "//h1[@id=\"title-h1\"]");
            //excerpt
            articleElement.Excerpt = Utilities._GetNode(doc, "//div[@class=\"sapo-news-detail\"]", "//h2");
            //author
            articleElement.Author = null;
            //content
            articleElement.Content = Utilities._GetNode(doc, "//div[@id=\"content-id\"]");
            if (articleElement.Content != null)
            {
                //get large image as thumbnail                    
                articleElement.Image = articleElement.Content.SelectSingleNode(".//img[1]");
                articleElement.Content.SelectSingleNode(".//img[1]").Remove();
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//div[@class=\"sapo-news-detail\"]");
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
