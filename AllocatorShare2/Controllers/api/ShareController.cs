using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using AllocatorShare2.Core.Models;
using FileService;

namespace AllocatorShare2.Controllers.api
{
    public class ShareController : ApiController
    {
     

        

        [System.Web.Http.HttpGet]
        public async Task<TreeListViewModel> Test(string id)
        {
            var sf = new ShareFileService();
            var list = await sf.GetFolderListContents(id, false);
            var template = list.Contents.FirstOrDefault(m => m.Name == "Allocator_Templates");
            var templatesList = await sf.GetFolderListContents(template.Id, true);
            return templatesList;
        }
    }
}
