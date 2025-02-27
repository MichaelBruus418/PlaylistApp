using BusinessLogic.BLL;
using DTO.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class TrackController : ApiController
    {
        TrackBLL TrackBLL = new TrackBLL();

        [HttpGet]
        public List<Track> GetAll()
        {
            return TrackBLL.GetAll();
        }

        [HttpGet]
        public Track GetById(int id)
        {
            return TrackBLL.GetById(id);
        }

        [HttpPost]
        public HttpResponseMessage Add(Track track)
        {
           try
           {
                TrackBLL.AddTrack(track);
                return Request.CreateResponse(HttpStatusCode.OK, "");
           }
           catch (Exception e)
           {
               return Request.CreateResponse(HttpStatusCode.InternalServerError, "Something went wrong.");
               
           }
        }
        
        [HttpDelete]
        public HttpResponseMessage DeleteById(int id)
        {
            try
            {
                int rowsDeleted = TrackBLL.DeleteById(id);
                //Trace.WriteLine("API: Rows deleted: " + rowsDeleted);
                if (rowsDeleted == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "");
                }
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Unable to delete Track with id: " + id);
            }
        }

    }
}
