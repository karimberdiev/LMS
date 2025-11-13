using System.ComponentModel.DataAnnotations;

namespace LMS.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Iltimos, Ismingizni kiriting!")]
        [StringLength(100, ErrorMessage ="Ism uzunligi 100 belgidan oshmasin")]
        [Display(Name = "To'liq ism")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Yoshingizni kiriting!")]
        [Range(7,70,ErrorMessage ="Yosh 7 dan 70 gacha bo'lishi kerak")]
        [Display(Name ="Yosh")]
        public int Age { get; set; }

        [EmailAddress(ErrorMessage ="Email formati noto'g'ri")]
        [StringLength(100)]
        [Display(Name ="Email manzil")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Telefon raqam formati noto'g'ri")]
        [Display(Name ="Telefon")]
        public string PhoneNumber { get; set; }

        [StringLength(200)]
        [Display(Name ="Manzil")]
        public string Address { get; set; }

        [Display(Name ="Jinsi")]
        public Gender Gender { get; set; }

        [Display(Name ="Holati")]
        public StudentStatus Status { get; set; } = StudentStatus.Active;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
        public string DisplayName => $"{FullName} ({Age} yosh)";
    }
    public enum Gender
    {
        [Display(Name ="Erkak")]
        Male = 1,
        [Display(Name ="Ayol")]
        Female = 2
    }
    public enum StudentStatus
    {
        [Display(Name ="Faol")]
        Active = 1,
        [Display(Name ="To'xtatilgan")]
        Suspended = 2,
        [Display(Name ="Bitirgan")]
        Graduated = 3
    }
}
