using Microsoft.EntityFrameworkCore;
using TrainTicket.Business.DTOs;
using TrainTicket.Business.Interfaces;
using TrainTicket.Data.DbContexts;
using TrainTicket.Data.Entities;
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

            // Ch? verify BCrypt khi hash ??ng format BCrypt ?? tránh l?i Invalid salt version.
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

        public async Task<bool> RegisterAsync(RegisterRequestDto request)
        {
            // 1) Ki?m tra email ?? ??ng k? ch?a.
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == request.Email && !x.IsDeleted);

            if (existingUser != null)
            {
                return false; // Email ?? t?n t?i
            }

            // 2) Hash m?t kh?u b?ng BCrypt
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // 3) T?o user m?i
            var newUser = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                PasswordHash = passwordHash,
                IsActive = true,
                RegionCode = "HQ" // Ho?c l?y t? context
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            // 4) G?n role "User" m?c ??nh
            var userRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "User");
            if (userRole != null)
            {
                _context.UserRoles.Add(new UserRole { UserID = newUser.UserID, RoleID = userRole.RoleID });
                await _context.SaveChangesAsync();
            }

            return true;
        }
    }
}
