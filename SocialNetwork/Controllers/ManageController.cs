using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CloudinaryDotNet.Actions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SocialNetwork.Database;
using SocialNetwork.Models;

namespace SocialNetwork.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ManageController()
        {
        }


        public ActionResult PrivateProfile()
        {
            var userModel = User.Identity.GetUser().Private;

            return View(userModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PrivateProfile(bool privateProfile)
        {


            using (SocialMediaEntities DbContext = new SocialMediaEntities())
            {
                var thisUser = User.Identity.GetUserId();

                var firstOrDefault = DbContext.Users.FirstOrDefault(x => x.UserId == thisUser);

                if (firstOrDefault != null) firstOrDefault.Private = Convert.ToInt32(privateProfile);

                DbContext.SaveChanges();
            }



            return RedirectToAction("Index", new { Message = ManageMessageId.PrivacySettingsChanged });
        }

        public ActionResult ProfilePicture()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProfilePicture(HttpPostedFileBase displayPicture)
        {

            CreateCloudinary cloudinary = new CreateCloudinary();
            ImageUploadResult cloudResult = null;
            

            if (displayPicture != null)
            {


                var filename = displayPicture.FileName;
                var filePathOriginal = Server.MapPath("/App_Data/");
                string savedFileName = Path.Combine(filePathOriginal, filename);

                displayPicture.SaveAs(savedFileName);
                ImageUploadParams image = new ImageUploadParams { File = new FileDescription(savedFileName) };

                cloudResult = cloudinary.Cloudinary.Upload(image);

                if (System.IO.File.Exists(savedFileName))
                {
                    System.IO.File.Delete(savedFileName);
                }
            }

            string publicImgID;
            string imgFormat;

            if (cloudResult != null)
            {
                publicImgID = cloudResult.PublicId;
                imgFormat = cloudResult.Format;
            }
            else
            {
                publicImgID = "dk4ihkrrjvllglzsp9js";
                imgFormat = "png";
            }

            using (SocialMediaEntities DbContext = new SocialMediaEntities())
            {
                var thisUser = User.Identity.GetUserId();

                var firstOrDefault = DbContext.Users.FirstOrDefault(x => x.UserId == thisUser);


                if (firstOrDefault != null) firstOrDefault.DisplayPicture = publicImgID + "." + imgFormat;

                DbContext.SaveChanges();
            }

           

            return RedirectToAction("Index", new { Message = ManageMessageId.ChangeProfilePicture });
        }


        public ActionResult Location()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Location(string location)
        {
            using (var context = new SocialMediaEntities())
            {
                var userid = User.Identity.GetUserId();
                var user = context.Users.Single(x => x.UserId == userid);

                user.Location = location ?? "";

                context.SaveChanges();
            }

            return RedirectToAction("Index", new { Message = ManageMessageId.ChangedLocation });
        }

        public ActionResult ChangeDateOfBirth()
        {
            return View();
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.ChangedLocation ? "Your Location has been changed"
                : message == ManageMessageId.ChangeProfilePicture ? "Your Profile Picture has been changed"
                : message == ManageMessageId.PrivacySettingsChanged ? "Your Privacy settings has been changed"
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";

            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId),
                User = User.Identity.GetUser()

            };
            return View(model);
        }


        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error,
            ChangedLocation,
            ChangeProfilePicture,
            PrivacySettingsChanged
        }

        #endregion
    }
}