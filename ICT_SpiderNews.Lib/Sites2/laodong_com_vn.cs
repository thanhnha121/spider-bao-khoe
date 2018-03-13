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
    public class laodong_com_vn : ISite
    {       
        public IEnumerable<LinkInfo> GetLinkElements(string channelUrl)
        {
            var ret = new List<LinkInfo>();
            var doc = Utilities.GetHtmlDocument(channelUrl);
            var links = Utilities._GetNodes(doc, "//a[@class=\"cms-link\"]");
            if (links != null)
            {
                foreach(HtmlNode item in links)
                {
                    var href = item.Attributes["href"].Value;
                    if (!href.StartsWith("/"))
                        continue;
                    var regex = new System.Text.RegularExpressions.Regex(@"/(.+).bld", System.Text.RegularExpressions.RegexOptions.Singleline);
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
            articleElement.Title = Utilities._GetNode(doc, "//h1[@class=\"cms-title\"]");
            //excerpt
            articleElement.Excerpt = Utilities._GetNode(doc, "//p[@class=\"sapo cms-desc\"]");
            //author
            articleElement.Author = null;
            //content
            articleElement.Content = Utilities._GetNode(doc, "//div[@class=\"cms-body\"]");

            articleElement.Image = Utilities._GetNode(doc, "//img[@class=\"cms-photo\"]");

            //publishtime
            var pubdate = Utilities._GetNodeInnerText(doc, "//time[@class=\"cms-date\"]");
            var matches = System.Text.RegularExpressions.Regex.Matches(pubdate, @"(\d+):(\d+)(.+)\s(\d+)\/(\d+)\/(\d+)$", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            if (matches.Count > 0)
            {
                pubdate = string.Format("{0}-{1}-{2} {3}:{4}:00", matches[0].Groups[6].Value, matches[0].Groups[5].Value, matches[0].Groups[4].Value, matches[0].Groups[1].Value, matches[0].Groups[2].Value);
            }
            articleElement.PublishedTime = HtmlNode.CreateNode("<span>" + pubdate + "</span>");

            if (articleElement.Content != null)
            {
                //get large image as thumbnail                    
                if (articleElement.Image == null)
                {
                    articleElement.Image = articleElement.Content.SelectSingleNode(".//img[1]");
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
