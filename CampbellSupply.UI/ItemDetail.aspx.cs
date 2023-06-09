using System;
using System.Configuration;
using System.Threading.Tasks;
using Amazon.S3.IO;
using Amazon;
using Amazon.S3;
using CampbellSupply.DataLayer.DataServices;
using CampbellSupply.DataLayer.DataContracts.RequestAndResponses;
using CampbellSupply.DataLayer.DataContracts.Models;
using System.Linq;
using CampbellSupply.Services;
using CampbellSupply.Common.RequestAndResponses;
using System.Collections.Generic;

namespace CampbellSupply
{
    public partial class ItemDetail : System.Web.UI.Page
    {
        public ProductModel Product { get; set; }

        /// <summary>
        /// Handles the page load event
        /// </summary>
        /// <param name="sender">The object sending the event</param>
        /// <param name="e">The event being sent by the object</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.Product = new ProductModel();
                if (!IsPostBack)
                {
                    this.divReviewLoggedIn.Visible = true;
                    this.divReviewThanks.Visible = false;
                    this.divReviewAdd.Visible = false;
                    if (Session["User"] != null)
                    {
                        this.divReviewLoggedIn.Visible = false;
                        this.divReviewAdd.Visible = true;
                    }

                    var productId = Guid.Empty;
                    if (string.IsNullOrEmpty(Request.QueryString["productid"]))
                    {
                        Response.Redirect("Error.aspx", false);
                        Context.ApplicationInstance.CompleteRequest();
                    }
                    else if (!Guid.TryParse(Request.QueryString["productid"].Trim(), out productId))
                    {
                        Response.Redirect("Error.aspx", false);
                        Context.ApplicationInstance.CompleteRequest();
                    }
                    else
                    {
                        this.ShowProductInformation(productId);
                        DataLayer.DataServices.ProductService.SaveRecentlyViewed(new SaveRecentlyViewedRequest { ProductId = productId });
                        if (!this.Product.IsActive)
                        {
                            Response.Redirect("Error.aspx", false);
                            Context.ApplicationInstance.CompleteRequest();
                        }
                        else
                        {
                            if (this.Product.RelatedSizeAndColor.Count < 2) this.ddlSizeAndColor.Visible = false;

                            this.ddlSizeAndColor.DataSource = this.Product.RelatedSizeAndColor;
                            this.ddlSizeAndColor.DataTextField = "item2";
                            this.ddlSizeAndColor.DataValueField = "item1";
                            this.ddlSizeAndColor.SelectedValue = this.Product.Id.ToString();
                            this.ddlSizeAndColor.DataBind();


                            this.lblDepartment.Text = this.Product.DepartmentName.Trim();
                            this.repCategorys.DataSource = MenuService.GetCategories(new GetCategoriesRequest { Department = this.Product.DepartmentName.Trim() }).Categories;
                            this.repCategorys.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Write To Error Log And Return Empty DataSet
                LoggingService.LogError(new LogErrorRequest { Class = "ItemDetail/Page_Load", Ex = ex });
            }
        }

        /// <summary>
        /// Handles the add to cart button click event
        /// </summary>
        /// <param name="sender">The object sending the event</param>
        /// <param name="e">The event being sent by the object</param>
        protected void AddToCart_Click(object sender, EventArgs e)
        {
            try
            {
                var productId = Guid.Parse(this.ddlSizeAndColor.SelectedValue);
                var request = new SaveShoppingCartItemRequest { SessionId = Session.SessionID.ToString(), ProductId = productId, Quantity = 1 };
                var response = OrderService.SaveShoppingCartItem(request);
                if (response.IsSuccess) Session["ShoppingCart"] = response.Items;
                Response.Redirect("Cart.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "ItemDetail/AddToCart_Click", Ex = ex });
            }
        }

        /// <summary>
        /// Handles the add to wish list button click event
        /// </summary>
        /// <param name="sender">The object sending the event</param>
        /// <param name="e">The event being sent by the object</param>
        protected void AddToWishList_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["User"] == null)
                {
                    Response.Redirect("Login.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
                var user = (AccountModel)Session["User"];
                var productId = Guid.Parse(this.ddlSizeAndColor.SelectedValue);
                AccountService.AddWishListItem(new SaveWishListItemRequest { AccountId = user.Id, ProductId = productId });
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "ItemDetail/AddToWishList_Click", Ex = ex });
            }
        }

