using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Controllers.Bases;
using CampbellSupply.Common.Enums;
using CampbellSupply.Common.Helpers;
using CampbellSupply.Common.RequestAndResponses;
using CampbellSupply.Data;
using CampbellSupply.DataLayer.DataContracts.Models;
using CampbellSupply.DataLayer.DataContracts.RequestAndResponses;
using CampbellSupply.Services;
using System;
using System.Configuration;
using System.Linq;
using System.Web.Script.Serialization;

namespace CampbellSupply.DataLayer.DataServices
{
    public static class OrderService
    {
        public static GetShoppingCartItemsResponse GetShoppingCartItems(GetShoppingCartItemsRequest request)
        {
            try
            {
                var response = new GetShoppingCartItemsResponse();
                using (var context = new CampbellSupplyEntities())
                {
                    var items = context.ShoppingCarts.AsNoTracking().Where(sc => (sc.AccountId == request.AccountId && sc.AccountId != null) || sc.SessionId == request.SessionId)
                                                     .Select(sc => new ShoppingCartModel
                                                     {
                                                         Id = sc.Id,
                                                         ProductId = sc.ProductId,
                                                         Name = sc.Product.Name,
                                                         PartNumber = sc.Product.PartNumber,
                                                         Sku = sc.Product.ManufacturerUPC,
                                                         Color = sc.Product.Color == "-1" ? "" : sc.Product.Color,
                                                         Size = sc.Product.Size == "-1" ? "" : sc.Product.Size,
                                                         Description = sc.Product.DescriptionShort,
                                                         Manufacturer = sc.Product.Manufacturer.Name,
                                                         ManufacturerUPC = sc.Product.ManufacturerUPC,
                                                         Brand = sc.Product.Brand,
                                                         Price = (sc.Product.OverridePrice > 0 ? sc.Product.OverridePrice : sc.Product.PurchasePrice) * sc.Quantity,
                                                         URL = sc.Product.ImageURL,
                                                         IsTaxable = sc.Product.IsTaxable,
                                                         Quantity = sc.Quantity,
                                                         ShippingPrice = sc.Product.ShippingPrice * sc.Quantity
                                                     });
                    foreach (var item in items)
                    {
                        if (item.Color.Trim().Equals("-1")) item.Color = "";
                        if (item.Size.Trim().Equals("-1")) item.Size = "";
                    }
                    response.Items = items.ToList();
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "GetShoppingCartItems", Ex = ex });
                return new GetShoppingCartItemsResponse { ErrorMessage = "Unable to get shopping cart items." };
            }
        }

