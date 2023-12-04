using AutoMapper;
using BLL.DTOs;
using DAL.EF;
using DAL.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class CategoryServices
    {
        public static CategoryDTO GetCategory(int id)
        {
            id *= 100;
            id++;
            var data = CategoryRepo.GetCategory(id);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Category, CategoryDTO>();
            });
            var mapper = new Mapper(config);
            var cData = mapper.Map<CategoryDTO>(data);
            return cData;
        }
        public static bool CreateCategory(CategoryDTO c)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CategoryDTO, Category>();
            });
            var mapper = new Mapper(config);
            var cData = mapper.Map<Category>(c);

            if (CategoryRepo.CreateCategory(cData))
            {
                return true;
            }
            return false;
        }
    }
}
