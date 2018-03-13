using ICT_SpiderNews.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICT_SpiderNews
{
    class Program
    {

        static Spider spider = null;
        static SQLHelper sqlhelperBackend = null;
        static Logging logging = null;
        
        static void Main(string[] args)
        {
            //test();
            //return;

            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var logPath = System.Configuration.ConfigurationManager.AppSettings.Get("spiderLogPath");
            logging = new Logging(true, logPath, true);

            logging.Log("Start");

            Utilities.logging = logging;

            //init spider
            sqlhelperBackend = new SQLHelper(System.Configuration.ConfigurationManager.ConnectionStrings["BackendDB"].ConnectionString);            
            spider = new Spider(sqlhelperBackend, logging, true);

            //sync list
            GetArticles("http://suckhoedoisong.vn/thoi-su-c6/", 77);
            GetArticles("http://suckhoedoisong.vn/dinh-duong-c38/", 148);
            GetArticles("http://suckhoedoisong.vn/nam-hoc-c49/", 21);
            GetArticles("http://suckhoedoisong.vn/khoe-dep--c13/", 21);
            GetArticles("http://suckhoedoisong.vn/y-hoc-co-truyen-c9/", 148);
            GetArticles("http://suckhoedoisong.vn/goc-chuyen-gia-c71/", 77);
            GetArticles("http://suckhoedoisong.vn/benh-lay-truyen--c47/", 77);
            GetArticles("http://suckhoedoisong.vn/tam-su-c51/", 77);
            GetArticles("http://suckhoedoisong.vn/phong-the-c50/", 21);
            GetArticles("http://suckhoedoisong.vn/day-tre-c37/", 21);
            GetArticles("http://suckhoedoisong.vn/phong-benh-c39/", 77);
            GetArticles("http://suckhoedoisong.vn/san-phu-khoa-c40/", 21);

            GetArticles("http://soha.vn/song-khoe/phong-va-chua-benh.htm", 77);
            GetArticles("http://soha.vn/song-khoe/bai-thuoc-quy.htm", 148);
            GetArticles("http://soha.vn/song-khoe/an-toan-thuc-pham.htm", 148);
            GetArticles("http://soha.vn/song-khoe/ung-thu.htm", 77);
            GetArticles("http://soha.vn/song-khoe.htm", 77);

            GetArticles("http://ttvn.vn/gia-dinh/suc-khoe.htm", 77);
            GetArticles("http://ttvn.vn/gia-dinh/nuoi-day-con.htm", 21);

            GetArticles("http://thegioitre.vn/tin-tuc/suc-khoe", 77);

            GetArticles("http://infonet.vn/suc-khoe/14.info", 77);

            GetArticles("http://bestie.vn/chuyen-muc/dep", 21);
            GetArticles("http://bestie.vn/chuyen-muc/song", 155);
            GetArticles("http://bestie.vn/chuyen-muc/khoe", 77);

            GetArticles("http://news.zing.vn/khoe-dep.html", 21);
            GetArticles("http://news.zing.vn/dinh-duong.html", 148);
            GetArticles("http://news.zing.vn/me-va-be.html", 21);
            GetArticles("http://news.zing.vn/benh-thuong-gap.html", 77);
            

            //close connection
            sqlhelperBackend.Dispose();

            //done
            logging.Log("End");
            logging.Log("Done All!");

            //Console.ReadKey();

        }

        static void GetArticles(string channelUrl, int channelId)
        {
            var lstArticles = spider.GetArticles(channelUrl, channelId);

            foreach (var article in lstArticles)
            {
                if (article.excerpt.Length > 999)
                {
                    article.excerpt = article.excerpt.Substring(0, 999);
                }
                var insertedID = sqlhelperBackend.AddToGetInsertID("[Article]", new Dictionary<string, string> {
                                    {"ChannelId", channelId.ToString()},
                                    {"Title", article.title},
                                    {"Thumbnail", article.lead_image_url},
                                    {"Headlines", article.excerpt},
                                    {"Content", article.content},
                                    {"Keyword", ""},
                                    {"Source", article.domain},
                                    {"SourceUrl", article.url},
                                    {"SourceType", "16"},
                                    {"FriendlyTitle", VietCMS.Framework.Core.Common.WebControl.ToFriendlyString(article.title)},
                                    {"TypeId", "1"},
                                    {"AuthorAlias", article.author},
                                    {"Status", "2"},
                                    {"CreatedAt", DateTime.Now.ToString()},
                                    {"CreatedBy", System.Configuration.ConfigurationManager.AppSettings.Get("User_ID")},
                                    {"LastModifiedBy", System.Configuration.ConfigurationManager.AppSettings.Get("User_ID")},
                                    {"LastModifiedAt", DateTime.Now.ToString()}
                                }, "INSERTED.ArticleId");
                sqlhelperBackend.Add("[ArticleRoyalty]", new Dictionary<string, string> {
                                    { "ArticleId",insertedID.ToString() },
                                    { "RoyaltyStepId","16" },
                                    { "CreatedBy",System.Configuration.ConfigurationManager.AppSettings.Get("User_ID") },
                                    { "CreatedAt",DateTime.Now.ToString() },
                                    { "LastModifiedBy",System.Configuration.ConfigurationManager.AppSettings.Get("User_ID") },
                                    { "LastModifiedAt",DateTime.Now.ToString() }
                                });
            }
        }

    }
}
