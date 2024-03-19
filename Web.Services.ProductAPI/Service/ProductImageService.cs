using AutoMapper;
using FileUpload;
using FileUpload.Models;
using Shared.Dtos;
using System.Net.WebSockets;
using Web.Services.ProductAPI.Models;
using Web.Services.ProductAPI.Models.Dto;
using Web.Services.ProductAPI.Repository;
using Web.Services.ProductAPI.Repository.IRepository;
using Web.Services.ProductAPI.Service.IService;

namespace Web.Services.ProductAPI.Service
{
    public class ProductImageService : IProductImageService
    {
        private readonly IProductImageRepository _repository;
        protected ResponseDto _response;
        private IMapper _mapper;
        private readonly IProductRepository _productRepository;
        public ProductImageService(IProductImageRepository repository,IMapper mapper, IProductRepository productRepository)
        {
            _repository = repository;
            _productRepository = productRepository;
            _response = new ResponseDto();  
            _mapper = mapper;

        }
        public async Task<ResponseDto> ChangeDefaultImage(int id, int pid)
        {
            try
            {
                var listImage = await _repository.GetAllByProductIdAsyns(pid);
                foreach (var img in listImage)
                {
                    img.IsDefault = false;
                }
                _repository.Save();
                _repository.ChangeDefaultImageAsync(id);
                _repository.Save();

                var objImage = _repository.GetByIdAsyns(id);
                // cập nhật lại ảnh ở product
                var productFromDb = _productRepository.GetByIdAsyns(pid);
                if(productFromDb != null && objImage !=null)
                {
                    productFromDb.Image = objImage.Image;
                }
                _productRepository.Save();

                _response.Result = true;
                _response.Message = "Change image default of product successfully";
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        public async Task<ResponseDto> DeleteImageOfProductImageAsync(int id)
        {
            try
            {
                var objInDb = _repository.GetByIdAsyns(id);
                if (objInDb != null)
                {
                    // cắt chuỗi để lấy tên ảnh 
                    string fileName = objInDb.Image.Substring(objInDb.Image.LastIndexOf('/') + 1);
                    // xóa ảnh ở cloudfare
                    FileUploadFunction f = new FileUploadFunction();
                    await f.DeleteFileAsync("productimagetable", fileName);
                    _repository.Remove(id);
                    _repository.Save();
                }
                _response.Result = true;
                _response.Message = "Delete image of product successfully";
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        public async Task<ResponseDto> GetListImageOfProduct(int idProduct)
        {
            try
            {
                IEnumerable<ProductImage> objList = await _repository.GetAllByProductIdAsyns(idProduct);
                _response.Result = _mapper.Map<IEnumerable<ProductImageDto>>(objList);
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        public async Task<ResponseDto> UploadMultiImage(int idProduct, IFormFile[] files)
        {
            try
            {
                var objInDb = _productRepository.GetByIdAsyns(idProduct);
                if (objInDb != null && files.Length > 0)
                {
                    // tim thay product va co chon mot hay nhieu anh
                    foreach (var file in files)
                    {
                        using var memoryStr = new MemoryStream();
                        await file.CopyToAsync(memoryStr);
                        var fileExtension = Path.GetExtension(file.FileName); // .jpg
                        var objName = $"{Guid.NewGuid()}-{Path.GetFileNameWithoutExtension(file.FileName)}{fileExtension}";
                        var s3Obj = new S3Object
                        {
                            BucketName = "productimagetable",
                            InputStream = memoryStr,
                            Name = objName,
                        };
                        var cred = new AwsCredentials()
                        {
                            AwsKey = "187a838703ea90ac7984002e8c0f4425",
                            AwsSecretKey = "044c4cbdd24628a314a263bff4fb59fcea8ceb64e49d92240c22986796503f07",

                        };
                        FileUploadFunction fuf = new FileUploadFunction();
                        var stringImage = await fuf.UploadImageAsync(s3Obj, cred);
                        //add product image
                        ProductImage obj = new ProductImage();
                        obj.ProductId = objInDb.Id;
                        obj.IsDefault = false;
                        obj.Image = Convert.ToString(stringImage);
                        _repository.AddAsync(obj);
                        _repository.Save();
                    }
                    _response.Result = true;
                    _response.Message = "Upload list of image for product successfully";
                }
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }


       
    }
}
