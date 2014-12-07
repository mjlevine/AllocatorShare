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
    public class ManagerListController : ApiController
    {
        [System.Web.Http.HttpGet]
        public async Task<List<SelectListItem>> Get(string id)
        {
            var sf = new ShareFileService();
            var list = await sf.GetFolderListContents(id, true);
            var listItems = new List<SelectListItem>();
            var managerContents = list.Contents.Where(m => !m.Name.Equals("Allocator_Templates"));
            foreach (var item in managerContents)
            {
                listItems.Add(new SelectListItem()
                {
                    Text = string.Format("{0}, {1}", item.Description, list.Description),
                    Value = item.Id
                });
            }
            return listItems;
        }
    }
}
