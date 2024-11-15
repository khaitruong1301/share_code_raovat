namespace WebsiteRaoVat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BinhLuan")]
    public partial class BinhLuan
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BinhLuan()
        {
            ChildComments = new HashSet<ChildComment>();
        }

        [Key]
        public int MaBL { get; set; }

        public string NoiDung { get; set; }

        public int? MaBaiDang { get; set; }

        [StringLength(200)]
        public string Username { get; set; }

        public int? ParentId { get; set; }

        public virtual BaiDang BaiDang { get; set; }

        public virtual TaiKhoan TaiKhoan { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChildComment> ChildComments { get; set; }
    }
}
