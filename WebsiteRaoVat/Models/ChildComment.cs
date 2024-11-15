namespace WebsiteRaoVat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChildComment")]
    public partial class ChildComment
    {
        [Key]
        public int MaBLChild { get; set; }

        [StringLength(50)]
        public string NoiDung { get; set; }

        public int? MaBL { get; set; }

        [StringLength(200)]
        public string Username { get; set; }

        public virtual BinhLuan BinhLuan { get; set; }

        public virtual TaiKhoan TaiKhoan { get; set; }
    }
}
