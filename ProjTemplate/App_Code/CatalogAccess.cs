using System;
using System.Data;
using System.Data.Common;

/// <summary>
/// Wraps department details data
/// </summary>
public struct DepartmentDetails
{
  public string Name;
  public string Description;
}

/// <summary>
/// Wraps category details data
/// </summary>
public struct CategoryDetails
{
  public int DepartmentId;
  public string Name;
  public string Description;
  
}

/// <summary>
/// Wraps product details data
/// </summary>
public struct ProductDetails
{
  public string Name;
  public string Description;
  public decimal Price;
  public string Image1FileName;
  public string Image2FileName;
  public bool OnDepartmentPromotion;
  public bool OnCatalogPromotion;
}

/// <summary>
/// Product catalog business tier component
/// </summary>
public static class CatalogAccess
{
  static CatalogAccess()
  {
    //
    // TODO: Add constructor logic here
    //
  }

  // Retrieve the list of departments 
  public static DataTable GetDepartments()
  {
    // get a configured DbCommand object
    DbCommand comm = GenericDataAccess.CreateCommand();
    // set the stored procedure name
    comm.CommandText = "CatalogGetDepartments";
    // execute the stored procedure and return the results
    return GenericDataAccess.ExecuteSelectCommand(comm);
  }

  // get department details
  public static DepartmentDetails GetDepartmentDetails(string departmentId)
  {
    // get a configured DbCommand object
    DbCommand comm = GenericDataAccess.CreateCommand();
    // set the stored procedure name
    comm.CommandText = "CatalogGetDepartmentDetails";
    // create a new parameter
    DbParameter param = comm.CreateParameter();
    param.ParameterName = "@DepartmentID";
    param.Value = departmentId;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // execute the stored procedure
    DataTable table = GenericDataAccess.ExecuteSelectCommand(comm);
    // wrap retrieved data into a DepartmentDetails object
    DepartmentDetails details = new DepartmentDetails();
    if (table.Rows.Count > 0)
    {
      details.Name = table.Rows[0]["Name"].ToString();
      details.Description = table.Rows[0]["Description"].ToString();
    }
    // return department details
    return details;
  }

  // Get category details
  public static CategoryDetails GetCategoryDetails(string categoryId)
  {
    // get a configured DbCommand object
    DbCommand comm = GenericDataAccess.CreateCommand();
    // set the stored procedure name
    comm.CommandText = "CatalogGetCategoryDetails";
    // create a new parameter
    DbParameter param = comm.CreateParameter();
    param.ParameterName = "@CategoryID";
    param.Value = categoryId;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // execute the stored procedure
    DataTable table = GenericDataAccess.ExecuteSelectCommand(comm);
    // wrap retrieved data into a CategoryDetails object
    CategoryDetails details = new CategoryDetails();
    if (table.Rows.Count > 0)
    {
      details.DepartmentId = Int32.Parse(table.Rows[0]["DepartmentID"].ToString());
      details.Name = table.Rows[0]["Name"].ToString();
      details.Description = table.Rows[0]["Description"].ToString();
    }
    // return department details
    return details;
  }

  // Get product details
  public static ProductDetails GetProductDetails(string productId)
  {
    // get a configured DbCommand object
    DbCommand comm = GenericDataAccess.CreateCommand();
    // set the stored procedure name
    comm.CommandText = "CatalogGetProductDetails";
    // create a new parameter
    DbParameter param = comm.CreateParameter();
    param.ParameterName = "@ProductID";
    param.Value = Convert.ToInt32(productId);
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // execute the stored procedure
    DataTable table = GenericDataAccess.ExecuteSelectCommand(comm);
    // wrap retrieved data into a ProductDetails object
    ProductDetails details = new ProductDetails();
    if (table.Rows.Count > 0)
    {
      // get the first table row
      DataRow dr = table.Rows[0];
      // get product details
      details.Name = dr["Name"].ToString();
      details.Description = dr["Description"].ToString();
      details.Price = Decimal.Parse(dr["Price"].ToString());
      details.Image1FileName = dr["Thumbnail"].ToString();
      details.Image2FileName = dr["Image"].ToString();
      details.OnDepartmentPromotion = bool.Parse(dr["PromoDept"].ToString());
      details.OnCatalogPromotion = bool.Parse(dr["PromoFront"].ToString());
    }
    // return department details
    return details;
  }

