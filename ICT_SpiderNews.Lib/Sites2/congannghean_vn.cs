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
    public class congannghean_vn : ISite
    {       
        public IEnumerable<LinkInfo> GetLinkElements(string channelUrl)
        {
            var ret = new List<LinkInfo>();
            var doc = Utilities.GetHtmlDocument(channelUrl);
            var links = Utilities._GetNodes(doc, "//a[@class=\"title-2 Article\"]");
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
            articleElement.Title = Utilities._GetNode(doc, "//h1[@id=\"title\"]");
            //excerpt
            articleElement.Excerpt = Utilities._GetNode(doc, "//*[@id=\"content\"]/div[1]");
            //author
            articleElement.Author = null;
            //content
            articleElement.Content = Utilities._GetNode(doc, "//div[@id=\"content\"]");

            //publishtime
            var pubdate = Utilities._GetNodeInnerText(doc, "//div[@id=\"date\"]");
            var matches = System.Text.RegularExpressions.Regex.Matches(pubdate, @"(.+)\s(\d+)\/(\d+)\/(\d+),\s(\d+):(\d+)(.+)$", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            if (matches.Count > 0)
            {
                pubdate = string.Format("{0}-{1}-{2} {3}:{4}:00", matches[0].Groups[4].Value, matches[0].Groups[3].Value, matches[0].Groups[2].Value, matches[0].Groups[5].Value, matches[0].Groups[6].Value);

            }
            articleElement.PublishedTime = HtmlNode.CreateNode("<span>" + pubdate + "</span>");

            if (articleElement.Content != null)
            {                
                //get large image as thumbnail                    
                articleElement.Image = articleElement.Content.SelectSingleNode(".//img[1]");

                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//*[@id=\"content\"]/p[1]");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//*[@id=\"content\"]/p[2]");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//*[@id=\"content\"]/p[3]");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, "//*[@id=\"content\"]/div[1]");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, ".//table[@class=\"rl box leftside\"]");
                articleElement.Content = Utilities._RemoveNodeForNode(articleElement.Content, ".//table[@class=\"rl center\"]");
                

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
