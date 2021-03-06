// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Models;

namespace WebProgramiranjeProjekat.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20220118205235_Migracija1")]
    partial class Migracija1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("BolnicaLekar", b =>
                {
                    b.Property<int>("BolniceID")
                        .HasColumnType("int");

                    b.Property<int>("LekariID")
                        .HasColumnType("int");

                    b.HasKey("BolniceID", "LekariID");

                    b.HasIndex("LekariID");

                    b.ToTable("BolnicaLekar");
                });

            modelBuilder.Entity("Models.Bolnica", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("BrMesta")
                        .HasColumnType("int");

                    b.Property<string>("Ime")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("ID");

                    b.ToTable("Bolnica");
                });

            modelBuilder.Entity("Models.Lecenje", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int?>("BolnicaID")
                        .HasColumnType("int");

                    b.Property<DateTime>("Kraj")
                        .HasColumnType("datetime2");

                    b.Property<int?>("LekarID")
                        .HasColumnType("int");

                    b.Property<int?>("PacijentID")
                        .HasColumnType("int");

                    b.Property<DateTime>("Pocetak")
                        .HasColumnType("datetime2");

                    b.Property<int>("SobaID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("BolnicaID");

                    b.HasIndex("LekarID");

                    b.HasIndex("PacijentID");

                    b.ToTable("Lecenje");
                });

            modelBuilder.Entity("Models.Lekar", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Ime")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Prezime")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("ID");

                    b.ToTable("Lekar");
                });

            modelBuilder.Entity("Models.Pacijent", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Ime")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("JMBG")
                        .HasMaxLength(13)
                        .HasColumnType("nvarchar(13)");

                    b.Property<string>("Prezime")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("ID");

                    b.ToTable("Pacijent");
                });

            modelBuilder.Entity("BolnicaLekar", b =>
                {
                    b.HasOne("Models.Bolnica", null)
                        .WithMany()
                        .HasForeignKey("BolniceID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.Lekar", null)
                        .WithMany()
                        .HasForeignKey("LekariID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Models.Lecenje", b =>
                {
                    b.HasOne("Models.Bolnica", "Bolnica")
                        .WithMany()
                        .HasForeignKey("BolnicaID");

                    b.HasOne("Models.Lekar", "Lekar")
                        .WithMany()
                        .HasForeignKey("LekarID");

                    b.HasOne("Models.Pacijent", "Pacijent")
                        .WithMany()
                        .HasForeignKey("PacijentID");

                    b.Navigation("Bolnica");

                    b.Navigation("Lekar");

                    b.Navigation("Pacijent");
                });
#pragma warning restore 612, 618
        }
    }
}