  // retrieve the list of categories in a department
  public static DataTable GetCategoriesInDepartment(string departmentId)
  {
    // get a configured DbCommand object
    DbCommand comm = GenericDataAccess.CreateCommand();
    // set the stored procedure name
    comm.CommandText = "CatalogGetCategoriesInDepartment";
    // create a new parameter
    DbParameter param = comm.CreateParameter();
    param.ParameterName = "@DepartmentID";
    param.Value = departmentId;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // execute the stored procedure
    return GenericDataAccess.ExecuteSelectCommand(comm);
  }

  // Retrieve the list of products on catalog promotion
  public static DataTable GetProductsOnCatalogPromotion(string pageNumber, out int howManyPages)
  {
    // get a configured DbCommand object
    DbCommand comm = GenericDataAccess.CreateCommand();
    // set the stored procedure name
    comm.CommandText = "CatalogGetProductsOnDeptPromo";
    // create a new parameter
    DbParameter param = comm.CreateParameter();
    param.ParameterName = "@DescriptionLength";
    param.Value = BalloonShopConfiguration.ProductDescriptionLength;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@PageNumber";
    param.Value = pageNumber;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@ProductsPerPage";
    param.Value = BalloonShopConfiguration.ProductsPerPage;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@HowManyProducts";
    param.Direction = ParameterDirection.Output;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // execute the stored procedure and save the results in a DataTable
    DataTable table = GenericDataAccess.ExecuteSelectCommand(comm);
    // calculate how many pages of products and set the out parameter
    int howManyProducts = Int32.Parse(comm.Parameters["@HowManyProducts"].Value.ToString());
    howManyPages = (int)Math.Ceiling((double)howManyProducts /
                           (double)BalloonShopConfiguration.ProductsPerPage);
    // return the page of products
    return table;
  }

  // retrieve the list of products featured for a department
  public static DataTable GetProductsOnDepartmentPromotion(string departmentId, string pageNumber, out int howManyPages)
  {
    // get a configured DbCommand object
    DbCommand comm = GenericDataAccess.CreateCommand();
    // set the stored procedure name
    comm.CommandText = "CatalogGetProductsOnDeptPromo";
    // create a new parameter
    DbParameter param = comm.CreateParameter();
    param.ParameterName = "@DepartmentID";
    param.Value = departmentId;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@DescriptionLength";
    param.Value = BalloonShopConfiguration.ProductDescriptionLength;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@PageNumber";
    param.Value = pageNumber;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@ProductsPerPage";
    param.Value = BalloonShopConfiguration.ProductsPerPage;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@HowManyProducts";
    param.Direction = ParameterDirection.Output;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // execute the stored procedure and save the results in a DataTable
    DataTable table = GenericDataAccess.ExecuteSelectCommand(comm);
    // calculate how many pages of products and set the out parameter
    int howManyProducts = Int32.Parse(comm.Parameters["@HowManyProducts"].Value.ToString());
    howManyPages = (int)Math.Ceiling((double)howManyProducts /
                   (double)BalloonShopConfiguration.ProductsPerPage);
    // return the page of products
    return table;
  }

  // retrieve the list of products in a category
  public static DataTable GetProductsInCategory(string categoryId, string pageNumber, out int howManyPages)
  {
    // get a configured DbCommand object
    DbCommand comm = GenericDataAccess.CreateCommand();
    // set the stored procedure name
    comm.CommandText = "CatalogGetProductsInCategory";
    // create a new parameter
    DbParameter param = comm.CreateParameter();
    param.ParameterName = "@CategoryID";
    param.Value = categoryId;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@DescriptionLength";
    param.Value = BalloonShopConfiguration.ProductDescriptionLength;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@PageNumber";
    param.Value = pageNumber;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@ProductsPerPage";
    param.Value = BalloonShopConfiguration.ProductsPerPage;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@HowManyProducts";
    param.Direction = ParameterDirection.Output;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // execute the stored procedure and save the results in a DataTable
    DataTable table = GenericDataAccess.ExecuteSelectCommand(comm);
    // calculate how many pages of products and set the out parameter
    int howManyProducts = Int32.Parse(comm.Parameters["@HowManyProducts"].Value.ToString());
    howManyPages = (int)Math.Ceiling((double)howManyProducts /
                   (double)BalloonShopConfiguration.ProductsPerPage);
    // return the page of products
    return table;
  }

