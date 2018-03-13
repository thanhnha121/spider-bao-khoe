using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using HtmlAgilityPack;

namespace ICT_SpiderNews.Lib
{
    public class ArticleElementInfo
    {
        public HtmlNode Title { get; set; }
        public HtmlNode Excerpt { get; set; }
        public HtmlNode Content { get; set; }
        public HtmlNode Image { get; set; }
        public HtmlNode Author { get; set; }
        public HtmlNode PublishedTime { get; set; }
        public HtmlNodeCollection Keyword { get; set; }

    }
}
