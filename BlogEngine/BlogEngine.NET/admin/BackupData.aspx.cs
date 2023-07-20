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
                var fileName = Path.Combine(Server.MapPath("~"), "Backup\\full.zip");
                // Deletes the old zip
                try
                {
                    System.IO.File.Delete(fileName);
                }
                catch { }

                // Starts the backup
                System.IO.Compression.ZipFile.CreateFromDirectory(inputDirectory, fileName);

                Response.Redirect("/Backup/full.zip");
            }
            else
                throw new UnauthorizedAccessException();
        }
    }
}