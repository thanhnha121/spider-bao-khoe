using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICT_SpiderNews.Lib
{
    public interface ISite
    {        
        IEnumerable<LinkInfo> GetLinkElements(string channelUrl);
        ArticleElementInfo GetArticleElement(string articleUrl);
        string[] GetStringToReplace();
    }
}
