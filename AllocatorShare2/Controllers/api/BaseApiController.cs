using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AllocatorShare2.Core.Interfaces;
using MK6.Common.Caching.Core;

namespace AllocatorShare2.Controllers.api
{
    public class BaseApiController : ApiController
    {
        protected IFileService _service;
        protected ICacheProvider _cacheProvider;
        public BaseApiController(IFileService service, ICacheProvider cacheProvider)
        {
            _cacheProvider = cacheProvider;
            _service = service;
        }
    }
}
