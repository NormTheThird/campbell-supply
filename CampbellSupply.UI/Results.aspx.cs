using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CampbellSupply.DataLayer.DataContracts.RequestAndResponses;
using CampbellSupply.DataLayer.DataServices;
using System.Web.UI;
using CampbellSupply.DataLayer.DataContracts.Models;
using CampbellSupply.Services;
using CampbellSupply.Common.RequestAndResponses;

namespace CampbellSupply
{
    public partial class Results : Page
    {
        public PagedDataSource _PageDataSource { get; set; }
        public string SearchValue { get; set; }
        int pageCount = 1;

        /// <summary>
        /// Handles the page load event
        /// </summary>
        /// <param name="sender">The object sending the event</param>
        /// <param name="e">The event being sent by the object</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.SearchValue = Request.QueryString["search"].Trim();
                if (!IsPostBack)
                {
                    // Get Featured Items
                    this.repFeatured.DataSource = (List<FeaturedModel>)Session["Featured"];
                    this.repFeatured.DataBind();

                    // Get products
                    this.GetProductsPerPage(1);
                }

            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "Results/Page_Load", Ex = ex });
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
                var productId = Guid.Parse(((HtmlAnchor)sender).Name.ToString().Trim());
                var request = new SaveShoppingCartItemRequest { SessionId = Session.SessionID.ToString(), ProductId = productId, Quantity = 1 };
                var response = OrderService.SaveShoppingCartItem(request);
                if (response.IsSuccess) Session["ShoppingCart"] = response.Items;
                Response.Redirect("Cart.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "Results/AddToCart_Click", Ex = ex });
            }
        }

        /// <summary>
        /// Handles the repeater button click event
        /// </summary>
        /// <param name="sender">The object sending the event</param>
        /// <param name="e">The event being sent by the object</param>
        protected void Page_Changed(object sender, EventArgs e)
        {
            try
            {
                // Set The New Current Page
                int currentPage = int.Parse((sender as LinkButton).CommandArgument);
                if (currentPage > 0)
                    this.GetProductsPerPage(currentPage);
                else
                {
                    // Get Current Middle Button And Set New Values
                    LinkButton linkButton = (LinkButton)repPaging.Items[1].FindControl("btnPage");
                    if (currentPage.Equals(-1))
                        this.GetProductsPerPage(Convert.ToInt32(linkButton.Text.Trim()) - 1);
                    else if (currentPage.Equals(-2))
                        this.GetProductsPerPage(Convert.ToInt32(linkButton.Text.Trim()) + 1);
                    else
                        this.GetProductsPerPage(1);
                }
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "Results/Page_Changed", Ex = ex });
            }
        }

        /// <summary>
        /// Gets all the products to display on the page
        /// </summary>
        /// <param name="currentPage">The current page number</param>
        private void GetProductsPerPage(int currentPage)
        {
            try
            {
                // Get List Of Products For Specific Category
                this.lblResults.Visible = false;
                var response = DataLayer.DataServices.ProductService.SerachProducts(new SearchProductsRequest { SearchValue = this.SearchValue, PageIndex = currentPage });
                if (response.IsSuccess)
                {
                    if (response.Products.Count == 0)
                    {
                        this.lblResults.Visible = true;
                        this.lblResults.Text = "No products were found for this search criteria.";
                    }
                    else
                    {
                        this.repProducts.DataSource = response.Products;
                        this.repProducts.DataBind();
                        this.GetPageCount(response.RecordCount, currentPage);
                    }
                }
                else this.lblResults.Text = response.ErrorMessage;

            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "Results/GetProductsPerPage", Ex = ex });
            }

        }

        /// <summary>
        /// Gets the page count then shows the correct repeater
        /// </summary>
        /// <param name="recordCount">The count of records in the table</param>
        /// <param name="currentPage">The current page number</param>
        private void GetPageCount(int recordCount, int currentPage)
        {
            try
            {
                // Get The Total Amount Of Pages
                double dblPageCount = (double)(recordCount / Convert.ToDecimal(12));
                this.pageCount = (int)Math.Ceiling(dblPageCount);

                // Hide Repeater Buttons
                this.btnFirst.Visible = false;
                this.btnPrevious.Visible = false;
                this.btnNext.Visible = false;
                this.btnLast.Visible = false;

                // Show Repeater
                if (this.pageCount < 7)
                    ShowStandardRepeater(currentPage);
                else
                    ShowAdvancedRepeater(currentPage);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Shows the standard repeater
        /// </summary>
        /// <param name="currentPage">The current page number</param>
        private void ShowStandardRepeater(int currentPage)
        {
            try
            {
                // Get The Buttons For Each Page
                List<ListItem> pages = new List<ListItem>();
                this.btnLast.CommandArgument = pageCount.ToString();
                if (pageCount > 0)
                    for (int i = 1; i <= pageCount; i++)
                        pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));

                // Bind Data To The Repeater
                repPaging.DataSource = pages;
                repPaging.DataBind();
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Shows the advanced repeater
        /// </summary>
        /// <param name="currentPage">The current page number</param>
        private void ShowAdvancedRepeater(int currentPage)
        {
            try
            {
                // Get The Buttons For Each Page
                List<ListItem> pages = new List<ListItem>();
                this.btnLast.CommandArgument = pageCount.ToString();

                // Check Current Page
                if (currentPage.Equals(1) || currentPage.Equals(2))
                {
                    // Show The First Three Pages
                    this.btnNext.Visible = true;
                    this.btnLast.Visible = true;
                    for (int i = 1; i <= 3; i++)
                        pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                }
                else if (currentPage.Equals(pageCount) || currentPage.Equals(pageCount - 1))
                {
                    // Show The Last Three Pages
                    this.btnFirst.Visible = true;
                    this.btnPrevious.Visible = true;
                    for (int i = pageCount - 2; i <= pageCount; i++)
                        pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                }
                else
                {
                    // Show All Buttons
                    this.btnNext.Visible = true;
                    this.btnLast.Visible = true;
                    this.btnFirst.Visible = true;
                    this.btnPrevious.Visible = true;

                    // Add Current Page Plus One On Either Side
                    pages.Add(new ListItem((currentPage - 1).ToString(), (currentPage - 1).ToString(), true));
                    pages.Add(new ListItem(currentPage.ToString(), currentPage.ToString(), false));
                    pages.Add(new ListItem((currentPage + 1).ToString(), (currentPage + 1).ToString(), true));
                }

                // Bind Data To The Repeater
                repPaging.DataSource = pages;
                repPaging.DataBind();
            }
            catch (Exception ex) { throw ex; }
        }
    }
}