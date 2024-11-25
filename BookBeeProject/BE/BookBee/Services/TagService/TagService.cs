using AutoMapper;

using BookBee.DTOs.Response;
using BookBee.DTOs.Tag;
using BookBee.Model;
using BookBee.Persistences;
using BookBee.Persistences.Repositories.CategoryRepository;

namespace BookBee.Services.CategoryService
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;
        public TagService(ITagRepository tagRepository, IMapper mapper)
        {
            _tagRepository = tagRepository;
            _mapper = mapper;
        }


        public ResponseDTO CreateCategorys(string name, string image)
        {
            var tag = new Tag { Name = name, Image = image};
            _tagRepository.CreateCategorys(tag);
            if (_tagRepository.IsSaveChanges()) return new ResponseDTO() { Message = "Tạo thành công" };
            else return new ResponseDTO() { Code = 400, Message = "Tạo thất bại" };

        }

        public ResponseDTO DeleteCategorys(int id)
        {
            var tag = _tagRepository.GetCategorysById(id);
            if (tag == null)
                return new ResponseDTO()
                {
                    Code = 400,
                    Message = "Category không tồn tại"
                };
            tag.IsDeleted = true;
            _tagRepository.UpdateCategorys(tag);
            if (_tagRepository.IsSaveChanges()) return new ResponseDTO() { Message = "Cập nhật thành công" };
            else return new ResponseDTO() { Code = 400, Message = "Cập nhật thất bại" };
        }

        public ResponseDTO GetCategorys(int? page = 1, int? pageSize = 10, string? key = "", string? sortBy = "ID")
        {
            var tags = _tagRepository.GetCategorys(page, pageSize, key, sortBy);
            return new ResponseDTO()
            {
                Data = _mapper.Map<List<TagDTO>>(tags),
                Total = _tagRepository.Total
            };
        }

        public ResponseDTO GetCategorysById(int id)
        {
            var tag = _tagRepository.GetCategorysById(id);
            if (tag == null)
                return new ResponseDTO()
                {
                    Code = 400,
                    Message = "Category không tồn tại"
                };
            return new ResponseDTO()
            {
                Data = _mapper.Map<TagDTO>(tag)
            };
        }

        public ResponseDTO UpdateCategorys(int id, TagRequestDTO tagRequestDto)
        {
            var tag = _tagRepository.GetCategorysById(id);
            if (tag == null)
                return new ResponseDTO()
                {
                    Code = 400,
                    Message = "Category không tồn tại"
                };
            tag.Update = DateTime.Now;
            tag.Name = tagRequestDto.Name ?? tag.Name;
            tag.Image = tagRequestDto.Image ?? tag.Image;
            _tagRepository.UpdateCategorys(tag);
            if (_tagRepository.IsSaveChanges()) return new ResponseDTO() { Message = "Cập nhật thành công" };
            else return new ResponseDTO() { Code = 400, Message = "Cập nhật thất bại" };
        }
    }
}
