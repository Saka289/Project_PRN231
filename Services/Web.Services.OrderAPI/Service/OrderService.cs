using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;
using Shared.Enums;
using Stripe;
using System.Collections.Generic;
using Web.Services.OrderAPI.Data;
using Web.Services.OrderAPI.Models;
using Web.Services.OrderAPI.Models.Dto;
using Web.Services.OrderAPI.Repository.IRepository;
using Web.Services.OrderAPI.Service.IService;
using static Shared.Enums.SD;

namespace Web.Services.OrderAPI.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IInventoryService _inventoryService;
        private readonly IVietQrService _vietQrService;
        private readonly AppDbContext _context;
        protected ResponseDto _response;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IMapper mapper, IProductService productService, ICategoryService categoryService, IInventoryService inventoryService, IVietQrService vietQrService, IOrderDetailRepository orderDetailRepository, AppDbContext context)
        {
            _orderRepository = orderRepository;
            _productService = productService;
            _categoryService = categoryService;
            _inventoryService = inventoryService;
            _vietQrService = vietQrService;
            _response = new ResponseDto();
            _mapper = mapper;
            _orderDetailRepository = orderDetailRepository;
            _context = context;
        }

        public async Task<ResponseDto> CreateOrder(CartDto cartDto)
        {
            try
            {
                var productList = cartDto.CartDetails.Select(p => new ProductRequest
                {
                    ProductId = p.ProductId,
                    Quantity = p.Quantity,
                }).ToList();

                var checkInventory = await _inventoryService.IsInStock(productList);
                if (checkInventory.Count() != productList.Count)
                {
                    _response.Result = false;
                    _response.IsSuccess = false;
                    _response.Message = "Order creation failed due to out of stock !!!";
                    return _response;
                }

                OrderDto orderDto = _mapper.Map<OrderDto>(cartDto.CartHeader);
                orderDto.OrderId = Guid.Empty;
                orderDto.OrderDate = DateTime.Now;
                orderDto.ShippedDate = DateTime.Now.AddDays(5);
                orderDto.RequiredDate = DateTime.Now.AddDays(7);
                orderDto.PaymentStatus = PaymentStatus.NOT_STARTED.ToString();
                orderDto.OrderDetails = _mapper.Map<IEnumerable<OrderDetailDto>>(cartDto.CartDetails).Select(orderDetails =>
                {
                    orderDetails.OrderDetailId = Guid.Empty;
                    return orderDetails;
                });

                Order orderCreated = await _orderRepository.CreateOrder(_mapper.Map<Order>(orderDto));
                _orderRepository.SaveChanges();
                var inventoryUpdate = new UpdateInvensRequestDto()
                {
                    products = productList,
                    status = "Order Success"
                };
                await _inventoryService.UpdateInventory(inventoryUpdate);
                _response.Result = _mapper.Map<OrderDto>(orderCreated);
                _response.Message = "Create Order Successfully !!!";
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        public async Task<ResponseDto> GetOrders()
        {
            try
            {
                var result = await _orderRepository.GetOrders();
                var orderDtos = _mapper.Map<IEnumerable<OrderDto>>(result);
                foreach (var itemOrder in orderDtos)
                {
                    foreach (var itemOrderDetails in itemOrder.OrderDetails)
                    {
                        var product = await _productService.GetProducts();
                        if (product != null)
                        {
                            itemOrderDetails.Product = product.FirstOrDefault(p => p.Id == itemOrderDetails.ProductId);
                        }
                    }
                }
                _response.Result = orderDtos;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        public async Task<ResponseDto> GetOrdersByUserId(string userId)
        {
            try
            {
                var result = await _orderRepository.GetOrdersByUserId(userId);
                if (result == null)
                {
                    _response.Message = "Not Found !!!";
                    _response.IsSuccess = false;
                    _response.Result = false;
                    return _response;
                }
                var orderDtos = _mapper.Map<IEnumerable<OrderDto>>(result);
                foreach (var itemOrder in orderDtos)
                {
                    foreach (var itemOrderDetails in itemOrder.OrderDetails.Where(od => od.OrderId == itemOrder.OrderId).ToList())
                    {
                        var product = await _productService.GetProducts();
                        if (product != null)
                        {
                            itemOrderDetails.Product = product.FirstOrDefault(p => p.Id == itemOrderDetails.ProductId);
                        }
                    }
                }
                _response.Result = _mapper.Map<IEnumerable<OrderDto>>(orderDtos);
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        public async Task<ResponseDto> UpdateStatus(string orderId, string status)
        {
            try
            {
                if (string.IsNullOrEmpty(status))
                {
                    _response.Message = "Not Found !!!";
                    _response.IsSuccess = false;
                    _response.Result = false;
                    return _response;
                }
                Order orderHeader = _context.Orders.First(u => u.OrderId.ToString() == orderId);
                if (status == SD.PaymentStatus.REFUND.ToString())
                {
                    //we will give refund
                    var options = new RefundCreateOptions
                    {
                        Reason = RefundReasons.RequestedByCustomer,
                        PaymentIntent = orderHeader.PaymentIntentId
                    };
                    var service = new RefundService();
                    Refund refund = service.Create(options);
                }
                var result = await _orderRepository.UpdateStatus(orderId, status);
                _orderRepository.SaveChanges();
                _response.Result = result;
                _response.Message = "Update Status Successfully !!!";
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        public async Task<ResponseDto> SearchOrder(string orderId)
        {
            try
            {
                if (string.IsNullOrEmpty(orderId))
                {
                    _response.Message = "Not Found !!!";
                    _response.IsSuccess = false;
                    _response.Result = false;
                    return _response;
                }
                var result = await _orderRepository.SearchOrder(orderId);
                var orderDtos = _mapper.Map<OrderDto>(result);
                foreach (var itemOrder in orderDtos.OrderDetails)
                {
                    var product = await _productService.GetProducts();
                    if (product != null)
                    {
                        itemOrder.Product = product.FirstOrDefault(p => p.Id == itemOrder.ProductId);
                    }
                }
                _response.Result = _mapper.Map<OrderDto>(orderDtos);
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        public async Task<ResponseDto> GenerateQR(string orderId, decimal amount)
        {
            try
            {
                var result = await _vietQrService.GenerateQR(orderId, amount);
                if (result.Data == null)
                {
                    _response.Result = false;
                    _response.Message = result.Desc;
                    _response.IsSuccess = false;
                    return _response;
                }
                _response.Message = result.Desc;
                _response.Result = result.Data.QrDataUrl;
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
            try
            {
                var result = await _orderDetailRepository.GetOrderDetailRepositoryAsync();
                if (result.Any())
                {
                    _response.Result = result;
                    _response.Message = "true";
                    _response.IsSuccess = true;
                    return _response;
                }
                _response.Message = "Not Have";
                _response.IsSuccess = false;
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
