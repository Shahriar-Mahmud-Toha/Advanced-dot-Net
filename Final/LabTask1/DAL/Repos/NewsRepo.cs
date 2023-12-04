using DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repos
{
    public class NewsRepo
    {
        private static List<News> allNews = new List<News>();
        public static News GetNews(int id)
        {
            News n = new News();
            n.Id = id;
            n.Title = "Demo Title";
            n.Date = DateTime.Now;
            return n;
        }
        public static bool CreateNews(News n) 
        {

            allNews.Add(n);
            return true;
        }
    }
}
