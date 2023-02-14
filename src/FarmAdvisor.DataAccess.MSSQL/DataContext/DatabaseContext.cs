using System;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

using System.Diagnostics;
using FarmAdvisor.Models.Models;
using static FarmAdvisor.Models.Models.NotificationModel;
using static FarmAdvisor.Models.Models.SensorModel;

namespace FarmAdvisor.DataAccess.MSSQL.DataContext
{
    public class DatabaseContext : DbContext
    {

        public static readonly OptionsBuild Options = new OptionsBuild();
        public class OptionsBuild
        {
            public OptionsBuild()
            {
                Settings = new AppConfiguration();

                OptionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();

                OptionsBuilder.UseSqlServer(Settings.SqlConnectionString);

                DatabaseOptions = OptionsBuilder.Options;

            }


            public DbContextOptionsBuilder<DatabaseContext> OptionsBuilder { get; set; }
            public AppConfiguration Settings { get; set; }
            public DbContextOptions<DatabaseContext> DatabaseOptions { get; set; }

        }


        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<UserModel> Users { get; set; }

        public DbSet<FarmModel> Farms { get; set; }

        public DbSet<SensorModel> Sensors { get; set; }

        public DbSet<SensorData> SensorDatas { get; set; }

        public DbSet<NotificationModel> Notifications { get; set; }

