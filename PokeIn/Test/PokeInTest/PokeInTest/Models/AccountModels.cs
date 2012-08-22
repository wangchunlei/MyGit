using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using PokeIn;
using PokeIn.Comet;

namespace PokeInTest.Models
{

    public class ChangePasswordModel
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

    public class LoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ServerInstance : IDisposable
    {
        public ServerInstance(string clientId, bool isDesktop)
        {
            IsDesktop = isDesktop;
            ClientId = clientId;

            string message = "";

            //if you call a method from DesktopClient you must serialize it by EXTML class
            if (IsDesktop)
                message = EXTML.Method("ClientTest", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));//calls a method from DesktopTest class defined on Desktop application
            else
                message = JSON.Method("ClientTest", DateTime.Now.ToLongDateString());

            CometWorker.SendToClient(ClientId, message);
        }

        public void Dispose()
        {
            //Do whatever you want here : PokeIn is going to call this method when user disconnectes
        }

        public void GetServerTime()
        {
            string msg = JSON.Method("ServerTime", DateTime.Now);
            CometWorker.SendToClient(ClientId, msg);


        }

        public bool IsDesktop { get; set; }

        public string ClientId { get; set; }
    }
}
