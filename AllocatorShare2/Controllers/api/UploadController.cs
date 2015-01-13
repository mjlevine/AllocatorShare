using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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

                var root = HttpContext.Current.Server.MapPath("~/App_Data");
                var provider = new MultipartFormDataStreamProvider(root); 

                // Read the form data and return an async task. 
                await Request.Content.ReadAsMultipartAsync(provider);
                
                // This illustrates how to get the file names for uploaded files. 
                foreach (var fileData in provider.FileData)
                {
                    string fileName = ""; 
                    fileName = fileData.Headers.ContentDisposition.FileName; 
                    if (fileName.StartsWith("\"") && fileName.EndsWith("\"")) 
                    { 
                      fileName = fileName.Trim('"'); 
                    } 
                    if (fileName.Contains(@"/") || fileName.Contains(@"\")) 
                    { 
                      fileName = Path.GetFileName(fileName); 
                    }

                    var renamedFilePath = Path.Combine(root, fileName);
                    
                    if(File.Exists(renamedFilePath))
                    {
                        File.Delete(renamedFilePath);
                    }
                    File.Move(fileData.LocalFileName,
                      renamedFilePath);

                    using (var filestream = File.OpenRead(renamedFilePath))
                    {
                        await _service.UploadFile(filestream, id, fileName);
                    }
                    File.Delete(fileName);
                }

                return new ResponseMessageResult(Request.CreateErrorResponse(HttpStatusCode.OK, "Success"));
            }
            catch (Exception e)
            {
                return new ResponseMessageResult(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message));
            } 

        }
    }
}