        public DbSet<FieldModel> Fields { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            #region User
            modelBuilder.Entity<UserModel>().ToTable("user");
            //Primary Key & Identity Column
            modelBuilder.Entity<UserModel>().HasKey(us => us.UserID);
            modelBuilder.Entity<UserModel>().Property(us => us.UserID).HasColumnName("user_id").HasDefaultValue(new Guid());
            //COLUMN SETTINGS 
            modelBuilder.Entity<UserModel>().Property(us => us.Name).HasMaxLength(100).HasColumnName("user_name").HasDefaultValue("user");
            modelBuilder.Entity<UserModel>().Property(us => us.Email).HasMaxLength(100).HasColumnName("email").HasDefaultValue("user@test.com");
            modelBuilder.Entity<UserModel>().Property(us => us.Phone).HasColumnName("phone_number");
            modelBuilder.Entity<UserModel>().Property(us => us.AuthId).HasMaxLength(250).HasColumnName("auth_id").HasDefaultValue("token");
            /*modelBuilder.Entity<UserModel>().HasMany<FarmModel>(us => us.Farms)
                .WithOne(us => us.User)
                .HasForeignKey(us => us.FarmId)
                .OnDelete(DeleteBehavior.Cascade);
*/


            #endregion


            #region Farm

            modelBuilder.Entity<FarmModel>().ToTable("farm");
            //Primary Key & Identity Column
            modelBuilder.Entity<FarmModel>().HasKey(us => us.FarmId);
            modelBuilder.Entity<FarmModel>().Property(us => us.FarmId).IsRequired().HasColumnName("farm_id").HasDefaultValue(new Guid());
            //COLUMN SETTINGS 
            modelBuilder.Entity<FarmModel>().Property(us => us.Name).HasMaxLength(100).HasColumnName("farm_name").HasDefaultValue("Farm mock");
            modelBuilder.Entity<FarmModel>().Property(us => us.Postcode).HasMaxLength(100).HasColumnName("postcode").HasDefaultValue("12,AA");
            modelBuilder.Entity<FarmModel>().Property(us => us.City).HasColumnName("city").HasDefaultValue("Addis Ababa");
            modelBuilder.Entity<FarmModel>().Property(us => us.Country).HasMaxLength(250).HasColumnName("country").HasDefaultValue("Ethio");
            
            /*modelBuilder.Entity<FarmModel>()
                .HasMany(us => us.Notifications)
                .WithOne(us => us.Farm)
                .HasForeignKey(us => us.NotificationId)
                .OnDelete(DeleteBehavior.Cascade);*/

            /*modelBuilder.Entity<FarmModel>().HasMany<FieldModel>(us => us.Fields)
                .WithOne(us => us.Farm)
                .HasForeignKey(us => us.FieldId)
                .OnDelete(DeleteBehavior.Cascade);*/

            #endregion


            #region Field
            modelBuilder.Entity<FieldModel>().ToTable("field");
            modelBuilder.Entity<FieldModel>().HasKey(us => us.FieldId);
            modelBuilder.Entity<FieldModel>().Property(us => us.FieldId).IsRequired().HasColumnName("Field_id").HasDefaultValue(new Guid());
            modelBuilder.Entity<FieldModel>().Property(us => us.Name).HasMaxLength(100).HasColumnName("Field_name");
            modelBuilder.Entity<FieldModel>().Property(us => us.Polygon).HasMaxLength(100).HasColumnName("polygon");
            modelBuilder.Entity<FieldModel>().Property(us => us.Alt).HasColumnName("altitude");
            modelBuilder.Entity<FieldModel>().Property(us => us.Name).IsRequired(true).HasMaxLength(100).HasColumnName("Field_name");
            modelBuilder.Entity<FieldModel>().Property(us => us.Polygon).IsRequired(true).HasMaxLength(100).HasColumnName("polygon");
            modelBuilder.Entity<FieldModel>().Property(us => us.Alt).IsRequired(true).HasColumnName("altitude");


            /*modelBuilder.Entity<FieldModel>().HasMany<SensorModel>(us => us.Sensors)
                .WithOne(us => us.Field)
                .HasForeignKey(us => us.SensorId)
                .OnDelete(DeleteBehavior.Restrict);*/
            modelBuilder.Entity<FieldModel>()
                .HasOne(us => us.Farm)
                .WithMany(b => b.Fields)
                .HasForeignKey(p => p.FarmId)
                .OnDelete(DeleteBehavior.Cascade);


            #endregion


            #region Sensor
            modelBuilder.Entity<SensorModel>().ToTable("sensor");
            modelBuilder.Entity<SensorModel>().HasKey(us => us.SensorId);
            modelBuilder.Entity<SensorModel>().Property(us => us.SensorId).IsRequired().HasColumnName("sensor_id");
            modelBuilder.Entity<SensorModel>().Property(us => us.SerialNumber).HasMaxLength(100).HasColumnName("serial_number");
            modelBuilder.Entity<SensorModel>().Property(us => us.Long).HasColumnName("longitude");
            modelBuilder.Entity<SensorModel>().Property(us => us.Lat).HasColumnName("latitude");
            modelBuilder.Entity<SensorModel>().Property(us => us.LastCommunication).HasColumnName("last_communication");
            modelBuilder.Entity<SensorModel>().Property(us => us.BatteryStatus).HasColumnName("battery_status");
            modelBuilder.Entity<SensorModel>().Property(us => us.OptimalGDD).HasColumnName("optimal_gdd");
            modelBuilder.Entity<SensorModel>().Property(us => us.CuttingDateTimeCalculated).HasColumnName("estimated_date");
            modelBuilder.Entity<SensorModel>().Property(us => us.LastForecastDate).HasColumnName("last_forecast_date");
            modelBuilder.Entity<SensorModel>().Property(us => us.State).HasConversion(
            v => v!.ToString(), v => (StateEnum)StateEnum.Parse(typeof(StateEnum), v)).HasMaxLength(250).HasColumnName("state");
            modelBuilder.Entity<SensorModel>()
                .HasOne(us => us.Field)
                .WithMany(b => b.Sensors)
                .HasForeignKey(p => p.FieldId)
                .OnDelete(DeleteBehavior.Cascade);



            #endregion

            #region Notification
            modelBuilder.Entity<NotificationModel>().ToTable("notification");
            modelBuilder.Entity<NotificationModel>().HasKey(us => us.NotificationId);
            modelBuilder.Entity<NotificationModel>().Property(us => us.NotificationId).IsRequired().HasColumnName("Notification_id");
            modelBuilder.Entity<NotificationModel>().Property(us => us.Title).HasMaxLength(100).HasColumnName("title");
            modelBuilder.Entity<NotificationModel>().Property(us => us.Message).HasMaxLength(200).HasColumnName("message");
            modelBuilder.Entity<NotificationModel>().Property(us => us.SentBy).HasConversion(
            v => v!.ToString(), v => (SenderEnum)SenderEnum.Parse(typeof(SenderEnum), v)).HasColumnName("sent_by");
            modelBuilder.Entity<NotificationModel>().Property(us => us.Status).HasConversion(
            v => v!.ToString(), v => (StatusEnum)Enum.Parse(typeof(StatusEnum), v)).HasColumnName("status");
            modelBuilder.Entity<NotificationModel>().Property(us => us.Title).IsRequired(true).HasMaxLength(100).HasColumnName("title");
            modelBuilder.Entity<NotificationModel>().Property(us => us.Message).IsRequired(true).HasMaxLength(200).HasColumnName("message");
            modelBuilder.Entity<NotificationModel>().Property(us => us.SentBy).IsRequired(true).HasConversion(
            v => v.ToString(), v => (SenderEnum)SenderEnum.Parse(typeof(SenderEnum), v)).HasColumnName("sent_by");
            modelBuilder.Entity<NotificationModel>().Property(us => us.Status).IsRequired(true).HasConversion(
            v => v.ToString(), v => (StatusEnum)StatusEnum.Parse(typeof(StatusEnum), v)).HasColumnName("status");
            modelBuilder.Entity<NotificationModel>()
               .HasOne(us => us.Farm)
               .WithMany(b => b.Notifications)
               .HasForeignKey(p => p.FarmId)
               .OnDelete(DeleteBehavior.Cascade);

            #endregion




        }


    }
}