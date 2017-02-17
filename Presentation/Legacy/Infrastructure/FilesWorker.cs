using System.Collections.Generic;
using System.Web;
using CloudinaryDotNet.Actions;

namespace Presentation.Legacy.Infrastructure
{
    public class FilesWorker
    {
        public List<string> UploadFile(HttpRequestBase request, HttpResponseBase response)
        {
            CloudinaryDotNet.Account account = new CloudinaryDotNet.Account("dodcik4uy",
                "119519442824995", "Ube1B3KlZ_fcC6PYe6_Hv472PJI");
            CloudinaryDotNet.Cloudinary cloudinary = new CloudinaryDotNet.Cloudinary(account);
            string url = null, filename = null;
            if (request.Files.Count > 0)
            {
                HttpPostedFileBase file = request.Files[0];
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, file.InputStream)
                };
                filename = file.FileName;
                url = cloudinary.Upload(uploadParams).SecureUri.ToString();
            }
            if (request.Params.Keys[0].Equals("CKEditor"))
            {
                response.Write("<script>window.parent.CKEDITOR.tools.callFunction(" +
                    request["CKEditorFuncNum"] + ", \"" + url + "\");</script>");
                response.End();
            }
            return new List<string> {filename, url};
        }
    }
}
