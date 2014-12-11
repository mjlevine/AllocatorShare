using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AllocatorShare2.Core.Interfaces;

namespace AllocatorShare2.Controllers
{
    public class DownloadController : Controller
    {
        private readonly IFileService _service;
        public DownloadController(IFileService service)
        {
            _service = service;
        }
        // GET: Download
        public async Task<ActionResult> Index(string id)
        {
            var fileUrl = await _service.DownloadFile(id);
            return Redirect(fileUrl);
        }
    }
}