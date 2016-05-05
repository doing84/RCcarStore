using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Security;
using System.Data.SqlClient;

public partial class UserControls_ProductReviews : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Retrieve ProductID from the query string
        string productId = Request.QueryString["ProductID"];
        // display product recommendations
        DataTable table = CatalogAccess.GetProductReviews(productId);
        if (table.Rows.Count > 0)
        {
            list.ShowHeader = true;
            list.DataSource = table;
            list.DataBind();
        }
    }

    protected void addReviewButton_Click(object sender, EventArgs e)
{
    string review = reviewTextBox.Text;
    IDbCommand comm = GenericDataAccess.CreateCommand();
    comm.Connection.Open();
    comm.CommandText = "SET IDENTITY_INSERT Reviews ON; INSERT INTO Reviews (ID, ProductID, Review, Date) VALUES (0, " + Request.QueryString["ProductID"] + " , '" + review + "','" + DateTime.Now.ToLongDateString() + "'); SET IDENTITY_INSERT Reviews OFF;";
    comm.CommandType = CommandType.Text;
    comm.ExecuteNonQuery();
        
    comm.Connection.Close();
        

     }


}