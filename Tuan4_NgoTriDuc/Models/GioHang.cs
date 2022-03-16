using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tuan4_NgoTriDuc.Models
{
    public class GioHang
    {
        MyDataDataContext data = new MyDataDataContext();
        internal int dThanhtien;

        public int masach { get; set; }

        [Display(Name = "ten sach")]
        public string tensach { get; set; }
        [Display(Name = "anh bia")]
        public string hinh { get; set; }
        [Display(Name = "gia ban")]
        public Double giaban { get; set; }
        [Display(Name = "so luong")]
        public int iSoluong  { get; set; }  
        [Display(Name = "thanh tien")]
        public Double dthanhtien
        {
            get { return iSoluong * giaban; }
        }
        public GioHang(int id)
        {
            masach = id;
            Sach sach = data.Saches.Single(n => n.masach == masach);
            tensach = sach.tensach;
            hinh = sach.hinh;
            giaban = double.Parse(sach.giaban.ToString());
            iSoluong = 1;
        }



    }
}