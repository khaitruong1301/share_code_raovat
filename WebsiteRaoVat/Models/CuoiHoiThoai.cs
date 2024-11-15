namespace WebsiteRaoVat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CuoiHoiThoai")]
    public partial class CuoiHoiThoai
    {
        [Key]
        public int MaHoiThoai { get; set; }

        [StringLength(200)]
        public string NguoiGui { get; set; }

        [StringLength(200)]
        public string NguoiNhan { get; set; }

        [Column(TypeName = "ntext")]
        public string NoiDung { get; set; }

        public DateTime? ThoiGianGui { get; set; }

        [Column(TypeName = "text")]
        public string Hinh { get; set; }
    }
}
