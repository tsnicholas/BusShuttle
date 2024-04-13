﻿// <auto-generated />
using System;
using BusShuttleDatabase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BusShuttleDatabase.Migrations
{
    [DbContext(typeof(BusShuttleContext))]
    partial class BusShuttleContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.3");

            modelBuilder.Entity("BusShuttleModel.Bus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("BusNumber")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id")
                        .HasName("PrimaryKey_Id");

                    b.ToTable("Buses");
                });

            modelBuilder.Entity("BusShuttleModel.BusRoute", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("LoopId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Order")
                        .HasColumnType("INTEGER");

                    b.Property<int>("StopId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id")
                        .HasName("PrimaryKey_Id");

                    b.HasIndex("LoopId");

                    b.HasIndex("StopId")
                        .IsUnique();

                    b.ToTable("Routes");
                });

            modelBuilder.Entity("BusShuttleModel.Driver", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id")
                        .HasName("PrimaryKey_Id");

                    b.ToTable("Drivers");
                });

            modelBuilder.Entity("BusShuttleModel.Entry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Boarded")
                        .HasColumnType("INTEGER");

                    b.Property<int>("BusId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DriverId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("LeftBehind")
                        .HasColumnType("INTEGER");

                    b.Property<int>("LoopId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("StopId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("TEXT");

                    b.HasKey("Id")
                        .HasName("PrimaryKey_Id");

                    b.HasIndex("BusId");

                    b.HasIndex("DriverId");

                    b.HasIndex("LoopId");

                    b.HasIndex("StopId");

                    b.ToTable("Entries");
                });

            modelBuilder.Entity("BusShuttleModel.Loop", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id")
                        .HasName("PrimaryKey_Id");

                    b.ToTable("Loops");
                });

            modelBuilder.Entity("BusShuttleModel.Stop", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("Latitude")
                        .HasColumnType("REAL");

                    b.Property<double>("Longitude")
                        .HasColumnType("REAL");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id")
                        .HasName("PrimaryKey_Id");

                    b.ToTable("Stops");
                });

            modelBuilder.Entity("BusShuttleModel.BusRoute", b =>
                {
                    b.HasOne("BusShuttleModel.Loop", "Loop")
                        .WithMany("Routes")
                        .HasForeignKey("LoopId");

                    b.HasOne("BusShuttleModel.Stop", "Stop")
                        .WithOne("Route")
                        .HasForeignKey("BusShuttleModel.BusRoute", "StopId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Loop");

                    b.Navigation("Stop");
                });

            modelBuilder.Entity("BusShuttleModel.Entry", b =>
                {
                    b.HasOne("BusShuttleModel.Bus", "Bus")
                        .WithMany("Entries")
                        .HasForeignKey("BusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BusShuttleModel.Driver", "Driver")
                        .WithMany("Entries")
                        .HasForeignKey("DriverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BusShuttleModel.Loop", "Loop")
                        .WithMany("Entries")
                        .HasForeignKey("LoopId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BusShuttleModel.Stop", "Stop")
                        .WithMany("Entries")
                        .HasForeignKey("StopId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bus");

                    b.Navigation("Driver");

                    b.Navigation("Loop");

                    b.Navigation("Stop");
                });

            modelBuilder.Entity("BusShuttleModel.Bus", b =>
                {
                    b.Navigation("Entries");
                });

            modelBuilder.Entity("BusShuttleModel.Driver", b =>
                {
                    b.Navigation("Entries");
                });

            modelBuilder.Entity("BusShuttleModel.Loop", b =>
                {
                    b.Navigation("Entries");

                    b.Navigation("Routes");
                });

            modelBuilder.Entity("BusShuttleModel.Stop", b =>
                {
                    b.Navigation("Entries");

                    b.Navigation("Route")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
