namespace WebsiteRaoVat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("QuangCao")]
    public partial class QuangCao
    {
        [Key]
        public int MaQuangCao { get; set; }

        public int? MaBaiDang { get; set; }

        public DateTime? NgayHetHan { get; set; }

        public DateTime? NgayMua { get; set; }

        public int? SoTien { get; set; }

        public virtual BaiDang BaiDang { get; set; }
    }
}
