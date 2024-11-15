namespace WebsiteRaoVat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DSYeuThich")]
    public partial class DSYeuThich
    {
        [Key]
        public int MaTinYT { get; set; }

        [StringLength(200)]
        public string Username { get; set; }

        public int? MaBaiDang { get; set; }
    }
}