  // Search the product catalog
  public static DataTable Search(string searchString, string allWords, string pageNumber, out int howManyPages)
  {
    // get a configured DbCommand object
    DbCommand comm = GenericDataAccess.CreateCommand();
    // set the stored procedure name
    comm.CommandText = "SearchCatalog";
    // create a new parameter
    DbParameter param = comm.CreateParameter();
    param.ParameterName = "@DescriptionLength";
    param.Value = BalloonShopConfiguration.ProductDescriptionLength;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@AllWords";
    param.Value = allWords.ToUpper() == "TRUE" ? "True" : "False";
    param.DbType = DbType.Boolean;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@PageNumber";
    param.Value = pageNumber;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@ProductsPerPage";
    param.Value = BalloonShopConfiguration.ProductsPerPage;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@HowManyResults";
    param.Direction = ParameterDirection.Output;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);

    // define the maximum number of words
    int howManyWords = 5;
    // transform search string into array of words
    char[] wordSeparators = new char[] { ',', ';', '.', '!', '?', '-', ' ' };
    string[] words = searchString.Split(wordSeparators, StringSplitOptions.RemoveEmptyEntries);
    int index = 1;

    // add the words as stored procedure parameters
    for (int i = 0; i <= words.GetUpperBound(0) && index <= howManyWords; i++)
      // ignore short words
      if (words[i].Length > 2)
      {
        // create the @Word parameters
        param = comm.CreateParameter();
        param.ParameterName = "@Word" + index.ToString();
        param.Value = words[i];
        param.DbType = DbType.String;
        comm.Parameters.Add(param);
        index++;
      }

