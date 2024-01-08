using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using NewClubProject.Models;

namespace NewClubProject.Database_access_layer
{
    public class dbAccess
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);

        public DataSet  ShowMemberInfo()
        {
            SqlCommand com = new SqlCommand("SELECT * FROM PersonalInfo", con);

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(com);
            da.Fill(ds);
            return ds;

        }

        public DataSet ShowClubList() 
        {
            SqlCommand com = new SqlCommand("Select * from ClubInfo", con);

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(com);
            da.Fill(ds);
            return ds;
        }
    }
}