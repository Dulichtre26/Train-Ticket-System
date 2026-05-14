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
        private const int MaxFailedAttempts = 5;
        private const int LockoutMinutes    = 30;
        private readonly TrainTicketDbContext _context;

        public AuthService(TrainTicketDbContext context) => _context = context;

        public async Task<UserSessionDto?> LoginAsync(LoginRequestDto request)
        {
            var user = await _context.Users
                .Include(x => x.UserRoles).ThenInclude(x => x.Role)
                .FirstOrDefaultAsync(x => x.Email == request.Email && !x.IsDeleted);

            if (user == null) return null;

            // Ki?m tra tài kho?n b? khóa t?m th?i
            if (user.LockoutUntil.HasValue && user.LockoutUntil > DateTime.UtcNow)
            {
                var remaining = (user.LockoutUntil.Value - DateTime.UtcNow).TotalMinutes;
                throw new InvalidOperationException(
                    $"Tài kho?n b? khóa. Th? l?i sau {remaining:F0} phút.");
            }

            if (!user.IsActive)
                throw new InvalidOperationException("Tài kho?n ?ã b? vô hi?u hóa.");

            var isValid = VerifyPassword(request.Password, user.PasswordHash);

            if (!isValid)
            {
                // T?ng ??m th?t b?i, khóa n?u quá gi?i h?n
                user.FailedLoginCount++;
                if (user.FailedLoginCount >= MaxFailedAttempts)
                {
                    user.LockoutUntil = DateTime.UtcNow.AddMinutes(LockoutMinutes);
                    user.FailedLoginCount = 0;
                }
                await _context.SaveChangesAsync();
                return null;
            }

            // ??ng nh?p thành công — reset lockout
            user.FailedLoginCount = 0;
            user.LockoutUntil     = null;
            user.LastLoginAt      = DateTime.UtcNow;
            user.UpdatedAt        = DateTime.Now;
            await _context.SaveChangesAsync();

            return new UserSessionDto
            {
                UserID   = user.UserID,
                FullName = user.FullName,
                Email    = user.Email,
                IsActive = user.IsActive,
                LoginAt  = DateTime.Now,
                Roles    = user.UserRoles.Select(ur => ur.Role.RoleName).Distinct().ToList()
            };
        }

        public async Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            if (!VerifyPassword(oldPassword, user.PasswordHash))
                throw new InvalidOperationException("M?t kh?u c? không ?úng.");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword, workFactor: 11);
            user.UpdatedAt    = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnlockAccountAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            user.LockoutUntil     = null;
            user.FailedLoginCount = 0;
            user.IsActive         = true;
            user.UpdatedAt        = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RegisterAsync(RegisterRequestDto request)
        {
            // Ki?m tra email ?ã t?n t?i
            var existing = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (existing != null) return false;

            var user = new User
            {
                FullName = request.FullName,
                Email    = request.Email,
                PhoneNumber = request.PhoneNumber,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                IsActive     = true,
                RegionCode   = "HQ"
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Gán role User
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "User");
            if (role != null)
            {
                _context.UserRoles.Add(new UserRole { UserID = user.UserID, RoleID = role.RoleID });
                await _context.SaveChangesAsync();
            }
            return true;
        }

        private static bool VerifyPassword(string input, string? stored)
        {
            if (string.IsNullOrWhiteSpace(input) || string.IsNullOrWhiteSpace(stored))
                return false;

            // Legacy seed data: "hash_<plain>"
            const string legacy = "hash_";
            if (stored.StartsWith(legacy, true, CultureInfo.InvariantCulture))
                return string.Equals(input, stored[legacy.Length..], StringComparison.Ordinal);

            // BCrypt
            if (stored.StartsWith("$2", StringComparison.Ordinal))
            {
                try { return BCrypt.Net.BCrypt.Verify(input, stored); }
                catch { return false; }
            }

            return string.Equals(input, stored, StringComparison.Ordinal);
        }
    }
}
