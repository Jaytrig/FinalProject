﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using SocialNetwork.Database;

namespace SocialNetwork.Models
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        //public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
        public User User { get; set; }
    }

    public class PostViewModel
    {
        [Required]
        [StringLength(500, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        public string Message { get; set; }

        public string Title { get; set; }
        public DateTime PostedTime { get; set; }
    }


    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }


    public class NotificationListItemModel
    {
        public User User { get; set; }
        public int FriendRowId { get; set; }
    }


    public class FriendsListItemModel
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string DisplayPicture { get; set; }
    }

    public class FriendsChatListItemModel
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string DisplayPicture { get; set; }
        public string Surname { get; set; }
    }

    public class FriendsPartialModel
    {
        public string UserId { get; set; }
        public int Page { get; set; }
    }

    public class ChatViewMessageModel
    {
        public List<Message> messages { get; set; }
        public string userID { get; set; }
    }
}