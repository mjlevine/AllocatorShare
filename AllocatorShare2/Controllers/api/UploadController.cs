using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using AllocatorShare2.Core.Interfaces;

namespace AllocatorShare2.Controllers
{
    public class UploadController : ApiController
    {
        private readonly IFileService _service;
        public UploadController(IFileService service)
        {
            _service = service;
        }

        // POST
        public async Task<ResponseMessageResult> Index(string id)
        {
            try
            {
                // Check if the request contains multipart/form-data. 
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                Request.Content.LoadIntoBufferAsync().Wait();

                var provider = new MultipartMemoryStreamProvider();

                provider = Request.Content.ReadAsMultipartAsync(provider).Result;

                foreach (var item in provider.Contents.Where(x => x.Headers.ContentDisposition.Name != "\"uploadRootFolderId\""))
                {
                    var stream = item.ReadAsStreamAsync().Result;
                    await _service.UploadFile(stream, id, item.Headers.ContentDisposition.FileName);
                }

                var response = Request.CreateResponse(HttpStatusCode.OK, "Success", "text/html");
                return new ResponseMessageResult(response);
            }
            catch (Exception e)
            {
                return new ResponseMessageResult(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message));
            } 

        }
    }
}
