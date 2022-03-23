using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tuan4_NgoTriDuc.Models;

namespace Tuan4_NgoTriDuc.Controllers
{
    public class GioHangController : Controller
    {
        // GET: GioHang
        MyDataDataContext data = new MyDataDataContext();
        public List<GioHang> Laygiohang()
        {
            List<GioHang> listGioHang = Session["GioHang"] as List<GioHang>;
            if (listGioHang == null)
            {
                listGioHang = new List<GioHang>();
                Session["GioHang"] = listGioHang;
            }
            return listGioHang;
        }
        public ActionResult ThemGioHang(int id, string strURl)
        {
            List<GioHang> listGioHang = Laygiohang();

            GioHang sanpham = listGioHang.Find(n => n.masach == id);

            if (sanpham == null)
            {
                sanpham = new GioHang(id);
                listGioHang.Add(sanpham);
                return Redirect(strURl);
            }
            else
            {
                sanpham.iSoluong++;
                return Redirect(strURl);
            }

        }

        private int TongSoLuong()
        {
            int tsl = 0;
            List<GioHang> listGioHang = Session["GioHang"] as List<GioHang>;

            if (listGioHang != null)
            {
                tsl = listGioHang.Sum(n => n.iSoluong);
            }
            return tsl;
        }
        private int TongSoLuongSanPham()
        {
            int tsl = 0;
            List<GioHang> listGioHang = Session["GioHang"] as List<GioHang>;
            if (listGioHang != null)
            {
                tsl = listGioHang.Count;
            }
            return tsl;
        }

        private double TongTien()
        {
            double tt = 0;
            List<GioHang> listGioHang = Session["GioHang"] as List<GioHang>;
            if (listGioHang != null)
            {
                tt = listGioHang.Sum(n => n.dThanhtien);
            }
            return tt;
        }

        public ActionResult GioHang()
        {
            List<GioHang> listGioHang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.TongSoLuongSanPham = TongSoLuongSanPham();
            return View(listGioHang);

        }

        public ActionResult GioHangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.TongSoLuongSanPham = TongSoLuongSanPham();
            return PartialView();
        }

        public ActionResult XoaGiohang(int id)
        {
            List<GioHang> listGioHang = Laygiohang();

            GioHang sanpham = listGioHang.SingleOrDefault(n => n.masach == id);
            if (sanpham != null)
            {
                listGioHang.RemoveAll(n => n.masach == id);
                return RedirectToAction("GioHang");
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult CapnhatGiohang(int id, FormCollection collection)
        {
            List<GioHang> listGioHang = Laygiohang();

            GioHang sanpham = listGioHang.SingleOrDefault(n => n.masach == id);
            if (sanpham != null)
            {
                sanpham.iSoluong = int.Parse(collection["txtSolg"].ToString());
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult XoatatcaGiohang()
        {
            List<GioHang> listGioHang = Laygiohang();
            listGioHang.Clear();
            return RedirectToAction("GioHang");
        }
        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Sach");
            }

            List<GioHang> lstGioHang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return View(lstGioHang);
        }

        public ActionResult DatHang(FormCollection collection)
        {
            DonHang dh = new DonHang();
            KhachHang kh = (KhachHang)Session["TaiKhoan"];
            Sach s = new Sach();

            List<GioHang> gh = Laygiohang();
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["NgayGiao"]);

            dh.makh = kh.makh;
            dh.ngaydat = DateTime.Now;
            dh.ngaygiao = DateTime.Parse(ngaygiao);
            dh.giaohang = false;
            dh.thanhtoan = false;

            data.DonHangs.InsertOnSubmit(dh);
            data.SubmitChanges();
            foreach (var item in gh)
            {
                ChiTietDonHang ctdh = new ChiTietDonHang();
                ctdh.madon = dh.madon;
                ctdh.masach = item.masach;
                ctdh.soluong = item.iSoluong;
                ctdh.gia = (decimal)item.giaban;
                s = data.Saches.Single(n => n.masach == item.masach);
                s.soluongton -= ctdh.soluong;
                data.SubmitChanges();

                data.ChiTietDonHangs.InsertOnSubmit(ctdh);
            }
            data.SubmitChanges();
            Session["Giohang"] = null;
            return RedirectToAction("XacnhanDonhang", "GioHang");
        }
        public ActionResult Index()
        {
            return View();
        }        
        public ActionResult XacNhanDonHang()
        {
            return View();
        }
    }
    }
  
