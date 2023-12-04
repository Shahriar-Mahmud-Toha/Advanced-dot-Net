using DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repos
{
    public class CategoryRepo
    {
        private static List<Category> categories = new List<Category>();
        public static Category GetCategory(int id)
        {
            Category c = new Category();
            c.Id = id;
            c.Title = "Demo Title";
            return c;
        }
        public static bool CreateCategory(Category c)
        {
            categories.Add(c);
            return true;
        }
    }
}
