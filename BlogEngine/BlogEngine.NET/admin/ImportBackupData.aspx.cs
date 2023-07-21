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
    public partial class ImportBackupData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {

            }
            else
                throw new UnauthorizedAccessException();
        }
        protected void Upload(object sender, EventArgs e)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                //Access the File using the Name of HTML INPUT File.
                HttpPostedFile postedFile = Request.Files["FileUpload"];

                //Check if File is available.
                if (postedFile != null && postedFile.ContentLength > 0)
                {
                    //Save the File.
                    string filePath = Server.MapPath("~/Backup/") + Path.GetFileName(postedFile.FileName);
                    postedFile.SaveAs(filePath);
                    lblMessage.Visible = true;

                    // Ok, file saved
                    // Deletes directory
                    var outputDirectory = Server.MapPath("~") + "\\App_Data";
                    Directory.Delete(outputDirectory, true);
                    // And recreates
                    Directory.CreateDirectory(outputDirectory);

                    // now unzip
                    System.IO.Compression.ZipFile.ExtractToDirectory(filePath, outputDirectory);

                }
            }
            else
                throw new UnauthorizedAccessException();
        }
    }
}