using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllocatorShare2.Core.Models
{
    public class TreeListViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public string Id { get; set; }

        public string DownloadUrl
        {
            get { return "/download/index/" + Id; }
        }

        public List<TreeListViewModel> Contents { get; set; }
    }
}
