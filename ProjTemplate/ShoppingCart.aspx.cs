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

public partial class ShoppingCart : System.Web.UI.Page
{
  protected void Page_Load(object sender, EventArgs e)
  {
    // populate the control only on the initial page load
    if (!IsPostBack)
      PopulateControls();
  }

  // fill shopping cart controls with data
  private void PopulateControls()
  {
   // set the title of the page
    this.Title = BalloonShopConfiguration.SiteName + " : Shopping Cart";
    // get the items in the shopping cart
    DataTable dt = ShoppingCartAccess.GetItems();
    // if the shopping cart is empty...
    if (dt.Rows.Count == 0)
    {
      titleLabel.Text = "Your shopping cart is empty!";
      grid.Visible = false;
      updateButton.Enabled = false;
      checkoutButton.Enabled = false;
      totalAmountLabel.Text = String.Format("{0:c}", 0);
    }
    else
    // if the shopping cart is not empty...
    {
      // populate the list with the shopping cart contents
      grid.DataSource = dt;
      grid.DataBind();
      // setup controls
      titleLabel.Text = "These are the products in your shopping cart:";
      grid.Visible = true;
      updateButton.Enabled = true;
      checkoutButton.Enabled = true;
      // display the total amount
      decimal orginalAmount = ShoppingCartAccess.GetTotalAmount(); //original total
      decimal amount = ShoppingCartAccess.ShoppingCartGetTotalAmountDiscount();  //price after discount 10%


      if (orginalAmount < 100)
      {
          orginalAmount = (decimal) orginalAmount * (decimal) 1.13;
          totalAmountLabel.Text = String.Format("{0:c}", orginalAmount);
          totalAmountLabel.Text += " (added tax 13%)";
   
      }

      else
      {
          

          if (orginalAmount > 200)
          {

              totalAmountLabel.Text = String.Format("{0:c}", orginalAmount-amount);
              totalAmountLabel.Text += " (No tax and 10% discount)";
          }
          else
          {
              
              totalAmountLabel.Text = String.Format("{0:c}", orginalAmount);
              totalAmountLabel.Text += " (No tax added)";
          }

      }
    }
  }

  // remove a product from the cart
  protected void grid_RowDeleting(object sender, GridViewDeleteEventArgs e)
  {
    // Index of the row being deleted
    int rowIndex = e.RowIndex;
    // The ID of the product being deleted
    string productId = grid.DataKeys[rowIndex].Value.ToString();
    // Remove the product from the shopping cart
    bool success = ShoppingCartAccess.RemoveItem(productId);
    // Display status
    statusLabel.Text = success ? "<br />Product successfully removed!<br />" :
                      "<br />There was an error removing the product!<br />";
    // Repopulate the control
    PopulateControls();
  }

  // update shopping cart product quantities
  protected void updateButton_Click(object sender, EventArgs e)
  {
    // Number of rows in the GridView
    int rowsCount = grid.Rows.Count;
    // Will store a row of the GridView
    GridViewRow gridRow;
    // Will reference a quantity TextBox in the GridView
    TextBox quantityTextBox;
    // Variables to store product ID and quantity
    string productId;
    int quantity;
    // Was the update successful?
    bool success = true;
    // Go through the rows of the GridView
    for (int i = 0; i < rowsCount; i++)
    {
      // Get a row
      gridRow = grid.Rows[i];
      // The ID of the product being deleted
      productId = grid.DataKeys[i].Value.ToString();
      // Get the quantity TextBox in the Row
      quantityTextBox = (TextBox)gridRow.FindControl("editQuantity");
      // Get the quantity, guarding against bogus values
      if (Int32.TryParse(quantityTextBox.Text, out quantity))
      {
        // Update product quantity
        success = success && ShoppingCartAccess.UpdateItem(productId, quantity);
      }
      else
      {
        // if TryParse didn't succeed
        success = false;
      }
      // Display status message
      statusLabel.Text = success ?
        "<br />Your shopping cart was successfully updated!<br />" :
        "<br />Some quantity updates failed! Please verify your cart!<br />";
    }
    // Repopulate the control
    PopulateControls();
  }

  // Redirects to the previously visited catalog page 
  // (an alternate to the functionality implemented here is to to 
  // Request.UrlReferrer, although that way you have no control to 
  // what pages you forward your visitor back to)
  protected void continueShoppingButton_Click(object sender, EventArgs e)
  {
    // redirect to the last visited catalog page, or to the
    // main page of the catalog
    object page;
    if ((page = Session["LastVisitedCatalogPage"]) != null)
      Response.Redirect(page.ToString());
    else
      Response.Redirect(Request.ApplicationPath);
  }

  // create a new order and redirect to a payment page
  protected void checkoutButton_Click(object sender, EventArgs e)
  {
    // Store the total amount because the cart 
    // is emptied when creating the order
    decimal amount = ShoppingCartAccess.GetTotalAmount();
    // Create the order and store the order ID
    string orderId = ShoppingCartAccess.CreateOrder();
    // Obtain the site name from the configuration settings
    string siteName = BalloonShopConfiguration.SiteName;
    // Create the PayPal redirect location
    string redirect = "";
    redirect += "https://www.paypal.com/xclick/business=dooing84@gmail.com";
    redirect += "&item_name=" + "Shoe Infinity and Beyond" + " Order " + orderId;
    redirect += "&item_number=" + orderId;
    redirect += "&amount=" + String.Format("{0:0.00} ", amount);
    redirect += "&currency=USD";
    redirect += "&return=http://www." + siteName + ".com";
    redirect += "&cancel_return=http://www." + siteName + ".com";
    // Redirect to the payment page
    Response.Redirect(redirect);
  }

    protected void grid_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}
