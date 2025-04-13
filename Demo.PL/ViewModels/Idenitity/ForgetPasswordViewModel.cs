using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels.Idenitity
{
    public class ForgetPasswordViewModel
    {
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }
    }
}
