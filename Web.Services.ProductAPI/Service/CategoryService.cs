using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using Amazon.S3;
using AutoMapper;
using FileUpload;
using FileUpload.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Shared.Dtos;
using System;
using Web.Services.ProductAPI.Models;
using Web.Services.ProductAPI.Models.Dto;
using Web.Services.ProductAPI.Repository.IRepository;
using Web.Services.ProductAPI.Service.IService;

namespace Web.Services.ProductAPI.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        protected ResponseDto _response;
        private IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _response = new ResponseDto();
            _mapper = mapper;
        }

        public async Task<ResponseDto> Add(CategoryDtoForCreateAndUpdate cateDto)
        {
            try
            {
                using var memoryStr = new MemoryStream();
                cateDto.Image.CopyToAsync(memoryStr);
                var fileExtension = Path.GetExtension(cateDto.Image.FileName); // .jpg
                var objName = $"{Guid.NewGuid()}-{Path.GetFileNameWithoutExtension(cateDto.Image.FileName)}{fileExtension}";
                var s3Obj = new S3Object
                {
                    BucketName = "categoryimages",
                    InputStream = memoryStr,
                    Name = objName,
                };
                var cred = new AwsCredentials()
                {
                    AwsKey = "187a838703ea90ac7984002e8c0f4425",
                    AwsSecretKey = "044c4cbdd24628a314a263bff4fb59fcea8ceb64e49d92240c22986796503f07",

                };
                FileUploadFunction fuf = new FileUploadFunction();
                var stringImage = fuf.UploadImageAsync(s3Obj, cred);
                Category obj = new Category();
                obj.Name = cateDto.Name;
                obj.Status = cateDto.Status;
                obj.Image = Convert.ToString(stringImage.Result);

                _categoryRepository.AddAsync(obj);
                _categoryRepository.Save();

                _response.Result = "";
                _response.Message = "Add category successfully !";
            }
            catch (Exception ex)
            {

                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        public ResponseDto Delete(int cateId)
        {
            try
            {
                _categoryRepository.Remove(cateId);
                _categoryRepository.Save();
                _response.Result = true;
                _response.Message = "Delete category successfully !";

            }
            catch (Exception ex)
            {

                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        public ResponseDto DeleteSoft(int cateId)
        {
            try
            {
                _categoryRepository.RemoveSoft(cateId);
                _categoryRepository.Save();
                _response.Result = true;
                _response.Message = "Delete category successfully";
            }
            catch (Exception ex)
            {

                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        public async Task<ResponseDto> GetAllCategories()
        {
            try
            {
                IEnumerable<Category> objList = await _categoryRepository.GetAllAsyns();
                _response.Result = _mapper.Map<IEnumerable<CategoryDto>>(objList);
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        public ResponseDto GetCategoryById(int cateId)
        {
            try
            {
                var objFind = _categoryRepository.GetByIdAsyns(cateId);
                _response.Result = _mapper.Map<CategoryDto>(objFind);
            }
            catch (Exception ex)
            {

                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        public async Task<ResponseDto> Update(CategoryDtoForCreateAndUpdate cateDto)
        {
            try
            {
                var obj = _categoryRepository.GetByIdAsyns(cateDto.Id);
                obj.Id = cateDto.Id;
                obj.Name = cateDto.Name;
                obj.Status = cateDto.Status;
                if (cateDto.Image == null)
                {
                    // nếu không chọn ảnh khách thì giữ nguyên ảnh hiện tại
                }
                else
                {
                    // cắt chuỗi để lấy tên ảnh 
                    string fileName = obj.Image.Substring(obj.Image.LastIndexOf('/') + 1);
                    // xóa ảnh ở cloudfare
                    FileUploadFunction f = new FileUploadFunction();
                    f.DeleteFileAsync("categoryimages", fileName);
                    // upload lại ảnh lên cloudfare 
                    using var memoryStr = new MemoryStream();
                    cateDto.Image.CopyToAsync(memoryStr);
                    var fileExtension = Path.GetExtension(cateDto.Image.FileName); // .jpg
                    var objName = $"{Guid.NewGuid()}-{Path.GetFileNameWithoutExtension(cateDto.Image.FileName)}{fileExtension}";
                    var s3Obj = new S3Object
                    {
                        BucketName = "categoryimages",
                        InputStream = memoryStr,
                        Name = objName,
                    };
                    var cred = new AwsCredentials()
                    {
                        AwsKey = "187a838703ea90ac7984002e8c0f4425",
                        AwsSecretKey = "044c4cbdd24628a314a263bff4fb59fcea8ceb64e49d92240c22986796503f07",

                    };
                    FileUploadFunction fuf = new FileUploadFunction();
                    var stringImage = fuf.UploadImageAsync(s3Obj, cred);


                    obj.Image = Convert.ToString(stringImage.Result);
                }

                _categoryRepository.UpdateAsync(obj);
                _categoryRepository.Save();
                _response.Result = true;
                _response.Message = "Update category successfully !!!";
            }
            catch (Exception ex)
            {

                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
    }
}
