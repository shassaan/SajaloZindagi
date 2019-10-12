using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SajaloZindagi.Models
{
    public partial class sajaloZindagiContext : DbContext
    {
        public virtual DbSet<Admin> Admin { get; set; }
        public virtual DbSet<Events> Events { get; set; }
        public virtual DbSet<EventServices> EventServices { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Services> Services { get; set; }
        public virtual DbSet<Simages> Simages { get; set; }
        public virtual DbSet<SubEvents> SubEvents { get; set; }
        public virtual DbSet<Volunteer> Volunteer { get; set; }
        public sajaloZindagiContext(DbContextOptions<sajaloZindagiContext> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(@"Server=DESKTOP-3TI0U5J\SQLEXPRESS;Database=sajaloZindagi;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasKey(e => e.AUname);

                entity.ToTable("admin");

                entity.Property(e => e.AUname)
                    .HasColumnName("a_uname")
                    .HasColumnType("char(120)")
                    .ValueGeneratedNever();

                entity.Property(e => e.AAccessType).HasColumnName("a_AccessType");

                entity.Property(e => e.AContact)
                    .HasColumnName("a_contact")
                    .HasColumnType("char(20)");

                entity.Property(e => e.ADp)
                    .HasColumnName("a_dp")
                    .HasColumnType("char(350)");

                entity.Property(e => e.AEmail)
                    .HasColumnName("a_email")
                    .HasColumnType("char(200)");

                entity.Property(e => e.AFullName)
                    .HasColumnName("a_fullName")
                    .HasColumnType("char(200)");

                entity.Property(e => e.APassword)
                    .HasColumnName("a_password")
                    .HasColumnType("char(200)");
            });

            modelBuilder.Entity<Events>(entity =>
            {
                entity.HasKey(e => e.EId);

                entity.ToTable("events");

                entity.Property(e => e.EId).HasColumnName("e_id");

                entity.Property(e => e.EDescriiption)
                    .HasColumnName("e_descriiption")
                    .HasColumnType("char(3000)");

                entity.Property(e => e.EDp)
                    .HasColumnName("e_dp")
                    .HasColumnType("char(350)");

                entity.Property(e => e.EName)
                    .HasColumnName("e_name")
                    .HasColumnType("char(64)");
            });

            modelBuilder.Entity<EventServices>(entity =>
            {
                entity.HasKey(e => e.EsId);

                entity.ToTable("eventServices");

                entity.Property(e => e.EsId).HasColumnName("es_id");

                entity.Property(e => e.SId).HasColumnName("s_id");

                entity.Property(e => e.SeId).HasColumnName("se_id");

                entity.HasOne(d => d.S)
                    .WithMany(p => p.EventServices)
                    .HasForeignKey(d => d.SId)
                    .HasConstraintName("FK__eventServi__s_id__6E01572D");

                entity.HasOne(d => d.Se)
                    .WithMany(p => p.EventServices)
                    .HasForeignKey(d => d.SeId)
                    .HasConstraintName("FK__eventServ__se_id__6D0D32F4");
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.HasKey(e => e.OId);

                entity.ToTable("orders");

                entity.Property(e => e.OId).HasColumnName("o_id");

                entity.Property(e => e.ImgId).HasColumnName("img_id");

                entity.Property(e => e.ODate)
                    .HasColumnName("o_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.ODescr)
                    .HasColumnName("o_descr")
                    .HasColumnType("char(3000)");

                entity.Property(e => e.OEmail)
                    .HasColumnName("o_email")
                    .HasColumnType("char(30)");

                entity.Property(e => e.OFuncTime)
                    .HasColumnName("o_func_time")
                    .HasColumnType("char(200)");

                entity.Property(e => e.OFuncType)
                    .HasColumnName("o_func_type")
                    .HasColumnType("char(200)");

                entity.Property(e => e.OName)
                    .HasColumnName("o_name")
                    .HasColumnType("char(30)");

                entity.Property(e => e.ONoOfGuests)
                    .HasColumnName("o_noOfGuests")
                    .HasColumnType("char(200)");

                entity.Property(e => e.OPh)
                    .HasColumnName("o_ph")
                    .HasColumnType("char(20)");

                entity.Property(e => e.OReadStatus).HasColumnName("o_readStatus");

                entity.Property(e => e.VId).HasColumnName("v_id");

                entity.HasOne(d => d.Img)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ImgId)
                    .HasConstraintName("FK__orders__img_id__73BA3083");

                entity.HasOne(d => d.V)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.VId)
                    .HasConstraintName("FK__orders__v_id__74AE54BC");
            });

            modelBuilder.Entity<Services>(entity =>
            {
                entity.HasKey(e => e.SId);

                entity.ToTable("services");

                entity.Property(e => e.SId).HasColumnName("s_id");

                entity.Property(e => e.SDetail)
                    .HasColumnName("s_detail")
                    .HasColumnType("char(3000)");

                entity.Property(e => e.SDp)
                    .HasColumnName("s_dp")
                    .HasColumnType("char(350)");

                entity.Property(e => e.SIsAvailable).HasColumnName("s_isAvailable");

                entity.Property(e => e.SName)
                    .HasColumnName("s_name")
                    .HasColumnType("char(64)");

                entity.Property(e => e.SPrice).HasColumnName("s_price");
            });

            modelBuilder.Entity<Simages>(entity =>
            {
                entity.HasKey(e => e.ImgId);

                entity.ToTable("SImages");

                entity.Property(e => e.ImgId).HasColumnName("img_id");

                entity.Property(e => e.ImgDescription)
                    .HasColumnName("img_description")
                    .HasColumnType("char(3000)");

                entity.Property(e => e.ImgName)
                    .HasColumnName("img_name")
                    .HasColumnType("char(200)");

                entity.Property(e => e.ImgPath)
                    .HasColumnName("img_path")
                    .HasColumnType("char(300)");

                entity.Property(e => e.ImgPrice).HasColumnName("img_price");

                entity.Property(e => e.IsDeal).HasColumnName("isDeal");

                entity.Property(e => e.SId).HasColumnName("s_id");

                entity.HasOne(d => d.S)
                    .WithMany(p => p.Simages)
                    .HasForeignKey(d => d.SId)
                    .HasConstraintName("FK__SImages__s_id__70DDC3D8");
            });

            modelBuilder.Entity<SubEvents>(entity =>
            {
                entity.HasKey(e => e.SeId);

                entity.ToTable("subEvents");

                entity.Property(e => e.SeId).HasColumnName("se_id");

                entity.Property(e => e.EId).HasColumnName("e_id");

                entity.Property(e => e.SeDp)
                    .HasColumnName("se_dp")
                    .HasColumnType("char(350)");

                entity.Property(e => e.SeName)
                    .HasColumnName("se_name")
                    .HasColumnType("char(64)");

                entity.HasOne(d => d.E)
                    .WithMany(p => p.SubEvents)
                    .HasForeignKey(d => d.EId)
                    .HasConstraintName("FK__subEvents__e_id__68487DD7");
            });

            modelBuilder.Entity<Volunteer>(entity =>
            {
                entity.HasKey(e => e.VId);

                entity.ToTable("volunteer");

                entity.Property(e => e.VId).HasColumnName("v_id");

                entity.Property(e => e.VAddress)
                    .HasColumnName("v_address")
                    .HasColumnType("char(350)");

                entity.Property(e => e.VContact)
                    .HasColumnName("v_contact")
                    .HasColumnType("char(20)");

                entity.Property(e => e.VDp)
                    .HasColumnName("v_dp")
                    .HasColumnType("char(350)");

                entity.Property(e => e.VName)
                    .HasColumnName("v_name")
                    .HasColumnType("char(64)");
            });
        }
    }
}
