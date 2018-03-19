using ICT_SpiderNews.Lib;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using BaoKhoe;
using BaoKhoe.Models;
using SpiderNews.Lib;

namespace ICT_SpiderNews
{
    class Program
    {

        static Spider _spider;
        static Logging _logging;
        private static AppDBContext _appDbContext;

        private static readonly List<Article> AddedArticles = new List<Article>();

        static void Main()
        {
            _appDbContext = new AppDBContext();
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            _logging = new Logging(_appDbContext);

            _logging.Log("Start Spider", "Start Spider");

            Utilities.Logging = _logging;

            //init spider
            _spider = new Spider(_appDbContext, _logging);

            ///////////////////////////////////////////////////////////////////////////////////////////
            GetArticles("http://suckhoedoisong.vn/thoi-su-c6/", "Tin Tuc");
            GetArticles("http://suckhoedoisong.vn/dinh-duong-c38/", "Dinh Duong");
            GetArticles("http://suckhoedoisong.vn/nam-hoc-c49/", "Gioi Tinh");
            GetArticles("http://suckhoedoisong.vn/khoe-dep--c13/", "Lam Dep");
            GetArticles("http://suckhoedoisong.vn/y-hoc-co-truyen-c9/", "Y Hoc Co Truyen");
            GetArticles("http://suckhoedoisong.vn/goc-chuyen-gia-c71/", "Goc Chuyen Gia");
            GetArticles("http://suckhoedoisong.vn/benh-lay-truyen--c47/", "Phong Va Chua Benh");
            GetArticles("http://suckhoedoisong.vn/phong-the-c50/", "Chuyen Phong The");
            GetArticles("http://suckhoedoisong.vn/day-tre-c37/", "Day Tre");
            GetArticles("http://suckhoedoisong.vn/phong-benh-c39/", "Phong Va Chua Benh");
            GetArticles("http://suckhoedoisong.vn/san-phu-khoa-c40/", "San Phu Khoa");

            //GetArticles("http://soha.vn/song-khoe/phong-va-chua-benh.htm", "Phong Va Chua Benh");
            //GetArticles("http://soha.vn/song-khoe/bai-thuoc-quy.htm", "Bai Thuoc Dan Gian");
            //GetArticles("http://soha.vn/song-khoe/an-toan-thuc-pham.htm", "An Toan Thuc Pham");
            //GetArticles("http://soha.vn/song-khoe/ung-thu.htm", "Benh Thuong Gap");
            //GetArticles("http://soha.vn/song-khoe.htm", "Tin Tuc");

            GetArticles("http://ttvn.vn/gia-dinh/suc-khoe.htm", "Tin Tuc");
            GetArticles("http://ttvn.vn/gia-dinh/nuoi-day-con.htm", "Day Tre");

            GetArticles("http://thegioitre.vn/tin-tuc/suc-khoe", "Tin Tuc");

            //GetArticles("http://infonet.vn/suc-khoe/14.info", "Tin Tuc");

            GetArticles("http://bestie.vn/chuyen-muc/dep", "Lam Dep");
            GetArticles("http://bestie.vn/chuyen-muc/khoe", "Tin Tuc");

            //GetArticles("http://news.zing.vn/khoe-dep.html", "Lam Dep");
            //GetArticles("http://news.zing.vn/dinh-duong.html", "Dinh Duong");
            //GetArticles("http://news.zing.vn/me-va-be.html", "Me Va Be");
            //GetArticles("http://news.zing.vn/benh-thuong-gap.html", "Benh Thuong Gap");

            GetArticles("http://www.suckhoegiadinh.com.vn/khoe-+/", "Tin Tuc");
            GetArticles("http://www.suckhoegiadinh.com.vn/thuoc-va-suc-khoe/", "Phong Va Chua Benh");
            GetArticles("http://www.suckhoegiadinh.com.vn/dinh-duong/", "Dinh Duong");
            GetArticles("http://www.suckhoegiadinh.com.vn/lam-dep/", "Lam Dep");
            GetArticles("http://www.suckhoegiadinh.com.vn/gia-dinh-khoe/", "Song Khoe Moi Ngay");
            GetArticles("http://www.suckhoegiadinh.com.vn/cho-con/", "Me Va Be");
            GetArticles("http://www.suckhoegiadinh.com.vn/giam-beo/", "Lam Dep");

            GetArticles("https://www.thuocbietduoc.com.vn/tin-tucsk-9-0/nghien-cuu-y-duoc.aspx", "Tien Bo Y Hoc");
            GetArticles("https://www.thuocbietduoc.com.vn/home/Newslist.aspx?grid=1", "Tin Tuc");
            GetArticles("https://www.thuocbietduoc.com.vn/tin-tucsk-51-0/duong-sinh.aspx", "Song Khoe Moi Ngay");

            GetArticles("http://giadinhcuocsong.net/suc-khoe", "Tin Tuc");
            GetArticles("http://giadinhcuocsong.net/lam-dep", "Lam Dep");
            GetArticles("http://giadinhcuocsong.net/me-va-be", "Me Va Be");
            GetArticles("http://giadinhcuocsong.net/chua-benh", "Bai Thuoc Dan Gian");

            GetArticles("http://benh.vn/tin-tuc/5.htm", "Tin Tuc");
            GetArticles("http://benh.vn/hoinghi/178.htm", "Tin Tuc");
            GetArticles("http://benh.vn/y-hoc/93.htm", "Tien Bo Y Hoc");
            GetArticles("http://benh.vn/thiet-bi-cong-nghe-moi/13.htm", "Tien Bo Y Hoc");
            GetArticles("http://benh.vn/benh/3.htm", "Phong Va Chua Benh");
            GetArticles("http://benh.vn/truyen-nhiem/35.htm", "Benh Lay Nhiem");
            GetArticles("http://benh.vn/tre-em/7.htm", "Me Va Be");
            GetArticles("http://benh.vn/kienthuc-ba-me-tre/128.htm", "Me Va Be");
            GetArticles("http://benh.vn/cham-soc-thai-nhi/42.htm", "Me Va Be");
            GetArticles("http://benh.vn/benh-ba-bau/43.htm", "Me Va Be");
            GetArticles("http://benh.vn/dinh-duong-sinhhoat/41.htm", "Me Va Be");
            GetArticles("http://benh.vn/day-tre/162.htm", "Day Tre");
            GetArticles("http://benh.vn/ba-bau/8.htm", "San Phu Khoa");
            GetArticles("http://benh.vn/cham-soc-sacdep/49.htm", "Lam Dep");
            GetArticles("http://benh.vn/rieng-cua-phunu/129.htm", "Tam Su Tham Kin");
            GetArticles("http://benh.vn/noi-tiet-to/48.htm", "Tam Su Tham Kin");
            GetArticles("http://benh.vn/rieng-cua-dan-ong/46.htm", "Tam Su Tham Kin");
            GetArticles("http://benh.vn/benh-cua-dan-ong/44.htm", "Gioi Tinh");
            GetArticles("http://benh.vn/benh-phunu/47.htm", "Gioi Tinh");
            GetArticles("http://benh.vn/song-khoe-manh/12.htm", "Song Khoe Moi Ngay");
            GetArticles("http://benh.vn/duong-sinh/55.htm", "Song Khoe Moi Ngay");
            GetArticles("http://benh.vn/che-do-an/57.htm", "Dinh Duong");
            GetArticles("http://benh.vn/dong-y/61.htm", "Y Hoc Co Truyen");

            GetArticles("http://khoeplus.vn/thoi-su", "Tin Tuc");
            GetArticles("http://khoeplus.vn/lam-dep", "Lam Dep");
            GetArticles("http://khoeplus.vn/nam-gioi-phu-nu", "Gioi Tinh");
            GetArticles("http://khoeplus.vn/tam-su", "Tam Su Tham Kin");
            GetArticles("http://khoeplus.vn/nuoi-con", "Day Tre");

            GetArticles("http://phunuonline.com.vn/suc-khoe/", "Tin Tuc");
            GetArticles("http://phunuonline.com.vn/suc-khoe/song-khoe/", "Song Khoe Moi Ngay");
            GetArticles("http://phunuonline.com.vn/suc-khoe/alo-bac-si/", "Goc Chuyen Gia");
            GetArticles("http://phunuonline.com.vn/suc-khoe/goc-dong-y/", "Y Hoc Co Truyen");

            GetArticles("http://www.yeutretho.vn/me-va-be/", "Me Va Be");
            GetArticles("http://www.yeutretho.vn/mang-thai/", "San Phu Khoa");
            GetArticles("http://www.yeutretho.vn/sau-khi-sinh/", "San Phu Khoa");
            GetArticles("http://www.yeutretho.vn/con-dang-lon/", "Me Va Be");
            GetArticles("http://www.yeutretho.vn/me-viet-day-con/", "Day Tre");
            GetArticles("http://www.yeutretho.vn/ky-nang-song/", "Day Tre");
            GetArticles("http://www.yeutretho.vn/sach-va-lanh/", "Song Khoe Moi Ngay");
            GetArticles("http://www.yeutretho.vn/tam-su-eva/", "Tam Su Tham Kin");
            GetArticles("http://www.yeutretho.vn/dinh-duong/", "Dinh Duong");
            GetArticles("http://www.yeutretho.vn/song-khoe/", "Song Khoe Moi Ngay");
            GetArticles("http://www.yeutretho.vn/bai-thuoc-hay/", "Bai Thuoc Dan Gian");
            GetArticles("http://www.yeutretho.vn/benh-theo-mua/", "Phong Va Chua Benh");

            ///////////////////////////////////////////////////////////////////////////////////////////

            // assigned
            foreach (Article t in AddedArticles)
            {
                if (t.Title.Length % 7 == 0)
                {
                    _appDbContext.AssignedArticles.Add(new AssignedArticle()
                    {
                        Type = "hot",
                        Article = t,
                        FromDate = DateTime.Now,
                        ToDate = DateTime.Now.AddDays(1)
                    });
                }
                if (t.Title.Length % 9 == 0)
                {
                    _appDbContext.AssignedArticles.Add(new AssignedArticle()
                    {
                        Type = "special",
                        Article = t,
                        FromDate = DateTime.Now,
                        ToDate = DateTime.Now.AddDays(1)
                    });
                }
            }

            _appDbContext.SaveChanges();

            //done
            _logging.Log("spider", "End");
            //Console.ReadKey();
        }

