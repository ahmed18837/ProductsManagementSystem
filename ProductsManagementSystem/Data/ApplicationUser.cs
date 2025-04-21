using Microsoft.AspNetCore.Identity;
using ProductsManagementSystem.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace ProductsManagementSystem.Data
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(50)]
        public string FullName { get; set; }

        [MaxLength(50)]
        [RegularExpression(@"^\d{14}$", ErrorMessage = "National ID must be exactly 14 digits.")]
        public string NationalId { get; set; }

        [MaxLength(8)]
        public string? ResetCode { get; set; }

        [MaxLength(8)]
        public string? TwoFactorCode { get; set; }
        public DateTime? TwoFactorCodeExpiration { get; set; }
        public DateTime? TwoFactorSentAt { get; set; } // 🔹 وقت إرسال الكود الأخير

        public int TwoFactorAttempts { get; set; } = 0; // عدد المحاولات خلال الساعة
        public DateTime? LastTwoFactorAttempt { get; set; } // آخر وقت للمحاولة

        // 🔹 جديد: محاولات الفشل وقفل الحساب
        public int FailedTwoFactorAttempts { get; set; } = 0;
        public DateTime? LockoutEnd { get; set; } // متى ينتهي القفل؟

        public List<RefreshToken>? RefreshTokens { get; set; }
    }
}
