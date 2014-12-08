using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using AllocatorShare2.Core.Models;
using AllocatorShare2.Models;
using FileService;

namespace AllocatorShare2.Controllers.api
{
    public class ShareController : ApiController
    {
        [System.Web.Http.HttpGet]
        public async Task<AllocatorTemplateViewModel> Test(string id)
        {
            var sf = new ShareFileService();
            var list = await sf.GetFolderListContents(id, false);
            
            //Manager List
            var managerList = sf.GetManagerListItems(list.Contents);
            var managerListItems = managerList.Select(item => new SelectListItem()
            {
                Text = string.Format("{0}, {1}", item.Description, list.Description), Value = item.Id
            }).ToList();

            //Tree List
            var template = list.Contents.FirstOrDefault(m => m.Name == "Allocator_Templates");
            var templatesList = await sf.GetFolderListContents(template.Id, true);
            templatesList.Description = string.Format("EZ Allocator - {0}", list.Description);
            var toReturn = new AllocatorTemplateViewModel()
            {
                AllocatorList = templatesList,
                ManagerList = managerListItems
            };

            return toReturn;
        }
    }
}
