using Tuan4_NgoTriDuc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tuan4_NgoTriDuc
{
    public class NguoidungController : Controller
    {
        // GET: Nguoidung
        MyDataDataContext data = new MyDataDataContext();
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        public ActionResult DangKy(FormCollection collection, KhachHang kh)
        {
            var hoten = collection["hoten"];
            var tendangnhap = collection["tendangnhap"];
            var matkhau = collection["matkhau"];
            var MatKhauXacNhan = collection["MatKhauXacNhan"];
            var email = collection["email"];
            var diachi = collection["diachi"];
            var dienthoai = collection["dienthoai"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", collection["ngaysinh"]);

            if (String.IsNullOrEmpty(MatKhauXacNhan))
            {
                ViewData["NhapMKXN"] = "Phải nhập mật khẩu xác nhận!";
            }
            else
            {
                if (!matkhau.Equals(MatKhauXacNhan))
                {
                    ViewData["MatKhauGiongNhau"] = "Mật khẩu và mật khẩu xác nhận phải giống nhau";
                }
                else
                {
                    kh.hoten = hoten;
                    kh.tendangnhap = tendangnhap;
                    kh.matkhau = matkhau;
                    kh.email = email;
                    kh.diachi = diachi;
                    kh.dienthoai = dienthoai;
                    kh.ngaysinh = DateTime.Parse(ngaysinh);

                    data.KhachHangs.InsertOnSubmit(kh);
                    data.SubmitChanges();

                    return RedirectToAction("DangNhap");
                }
            }

            return this.DangKy();

        }

        [HttpGet]

        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(FormCollection collection, KhachHang kh)
        {
            var tendn = collection["TenDN"];
            var matkhau = collection["MatKhau"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                kh = data.KhachHangs.SingleOrDefault(n => n.tendangnhap == tendn && n.matkhau == matkhau);
                if (kh != null)
                {
                    ViewBag.ThongBao = "Đăng nhập thành công!!!";
                    Session["TaiKhoan"] = kh;
                }
                else
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng!!!";
                }
                return View();
            }
            return RedirectToAction("Index", "Home");

        }
    }
}