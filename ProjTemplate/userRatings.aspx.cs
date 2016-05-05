using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;

public partial class userRatings : System.Web.UI.Page
{

    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
    protected void Page_Load(object sender, EventArgs e)
    {
        //con = OpenFile();

        if (!IsPostBack)
        {
            
            int productID = Convert.ToInt32(Request.QueryString["pid"]);
            int rate = Convert.ToInt32(Request.QueryString["r"]);

            insertToDb(productID, rate);
        }
    }

    private void insertToDb(int p, int r){
        //inserting into db
        SqlCommand cmd = new SqlCommand("INSERT INTO userRatings", con);
    }
}