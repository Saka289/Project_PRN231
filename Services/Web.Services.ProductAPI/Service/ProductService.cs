﻿using AutoMapper;
using FileUpload;
using FileUpload.Models;
using Newtonsoft.Json;
using Shared.Dtos;
using System.Security.Cryptography;
using Web.Services.ProductAPI.Data;
using Web.Services.ProductAPI.Models;
using Web.Services.ProductAPI.Models.Dto;
using Web.Services.ProductAPI.Repository;
using Web.Services.ProductAPI.Repository.IRepository;
using Web.Services.ProductAPI.Service.IService;

namespace Web.Services.ProductAPI.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductImageRepository _productImageRepository;
        private readonly IOrderService _orderService;
        private readonly AppDbContext _context;
        protected ResponseDto _response;
        private IMapper _mapper;
        public ProductService(IProductRepository productRepository, IMapper mapper, IProductImageRepository productImageRepository, IOrderService orderService, AppDbContext con)
        {
            _productRepository = productRepository;
            _productImageRepository = productImageRepository;
            _mapper = mapper;
            _response = new ResponseDto();
            _orderService = orderService;
            _context = con;
        }
        public async Task<ResponseDto> Add(ProductDtoForCreateAndUpdate model)
        {
            try
            {
                using var memoryStr = new MemoryStream();
                await model.Image.CopyToAsync(memoryStr);
                var fileExtension = Path.GetExtension(model.Image.FileName); // .jpg
                var objName = $"{Guid.NewGuid()}-{Path.GetFileNameWithoutExtension(model.Image.FileName)}{fileExtension}";
                var s3Obj = new S3Object
                {
                    BucketName = "productimages",
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


                Product obj = new Product();
                obj.Title = model.Title;
                obj.Price = model.Price;
                obj.ProductCode = model.ProductCode;
                obj.CategoryId = model.CategoryId;
                obj.Description = model.Description;
                obj.Status = model.Status;
                obj.Image = Convert.ToString(stringImage.Result);
                _productRepository.AddAsync(obj);
                _productRepository.Save();

                // sau khi create product và ảnh bên productimage cloud
                // cập nhật thêm vào bảng productimage in db
                ProductImageDto productImageDto = new ProductImageDto
                {
                    Image = Convert.ToString(stringImage.Result),
                    IsDefault = true,   
                    ProductId = obj.Id,
                };
                _productImageRepository.AddAsync(productImageDto);
                _productImageRepository.Save();

                _response.Result = "";
                _response.Message = "Add product successfully !";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;

        }

        public ResponseDto Delete(int pId)
        {
            try
            {
                _productRepository.Remove(pId);
                _productRepository.Save();
                _response.Result = true;
                _response.Message = "Delete product successfully !";
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        public ResponseDto DeleteSoft(int pId)
        {
            try
            {
                _productRepository.RemoveSoft(pId);
                _productRepository.Save();
                _response.Result = true;
                _response.Message = "Delete product successfully";
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        public async Task<ResponseDto> GetAllProducts()
        {
            try
            {
                IEnumerable<Product> objList = await _productRepository.GetAllAsyns();
                _response.Result = _mapper.Map<IEnumerable<ProductDto>>(objList);
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }
        public async Task<ResponseDto> SearchProductInShopPage(ProductSearchDto model)
        {
            try
            {
                IEnumerable<Product> objList = await _productRepository.SearchInShopPageAsyns(model);
                _response.Result = _mapper.Map<IEnumerable<ProductDto>>(objList);
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        public ResponseDto GetProductById(int pId)
        {
            try
            {
                var objFind = _productRepository.GetByIdAsyns(pId);
                _response.Result = _mapper.Map<ProductDto>(objFind);
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        public async Task<ResponseDto> SearchProducts(string seachValue)
        {
            try
            {
                IEnumerable<Product> objList = await _productRepository.SearchAsyns(seachValue);
                _response.Result = _mapper.Map<IEnumerable<ProductDto>>(objList);
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        public async Task<ResponseDto> Update(ProductDtoForCreateAndUpdate model)
        {
            try
            {
                var obj = _productRepository.GetByIdAsyns(model.Id);
                obj.Title = model.Title;
                obj.Price = model.Price;
                obj.ProductCode = model.ProductCode;
                obj.CategoryId = model.CategoryId;
                obj.Description = model.Description;
                obj.Status = model.Status;


                if (model.Image == null)
                {
                    // nếu không chọn ảnh khách thì giữ nguyên ảnh hiện tại
                }
                else
                {
                    // cắt chuỗi để lấy tên ảnh 
                    string fileName = obj.Image.Substring(obj.Image.LastIndexOf('/') + 1);
                    // xóa ảnh ở cloudfare
                    FileUploadFunction f = new FileUploadFunction();
                    await f.DeleteFileAsync("productimages", fileName);
                    // upload lại ảnh lên cloudfare 
                    using var memoryStr = new MemoryStream();
                    await model.Image.CopyToAsync(memoryStr);
                    var fileExtension = Path.GetExtension(model.Image.FileName); // .jpg
                    var objName = $"{Guid.NewGuid()}-{Path.GetFileNameWithoutExtension(model.Image.FileName)}{fileExtension}";
                    var s3Obj = new S3Object
                    {
                        BucketName = "productimages",
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

                    obj.Image = Convert.ToString(stringImage);
                }


                _productRepository.UpdateAsync(obj);
                _productRepository.Save();
                _response.Result = true;
                _response.Message = "Update product successfully !!!";
            }
            catch (Exception ex)
            {

                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        public async Task<ResponseDto> GetAllProductByCateAsync(int id)
        {
            try
            {
                IEnumerable<Product> objList = await _productRepository.GetAllByCateAsyns(id);
                _response.Result = _mapper.Map<IEnumerable<ProductDto>>(objList);
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        public async Task<ResponseDto> GetBestSeller()
        {
            List<ProductDto> productDtos = new List<ProductDto>();
            try
            {
                var bestSaler = await _orderService.GetBestSellers();
                if(bestSaler != null)
                {
                    foreach (var proId in bestSaler.ToList())
                    {
                        productDtos.Add(_mapper.Map<ProductDto>(_context.Products.FirstOrDefault(ite => ite.Id == int.Parse(proId.ProductId))));
                    }
                }
                _response.Result = productDtos;
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
