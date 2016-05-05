using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using AjaxControlToolkit;

public partial class Product : ScriptPage
{
    public string ProductID = string.Empty;
    public string stars;
    HttpContext context = HttpContext.Current;

    protected void Page_Load(object sender, EventArgs e)
    {
        //Set Book id from query string
        ProductID = Request.QueryString["ProductID"];
                
        getReview(ProductID);

        // don't reload data during postbacks
        if (!IsPostBack)
    {
      PopulateControls();
    }
  }

    private void getReview(string ProductID)
    {
        //select reviews.
        DataTable dsComm = CatalogAccess.GetReviews(Convert.ToInt32(ProductID));

        commList.DataSource = dsComm;
        commList.DataBind();
        UpdatePanel1.Update();
        commListUp.Update();
    }
    // Fill the control with data
    private void PopulateControls()
  {
    // Retrieve ProductID from the query string
    string productId = Request.QueryString["ProductID"];
    // stores product details
    ProductDetails pd;
    // Retrieve product details 
    pd = CatalogAccess.GetProductDetails(productId);
    // Display product details
    titleLabel.Text = pd.Name;
    descriptionLabel.Text = pd.Description;
    priceLabel.Text = String.Format("{0:c}", pd.Price);
    productImage.ImageUrl = "ProductImages/" + pd.Image2FileName;
    // Set the title of the page
    this.Title = BalloonShopConfiguration.SiteName +
                 " : Product : " + pd.Name;
  }

  // Add the product to cart
  protected void addToCartButton_Click(object sender, EventArgs e)
  {
    // Retrieve ProductID from the query string
    string productId = Request.QueryString["ProductID"];
    // Add the product to the shopping cart
    ShoppingCartAccess.AddItem(productId);
  }

  // Redirects to the previously visited catalog page 
  protected void continueShoppingButton_Click(object sender, EventArgs e)
  {
    // redirect to the last visited catalog page
    object page;
    if ((page = Session["LastVisitedCatalogPage"]) != null)
      Response.Redirect(page.ToString());
    else
      Response.Redirect(Request.ApplicationPath);
  }

    protected void addComm_Click(object sender, EventArgs e)
    {
        if (Convert.ToString(Request.Cookies["userInfo"]) != "")
        {
            string userID = Request.Cookies["userInfo"]["mem_ID"];
            string memo = editor1.Text;
            string reviewTitle = reviewTl.Text;
            ProductID = Request.QueryString["ProductID"];

            if (memo == null && memo == "" && memo == "<br />" && memo == "&nbsp;")
            {
                lblError.Text = "Please enter your review.";
                Response.End();
            }
            else
            {
                //review insert.
                bool reVal = CatalogAccess.CreateReview(userID, Convert.ToInt32(ProductID), reviewTitle, memo);

                if (reVal)
                {
                    editor1.Text = string.Empty;
                    UpdatePanel1.Update();
                    getReview(ProductID);

                }
                else
                {
                    ErrorMsg("Errors!!");
                }
            }
        }
        else
        {
            Response.Write("<script language=javascript>alert('You need login first for using wishList');history.back;</script>");
        }
    }
    protected void imgBtn_Click(Object sender, CommandEventArgs e)
    {
        if (e.CommandName == "reviewDel")
        {
            Response.Write("<script>alert('Error! Please try again');</script>");

            if (Convert.ToString(Request.Cookies["userInfo"]) != "")
            {

                string userID = Request.Cookies["userInfo"]["mem_ID"];
                ProductID = Request.QueryString["ProductID"];

                //review delete
                bool reVal = CatalogAccess.DeleteReview(userID, Convert.ToInt16(e.CommandArgument));


                if (reVal)
                {
                    UpdatePanel1.Update();
                    getReview(ProductID);
                }
                else
                {
                    Response.Write("<script>alert('Error! Please try again');</script>");
                }
            }
        }
    }
    protected void ListView_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        ListViewDataItem item = (ListViewDataItem)e.Item;

        if (item.ItemType == ListViewItemType.DataItem)
        {
            if (Convert.ToString(Request.Cookies["userInfo"]) != "")
            {

                string userID = Request.Cookies["userInfo"]["mem_ID"];

                //if it matches between logined ID and author's ID,
                string wID = DataBinder.Eval(item.DataItem, "UserID").ToString();

                if (userID == wID)
                {
                    ImageButton imgBtn = item.FindControl("x") as ImageButton;
                    imgBtn.Visible = true;
                    imgBtn.ImageUrl = "../images/x-gif.gif";
                }
            }

        }
    }

    protected void editor1_TextChanged(object sender, EventArgs e)
    {

    }
}

