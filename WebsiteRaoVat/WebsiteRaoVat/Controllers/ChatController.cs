using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteRaoVat.Hubs;
using WebsiteRaoVat.Models;

namespace WebsiteRaoVat.Controllers
{
    public class ChatController : Controller
    {
        // GET: Chat
        private RaoVatDB db = new RaoVatDB();
        public ActionResult Chat()
        {
            TaiKhoan taikhoan = (TaiKhoan)Session["TaiKhoan"];

            var mess = db.CuoiHoiThoais.Where(x => x.NguoiNhan.Equals(taikhoan.Username) || x.NguoiGui.Equals(taikhoan.Username)).ToList();

            List<TaiKhoan> taiKhoans = new List<TaiKhoan>();
            foreach (var item in mess.OrderByDescending(x => x.ThoiGianGui))
            {
                TaiKhoan taiKhoan = new TaiKhoan();
                if (item.NguoiGui.Equals(taikhoan.Username))
                {
                    if (!taiKhoans.Exists(x => x.Username.Equals(item.NguoiNhan)))
                    {

                        taiKhoans.Add(db.TaiKhoans.Find(item.NguoiNhan));

                    }

                }
                else
                {
                    if (!taiKhoans.Exists(x => x.Username.Equals(item.NguoiGui)))
                    {

                        taiKhoans.Add(db.TaiKhoans.Find(item.NguoiGui));

                    }
                }
            }
            if(taiKhoans.Count > 0)
            {
                ViewBag.ChatUser = taiKhoans.First().Username;
                ViewBag.Ten = taiKhoans.First().TenNguoiDung;
            }
            //return View(taiKhoans.First());
            return View();
        }

        public JsonResult getCuocHoiThoai(string username)
        {
            try
            {
                TaiKhoan taikhoan = (TaiKhoan)Session["TaiKhoan"];             
                var hoithoai = (from h in db.CuoiHoiThoais
                                where (h.NguoiGui == taikhoan.Username && h.NguoiNhan == username)|| (h.NguoiNhan == taikhoan.Username && h.NguoiGui == username) 
                                select h);
                //SqlDependency dependency = new SqlDependency();
                return Json(new { code = 200, listhoithoai = hoithoai}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult getTaiKhoan()
        {
            try
            {
                TaiKhoan taikhoan = (TaiKhoan)Session["TaiKhoan"];

                var mess = db.CuoiHoiThoais.Where(x => x.NguoiNhan.Equals(taikhoan.Username) || x.NguoiGui.Equals(taikhoan.Username)).ToList();

                List<TaiKhoan> taiKhoans = new List<TaiKhoan>();
                foreach (var item in mess.OrderByDescending(x => x.ThoiGianGui))
                {
                    TaiKhoan taiKhoan = new TaiKhoan();
                    if (item.NguoiGui.Equals(taikhoan.Username))
                    {
                        if (!taiKhoans.Exists(x => x.Username.Equals(item.NguoiNhan)))
                        {

                            taiKhoans.Add(db.TaiKhoans.Find(item.NguoiNhan));
                        }

                    }
                    else
                    {
                        if (!taiKhoans.Exists(x => x.Username.Equals(item.NguoiGui)))
                        {

                            taiKhoans.Add(db.TaiKhoans.Find(item.NguoiGui));

                        }
                    }
                }
                var c = (from v in taiKhoans select new { Username = v.Username, TenNguoiDung = v.TenNguoiDung , Hinh = v.Hinh}).ToList();
                return Json(new { code = 200, listtaikhoan = c }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        //public JsonResult getData(string username)
        //{
        //    TaiKhoan taikhoan = (TaiKhoan)Session["TaiKhoan"];
        //    using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["RaoVatDB"].ConnectionString))
        //    {
        //        connection.Open();
        //        using (SqlCommand command = new SqlCommand(@"select * from CuoiHoiThoai where NguoiNhan = '"+username+"')", connection))
        //        {
        //            // Make sure the command object does not already have
        //            // a notification object associated with it.
        //            command.Notification = null;

        //            SqlDependency dependency = new SqlDependency(command);
        //            dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

        //            if (connection.State == ConnectionState.Closed)
        //                connection.Open();

        //            SqlDataReader reader = command.ExecuteReader();

        //            var listCus = reader.Cast<IDataRecord>()
        //                    .Select(x => new
        //                    {
        //                        //Id = (int)x["Id"],
        //                        //CusId = (string)x["CusId"],
        //                        //CusName = (string)x["CusName"],
        //                        MaHoiThoai = (int)x["MaHoiThoai"],
        //                        nguoigui = (string)x["NguoiGui"],
        //                        nguoinhan = (string)x["NguoiNhan"],
        //                        noidung = (string)x["NoiDung"],
        //                        thoigiangui = (string)x["ThoiGianGui"],
        //                    }).ToList();

        //            return Json(new { listhoithoai = listCus}, JsonRequestBehavior.AllowGet);

        //        }
        //    }

        //}
        //private void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        //{
        //    CuocHoiThoaiHub.Show();
        //}
        public JsonResult AddMessage(string username, string noidung, string hinh)
        {
            try
            {
                TaiKhoan taikhoan = (TaiKhoan)Session["TaiKhoan"];
                CuoiHoiThoai ht = new CuoiHoiThoai();
                ht.NguoiGui = taikhoan.Username;
                ht.NguoiNhan = username;
                if (noidung != "")
                {
                    ht.NoiDung = noidung;
                }
                if (hinh != "")
                {
                    ht.Hinh = hinh;
                }
                ht.ThoiGianGui = DateTime.Now;
                db.CuoiHoiThoais.Add(ht);
                db.SaveChanges();
                return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult XoaHoiThoai(string username)
        {
            try
            {
                TaiKhoan taikhoan = (TaiKhoan)Session["TaiKhoan"];
                var hoithoai = (from h in db.CuoiHoiThoais
                                where (h.NguoiGui == taikhoan.Username && h.NguoiNhan == username) || (h.NguoiNhan == taikhoan.Username && h.NguoiGui == username)
                                select h);
                db.CuoiHoiThoais.RemoveRange(hoithoai);
                db.SaveChanges();
                return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }   
}