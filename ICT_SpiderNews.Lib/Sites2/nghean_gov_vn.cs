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
    public class nghean_gov_vn : ISite
    {       
        public IEnumerable<LinkInfo> GetLinkElements(string channelUrl)
        {
            var ret = new List<LinkInfo>();
            var doc = Utilities.GetHtmlDocument(channelUrl);
            var links = Utilities._GetNodes(doc, "//*[@id=\"mainContent\"]/table/tbody/tr/td[3]/table/tbody/tr[2]/td/div/div/table[3]/tbody/tr/td/table", ".//a");
            if (links != null)
            {
                foreach(HtmlNode item in links)
                {
                    var href = item.Attributes["href"].Value;
                    if (!href.StartsWith("/"))
                        continue;
                    ret.Add(new LinkInfo
                    {
                        Url = href
                    });
                }
            }
            return ret;

        }

        public ArticleElementInfo GetArticleElement(string articleUrl)
        {
            var articleElement = new ArticleElementInfo();

            var doc = Utilities.GetHtmlDocument(articleUrl);

            //title
            articleElement.Title = Utilities._GetNode(doc, "//span[@class=\"news_title\"]");
            //excerpt
            //articleElement.Excerpt = Utilities._GetNode(doc, "//td[@class=\"news_content\"]", ".//strong");
            //author
            articleElement.Author = null;
            //content
            articleElement.Content = Utilities._GetNode(doc, "//td[@class=\"news_content\"]");
            if (articleElement.Content != null)
            {                
                //get large image as thumbnail                    
                articleElement.Image = articleElement.Content.SelectSingleNode(".//img[1]");

                if (articleElement.Content.SelectSingleNode(".//p[1]").InnerText.Trim() != string.Empty) {
                    articleElement.Excerpt = articleElement.Content.SelectSingleNode(".//p[1]");

                    articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//*[@class=\"news_content\"]/p[1]");
                }
                else
                {
                    articleElement.Excerpt = articleElement.Content.SelectSingleNode(".//p[2]");

                    articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//*[@class=\"news_content\"]/p[1]");
                    articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//*[@class=\"news_content\"]/p[2]");
                }
                
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