        static void GetArticles(string channelUrl, string category)
        {
            var lstArticles = _spider.GetArticles(channelUrl);
            lstArticles.ForEach(item =>
            {
                InsertArticle(_appDbContext.Users.FirstOrDefault(x => x.Username.Equals("spider"))?.Username, item, category);
            });
        }

        static void InsertArticle(string username, RemoteArticleInfo article, string category)
        {
            if (article.url.Contains("http://bestie.vn/") || article.url.Contains("http://thegioitre.vn/"))
            {
                article.content = System.Net.WebUtility.HtmlDecode(article.content);
                article.keyword = string.Empty;
                article.author = article.author.Split('-')[0].Trim();
            }

            if (article.author.Length > 64)
            {
                article.author = string.Empty;
            }

            if (string.IsNullOrEmpty(article.keyword))
            {
                article.keyword = "";
            }
            if (article.keyword.Length > 255)
            {
                string tmp = "";
                for (int i = 0; i < article.keyword.Split(',').Length; i++)
                {
                    if ((tmp + article.keyword.Split(',')[i]).Length < 255)
                    {
                        tmp += article.keyword.Split(',')[i];
                    }
                }
                article.keyword = tmp;
            }

            if (string.IsNullOrEmpty(article.lead_image_url)
                || string.IsNullOrEmpty(article.content)
                || string.IsNullOrEmpty(article.title)
                || string.IsNullOrWhiteSpace(article.title)
                || string.IsNullOrWhiteSpace(article.content)
            )
            {
                return;
            }

            if (article.excerpt.Length > 999)
            {
                article.excerpt = article.excerpt.Substring(0, 999);
            }

            //headline re-validate
            article.excerpt = RevalidateHeadline(article.excerpt);

            if (string.IsNullOrEmpty(article.keyword))
            {
                article.keyword = string.Empty;
            }

            article.date_published = article.date_published.Replace("  ", " ");
            article.date_published = DateTimeParse(article.date_published.Trim());

            //publish time
            article.date_published = DateTime.TryParse(article.date_published.Trim(), out var publishTime) ? publishTime.ToString(CultureInfo.InvariantCulture) : DateTime.Now.ToString(CultureInfo.InvariantCulture);

            //author
            if (!string.IsNullOrEmpty(article.author))
            {
                article.author = RemoveAtEnd(article.author);
            }

            string frendlyTitle = VietCMS.Framework.Core.Common.WebControl.ToFriendlyString(article.title);
            if (_appDbContext.Articles.Count(x => x.SourceUrl.Equals(article.url) 
                || x.FriendlyTitle.Equals(frendlyTitle)) == 0)
            {
                string tmp = "";
                if (string.IsNullOrEmpty(article.keyword))
                {
                    article.keyword = String.Empty;
                }
                string[] s = article.keyword.Split(',');
                List<Keyword> keywords = new List<Keyword>();

                for (int i = 0; i < s.Length; i++)
                {
                    string si = s[i];
                    if (si.Trim().Length > 40 || si.Trim().Length < 2)
                    {
                        continue;
                    }
                    tmp += VietCMS.Framework.Core.Common.WebControl.ToFriendlyString(si);
                    if (i != s.Length - 1)
                    {
                        tmp += ",";
                    }
                    
                    if (_appDbContext.Keywords.Count(x => x.Title.Equals(si.Trim())) == 0)
                    {
                        keywords.Add(_appDbContext.Keywords.Add(new Keyword()
                        {
                            Title = s[i],
                            CreatedAt = DateTime.Now,
                            FriendlyTitle = VietCMS.Framework.Core.Common.WebControl.ToFriendlyString(si),
                            Type = "Keyword",
                            VisitCount = 0
                        }));
                    }
                    else
                    {
                        keywords.Add(_appDbContext.Keywords.FirstOrDefault(x => x.Title.Equals(si.Trim())));
                    }
                }

                _appDbContext.SaveChanges();

                List<RelatedArticle> relatedArticles = new List<RelatedArticle> ();
                foreach (Keyword k in keywords)
                {
                    List<Article> articles = _appDbContext.ArticleKeywords
                        .Where(y => y.Keyword.Id == k.Id)
                        .Take(20)
                        .OrderByDescending(x => x.Article.CreatedAt)
                        .Select(x => x.Article)
                        .ToList();

                    foreach (Article item in articles)
                    {
                        bool check = false;
                        foreach (RelatedArticle ra in relatedArticles)
                        {
                            if (item.Id == ra.Origin.Id)
                            {
                                ra.Index++;
                                check = true;
                            }
                        }
                        if (!check)
                        {
                            RelatedArticle relatedArticle = new RelatedArticle()
                            {
                                Origin = item,
                                Index = 1,
                                CreatedAt = DateTime.Now,
                                Type = "Related"
                            };
                            relatedArticles.Add(relatedArticle);
                        }
                    }
                }

                Article a = new Article()
                {
                    Title = article.title.Trim(),
                    Thumbnail = article.lead_image_url,
                    Category = _appDbContext.Categories.FirstOrDefault(x => x.FriendlyName.Equals(category)),
                    Headlines = article.excerpt.Trim(),
                    Content = article.content,
                    Keywords = article.keyword.Trim(),
                    FriendlyKeywords = tmp,
                    Source = article.domain,
                    SourceUrl = article.url,
                    FriendlyTitle = VietCMS.Framework.Core.Common.WebControl.ToFriendlyString(article.title.Trim()),
                    AuthorAlias = article.author.Trim(),
                    Status = Const.ArticleStatusActive,
                    CreatedAt = DateTime.Now,
                    CreatedBy = _appDbContext.Users.FirstOrDefault(x => x.Username.Equals(username)),
                    LastModifiedBy = _appDbContext.Users.FirstOrDefault(x => x.Username.Equals(username)),
                    LastModifiedAt = DateTime.Now,
                    ViewCount = 0,
                    SubTitle = article.title.Trim().Length > 50 ? article.title.Trim().Substring(0, 47) + "..." : article.title.Trim()
                };

                foreach (Keyword keyword in keywords)
                {
                    _appDbContext.ArticleKeywords.Add(new ArticleKeyword()
                    {
                        Article = a,
                        Keyword = keyword
                    });
                }

                for (int i = 0; i < relatedArticles.Count; i++)
                {
                    var r = relatedArticles[i];
                    r.Related = a;
                    _appDbContext.RelatedArticles.Add(r);
                    _appDbContext.RelatedArticles.Add(new RelatedArticle()
                    {
                        CreatedAt = DateTime.Now,
                        Index = r.Index,
                        Origin = a,
                        Related = r.Origin,
                        Type = "Related"
                    });
                }

                _appDbContext.Articles.Add(a);
                
                AddedArticles.Add(a);
                _appDbContext.SaveChanges();
            }
        }

