using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels.Idenitity
{
    public class ResetPasswordViewModel
    {
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is Required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password Doesn't Match")]
        public string ConfirmPassword { get; set; }
    }
}
