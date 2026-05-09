using TrainTicket.Business.DTOs;
using TrainTicket.Business.Interfaces;
using TrainTicket.Data.DbContexts;
using TrainTicket.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace TrainTicket.Business.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly TrainTicketDbContext _context;

        public DiscountService(TrainTicketDbContext context)
        {
            _context = context;
        }

        public async Task<DiscountDto?> GetDiscountByCodeAsync(string code)
        {
            var discount = await _context.Discounts
                .FirstOrDefaultAsync(d => d.Code == code && d.IsActive && d.ValidFrom <= DateTime.Now && d.ValidTo >= DateTime.Now);

            if (discount == null) return null;

            return new DiscountDto
            {
                DiscountID = discount.DiscountID,
                Code = discount.Code,
                Description = discount.Description,
                DiscountType = discount.DiscountType,
                Amount = discount.Amount,
                MinPrice = discount.MinPrice,
                MaxUses = discount.MaxUses,
                UsedCount = discount.UsedCount,
                ValidFrom = discount.ValidFrom,
                ValidTo = discount.ValidTo,
                IsActive = discount.IsActive,
                CreatedAt = discount.CreatedAt
            };
        }

        public async Task<bool> ApplyDiscountAsync(string code)
        {
            var discount = await _context.Discounts.FirstOrDefaultAsync(d => d.Code == code);
            if (discount == null || !discount.IsActive || discount.ValidTo < DateTime.Now) return false;

            if (discount.MaxUses.HasValue && discount.UsedCount >= discount.MaxUses.Value) return false;

            discount.UsedCount++;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<DiscountDto>> GetActiveDiscountsAsync()
        {
            var discounts = await _context.Discounts
                .Where(d => d.IsActive && d.ValidFrom <= DateTime.Now && d.ValidTo >= DateTime.Now)
                .ToListAsync();

            return discounts.Select(d => new DiscountDto
            {
                DiscountID = d.DiscountID,
                Code = d.Code,
                Description = d.Description,
                DiscountType = d.DiscountType,
                Amount = d.Amount,
                MinPrice = d.MinPrice,
                MaxUses = d.MaxUses,
                UsedCount = d.UsedCount,
                ValidFrom = d.ValidFrom,
                ValidTo = d.ValidTo,
                IsActive = d.IsActive,
                CreatedAt = d.CreatedAt
            });
        }
    }
}