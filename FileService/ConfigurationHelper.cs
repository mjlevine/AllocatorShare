using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileService
{
    public class ConfigurationHelper
    {
        public static string ClientSecret 
        {
            get { return ConfigurationManager.AppSettings["ShareFileClientSecret"]; }
        }

        public static string ClientId
        {
            get { return ConfigurationManager.AppSettings["ShareFileClientId"]; }
        }

        public static string Subdomain
        {
            get { return ConfigurationManager.AppSettings["ShareFileSubdomain"]; }
        }

        public static string Username
        {
            get { return ConfigurationManager.AppSettings["ShareFileUser"]; }
        }

        public static string Password
        {
            get { return ConfigurationManager.AppSettings["ShareFilePassword"]; }
        }

        public static string ApplicationControlPlane
        {
            get { return ConfigurationManager.AppSettings["ShareFileApplicationControlPlane"]; }
        }

        public static string ApiUrl
        {
            get { return ConfigurationManager.AppSettings["ShareFileApiUrl"]; }
        }
    }
}
