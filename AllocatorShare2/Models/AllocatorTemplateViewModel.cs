using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AllocatorShare2.Core.Models;

namespace AllocatorShare2.Models
{
    public class AllocatorTemplateViewModel
    {
        public TreeListViewModel AllocatorList { get; set; }
        public List<SelectListItem> ManagerList { get; set; }

    }
}