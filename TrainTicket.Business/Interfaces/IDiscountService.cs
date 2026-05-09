using TrainTicket.Business.DTOs;

namespace TrainTicket.Business.Interfaces
{
    public interface IDiscountService
    {
        Task<DiscountDto?> GetDiscountByCodeAsync(string code);
        Task<bool> ApplyDiscountAsync(string code);
        Task<IEnumerable<DiscountDto>> GetActiveDiscountsAsync();
    }
}