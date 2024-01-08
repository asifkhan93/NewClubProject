using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using NewClubProject.Models;

namespace NewClubProject.Controllers
{
    public class HomeController : Controller
    {
        private SqlTransaction _transaction;
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
        Database_access_layer.dbAccess dblayer = new Database_access_layer.dbAccess();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ClubInfo()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public JsonResult getMembersInfo() 
        {
            DataSet ds = dblayer.ShowMemberInfo();
            List<ClubMemberInfoModel> listing = new List<ClubMemberInfoModel>();
            foreach (DataRow dr in ds.Tables[0].Rows) 
            {
                listing.Add(new ClubMemberInfoModel 
                {
                    MemberId = Convert.ToInt32(dr["Id"]),
                    MemberName = dr["Name"].ToString(),
                    MemberAddress = dr["Address"].ToString(),
                    MobileNumber = Convert.ToInt32(dr["MobileNumber"]),
                    NID = Convert.ToInt32(dr["NID"]),
                    ClubId = Convert.ToInt32(dr["ClubID"]),
                    ClubName = dr["ClubName"].ToString()
                });
               
            }
            var data= listing;
            return Json(listing, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getClubInfo()
        {
            DataSet ds = dblayer.ShowClubList();
            List<ClubMemberInfoModel> listing = new List<ClubMemberInfoModel>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                listing.Add(new ClubMemberInfoModel
                {
                    ClubId = Convert.ToInt32(dr["Id"]),
                    ClubName = dr["ClubName"].ToString(),
                    ClubAddress = dr["ClubAddress"].ToString()
                });

            }
            
            return Json(listing, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult saveData(List<ClubMemberInfoModel> JMemberList)
        {
            try
            {
                con.Open();
                _transaction = con.BeginTransaction();
                foreach (var member in JMemberList)
                {
                    if (!IsDuplicateNID(con, member.NID))
                    {
                        string query = "INSERT INTO PersonalInfo (Name, MobileNumber, NID, Address, ClubId, ClubName) VALUES (@Name, @MobileNumber, @NID,  @Address, @ClubId, @ClubName)";
                        var cmd = new SqlCommand(query, con, _transaction);
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@Name", member.MemberName);
                        cmd.Parameters.AddWithValue("@MobileNumber", member.MobileNumber);
                        cmd.Parameters.AddWithValue("@NID", member.NID);
                        cmd.Parameters.AddWithValue("@Address", member.MemberAddress);
                        cmd.Parameters.AddWithValue("@ClubId", member.ClubId);
                        cmd.Parameters.AddWithValue("@ClubName", member.ClubName);
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        return Json("NID already Exists.");
                    }

                }

                _transaction.Commit();
                con.Close();
                return Json("Data Inserted", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("Error..! No Data Inserted", JsonRequestBehavior.AllowGet);
            }
        }

        public bool IsDuplicateNID(SqlConnection con, int Nid) 
        {
            string query = "SELECT COUNT(*) From PersonalInfo where NID = @Nid";
            var cmd = new SqlCommand(query, con, _transaction);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@NID", Nid);

            int count = (int)cmd.ExecuteScalar();
            return count > 0; 
        }


        [HttpPost]
        public JsonResult AddClub(List<ClubMemberInfoModel> JClubList)
        {
            try
            {
                con.Open();
                _transaction = con.BeginTransaction();

                foreach (var club in JClubList) {

                    if (!IsDuplicateClub(con, club.ClubName)) 
                    {
                        const string query = @"INSERT INTO ClubInfo (ClubName, ClubAddress) OUTPUT INSERTED.ID VALUES (@ClubName, @ClubAddress)";
                        var cmd = new SqlCommand(query, con, _transaction);
                        cmd.Parameters.Clear();

                        cmd.Parameters.AddWithValue("@ClubName", club.ClubName);
                        cmd.Parameters.AddWithValue("@ClubAddress", club.ClubAddress);
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        return Json("Club already Exists.");
                    }

                }


                _transaction.Commit();
                con.Close();
                return Json("Data Inserted", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("Error..!", JsonRequestBehavior.AllowGet);
            }
        }

        public bool IsDuplicateClub(SqlConnection con, string clubName)
        {
            string query = "SELECT COUNT(*) From PersonalInfo where ClubName = @clubName";
            var cmd = new SqlCommand(query, con, _transaction);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@clubName", clubName);

            int count = (int)cmd.ExecuteScalar();
            return count > 0;
        }

    }
}