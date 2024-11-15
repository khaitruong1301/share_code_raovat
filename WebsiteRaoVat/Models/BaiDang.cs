namespace WebsiteRaoVat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BaiDang")]
    public partial class BaiDang
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BaiDang()
        {
            BinhLuans = new HashSet<BinhLuan>();
            QuangCaos = new HashSet<QuangCao>();
        }

        [Key]
        public int MaBaiDang { get; set; }

        [StringLength(200)]
        public string Username { get; set; }

        public int? MaLoaiSP { get; set; }

        [Column(TypeName = "ntext")]
        public string TieuDe { get; set; }

        public long? Gia { get; set; }

        public bool? TinhTrang { get; set; }

        [Column(TypeName = "ntext")]
        public string MoTa { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string HinhAnh { get; set; }

        public int? TrangThai { get; set; }

        public DateTime? NgayDang { get; set; }

        [Column(TypeName = "text")]
        public string HinhAnh1 { get; set; }

        [Column(TypeName = "text")]
        public string HinhAnh2 { get; set; }

        [Column(TypeName = "text")]
        public string HinhAnh3 { get; set; }

        [Column(TypeName = "text")]
        public string HinhAnh4 { get; set; }

        public int? Cout { get; set; }

        public virtual LoaiSanPham LoaiSanPham { get; set; }

        public virtual TaiKhoan TaiKhoan { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BinhLuan> BinhLuans { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuangCao> QuangCaos { get; set; }
    }
}
