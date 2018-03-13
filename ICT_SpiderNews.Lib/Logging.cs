using System;
using BaoKhoe;
using BaoKhoe.Models;

namespace SpiderNews.Lib
{
    public class Logging
    {
        private readonly AppDBContext _appDbContext;

        public Logging(AppDBContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void Log(string type, string content)
        {
            Console.WriteLine(content);
            _appDbContext.Logs.Add(new Log()
            {
                Type = type,
                Content = content,
                CreatedAt = DateTime.Now
            });
            _appDbContext.SaveChanges();
        }
    }
}
