using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CampbellSupply.DataLayer.DataServices;
using CampbellSupply.DataLayer.DataContracts.RequestAndResponses;
using CampbellSupply.DataLayer.DataContracts.Models;
using CampbellSupply.Common.RequestAndResponses;
using CampbellSupply.Services;

namespace CampbellSupply
{
    public partial class Category : System.Web.UI.Page
    {
        public PagedDataSource _PageDataSource { get; set; }

        int pageCount = 1;
        public string department { get; private set; }
        public string category { get; private set; }

        /// <summary>
        /// Handles the page load event
        /// </summary>
        /// <param name="sender">The object sending the event</param>
        /// <param name="e">The event being sent by the object</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.department = Request.QueryString["department"];
                this.category = Request.QueryString["category"];
                if (!IsPostBack)
                {
                    // Populate the products page
                    this.GetProductsPerPage(1);

                    // Set Category Repeater
                    this.lblDepartment.Text = department.Trim();
                    this.repCategorys.DataSource = MenuService.GetCategories(new GetCategoriesRequest { Department = this.department }).Categories;
                    this.repCategorys.DataBind();

                    // Get Featured Items
                    this.repFeatured.DataSource = (List<FeaturedModel>)Session["Featured"];
                    this.repFeatured.DataBind();
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "Category/Page_Load", Ex = ex });
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
                var productId = Guid.Parse((sender as HtmlAnchor).Name.ToString().Trim());
                AccountService.AddWishListItem(new SaveWishListItemRequest { AccountId = user.Id, ProductId = productId });
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "Category/AddToWishList_Click", Ex = ex });
               
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
                LoggingService.LogError(new LogErrorRequest { Class = "Category/Page_Changed", Ex = ex });
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
                var response = DataLayer.DataServices.ProductService.GetProductsByPage(new GetProductsByPageRequest { Department = department, Category = category, PageIndex = currentPage });
                this.repProducts.DataSource = response.Products;
                this.repProducts.DataBind();
                this.GetPageCount(response.RecordCount, currentPage);
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "Category/GetProductsPerPage", Ex = ex });
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