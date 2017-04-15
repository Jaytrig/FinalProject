using System.IO;
using System.Web;
using System.Web.Mvc;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace SocialNetwork
{
    //Tregilgasj@gmail.com
    public class CreateCloudinary : Controller
    {
        private readonly Account _account = new Account(
          "socialmedia",
          "945839472949782",
          "eEqZjN-8QmH44awHLjEvvM4jrbY");

        public Cloudinary Cloudinary;

        public CreateCloudinary()
        {
            Cloudinary = new Cloudinary(_account);
        }

        public string UploadImage(HttpPostedFileBase UploadedFile)
        {
            var filename = UploadedFile.FileName;
            var filePathOriginal = Server.MapPath("/App_Data/");
            string savedFileName = Path.Combine(filePathOriginal, filename);

            UploadedFile.SaveAs(savedFileName);
            ImageUploadParams image = new ImageUploadParams();

            image.File = new FileDescription(savedFileName);

            var test = Cloudinary.Upload(image);

            if (System.IO.File.Exists(savedFileName))
            {
                System.IO.File.Delete(savedFileName);
            }


            return test.PublicId + "." + test.Format;
        }

    }
}