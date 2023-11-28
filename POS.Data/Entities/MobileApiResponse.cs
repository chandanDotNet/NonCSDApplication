using POS.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Entities
{
    public class MobileApiResponse
    {
        public int Id { get; set; }

    }

    public class CustomerResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public CustomerDto Data { get; set; }
    }

    public class NonCSDResponseNameData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public IList<CounterDto> Data { get; set; }
    }

    public class ProductListResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public int Skip { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IList<ProductDto> Data { get; set; }
    }

    public class ProductDetailsResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public ProductDto Data { get; set; }
    }

    public class ProductDetailsRequestData
    {
        public string Id { get; set; }

    }

    public class ProductCategoriesResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public IList<ProductCategoryDto> Data { get; set; }
    }

    public class IUDResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }

    }

    public class CartListResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public int Skip { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IList<CartDto> Data { get; set; }
    }
    public class CustomerAddressListResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public int Skip { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IList<CustomerAddressDto> Data { get; set; }
    }
    public class CustomerAddressResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public CustomerAddressDto Data { get; set; }
    }

    public class WishlistResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public int Skip { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IList<WishlistDto> Data { get; set; }
    }



    public class PaymentCardResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public PaymentCardDto Data { get; set; }
    }

    public class PaymentCardListResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public int Skip { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IList<PaymentCardDto> Data { get; set; }
    }

    public class CustomerProfileResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public CustomerDto Data { get; set; }
    }

    public class CustomerOrderListResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public int Skip { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IList<SalesOrderDto> Data { get; set; }
    }

    public class ReminderListResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public int Skip { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IList<ReminderDto> Data { get; set; }
    }

    public class LogoutResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public string Data { get; set; }
    }



    public class CustomerOrderDetailsResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public SalesOrderDto Data { get; set; }
    }
    public class CustomerOrderSummaryResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public OrderSummary Data { get; set; }
    }

    public class OrderSummary
    {
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal Items { get; set; }
        public decimal DeliveryCharges { get; set; }
    }

    public class OrderNumberResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public string OrderNumber { get; set; }
    }

    public class SendEmailResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }       
    }

    public class BannerListResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public int Skip { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IList<BannerDto> Data { get; set; }
    }

    public class LoginPageBannerListResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public int Skip { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IList<LoginPageBannerDto> Data { get; set; }
    }
}



