﻿// <auto-generated />
using System;
using FarmAdvisor.DataAccess.MSSQL.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FarmAdvisor.DataAccess.MSSQL.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20230119183923_initializedatabase")]
    partial class initializedatabase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("FarmAdvisor.Models.Models.FarmModel", b =>
                {
                    b.Property<Guid>("FarmId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("farm_id");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("city");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)")
                        .HasColumnName("country");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("farm_name");

                    b.Property<string>("Postcode")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("postcode");

                    b.HasKey("FarmId");

                    b.ToTable("farm", (string)null);
                });

            modelBuilder.Entity("FarmAdvisor.Models.Models.FieldModel", b =>
                {
                    b.Property<Guid>("FieldId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Field_id");

                    b.Property<int?>("Alt")
                        .IsRequired()
                        .HasColumnType("int")
                        .HasColumnName("altitude");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("Field_name");

                    b.Property<string>("Polygon")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("polygon");

                    b.HasKey("FieldId");

                    b.ToTable("field", (string)null);
                });

            modelBuilder.Entity("FarmAdvisor.Models.Models.NotificationModel", b =>
                {
                    b.Property<Guid>("NotificationId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Notification_id");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("message");

                    b.Property<string>("SentBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("sent_by");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("status");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("title");

                    b.HasKey("NotificationId");

                    b.ToTable("notification", (string)null);
                });

            modelBuilder.Entity("FarmAdvisor.Models.Models.SensorModel", b =>
                {
                    b.Property<Guid>("SensorId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("sensor_id");

                    b.Property<int>("BatteryStatus")
                        .HasColumnType("int")
                        .HasColumnName("battery_status");

                    b.Property<DateTime>("CuttingDateTimeCalculated")
                        .HasColumnType("datetime2")
                        .HasColumnName("estimated_date");

                    b.Property<DateTime>("LastCommunication")
                        .HasColumnType("datetime2")
                        .HasColumnName("last_communication");

                    b.Property<DateTime>("LastForecastDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("last_forecast_date");

                    b.Property<double>("Lat")
                        .HasColumnType("float")
                        .HasColumnName("latitude");

                    b.Property<double>("Long")
                        .HasColumnType("float")
                        .HasColumnName("longitude");

                    b.Property<int>("OptimalGDD")
                        .HasColumnType("int")
                        .HasColumnName("optimal_gdd");

                    b.Property<string>("SerialNumber")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("serial_number");

                    b.Property<string>("State")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)")
                        .HasColumnName("state");

                    b.HasKey("SensorId");

                    b.ToTable("sensor", (string)null);
                });

            modelBuilder.Entity("FarmAdvisor.Models.Models.UserModel", b =>
                {
                    b.Property<Guid>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("user_id");

                    b.Property<string>("AuthId")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)")
                        .HasColumnName("auth_id");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("email");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("user_name");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("phone_number");

                    b.HasKey("UserID");

                    b.ToTable("user", (string)null);
                });

            modelBuilder.Entity("FarmModelUserModel", b =>
                {
                    b.Property<Guid>("FarmsFarmId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UsersUserID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("FarmsFarmId", "UsersUserID");

                    b.HasIndex("UsersUserID");

                    b.ToTable("FarmModelUserModel");
                });

            modelBuilder.Entity("SensorModelUserModel", b =>
                {
                    b.Property<Guid>("SensorsSensorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UsersUserID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("SensorsSensorId", "UsersUserID");

                    b.HasIndex("UsersUserID");

                    b.ToTable("SensorModelUserModel");
                });

            modelBuilder.Entity("FarmAdvisor.Models.Models.FieldModel", b =>
                {
                    b.HasOne("FarmAdvisor.Models.Models.FarmModel", "Farm")
                        .WithMany("Fields")
                        .HasForeignKey("FieldId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Farm");
                });

            modelBuilder.Entity("FarmAdvisor.Models.Models.NotificationModel", b =>
                {
                    b.HasOne("FarmAdvisor.Models.Models.FarmModel", "Farm")
                        .WithMany("Notifications")
                        .HasForeignKey("NotificationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Farm");
                });

            modelBuilder.Entity("FarmAdvisor.Models.Models.SensorModel", b =>
                {
                    b.HasOne("FarmAdvisor.Models.Models.FieldModel", "Field")
                        .WithMany("Sensors")
                        .HasForeignKey("SensorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Field");
                });

            modelBuilder.Entity("FarmModelUserModel", b =>
                {
                    b.HasOne("FarmAdvisor.Models.Models.FarmModel", null)
                        .WithMany()
                        .HasForeignKey("FarmsFarmId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FarmAdvisor.Models.Models.UserModel", null)
                        .WithMany()
                        .HasForeignKey("UsersUserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SensorModelUserModel", b =>
                {
                    b.HasOne("FarmAdvisor.Models.Models.SensorModel", null)
                        .WithMany()
                        .HasForeignKey("SensorsSensorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FarmAdvisor.Models.Models.UserModel", null)
                        .WithMany()
                        .HasForeignKey("UsersUserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FarmAdvisor.Models.Models.FarmModel", b =>
                {
                    b.Navigation("Fields");

                    b.Navigation("Notifications");
                });

            modelBuilder.Entity("FarmAdvisor.Models.Models.FieldModel", b =>
                {
                    b.Navigation("Sensors");
                });
#pragma warning restore 612, 618
        }
    }
}
