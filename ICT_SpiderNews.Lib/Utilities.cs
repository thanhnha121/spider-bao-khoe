using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using ICT_SpiderNews.Lib;
using VietCMS.AddOn.FileManager.Core.Common;

namespace SpiderNews.Lib
{
    public class Utilities
    {
        public static Logging Logging = null;

        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static HtmlDocument GetHtmlDocument(string url)
        {
            HtmlDocument doc = null;

            int retries = 3;
            bool downOk = false;
            while (true)
            {
                try
                {
                    var request = (HttpWebRequest)WebRequest.Create(url);
                    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.106 Safari/537.36";
                    request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    string xml;
                    using (XmlSanitizingStream reader =
                            new XmlSanitizingStream(response.GetResponseStream()))
                    {
                        xml = reader.ReadToEnd();

                        Uri uri = new Uri(url);
                        xml = xml.Replace(" src=\"/", " src=\"http://" + uri.Host + "/");

                    }

                    // xml contains no illegal characters
                    doc = new HtmlDocument
                    {
                        OptionOutputAsXml = true,
                        OptionFixNestedTags = true
                    };
                    doc.LoadHtml(xml);

                    downOk = true;

                    break;
                }
                catch (Exception ex)
                {
                    Logging?.Log("GetHtmlDocument ERROR", ex.Message);
                    if (--retries == 0) break;
                    else System.Threading.Thread.Sleep(1000);
                }
            }
            if (downOk)
            {
                return doc;
            }
            return null;
        }

