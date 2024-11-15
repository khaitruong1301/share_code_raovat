using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using WebsiteRaoVat.Models;
using Facebook;
using System.Configuration;
using System.Web.Security;
using System.IO;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Net.Mail;

namespace WebsiteRaoVat.Controllers
{
    public class LoginController : Controller
    {
        RaoVatDB db = new RaoVatDB();
        // GET: Login
        public ActionResult login()
        {
            return PartialView();
        }
        //[HttpPost]
        // đăng nhập thường
        //public ActionResult login(TaiKhoan tk)
        //{

        //    var a = GetMD5(tk.Password).ToString();

        //    var tv = (from c in db.TaiKhoans where c.Username == tk.Username && c.Password == tk.Password select c).FirstOrDefault();
        //    if (tv != null)
        //    {
        //        Session["TaiKhoan"] = tv;
        //        return RedirectToAction("Index", "Home");
        //    }
        //    return View();
            
        //}
        
        public JsonResult KTDangNhap(string username, string password)
        {
            try
            {
                var a = GetMD5(password).ToString();
                var taikhoan = (from t in db.TaiKhoans where t.Username == username && t.Password == a select t).FirstOrDefault();
                int thanhcong = 1;
                int quyen = 0;
                if(taikhoan != null)
                {
                    if(taikhoan.Quyen == 1)
                    {
                        quyen = 1;
                    }
                    Session["TaiKhoan"] = taikhoan;
                }
                else
                {
                    thanhcong = 0;
                }
                return Json(new { code = 200, thanhcong = thanhcong, quyen = quyen}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        //Đăng nhập bằng facebook
        public Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }

        public ActionResult loginFacebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["FbAppID"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                respone_type = "code",
                scope = "email",
            });

            return Redirect(loginUrl.AbsoluteUri);
        }

        public string InsertLoginFacebook(TaiKhoan enty)
        {
            var user = db.TaiKhoans.SingleOrDefault(n => n.Username == enty.Username );
            if (user == null)
            {
                db.TaiKhoans.Add(enty);
                db.SaveChanges();
                return enty.Username;
            }

            return user.Username;

        }

        //save ảnh
        public void SaveImage(string imageUrl, string filename, ImageFormat format)
        {
            WebClient client = new WebClient();
            Stream stream = client.OpenRead(imageUrl);
            Bitmap bitmap; bitmap = new Bitmap(stream);

            if (bitmap != null)
            {
                bitmap.Save(filename, format);
            }

            try
            {
                SaveImage(imageUrl, "~/Content/Images/", ImageFormat.Png);
}
            catch (ExternalException)
            {
                // Something is wrong with Format -- Maybe required Format is not 
                // applicable here
            }
            catch (ArgumentNullException)
            {
                // Something wrong with Stream
            }
            stream.Flush();
            stream.Close();
            client.Dispose();
        }

        //lấy ra url hình ảnh
        public static string GetPictureUrl(string faceBookId)
        {
            WebResponse response = null;
            string pictureUrl = string.Empty;
            try
            {
                WebRequest request = WebRequest.Create(string.Format("https://graph.facebook.com/{0}/picture", faceBookId));
                response = request.GetResponse();
                pictureUrl = response.ResponseUri.ToString();
            }
            catch (Exception ex)
            {
                //? handle
            }
            finally
            {
                if (response != null) response.Close();
            }
            return pictureUrl;
        }

        //https://localhost:44349/Login/FacebookCallback
        //https://localhost:44349/Login/loginFacebook
        //https://localhost:44349/Login/RedirectUri
        //https://localhost:44349/

        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["FbAppID"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });

