using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using AllocatorShare2.Core.Interfaces;
using AllocatorShare2.Core.Models;
using AllocatorShare2.Models;
using FileService;

namespace AllocatorShare2.Controllers.api
{
    public class ShareController : ApiController
    {
        private IFileService _service;
        public ShareController(IFileService service)
        {
            _service = service;
        }
        [System.Web.Http.HttpGet]
        public async Task<AllocatorTemplateViewModel> Get(string id)
        {
            var list = await _service.GetFolderListContents(id, false);
            
            //Manager List
            var managerList = _service.GetManagerListItems(list.Contents);
            var managerListItems = managerList.Select(item => new SelectListItem()
            {
                Text = string.Format("{0}, {1}", item.Description, list.Description), Value = item.Id
            }).ToList();

            //Tree List
            var template = list.Contents.FirstOrDefault(m => m.Name == "Allocator_Templates");
            var templatesList = await _service.GetFolderListContents(template.Id, true, true);
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