        /// <summary>
        /// Handles the add a review submit button
        /// </summary>
        /// <param name="sender">The object sending the event</param>
        /// <param name="e">The event being sent by the object</param>
        protected void AddReview_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["user"] == null)
                {
                    Response.Redirect("Login.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
                var user = (AccountModel)Session["User"];
                var productId = Guid.Parse(this.ddlSizeAndColor.SelectedValue);
                var rated = new AdminRatedReviewedModel
                {
                    ProductId = productId,
                    AccountId = user.Id,
                    Review = this.reviewText.Value.ToString().Trim(),
                    Rating = Convert.ToInt32(Request["score"].Substring(1, 1))
                };
                var response = DataLayer.DataServices.ProductService.SaveRatedAndReviewed(new SaveRatedAndReviewedRequest { RatedAndReviewed = rated });
                this.divReviewThanks.Visible = true;
                this.divReviewThanks.DataBind();
                this.divReviewAdd.Visible = false;
                this.divReviewAdd.DataBind();
                //TODO: Fix thank you message not showing
            }
            catch (Exception ex)
            {
                // Write To Error Log And Return Empty DataSet
                LoggingService.LogError(new LogErrorRequest { Class = "ItemDetail/AddReview_Click", Ex = ex });
            }
        }

        /// <summary>
        /// Handles the size and color dropdown selection
        /// </summary>
        /// <param name="sender">The object sending the event</param>
        /// <param name="e">The event being sent by the object</param>
        protected void ddlSizeAndColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.ShowProductInformation(Guid.Parse(this.ddlSizeAndColor.SelectedValue.Trim()));
            }
            catch (Exception ex)
            {
                // Write To Error Log And Return Empty DataSet
                LoggingService.LogError(new LogErrorRequest { Class = "ItemDetail/ddlSizeAndColor_SelectedIndexChanged", Ex = ex });
            }
        }

        /// <summary>
        /// Shows all the information for the product
        /// </summary>
        /// <param name="_productID">The product id to show the information for</param>
        private void ShowProductInformation(Guid _productID)
        {
            try
            {
                // Get Product And Populate Reviews
                var productResponse = DataLayer.DataServices.ProductService.GetProduct(new GetProductRequest {ProductId = _productID});
                if (productResponse == null)
                {
                    Response.Redirect("Home.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }

                this.Product = productResponse.Product;
                this.repReview.DataSource = this.Product.RatedReviewed;
                this.repReview.DataBind();

                // Bind Data To Property
                this.imgMain.DataBind();
                this.lblName.DataBind();
                this.lblShortDesc.DataBind();
                this.lblLongDesc.DataBind();
                //this.lblManufacturer.DataBind();
                this.lblManufacturerUPC.DataBind();
                this.lblBrand.DataBind();
                this.lblPurchasePrice.DataBind();

                this.lblExtraID.Text = Product.PartNumber.Length > 0 ? "Part #" + Product.PartNumber : "";
                this.lblExtraID.Text += Product.Sku.Length > 0 ? "<br>SKU: " + Product.Sku : "";

                if (Session["User"] != null)
                {
                    var user = (AccountModel)Session["User"];
                    var review = this.Product.RatedReviewed.FirstOrDefault(r => r.AccountId == user.Id);
                    if (review != null)
                    {
                        this.divReviewLoggedIn.Visible = false;
                        this.divReviewThanks.Visible = true;
                        this.divReviewAdd.Visible = false;
                    }
                }

                //Check if Item is In Store Only          
                if (!this.Product.IsShippingValid)
                {
                    lblStoreOnly.Visible = true;
                    Button1.Visible = false;
                }
                else
                {
                    lblStoreOnly.Visible = false;
                    Button1.Visible = true;
                }

                // Check If Image Exists
                if (string.IsNullOrEmpty(this.Product.ImageName))
                {
                    Task.Factory.StartNew(this.SendNoImageEmail);
                    this.imgMain.ImageUrl = "assets/images/NoImgAvail.png";
                    return;
                }

                var config = new AmazonS3Config { RegionEndpoint = RegionEndpoint.USEast1 };
                using (var client = new AmazonS3Client(ConfigurationManager.AppSettings["AWSAccessKey"], ConfigurationManager.AppSettings["AWSSecretKey"], config))
                {
                    S3FileInfo s3FileInfo = new S3FileInfo(client, "campbellsp/Product", this.Product.ImageName);
                    if (!s3FileInfo.Exists)
                    {
                        Task.Factory.StartNew(this.SendNoImageEmail);
                        this.imgMain.ImageUrl = "assets/images/NoImgAvail.png";
                    }
                }
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "ItemDetail/ShowProductInformation", Ex = ex });
            }
        }

        /// <summary>
        /// Sends an email if the product does not have a picture attached
        /// </summary>
        private void SendNoImageEmail()
        {
            try
            {
                var request = new SendEmailRequest
                {
                    Recipients = ConfigurationManager.AppSettings["AdvertisingToEmail"].Split('|').ToList(),
                    From = "no-reply@campbellssupply.net",
                    Subject = "Campbell Supply : Website Info",
                    Body = "No Image Exists For This Product " + Environment.NewLine + Environment.NewLine +
                           "Product Name: " + this.Product.Name.Trim() + Environment.NewLine +
                           "WebPartNumber: " + this.Product.WebPartNumber + Environment.NewLine +
                           "UPC: " + this.Product.ManufacturerUPC.Trim()
                };

                var response = MessagingService.SendEmail(request);
                if (!response.IsSuccess) throw new ApplicationException(response.ErrorMessage);

            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "ItemDetail/SendNoImageEmail", Ex = ex });
            }
        }
    }
}