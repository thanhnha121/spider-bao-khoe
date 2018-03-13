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
    public class baodatviet_vn : ISite
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
            articleElement.Title = Utilities._GetNode(doc, "//h1[@class=\"title\"]");
            //excerpt
            var desNode = Utilities._GetNode(doc, "//h2[@class=\"lead\"]");
            string desc_text = desNode.InnerText;
            if (desc_text.Contains("-"))
            {
                desc_text = desc_text.Substring(desc_text.IndexOf("-") + 1).Trim();
            }
            articleElement.Excerpt = Utilities._CreateNodeFromString(desc_text);
            //author
            articleElement.Author = null;
            //content
            articleElement.Content = Utilities._GetNode(doc, "//div[@id=\"detail\"]");
            if (articleElement.Content != null)
            {                
                //get large image as thumbnail                    
                articleElement.Image = articleElement.Content.SelectSingleNode(".//img[1]");                
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//*[@class=\"bar-left_th\"]");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//h1[@class=\"title\"]");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//h2[@class=\"lead\"]");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//ul[@class=\"ul_relate\"]");

                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//div[@id=\"AdAsia\"]");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//div[@id=\"itvcplayer\"]");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//ins");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//div[@style=\"height:30px;margin-right:10px;float:right\"]");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "(//center)[last()]");
                               
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
