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

        public void Log(string type, string content, Boolean save = true)
        {
            Console.WriteLine(content);
            if (save)
            {
                _appDbContext.Logs.Add(new Log()
                {
                    Type = type,
                    Content = content,
                    CreatedAt = DateTime.Now
                });
                try
                {
                    _appDbContext.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine("WRITE LOG ERROR: ", e);
                }
            }
        }
    }
}