            var accessToken = result.access_token;
            if (!string.IsNullOrEmpty(accessToken))
            {
                fb.AccessToken = accessToken;
                dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,picture,id,email");
                string id = me.id;
                string email = me.email;
                string firstname = me.first_name;
                string lastname = me.last_name;
                string middlename = me.middle_name;
                var hinh = me.picture;
                
                var user = new TaiKhoan();
                user.Username = id;
                user.Email = email;
                //var a = GetMD5(email).ToString();
                //user.Password = email;
                user.NgayThamGia = DateTime.Now;
                user.TenNguoiDung = lastname + " " + middlename + " " + firstname;
                //user.Quyen = 3;
                var anh = GetPictureUrl(id);
                //SaveImage(anh, email, ImageFormat.Png);

                //using (WebClient webClient = new WebClient())
                //{
                //    byte[] data = webClient.DownloadData(anh);

                //    using (MemoryStream mem = new MemoryStream(data))
                //    {
                //        using (var yourImage = Image.FromStream(mem))
                //        {
                //            // If you want it as Png
                //            yourImage.Save(@"~/Content/Images/"+middlename+".png", ImageFormat.Png);

                //            // If you want it as Jpeg
                //            yourImage.Save("~/Content/Images/", ImageFormat.Jpeg);
                //        }
                //    }

                //}

                var resultInsert = (InsertLoginFacebook(user).ToString());
                if (resultInsert != null)
                {
                    //var tv = (from c in db.TaiKhoans where c.Username == user.Username && c.Password== user.Password select c).FirstOrDefault();
                    var tv = (from c in db.TaiKhoans where c.Username == user.Username select c).FirstOrDefault();
                    if (tv != null)
                    {
                        Session["TaiKhoan"] = tv;
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return View();
        }

        //mã hóa md5
        public string GetMD5(string chuoi)
        {
            string str_md5 = "";
            byte[] mang = System.Text.Encoding.UTF8.GetBytes(chuoi);

            MD5CryptoServiceProvider my_md5 = new MD5CryptoServiceProvider();
            mang = my_md5.ComputeHash(mang);

            foreach (byte b in mang)
            {
                str_md5 += b.ToString("X2");
            }

            return str_md5;
        }

        //Đăng xuất
        public ActionResult DangXuat()
        {
            Session["Taikhoan"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
       
        public ActionResult changepass()
        { 
            return PartialView();
        }
        public string CreateLostPassword(int PasswordLength)
        {
            string _allowedChars = "abcdefghijk0123456789mnopqrstuvwxyz";
            Random randNum = new Random();
            char[] chars = new char[PasswordLength];
            int allowedCharCount = _allowedChars.Length;
            for (int i = 0; i < PasswordLength; i++)
            {
                chars[i] = _allowedChars[(int)((_allowedChars.Length) * randNum.NextDouble())];
            }
            return new string(chars);
        }
    [HttpPost]
        public JsonResult createpass(string maxn, string pass)
        {
            try
            {
                String b = Session["Email"].ToString();
                var newpass = (from t in db.TaiKhoans where t.Email == b select t).SingleOrDefault();
                string macode = Session["Ma"].ToString();
                if (macode.Equals(maxn))
                {
                    if (macode != maxn)
                    {
                        return Json(new { code = 500, msg = "mã code sai" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {


                        
                        newpass.Password = GetMD5(pass);
                        db.SaveChanges();

                        Session["Ma"] = "";

                        return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
                    }

                }
            }
            catch
            {
                return Json(new { code = 500, msg = "Không thành công" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
         }

    
    [HttpPost]
        public JsonResult changepass(string email1)
        {

            try
            {
                var Tk = new RaoVatDB().TaiKhoans.Where(x => x.Email == email1).FirstOrDefault();

                if (Tk != null)
                {
                    string ma = CreateLostPassword(4);

                    SendEmail(email1, "Xác nhận thay đổi mật khẩu","Mật khẩu mới của bạn là: "+ma);
                    Session["Ma"] = ma;
                    Session["Email"] = email1;
                }
                else
                {
                    return Json(new { code = 500, msg = "Mail khong toon tai" }, JsonRequestBehavior.AllowGet);
                }


                return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Không thành công" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        //[HttpPost]
        //public JsonResult changepass(string email1)
        //{

        //    try
        //    {
        //        var Tk = new RaoVatDB().TaiKhoans.Where(x => x.Email == email1).FirstOrDefault();

        //        if (Tk != null)
        //        {


        //            SendEmail(email1, "Xác nhận thay đổi mật khẩu", "Please reset your password by clicking <a class='btn btn-success' href ='https://localhost:44349/Login/resetpass' >Xác Nhận</a> ");

        //        }
        //        else
        //        {
        //            return Json(new { code = 500, msg = "Mail khong toon tai" }, JsonRequestBehavior.AllowGet);
        //        }


        //        return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);

        //    }
        //    catch (Exception e)
        //    {
        //        return Json(new { code = 500, msg = "Không thành công" + e.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}
        //public string SendEmail(string userEmail, string confirmationLink, string Subject)
        //{
        //    MailMessage mailMessage = new MailMessage();
        //    mailMessage.From = new MailAddress("dieuvothi635@mail.com");
        //    mailMessage.To.Add(userEmail);
        //    mailMessage.Subject = Subject;
        //    mailMessage.IsBodyHtml = true;
        //    mailMessage.Body = confirmationLink;

        //    try
        //    {
        //        using (SmtpClient client = new SmtpClient("smtp.gmail.com"))
        //        {
        //            //client.Port = 587;
        //            //client.UseDefaultCredentials = false;
        //            //// Tạo xác thực bằng địa chỉ gmail và password
        //            //client.Credentials = new NetworkCredential("websitefashi@gmail.com", "fashi*123*");
        //            //client.EnableSsl = true;
        //            //client.Send(mailMessage);

        //            client.EnableSsl = true;
        //            client.UseDefaultCredentials = false;
        //            client.Credentials = new NetworkCredential("dieuvothi635@mail.com", "Dieu12345");
        //            client.Port = 587;
        //            client.DeliveryMethod = SmtpDeliveryMethod.Network;
        //            client.Send(mailMessage);
        //        }
        //        return "" ;
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //        // log exception
        //    }


        //}



        public void SendEmail(string address, string subject, string message)
        {
            string email = "18110269@student.hcmute.edu.vn";
            string password = "19022000Dinh";

            var email1 = new NetworkCredential(email, password);
            var msg = new System.Net.Mail.MailMessage();
            var smtpClient = new SmtpClient("smtp.gmail.com", 587);

            msg.From = new MailAddress(email);
            msg.To.Add(new MailAddress(address));
            msg.Subject = subject;
            msg.Body = message;
            msg.IsBodyHtml = true;

            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = email1;
            smtpClient.Send(msg);
        }

        public ActionResult resetpass()
        {
            return PartialView();
        }
        public ActionResult Register()
        {
            return PartialView();
        }
        public JsonResult DangKyTK(string username, string password, string hoten, string sdt)
        {
            try
            {
                var taikhoan = (from t in db.TaiKhoans where t.Username == username select t).Count();
                int thanhcong = 1;
                if(taikhoan > 0)
                {
                    thanhcong = 0;
                }
                else
                {
                    TaiKhoan tk = new TaiKhoan();
                    tk.Username = username;
                    tk.Password = GetMD5(password);
                    tk.TenNguoiDung = hoten;
                    tk.SDT = sdt;
                    tk.NgayThamGia = DateTime.Now;
                    db.TaiKhoans.Add(tk);
                    db.SaveChanges();
                }
                return Json(new { code = 200 , thanhcong = thanhcong}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        
        public JsonResult GoogleLogin(string Username, string TenNguoiDung,string Email, string Hinh)
        {
            try
            {
                var taikhoan = (from c in db.TaiKhoans where c.Username == Username select c).FirstOrDefault();
                if (taikhoan != null)
                {
                    Session["TaiKhoan"] = taikhoan;
                }
                else
                {
                    TaiKhoan tk = new TaiKhoan();
                    tk.Username = Username;
                    tk.TenNguoiDung = TenNguoiDung;
                    tk.Email = Email;
                    tk.Hinh = Hinh;
                    tk.NgayThamGia = DateTime.Now;
                    db.TaiKhoans.Add(tk);
                    db.SaveChanges();
                    Session["TaiKhoan"] = tk;
                }
                
                return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }

}