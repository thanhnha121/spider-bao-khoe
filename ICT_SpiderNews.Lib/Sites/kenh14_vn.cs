using System.Collections.Generic;
using SpiderNews.Lib;

namespace ICT_SpiderNews.Lib
{
    public class kenh14_vn : ISite
    {       
        public IEnumerable<LinkInfo> GetLinkElements(string channelUrl)
        {
            var ret = new List<LinkInfo>();            
            return ret;

        }

        public ArticleElementInfo GetArticleElement(string articleUrl)
        {
            var articleElement = new ArticleElementInfo();

            var doc = Utilities.GetHtmlDocument(articleUrl);

            //title
            articleElement.Title = Utilities._GetNode(doc, "//h1[@class=\"kbwc-title\"]");
            //excerpt
            articleElement.Excerpt = Utilities._GetNode(doc, "//h2[@class=\"knc-sapo\"]");
            //author
            articleElement.Author = null;
            //content
            articleElement.Content = Utilities._GetNode(doc, "//div[@class=\"knc-content\"]");
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
