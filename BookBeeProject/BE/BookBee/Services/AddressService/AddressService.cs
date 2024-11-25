using AutoMapper;
using BookBee.DTOs.Address;
using BookBee.DTOs.Response;
using BookBee.Model;
using BookBee.Persistences.Repositories.AddressRepository;
using BookBee.Persistences.Repositories.UserRepository;
using BookBee.Utilities;

namespace BookBee.Services.AddressService
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly UserAccessor _userAccessor;
        public AddressService(IAddressRepository addressRepository, IUserRepository userRepository, IMapper mapper, UserAccessor userAccessor)
        {
            _addressRepository = addressRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _userAccessor = userAccessor;
        }
        public ResponseDTO CreateAddress(CreateAddressDTO createAddressDTO)
        {
            var user = _userRepository.GetUserById(createAddressDTO.UserId);
            if (user == null) return new ResponseDTO()
            {
                Code = 400,
                Message = "User không tồn tại"
            };
            var address = _mapper.Map<Address>(createAddressDTO);
            _addressRepository.CreateAddress(address);
            if (_addressRepository.IsSaveChanges())
                return new ResponseDTO()
                {
                    Message = "Tạo thành công",
                    Data = _mapper.Map<AddressDTO>(_addressRepository.GetAddressById())
                };
            else
                return new ResponseDTO()
                {
                    Code = 400,
                    Message = "Tạo thất bại"
                };
        }


        public ResponseDTO SelfCreateAddress(SelfCreateAddressDTO selfCreateAddressDTO)
        {
            var userId = _userAccessor.GetCurrentUserId();
            if (userId != null)
            {
                var address = _mapper.Map<Address>(selfCreateAddressDTO);
                address.UserId = (int)userId;
                _addressRepository.CreateAddress(address);
                if (_addressRepository.IsSaveChanges())
                    return new ResponseDTO()
                    {
                        Message = "Tạo mới thành công",
                        Data = address.Id
                    };
                return new ResponseDTO()
                {
                    Code = 400,
                    Message = "Tạo thất bại",
                };
            }

            return new ResponseDTO()
            {
                Code = 404,
                Message = "User không tồn tại"
            };
        }

        public ResponseDTO DeleteAddress(int id)
        {
            var address = _addressRepository.GetAddressById(id);
            if (address == null)
            {
                return new ResponseDTO()
                {
                    Code = 400,
                    Message = "Địa chỉ không tồn tại"
                };
            }

            address.IsDeleted = true;
            _addressRepository.UpdateAddress(address);
            if (_addressRepository.IsSaveChanges())
                return new ResponseDTO()
                {
                    Message = "Xóa thành công"
                };
            else
                return new ResponseDTO()
                {
                    Code = 400,
                    Message = "Xóa thất bại"
                };
        }

        public ResponseDTO GetAddressById(int id)
        {
            var address = _addressRepository.GetAddressById(id);
            if (address == null)
            {
                return new ResponseDTO()
                {
                    Code = 400,
                    Message = "Địa chỉ không tồn tại"
                };
            }
            return new ResponseDTO()
            {
                Data = _mapper.Map<AddressDTO>(address)
            };
        }

        public ResponseDTO GetAddressByUser(int userId)
        {
            var addresses = _addressRepository.GetAddressByUser(userId);
            return new ResponseDTO()
            {
                Data = _mapper.Map<List<AddressDTO>>(addresses)
            };
        }

        public ResponseDTO GetSelfAddresses()
        {
            var userId = _userAccessor.GetCurrentUserId();
            if (userId != null) return GetAddressByUser((int)userId);
            return new ResponseDTO()
            {
                Code = 400,
                Message = "User không tồn tại"
            };
        }

        public ResponseDTO GetAddresses(int? page = 1, int? pageSize = 10, string? key = "", string? sortBy = "ID")
        {
            var addresses = _addressRepository.GetAddresses(page, pageSize, key, sortBy);
            return new ResponseDTO()
            {
                Data = _mapper.Map<List<AddressDTO>>(addresses),
                Total = _addressRepository.GetAddressCount()
            };
        }

        public ResponseDTO UpdateAddress(int id, UpdateAddressDTO updateAddressDTO)
        {
            var address = _addressRepository.GetAddressById(id);
            if (address == null)
            {
                return new ResponseDTO()
                {
                    Code = 400,
                    Message = "Địa chỉ không tồn tại"
                };
            }

            address.Update = DateTime.Now;
            address.Name = updateAddressDTO.Name;
            address.Street = updateAddressDTO.Street;
            address.City = updateAddressDTO.City;
            address.State = updateAddressDTO.State;
            address.Phone = updateAddressDTO.Phone;

            _addressRepository.UpdateAddress(address);
            if (_addressRepository.IsSaveChanges())
                return new ResponseDTO()
                {
                    Message = "Cập nhật thành công"
                };
            else
                return new ResponseDTO()
                {
                    Code = 400,
                    Message = "Cập nhật thất bại"
                };
        }
    }
}
