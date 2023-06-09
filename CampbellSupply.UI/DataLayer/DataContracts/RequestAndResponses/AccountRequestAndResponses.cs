using CampbellSupply.DataLayer.DataContracts.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CampbellSupply.DataLayer.DataContracts.RequestAndResponses
{
    public class RegisterUserRequest : BaseRequest
    {
        public RegisterUserRequest()
        {
            this.User = new RegisterModel();
        }

        [DataMember(IsRequired = true)]
        public RegisterModel User { get; set; }
    }

    public class RegisterUserResponse : BaseResponse
    {
        public RegisterUserResponse()
        {
            this.User = new AccountModel();
        }

        [DataMember(IsRequired = true)]
        public AccountModel User { get; set; }
    }


    public class ValidateUserRequest : BaseRequest
    {
        public ValidateUserRequest()
        {
            this.Email = string.Empty;
            this.Password = string.Empty;
        }

        [DataMember(IsRequired = true)]
        public string Email { get; set; }
        [DataMember(IsRequired = true)]
        public string Password { get; set; }
    }

    public class ValidateUserResponse : BaseResponse
    {
        public ValidateUserResponse()
        {
            this.User = new AccountModel();
        }

        [DataMember(IsRequired = true)]
        public AccountModel User { get; set; }
    }


    public class PasswordResetRequest : BaseRequest
    {
        public PasswordResetRequest()
        {
            this.Email = string.Empty;
            this.ResetId = Guid.Empty;
        }

        [DataMember(IsRequired = true)]
        public string Email { get; set; }
        [DataMember(IsRequired = true)]
        public Guid ResetId { get; set; }
    }

    public class PasswordResetResponse : BaseResponse { }


    public class ValidatePasswordResetRequest : BaseRequest
    {
        public ValidatePasswordResetRequest()
        {
            this.ResetId = Guid.Empty;
        }

        [DataMember(IsRequired = true)]
        public Guid ResetId { get; set; }
    }

    public class ValidatePasswordResetResponse : BaseResponse { }


    public class UpdatePasswordRequest : BaseRequest
    {
        public UpdatePasswordRequest()
        {
            this.ResetId = Guid.Empty;
            this.NewPassword = string.Empty;
        }

        [DataMember(IsRequired = true)]
        public Guid ResetId { get; set; }
        [DataMember(IsRequired = true)]
        public string NewPassword { get; set; }
    }

    public class UpdatePasswordResponse : BaseResponse { }


    public class GetWishListRequest : BaseRequest
    {
        public GetWishListRequest()
        {
            this.AccountId = Guid.Empty;
        }

        [DataMember(IsRequired = true)]
        public Guid AccountId { get; set; }
    }

    public class GetWishListResponse : BaseResponse
    {
        public GetWishListResponse()
        {
            this.WishList = new List<ProductQuickModel>();
        }

        [DataMember(IsRequired = true)]
        public List<ProductQuickModel> WishList { get; set; }
    }

    public class SaveWishListItemRequest : BaseRequest
    {
        public SaveWishListItemRequest()
        {
            this.AccountId = Guid.Empty;
            this.ProductId = Guid.Empty;
        }

        [DataMember(IsRequired = true)]
        public Guid AccountId { get; set; }
        [DataMember(IsRequired = true)]
        public Guid ProductId { get; set; }
    }

    public class SaveWishListItemResponse : BaseResponse { }
}