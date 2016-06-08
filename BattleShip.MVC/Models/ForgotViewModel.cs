using System.ComponentModel.DataAnnotations;

namespace BattleShip.MVC.Models
{
    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}