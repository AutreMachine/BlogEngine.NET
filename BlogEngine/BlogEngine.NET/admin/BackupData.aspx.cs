using BlogEngine.Core;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace BlogEngine.NET.admin
{
    public partial class BackupData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var inputDirectory = Server.MapPath("~") + "\\App_Data";

                // Get site name
                var siteRoot = Utils.AbsoluteWebRoot.Host;
                // Extract only name
                var zipName = "Backup\\" + siteRoot + "-" + Guid.NewGuid().ToString() + ".zip";
                var fileName = Path.Combine(Server.MapPath("~"), zipName);
                // Deletes the old zip
                try
                {
                    System.IO.File.Delete(fileName);
                }
                catch { }

                // Starts the backup
                System.IO.Compression.ZipFile.CreateFromDirectory(inputDirectory, fileName);

                Response.Redirect("/" + zipName);
            }
            else
                throw new UnauthorizedAccessException();
        }
    }
}