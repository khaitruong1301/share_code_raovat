using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebsiteRaoVat.Models;

namespace WebsiteRaoVat.Controllers
{
    public class TinDangController : Controller
    {
        // GET: TinDang
        private RaoVatDB db = new RaoVatDB();
        public ActionResult DSTinDang(int id)
        {
            DateTime now = DateTime.Now;
            var dsTin = (from tin in db.QuangCaos where tin.BaiDang.LoaiSanPham.MaDanhMuc == id && DateTime.Compare(now, (DateTime)tin.NgayHetHan) == -1 select tin).ToList();
            ViewBag.MaDanhMuc = id;
            ViewBag.TenDanhMuc = db.DanhMucs.Where(x=>x.MaDanhMuc == id).Select(x=>x.TenDanhMuc).FirstOrDefault();
            return View(dsTin);
        }

        public ActionResult CreateYeuThich()
        {

            return View();
        }

        public JsonResult getTinYT()
        {
            try
            {
                TaiKhoan taikhoan = (TaiKhoan)Session["TaiKhoan"];
                var yt = (from c in db.DSYeuThiches
                          where c.Username == taikhoan.Username
                          select new
                          {
                              MaTinYT = c.MaTinYT,
                              Username = c.Username,
                              MaBaiDang = c.MaBaiDang
                          }).ToList();
                var lstbaidang = (from a in yt
                                  join b in db.BaiDangs on a.MaBaiDang equals b.MaBaiDang
                                  where b.TrangThai == 0
                                  select new
                                  {
                                      MaBaiDang = a.MaBaiDang,
                                      Username = a.Username,
                                      TieuDe = b.TieuDe,
                                      Gia = b.Gia.GetValueOrDefault(0).ToString("N0"),
                                      HinhAnh = b.HinhAnh,
                                      NgayDang = b.NgayDang.ToString()
                                  }).ToList();
                return Json(new { code = 200, listBaiDang = lstbaidang }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult getBaiDang(int id)
        {
            try
            {
                var listBaiDang = (from l in db.BaiDangs
                                   where l.LoaiSanPham.MaDanhMuc == id && l.TrangThai == 0
                                   orderby l.NgayDang descending
                                   select new
                                   {
                                       MaBaiDang = l.MaBaiDang,
                                       Username = l.Username,
                                       TieuDe = l.TieuDe,
                                       Gia = l.Gia,
                                       HinhAnh = l.HinhAnh,
                                       NgayDang = l.NgayDang
                                   }).ToList();
                var lstGiaGiamDan = (from l in listBaiDang orderby l.Gia ascending
                                    select new
                                    {
                                        MaBaiDang = l.MaBaiDang,
                                        Username = l.Username,
                                        TieuDe = l.TieuDe,
                                        Gia = l.Gia,
                                        HinhAnh = l.HinhAnh,
                                        NgayDang = l.NgayDang
                                    }).ToList();
                return Json(new { code = 200, lstBaiDang = lstGiaGiamDan }, JsonRequestBehavior.AllowGet);
            }catch(Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult getLoaiSP(int id)
        {
            try
            {
                var listLoaiSP = (from c in db.LoaiSanPhams where c.MaDanhMuc == id select new { 
                    MaLoaiSP = c.MaLoaiSP,
                    TenLoaiSP = c.TenLoaiSP
                }).ToList();
                return Json(new { code = 200, listLoaiSP = listLoaiSP }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult TrangCaNhan(string id)
        {
            var tk = db.TaiKhoans.Where(x => x.Username == id).FirstOrDefault();
            return View(tk);
        }
        public ActionResult SuaTrangCaNhan(string id)
        {
            var tk = db.TaiKhoans.Where(x => x.Username == id).FirstOrDefault();
            return View(tk);
        }
        public JsonResult SuaThongTin(string username, string tennguoidung, string sdt, string diachi, string email)
        {
            try
            {
                var taikhoan = db.TaiKhoans.Where(x => x.Username == username).FirstOrDefault();
                taikhoan.TenNguoiDung = tennguoidung;
                taikhoan.SDT = sdt;
                taikhoan.DiaChi = diachi;
                taikhoan.Email = email;
                db.SaveChanges();
                return Json(new { code = 200}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult getTongSP(string username)
        {
            try
            {
                var spdangban = (from c in db.BaiDangs where c.TaiKhoan.Username == username && c.TrangThai == 0 select c).Count();
                var spdaban = (from c in db.BaiDangs where c.TaiKhoan.Username == username && c.TrangThai == 3 select c).Count();
                return Json(new { code = 200, spdangban = spdangban, spdaban = spdaban }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult getBaiDangUser(string username)
        {
            try
            {

                var listBaiDang = (from c in db.BaiDangs where c.TaiKhoan.Username == username && c.TrangThai == 0 select new { MaBaiDang = c.MaBaiDang, HinhAnh = c.HinhAnh, TieuDe = c.TieuDe, Gia = c.Gia }).ToList();
                return Json(new { code = 200, lstBaiDang = listBaiDang }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        
        public JsonResult LocTheoLoai(string arr, int? maloai)
        {
            //try
            //{

            WebsiteRaoVat.Models.RaoVatDB DB = new WebsiteRaoVat.Models.RaoVatDB();
            List<BaiDang> baiDangs = new List<BaiDang>();
            var tindau1 = db.QuangCaos.Where(x=>x.NgayHetHan>=DateTime.Now && x.BaiDang.MaLoaiSP == maloai).ToList();
            List<BaiDang> dangbai1 = new List<BaiDang>();
            var tinuutien = new List<BaiDang>();
            try
            {

                foreach (var item in tindau1)
                {
                    dangbai1.Add(item.BaiDang);       
                    
                }

                var tindau = dangbai1.Where(y => y.MaLoaiSP == maloai && y.TrangThai==0).OrderBy(y => y.Cout).ToList().First().Cout;
                 tinuutien = dangbai1.Where(x => x.MaLoaiSP == maloai && x.Cout == tindau && x.TrangThai==0).ToList();
            }
            catch { }
            int dem = 0;
            for (int i = 0; i < tinuutien.ToList().Count; i++)
            {
                while (true)
                {
                    int n = new Random().Next(0, tinuutien.ToList().Count);
                    if(!baiDangs.Exists(x=>x.MaBaiDang== tinuutien.ToList()[n].MaBaiDang))
                    {
                       


                        baiDangs.Add(tinuutien.ToList()[n]);
                        if (dem < 3)
                        {
                            var a = DB.BaiDangs.Find(tinuutien.ToList()[n].MaBaiDang);
                            a.Cout++;
                            DB.SaveChanges();
                            dem++;

                        }

                        break;

                    }
                }

            }

            tinuutien = dangbai1.Where(x => x.MaLoaiSP == maloai ).ToList();
            int leng = tinuutien.ToList().Count - baiDangs.Count;
            for (int i = 0; i < leng; i++)
            {
                while (true)
                {
                    int n = new Random().Next(0, tinuutien.ToList().Count);
                    if (!baiDangs.Exists(x => x.MaBaiDang == tinuutien.ToList()[n].MaBaiDang))
                    {

                        baiDangs.Add(tinuutien.ToList()[n]);
                      
                        break;

                    }
                }

            }

            foreach (var item in DB.BaiDangs.Where(x => x.MaLoaiSP == maloai && x.TrangThai==0))
            {

                if (!baiDangs.Exists(x => x.MaBaiDang == item.MaBaiDang))
                {
                    item.TrangThai = 10;
                    baiDangs.Add(item);                    

                }

               
            }


var listBaiDang = (from l in baiDangs.Where(x =>x.TrangThai==0 || x.TrangThai==10)

                   select new
                     {
                         MaBaiDang = l.MaBaiDang,
                         Username = l.Username,
                         TieuDe = l.TieuDe,
                       Gia = l.Gia.GetValueOrDefault(0).ToString("N0"),
                       HinhAnh = l.HinhAnh,
                         check = l.TrangThai,
                         //NgayDang = l.NgayDang
                                                            NgayDang = l.NgayDang.ToString()
                   }).ToList();
            return Json(new { code = 200, lstBaiDang = listBaiDang }, JsonRequestBehavior.AllowGet);
            //}
            //catch (Exception ex)
            //{
            //    return Json(new { code = 500, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            //}
        }

        public JsonResult LocTheotinhtrang( bool? tinhtrang ,int id )
        {
            //try
            //{

            WebsiteRaoVat.Models.RaoVatDB DB = new WebsiteRaoVat.Models.RaoVatDB();
            List<BaiDang> baiDangs = new List<BaiDang>();
            var LoaiSP = DB.LoaiSanPhams.Where(X => X.MaDanhMuc == id).ToList();
            var tindau1 = db.QuangCaos.Where(x => x.NgayHetHan >= DateTime.Now && x.BaiDang.LoaiSanPham.MaDanhMuc == id).ToList();
            List<BaiDang> dangbai1 = new List<BaiDang>();
            var tinuutien = new List<BaiDang>();
            
            

                foreach (var item in tindau1)
                {
                    dangbai1.Add(item.BaiDang);

                }

            foreach (var item in LoaiSP)
            {
                try
                {
                    var tindau = dangbai1.Where(y => y.TinhTrang == tinhtrang && y.MaLoaiSP == item.MaLoaiSP && y.TrangThai == 0).OrderBy(y => y.Cout).ToList().First().Cout;
                    tinuutien = dangbai1.Where(x => x.TinhTrang == tinhtrang && x.Cout == tindau && x.MaLoaiSP == item.MaLoaiSP && x.TrangThai == 0).ToList();
                }

                catch
                {

                }
            }

            
            
            int dem = 0;
            for (int i = 0; i < tinuutien.ToList().Count; i++)
            {
                while (true)
                {
                    int n = new Random().Next(0, tinuutien.ToList().Count);
                    if (!baiDangs.Exists(x => x.MaBaiDang == tinuutien.ToList()[n].MaBaiDang))
                    {

                        baiDangs.Add(tinuutien.ToList()[n]);
                        if (dem < 3)
                        {
                            var a = DB.BaiDangs.Find(tinuutien.ToList()[n].MaBaiDang);
                            a.Cout++;
                            DB.SaveChanges();
                            dem++;

                        }

                        break;

                    }
                }

            }

            tinuutien = dangbai1.Where(x =>  x.TinhTrang == tinhtrang && x.TrangThai == 0).ToList();
            int leng = tinuutien.ToList().Count - baiDangs.Count;
            for (int i = 0; i < leng; i++)
            {
                while (true)
                {
                    int n = new Random().Next(0, tinuutien.ToList().Count);
                    if (!baiDangs.Exists(x => x.MaBaiDang == tinuutien.ToList()[n].MaBaiDang))
                    {

                        baiDangs.Add(tinuutien.ToList()[n]);

                        break;

                    }
                }

            }

            var tinthuong = new List<BaiDang>();
            foreach (var item1 in LoaiSP)
            {

                foreach (var item in DB.BaiDangs.Where(x => x.MaLoaiSP == item1.MaLoaiSP && x.TrangThai == 0  && x.TinhTrang == tinhtrang))
                {

                    if (!baiDangs.Exists(x => x.MaBaiDang == item.MaBaiDang))
                    {
                        item.TrangThai = 10;
                        baiDangs.Add(item);

                    }


                }
                //tinthuong.AddRange(DB.BaiDangs.Where(x => x.TinhTrang == tinhtrang && x.MaLoaiSP == item.MaLoaiSP).ToList());
            }

            // baiDangs.AddRange(tinthuong);
            //  var list = new JavaScriptSerializer().Deserialize<List<BaiDang>>(arr);
            var listBaiDang = (from l in baiDangs.Where(x => x.TrangThai ==0 ||x.TrangThai==10 )

                               select new
                               {
                                   MaBaiDang = l.MaBaiDang,
                                   Username = l.Username,
                                   TieuDe = l.TieuDe,
                                   Gia = l.Gia.GetValueOrDefault(0).ToString("N0"),
                                   HinhAnh = l.HinhAnh,
                                   check = l.TrangThai
                                   //NgayDang = l.NgayDang
                                   ,
                                   NgayDang = l.NgayDang.ToString()
                               }).ToList();
            //    Console.WriteLine(list);
            return Json(new { code = 200, lstBaiDang = listBaiDang }, JsonRequestBehavior.AllowGet);
            //}
            //catch (Exception ex)
            //{
            //    return Json(new { code = 500, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            //}
        }

        public JsonResult LocTheothoigian(int? tinhtrang ,int id)
        {
            //try
            //{
            
            WebsiteRaoVat.Models.RaoVatDB DB = new WebsiteRaoVat.Models.RaoVatDB();
            var LoaiSP = DB.LoaiSanPhams.Where(X => X.MaDanhMuc == id).ToList();
            List<BaiDang> baiDangs = new List<BaiDang>();
            var tindau1 = db.QuangCaos.Where(x => x.NgayHetHan >= DateTime.Now && x.BaiDang.LoaiSanPham.MaDanhMuc == id).ToList();
            List<BaiDang> dangbai1 = new List<BaiDang>();
            var tinuutien = new List<BaiDang>();
            try
            {

                foreach (var item in tindau1)
                {
                    dangbai1.Add(item.BaiDang);

                }
                foreach (var item in LoaiSP)
                {
                    var tindau = dangbai1.Where(  y=>y.MaLoaiSP==item.MaLoaiSP).OrderBy(y => y.Cout).ToList().First().Cout;
                    tinuutien = dangbai1.Where(x => x.Cout == tindau && x.MaLoaiSP == item.MaLoaiSP).ToList();
                }


            }
            catch { }
            int dem = 0;
            for (int i = 0; i < tinuutien.ToList().Count; i++)
            {
                while (true)
                {
                    int n = new Random().Next(0, tinuutien.ToList().Count);
                    if (!baiDangs.Exists(x => x.MaBaiDang == tinuutien.ToList()[n].MaBaiDang))
                    {

                        baiDangs.Add(tinuutien.ToList()[n]);
                        if (dem < 3)
                        {
                            var a = DB.BaiDangs.Find(tinuutien.ToList()[n].MaBaiDang);
                            a.Cout++;
                            DB.SaveChanges();
                            dem++;

                        }

                        break;

                    }
                }

            }

            tinuutien = dangbai1.ToList();
            int leng = tinuutien.ToList().Count - baiDangs.Count;
            for (int i = 0; i < leng; i++)
            {
                while (true)
                {
                    int n = new Random().Next(0, tinuutien.ToList().Count);
                    if (!baiDangs.Exists(x => x.MaBaiDang == tinuutien.ToList()[n].MaBaiDang))
                    {

                        baiDangs.Add(tinuutien.ToList()[n]);

                        break;

                    }
                }

            }

            var tinthuong = new List<BaiDang>();
            if (tinhtrang == 2)
            {
                foreach (var item1 in LoaiSP)
                {

                    foreach (var item in DB.BaiDangs.Where(x => x.MaLoaiSP == item1.MaLoaiSP && x.TrangThai == 0).OrderBy(x=>x.NgayDang))
                    {

                        if (!baiDangs.Exists(x => x.MaBaiDang == item.MaBaiDang))
                        {
                            item.TrangThai = 10;
                            baiDangs.Add(item);

                        }


                    }
                    //tinthuong.AddRange(DB.BaiDangs.Where(x => x.TinhTrang == tinhtrang && x.MaLoaiSP == item.MaLoaiSP).ToList());
                }

            }
            else
            {
                foreach (var item1 in LoaiSP)
                {

                    foreach (var item in DB.BaiDangs.Where(x => x.MaLoaiSP == item1.MaLoaiSP && x.TrangThai == 0).OrderByDescending(x => x.NgayDang))
                    {

                        if (!baiDangs.Exists(x => x.MaBaiDang == item.MaBaiDang))
                        {
                            item.TrangThai = 10;
                            baiDangs.Add(item);

                        }


                    }
                    //tinthuong.AddRange(DB.BaiDangs.Where(x => x.TinhTrang == tinhtrang && x.MaLoaiSP == item.MaLoaiSP).ToList());
                }
            }


            // var list = new JavaScriptSerializer().Deserialize<List<BaiDang>>(arr);
            var listBaiDang = (from l in baiDangs.Where(x=>x.TrangThai==0 || x.TrangThai==10 )
                               
                               select new
                               {
                                   MaBaiDang = l.MaBaiDang,
                                   Username = l.Username,
                                   TieuDe = l.TieuDe,
                                   Gia = l.Gia.GetValueOrDefault(0).ToString("N0"),
                                   HinhAnh = l.HinhAnh,
                                   check=l.TrangThai,
                                   NgayDang = l.NgayDang.ToString()
                               }).ToList();
        //    Console.WriteLine(list);
            return Json(new { code = 200, lstBaiDang = listBaiDang }, JsonRequestBehavior.AllowGet);
            //}
            //catch (Exception ex)
            //{
            //    return Json(new { code = 500, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            //}
        }


        public JsonResult LocTheogia(int? tinhtrang, int id)
        {
            //try
            //{

            WebsiteRaoVat.Models.RaoVatDB DB = new WebsiteRaoVat.Models.RaoVatDB();
            var LoaiSP = DB.LoaiSanPhams.Where(X => X.MaDanhMuc == id).ToList();
            List<BaiDang> baiDangs = new List<BaiDang>();
            var tindau1 = db.QuangCaos.Where(x => x.NgayHetHan >= DateTime.Now && x.BaiDang.LoaiSanPham.MaDanhMuc == id).ToList();
            List<BaiDang> dangbai1 = new List<BaiDang>();
            var tinuutien = new List<BaiDang>();
            try
            {

                foreach (var item in tindau1)
                {
                    dangbai1.Add(item.BaiDang);

                }
                foreach (var item in LoaiSP)
                {
                    var tindau = dangbai1.Where(y => y.MaLoaiSP == item.MaLoaiSP).OrderBy(y => y.Cout).ToList().First().Cout;
                    tinuutien = dangbai1.Where(x => x.Cout == tindau && x.MaLoaiSP == item.MaLoaiSP).ToList();
                }


            }
            catch { }
            int dem = 0;
            for (int i = 0; i < tinuutien.ToList().Count; i++)
            {
                while (true)
                {
                    int n = new Random().Next(0, tinuutien.ToList().Count);
                    if (!baiDangs.Exists(x => x.MaBaiDang == tinuutien.ToList()[n].MaBaiDang))
                    {

                        baiDangs.Add(tinuutien.ToList()[n]);
                        if (dem < 3)
                        {
                            var a = DB.BaiDangs.Find(tinuutien.ToList()[n].MaBaiDang);
                            a.Cout++;
                            DB.SaveChanges();
                            dem++;

                        }

                        break;

                    }
                }

            }

            tinuutien = dangbai1.ToList();
            int leng = tinuutien.ToList().Count - baiDangs.Count;
            for (int i = 0; i < leng; i++)
            {
                while (true)
                {
                    int n = new Random().Next(0, tinuutien.ToList().Count);
                    if (!baiDangs.Exists(x => x.MaBaiDang == tinuutien.ToList()[n].MaBaiDang))
                    {

                        baiDangs.Add(tinuutien.ToList()[n]);

                        break;

                    }
                }

            }

            var tinthuong = new List<BaiDang>();
            if (tinhtrang == 1)
            {
                foreach (var item1 in LoaiSP)
                {

                    foreach (var item in DB.BaiDangs.Where(x => x.MaLoaiSP == item1.MaLoaiSP && x.TrangThai == 0).OrderBy(x => x.Gia))
                    {

                        if (!baiDangs.Exists(x => x.MaBaiDang == item.MaBaiDang))
                        {
                            item.TrangThai = 10;
                            baiDangs.Add(item);

                        }


                    }
                    //tinthuong.AddRange(DB.BaiDangs.Where(x => x.TinhTrang == tinhtrang && x.MaLoaiSP == item.MaLoaiSP).ToList());
                }

            }
            else
            {
                foreach (var item1 in LoaiSP)
                {

                    foreach (var item in DB.BaiDangs.Where(x => x.MaLoaiSP == item1.MaLoaiSP && x.TrangThai == 0).OrderByDescending(x => x.Gia))
                    {

                        if (!baiDangs.Exists(x => x.MaBaiDang == item.MaBaiDang))
                        {
                            item.TrangThai = 10;
                            baiDangs.Add(item);

                        }


                    }
                    //tinthuong.AddRange(DB.BaiDangs.Where(x => x.TinhTrang == tinhtrang && x.MaLoaiSP == item.MaLoaiSP).ToList());
                }
            }


            // var list = new JavaScriptSerializer().Deserialize<List<BaiDang>>(arr);
            var listBaiDang = (from l in baiDangs.Where(x => x.TrangThai == 0 || x.TrangThai == 10)

                               select new
                               {
                                   MaBaiDang = l.MaBaiDang,
                                   Username = l.Username,
                                   TieuDe = l.TieuDe,
                                   Gia = l.Gia.GetValueOrDefault(0).ToString("N0"),
                                   HinhAnh = l.HinhAnh,
                                   check = l.TrangThai,
                                   NgayDang = l.NgayDang.ToString()
                               }).ToList();
            //    Console.WriteLine(list);
            return Json(new { code = 200, lstBaiDang = listBaiDang }, JsonRequestBehavior.AllowGet);
            //}
            //catch (Exception ex)
            //{
            //    return Json(new { code = 500, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            //}
        }

        public JsonResult ThemTinYeuThich(int mabaidang)
        {
            try
            {
                TaiKhoan taikhoan = (TaiKhoan)Session["TaiKhoan"];
                var tin = db.DSYeuThiches.Where(x => x.MaBaiDang == mabaidang && x.Username == taikhoan.Username).Count();
                if (tin == 0)
                {
                    DSYeuThich yt = new DSYeuThich();
                    yt.Username = taikhoan.Username;
                    yt.MaBaiDang = mabaidang;
                    db.DSYeuThiches.Add(yt);
                    db.SaveChanges();
                }
                
                return Json(new { code = 200}, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
       
        public JsonResult XoaTinYT(int ma)
        {
            try
            {
                TaiKhoan taikhoan = (TaiKhoan)Session["TaiKhoan"];
                DSYeuThich yt = db.DSYeuThiches.Where(x => x.MaBaiDang == ma && x.Username == taikhoan.Username).FirstOrDefault();
                db.DSYeuThiches.Remove(yt);
                db.SaveChanges();
                return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult TimKiemTinDang(string id)
        {
            ViewBag.TuKhoa = id;
            return View();
        }
        public JsonResult TimKiem(string tukhoa)
        {
            //try
            //{

                RaoVatDB DB = new RaoVatDB();
                List<BaiDang> baiDangs = new List<BaiDang>();
                var tindau = db.QuangCaos.Where(x => x.NgayHetHan >= DateTime.Now && x.BaiDang.TieuDe.Contains(tukhoa) && x.BaiDang.TrangThai == 0).ToList();
                
                foreach(var item in tindau)
                {
                    baiDangs.Add(item.BaiDang);
                }

                var lstbai = (from l in tindau
                              select new
                              {
                                  MaBaiDang = l.MaBaiDang,
                                  Username = l.BaiDang.Username,
                                  TieuDe = l.BaiDang.TieuDe,
                                  Gia = l.BaiDang.Gia.GetValueOrDefault(0).ToString("N0"),
                                  HinhAnh = l.BaiDang.HinhAnh,                                 
                                  NgayDang = l.BaiDang.NgayDang.ToString()
                              }).ToList();
                List<BaiDang> list = (db.BaiDangs.Where(x => x.TieuDe.Contains(tukhoa) && x.TrangThai == 0)).ToList();
                var list1 = list.Except(baiDangs);
                var lstbai2 = (from l in list1
                               select new
                              {
                                  MaBaiDang = l.MaBaiDang,
                                  Username = l.Username,
                                  TieuDe = l.TieuDe,
                                  Gia = l.Gia.GetValueOrDefault(0).ToString("N0"),
                                  HinhAnh = l.HinhAnh,
                                  NgayDang = l.NgayDang.ToString()
                              }).ToList();

                return Json(new { code = 200, listbaidang = lstbai, listbaithuong = lstbai2 }, JsonRequestBehavior.AllowGet);
            //}
            //catch (Exception ex)
            //{
            //    return Json(new { code = 500, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            //}
        }
    }
}