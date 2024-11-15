namespace WebsiteRaoVat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LoaiSanPham")]
    public partial class LoaiSanPham
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LoaiSanPham()
        {
            BaiDangs = new HashSet<BaiDang>();
        }

        [Key]
        public int MaLoaiSP { get; set; }

        [StringLength(50)]
        public string TenLoaiSP { get; set; }

        public int? MaDanhMuc { get; set; }

        [Column(TypeName = "text")]
        public string Hinh { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BaiDang> BaiDangs { get; set; }

        public virtual DanhMuc DanhMuc { get; set; }
    }
}