        public static HtmlNodeCollection _GetNodes(HtmlDocument doc, string xpath1, string xpath2 = "")
        {
            try
            {
                if (string.IsNullOrEmpty(xpath2))
                {
                    return doc.DocumentNode.SelectNodes(xpath1);
                }
                else
                {
                    var node1 = doc.DocumentNode.SelectSingleNode(xpath1);
                    return node1?.SelectNodes(xpath2);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static HtmlNode _GetNode(HtmlDocument doc, string xpath1, string xpath2 = "")
        {
            try
            {
                var node1 = doc.DocumentNode.SelectSingleNode(xpath1);
                if (string.IsNullOrEmpty(xpath2))
                {
                    return node1;
                }
                else if (node1 != null)
                {
                    var node2 = node1.SelectSingleNode(xpath2);
                    return node2;
                }
                return null;
            }
            catch (Exception ex)
            {
                Logging?.Log("_GetNodes error", ex.Message);
                return null;
            }
        }

        public static HtmlNode _CreateNodeFromString(string input)
        {
            try
            {
                HtmlDocument doc = new HtmlDocument();
                HtmlNode textNode = doc.CreateElement("div");
                textNode.InnerHtml = HtmlDocument.HtmlEncode(input);
                return textNode;
            }
            catch
            {
                return null;
            }
        }

        public static HtmlNode _RemoveNodeForNode(HtmlNode node, string xpath1, string xpath2 = "")
        {
            try
            {
                if (string.IsNullOrEmpty(xpath2))
                {
                    var nodeToRemove = node.SelectNodes(xpath1);
                    if (nodeToRemove != null)
                    {
                        foreach (HtmlNode item in nodeToRemove)
                        {
                            item.Remove();
                        }
                    }
                }
                else
                {
                    var parentNode = node.SelectSingleNode(xpath1);

                    var nodeToRemove = parentNode?.SelectNodes(xpath2);
                    if (nodeToRemove != null)
                    {
                        foreach (HtmlNode item in nodeToRemove)
                        {
                            item.Remove();
                        }
                    }
                }
                return node;
            }
            catch (Exception ex) { Logging.Log("_RemoveNodeForNode ERROR", ex.Message); return null; }
        }

        public static HtmlNode _ChangeNodeOfVideo(HtmlNode node, string xpath1, string xpath2, string xAttriVideo)
        {
            try
            {
                if (string.IsNullOrEmpty(xpath2))
                {
                    var nodeToRemove = node.SelectNodes(xpath1);
                    if (nodeToRemove != null)
                    {
                        foreach (HtmlNode item in nodeToRemove)
                        {
                            var linkVideo = item.GetAttributeValue(xAttriVideo, string.Empty);
                            if (!string.IsNullOrEmpty(linkVideo))
                            {
                                node.InsertAfter(HtmlNode.CreateNode("<div algin=\"center\"><video width=\"480\" autoplay controls> <source src = \"" + linkVideo + "\"type = \"video/mp4\" ></video></div>"), item);
                            }
                            item.Remove();
                        }
                    }
                }
                else
                {
                    var parentNode = node.SelectSingleNode(xpath1);
                    var nodeToRemove = parentNode?.SelectNodes(xpath2);
                    if (nodeToRemove != null)
                    {
                        foreach (HtmlNode item in nodeToRemove)
                        {
                            var linkVideo = item.GetAttributeValue(xAttriVideo, string.Empty);
                            if (!string.IsNullOrEmpty(linkVideo))
                            {
                                node.InsertAfter(item, HtmlNode.CreateNode("<div algin=\"center\"><video width=\"480\" autoplay controls> <source src = \"" + linkVideo + "\"type = \"video/mp4\" ></video></div>"));
                            }
                            item.Remove();
                        }
                    }
                }
                return node;
            }
            catch (Exception ex) { Logging.Log("_ChangeNodeOfVideo ERROR", ex.Message); return null; }
        }

        public static HtmlNode _GetVideoVnexpress(HtmlNode node)
        {
            try
            {
                var nodeToRemove = node.SelectNodes("//div[@data-component-type=\"video\"]");
                if (nodeToRemove != null)
                {
                    foreach (HtmlNode item in nodeToRemove)
                    {
                        var linkVideo = "http://video.vnexpress.net/parser.html?id=" + item.Attributes["data-component-value"].Value + "&t=" + item.Attributes["data-component-typevideo"].Value;
                        if (!string.IsNullOrEmpty(linkVideo))
                        {
                            item.ParentNode.InsertAfter(HtmlNode.CreateNode("<iframe width=\"480\" height=\"270\" src=\"" + linkVideo + "\" frameborder=\"0\" allowfullscreen=\"\"></iframe>"), item);
                        }
                        item.Remove();
                    }
                }

                return node;
            }
            catch (Exception ex) { Logging.Log("_GetVideoVnexpress ERROR", ex.Message); return null; }
        }

        //call by backend cms
        public static string _ClearHtmlContent(string content)
        {
            try
            {
                //clear tags
                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(content);
                var myNode = _ClearHtmlTag(htmlDocument.DocumentNode);
                content = myNode.InnerHtml;

                //clear content
                content = _ClearContent(content);

                //clear comment block
                content = _RemoveHTMLComments(content);

                return content;
            }
            catch (Exception ex) { Logging.Log("_ClearHtmlContent ERROR", ex.Message); return content; }
        }

        public static HtmlNode _ClearHtmlTag(HtmlNode myNode)
        {
            try
            {
                //clear imgs tag
                var imgs = myNode.SelectNodes(".//img");
                if (imgs != null)
                {
                    foreach (HtmlNode img in imgs)
                    {
                        foreach (HtmlAttribute a in img.Attributes.ToList())
                        {
                            if (a.Name == "src" || a.Name == "alt" || a.Name == "title")
                            {
                                continue;
                            }
                            img.Attributes.Remove(a);
                        }
                    }
                }

                //clear div
                var divTags = myNode.SelectNodes("//div");
                if (divTags != null)
                {
                    foreach (HtmlNode tagNode in divTags)
                    {
                        tagNode.Attributes.RemoveAll();
                    }
                }

                //clear other tags
                var tags = new[] { "h1", "h2", "h3", "p", "table", "span", "strong" };
                foreach (string tag in tags)
                {
                    var tagNodes = myNode.SelectNodes("//" + tag);
                    if (tagNodes != null)
                    {
                        foreach (HtmlNode tagNode in tagNodes)
                        {
                            tagNode.Attributes.Remove("class");
                            tagNode.Attributes.Remove("type");
                            tagNode.Attributes.Remove("style");
                            tagNode.Attributes.Remove("id");
                        }
                    }
                }


                //remove tags
                var removeTags = new[] { "script", "style" };
                foreach (string tag in removeTags)
                {
                    var tagNodes = myNode.SelectNodes("//" + tag);
                    if (tagNodes != null)
                    {
                        foreach (HtmlNode tagNode in tagNodes)
                        {
                            tagNode.Remove();
                        }
                    }
                }

                //remove empty tags
                //Regex.Replace(input, @"<([^>/][^>]*)>((&nbsp;)*|\s*)</\1>", String.Empty);
                var checkTags = new[] { "h1", "h2", "h3", "p", "table", "span", "strong", "div" };
                foreach (string tag in checkTags)
                {
                    var tagNodes = myNode.SelectNodes("//" + tag);
                    if (tagNodes != null)
                    {
                        foreach (HtmlNode tagNode in tagNodes)
                        {
                            var html = System.Web.HttpUtility.HtmlDecode(tagNode.InnerHtml);
                            if (html != null)
                            {
                                html = html.Trim();
                                if (html == string.Empty)
                                {
                                    tagNode.Remove();
                                }
                            }
                        }
                    }
                }

                return myNode;
            }
            catch (Exception ex) { Logging.Log("_ClearHtmlTag ERROR", ex.Message); return null; }
        }

        public static string _ClearContent(string content)
        {
            try
            {
                //clear hyperlink            
                content = Regex.Replace(content, @"<a[^>]*>([^<]+)<\/a>", "$1");

                //replace newline + tab
                content = content.Replace("\n", "").Replace("\r", "").Replace("\t", "&nbsp;");

                return content.Trim();
            }
            catch (Exception ex) { Logging.Log("_ClearContent ERROR", ex.Message); return null; }
        }

        public static string _RemoveHTMLComments(string input)
        {
            try
            {
                string output = string.Empty;
                string[] temp = Regex.Split(input, "<!--");
                foreach (string s in temp)
                {
                    var str = !s.Contains("-->") ? s : s.Substring(s.IndexOf("-->", StringComparison.Ordinal) + 3);
                    if (str.Trim() != string.Empty)
                    {
                        output = output + str.Trim();
                    }
                }
                return output;
            }
            catch (Exception ex) { Logging.Log("_RemoveHTMLComments ERROR", ex.Message); return null; }
        }


        public static string _GetNodeInnerText(HtmlDocument doc, string xpath1, string xpath2 = "")
        {
            try
            {
                var node = _GetNode(doc, xpath1, xpath2);
                if (node != null)
                {
                    return System.Web.HttpUtility.HtmlDecode(node.InnerText)?.Trim();
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                Logging?.Log("_GetNodeInnerText ERROR", ex.Message);
                return string.Empty;
            }
        }
        public static string _GetNodeInnerHtml(HtmlDocument doc, string xpath1, string xpath2 = "")
        {
            try
            {
                var node = _GetNode(doc, xpath1, xpath2);
                if (node != null)
                {
                    return System.Web.HttpUtility.HtmlDecode(node.InnerHtml.Trim());
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                Logging?.Log("_GetNodeInnerHtml ERROR", ex.Message);
                return string.Empty;
            }
        }
        public static string _GetNodeAttributeValue(HtmlDocument doc, string attributeName, string xpath1, string xpath2 = "")
        {
            try
            {
                var node1 = doc.DocumentNode.SelectSingleNode(xpath1);
                if (string.IsNullOrEmpty(xpath2))
                {
                    if (node1?.Attributes[attributeName] != null)
                        return System.Web.HttpUtility.HtmlDecode(node1.Attributes[attributeName].Value)?.Trim();
                    else
                        return string.Empty;
                }
                else if (node1 != null)
                {
                    var node2 = node1.SelectSingleNode(xpath2);
                    if (node2?.Attributes[attributeName] != null)
                        return System.Web.HttpUtility.HtmlDecode(node2.Attributes[attributeName].Value)?.Trim();
                    else
                        return string.Empty;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                Logging?.Log("_GetNodeAttributeValue ERROR", ex.Message);
                return string.Empty;
            }
        }

        public static void ProcessImage(ref string thumbnail, ref string newcontent)
        {
            try
            {
                if (!string.IsNullOrEmpty(thumbnail))
                {
                    thumbnail = DownloadImage(thumbnail);
                }
                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(newcontent);

                var imgs = htmlDocument.DocumentNode.SelectNodes("//img");
                if (imgs != null)
                {
                    foreach (HtmlNode img in imgs)
                    {
                        if (string.IsNullOrEmpty(img.Attributes["src"]?.Value))
                            continue;
                        string src = img.Attributes["src"].Value;
                        //save image
                        string wwwUrl = DownloadImage(src);

                        if (string.IsNullOrEmpty(thumbnail))
                        {
                            thumbnail = wwwUrl;
                        }

                        //replace content
                        newcontent = newcontent.Replace(src, wwwUrl);
                    }
                }
            }
            catch (Exception ex) { Logging.Log("ProcessImage ERROR", ex.Message); }
        }

        public static void ProcessContent(ref string newcontent)
        {
            try
            {
                var matches = Regex.Matches(newcontent, "<a.+?href=[\"'](.+?)[\"'](.*?)>(.*?)<\\/a>", RegexOptions.IgnoreCase);
                if (matches.Count > 0)
                {
                    foreach (Match match in matches)
                    {
                        if (match.Success)
                        {
                            string a = match.Groups[0].Value;
                            string t = match.Groups[3].Value;
                            //replace content
                            newcontent = newcontent.Replace(a, t);
                        }
                    }
                }
            }
            catch (Exception ex) { Logging.Log("ProcessContent ERROR", ex.Message); }
        }

        public static string DownloadImage(string url)
        {
            int retries = 3;
            bool downOk = false;
            while (true)
            {
                try
                {
                    //if (url.Contains("?"))
                    //{
                    //    url = url.Substring(0, url.IndexOf("?"));
                    //}
                    url = System.Web.HttpUtility.HtmlDecode(url);

                    string pattern = @"([a-zA-Z0-9_-]+)\.(jpg|png|gif)";
                    string fileName;
                    if (Regex.IsMatch(url ?? throw new ArgumentNullException(nameof(url)), pattern))
                    {
                        fileName = Regex.Match(url, pattern).Value;
                    }
                    else
                    {
                        fileName = Guid.NewGuid().ToString().ToLower() + ".jpg";
                    }

                    string fileExtension = Path.GetExtension(fileName);
                    string folderPath = DateTime.Now.ToString(@"yyyy/MM/dd");
                    if (!Directory.Exists(Path.Combine(Config.FtpFolder, folderPath)))
                    {
                        Directory.CreateDirectory(Path.Combine(Config.FtpFolder, folderPath));
                    }

                    //edit filename
                    fileName = fileName.Replace(" ", "-");
                    fileName = VietCMS.Framework.Core.Common.WebControl.ToFriendlyString(fileName.ToLower(), ".");

                    //save file
                    int count = 1;
                    string filePath = Path.Combine(folderPath, fileName);
                    string fileNameOnly = Path.GetFileNameWithoutExtension(filePath);
                    while (File.Exists(Path.Combine(Config.FtpFolder, filePath)))
                    {
                        string tempFileName = $"{fileNameOnly}({count++})";
                        fileName = tempFileName + fileExtension;
                        filePath = Path.Combine(folderPath, fileName);
                    }
                    WebClient wc = new WebClient();
                    wc.Headers.Add("user-agent",
                        "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                    Stream s = new MemoryStream(wc.DownloadData(url));
                    Bitmap bmp = new Bitmap(s);
                    bmp.Save(Path.Combine(Config.FtpFolder, filePath));
                    bmp.Dispose();
                    wc.Dispose();
                    url = Config.FtpHost + "/" + filePath;
                    downOk = true;
                    break;
                }
                catch (WebException e)
                {
                    if (e.Status == WebExceptionStatus.ProtocolError)
                    {
                        Logging?.Log("DownloadImage ERROR", "WebExceptionStatus.ProtocolError --- GET IMAGE FROM --- " + url);
                        return string.Empty;
                    }
                    if (e.Status == WebExceptionStatus.ConnectionClosed)
                    {
                        Logging?.Log("DownloadImage ERROR", "WebExceptionStatus.ConnectionClosed --- GET IMAGE FROM --- " + url);
                        return string.Empty;
                    }
                    if (((HttpWebResponse)e.Response).StatusCode == HttpStatusCode.NotFound)
                    {
                        Logging?.Log("DownloadImage ERROR", "HttpStatusCode.NotFound --- GET IMAGE FROM --- " + url);
                        return string.Empty;
                    }
                }
                catch (Exception ex)
                {
                    if (Logging != null)
                    {
                        Logging.Log("DownloadImage ERROR", ex.Message);
                        return string.Empty;
                    }
                    if (--retries == 0) break;
                    else System.Threading.Thread.Sleep(1000);
                }
            }
            if (downOk)
            {
                return url.Replace("\\", "/");
            }
            else
            {
                return string.Empty;
            }

        }

        public static void GetFirstLargeImage(ref string thumbnail, string content)
        {
            try
            {
                var p1 = "<meta.+?property=\"og:image:url\" content=[\"'](.+?)[\"'].*?>";
                var p2 = "<meta.+?property=\"og:image\" content=[\"'](.+?)[\"'].*?>";
                var matches = Regex.Matches(content, p1, RegexOptions.IgnoreCase);
                if (matches.Count > 0)
                {
                    //save image                
                    thumbnail = DownloadImage(matches[0].Groups[1].Value);
                }
                else
                {
                    matches = Regex.Matches(content, p2, RegexOptions.IgnoreCase);
                    if (matches.Count > 0)
                    {
                        //save image                
                        thumbnail = DownloadImage(matches[0].Groups[1].Value);
                    }
                }
            }
            catch (Exception ex) { Logging.Log("GetFirstLargeImage ERROR", ex.Message); }
        }

    }
}
