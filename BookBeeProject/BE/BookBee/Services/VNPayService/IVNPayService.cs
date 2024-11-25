using BookBee.DTOs.Response;

namespace BookBee.Services.VNPayService
{
    public interface IVNPayService
    {
        Task<ResponseDTO> CreateUrlPayment(int orderId, double total);
        Task<ResponseDTO> ReturnPayment(IQueryCollection vnpayData);
    }
}
