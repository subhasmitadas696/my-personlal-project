using System.ComponentModel.DataAnnotations;

namespace CTMS.Model.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Username is required.")]
        public string? USERNAME { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string? UPASSWORD { get; set; }
        [Required(ErrorMessage = "Otp is required.")]
        public string? OTP { get; set; } 
        public string? captcha { get; set; } 
        [Required(ErrorMessage = "Mobile is required.")]
        public string? MOBILENO { get; set; }
    }
    public class VerifyUser
    {
        public string? MOBILENO { get; set; } 
        public string? captcha { get; set; } 
    }
    public class VerifyOfficerRequest
    {
        public string? mobileno { get; set; }
        public string? TYPE { get; set; }
        public string? otp { get; set; }
    }
}

