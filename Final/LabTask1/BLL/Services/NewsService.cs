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
    public class NewsService
    {
        public static NewsDTO GetNews(int id)
        {
            id *= 1000;
            var data = NewsRepo.GetNews(id);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<News, NewsDTO>();
            });
            var mapper = new Mapper(config);
            var cData = mapper.Map<NewsDTO>(data);
            return cData;
        }
        public static bool CreateNews(NewsDTO n)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<NewsDTO, News>();
            });
            var mapper = new Mapper(config);
            var cData = mapper.Map<News>(n);

            if (NewsRepo.CreateNews(cData))
            {
                return true;
            }
            return false;
        }
    }
}