    // execute the stored procedure and save the results in a DataTable
    DataTable table = GenericDataAccess.ExecuteSelectCommand(comm);
    // calculate how many pages of products and set the out parameter
    int howManyProducts = Int32.Parse(comm.Parameters["@HowManyResults"].Value.ToString());
    howManyPages = (int)Math.Ceiling((double)howManyProducts /
                    (double)BalloonShopConfiguration.ProductsPerPage);
    // return the page of products
    return table;
  }

  // Update department details
  public static bool UpdateDepartment(string id, string name, string description)
  {
    // get a configured DbCommand object
    DbCommand comm = GenericDataAccess.CreateCommand();
    // set the stored procedure name
    comm.CommandText = "CatalogUpdateDepartment";
    // create a new parameter
    DbParameter param = comm.CreateParameter();
    param.ParameterName = "@DepartmentId";
    param.Value = id;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@DepartmentName";
    param.Value = name;
    param.DbType = DbType.String;
    param.Size = 50;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@DepartmentDescription";
    param.Value = description;
    param.DbType = DbType.String;
    param.Size = 1000;
    comm.Parameters.Add(param);
    // result will represent the number of changed rows
    int result = -1;
    try
    {
      // execute the stored procedure
      result = GenericDataAccess.ExecuteNonQuery(comm);
    }
    catch 
    { 
      // any errors are logged in GenericDataAccess, we ingore them here
    }
    // result will be 1 in case of success 
    return (result != -1);
  }

  // Delete department
  public static bool DeleteDepartment(string id)
  {
    // get a configured DbCommand object
    DbCommand comm = GenericDataAccess.CreateCommand();
    // set the stored procedure name
    comm.CommandText = "CatalogDeleteDepartment";
    // create a new parameter
    DbParameter param = comm.CreateParameter();
    param.ParameterName = "@DepartmentId";
    param.Value = id;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // execute the stored procedure; an error will be thrown by the
    // database in case the department has related categories, in which case
    // it is not deleted
    int result = -1;
    try
    {
      result = GenericDataAccess.ExecuteNonQuery(comm);
    }
    catch
    {
      // any errors are logged in GenericDataAccess, we ingore them here
    }
    // result will be 1 in case of success
    return (result != -1);
  }

  // Add a new department
  public static bool AddDepartment(string name, string description)
  {
    // get a configured DbCommand object
    DbCommand comm = GenericDataAccess.CreateCommand();
    // set the stored procedure name
    comm.CommandText = "CatalogAddDepartment";
    // create a new parameter
    DbParameter param = comm.CreateParameter();
    param.ParameterName = "@DepartmentName";
    param.Value = name;
    param.DbType = DbType.String;
    param.Size = 50;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@DepartmentDescription";
    param.Value = description;
    param.DbType = DbType.String;
    param.Size = 1000;
    comm.Parameters.Add(param);
    // result will represent the number of changed rows
    int result = -1;
    try
    {
      // execute the stored procedure
      result = GenericDataAccess.ExecuteNonQuery(comm);
    }
    catch
    {
      // any errors are logged in GenericDataAccess, we ingore them here
    }
    // result will be 1 in case of success 
    return (result != -1);
  }

  // Create a new Category
  public static bool CreateCategory(string departmentId, string name, string description)
  {
    // get a configured DbCommand object
    DbCommand comm = GenericDataAccess.CreateCommand();
    // set the stored procedure name
    comm.CommandText = "CatalogCreateCategory";
    // create a new parameter
    DbParameter param = comm.CreateParameter();
    param.ParameterName = "@DepartmentID";
    param.Value = departmentId;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@CategoryName";
    param.Value = name;
    param.DbType = DbType.String;
    param.Size = 50;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@CategoryDescription";
    param.Value = description;
    param.DbType = DbType.String;
    param.Size = 1000;
    comm.Parameters.Add(param);
    // result will represent the number of changed rows
    int result = -1;
    try
    {
      // execute the stored procedure
      result = GenericDataAccess.ExecuteNonQuery(comm);
    }
    catch
    {
      // any errors are logged in GenericDataAccess, we ingore them here
    }
    // result will be 1 in case of success 
    return (result != -1);
  }

  // Update category details
  public static bool UpdateCategory(string id, string name, string description)
  {
    // get a configured DbCommand object
    DbCommand comm = GenericDataAccess.CreateCommand();
    // set the stored procedure name
    comm.CommandText = "CatalogUpdateCategory";
    // create a new parameter
    DbParameter param = comm.CreateParameter();
    param.ParameterName = "@CategoryId";
    param.Value = id;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@CategoryName";
    param.Value = name;
    param.DbType = DbType.String;
    param.Size = 50;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@CategoryDescription";
    param.Value = description;
    param.DbType = DbType.String;
    param.Size = 1000;
    comm.Parameters.Add(param);
    // result will represent the number of changed rows
    int result = -1;
    try
    {
      // execute the stored procedure
      result = GenericDataAccess.ExecuteNonQuery(comm);
    }
    catch
    {
      // any errors are logged in GenericDataAccess, we ingore them here
    }
    // result will be 1 in case of success 
    return (result != -1);
  }

  // Delete Category
  public static bool DeleteCategory(string id)
  {
    // get a configured DbCommand object
    DbCommand comm = GenericDataAccess.CreateCommand();
    // set the stored procedure name
    comm.CommandText = "CatalogDeleteCategory";
    // create a new parameter
    DbParameter param = comm.CreateParameter();
    param.ParameterName = "@CategoryId";
    param.Value = id;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // execute the stored procedure; an error will be thrown by the
    // database in case the Category has related categories, in which case
    // it is not deleted
    int result = -1;
    try
    {
      result = GenericDataAccess.ExecuteNonQuery(comm);
    }
    catch (Exception e)
    {
      // any errors are logged in GenericDataAccess, we ingore them here
        Console.WriteLine(e.Message);
    }
    // result will be 1 in case of success
    return (result != -1);
  }

  // retrieve the list of products in a category
  public static DataTable GetAllProductsInCategory(string categoryId)
  {
    // get a configured DbCommand object
    DbCommand comm = GenericDataAccess.CreateCommand();
    // set the stored procedure name
    comm.CommandText = "CatalogGetAllProductsInCategory";
    // create a new parameter
    DbParameter param = comm.CreateParameter();
    param.ParameterName = "@CategoryID";
    param.Value = categoryId;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // execute the stored procedure and save the results in a DataTable
    DataTable table = GenericDataAccess.ExecuteSelectCommand(comm);
    return table;
  }

  // Create a new product
  public static bool CreateProduct(string categoryId, string name, string description, string price, string image1FileName, string image2FileName, string onDepartmentPromotion, string onCatalogPromotion)
  {
    // get a configured DbCommand object
    DbCommand comm = GenericDataAccess.CreateCommand();
    // set the stored procedure name
    comm.CommandText = "CatalogCreateProduct";
    // create a new parameter
    DbParameter param = comm.CreateParameter();
    param.ParameterName = "@CategoryID";
    param.Value = categoryId;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@ProductName";
    param.Value = name;
    param.DbType = DbType.String;
    param.Size = 50;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@ProductDescription";
    param.Value = description;
    param.DbType = DbType.AnsiString;
    param.Size = 5000;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@Price";
    param.Value = price;
    param.DbType = DbType.Decimal;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@Thumbnail";
    param.Value = image1FileName;
    param.DbType = DbType.String;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@Image";
    param.Value = image2FileName;
    param.DbType = DbType.String;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@PromoDept";
    param.Value = onDepartmentPromotion;
    param.DbType = DbType.Boolean;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@PromoFront";
    param.Value = onCatalogPromotion;
    param.DbType = DbType.Boolean;
    comm.Parameters.Add(param);
    // result will represent the number of changed rows
    int result = -1;
    try
    {
      // execute the stored procedure
      result = GenericDataAccess.ExecuteNonQuery(comm);
    }
    catch  
    {
      // any errors are logged in GenericDataAccess, we ingore them here
       
    }
    // result will be 1 in case of success 
    return (result >= 1);
  }

  // Update an existing product
  public static bool UpdateProduct(string productId, string name, string description, string price, string image1FileName, string image2FileName, string onDepartmentPromotion, string onCatalogPromotion)
  {
    // get a configured DbCommand object
    DbCommand comm = GenericDataAccess.CreateCommand();
    // set the stored procedure name
    comm.CommandText = "CatalogUpdateProduct";
    // create a new parameter
    DbParameter param = comm.CreateParameter();
    param.ParameterName = "@ProductID";
    param.Value = productId;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@ProductName";
    param.Value = name;
    param.DbType = DbType.String;
    param.Size = 50;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@ProductDescription";
    param.Value = description;
    param.DbType = DbType.AnsiString;
    param.Size = 5000;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@Price";
    param.Value = price;
    param.DbType = DbType.Decimal;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@Thumbnail";
    param.Value = image1FileName;
    param.DbType = DbType.String;
    param.Size = 50;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@Image";
    param.Value = image2FileName;
    param.DbType = DbType.String;
    param.Size = 50;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@PromoDept";
    param.Value = onDepartmentPromotion;
    param.DbType = DbType.Boolean;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@PromoFront";
    param.Value = onCatalogPromotion;
    param.DbType = DbType.Boolean;
    comm.Parameters.Add(param);
    // result will represent the number of changed rows
    int result = -1;
    try
    {
      // execute the stored procedure
      result = GenericDataAccess.ExecuteNonQuery(comm);
    }
    catch (Exception e)
    {
      // any errors are logged in GenericDataAccess, we ingore them here
        Console.WriteLine(e.Message);
    }
    // result will be 1 in case of success 
    return (result != -1);
  }

  // get categories that contain a specified product
  public static DataTable GetCategoriesWithProduct(string productId)
  {
    // get a configured DbCommand object
    DbCommand comm = GenericDataAccess.CreateCommand();
    // set the stored procedure name
    comm.CommandText = "CatalogGetCategoriesWithProduct";
    // create a new parameter
    DbParameter param = comm.CreateParameter();
    param.ParameterName = "@ProductID";
    param.Value = productId;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // execute the stored procedure
    return GenericDataAccess.ExecuteSelectCommand(comm);
  }

  // get categories that do not contain a specified product
  public static DataTable GetCategoriesWithoutProduct(string productId)
  {
    // get a configured DbCommand object
    DbCommand comm = GenericDataAccess.CreateCommand();
    // set the stored procedure name
    comm.CommandText = "CatalogGetCategoriesWithoutProduct";
    // create a new parameter
    DbParameter param = comm.CreateParameter();
    param.ParameterName = "@ProductID";
    param.Value = productId;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // execute the stored procedure
    return GenericDataAccess.ExecuteSelectCommand(comm);
  }

  // assign a product to a new category
  public static bool AssignProductToCategory(string productId, string categoryId)
  {
    // get a configured DbCommand object
    DbCommand comm = GenericDataAccess.CreateCommand();
    // set the stored procedure name
    comm.CommandText = "CatalogAssignProductToCategory";
    // create a new parameter
    DbParameter param = comm.CreateParameter();
    param.ParameterName = "@ProductID";
    param.Value = productId;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@CategoryID";
    param.Value = categoryId;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // result will represent the number of changed rows
    int result = -1;
    try
    {
      // execute the stored procedure
      result = GenericDataAccess.ExecuteNonQuery(comm);
    }
    catch (Exception e)
    {
      // any errors are logged in GenericDataAccess, we ingore them here
        Console.WriteLine(e.Message);
    }
    // result will be 1 in case of success 
    return (result != -1);
  }

  // move product to a new category
  public static bool MoveProductToCategory(string productId, string oldCategoryId, string newCategoryId)
  {
    // get a configured DbCommand object
    DbCommand comm = GenericDataAccess.CreateCommand();
    // set the stored procedure name
    comm.CommandText = "CatalogMoveProductToCategory";
    // create a new parameter
    DbParameter param = comm.CreateParameter();
    param.ParameterName = "@ProductID";
    param.Value = productId;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@OldCategoryID";
    param.Value = oldCategoryId;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@NewCategoryID";
    param.Value = newCategoryId;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // result will represent the number of changed rows
    int result = -1;
    try
    {
      // execute the stored procedure
      result = GenericDataAccess.ExecuteNonQuery(comm);
    }
    catch
    {
      // any errors are logged in GenericDataAccess, we ingore them here
    }
    // result will be 1 in case of success 
    return (result != -1);
  }

  // removes a product from a category 
  public static bool RemoveProductFromCategory(string productId, string categoryId)
  {
    // get a configured DbCommand object
    DbCommand comm = GenericDataAccess.CreateCommand();
    // set the stored procedure name
    comm.CommandText = "CatalogRemoveProductFromCategory";
    // create a new parameter
    DbParameter param = comm.CreateParameter();
    param.ParameterName = "@ProductID";
    param.Value = productId;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@CategoryID";
    param.Value = categoryId;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // result will represent the number of changed rows
    int result = -1;
    try
    {
      // execute the stored procedure
      result = GenericDataAccess.ExecuteNonQuery(comm);
    }
    catch
    {
      // any errors are logged in GenericDataAccess, we ingore them here
    }
    // result will be 1 in case of success 
    return (result != -1);
  }

  // deletes a product from the product catalog
  public static bool DeleteProduct(string productId)
  {
    // get a configured DbCommand object
    DbCommand comm = GenericDataAccess.CreateCommand();
    // set the stored procedure name
    comm.CommandText = "CatalogDeleteProduct";
    // create a new parameter
    DbParameter param = comm.CreateParameter();
    param.ParameterName = "@ProductID";
    param.Value = productId;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // result will represent the number of changed rows
    int result = -1;
    try
    {
      // execute the stored procedure
      result = GenericDataAccess.ExecuteNonQuery(comm);
    }
    catch
    {
      // any errors are logged in GenericDataAccess, we ingore them here
    }
    // result will be 1 in case of success 
    return (result != -1);
  }
    public static DataTable GetReviews(int productId)
    {
        // get a configured DbCommand object
        DbCommand comm = GenericDataAccess.CreateCommand();
        // set the stored procedure name
        comm.CommandText = "ReviewList";
        // create a new parameter
        DbParameter param = comm.CreateParameter();
        param.ParameterName = "@ProductID";
        param.Value = productId;
        param.DbType = DbType.Int32;
        comm.Parameters.Add(param);

        // execute the stored procedure and save the results in a DataTable
        DataTable table = GenericDataAccess.ExecuteSelectCommand(comm);

        // return the page of products
        return table;
    }


    public static bool CreateReview(string userID, int productId, string name, string desc)
    {
        // get a configured DbCommand object
        DbCommand comm = GenericDataAccess.CreateCommand();
        // set the stored procedure name
        comm.CommandText = "ReviewCreate";
        // create a new parameter
        DbParameter param = comm.CreateParameter();
        param.ParameterName = "@UserID";
        param.Value = userID;
        param.DbType = DbType.String;
        param.Size = 30;
        comm.Parameters.Add(param);
        // create a new parameter
        param = comm.CreateParameter();
        param.ParameterName = "@ProductID";
        param.Value = productId;
        param.DbType = DbType.Int32;
        comm.Parameters.Add(param);
        // create a new parameter
        param = comm.CreateParameter();
        param.ParameterName = "@Name";
        param.Value = name;
        param.DbType = DbType.String;
        param.Size = 200;
        comm.Parameters.Add(param);
        // create a new parameter
        param = comm.CreateParameter();
        param.ParameterName = "@Description";
        param.Value = desc;
        param.DbType = DbType.String;
        param.Size = 1000;
        comm.Parameters.Add(param);

        // result will represent the number of changed rows
        int result = -1;
        try
        {
            // execute the stored procedure
            result = GenericDataAccess.ExecuteNonQuery(comm);
        }
        catch
        {
            // any errors are logged in GenericDataAccess, we ingore them here
        }
        // result will be 1 in case of success 
        return (result != -1);
    }

    // Delete a WishLIst
    public static bool DeleteReview(string userID, int productID)
    {
        // get a configured DbCommand object
        DbCommand comm = GenericDataAccess.CreateCommand();
        // set the stored procedure name
        comm.CommandText = "ReviewDelete";
        // create a new parameter
        DbParameter param = comm.CreateParameter();
        param.ParameterName = "@UserID";
        param.Value = userID;
        param.DbType = DbType.String;
        param.Size = 30;
        comm.Parameters.Add(param);
        // create a new parameter
        param = comm.CreateParameter();
        param.ParameterName = "@ProductID";
        param.Value = productID;
        param.DbType = DbType.Int32;
        comm.Parameters.Add(param);

        // result will represent the number of changed rows
        int result = -1;
        try
        {
            // execute the stored procedure
            result = GenericDataAccess.ExecuteNonQuery(comm);
        }
        catch
        {
            // any errors are logged in GenericDataAccess, we ingore them here
        }
        // result will be 1 in case of success 
        return (result != -1);
    }

    // gets product recommendations
    public static DataTable GetRecommendations(string productId)
  {
    // get a configured DbCommand object
    DbCommand comm = GenericDataAccess.CreateCommand();
    // set the stored procedure name
    comm.CommandText = "CatalogGetProductRecommendations";
    // create a new parameter
    DbParameter param = comm.CreateParameter();
    param.ParameterName = "@ProductID";
    param.Value = productId;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@DescriptionLength";
    param.Value = BalloonShopConfiguration.ProductDescriptionLength;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // execute the stored procedure
    return GenericDataAccess.ExecuteSelectCommand(comm);
  }
