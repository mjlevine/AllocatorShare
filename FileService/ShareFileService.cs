using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AllocatorShare2.Core.Models;
using ShareFile.Api.Client;
using ShareFile.Api.Client.Enums;
using ShareFile.Api.Client.Extensions;
using ShareFile.Api.Client.Security.Authentication.OAuth2;
using ShareFile.Api.Models;
using AllocatorShare2.Core.Models;
using AllocatorShare2.Core.Interfaces;
using File = ShareFile.Api.Models.File;

namespace FileService
{
    public interface IShareFileService
    {
        
    }

    public class ShareFileService : IFileService
    {
        private string applicationControlPlane = ConfigurationHelper.ApplicationControlPlane;
        private string applicationApiUrl = ConfigurationHelper.ApiUrl;
        private string subdomain = ConfigurationHelper.Subdomain;
        private string username = ConfigurationHelper.Username;
        private string password = ConfigurationHelper.Password;


        private ShareFileClient _client;
        private string _oathToken;
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

        public async Task<string> DownloadFile(string id)
        {
            if (_client == null)
            {
                _client = await GetShareFileClient();
            }
            var auth = GetDownloadAuth();

            return GetDownloadUrl(string.Format("https://{0}.sharefile.com/rest/file.aspx?op=download&authid={1}&id={2}", subdomain, auth, id));

        }

        private string GetDownloadUrl(string url)
        {
            WebClient client = new WebClient();
            string downloadUrl = string.Empty;
            using (Stream data = client.OpenRead(url))
            {
                using (StreamReader reader = new StreamReader(data))
                {
                    downloadUrl = reader.ReadLine();
                }
            }

            return downloadUrl;
        }

        private string GetDownloadAuth()
        {
            string URL = string.Format("https://{0}.sharefile.com/rest/getAuthID.aspx?username={1}&password={2}", subdomain, username, password);
            WebClient client = new WebClient();
            string auth = string.Empty;
            using (Stream data = client.OpenRead(URL))
            {
                using (StreamReader reader = new StreamReader(data))
                {
                    auth = reader.ReadLine();
                }
            }
            
            return auth;
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

        private async Task<Item> GetItem(string id)
        {
            if (_client == null)
            {
                _client = await GetShareFileClient();
            }
            var uri = BuildUriFromId(id);
            var folder = _client.Items.Get(uri)
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
                Url = !isFolder ? c.GetObjectUri().AbsoluteUri : null,
                Contents = isFolder ? contents : null
            };
        }

        private async Task<ShareFileClient> GetShareFileClient()
        {
            var sfClient = new ShareFileClient("https://secure.sf-api.com/sf/v3/");
            sfClient.Configuration.ProxyConfiguration = new WebProxy(new Uri("http://127.0.0.1:8888"), false);
            var oauthService = new OAuthService(sfClient, ConfigurationHelper.ClientId, ConfigurationHelper.ClientSecret);

            var oauthToken = await oauthService.PasswordGrantAsync(username,
                password, subdomain, applicationControlPlane);

            _oathToken = oauthToken.AccessToken;

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