        static string DateTimeParse(string input)
        {
            try
            {
                if (input.Equals(""))
                {
                    return "";
                }
                string result;
                string patternSKDS = @"\bNGÀY \w+ THÁNG \w+, \w+ [|] \w+:\w+\b";
                string patternSingle1 = @"\b\d+[\/]\d+[\/]\d+\b";
                string patternSingle3 = @"\b\d+-\d+-\d+\b";
                string patternSingle5 = @"\b\d+[.]\d+[.]\d+\b";
                string patternSingle2 = @"\b\d+:\d+\b";
                string patternSingle4 = @"\b\d+:\d+:\d+\b";
                string patternVNN = @"\b\d+[\/]\d+[\/]\d+.+\d+:\d+ GMT[+]\d+\b";
                string patternTTO = @"\b\d+[\/]\d+[\/]\d+.+\d+:\d+ GMT[+]\d+\b";
                string patternALO = @"\b\w+[,]\s\d+[\/]\d+[\/]\d+\s\d+:\d+\b";
                string patternGDTD = @"\b\w+ \w+[,]\s\d+[\/]\d+[\/]\d+\s\d+:\d+\b";
                string patternALO_T4 = @"\bThứ tư[,]\s\d+[\/]\d+[\/]\d+\s\d+:\d+\b";
                string patternNLD = @"\b\d+[\/]\d+[\/]\d+ \d+:\d+\b";
                string patternTN = @"\b\d+:\d+ \w+ [-] \d+[\/]\d+[\/]\d+\b";
                string patternAF = @"\b\d+-\d+-\d+\b";
                string patternSOHA = @"\b\d+[\/]\d+[\/]\d+.+\d+:\d+\s\w+\b";
                string patternPM = @"PM";
                string patternAM = @"AM";
                string patternBestie = @"\b\d+.\d+.\d+\b";
                string patternK14 = @"\b\d+:\d+\s\d+\d+[\/]\d+[\/]\d+\b";
                string patternBQT = @"\b\d+:\d+ [|] \d+[\/]\d+[\/]\d+\b";
                string patternCongLy = @"\b\d+[\/]\d+[\/]\d+ \d+:\d+ UTC[+]\d+\b";
                string patternATGT = @"\b\d+[\/]\d+[\/]\d+ - \d+:\d+\s[\(]GMT[\+]\d+\b";
                string patternIONE_AM = @"\b\d+:\d+ AM [\|] \d+[\/]\d+[\/]\d+\b";
                string patternIONE_PM = @"\b\d+:\d+ PM [\|] \d+[\/]\d+[\/]\d+\b";
                string patternConnguoi_laodong = @"\b\d+:\d+ ngày \d+[\/]\d+[\/]\d+\b";
                string patternKTVN = @"\b\d+[\/]\d+[\/]\d+\b";
                string patternKinhtevadubao = @"\b\d+[\/]\d+[\/]\d+ - \d+:\d+:\d+\b";
                string patternGiadinh_netAM = @"\bNgày \d+ Tháng \d+, \d+ [|] \d+:\d+ AM\b";
                string patternGiadinh_netPM = @"\bNgày \d+ Tháng \d+, \d+ [|] \d+:\d+ PM\b";
                string patternKTNT = @"\bngày \d+ tháng \d+ năm \d+ [|] \d+:\d+\b";
                string patternVietTime = @"\b[A-Za-zÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚÝàáâãèéêìíòóôõùúýĂăĐđĨĩŨũủƠơƯứưẠ-ỹ]+ [A-Za-zÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚÝàáảâậãèéêìíòóôõùúýĂăĐđĨĩŨũƠơƯưẠ-ỹ]+, [A-Za-zÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚÝàáâãèéêìíòóôõùúýĂăĐđĨĩŨũƠơƯưẠ-ỹ]+ \d+[\/]\d+[\/]\d+ - \d+:\d+\b";
                string patternVneconomy = @"\b\d+:\d+ - [A-Za-zÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚÝàáâãèéêìíòóôõùúýĂăĐđĨĩŨũủƠơƯứưẠ-ỹ]+ [A-Za-zÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚÝàáâãèéêìíòóôõùúýĂăĐđĨĩŨũủƠơƯứưẠ-ỹ]+[,] \d+[\/]\d+[\/]\d+\b";

                Match m = Regex.Match(input, patternSKDS);
                if (m.Value.Length == input.Length)
                {
                    result = input.Substring(14, 2)
                             + "/" + input.Substring(5, 2)
                             + "/" + input.Substring(18, 4)
                             + " " + input.Substring(25, 5);
                    return result;
                }

                // solo
                Match mPm = Regex.Match(input, patternPM);
                Match mAm = Regex.Match(input, patternAM);
                Match m1 = Regex.Match(input, patternSingle1);
                Match m4 = Regex.Match(input, patternSingle4);
                if ((mPm.Value.Length == 2 || mAm.Value.Length == 2) && m1.Length >= 8 && m4.Value.Length >= 5)
                {
                    var xxx = mAm.Length > 0 ? "AM" : "PM";
                    if ((xxx + m1.Value + m4.Value).Length + 3 == input.Trim().Length)
                    {
                        result = m1.Value.Trim()
                                 + " " + m4.Value.Trim()
                                 + " " + xxx;
                        return result;
                    }
                }

                // solo
                Match m5 = Regex.Match(input, patternSingle5);
                Match m2 = Regex.Match(input, patternSingle2);
                if (m5.Length >= 8 && m2.Value.Length >= 3)
                {
                    result = m5.Value.Split('.')[1]
                             + "/" + m5.Value.Split('.')[0]
                             + "/" + m5.Value.Split('.')[2]
                             + " " + m2.Value.Trim();
                    return result;
                }

                // solo
                m1 = Regex.Match(input, patternSingle1);
                m2 = Regex.Match(input, patternSingle2);
                if (m1.Value.Length >= 8 && m2.Length >= 3)
                {
                    result = m1.Value.Split('/')[1]
                             + "/" + m1.Value.Split('/')[0]
                             + "/" + m1.Value.Split('/')[2]
                             + " " + m2.Value.Trim();
                    return result;
                }

                // solo
                m4 = Regex.Match(input, patternSingle4);
                m1 = Regex.Match(input, patternSingle1);
                if (m1.Value.Length >= 8 && m4.Length >= 5)
                {
                    result = m1.Value.Split('/')[1]
                             + "/" + m1.Value.Split('/')[0]
                             + "/" + m1.Value.Split('/')[2]
                             + " " + m4.Value.Trim();
                    return result;
                }

                // solo
                Match m3 = Regex.Match(input, patternSingle3);
                m2 = Regex.Match(input, patternSingle2);
                if (m3.Value.Length >= 8 && m2.Length >= 3)
                {
                    result = m3.Value.Split('-')[1]
                             + "/" + m3.Value.Split('-')[0]
                             + "/" + m3.Value.Split('-')[2]
                             + " " + m2.Value.Trim();
                    return result;
                }

                // Giadinh_netAM 
                m = Regex.Match(input, patternGiadinh_netAM);
                if (m.Value.Length == input.Trim().Length)
                {
                    var xxx = m.Value.Split(',');
                    result = xxx[0].Trim().Split(' ')[3]
                        + "/" + xxx[0].Trim().Split(' ')[1]
                        + "/" + xxx[1].Trim().Split(' ')[0]
                        + " " + xxx[1].Trim().Split(' ')[2]
                        + " AM";
                    return result;
                }
                // Giadinh_netPM
                m = Regex.Match(input, patternGiadinh_netPM);
                if (m.Value.Length == input.Trim().Length)
                {
                    var xxx = m.Value.Split(',');
                    result = xxx[0].Trim().Split(' ')[3]
                        + "/" + xxx[0].Trim().Split(' ')[1]
                        + "/" + xxx[1].Trim().Split(' ')[0]
                        + " " + xxx[1].Trim().Split(' ')[2]
                        + " PM";
                    return result;
                }

                // kinhtevadubao 
                m = Regex.Match(input, patternKinhtevadubao);
                if (m.Value.Length > 20)
                {
                    var xxx = m.Value.Split('-');
                    result = xxx[0].Trim().Split('/')[1]
                             + "/" + xxx[0].Trim().Split('/')[0]
                             + "/" + xxx[0].Trim().Split('/')[2]
                             + " " + xxx[1].Trim();
                    return result;
                }

                // KTNT 
                m = Regex.Match(input, patternKTNT);
                if (m.Value.Length > 20)
                {
                    var xxx = m.Value.Split(' ');
                    result = xxx[3]
                             + "/" + xxx[1]
                             + "/" + xxx[5]
                             + " " + xxx[7];
                    return result;
                }

                // Connguoi_laodong 
                m = Regex.Match(input, patternConnguoi_laodong);
                if (m.Value.Length > 20)
                {
                    var xxx = m.Value.Split(new[] { "ngày" }, StringSplitOptions.None);
                    result = xxx[1].Trim().Split('/')[1]
                             + "/" + xxx[1].Trim().Split('/')[0]
                             + "/" + xxx[1].Trim().Split('/')[2]
                             + " " + xxx[0];
                    return result;
                }

                // Vneconomy
                m = Regex.Match(input, patternVneconomy);
                if (m.Value.Length == input.Trim().Length)
                {
                    var xxx = m.Value.Split(',');
                    result = xxx[1].Trim().Split('/')[1]
                        + "/" + xxx[1].Trim().Split('/')[0]
                        + "/" + xxx[1].Trim().Split('/')[2]
                        + " " + xxx[0].Trim().Split('-')[0].Trim();
                    return result;
                }

                // VietTime
                m = Regex.Match(input, patternVietTime);
                if (m.Value.Length == input.Trim().Length)
                {
                    var xxx = m.Value.Split(new[] { "ngày" }, StringSplitOptions.None);
                    result = xxx[1].Trim().Split('-')[0].Trim().Split('/')[1]
                        + "/" + xxx[1].Trim().Split('-')[0].Trim().Split('/')[0]
                        + "/" + xxx[1].Trim().Split('-')[0].Trim().Split('/')[2]
                        + " " + xxx[1].Trim().Split('-')[1];
                    return result;
                }

                // KTVN
                m = Regex.Match(input, patternKTVN);
                if (m.Value.Length == input.Trim().Length)
                {
                    result = m.Value.Trim().Split('/')[1]
                        + "/" + m.Value.Trim().Split('/')[0]
                        + "/" + m.Value.Trim().Split('/')[2];
                    return result;
                }

                // IONE AM
                m = Regex.Match(input, patternIONE_AM);
                if (m.Value.Length == input.Trim().Length)
                {
                    var xxx = m.Value.Split('|');
                    result = xxx[1].Trim().Split('/')[1]
                        + "/" + xxx[1].Trim().Split('/')[0]
                        + "/" + xxx[1].Trim().Split('/')[2]
                        + " " + xxx[0].Trim().Split(' ')[0] + " am";
                    return result;
                }

                // IONE PM
                m = Regex.Match(input, patternIONE_PM);
                if (m.Value.Length == input.Trim().Length)
                {
                    var xxx = m.Value.Split('|');
                    result = xxx[1].Trim().Split('/')[1]
                        + "/" + xxx[1].Trim().Split('/')[0]
                        + "/" + xxx[1].Trim().Split('/')[2]
                        + " " + xxx[0].Trim().Split(' ')[0] + " pm";
                    return result;
                }

                // atgt
                m = Regex.Match(input, patternATGT);
                if (m.Value.Length + 1 == input.Trim().Length)
                {
                    var xxx = m.Value.Split('-');
                    result = xxx[0].Trim().Split('/')[1]
                        + "/" + xxx[0].Trim().Split('/')[0]
                        + "/" + xxx[0].Trim().Split('/')[2]
                        + " " + xxx[1].Trim().Split(' ')[0];
                    return result;
                }

                // baoquocte_vn
                m = Regex.Match(input, patternBQT);
                if (m.Value.Length == input.Trim().Length)
                {
                    int length = m.Value.Length;
                    result = m.Value.Substring(length - 7, 2)
                             + "/" + input.Substring(length - 10, 2)
                             + "/" + input.Substring(length - 4, 4)
                             + " " + input.Substring(0, 5);
                    return result;
                }

                // vietnamnet
                m = Regex.Match(input, patternVNN);
                if (m.Value.Length == input.Length)
                {
                    result = input.Substring(3, 2)
                             + "/" + input.Substring(0, 2)
                             + "/" + input.Substring(6, 4)
                             + " " + input.Substring(11, 5);
                    return result;
                }

                // congly
                m = Regex.Match(input, patternCongLy);
                if (m.Value.Length == input.Length)
                {
                    var xxx = m.Value.Split(' ');
                    result = xxx[0].Split('/')[1]
                        + "/" + xxx[0].Split('/')[0]
                        + "/" + xxx[0].Split('/')[2]
                        + " " + xxx[1];
                    return result;
                }

                // tuoitre
                m = Regex.Match(input, patternTTO);
                if (m.Value.Length == input.Length)
                {
                    result = input.Substring(3, 2)
                             + "/" + input.Substring(0, 2)
                             + "/" + input.Substring(6, 4)
                             + " " + input.Substring(12, 5);
                    return result;
                }

                // alobacsi
                m = Regex.Match(input, patternGDTD);
                if (m.Value.Length >= 20)
                {
                    var tmp = m.Value.Split(',')[1].Trim();
                    var time = tmp.Split(' ')[1];
                    var date = tmp.Split(' ')[0];
                    result = date.Split('/')[1]
                             + "/" + date.Split('/')[0]
                             + "/" + date.Split('/')[2]
                             + " " + time;
                    return result;
                }

                // alobacsi
                m = Regex.Match(input, patternALO);
                if (m.Value.Length >= 20)
                {
                    var tmp = m.Value.Substring(m.Value.Length - 16, 16).Trim();
                    result = tmp.Substring(3, 2)
                             + "/" + tmp.Substring(0, 2)
                             + "/" + tmp.Substring(6, 4)
                             + " " + tmp.Substring(11, tmp.Length - 11);
                    return result;
                }

                // alobacsi thứ tư
                m = Regex.Match(input, patternALO_T4);
                if (m.Value.Length >= 20)
                {
                    var tmp = m.Value.Substring(m.Value.Length - 16, 16).Trim();
                    result = tmp.Substring(3, 2)
                             + "/" + tmp.Substring(0, 2)
                             + "/" + tmp.Substring(6, 4)
                             + " " + tmp.Substring(11, tmp.Length - 11);
                    return result;
                }

                // ndl 
                m = Regex.Match(input, patternNLD);
                if (m.Value.Length == input.Length)
                {
                    result = input.Substring(3, 2)
                             + "/" + input.Substring(0, 2)
                             + "/" + input.Substring(6, 4)
                             + " " + input.Substring(11, input.Length - 11);
                    return result;
                }

                // thanhnien
                m = Regex.Match(input, patternTN);
                if (!string.IsNullOrEmpty(m.Value))
                {
                    var tmp = m.Value.Substring(m.Value.Length - 10, 10);
                    result = tmp.Substring(3, 2)
                             + "/" + tmp.Substring(0, 2)
                             + "/" + tmp.Substring(6, 4)
                             + " " + m.Value.Substring(0, m.Value.Length - 16);
                    return result;
                }

                // afamily
                m = Regex.Match(input, patternAF);
                if (m.Value.Length == input.Trim().Length)
                {
                    result = m.Value.Substring(3, 2)
                             + "/" + m.Value.Substring(0, 2)
                             + "/" + m.Value.Substring(6, 4);
                    return result;
                }

                // soha
                m = Regex.Match(input, patternSOHA);
                if (m.Value.Length == input.Trim().Length)
                {
                    result = m.Value.Substring(3, 2)
                             + "/" + m.Value.Substring(0, 2)
                             + "/" + m.Value.Substring(6, 4)
                             + " " + m.Value.Substring(11, m.Value.Length - 11);
                    return result;
                }

                // kenh14
                m = Regex.Match(input, patternK14);
                if (m.Value.Length == input.Trim().Length)
                {
                    result = m.Value.Substring(9, 2)
                             + "/" + m.Value.Substring(6, 2)
                             + "/" + m.Value.Substring(12, 4)
                             + " " + m.Value.Substring(0, m.Value.Length - 11);
                    return result;
                }

                // bestie.vn
                m = Regex.Match(input, patternBestie);
                if (m.Value.Length == input.Trim().Length)
                {
                    result = m.Value.Split('.')[1]
                             + "/" + m.Value.Split('.')[0]
                             + "/" + m.Value.Split('.')[2];
                    return result;
                }
            }
            // Suckhoe.vnex
            catch (Exception e)
            {
                // ignored
            }

            return input;
        }

