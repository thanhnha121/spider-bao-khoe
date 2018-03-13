using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICT_SpiderNews.Lib
{
    public class RemoteArticleInfo
    {
        public string domain { get; set; }
        public string url { get; set; }
        public string title { get; set; }
        public string lead_image_url { get; set; }
        public string excerpt { get; set; }
        public string content { get; set; }
        public string author { get; set; }
        public string date_published { get; set; }
        public string error { get; set; }
        public string keyword { get; set; }

    }
}
