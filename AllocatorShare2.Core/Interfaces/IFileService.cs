﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AllocatorShare2.Core.Models;

namespace AllocatorShare2.Core.Interfaces
{
    public interface IFileService
    {
        Task<TreeListViewModel> GetRootList();
        Task<TreeListViewModel> GetTemplateFolderList(string name);
        Task<TreeListViewModel> GetFolderList();
        Task<TreeListViewModel> GetFolderListContents(string id, bool expand, bool goDeep = false);
        List<TreeListViewModel> GetManagerListItems(List<TreeListViewModel> contents);
        TreeListViewModel GetAllocatorTemplate(List<TreeListViewModel> contents);
        Task<string> DownloadFile(string id);
        Task<bool> UploadFile(Stream stream, string parentFolderId, string fileName);
    }
}
