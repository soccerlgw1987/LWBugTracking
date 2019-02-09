using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace LWBugTracking.Helper
{
    public class FileUploadValidator
    {
        public static bool IsWebFriendlyImage(HttpPostedFileBase file)
        {
            if (file == null)
                return false;
            if (file.ContentLength > 2 * 1024 * 1024 || file.ContentLength < 1024)
                return false;

            try
            {
                var validExtensions = new List<string>();

                foreach(var ext in WebConfigurationManager.AppSettings["validExtensions"].Split(','))
                {
                    if(Path.GetExtension(file.FileName).Contains(ext))
                    {
                        return true;
                    }
                }      
            }
            catch
            {
                return false;
            }
            return false;
        }
    }
}
