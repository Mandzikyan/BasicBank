using System.ComponentModel.DataAnnotations;

namespace Models.ChangePassword
{
    public class NewPassword
    {
        [Required]
        public string Password { get; set; }
        [Required]
        public string RepeatPassword { get; set; }
    }
}
