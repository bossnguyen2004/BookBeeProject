using AutoMapper;
using BookBee.DTOs.Publisher;
using BookBee.DTOs.Response;
using BookBee.Model;
using BookBee.Persistences.Repositories.PublisherRepository;

namespace BookBee.Services.PublisherService
{
    public class PublisherService : IPublisherService
    {
        private readonly IPublisherRepository _publisherRepository;
        private readonly IMapper _mapper;
        public PublisherService(IPublisherRepository publisherRepository, IMapper mapper)
        {
            _publisherRepository = publisherRepository;
            _mapper = mapper;
        }

        public ResponseDTO CreatePublisher(string name)
        {
            var publisher = new Publisher { Name = name };
            _publisherRepository.CreatePublisher(publisher);
            if (_publisherRepository.IsSaveChanges()) return new ResponseDTO() { Message = "Tạo thành công" };
            else return new ResponseDTO() { Code = 400, Message = "Tạo thất bại" };
        }

        public ResponseDTO DeletePublisher(int id)
        {
            var publisher = _publisherRepository.GetPublisherById(id);
            if (publisher == null)
                return new ResponseDTO()
                {
                    Code = 400,
                    Message = "Publisher không tồn tại"
                };
            publisher.IsDeleted = true;
            _publisherRepository.UpdatePublisher(publisher);
            if (_publisherRepository.IsSaveChanges()) return new ResponseDTO() { Message = "Cập nhật thành công" };
            else return new ResponseDTO() { Code = 400, Message = "Cập nhật thất bại" };
        }

        public ResponseDTO GetPublisherById(int id)
        {
            var publisher = _publisherRepository.GetPublisherById(id);
            if (publisher == null)
                return new ResponseDTO()
                {
                    Code = 400,
                    Message = "Publisher không tồn tại"
                };
            return new ResponseDTO()
            {
                Data = _mapper.Map<PublisherDTO>(publisher)
            };
        }

        public ResponseDTO GetPublishers(int? page = 1, int? pageSize = 10, string? key = "", string? sortBy = "ID")
        {
            var publishers = _publisherRepository.GetPublishers(page, pageSize, key, sortBy);
            return new ResponseDTO()
            {
                Data = _mapper.Map<List<PublisherDTO>>(publishers),
                Total = _publisherRepository.Total
            };
        }

        public ResponseDTO UpdatePublisher(int id, string name)
        {
            var publisher = _publisherRepository.GetPublisherById(id);
            if (publisher == null)
                return new ResponseDTO()
                {
                    Code = 400,
                    Message = "Publisher không tồn tại"
                };
            publisher.Update = DateTime.Now;
            publisher.Name = name;
            _publisherRepository.UpdatePublisher(publisher);
            if (_publisherRepository.IsSaveChanges()) return new ResponseDTO() { Message = "Cập nhật thành công" };
            else return new ResponseDTO() { Code = 400, Message = "Cập nhật thất bại" };
        }
    }
}
