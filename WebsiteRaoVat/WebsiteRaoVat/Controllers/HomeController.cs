using MoMo;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebsiteRaoVat.Models;



namespace WebsiteRaoVat.Controllers
{
    public class HomeController : Controller
    {
        private RaoVatDB db = new RaoVatDB();
        public ActionResult Index()
        {
            var listDM = (from d in db.DanhMucs select d).ToList();
            return View(listDM);
        }
        public ActionResult DangTin()
        {
            List<DanhMuc> lstDanhMuc = db.DanhMucs.ToList();
            return View(lstDanhMuc);

        }
        public JsonResult AddBinhLuan(int idbaidang, string noidung)
        {

            try
            {

                TaiKhoan taikhoan = (TaiKhoan)Session["TaiKhoan"];
                if (taikhoan.Username != "null")
                {

                    ViewBag.Session = taikhoan.Username;
                    BinhLuan binhLuan = new BinhLuan();

                    binhLuan.Username = taikhoan.Username;
                    binhLuan.NoiDung = noidung;
                    binhLuan.MaBaiDang = idbaidang;
                    binhLuan.ParentId = 0;
                    db.BinhLuans.Add(binhLuan);

                    db.SaveChanges();


                }
                else
                {
                    return Json(new { code = 500, }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { code = 200, msg = "Bình Luận thành công" }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult AddChildBinhLuan(string noidung, int idbl, string username)
        {

            try
            {

                TaiKhoan taikhoan = (TaiKhoan)Session["TaiKhoan"];

                if (taikhoan.Username != "null")
                {

                    ViewBag.Session = taikhoan.Username;

                    ChildComment binhLuan = new ChildComment();
                    binhLuan.Username = taikhoan.Username;
                    binhLuan.MaBL = idbl;
                    binhLuan.NoiDung = noidung;

                    db.ChildComments.Add(binhLuan);

                    db.SaveChanges();

                }
                else
                {
                    return Json(new { code = 500, }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { code = 200, msg = "Bình Luận thành công" }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public JsonResult EditCommnent(int idbaidang, int mabl, string noidung, string username)
        {

            try
            {

                TaiKhoan taikhoan = (TaiKhoan)Session["TaiKhoan"];
                var binhluan = (from c in db.BinhLuans where c.MaBL == mabl && c.Username == username select c).SingleOrDefault();

                binhluan.NoiDung = noidung;


                db.SaveChanges();
                return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Không thành công" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult XoaBinhLuan(int mabl)
        {
            try
            {
                BinhLuan bl = db.BinhLuans.Where(c => c.MaBL == mabl).FirstOrDefault();
                db.BinhLuans.Remove(bl);
                db.SaveChanges();

                return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Không thành công" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        //SỬA XÓA CMT CON
        public JsonResult EditChildCommnent(int idbaidang, int mabl, string noidung, int mablparent)
        {

            try
            {

                TaiKhoan taikhoan = (TaiKhoan)Session["TaiKhoan"];
                var binhluan = (from c in db.ChildComments where c.MaBL == mabl && c.BinhLuan.MaBL == mablparent select c).SingleOrDefault();

                binhluan.NoiDung = noidung;

                db.SaveChanges();
                return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Không thành công" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult XoaChildBinhLuan(int? mabl)
        {
            try
            {
                ChildComment bl = db.ChildComments.Where(c => c.MaBLChild == mabl).FirstOrDefault();
                db.ChildComments.Remove(bl);
                db.SaveChanges();

                return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Không thành công" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult getLoaiSP(int? MaDanhMuc)
        {
            try
            {
                var lstLoaiSP = from l in db.LoaiSanPhams where l.MaDanhMuc == MaDanhMuc select new { MaLoaiSP = l.MaLoaiSP, TenLoaiSP = l.TenLoaiSP };
                return Json(new { code = 200, lstLoaiSP = lstLoaiSP }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult AddBaiDang(int MaLoaiSP, string TieuDe, long Gia, bool TinhTrang, string MoTa, string Hinh, string Hinh1, string Hinh2, string Hinh3, string Hinh4)
        {
            try
            {

                TaiKhoan taikhoan = (TaiKhoan)Session["TaiKhoan"];
                BaiDang baidang = new BaiDang();
                baidang.MaLoaiSP = MaLoaiSP;
                baidang.TieuDe = TieuDe;
                baidang.Gia = Gia;
                baidang.TinhTrang = TinhTrang;
                baidang.MoTa = MoTa;
                if (Hinh != "NULL")
                {
                    baidang.HinhAnh = Hinh;
                }
                if (Hinh1 != "NULL")
                {
                    baidang.HinhAnh1 = Hinh1;
                }
                if (Hinh2 != "NULL")
                {
                    baidang.HinhAnh2 = Hinh2;
                }
                if (Hinh3 != "NULL")
                {
                    baidang.HinhAnh3 = Hinh3;
                }
                if (Hinh4 != "NULL")
                {
                    baidang.HinhAnh4 = Hinh4;
                }
                baidang.Username = taikhoan.Username;
                baidang.TrangThai = 2;
                baidang.NgayDang = DateTime.Now;
                db.BaiDangs.Add(baidang);
                db.SaveChanges();
                return Json(new { code = 200, msg = "Đăng thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public string XuLyFile(HttpPostedFileBase file)
        {
            string path = Server.MapPath("~/Images/" + file.FileName);
            try
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                file.SaveAs(path);
            }
            catch { }

            return "/Images/" + file.FileName;

        }
        public string XuLyFileHinh(HttpPostedFileBase file)
        {
            TaiKhoan taikhoan = (TaiKhoan)Session["TaiKhoan"];

            string path = Server.MapPath("~/Images/" + file.FileName);
            try
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                file.SaveAs(path);
            }
            catch { }
            var tk = (from t in db.TaiKhoans where t.Username == taikhoan.Username select t).FirstOrDefault();
            tk.Hinh = "/Images/" + file.FileName;
            db.SaveChanges();
            return "/Images/" + file.FileName;

        }
        public ActionResult Menu()
        {
            TaiKhoan taikhoan = (TaiKhoan)Session["TaiKhoan"];
            if (taikhoan != null)
            {
                ViewBag.Username = taikhoan.Username;
            }
            return PartialView();
        }
        public ActionResult QuanLyTin()
        {
            TaiKhoan taikhoan = (TaiKhoan)Session["TaiKhoan"];
            ViewBag.TenNguoiDung = taikhoan.TenNguoiDung;
            if (taikhoan.Hinh == null)
            {
                ViewBag.Hinh = "/Images/img_avatar.png";
            }
            else
            {
                ViewBag.Hinh = taikhoan.Hinh;
            }
            return View();
        }
        public JsonResult getBaiDang(int TrangThai)
        {
            try
            {
                TaiKhoan taikhoan = (TaiKhoan)Session["TaiKhoan"];
                //Lấy bài đăng theo trạng thái
                var lstbaidang = (from b in db.BaiDangs where b.TrangThai == TrangThai && b.Username == taikhoan.Username select new { MaBaiDang = b.MaBaiDang, TieuDe = b.TieuDe, Gia = b.Gia, HinhAnh = b.HinhAnh, TrangThai = b.TrangThai, NgayDang = b.NgayDang }).ToList();
                //Lấy bài đăng còn hạn quảng cáo
                var listqc = (from tin in db.QuangCaos
                              where DateTime.Compare(DateTime.Now, (DateTime)tin.NgayHetHan) == -1
                              select tin).ToList();
                //var query = from person in people
                //            join pet in pets on person equals pet.Owner into gj
                //            from subpet in gj.DefaultIfEmpty()
                //            select new { person.FirstName, PetName = subpet?.Name ?? String.Empty };
                var lstGop = (from baidang in lstbaidang
                              join quangcao in listqc on baidang.MaBaiDang equals quangcao.MaBaiDang into gr
                              from gop in gr.DefaultIfEmpty()
                              select new
                              {
                                  MaBaiDang = baidang.MaBaiDang,
                                  TieuDe = baidang.TieuDe,
                                  Gia = baidang.Gia,
                                  HinhAnh = baidang.HinhAnh,
                                  TrangThai = baidang.TrangThai,
                                  NgayHetHan = gop?.NgayHetHan ?? null
                              }).ToList();
                var lst1 = (from baidang in lstGop
                            select new
                            {
                                MaBaiDang = baidang.MaBaiDang,
                                TieuDe = baidang.TieuDe,
                                Gia = baidang.Gia.GetValueOrDefault(0).ToString("N0"),
                                HinhAnh = baidang.HinhAnh,
                                TrangThai = baidang.TrangThai,
                                NgayHetHan = baidang.NgayHetHan.ToString()
                            }).ToList();
                //Console.WriteLine(lstGop);
                return Json(new { code = 200, lstBaiDang = lst1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
         public JsonResult gettinnoibat(int trang)
        {
            try
            {
                var listtinnoibat = (from b in db.QuangCaos.Where(b => b.NgayHetHan.Value.Year == DateTime.Now.Year && b.NgayHetHan.Value.Month == DateTime.Now.Month && b.NgayHetHan.Value.Day >= DateTime.Now.Day).OrderByDescending(b => b.NgayHetHan).ToList()
                                     select new {
                                         MaBaiDang = b.MaBaiDang,
                                         TieuDe = b.BaiDang.TieuDe,
                                         Gia = b.BaiDang.Gia.GetValueOrDefault(0).ToString("N0"),
                                         HinhAnh = b.BaiDang.HinhAnh,
                                     }).ToList();
                var trangSP = listtinnoibat.Count() % 30 == 0 ? listtinnoibat.Count() / 30 : listtinnoibat.Count() / 30 + 1;
                var kqpt = listtinnoibat.Skip((trang - 1) * 30).Take(30).ToList();
                return Json(new { code = 200, trangSP = trangSP, lstTinnoibat= kqpt }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    
        public JsonResult getDanhSachSP(int trang)
        {
            try
            {
                var lstbaidang = (from b in db.BaiDangs.Where(x => x.TrangThai == 0).OrderByDescending(x => x.NgayDang).ToList()
                                  select new
                                  {
                                      MaBaiDang = b.MaBaiDang,
                                      TieuDe = b.TieuDe,
                                      Gia = b.Gia.GetValueOrDefault(0).ToString("N0"),
                                      HinhAnh = b.HinhAnh,
                                      NgayDang = b.NgayDang.ToString()
                                  }).ToList();
                //Lấy tin ưu tiên

                var trangSP = lstbaidang.Count() % 30 == 0 ? lstbaidang.Count() / 30 : lstbaidang.Count() / 30 + 1;
                var kqpt = lstbaidang.Skip((trang - 1) * 30).Take(30).ToList();
                return Json(new { code = 200, trangSP = trangSP, lstBaiDang = kqpt }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult UpdateTrangThai(int trangthai, int mabaidang)
        {
            try
            {
                var baidang = (from c in db.BaiDangs where c.MaBaiDang == mabaidang select c).FirstOrDefault();
                baidang.TrangThai = trangthai;
                db.SaveChanges();
                return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Không thành công" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult BaiDang(int id)
        {
            TaiKhoan tk = (TaiKhoan)Session["TaiKhoan"];
            BaiDang baidang = db.BaiDangs.Where(x => x.MaBaiDang == id).FirstOrDefault();
            // lấy all cmt
            List<BinhLuan> lstBL = (db.BinhLuans.Where(n => n.MaBaiDang == id && n.BaiDang.MaBaiDang == n.MaBaiDang)).ToList();
            //khởi tại child cmt
            List<ChildComment> lstChildCmt = new List<ChildComment>();

            //laaays cmt con
            foreach (var item in lstBL)
            {
                List<ChildComment> listcmt = (db.ChildComments.Where(c => c.MaBL == item.MaBL)).ToList();

                lstChildCmt.AddRange(listcmt);
            }
            ViewBag.listChildCmt = lstChildCmt;
            if (tk.Username != null)
            {
                ViewBag.Session = tk.Username;
                ViewBag.img = tk.Hinh;
            }
            else
            {

            ViewBag.listBL = lstBL;

            }
            ViewBag.listBL = lstBL;
            return View(baidang);
        }
        public JsonResult getchildcmt(int mabl, int mabaidang, int machild)
        {
            try
            {
                TaiKhoan tk = (TaiKhoan)Session["TaiKhoan"];
                List<ChildComment> lstbl = (from c in db.ChildComments where c.MaBL == mabl && c.BinhLuan.MaBaiDang == mabaidang && c.Username == tk.Username && c.MaBLChild==machild select c).ToList();
                List<BinhLuan> listbl = new List<BinhLuan>();
                var bl = (from b in lstbl
                          select new
                          {
                              us = b.Username,
                              mabl=b.MaBL,
                              nd = b.NoiDung,

                          }).ToList();
                return Json(new { code = 200, lstbl = bl }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Không thành công" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult getcmt(int mabl, int mabaidang)
        {
            try
            {
                TaiKhoan tk = (TaiKhoan)Session["TaiKhoan"];
                List<BinhLuan> lstbl = (from c in db.BinhLuans where c.MaBL == mabl && c.MaBaiDang == mabaidang && c.Username == tk.Username select c).ToList();
                List<BinhLuan> listbl = new List<BinhLuan>();
                var bl = (from b in lstbl
                          select new
                          {
                              us = b.Username,
                              nd = b.NoiDung,

                          }).ToList();
                return Json(new { code = 200, lstbl = bl }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Không thành công" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult getTinLienQuan(int madanhmuc, int mabaidang)
        {
            try
            {
                List<BaiDang> lst = (from b in db.BaiDangs
                                     where b.LoaiSanPham.MaDanhMuc == madanhmuc && b.TrangThai == 0
                                     select b).ToList();
                List<BaiDang> baidang = (from b in db.BaiDangs
                                         where b.MaBaiDang == mabaidang
                                         select b).ToList();
                List<BaiDang> lsttin = (lst.Except(baidang)).ToList();
                List<BaiDang> listBaiBang = new List<BaiDang>();
                if (lsttin.Count > 6)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        listBaiBang.Add(lsttin[i]);
                    }
                }
                else
                {
                    listBaiBang = lsttin;
                }
                var tin = (from b in listBaiBang
                           select new
                           {
                               MaBaiDang = b.MaBaiDang,
                               TieuDe = b.TieuDe,
                               Gia = b.Gia.GetValueOrDefault(0).ToString("N0"),
                               HinhAnh = b.HinhAnh,
                               NgayDang = b.NgayDang.ToString()
                           }).ToList();
                return Json(new { code = 200, listTin = tin }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Không thành công" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string serectkey;
        public static string amount;
        //public void ThanhToanMomo(int tongtien, int mabaidang)
        //{

        //    //string serectkey = "MOMOIQFF20220524";
        //    //string partnerCode = "MOMOOJOI20210710";
        //    //string accessKey = "8onTTFnTUuE4YSL7";
        //    //request params need to request to MoMo system
        //    string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
        //    string partnerCode = "MOMOIQFF20220524";
        //    string accessKey = "8onTTFnTUuE4YSL7";
        //     string serectkey = "sxYpKQjIzvxSY1hbgokJdIsQeT3iFulI";
        //    string orderInfo = "Mua quảng cáo";
        //    string returnUrl = "https://localhost:44349/Home/returnUrl/" + mabaidang;
        //    string notifyurl = "https://localhost:44349/Home/notifyurl";

        //    amount = "" + tongtien;
        //    string orderid = Guid.NewGuid().ToString();
        //    string requestId = Guid.NewGuid().ToString();
        //    string extraData = "";

        //    //Before sign HMAC SHA256 signature
        //    string rawHash = "partnerCode=" +
        //        partnerCode + "&accessKey=" +
        //        accessKey + "&requestId=" +
        //        requestId + "&amount=" +
        //        amount + "&orderId=" +
        //        orderid + "&orderInfo=" +
        //        orderInfo + "&returnUrl=" +
        //        returnUrl + "&notifyUrl=" +
        //        notifyurl + "&extraData=" +
        //        extraData;

        //    log.Debug("rawHash = " + rawHash);

        //    MoMoSecurity crypto = new MoMoSecurity();
        //    //sign signature SHA256
        //    string signature = crypto.signSHA256(rawHash, serectkey);
        //    log.Debug("Signature = " + signature);

        //    //build body json request
        //    JObject message = new JObject
        //    {
        //        { "partnerCode", partnerCode },
        //        { "accessKey", accessKey },
        //        { "requestId", requestId },
        //        { "amount", amount },
        //        { "orderId", orderid },
        //        { "orderInfo", orderInfo },
        //        { "returnUrl", returnUrl },
        //        { "notifyUrl", notifyurl },
        //        { "extraData", extraData },
        //        { "requestType", "captureMoMoWallet" },
        //        { "signature", signature }

        //    };
        //    //log.Debug("Json request to MoMo: " + message.ToString());
        //    string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());

        //    JObject jmessage = JObject.Parse(responseFromMomo);
        //    //log.Debug("Return from MoMo: " + jmessage.ToString());

        //    //yes...
        //    System.Diagnostics.Process.Start(jmessage.GetValue("payUrl").ToString());

        //}


        public async Task<JsonResult> ThanhToanMomo(int tongtien, int mabaidang)
        {


            //string serectkey = "sFcbSGRSJjwGxwhhcEktCHWYUuTuPNDB";
            //string partnerCode = "MOMOOJOI20210710";
            //string accessKey = "iPXneGmrJH0G8FOP";
            string partnerCode = "MOMOIQFF20220524";
            string accessKey = "8onTTFnTUuE4YSL7";
            string serectkey = "sxYpKQjIzvxSY1hbgokJdIsQeT3iFulI";
            string orderId = Guid.NewGuid().ToString();
            string requestId = Guid.NewGuid().ToString();
            string extraData = "";
            string orderInfo = "Mua quảng cáo Fashi";
            string amount = tongtien + "";
            Session["tongtien"] = tongtien;
            string redirectUrl = "https://localhost:44349/Home/returnUrl/" + mabaidang;
            string ipnUrl = "https://localhost:44349/Home/notifyurl";
            string requestType = "captureWallet";
            //Before sign HMAC SHA256 signature
            string rawHash = "accessKey=" + accessKey +
                "&amount=" + amount +
                "&extraData=" + extraData +
                "&ipnUrl=" + ipnUrl +
                "&orderId=" + orderId +
                "&orderInfo=" + orderInfo +
                "&partnerCode=" + partnerCode +
                "&redirectUrl=" + redirectUrl +
                "&requestId=" + requestId +
                "&requestType=" + requestType
                ;


            MoMoSecurity crypto = new MoMoSecurity();
            //sign signature SHA256
            string signature = crypto.signSHA256(rawHash, serectkey);


            //build body json request
            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "partnerName", "Test" },
                { "storeId", "MomoTestStore" },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderId },
                { "orderInfo", orderInfo },
                { "redirectUrl", redirectUrl },
                { "ipnUrl", ipnUrl },
                { "lang", "en" },
                { "extraData", extraData },
                { "requestType", requestType },
                { "signature", signature }

            };
            var httpClient = new HttpClient();

            var httpContent = new StringContent(message.ToString(), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync("https://test-payment.momo.vn/v2/gateway/api/create", httpContent);
            string a = await response.Content.ReadAsStringAsync();
            JObject jmessage = JObject.Parse(a);
            string paymentUrl = jmessage.GetValue("payUrl").ToString();
            return Json(new { code = 200, redirectUrl = paymentUrl, isRedirect = true }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult returnUrl(int id)
        {
            int tongtien = Convert.ToInt32(Session["tongtien"].ToString());
            string param = Request.QueryString.ToString().Substring(0, Request.QueryString.ToString().IndexOf("signature") - 1);
            var tk = Session["TaiKhoan"];
            TaiKhoan taikhoan = (TaiKhoan)Session["TaiKhoan"];
            var list = (from c in db.BaiDangs where (c.Username == taikhoan.Username) select c).FirstOrDefault();

            //Thành công

            if (Request.QueryString["message"].Equals("Successful."))
            {

                //Thay đổi trạng thái vé
                DateTime now = DateTime.Now;
                DateTime ngayhethan = new DateTime();
                if (tongtien == 10000)
                {
                    ngayhethan = now.AddDays(1);
                }
                else if (tongtien == 30000)
                {
                    ngayhethan = now.AddDays(3);
                }
                else if (tongtien == 50000)
                {
                    ngayhethan = now.AddDays(5);
                }
                QuangCao qc = new QuangCao();
                qc.NgayMua = now;
                qc.SoTien = tongtien;
                qc.MaBaiDang = id;
                qc.NgayHetHan = ngayhethan;
                db.QuangCaos.Add(qc);
                db.SaveChanges();

                ViewBag.Message = "Thanh toán thành công!";

                SendEmail(list.TaiKhoan.Email, "Xác nhận thanh toán của Fashi", "Bạn đã thanh toán quảng cáo tin thành công cho Fashi số tiền là :" + tongtien);

            }
            else
            {
                ViewBag.Message = "Thanh toán thất bại!";
                //Thay đổi trạng thái vé

                return View();

            }
            return View();
        }

        public JsonResult notifyurl(int id)
        {
            //string param = "";
            //param = "partnerCode=" + Request["partnerCcode"] +"&accessKey=" + Request["accessKey"]+               
            //  "&amount=" + Request["amount"] +
            //  "&orderId=" + Request["orderId"] +
            //  "&orderInfo=" + Request["orderInfo"]+
            //  "&orderType=" + Request["orderType"] +
            //  "&transId=" + Request["transId"] +
            //  "&message="+ Request["message"]+
            //  "&responseTime=" + Request["responseTime"]
            //  + "&errorCode" + Request["errorCode"];
            //param = Server.UrlDecode(param);
            //MoMoSecurity crypto = new MoMoSecurity();
            //string serectKey = serectkey.ToString();
            //string signature = crypto.signSHA256(param, serectKey);
            //if (signature != Request["signature"].ToString())
            //{

            //}
            //string errorCode = Request["errorCode"].ToString();
            //if(errorCode != "0")
            //{
            //    //That bai
            //}
            //else
            //{
            //    //Thanh cong
            //    DateTime now = DateTime.Now;
            //    DateTime ngayhethan = new DateTime();
            //    if(amount == "1000")
            //    {
            //        ngayhethan = now.AddDays(1);
            //    }else if(amount == "3000")
            //    {
            //        ngayhethan = now.AddDays(3);
            //    }
            //    else if (amount == "5000")
            //    {
            //        ngayhethan = now.AddDays(5);
            //    }
            //    QuangCao qc = new QuangCao();
            //    qc.MaBaiDang = id;
            //    qc.NgayHetHan = ngayhethan;
            //    db.QuangCaos.Add(qc);
            //    db.SaveChanges();
            //}
            //Thanh cong
            DateTime now = DateTime.Now;
            DateTime ngayhethan = new DateTime();
            if (amount == "10000")
            {
                ngayhethan = now.AddDays(1);
            }
            else if (amount == "30000")
            {
                ngayhethan = now.AddDays(3);
            }
            else if (amount == "50000")
            {
                ngayhethan = now.AddDays(5);
            }
            QuangCao qc = new QuangCao();
            qc.MaBaiDang = id;
            qc.NgayHetHan = ngayhethan;
            db.QuangCaos.Add(qc);
            db.SaveChanges();
            return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NotAuthorize()
        {
            return View();
        }

        //Thanh toán ngân hàng
        [HttpPost]
        public JsonResult Payment(int tongtien, int mabaidang)
        {
            string url = "http://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
            string returnUrl = "https://localhost:44349/Home/PaymentConfirm" + "/" + mabaidang;
            string tmnCode = "GHHNT2HB";
            string hashSecret = "BAGAOHAPRHKQZASKQZASVPRSAKPXNYXS";
            Session["ma"] = mabaidang;
            PayLib pay = new PayLib();
            amount = "" + tongtien * 100;
            pay.AddRequestData("vnp_Version", "2.0.0"); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.0.0
            pay.AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
            pay.AddRequestData("vnp_TmnCode", tmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
            pay.AddRequestData("vnp_Amount", amount); //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
            pay.AddRequestData("vnp_BankCode", ""); //Mã Ngân hàng thanh toán (tham khảo: https://sandbox.vnpayment.vn/apis/danh-sach-ngan-hang/), có thể để trống, người dùng có thể chọn trên cổng thanh toán VNPAY
            pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss")); //ngày thanh toán theo định dạng yyyyMMddHHmmss
            pay.AddRequestData("vnp_CurrCode", "VND"); //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
            pay.AddRequestData("vnp_IpAddr", TTUtil.GetIpAddress()); //Địa chỉ IP của khách hàng thực hiện giao dịch
            pay.AddRequestData("vnp_Locale", "vn"); //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
            pay.AddRequestData("vnp_OrderInfo", "Mua quang cáo"); //Thông tin mô tả nội dung thanh toán
            pay.AddRequestData("vnp_OrderType", "other"); //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
            pay.AddRequestData("vnp_ReturnUrl", returnUrl); //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
            pay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString()); //mã hóa đơn

            string paymentUrl = pay.CreateRequestUrl(url, hashSecret);

            return Json(new { code = 200, redirectUrl = paymentUrl, isRedirect = true }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PaymentConfirm()
        {
            if (Request.QueryString.Count > 0)
            {
                string hashSecret = "BAGAOHAPRHKQZASKQZASVPRSAKPXNYXS"; //Chuỗi bí mật
                var vnpayData = Request.QueryString;
                PayLib pay = new PayLib();

                //lấy toàn bộ dữ liệu được trả về
                foreach (string s in vnpayData)
                {
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        pay.AddResponseData(s, vnpayData[s]);
                    }
                }

                long orderId = Convert.ToInt64(pay.GetResponseData("vnp_TxnRef")); //mã hóa đơn
                long vnpayTranId = Convert.ToInt64(pay.GetResponseData("vnp_TransactionNo")); //mã giao dịch tại hệ thống VNPAY
                string vnp_ResponseCode = pay.GetResponseData("vnp_ResponseCode"); //response code: 00 - thành công, khác 00 - xem thêm https://sandbox.vnpayment.vn/apis/docs/bang-ma-loi/
                string vnp_SecureHash = Request.QueryString["vnp_SecureHash"]; //hash của dữ liệu trả về

                bool checkSignature = pay.ValidateSignature(vnp_SecureHash, hashSecret); //check chữ ký đúng hay không?
                TaiKhoan taikhoan = (TaiKhoan)Session["TaiKhoan"];
                var list = (from c in db.BaiDangs where (c.Username == taikhoan.Username) select c).FirstOrDefault();
                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00")
                    {
                        DateTime now = DateTime.Now;
                        DateTime ngayhethan = DateTime.Now;
                         
                        if (amount == "1000000")
                        {
                            ngayhethan = now.AddDays(1);
                        }
                        else if (amount == "3000000")
                        {
                            ngayhethan = now.AddDays(3);
                        }
                        else if (amount == "5000000")
                        {
                            ngayhethan = now.AddDays(5);
                        }
                        QuangCao qc = new QuangCao();
                        int a = Convert.ToInt32(Session["ma"].ToString());
                        qc.NgayMua = now;
                        qc.SoTien = int.Parse(amount);
                        qc.MaBaiDang = a;
                        qc.NgayHetHan = ngayhethan;
                        db.QuangCaos.Add(qc);
                        db.SaveChanges();
                        //Thanh toán thành công
                        ViewBag.Message = "Thanh toán thành công hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId;
                        SendEmail(list.TaiKhoan.Email, "Xác nhận thanh toán của Fashi", "Bạn đã thanh toán quảng cáo tin thành công cho Fashi số tiền là :" + amount);
                    }
                    else
                    {
                        //Thanh toán không thành công. Mã lỗi: vnp_ResponseCode
                        ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId + " | Mã lỗi: " + vnp_ResponseCode;
                    }
                }
                else
                {

                    ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý";
                }
            }

            return View();
        }

        public void SendEmail(string address, string subject, string message)
        {
            string email = "";
            string password = "";

            var email1 = new NetworkCredential(email, password);
            var msg = new MailMessage();
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
    }

}