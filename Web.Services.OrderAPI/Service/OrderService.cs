using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;
using Shared.Enums;
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
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IInventoryService _inventoryService;
        protected ResponseDto _response;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IMapper mapper, IProductService productService, ICategoryService categoryService, IInventoryService inventoryService)
        {
            _orderRepository = orderRepository;
            _productService = productService;
            _categoryService = categoryService;
            _inventoryService = inventoryService;
            _response = new ResponseDto();
            _mapper = mapper;
        }

        public async Task<ResponseDto> CreateOrder(CartDto cartDto)
        {
            try
            {
                var productList = cartDto.CartOrderDetails.Select(p => new ProductRequestDto
                {
                    ProductId = p.ProductId,
                    Quantity = p.Quantity,
                }).ToList();

                var checkInventory = await _inventoryService.IsInStock(productList);
                bool flagOrder = checkInventory.All(c => c.isInStock);
                if (flagOrder == false)
                {
                    _response.Result = false;
                    _response.IsSuccess = false;
                    _response.Message = "Order creation failed due to out of stock !!!";
                    return _response;
                }

                OrderDto orderDto = _mapper.Map<OrderDto>(cartDto.CartOrder);
                orderDto.OrderId = Guid.Empty;
                orderDto.OrderDate = DateTime.Now;
                orderDto.ShippedDate = DateTime.Now.AddDays(5);
                orderDto.RequiredDate = DateTime.Now.AddDays(7);
                orderDto.PaymentStatus = PaymentStatus.NOT_STARTED.ToString();
                orderDto.OrderDetails = _mapper.Map<IEnumerable<OrderDetailDto>>(cartDto.CartOrderDetails).Select(orderDetails =>
                {
                    orderDetails.OrderDetailId = Guid.Empty;
                    return orderDetails;
                });

                Order orderCreated = await _orderRepository.CreateOrder(_mapper.Map<Order>(orderDto));
                _orderRepository.SaveChanges();
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
                        itemOrderDetails.Product = await _productService.GetProduct(itemOrderDetails.ProductId);
                        if (itemOrderDetails.Product != null)
                        {
                            itemOrderDetails.Product.Category = await _categoryService.GetCategory(itemOrderDetails.Product.CategoryId);
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
                _response.Result = _mapper.Map<IEnumerable<OrderDto>>(result);
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
                _response.Result = _mapper.Map<OrderDto>(result);
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
