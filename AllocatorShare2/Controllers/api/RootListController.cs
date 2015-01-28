using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using AllocatorShare2.Constants;
using AllocatorShare2.Core.Interfaces;
using FileService;
using MK6.Common.Caching.Core;
using MK6.Common.Caching.Providers;

namespace AllocatorShare2.Controllers.api
{
    public class RootListController : BaseApiController
    {
       
        public RootListController(IFileService service, ICacheProvider cacheProvider) : base(service, cacheProvider)
        {
        }

        [System.Web.Http.HttpGet]
        public async Task<List<SelectListItem>> Get(string id)
        {

            const string rootListKey = SiteSettings.RootListCacheKey;

            if (_cacheProvider.Exists(rootListKey))
            {
                var cachedList = _cacheProvider.Get<List<SelectListItem>>(rootListKey);
                if (cachedList != null)
                {
                    return cachedList;
                }
            }

            var listItems = new List<SelectListItem>();
            var list = await _service.GetRootList();
            foreach (var item in list.Contents.OrderBy(x => x.Description))
            {
                listItems.Add(new SelectListItem()
                {
                    Text = item.Description,
                    Value = item.Id
                });
            }
            _cacheProvider.Set(rootListKey, listItems, SiteSettings.DefaultCacheTimeSpan);
            return listItems;
        }
    }
}
