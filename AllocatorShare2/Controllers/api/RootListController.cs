using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using FileService;

namespace AllocatorShare2.Controllers.api
{
    public class RootListController : ApiController
    {
        [System.Web.Http.HttpGet]
        public async Task<List<SelectListItem>> Get(string id)
        {
            var sf = new ShareFileService();
            var list = await sf.GetRootList();
            var listItems = new List<SelectListItem>();
            foreach (var item in list.Contents)
            {
                listItems.Add(new SelectListItem()
                {
                    Text = item.Name,
                    Value = item.Id
                });
            }
            return listItems;
        }
    }
}
