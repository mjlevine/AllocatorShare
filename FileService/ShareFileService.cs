using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AllocatorShare2.Core.Models;
using ShareFile.Api.Client;
using ShareFile.Api.Client.Security.Authentication.OAuth2;
using ShareFile.Api.Models;
using AllocatorShare2.Core.Models;

namespace FileService
{
    public class ShareFileService
    {
        private string applicationControlPlane = "sharefile.com";
        private string applicationApiUrl = "sf-api.com";
        private string subdomain = "market6";
        private string username = "rick.svitak@market6.com";
        private string password = "dime1400";
        //private string subdomain = "spaceshipb";
        //private string username = "adam@spaceshipb.com";
        //private string password = "D&y4A5J5Em4&";

        public async Task<TreeListViewModel> GetRootList()
        {
            var list = new TreeListViewModel();
       
            var folders = await GetRoot();
            var f = folders;
            list.Name = f.Name;
            list.Contents = new List<TreeListViewModel>();
            foreach (var c in f.Children)
            {
                var tr = await ProcessShareFileItem(c);
                list.Contents.Add(tr);
            }
            return list;
        }
        
        public async Task<TreeListViewModel> GetFolderList()
        {
            var list = new TreeListViewModel();

            var folders = await GetRoot();
            list.Name = folders.Name;
            list.Contents = new List<TreeListViewModel>();
            foreach (var c in folders.Children)
            {
                var tr = await ProcessShareFileItem(c, true);
                list.Contents.Add(tr);
            }
            return list;
        }

        public async Task<TreeListViewModel> GetFolderListContents(string id)
        {
            var items = await GetItemList(BuildUriFromId(id));
            var list = new TreeListViewModel();
            list.Name = items.Name;
            list.Contents = new List<TreeListViewModel>();
            foreach (var c in items.Children)
            {
                var t = await ProcessShareFileItem(c, true);
                list.Contents.Add(t);
            }
            return list;
        }

        private async Task<List<TreeListViewModel>> GetFolderListContents(Uri uri)
        {
            var items = await GetItemList(uri);
            var list = new List<TreeListViewModel>();
            foreach (var c in items.Children)
            {
                var t = await ProcessShareFileItem(c);
                list.Add(t);
            }
            return list;
        }

        private async Task<Folder> GetRoot()
        {
            var sfClient = await GetShareFileClient();

            var folder = (Folder)sfClient.Items.Get()
                .Execute();

            return folder;
        }

        private async Task<Folder> GetItemList(Uri uri)
        {
            var sfClient = await GetShareFileClient();

            var folder = (Folder)sfClient.Items.Get(uri)
               .Expand("Children")
               .Execute();

            return folder;
        }


        private async Task<TreeListViewModel> ProcessShareFileItem(Item c, bool expand = false)
        {
            var folder = c as Folder;
            var isFolder = folder != null;
            var contents = new List<TreeListViewModel>();
            if (isFolder && expand)
            {
                contents = await GetFolderListContents(folder.url);
            }
            var tr = new TreeListViewModel()
            {
                Name = c.Name,
                Type = isFolder ? "folder" : "file",
                Id = c.Id,
                Contents = isFolder ? contents : null
            };
            return tr;
        }
        private async Task<ShareFileClient> GetShareFileClient()
        {
            var sfClient = new ShareFileClient("https://secure.sf-api.com/sf/v3/");
            var oauthService = new OAuthService(sfClient, ConfigurationHelper.ClientId, ConfigurationHelper.ClientSecret);

            var oauthToken = await oauthService.PasswordGrantAsync(username,
                password, subdomain, applicationControlPlane);

            sfClient.AddOAuthCredentials(oauthToken);
            sfClient.BaseUri = oauthToken.GetUri();
            return sfClient;
        }

        private Uri BuildUriFromId(string id)
        {
            return new Uri(string.Format("https://{0}.{1}/sf/v3/Items({2})", subdomain, applicationApiUrl, id));
        }

        
    }
}