        static string RemoveAtEnd(string input)
        {
            List<string> xList = new List<string>
            {
                " - ",
                " -",
                "- ",
            };

            foreach (var x in xList)
            {
                if (input.EndsWith(x) && input.Length >= x.Length)
                {
                    input = input.Substring(0, input.Length - x.Length);
                }
            }

            return input;
        }

        static string RevalidateHeadline(string input)
        {
            input = ReplaceAtStart(input, "TTO");
            input = ReplaceAtStart(input, "TT");
            input = ReplaceAtStart(input, "NLĐO");
            input = ReplaceAtStart(input, "SKĐS");
            input = ReplaceAtStart(input, "TĐO");
            input = ReplaceAtStart(input, "GiadinhNet");
            input = ReplaceAtStart(input, "NDĐT");
            input = ReplaceAtStart(input, "PLO");
            input = ReplaceAtStart(input, "BVPL");
            input = ReplaceAtStart(input, "Công lý");
            input = ReplaceAtStart(input, "Xây dựng");
            input = ReplaceAtStart(input, "KTNĐ");
            input = ReplaceAtStart(input, "VietTimes\n–");
            input = ReplaceAtStart(input, "VietTimes --");
            input = ReplaceAtStart(input, "VietTimes");
            input = ReplaceAtStart(input, "ĐTTCO");
            input = ReplaceAtStart(input, "KTNT");
            input = ReplaceAtStart(input, "Kinhtedothi");
            input = ReplaceAtStart(input, "TBKTSG Online");
            input = ReplaceAtStart(input, "TBKTSG");
            input = ReplaceAtStart(input, "TTĐ.VN");
            input = ReplaceAtStart(input, "TTĐ");
            input = ReplaceAtStart(input, "Phái đẹp - ELLE");
            input = ReplaceAtStart(input, "ELLE");
            input = ReplaceAtStart(input, "TGT");
            input = ReplaceAtStart(input, "Suckhoedoisong.vn");
            input = ReplaceAtStart(input, "suckhoedoisong.vn");
            return input;
        }

        static string ReplaceAtStart(string input, string removeString)
        {
            if (input.Length < removeString.Length)
            {
                return input;
            }
            string result = input;
            List<string> prepareStr = new List<string>
            {
                removeString + " - ",
                removeString + " – ",
                removeString + " -",
                removeString + " –",
                removeString + "– ",
                removeString + "- ",
                removeString + "-",
                removeString + "–",
                "(" + removeString + ") - ",
                "(" + removeString + ") - ",
                "(" + removeString + ") – ",
                "(" + removeString + ") -",
                "(" + removeString + ") –",
                "(" + removeString + ")- ",
                "(" + removeString + ")– ",
                "(" + removeString + ")-",
                "(" + removeString + ")–",
                "(" + removeString + ") ",
                "(" + removeString + ")",
                removeString,
                " - ",
                " – ",
                "- ",
                " -",
                "– ",
                " –",
                " ",
                "–",
                "-",
                "-",
                "*",
            };
            foreach (var str in prepareStr)
            {
                if (result.StartsWith(str))
                {
                    result = result.Substring(str.Length - 1, result.Length - str.Length).Trim();
                }
            }
            return result;
        }

    }
}
