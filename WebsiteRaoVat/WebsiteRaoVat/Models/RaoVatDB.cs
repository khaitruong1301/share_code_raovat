using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace WebsiteRaoVat.Models
{
    public partial class RaoVatDB : DbContext
    {
        public RaoVatDB()
            : base("name=RaoVatDB")
        {
        }

        public virtual DbSet<BaiDang> BaiDangs { get; set; }
        public virtual DbSet<BinhLuan> BinhLuans { get; set; }
        public virtual DbSet<CuoiHoiThoai> CuoiHoiThoais { get; set; }
        public virtual DbSet<DanhMuc> DanhMucs { get; set; }
        public virtual DbSet<DSYeuThich> DSYeuThiches { get; set; }
        public virtual DbSet<LoaiSanPham> LoaiSanPhams { get; set; }
        public virtual DbSet<QuangCao> QuangCaos { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }
        public virtual DbSet<ChildComment> ChildComments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BaiDang>()
                .Property(e => e.Username)
                .IsUnicode(false);

            modelBuilder.Entity<BaiDang>()
                .Property(e => e.HinhAnh)
                .IsUnicode(false);

            modelBuilder.Entity<BaiDang>()
                .Property(e => e.HinhAnh1)
                .IsUnicode(false);

            modelBuilder.Entity<BaiDang>()
                .Property(e => e.HinhAnh2)
                .IsUnicode(false);

            modelBuilder.Entity<BaiDang>()
                .Property(e => e.HinhAnh3)
                .IsUnicode(false);

            modelBuilder.Entity<BaiDang>()
                .Property(e => e.HinhAnh4)
                .IsUnicode(false);

            modelBuilder.Entity<BinhLuan>()
                .Property(e => e.Username)
                .IsUnicode(false);

            modelBuilder.Entity<CuoiHoiThoai>()
                .Property(e => e.NguoiGui)
                .IsUnicode(false);

            modelBuilder.Entity<CuoiHoiThoai>()
                .Property(e => e.NguoiNhan)
                .IsUnicode(false);

            modelBuilder.Entity<CuoiHoiThoai>()
                .Property(e => e.Hinh)
                .IsUnicode(false);

            modelBuilder.Entity<DanhMuc>()
                .Property(e => e.Hinh)
                .IsUnicode(false);

            modelBuilder.Entity<DanhMuc>()
                .HasMany(e => e.LoaiSanPhams)
                .WithOptional(e => e.DanhMuc)
                .WillCascadeOnDelete();

            modelBuilder.Entity<DSYeuThich>()
                .Property(e => e.Username)
                .IsUnicode(false);

            modelBuilder.Entity<LoaiSanPham>()
                .Property(e => e.Hinh)
                .IsUnicode(false);

            modelBuilder.Entity<LoaiSanPham>()
                .HasMany(e => e.BaiDangs)
                .WithOptional(e => e.LoaiSanPham)
                .WillCascadeOnDelete();

            modelBuilder.Entity<TaiKhoan>()
                .Property(e => e.Username)
                .IsUnicode(false);

            modelBuilder.Entity<TaiKhoan>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<TaiKhoan>()
                .Property(e => e.SDT)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TaiKhoan>()
                .Property(e => e.Hinh)
                .IsUnicode(false);

            modelBuilder.Entity<TaiKhoan>()
                .HasMany(e => e.BaiDangs)
                .WithOptional(e => e.TaiKhoan)
                .WillCascadeOnDelete();

            modelBuilder.Entity<ChildComment>()
                .Property(e => e.Username)
                .IsUnicode(false);
        }
    }
}