public static DataTable GetProductReviews(string productId)
{
// get a configured DbCommand object
DbCommand comm = GenericDataAccess.CreateCommand();
// set the stored procedure name
comm.CommandText = "CatalogGetProductReviews";
// create a new parameter
DbParameter param = comm.CreateParameter();
param.ParameterName = "@ProductID";
param.Value = productId;
param.DbType = DbType.Int32;
comm.Parameters.Add(param);
// execute the stored procedure
return GenericDataAccess.ExecuteSelectCommand(comm);
}
// Add a new shopping cart item
public static bool AddReview(string productId, string review)
{
// get a configured DbCommand object
DbCommand comm = GenericDataAccess.CreateCommand();
// set the stored procedure name
comm.CommandText = "CatalogAddProductReview ";
// create a new parameter
DbParameter param = comm.CreateParameter();
param.DbType = DbType.String;
comm.Parameters.Add(param);
// create a new parameter
param = comm.CreateParameter();
param.ParameterName = "@ProductID";
param.Value = productId;
param.DbType = DbType.Int32;
comm.Parameters.Add(param);
// create a new parameter
param = comm.CreateParameter();
param.ParameterName = "@Review";
param.Value = review;
param.DbType = DbType.String;
comm.Parameters.Add(param);
// returns true in case of success or false in case of an error
try
{
    // execute the stored procedure and return true if it executes
    // successfully, or false otherwise
    return (GenericDataAccess.ExecuteNonQuery(comm) != -1);
}
catch
{
    // prevent the exception from propagating, but return false to
    // signal the error
    return false;
}
}
}