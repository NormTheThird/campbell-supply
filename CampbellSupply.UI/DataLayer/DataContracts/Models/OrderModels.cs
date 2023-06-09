using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CampbellSupply.DataLayer.DataContracts.Models
{
    [DataContract]
    public class ShoppingCartModel : ProductQuickModel
    {
        public ShoppingCartModel()
        {
            this.Id = Guid.Empty;
            this.Quantity = 0;
            this.ShippingPrice = 0.0m;
        }

        [DataMember(IsRequired = true)]
        public Guid Id { get; set; }
        [DataMember(IsRequired = true)]
        public int Quantity { get; set; }
        [DataMember(IsRequired = true)]
        public decimal ShippingPrice { get; set; }
    }

    [DataContract]
    public class CouponModel : BaseModel
    {
        public CouponModel()
        {
            this.Code = string.Empty;
            this.Value = 0.0m;
            this.IsPercent = false;
            this.IsAmount = false;
        }

        [DataMember(IsRequired = true)]
        public string Code { get; set; }
        [DataMember(IsRequired = true)]
        public decimal Value { get; set; }
        [DataMember(IsRequired = true)]
        public bool IsPercent { get; set; }
        [DataMember(IsRequired = true)]
        public bool IsAmount { get; set; }
    }

    [DataContract]
    public class AdminCouponModel : CouponModel
    {
        public AdminCouponModel()
        {
            this.Description = string.Empty;
            this.IsOneTime = false;
            this.IsActive = false;
            this.DateActiveChanged = DateTime.MinValue;
            this.DateCreated = DateTime.MinValue;
        }

        [DataMember(IsRequired = true)]
        public string Description { get; set; }
        [DataMember(IsRequired = true)]
        public bool IsOneTime { get; set; }
        [DataMember(IsRequired = true)]
        public bool IsActive { get; set; }
        [DataMember(IsRequired = true)]
        public DateTime DateActiveChanged { get; set; }
        [DataMember(IsRequired = true)]
        public DateTime DateCreated { get; set; }

    }

    [DataContract]
    public class OrderModel : BaseModel
    {
        public OrderModel()
        {
            this.AccountId = null;
            this.Email = string.Empty;
            this.ShippingAddress = new AddressModel();
            this.BillingAddress = new AddressModel();
            this.CouponId = null;
            this.Number = string.Empty;
            this.PaymentTransactionId = string.Empty;
            this.PaymentExpiration = string.Empty;
            this.PaymentLast4 = 0;
            this.SubTotal = 0.0m;
            this.ShippingTotal = 0.0m;
            this.SalesTax = 0.0m;
            this.OrderItems = new List<OrderItemModel>();
        }

        [DataMember(IsRequired = true)]
        public Guid? AccountId { get; set; }
        [DataMember(IsRequired = true)]
        public string Email { get; set; }
        [DataMember(IsRequired = true)]
        public AddressModel ShippingAddress { get; set; }
        [DataMember(IsRequired = true)]
        public AddressModel BillingAddress { get; set; }
        [DataMember(IsRequired = true)]
        public Guid? CouponId { get; set; }
        [DataMember(IsRequired = true)]
        public string Number { get; set; }
        [DataMember(IsRequired = true)]
        public string PaymentTransactionId { get; set; }
        [DataMember(IsRequired = true)]
        public string PaymentExpiration { get; set; }
        [DataMember(IsRequired = true)]
        public int PaymentLast4 { get; set; }
        [DataMember(IsRequired = true)]
        public decimal SubTotal { get; set; }
        [DataMember(IsRequired = true)]
        public decimal ShippingTotal { get; set; }
        [DataMember(IsRequired = true)]
        public decimal SalesTax { get; set; }
        [DataMember(IsRequired = true)]
        public List<OrderItemModel> OrderItems { get; set; }
    }

    [DataContract]
    public class FullOrderModel : OrderModel
    {
        public FullOrderModel()
        {
            this.Account = new AccountModel();
            this.Coupon = new CouponModel();
            this.IsPaid = false;
            this.IsProcessed = false;
            this.IsShipped = false;
            this.DatePaid = null;
            this.DateProcessed = null;
            this.DateShipped = null;
            this.DateCreated = DateTime.MinValue;
        }

        [DataMember(IsRequired = true)]
        public AccountModel Account { get; set; }
        [DataMember(IsRequired = true)]
        public CouponModel Coupon { get; set; }
        [DataMember(IsRequired = true)]
        public bool IsPaid { get; set; }
        [DataMember(IsRequired = true)]
        public bool IsProcessed { get; set; }
        [DataMember(IsRequired = true)]
        public bool IsShipped { get; set; }
        [DataMember(IsRequired = true)]
        public DateTime? DatePaid { get; set; }
        [DataMember(IsRequired = true)]
        public DateTime? DateProcessed { get; set; }
        [DataMember(IsRequired = true)]
        public DateTime? DateShipped { get; set; }
        [DataMember(IsRequired = true)]
        public DateTime DateCreated { get; set; }

    }

    [DataContract]
    public class OrderItemModel : ProductQuickModel
    {
        public OrderItemModel()
        {
            this.Id = Guid.Empty;
            this.ShippingPrice = 0.0m;
            this.Quantity = 0;
        }

        [DataMember(IsRequired = true)]
        public Guid Id { get; set; }
        [DataMember(IsRequired = true)]
        public decimal ShippingPrice { get; set; }
        [DataMember(IsRequired = true)]
        public int Quantity { get; set; }
    }

    [DataContract]
    public class OrderHistoryUserModel
    {
        public OrderHistoryUserModel()
        {
            this.OrderId = Guid.Empty;
            this.OrderNumber = string.Empty;
            this.OrderAmount = 0.0m;
            this.OrderDate = DateTime.MinValue;
        }

        [DataMember(IsRequired = true)]
        public Guid OrderId { get; set; }
        [DataMember(IsRequired = true)]
        public string OrderNumber { get; set; }
        [DataMember(IsRequired = true)]
        public decimal OrderAmount { get; set; }
        [DataMember(IsRequired = true)]
        public DateTime OrderDate { get; set; }
    }

    [DataContract]
    public class OrderAdminHistoryUserModel : OrderHistoryUserModel
    {
        public OrderAdminHistoryUserModel()
        {
            this.FirstName = string.Empty;
            this.LastName = string.Empty;
        }

        [DataMember(IsRequired = true)]
        public string FirstName { get; set; }
        [DataMember(IsRequired = true)]
        public string LastName { get; set; }
    }
}