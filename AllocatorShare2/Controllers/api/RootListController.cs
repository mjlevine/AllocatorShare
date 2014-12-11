using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using AllocatorShare2.Core.Interfaces;
using FileService;

namespace AllocatorShare2.Controllers.api
{
    public class RootListController : ApiController
    {
        private IFileService _service;
        public RootListController(IFileService service)
        {
            _service = service;
        }

        [System.Web.Http.HttpGet]
        public async Task<List<SelectListItem>> Get(string id)
        {
            var list = await _service.GetRootList();
            var listItems = new List<SelectListItem>();
            foreach (var item in list.Contents)
            {
                listItems.Add(new SelectListItem()
                {
                    Text = item.Description,
                    Value = item.Id
                });
            }
            return listItems;
        }
    }
}
