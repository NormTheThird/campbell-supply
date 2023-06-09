using CampbellSupply.Common.Enums;
using CampbellSupply.Common.Helpers;
using CampbellSupply.Common.RequestAndResponses;
using CampbellSupply.Data;
using CampbellSupply.DataLayer.DataContracts.Models;
using CampbellSupply.DataLayer.DataContracts.RequestAndResponses;
using CampbellSupply.Services;
using System;
using System.Linq;

namespace CampbellSupply.DataLayer.DataServices
{
    public static class AccountService
    {
        public static RegisterUserResponse RegisterUser(RegisterUserRequest request)
        {
            try
            {
                var now = DateTimeConvert.GetTimeZoneDateTime(TimeZoneInfoId.CentralStandardTime);
                var response = new RegisterUserResponse();
                using (var context = new CampbellSupplyEntities())
                {
                    var user = context.Accounts.FirstOrDefault(a => a.Email.Trim().Equals(request.User.Email.Trim(), StringComparison.InvariantCultureIgnoreCase));
                    if (user != null) return new RegisterUserResponse { ErrorMessage = "An Account already exists. Please <A HREF=/login><u>click here</u></A> to login or reset your password" };
                    user = new Data.Account
                    {
                        Id = Guid.NewGuid(),
                        FirstName = request.User.FirstName,
                        LastName = request.User.LastName,
                        Username = "",
                        Password = Helpers.PasswordHash.HashPassword(request.User.Password.Trim()),
                        Email = request.User.Email,
                        IsRegistered = true,
                        IsActive = true,
                        IsAdmin = false,
                        DateRegisteredChaged = now,
                        DateActiveChanged = now,
                        DateLastLogin = now,
                        DateCreated = now
                    };
                    context.Accounts.Add(user);
                    context.SaveChanges();

                    response.User = new DataContracts.Models.AccountModel
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Username = user.Username,
                        Email = user.Email,
                        IsRegistered = user.IsRegistered,
                        IsActive = user.IsActive,
                        IsAdmin = user.IsAdmin,
                        DateRegisteredChanged = user.DateRegisteredChaged,
                        DateActiveChanged = user.DateActiveChanged,
                        DateLastLogin = user.DateLastLogin,
                        DateCreated = user.DateCreated,
                    };
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "RegisterUser", Ex = ex });
                return new RegisterUserResponse { ErrorMessage = "Unable to register new user." };
            }
        }

        public static ValidateUserResponse ValidateUser(ValidateUserRequest request)
        {
            try
            {
                var response = new ValidateUserResponse();
                using (var context = new CampbellSupplyEntities())
                {
                    var user = context.Accounts.FirstOrDefault(a => a.Email.Trim().Equals(request.Email.Trim(), StringComparison.InvariantCultureIgnoreCase));
                    if (user == null) throw new ApplicationException("Email does not exist for " + request.Email.Trim());
                    if (!user.IsActive) throw new ApplicationException("This account is set to inactive for " + request.Email.Trim());
                    if (!Helpers.PasswordHash.ValidatePassword(request.Password.Trim(), user.Password)) throw new ApplicationException("Invalid password for " + request.Email.Trim());
                    user.DateLastLogin = DateTime.Now;
                    context.SaveChanges();
                    response.User = new DataContracts.Models.AccountModel
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Username = user.Username,
                        Email = user.Email,
                        IsRegistered = user.IsRegistered,
                        IsActive = user.IsActive,
                        IsAdmin = user.IsAdmin,
                        DateRegisteredChanged = user.DateRegisteredChaged,
                        DateActiveChanged = user.DateActiveChanged,
                        DateLastLogin = user.DateLastLogin,
                        DateCreated = user.DateCreated,
                    };
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "ValidateUser", Ex = ex, SendEmail = false });
                return new ValidateUserResponse { ErrorMessage = "Unable to validate user." };
            }
        }

        public static PasswordResetResponse PasswordReset(PasswordResetRequest request)
        {
            try
            {
                var now = DateTimeConvert.GetTimeZoneDateTime(TimeZoneInfoId.CentralStandardTime);
                var response = new PasswordResetResponse();
                using (var context = new CampbellSupplyEntities())
                {
                    var user = context.Accounts.FirstOrDefault(a => a.Email.Trim().Equals(request.Email.Trim(), StringComparison.InvariantCultureIgnoreCase));
                    if (user == null) return new PasswordResetResponse { ErrorMessage = "Oops!  Sorry that email cannot be found." };
                    if (!user.IsActive) throw new ApplicationException("This account is set to inactive: " + request.Email.Trim());
                    var resets = user.PasswordResets.Where(r => r.IsActive);
                    foreach (var reset in resets)
                    {
                        reset.IsActive = false;
                        reset.DateActiveChanged = now;
                    }
                    var newReset = new Data.PasswordReset
                    {
                        Id = Guid.NewGuid(),
                        AccountId = user.Id,
                        ResetId = request.ResetId,
                        IsActive = true,
                        DateActiveChanged = now,
                        DateCreated = now
                    };
                    context.PasswordResets.Add(newReset);
                    context.SaveChanges();
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "PasswordReset", Ex = ex, SendEmail = false });
                return new PasswordResetResponse { ErrorMessage = "Unable to reset password. Please contact us at info@campbellsupply.net" };
            }
        }

        public static ValidatePasswordResetResponse ValidatePasswordReset(ValidatePasswordResetRequest request)
        {
            try
            {
                var now = DateTimeConvert.GetTimeZoneDateTime(TimeZoneInfoId.CentralStandardTime);
                var response = new ValidatePasswordResetResponse();
                using (var context = new CampbellSupplyEntities())
                {
                    var expired = now.AddMinutes(-15);
                    var reset = context.PasswordResets.FirstOrDefault(r => r.ResetId == request.ResetId && r.IsActive && r.DateCreated > expired);
                    if (reset == null) throw new ApplicationException("The time for this link has expired for id: " + request.ResetId);
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "ValidatePasswordReset", Ex = ex, SendEmail = false });
                return new ValidatePasswordResetResponse { ErrorMessage = "Unable to reset password. Please contact us at info@campbellsupply.net" };
            }
        }

        public static UpdatePasswordResponse UpdatePassword(UpdatePasswordRequest request)
        {
            try
            {
                var now = DateTimeConvert.GetTimeZoneDateTime(TimeZoneInfoId.CentralStandardTime);
                var response = new UpdatePasswordResponse();
                using (var context = new CampbellSupplyEntities())
                {
                    var expired =now.AddMinutes(-15);
                    var reset = context.PasswordResets.FirstOrDefault(r => r.ResetId == request.ResetId && r.IsActive && r.DateCreated > expired);
                    if (reset == null) return new UpdatePasswordResponse { ErrorMessage = "The time for this link has expired. Please request a new link <a href=PasswordReset.aspx>here</a>." };
                    reset.IsActive = false;
                    reset.DateActiveChanged = now;

                    var user = context.Accounts.FirstOrDefault(a => a.Id == reset.AccountId && a.IsActive);
                    if (user == null) throw new ApplicationException("User not found or account is inactive for account id: " + reset.AccountId.ToString());
                    user.Password = Helpers.PasswordHash.HashPassword(request.NewPassword.Trim());

                    context.SaveChanges();
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "UpdatePassword", Ex = ex, SendEmail = false });
                return new UpdatePasswordResponse { ErrorMessage = "Unable to reset password. Please contact us at info@campbellsupply.net" };
            }
        }



        public static GetWishListResponse GetWishList(GetWishListRequest request)
        {
            try
            {
                var response = new GetWishListResponse();
                using (var context = new CampbellSupplyEntities())
                {
                    var lst = context.WishLists.AsNoTracking().Where(w => w.AccountId == request.AccountId)
                                               .OrderBy(w => w.Product.Name)
                                               .Select(w => new ProductQuickModel
                                               {
                                                   ProductId = w.ProductId,
                                                   Name = w.Product.Name,
                                                   Description = w.Product.DescriptionShort,
                                                   Manufacturer = w.Product.Manufacturer.Name,
                                                   Brand = w.Product.Brand,
                                                   Price = w.Product.OverridePrice > 0 ? w.Product.OverridePrice : w.Product.PurchasePrice,
                                                   URL = w.Product.ImageURL,
                                                   IsTaxable = w.Product.IsTaxable
                                               });

                    if (lst != null) response.WishList = lst.ToList();
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "GetWishList", Ex = ex });
                return new GetWishListResponse { ErrorMessage = "Unable to get wish list." };
            }
        }

        public static SaveWishListItemResponse AddWishListItem(SaveWishListItemRequest request)
        {
            try
            {
                var response = new SaveWishListItemResponse();
                using (var context = new CampbellSupplyEntities())
                {
                    var item = context.WishLists.FirstOrDefault(i => i.AccountId == request.AccountId && i.ProductId == request.ProductId);
                    if (item == null)
                    {
                        item = new Data.WishList { Id = Guid.NewGuid(), AccountId = request.AccountId, ProductId = request.ProductId, DateCreated = DateTime.Now };
                        context.WishLists.Add(item);
                        context.SaveChanges();
                    }

                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "GetWishList", Ex = ex });
                return new SaveWishListItemResponse { ErrorMessage = "Unable to Add wish list item." };
            }
        }

        public static SaveWishListItemResponse RemoveWishListItem(SaveWishListItemRequest request)
        {
            try
            {
                var response = new SaveWishListItemResponse();
                using (var context = new CampbellSupplyEntities())
                {
                    var item = context.WishLists.FirstOrDefault(i => i.AccountId == request.AccountId && i.ProductId == request.ProductId);
                    if (item != null)
                    {
                        context.WishLists.Remove(item);
                        context.SaveChanges();
                    }

                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "GetWishList", Ex = ex });
                return new SaveWishListItemResponse { ErrorMessage = "Unable to remove wish list item." };
            }
        }
    }
}