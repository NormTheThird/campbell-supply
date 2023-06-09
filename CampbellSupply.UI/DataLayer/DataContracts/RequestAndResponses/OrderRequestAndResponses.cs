using CampbellSupply.DataLayer.DataContracts.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CampbellSupply.DataLayer.DataContracts.RequestAndResponses
{
    public class GetShoppingCartItemsRequest : BaseRequest
    {
        public GetShoppingCartItemsRequest()
        {
            this.AccountId = null;
            this.SessionId = string.Empty;
        }

        [DataMember(IsRequired = true)]
        public Guid? AccountId { get; set; }
        [DataMember(IsRequired = true)]
        public string SessionId { get; set; }
    }

    public class GetShoppingCartItemsResponse : BaseResponse
    {
        public GetShoppingCartItemsResponse()
        {
            this.Items = new List<ShoppingCartModel>();
        }

        [DataMember(IsRequired = true)]
        public List<ShoppingCartModel> Items { get; set; }
    }


    public class SaveShoppingCartItemRequest : BaseRequest
    {
        public SaveShoppingCartItemRequest()
        {
            this.AccountId = null;
            this.SessionId = string.Empty;
            this.ProductId = Guid.Empty;
            this.Quantity = 0;
        }

        [DataMember(IsRequired = true)]
        public Guid? AccountId { get; set; }
        [DataMember(IsRequired = true)]
        public string SessionId { get; set; }
        [DataMember(IsRequired = true)]
        public Guid ProductId { get; set; }
        [DataMember(IsRequired = true)]
        public int Quantity { get; set; }
    }

    public class SaveShoppingCartItemResponse : BaseResponse
    {
        public SaveShoppingCartItemResponse()
        {
            this.Items = new List<ShoppingCartModel>();
        }

        [DataMember(IsRequired = true)]
        public List<ShoppingCartModel> Items { get; set; }
    }


    public class AdjustShoppingCartItemRequest : BaseRequest
    {
        public AdjustShoppingCartItemRequest()
        {
            this.AccountId = null;
            this.SessionId = string.Empty;
            this.Id = Guid.Empty;
            this.Quantity = 0;
        }

        [DataMember(IsRequired = true)]
        public Guid? AccountId { get; set; }
        [DataMember(IsRequired = true)]
        public string SessionId { get; set; }
        [DataMember(IsRequired = true)]
        public Guid Id { get; set; }
        [DataMember(IsRequired = true)]
        public int Quantity { get; set; }
    }

    public class AdjustShoppingCartItemResponse : BaseResponse
    {
        public AdjustShoppingCartItemResponse()
        {
            this.Items = new List<ShoppingCartModel>();
        }

        [DataMember(IsRequired = true)]
        public List<ShoppingCartModel> Items { get; set; }
    }


    public class GetCouponRequest : BaseRequest
    {
        public GetCouponRequest()
        {
            this.Code = string.Empty;
        }

        [DataMember(IsRequired = true)]
        public string Code { get; set; }
    }

    public class GetCouponResponse : BaseResponse
    {
        public GetCouponResponse()
        {
            this.Coupon = new CouponModel();
        }

        [DataMember(IsRequired = true)]
        public CouponModel Coupon { get; set; }
    }

    public class GetCouponsRequest : BaseRequest { }

    public class GetCouponsResponse : BaseResponse
    {
        public GetCouponsResponse()
        {
            this.Coupons = new List<AdminCouponModel>();
        }

        [DataMember(IsRequired = true)]
        public List<AdminCouponModel> Coupons { get; set; }
    }

    public class SaveCouponRequest : BaseRequest
    {
        public SaveCouponRequest()
        {
            this.Coupon = new AdminCouponModel();
        }

        [DataMember(IsRequired = true)]
        public AdminCouponModel Coupon { get; set; }
    }

    public class SaveCouponResponse : BaseResponse { }


    public class GetPreviousAddressRequest : BaseRequest
    {
        public GetPreviousAddressRequest()
        {
            this.AccountId = Guid.Empty;
        }

        [DataMember(IsRequired = true)]
        public Guid AccountId { get; set; }
    }

    public class GetPreviousAddressResponse : BaseResponse
    {
        public GetPreviousAddressResponse()
        {
            this.PrevShippingAddress = new AddressModel();
            this.PrevBillingAddress = new AddressModel();
        }

        [DataMember(IsRequired = true)]
        public AddressModel PrevShippingAddress { get; set; }
        [DataMember(IsRequired = true)]
        public AddressModel PrevBillingAddress { get; set; }
    }


    public class GetOrderRequest : BaseRequest
    {
        public GetOrderRequest()
        {
            this.OrderNumber = string.Empty;
        }

        [DataMember(IsRequired = true)]
        public string OrderNumber { get; set; }
    }

    public class GetOrderResponse : BaseResponse
    {
        public GetOrderResponse()
        {
            this.Order = new FullOrderModel();
        }

        [DataMember(IsRequired = true)]
        public FullOrderModel Order { get; set; }
    }


    public class GetPlaceOrderRequest : BaseRequest
    {
        public GetPlaceOrderRequest()
        {
            this.Order = new OrderModel();
            this.CCNumber = string.Empty;
            this.ExpMonth = 0;
            this.ExpYear = 0;
            this.SessionId = string.Empty;
            this.CCV = 0;
        }

        [DataMember(IsRequired = true)]
        public OrderModel Order { get; set; }
        [DataMember(IsRequired = true)]
        public string CCNumber { get; set; }
        [DataMember(IsRequired = true)]
        public int ExpMonth { get; set; }
        [DataMember(IsRequired = true)]
        public int ExpYear { get; set; }
        [DataMember(IsRequired = true)]
        public string SessionId { get; set; }
        [DataMember(IsRequired = true)]
        public int CCV { get; set; }

    }

    public class GetPlaceOrderResponse : BaseResponse { }


    public class GetOrderHistoryByPageRequest : BaseRequest
    {
        public GetOrderHistoryByPageRequest()
        {
            this.AccountId = Guid.Empty;
            this.PageIndex = 0;
        }

        [DataMember(IsRequired = true)]
        public Guid AccountId { get; set; }
        [DataMember(IsRequired = true)]
        public int PageIndex { get; set; }
    }

    public class GetOrderHistoryByPageResponse : BaseResponse
    {
        public GetOrderHistoryByPageResponse()
        {
            this.Orders = new List<OrderHistoryUserModel>();
            this.RecordCount = 0;
        }

        [DataMember(IsRequired = true)]
        public List<OrderHistoryUserModel> Orders { get; set; }
        [DataMember(IsRequired = true)]
        public int RecordCount { get; set; }
    }

    public class GetOrderHistoryDetailRequest : BaseRequest
    {
        public GetOrderHistoryDetailRequest()
        {
            this.OrderId = Guid.Empty;
            this.AccountId = Guid.Empty;
        }

        [DataMember(IsRequired = true)]
        public Guid OrderId { get; set; }
        [DataMember(IsRequired = true)]
        public Guid AccountId { get; set; }
    }

    public class GetOrderHistoryDetailResponse : BaseResponse
    {
        public GetOrderHistoryDetailResponse()
        {
            this.Order = new FullOrderModel();
        }

        [DataMember(IsRequired = true)]
        public FullOrderModel Order { get; set; }
    }

    public class SearchOrderHistoryDetailRequest : BaseRequest
    {
        public SearchOrderHistoryDetailRequest()
        {
            this.OrderNumber = string.Empty;
            this.AccountId = Guid.Empty;
        }

        [DataMember(IsRequired = true)]
        public string OrderNumber { get; set; }
        [DataMember(IsRequired = true)]
        public Guid AccountId { get; set; }
    }

    public class SearchOrderHistoryDetailResponse : BaseResponse
    {
        public SearchOrderHistoryDetailResponse()
        {
            this.Order = new FullOrderModel();
        }

        [DataMember(IsRequired = true)]
        public FullOrderModel Order { get; set; }
    }


    public class GetAdminOrderHistoryByPageRequest : BaseRequest
    {
        public GetAdminOrderHistoryByPageRequest()
        {
            this.PageIndex = 0;
        }

        [DataMember(IsRequired = true)]
        public int PageIndex { get; set; }
    }

    public class GetAdminOrderHistoryByPageResponse : BaseResponse
    {
        public GetAdminOrderHistoryByPageResponse()
        {
            this.Orders = new List<OrderAdminHistoryUserModel>();
            this.RecordCount = 0;
        }

        [DataMember(IsRequired = true)]
        public List<OrderAdminHistoryUserModel> Orders { get; set; }
        [DataMember(IsRequired = true)]
        public int RecordCount { get; set; }
    }

    public class GetAdminOrderHistoryDetailRequest : BaseRequest
    {
        public GetAdminOrderHistoryDetailRequest()
        {
            this.OrderId = Guid.Empty;
        }

        [DataMember(IsRequired = true)]
        public Guid OrderId { get; set; }
    }

    public class GetAdminOrderHistoryDetailResponse : BaseResponse
    {
        public GetAdminOrderHistoryDetailResponse()
        {
            this.Order = new FullOrderModel();
        }

        [DataMember(IsRequired = true)]
        public FullOrderModel Order { get; set; }
    }

    public class SearchAdminOrderHistoryDetailRequest : BaseRequest
    {
        public SearchAdminOrderHistoryDetailRequest()
        {
            this.OrderNumber = string.Empty;
        }

        [DataMember(IsRequired = true)]
        public string OrderNumber { get; set; }
    }

    public class SearchAdminOrderHistoryDetailResponse : BaseResponse
    {
        public SearchAdminOrderHistoryDetailResponse()
        {
            this.Order = new FullOrderModel();
        }

        [DataMember(IsRequired = true)]
        public FullOrderModel Order { get; set; }
    }
}