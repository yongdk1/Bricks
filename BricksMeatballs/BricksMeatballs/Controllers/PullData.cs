using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

/// <summary>
/// Author: Huang Chaoshan, Lim Pei Yan
/// The PullData class get the dataset from the local MySQL database
/// </summary>
public class PullData
{
    // your data table
    DataSet ds = new DataSet();
    public IConfiguration Configuration { get; }
    public static string ConectionString = "server=localhost; username=root;password=2012;database=bricks";

    /// <summary>
    /// This method define the configuration
    /// </summary>
    /// <param name="configuration"></param>
    public PullData(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    /// <summary>
    /// The LoadData method is to pull data from database to datatable
    /// </summary>
    /// <param name="query"></param>
    /// <param name="dsName"></param>
    /// <returns></returns>
    public DataTable LoadData(string query, string dsName)
    {
        MySqlConnection objConn = new MySqlConnection(ConectionString);
        objConn.Open();
        MySqlCommand command = new MySqlCommand(query, objConn);
        MySqlDataAdapter adapter = new MySqlDataAdapter(query, objConn) {
            SelectCommand = new MySqlCommand(query, objConn)
        };
        DataTable dt = new DataTable();
        adapter.Fill(ds, dsName);
        if (ds.Tables.Count > 1)
        {
            for (int i = 1; i < ds.Tables.Count; i++)
            {
                ds.Tables[dsName].Merge(ds.Tables[i]);
            }
            objConn.Close();
            return ds.Tables[dsName];

        }

        objConn.Close();
        return ds.Tables[dsName];

    }

    /// <summary>
    /// The method will get the dropdown data selected by user and bind them with the value inside the database
    /// </summary>
    /// <param name="query"></param>
    /// <param name="ChooseName"></param>
    /// <returns></returns>
    public List<SelectListItem> BindDropDownListCustomer(string query,string ChooseName)
    {
        MySqlConnection objConn = new MySqlConnection(ConectionString);
        objConn.Open();

        List<SelectListItem> items = new List<SelectListItem>();
        if (ds.Tables["tblItem"] != null)
        {
            ds.Tables["tblItem"].Clear();
        }
        string sr = " " + query + " ";
        MySqlDataAdapter sqlda = new MySqlDataAdapter(sr, objConn);
        sqlda.SelectCommand = new MySqlCommand(sr, objConn);
        sqlda.Fill(ds, "tblItem");

        items.Add(new SelectListItem { Text = ChooseName, Value = "" });
        if (ds.Tables["tblItem"].Rows.Count > 0)
        {
            for (int j = 0; j < ds.Tables["tblItem"].Rows.Count; j++)
            {
                items.Add(new SelectListItem { Text = "" + ds.Tables["tblItem"].Rows[j]["Name"].ToString() + "", Value = "" + ds.Tables["tblItem"].Rows[j]["Name"].ToString() + "" });
            }
        }
        objConn.Close();
        objConn.Dispose();
        return items;
    }
}