        public static SaveShoppingCartItemResponse SaveShoppingCartItem(SaveShoppingCartItemRequest request)
        {
            try
            {
                var response = new SaveShoppingCartItemResponse();
                using (var context = new CampbellSupplyEntities())
                {
                    var item = context.ShoppingCarts.FirstOrDefault(sc => sc.AccountId == request.AccountId && sc.SessionId == request.SessionId && sc.ProductId == request.ProductId);
                    if (item == null)
                    {
                        item = new ShoppingCart
                        {
                            Id = Guid.NewGuid(),
                            AccountId = request.AccountId,
                            SessionId = request.SessionId,
                            ProductId = request.ProductId,
                            DateCreated = DateTime.Now,
                        };
                        context.ShoppingCarts.Add(item);
                    }
                    item.Quantity += request.Quantity;
                    context.SaveChanges();

                    var getCartResponse = GetShoppingCartItems(new GetShoppingCartItemsRequest { AccountId = request.AccountId, SessionId = request.SessionId });
                    response.Items = getCartResponse.Items;
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "SaveShoppingCartItem", Ex = ex });
                return new SaveShoppingCartItemResponse { ErrorMessage = "Unable to save shopping cart item." };
            }
        }

        public static AdjustShoppingCartItemResponse AdjustShoppingCartItem(AdjustShoppingCartItemRequest request)
        {
            try
            {
                var response = new AdjustShoppingCartItemResponse();
                using (var context = new CampbellSupplyEntities())
                {
                    var item = context.ShoppingCarts.FirstOrDefault(sc => sc.Id == request.Id);
                    if (item == null) throw new ApplicationException("Item does not exist in cart: " + request.Id.ToString());
                    item.Quantity += request.Quantity;
                    if (item.Quantity <= 0) context.ShoppingCarts.Remove(item);
                    context.SaveChanges();

                    var getCartResponse = GetShoppingCartItems(new GetShoppingCartItemsRequest { AccountId = request.AccountId, SessionId = request.SessionId });
                    response.Items = getCartResponse.Items;
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "AdjustShoppingCartItem", Ex = ex });
                return new AdjustShoppingCartItemResponse { ErrorMessage = "Unable to adjust shopping cart item." };
            }
        }


        public static GetCouponResponse GetCoupon(GetCouponRequest request)
        {
            try
            {
                var response = new GetCouponResponse();
                using (var context = new CampbellSupplyEntities())
                {
                    var coupon = context.Coupons.FirstOrDefault(c => c.Code.Trim().Equals(request.Code, StringComparison.InvariantCultureIgnoreCase) && c.IsActive);
                    if (coupon == null) throw new ApplicationException("Unable to retrieve code: " + request.Code);
                    response.Coupon = new CouponModel { Id = coupon.Id, Code = coupon.Code, Value = coupon.Value, IsAmount = coupon.IsAmount, IsPercent = coupon.IsPercent };
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "GetCoupon", Ex = ex });
                return new GetCouponResponse { ErrorMessage = "Unable to get coupon." };
            }
        }

        public static GetCouponsResponse GetCoupons(GetCouponsRequest request)
        {
            try
            {
                var response = new GetCouponsResponse();
                using (var context = new CampbellSupplyEntities())
                {
                    var coupons = context.Coupons.AsNoTracking().OrderBy(c => c.DateCreated)
                                                 .Select(c => new AdminCouponModel
                                                 {
                                                     Id = c.Id,
                                                     Code = c.Code,
                                                     Description = c.Description,
                                                     Value = c.Value,
                                                     IsPercent = c.IsPercent,
                                                     IsAmount = c.IsAmount,
                                                     IsOneTime = c.IsOneTime,
                                                     IsActive = c.IsActive,
                                                     DateActiveChanged = c.DateActiveChanged,
                                                     DateCreated = c.DateCreated
                                                 });
                    if (coupons != null) response.Coupons = coupons.ToList();
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "GetCoupons", Ex = ex });
                return new GetCouponsResponse { ErrorMessage = "Unable to get coupons." };
            }
        }

        public static SaveCouponResponse SaveCoupon(SaveCouponRequest request)
        {
            try
            {
                var now = DateTimeConvert.GetTimeZoneDateTime(TimeZoneInfoId.CentralStandardTime);
                var response = new SaveCouponResponse();
                using (var context = new CampbellSupplyEntities())
                {
                    var coupon = context.Coupons.FirstOrDefault(c => c.Id == request.Coupon.Id);
                    if (coupon == null)
                    {
                        coupon = new Coupon
                        {
                            Id = request.Coupon.Id,
                            Code = request.Coupon.Code,
                            Value = request.Coupon.Value,
                            IsPercent = request.Coupon.IsPercent,
                            IsAmount = request.Coupon.IsAmount,
                            IsActive = request.Coupon.IsActive,
                            DateActiveChanged = now,
                            DateCreated = now
                        };
                        context.Coupons.Add(coupon);
                    }
                    else if (coupon.IsActive != request.Coupon.IsActive)
                    {
                        coupon.IsActive = request.Coupon.IsActive;
                        coupon.DateActiveChanged = now;
                    }
                    coupon.Description = request.Coupon.Description;
                    coupon.IsOneTime = request.Coupon.IsOneTime;
                    coupon.IsActive = request.Coupon.IsActive;
                    context.SaveChanges();
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "SaveCoupon", Ex = ex });
                return new SaveCouponResponse { ErrorMessage = "Unable to save coupon." };
            }
        }


        public static GetPreviousAddressResponse GetPreviousAddress(GetPreviousAddressRequest request)
        {
            try
            {
                var response = new GetPreviousAddressResponse();
                using (var context = new CampbellSupplyEntities())
                {
                    var order = context.Orders.AsNoTracking().Where(o => o.AccountId == request.AccountId).OrderByDescending(o => o.DateCreated).FirstOrDefault();
                    if (order == null) return new GetPreviousAddressResponse { ErrorMessage = "No previous address currently exists." };
                    response.PrevShippingAddress = new AddressModel
                    {
                        Id = order.ShippingAddressId,
                        FirstName = order.Address.FirstName,
                        LastName = order.Address.LastName,
                        PhoneNumber = order.Address.PhoneNumber,
                        Address1 = order.Address.Address1,
                        Address2 = order.Address.Address2,
                        City = order.Address.City,
                        State = order.Address.State,
                        ZipCode = order.Address.ZipCode
                    };
                    response.PrevShippingAddress = new AddressModel
                    {
                        Id = order.BillingAddressId,
                        FirstName = order.Address1.FirstName,
                        LastName = order.Address1.LastName,
                        PhoneNumber = order.Address1.PhoneNumber,
                        Address1 = order.Address1.Address1,
                        Address2 = order.Address1.Address2,
                        City = order.Address1.City,
                        State = order.Address1.State,
                        ZipCode = order.Address1.ZipCode
                    };
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "GetPreviousAddress", Ex = ex });
                return new GetPreviousAddressResponse { ErrorMessage = "Unable to get previous address." };
            }
        }

        public static GetOrderResponse GetOrder(GetOrderRequest request)
        {
            try
            {
                var response = new GetOrderResponse();
                using (var context = new CampbellSupplyEntities())
                {
                    var order = context.Orders.FirstOrDefault(o => o.Number.Equals(request.OrderNumber));
                    if (order == null) throw new ApplicationException("Order could not be found: " + request.OrderNumber);
                    response.Order = new FullOrderModel
                    {
                        Id = order.Id,
                        AccountId = order.AccountId,
                        CouponId = order.CouponId,
                        ShippingAddress = new AddressModel
                        {
                            Id = order.ShippingAddressId,
                            FirstName = order.Address1.FirstName,
                            LastName = order.Address1.LastName,
                            PhoneNumber = order.Address1.PhoneNumber,
                            Address1 = order.Address1.Address1,
                            Address2 = order.Address1.Address2,
                            City = order.Address1.City,
                            State = order.Address1.State,
                            ZipCode = order.Address1.ZipCode
                        },
                        BillingAddress = new AddressModel
                        {
                            Id = order.BillingAddressId,
                            FirstName = order.Address.FirstName,
                            LastName = order.Address.LastName,
                            PhoneNumber = order.Address.PhoneNumber,
                            Address1 = order.Address.Address1,
                            Address2 = order.Address.Address2,
                            City = order.Address.City,
                            State = order.Address.State,
                            ZipCode = order.Address.ZipCode
                        },
                        Number = order.Number,
                        PaymentTransactionId = order.PaymentTransactionId,
                        PaymentLast4 = order.PaymentLast4,
                        PaymentExpiration = order.PaymentExpiration,
                        SubTotal = order.SubTotal,
                        ShippingTotal = order.ShippingTotal,
                        SalesTax = order.SalesTax,
                        IsPaid = order.IsPaid,
                        IsProcessed = order.IsProcessed,
                        IsShipped = order.IsShipped,
                        DatePaid = order.DatePaid,
                        DateProcessed = order.DateProcessed,
                        DateShipped = order.DateShipped,
                        DateCreated = order.DateCreated,
                        OrderItems = order.OrderItems.Select(o => new OrderItemModel
                        {
                            Id = Guid.NewGuid(),
                            ProductId = o.ProductId,
                            Name = o.Product.Name,
                            Sku = o.Product.ManufacturerUPC,
                            Color = o.Product.Color == "-1" ? "" : o.Product.Color,
                            Size = o.Product.Size == "-1" ? "" : o.Product.Size,
                            PartNumber = o.Product.PartNumber,
                            Description = o.Product.DescriptionShort,
                            Manufacturer = o.Product.Manufacturer.Name,
                            ManufacturerUPC = o.Product.ManufacturerUPC,
                            Brand = o.Product.Brand,
                            Price = o.ItemPrice,
                            ShippingPrice = o.ShippingPrice,
                            URL = o.Product.ImageURL,
                            IsTaxable = o.Product.IsTaxable,
                            Quantity = o.Quantity
                        }).ToList()
                    };

                    if (order.AccountId != null)
                        response.Order.Account = new AccountModel
                        {
                            Id = order.Account.Id,
                            FirstName = order.Account.FirstName,
                            LastName = order.Account.LastName,
                            Email = order.Account.Email,
                        };

                    if (response.Order.CouponId != null)
                        response.Order.Coupon = new CouponModel
                        {
                            Id = order.Coupon.Id,
                            Code = order.Coupon.Code,
                            IsAmount = order.Coupon.IsAmount,
                            IsPercent = order.Coupon.IsPercent,
                            Value = order.Coupon.Value
                        };

                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "GetOrder", Ex = ex });
                return new GetOrderResponse { ErrorMessage = "Unable to retrieve order." };
            }
        }

        public static GetPlaceOrderResponse PlaceOrder(GetPlaceOrderRequest request)
        {
            try
            {
                var now = DateTimeConvert.GetTimeZoneDateTime(TimeZoneInfoId.CentralStandardTime);
                var response = new GetPlaceOrderResponse();
                using (var context = new CampbellSupplyEntities())
                {
                    var accountId = (Guid?)null;
                    if (request.Order.AccountId != null) accountId = request.Order.AccountId;
                    else
                    {
                        var account = context.Accounts.FirstOrDefault(a => a.Email.Equals(request.Order.Email, StringComparison.CurrentCultureIgnoreCase));
                        if (account == null)
                        {
                            account = new Account
                            {
                                Id = Guid.NewGuid(),
                                FirstName = "",
                                LastName = "",
                                Email = request.Order.Email,
                                Username = "",
                                Password = null,
                                IsActive = true,
                                IsAdmin = false,
                                IsRegistered = false,
                                DateActiveChanged = now,
                                DateCreated = now,
                                DateLastLogin = now,
                                DateRegisteredChaged = now
                            };
                            context.Accounts.Add(account);
                        }
                        accountId = account.Id;
                    }

                    if (request.Order.ShippingAddress.Id == Guid.Empty) request.Order.ShippingAddress.Id = Guid.NewGuid();
                    var shippingAddress = context.Addresses.FirstOrDefault(a => a.Id == request.Order.ShippingAddress.Id);
                    if (shippingAddress == null)
                    {
                        shippingAddress = new Address
                        {
                            Id = request.Order.ShippingAddress.Id,
                            FirstName = request.Order.ShippingAddress.FirstName,
                            LastName = request.Order.ShippingAddress.LastName,
                            Address1 = request.Order.ShippingAddress.Address1,
                            Address2 = request.Order.ShippingAddress.Address2,
                            City = request.Order.ShippingAddress.City,
                            State = request.Order.ShippingAddress.State,
                            ZipCode = request.Order.ShippingAddress.ZipCode,
                            PhoneNumber = request.Order.ShippingAddress.PhoneNumber,
                            DateCreated = now
                        };
                        context.Addresses.Add(shippingAddress);
                    }

                    if (request.Order.BillingAddress.Id == Guid.Empty) request.Order.BillingAddress.Id = Guid.NewGuid();
                    var billingAddress = context.Addresses.FirstOrDefault(a => a.Id == request.Order.BillingAddress.Id);
                    if (billingAddress == null)
                    {
                        billingAddress = new Address
                        {
                            Id = request.Order.BillingAddress.Id,
                            FirstName = request.Order.BillingAddress.FirstName,
                            LastName = request.Order.BillingAddress.LastName,
                            Address1 = request.Order.BillingAddress.Address1,
                            Address2 = request.Order.BillingAddress.Address2,
                            City = request.Order.BillingAddress.City,
                            State = request.Order.BillingAddress.State,
                            ZipCode = request.Order.BillingAddress.ZipCode,
                            PhoneNumber = request.Order.BillingAddress.PhoneNumber,
                            DateCreated = now
                        };
                        context.Addresses.Add(billingAddress);
                    }

                    var order = new Order
                    {
                        Id = Guid.NewGuid(),
                        AccountId = accountId,
                        ShippingAddressId = request.Order.ShippingAddress.Id,
                        BillingAddressId = request.Order.BillingAddress.Id,
                        CouponId = request.Order.CouponId,
                        Number = request.Order.Number,
                        PaymentTransactionId = "",
                        PaymentLast4 = Convert.ToInt32(request.CCNumber.Substring(request.CCNumber.Length - 4)),
                        PaymentExpiration = request.ExpMonth.ToString() + "/" + request.ExpYear.ToString(),
                        SubTotal = request.Order.SubTotal,
                        ShippingTotal = request.Order.ShippingTotal,
                        SalesTax = request.Order.SalesTax,
                        IsPaid = false,
                        IsProcessed = false,
                        IsShipped = false,
                        DatePaid = null,
                        DateProcessed = null,
                        DateShipped = null,
                        DateCreated = now
                    };
                    context.Orders.Add(order);

                    foreach (var item in request.Order.OrderItems)
                        context.OrderItems.Add(new OrderItem
                        {
                            Id = Guid.NewGuid(),
                            OrderId = order.Id,
                            ProductId = item.ProductId,
                            ItemPrice = item.Price,
                            ShippingPrice = item.ShippingPrice,
                            Quantity = item.Quantity,
                            DateCreated = now,
                        });

                    var paymentResponse = ChargeCard(request);
                    if (paymentResponse == null) throw new ApplicationException("Create transaction response is null.");
                    order.PaymentResponse = new JavaScriptSerializer().Serialize(paymentResponse);
                    if (paymentResponse.messages.resultCode == messageTypeEnum.Ok)
                    {
                        order.PaymentTransactionId = paymentResponse.transactionResponse.transId;
                        if (paymentResponse.transactionResponse.messages != null)
                        {
                            order.IsPaid = true;
                            order.DatePaid = now;
                            var cart = context.ShoppingCarts.Where(c => c.AccountId == order.AccountId || c.SessionId == request.SessionId).ToList();
                            foreach (var item in cart) context.ShoppingCarts.Remove(item);
                            context.SaveChanges();
                            response.IsSuccess = true;
                            return response;
                        }
                        else
                        {
                            context.SaveChanges();
                            throw new ApplicationException("Unable to process order: " + paymentResponse.transactionResponse.errors[0].errorCode + "  " + paymentResponse.transactionResponse.errors[0].errorText);
                        }
                    }

                    context.SaveChanges();
                    throw new ApplicationException("Unable to process order: " + paymentResponse.messages.message[0].code + "  " + paymentResponse.messages.message[0].text);
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "PlaceOrder", Ex = ex });
                return new GetPlaceOrderResponse { ErrorMessage = "Unable to place order. Please contact us at info@campbellsupply.net" };
            }
        }

        public static createTransactionResponse ChargeCard(GetPlaceOrderRequest request)
        {
            try
            {
                var environment = AuthorizeNet.Environment.SANDBOX;
                if (ConfigurationManager.AppSettings["AuthorizeNetEnvironment"].Equals("Production", StringComparison.CurrentCultureIgnoreCase)) environment = AuthorizeNet.Environment.PRODUCTION;

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = environment;
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ConfigurationManager.AppSettings["AuthorizeNetLogin"],
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ConfigurationManager.AppSettings["AuthorizeNetTranKey"]
            };

                var orderTotal = request.Order.SubTotal + request.Order.ShippingTotal + request.Order.SalesTax;
                var shippingAmount = new extendedAmountType { amount = request.Order.ShippingTotal, description = "Shipping Amount", name = "Shipping Amount" };
                var salesTaxAmount = new extendedAmountType { amount = request.Order.SalesTax, description = "Sales Tax Amount", name = "Sales Tax Amount" };
                var order = new orderType { invoiceNumber = request.Order.Number, description = "Campbells Online Purchase " + DateTime.Now.ToShortDateString() };
                var creditCard = new creditCardType
                {
                    cardNumber = request.CCNumber.Replace(" ", "").Replace("-", ""),
                    expirationDate = request.ExpMonth.ToString().PadLeft(2, '0') + request.ExpYear.ToString().Substring(2, 2),
                    cardCode = request.CCV.ToString()

                };
                var paymentType = new paymentType { Item = creditCard };
                var billAddress = new customerAddressType
                {
                    firstName = request.Order.BillingAddress.FirstName,
                    lastName = request.Order.BillingAddress.LastName,
                    address = request.Order.BillingAddress.Address1,
                    city = request.Order.BillingAddress.City,
                    state = request.Order.BillingAddress.State,
                    zip = request.Order.BillingAddress.ZipCode,
                    email = request.Order.Email,
                    phoneNumber = request.Order.BillingAddress.PhoneNumber
                };
                var shipAddress = new nameAndAddressType
                {
                    firstName = request.Order.ShippingAddress.FirstName,
                    lastName = request.Order.ShippingAddress.LastName,
                    address = request.Order.ShippingAddress.Address1,
                    city = request.Order.ShippingAddress.City,
                    state = request.Order.ShippingAddress.State,
                    zip = request.Order.ShippingAddress.ZipCode
                };
                var transactionRequestType = new transactionRequestType
                {
                    transactionType = transactionTypeEnum.authCaptureTransaction.ToString(),
                    amount = orderTotal,
                    payment = paymentType,
                    billTo = billAddress,
                    shipTo = shipAddress,
                    shipping = shippingAmount,
                    tax = salesTaxAmount,
                    poNumber = request.Order.Number,
                    order = order,
                };
                var transactionRequest = new createTransactionRequest { transactionRequest = transactionRequestType };
                var controller = new createTransactionController(transactionRequest);
                controller.Execute();
                return controller.GetApiResponse();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static GetOrderHistoryByPageResponse GetOrderHistoryByPage(GetOrderHistoryByPageRequest request)
        {
            try
            {
                var response = new GetOrderHistoryByPageResponse();
                using (var context = new CampbellSupplyEntities())
                {
                    response.RecordCount = context.Orders.AsNoTracking().Where(o => o.AccountId == request.AccountId).Count();
                    var order = context.Orders.AsNoTracking().Where(o => o.AccountId == request.AccountId)
                                              .OrderBy(o => o.DateCreated).Skip(12 * (request.PageIndex - 1)).Take(12)
                                              .Select(o => new OrderHistoryUserModel
                                              {
                                                  OrderId = o.Id,
                                                  OrderNumber = o.Number,
                                                  OrderAmount = o.SubTotal + o.ShippingTotal + o.SalesTax,
                                                  OrderDate = o.DateCreated
                                              });
                    if (order != null) response.Orders = order.ToList();
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "GetOrderHistoryByPage", Ex = ex });
                return new GetOrderHistoryByPageResponse { ErrorMessage = "Unable to retrieve orders." };
            }
        }

        public static GetOrderHistoryDetailResponse GetOrderHistroyDetail(GetOrderHistoryDetailRequest request)
        {
            try
            {
                var response = new GetOrderHistoryDetailResponse();
                using (var context = new CampbellSupplyEntities())
                {
                    var order = context.Orders.FirstOrDefault(o => o.Id == request.OrderId && o.AccountId == request.AccountId);
                    if (order == null) throw new ApplicationException("Order could not be found: " + request.OrderId.ToString() + " | " + request.AccountId.ToString());
                    response.Order = new FullOrderModel
                    {
                        Id = order.Id,
                        AccountId = order.AccountId,
                        Account = new AccountModel
                        {
                            Id = order.Account.Id,
                            FirstName = order.Account.FirstName,
                            LastName = order.Account.LastName,
                            Email = order.Account.Email,
                        },
                        CouponId = order.CouponId,
                        ShippingAddress = new AddressModel
                        {
                            Id = order.ShippingAddressId,
                            FirstName = order.Address1.FirstName,
                            LastName = order.Address1.LastName,
                            PhoneNumber = order.Address1.PhoneNumber,
                            Address1 = order.Address1.Address1,
                            Address2 = order.Address1.Address2,
                            City = order.Address1.City,
                            State = order.Address1.State,
                            ZipCode = order.Address1.ZipCode
                        },
                        BillingAddress = new AddressModel
                        {
                            Id = order.BillingAddressId,
                            FirstName = order.Address.FirstName,
                            LastName = order.Address.LastName,
                            PhoneNumber = order.Address.PhoneNumber,
                            Address1 = order.Address.Address1,
                            Address2 = order.Address.Address2,
                            City = order.Address.City,
                            State = order.Address.State,
                            ZipCode = order.Address.ZipCode
                        },
                        Number = order.Number,
                        PaymentTransactionId = order.PaymentTransactionId,
                        PaymentLast4 = order.PaymentLast4,
                        PaymentExpiration = order.PaymentExpiration,
                        SubTotal = order.SubTotal,
                        ShippingTotal = order.ShippingTotal,
                        SalesTax = order.SalesTax,
                        IsPaid = order.IsPaid,
                        IsProcessed = order.IsProcessed,
                        IsShipped = order.IsShipped,
                        DatePaid = order.DatePaid,
                        DateProcessed = order.DateProcessed,
                        DateShipped = order.DateShipped,
                        DateCreated = order.DateCreated,
                        OrderItems = order.OrderItems.Select(o => new OrderItemModel
                        {
                            Id = o.Id,
                            ProductId = o.ProductId,
                            Quantity = o.Quantity,
                            Description = o.Product.DescriptionShort,
                            Manufacturer = o.Product.Manufacturer.Name,
                            ManufacturerUPC = o.Product.ManufacturerUPC,
                            Brand = o.Product.Brand,
                            Name = o.Product.Name,
                            Price = o.ItemPrice,
                            ShippingPrice = o.ShippingPrice,
                            IsTaxable = o.Product.IsTaxable,
                            PartNumber = o.Product.PartNumber,
                            Color = o.Product.Color == "-1" ? "" : o.Product.Color,
                            Size = o.Product.Size == "-1" ? "" : o.Product.Size,
                            Sku = o.Product.ManufacturerUPC,
                            URL = o.Product.ImageURL
                        }).ToList()
                    };

                    if (order.CouponId != null)
                        response.Order.Coupon = new CouponModel
                        {
                            Id = order.Coupon.Id,
                            Code = order.Coupon.Code,
                            IsAmount = order.Coupon.IsAmount,
                            IsPercent = order.Coupon.IsPercent,
                            Value = order.Coupon.Value
                        };

                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "GetOrderHistroyDetail", Ex = ex });
                return new GetOrderHistoryDetailResponse { ErrorMessage = "Unable to retrieve order history detail." };
            }
        }

        public static SearchOrderHistoryDetailResponse SearchOrderHistoryDetail(SearchOrderHistoryDetailRequest request)
        {
            try
            {
                var response = new SearchOrderHistoryDetailResponse();
                using (var context = new CampbellSupplyEntities())
                {
                    var order = context.Orders.FirstOrDefault(o => o.Number.Equals(request.OrderNumber, StringComparison.InvariantCultureIgnoreCase) && o.AccountId == request.AccountId);
                    if (order == null) throw new ApplicationException("Order could not be found: " + request.OrderNumber + " | " + request.AccountId.ToString());
                    response.Order = GetOrderHistroyDetail(new GetOrderHistoryDetailRequest { OrderId = order.Id }).Order;
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "SearchOrderHistoryDetail", Ex = ex });
                return new SearchOrderHistoryDetailResponse { ErrorMessage = "Unable to retrieve order history detail." };
            }
        }


        public static GetAdminOrderHistoryByPageResponse GetAdminOrderHistoryByPage(GetAdminOrderHistoryByPageRequest request)
        {
            try
            {
                var response = new GetAdminOrderHistoryByPageResponse();
                using (var context = new CampbellSupplyEntities())
                {
                    response.RecordCount = context.Orders.AsNoTracking().Count();
                    var order = context.Orders.AsNoTracking()
                                              .OrderBy(o => o.DateCreated).Skip(12 * (request.PageIndex - 1)).Take(12)
                                              .Select(o => new OrderAdminHistoryUserModel
                                              {
                                                  OrderId = o.Id,
                                                  OrderNumber = o.Number,
                                                  FirstName = o.Account.FirstName,
                                                  LastName = o.Account.LastName,
                                                  OrderAmount = o.SubTotal + o.ShippingTotal + o.SalesTax,
                                                  OrderDate = o.DateCreated
                                              });
                    if (order != null) response.Orders = order.ToList();
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "GetAdminOrderHistoryByPage", Ex = ex });
                return new GetAdminOrderHistoryByPageResponse { ErrorMessage = "Unable to retrieve orders." };
            }
        }

        public static GetAdminOrderHistoryDetailResponse GetAdminOrderHistoryDetail(GetAdminOrderHistoryDetailRequest request)
        {
            try
            {
                var response = new GetAdminOrderHistoryDetailResponse();
                using (var context = new CampbellSupplyEntities())
                {
                    var order = context.Orders.FirstOrDefault(o => o.Id == request.OrderId);
                    if (order == null) throw new ApplicationException("Order could not be found: " + request.OrderId.ToString());
                    response.Order = new FullOrderModel
                    {
                        Id = order.Id,
                        AccountId = order.AccountId,
                        CouponId = order.CouponId,
                        ShippingAddress = new AddressModel
                        {
                            Id = order.ShippingAddressId,
                            FirstName = order.Address1.FirstName,
                            LastName = order.Address1.LastName,
                            Address1 = order.Address1.Address1,
                            Address2 = order.Address1.Address2,
                            City = order.Address1.City,
                            State = order.Address1.State,
                            ZipCode = order.Address1.ZipCode
                        },
                        BillingAddress = new AddressModel
                        {
                            Id = order.BillingAddressId,
                            FirstName = order.Address.FirstName,
                            LastName = order.Address.LastName,
                            Address1 = order.Address.Address1,
                            Address2 = order.Address.Address2,
                            City = order.Address.City,
                            State = order.Address.State,
                            ZipCode = order.Address.ZipCode
                        },
                        Number = order.Number,
                        PaymentTransactionId = order.PaymentTransactionId,
                        PaymentLast4 = order.PaymentLast4,
                        PaymentExpiration = order.PaymentExpiration,
                        SubTotal = order.SubTotal,
                        ShippingTotal = order.ShippingTotal,
                        SalesTax = order.SalesTax,
                        IsPaid = order.IsPaid,
                        IsProcessed = order.IsProcessed,
                        IsShipped = order.IsShipped,
                        DatePaid = order.DatePaid,
                        DateProcessed = order.DateProcessed,
                        DateShipped = order.DateShipped,
                        DateCreated = order.DateCreated,
                        OrderItems = order.OrderItems.Select(o => new OrderItemModel
                        {
                            Id = o.Id,
                            ProductId = o.ProductId,
                            Quantity = o.Quantity,
                            Description = o.Product.DescriptionShort,
                            Manufacturer = o.Product.Manufacturer.Name,
                            ManufacturerUPC = o.Product.ManufacturerUPC,
                            Brand = o.Product.Brand,
                            Name = o.Product.Name,
                            Price = o.ItemPrice,
                            ShippingPrice = o.ShippingPrice,
                            IsTaxable = o.Product.IsTaxable,
                            PartNumber = o.Product.PartNumber,
                            Color = o.Product.Color == "-1" ? "" : o.Product.Color,
                            Size = o.Product.Size == "-1" ? "" : o.Product.Size,
                            Sku = o.Product.ManufacturerUPC,
                            URL = o.Product.ImageURL
                        }).ToList()
                    };

                    if (order.AccountId != null)
                        response.Order.Account = new AccountModel
                        {
                            Id = order.Account.Id,
                            FirstName = order.Account.FirstName,
                            LastName = order.Account.LastName,
                            Email = order.Account.Email,
                        };

                    if (order.CouponId != null)
                        response.Order.Coupon = new CouponModel
                        {
                            Id = order.Coupon.Id,
                            Code = order.Coupon.Code,
                            IsAmount = order.Coupon.IsAmount,
                            IsPercent = order.Coupon.IsPercent,
                            Value = order.Coupon.Value
                        };

                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "GetAdminOrderHistroyDetail", Ex = ex });
                return new GetAdminOrderHistoryDetailResponse { ErrorMessage = "Unable to retrieve order history detail." };
            }
        }

        public static SearchAdminOrderHistoryDetailResponse SearchAdminOrderHistoryDetail(SearchAdminOrderHistoryDetailRequest request)
        {
            try
            {
                var response = new SearchAdminOrderHistoryDetailResponse();
                using (var context = new CampbellSupplyEntities())
                {
                    var order = context.Orders.FirstOrDefault(o => o.Number.Equals(request.OrderNumber, StringComparison.InvariantCultureIgnoreCase));
                    if (order == null) throw new ApplicationException("Order could not be found: " + request.OrderNumber);
                    response.Order = GetAdminOrderHistoryDetail(new GetAdminOrderHistoryDetailRequest { OrderId = order.Id }).Order;
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "SearchAdminOrderHistoryDetail", Ex = ex });
                return new SearchAdminOrderHistoryDetailResponse { ErrorMessage = "Unable to retrieve order history detail." };
            }
        }

    }
}