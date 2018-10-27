using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;
using System.Linq;

namespace Nomaddoors.Models
{
    public class UniqueEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DB_A095E6_nomaddoorsEntities nomadDB = new DB_A095E6_nomaddoorsEntities();
            if (value != null)
            {
                string Email = value.ToString();
                int count = nomadDB.Users.Where(x => x.Email == Email).ToList().Count();
                if (count != 0)
                    return new ValidationResult("This email already exists");
                return ValidationResult.Success;
            }
            return new ValidationResult("Please provide your Email");
        }
    }
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }

    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }
    }

    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel
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
        [Required(ErrorMessage = "Provide your E-mail address", AllowEmptyStrings = false)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Provide your password", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required(ErrorMessage = "Provide your name", AllowEmptyStrings = false)]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Provide your surname")]
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        [UniqueEmail]
        [Required(ErrorMessage = "Provide your E-mail address", AllowEmptyStrings = false)]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Provide your gender", AllowEmptyStrings = false)]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Password mus be at least {2} elements", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password Confirmation")]
        [Compare("Password", ErrorMessage = "Password and the confirmation does not match")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Provide your country", AllowEmptyStrings = false)]
        [Display(Name = "Country")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Provide your birthday")]
        [Display(Name = "Birth")]
        public DateTime Birthday { get; set; }

    }

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }
}
