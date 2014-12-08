using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AllocatorShare2.Core.Models;
using ShareFile.Api.Client;
using ShareFile.Api.Client.Enums;
using ShareFile.Api.Client.Extensions;
using ShareFile.Api.Client.Security.Authentication.OAuth2;
using ShareFile.Api.Models;
using AllocatorShare2.Core.Models;

namespace FileService
{
    public class ShareFileService
    {
        private string applicationControlPlane = ConfigurationHelper.ApplicationControlPlane;
        private string applicationApiUrl = ConfigurationHelper.ApiUrl;
        private string subdomain = ConfigurationHelper.Subdomain;
        private string username = ConfigurationHelper.Username;
        private string password = ConfigurationHelper.Password;


        private ShareFileClient _client;

        public async Task<TreeListViewModel> GetRootList()
        {
            var list = new TreeListViewModel();

            var folders = await GetFolderListContents(ItemAlias.Root.ToString(), true, false);
            
            list.Name = folders.Name;
            list.Contents = folders.Contents.First().Contents;
            
            return list;
        }
        
        public async Task<TreeListViewModel> GetFolderList()
        {
            var items = await GetRoot();
            var list = PopulateTreeListViewModel(items, true, new List<TreeListViewModel>());
            foreach (var c in items.Children)
            {
                var tr = await ProcessShareFileItem(c, true);
                list.Contents.Add(tr);
            }
            return list;
        }


        public async Task<TreeListViewModel> GetFolderListContents(string id, bool expand, bool goDeep = false)
        {
            var items = await GetItemList(BuildUriFromId(id));
            var list = PopulateTreeListViewModel(items, true, new List<TreeListViewModel>());
            foreach (var c in items.Children)
            {
                var t = await ProcessShareFileItem(c, expand, goDeep);
                list.Contents.Add(t);
            }
            return list;
        }

        public List<TreeListViewModel> GetManagerListItems(List<TreeListViewModel> contents)
        {
            var managerContents = contents.Where(m => !m.Name.Equals("Allocator_Templates")).OrderBy(m => m.Name).ToList();

            return managerContents; 
        }

        public TreeListViewModel GetAllocatorTemplate(List<TreeListViewModel> contents)
        {
            var allocatorContent = contents.FirstOrDefault(m => m.Name.Equals("Allocator_Templates"));

            return allocatorContent;
        }

        private async Task<List<TreeListViewModel>> GetFolderListContents(Uri uri, bool expand, bool goDeep = false)
        {
            var items = await GetItemList(uri);
            var list = new List<TreeListViewModel>();
            foreach (var c in items.Children)
            {
                var t = await ProcessShareFileItem(c, expand, goDeep);
                list.Add(t);
            }
            return list;
        }

        private async Task<Folder> GetRoot()
        {
            if (_client == null)
            {
                _client = await GetShareFileClient();
            }

            var itemUri = _client.Items.GetAlias(ItemAlias.Home);
            var folder = (Folder)await _client.Items.Get(itemUri).Expand("Children").ExecuteAsync();

            return folder;
        }

        private async Task<Folder> GetItemList(Uri uri)
        {
            if (_client == null)
            {
                _client = await GetShareFileClient();
            }

            var folder = (Folder)_client.Items.Get(uri)
               .Expand("Children")
               .Execute();

            return folder;
        }


        private async Task<TreeListViewModel> ProcessShareFileItem(Item c, bool expand, bool goDeep = false)
        {
            var folder = c as Folder;
            var isFolder = folder != null;
            var contents = new List<TreeListViewModel>();
            if (isFolder && expand)
            {
                contents = await GetFolderListContents(folder.url, goDeep);
            }
            var tr = PopulateTreeListViewModel(c, isFolder, contents);
            return tr;
        }

        private static TreeListViewModel PopulateTreeListViewModel(Item c, bool isFolder, List<TreeListViewModel> contents)
        {
            return new TreeListViewModel()
            {
                Name = c.Name,
                Description = !string.IsNullOrEmpty(c.Description) ? c.Description : c.Name.Replace("_", " "),
                Type = isFolder ? "folder" : "file",
                Id = c.Id,
                Contents = isFolder ? contents : null
            };
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
