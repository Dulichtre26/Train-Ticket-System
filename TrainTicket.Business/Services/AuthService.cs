using Microsoft.EntityFrameworkCore;
using TrainTicket.Business.DTOs;
using TrainTicket.Business.Interfaces;
using TrainTicket.Data.DbContexts;
using System.Globalization;

namespace TrainTicket.Business.Services
{
    // Service ??ng nh?p: dùng EF ?? l?y user/role và BCrypt ?? ki?m tra m?t kh?u.
    public class AuthService : IAuthService
    {
        private readonly TrainTicketDbContext _context;

        public AuthService(TrainTicketDbContext context)
        {
            _context = context;
        }

        public async Task<UserSessionDto?> LoginAsync(LoginRequestDto request)
        {
            // 1) Tìm user theo email và b? qua tài kho?n ?ã soft-delete.
            var user = await _context.Users
                .Include(x => x.UserRoles)
                .ThenInclude(x => x.Role)
                .FirstOrDefaultAsync(x => x.Email == request.Email && !x.IsDeleted);

            // 2) User không t?n t?i ho?c b? khóa -> ??ng nh?p th?t b?i.
            if (user == null || !user.IsActive)
            {
                return null;
            }

            // 3) So kh?p m?t kh?u theo ??nh d?ng ?ang l?u trong DB.
            var isValidPassword = VerifyPassword(request.Password, user.PasswordHash);
            if (!isValidPassword)
            {
                return null;
            }

            // 4) Tr? session DTO ?? UI l?u vào SessionManager.
            return new UserSessionDto
            {
                UserID = user.UserID,
                FullName = user.FullName,
                Email = user.Email,
                IsActive = user.IsActive,
                Roles = user.UserRoles.Select(ur => ur.Role.RoleName).Distinct().ToList()
            };
        }

        private static bool VerifyPassword(string inputPassword, string? storedHash)
        {
            if (string.IsNullOrWhiteSpace(inputPassword) || string.IsNullOrWhiteSpace(storedHash))
            {
                return false;
            }

            // D? li?u seed hi?n t?i dùng format: hash_<plainPassword>
            const string legacyPrefix = "hash_";
            if (storedHash.StartsWith(legacyPrefix, true, CultureInfo.InvariantCulture))
            {
                var plainPassword = storedHash.Substring(legacyPrefix.Length);
                return string.Equals(inputPassword, plainPassword, StringComparison.Ordinal);
            }

            // Ch? verify BCrypt khi hash ?úng format BCrypt ?? tránh l?i Invalid salt version.
            if (storedHash.StartsWith("$2", StringComparison.Ordinal))
            {
                try
                {
                    return BCrypt.Net.BCrypt.Verify(inputPassword, storedHash);
                }
                catch
                {
                    return false;
                }
            }

            // Fallback cho d? li?u legacy l?u plain text.
            return string.Equals(inputPassword, storedHash, StringComparison.Ordinal);
        }
    }
}
