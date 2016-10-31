using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
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

    public static class TreeListViewModelExtensions
    {
        public static TreeListViewModel FilterRootList(this TreeListViewModel treeList)
        {
            treeList.Contents = treeList.Contents.Where(t =>
            {
                //Templates directory should not be included in root list - it is "special"
                if (t.Name.Equals("templates", StringComparison.CurrentCultureIgnoreCase))
                    return false;

                //ShareFile can have hidden directories that can't be seen in the portal and can't be deleted.  At the time
                //this is written, the object returned from the api has nothing to indicate this.  Since the only one we've seen
                //starts with a GUID, let's use that to filter them out.  The regex is detecting a string starting with a guid.
                if (t.Name.Length > 36 && Regex.IsMatch(t.Name.Substring(0, 36), "[A-F0-9]{8}(?:-[A-F0-9]{4}){3}-[A-F0-9]{12}", RegexOptions.IgnoreCase))
                    return false;

                return true;
            }).ToList();

            return treeList;
        }
    }

}
