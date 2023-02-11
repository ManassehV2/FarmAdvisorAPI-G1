using System;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using FarmAdvisor.DataAccess.MSSQL.Entities;
using System.Diagnostics;
using FarmAdvisor.Models.Models;

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

        public DbSet<User> Users { get; set; }

        public DbSet<Farm> Farms { get; set; }

        public DbSet<Sensor> Sensors { get; set; }

        public DbSet<SensorData> SensorData { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Field> Fields { get; set; }
    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
         
            DateTime modifiedDate = new DateTime(1900, 1, 1);

            #region User
            modelBuilder.Entity<User>().ToTable("user");
            //Primary Key & Identity Column
            modelBuilder.Entity<User>().HasKey(us => us.UserID);
            modelBuilder.Entity<User>().Property(us => us.UserID).HasColumnName("user_id");
            //COLUMN SETTINGS 
            modelBuilder.Entity<User>().Property(us => us.Name).HasMaxLength(100).HasColumnName("user_name");
            modelBuilder.Entity<User>().Property(us => us.Email).HasMaxLength(100).HasColumnName("email");
            modelBuilder.Entity<User>().Property(us => us.Phone).HasColumnName("phone_number");
            modelBuilder.Entity<User>().Property(us => us.AuthId).HasMaxLength(250).HasColumnName("auth_id");
            modelBuilder.Entity<User>().HasMany<Farm>(us => us.Farms)
                .WithMany(us => us.Users);
           
            #endregion


            #region Farm
            modelBuilder.Entity<Farm>().ToTable("farm");
            //Primary Key & Identity Column
            modelBuilder.Entity<Farm>().HasKey(us => us.FarmId);
            modelBuilder.Entity<Farm>().Property(us => us.FarmId).IsRequired().HasColumnName("farm_id");
            //COLUMN SETTINGS 
            modelBuilder.Entity<Farm>().Property(us => us.Name).HasMaxLength(100).HasColumnName("farm_name");
            modelBuilder.Entity<Farm>().Property(us => us.Postcode).HasMaxLength(100).HasColumnName("postcode");
            modelBuilder.Entity<Farm>().Property(us => us.City).HasColumnName("city");
            modelBuilder.Entity<Farm>().Property(us => us.Country).HasMaxLength(250).HasColumnName("country");
            modelBuilder.Entity<Farm>().Property(us => us.Name).IsRequired(true).HasMaxLength(100).HasColumnName("farm_name");
            modelBuilder.Entity<Farm>().Property(us => us.Postcode).IsRequired(true).HasMaxLength(100).HasColumnName("postcode");
            modelBuilder.Entity<Farm>().Property(us => us.City).IsRequired(true).HasColumnName("city");
            modelBuilder.Entity<Farm>().Property(us => us.Country).IsRequired(true).HasMaxLength(250).HasColumnName("country");

            modelBuilder.Entity<Farm>()
                .HasMany<Notification>(us=>us.Notifications)
                .WithOne(us=>us.Farm)
                .HasForeignKey(us=>us.NotificationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Farm>().HasMany<Field>(us => us.Fields)
                .WithOne(us => us.Farm)
                .HasForeignKey(us => us.FieldId)
                .OnDelete(DeleteBehavior.Restrict);

            #endregion


            #region Field
            modelBuilder.Entity<Field>().ToTable("field");
            //Primary Key & Identity Column
            modelBuilder.Entity<Field>().HasKey(us => us.FieldId);
            modelBuilder.Entity<Field>().Property(us => us.FieldId).IsRequired().HasColumnName("Field_id");
            //COLUMN SETTINGS 
            modelBuilder.Entity<Field>().Property(us => us.Name).HasMaxLength(100).HasColumnName("Field_name");
            modelBuilder.Entity<Field>().Property(us => us.Polygon).HasMaxLength(100).HasColumnName("polygon");
            modelBuilder.Entity<Field>().Property(us => us.Alt).HasColumnName("altitude");
            modelBuilder.Entity<Field>().Property(us => us.Name).IsRequired(true).HasMaxLength(100).HasColumnName("Field_name");
            modelBuilder.Entity<Field>().Property(us => us.Polygon).IsRequired(true).HasMaxLength(100).HasColumnName("polygon");
            modelBuilder.Entity<Field>().Property(us => us.Alt).IsRequired(true).HasColumnName("altitude");
            
            modelBuilder.Entity<Field>().HasMany<Sensor>(us => us.Sensors)
                .WithOne(us => us.Field)
                .HasForeignKey(us => us.SensorId)
                .OnDelete(DeleteBehavior.Restrict);


            #endregion


            #region Sensor
            modelBuilder.Entity<Sensor>().ToTable("sensor");
            //Primary Key & Identity Column
            modelBuilder.Entity<Sensor>().HasKey(us => us.SensorId);
            modelBuilder.Entity<Sensor>().Property(us => us.SensorId).IsRequired().HasColumnName("sensor_id");
            //COLUMN SETTINGS 
            modelBuilder.Entity<Sensor>().Property(us => us.SerialNumber).HasMaxLength(100).HasColumnName("serial_number");
            modelBuilder.Entity<Sensor>().Property(us => us.Long).HasColumnName("longitude");
            modelBuilder.Entity<Sensor>().Property(us => us.Lat).HasColumnName("latitude");
            modelBuilder.Entity<Sensor>().Property(us => us.LastCommunication).HasColumnName("last_communication");
            modelBuilder.Entity<Sensor>().Property(us => us.BatteryStatus).HasColumnName("battery_status");
            modelBuilder.Entity<Sensor>().Property(us => us.OptimalGDD).HasColumnName("optimal_gdd");
            modelBuilder.Entity<Sensor>().Property(us => us.CuttingDateTimeCalculated).HasColumnName("estimated_date");
            modelBuilder.Entity<Sensor>().Property(us => us.LastForecastDate).HasColumnName("last_forecast_date");
            modelBuilder.Entity<Sensor>().Property(us => us.State).HasConversion(
            v => v!.ToString(), v => (Enum)Enum.Parse(typeof(Enum), v)).HasMaxLength(250).HasColumnName("state");
            

            modelBuilder.Entity<Sensor>().HasMany<User>(us => us.Users)
                .WithMany(us => us.Sensors);


            #endregion
         
            #region Notification
            modelBuilder.Entity<Notification>().ToTable("notification");
            //Primary Key & Identity Column
            modelBuilder.Entity<Notification>().HasKey(us => us.NotificationId);
            modelBuilder.Entity<Notification>().Property(us => us.NotificationId).IsRequired().HasColumnName("Notification_id");
            //COLUMN SETTINGS 
            modelBuilder.Entity<Notification>().Property(us => us.Title).HasMaxLength(100).HasColumnName("title");
            modelBuilder.Entity<Notification>().Property(us => us.Message).HasMaxLength(200).HasColumnName("message");
            modelBuilder.Entity<Notification>().Property(us => us.SentBy).HasConversion(
            v => v!.ToString(),v => (Enum)Enum.Parse(typeof(Enum), v)).HasColumnName("sent_by");
            modelBuilder.Entity<Notification>().Property(us => us.Status).HasConversion(
            v => v!.ToString(),v => (Enum)Enum.Parse(typeof(Enum), v)).HasColumnName("status");
            modelBuilder.Entity<Notification>().Property(us => us.Title).IsRequired(true).HasMaxLength(100).HasColumnName("title");
            modelBuilder.Entity<Notification>().Property(us => us.Message).IsRequired(true).HasMaxLength(200).HasColumnName("message");
            modelBuilder.Entity<Notification>().Property(us => us.SentBy).IsRequired(true).HasConversion(
            v => v.ToString(),v => (Enum)Enum.Parse(typeof(Enum), v)).HasColumnName("sent_by");
            modelBuilder.Entity<Notification>().Property(us => us.Status).IsRequired(true).HasConversion(
            v => v.ToString(),v => (Enum)Enum.Parse(typeof(Enum), v)).HasColumnName("status");
         

            #endregion



        }
    }
}