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
        public int Skip { get;  set; }
        public int TotalPages { get;  set; }
        public int PageSize { get;  set; }
        public int TotalCount { get;  set; }
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
}

