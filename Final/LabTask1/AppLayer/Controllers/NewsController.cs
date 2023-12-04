using BLL.DTOs;
using BLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AppLayer.Controllers
{
    public class NewsController : ApiController
    {
        [HttpGet]
        [Route("api/News/{id}")]
        public HttpResponseMessage GetNews(int id)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, NewsService.GetNews(id));
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }
        [HttpPost]
        [Route("api/News/create")]
        public HttpResponseMessage CreateNews(NewsDTO n)
        {
            try
            {
                if(NewsService.CreateNews(n))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { message = "News Added successfully" });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { message = "News Not Added" });
                }
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }
        [HttpGet]
        [Route("api/Category/{id}")]
        public HttpResponseMessage GetCategory(int id)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, CategoryServices.GetCategory(id));
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }
        [HttpPost]
        [Route("api/Category/create")]
        public HttpResponseMessage CreateCategory(CategoryDTO c)
        {
            try
            {
                if (CategoryServices.CreateCategory(c))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { message = "Category Added successfully" });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { message = "Category Not Added" });
                }
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }
    }
}
