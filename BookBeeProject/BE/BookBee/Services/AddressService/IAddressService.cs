using BookBee.DTOs.Address;
using BookBee.DTOs.Response;

namespace BookBee.Services.AddressService
{
    public interface IAddressService
    {
        ResponseDTO GetAddresses(int? page = 1, int? pageSize = 10, string? key = "", string? sortBy = "ID");
        ResponseDTO GetAddressByUser(int userId);
        ResponseDTO GetSelfAddresses();
        ResponseDTO GetAddressById(int id);
        ResponseDTO UpdateAddress(int id, UpdateAddressDTO updateAddressDTO);
        ResponseDTO DeleteAddress(int id);
        ResponseDTO CreateAddress(CreateAddressDTO createAddressDTO);
        ResponseDTO SelfCreateAddress(SelfCreateAddressDTO selfCreateAddressDTO);
    }
}
