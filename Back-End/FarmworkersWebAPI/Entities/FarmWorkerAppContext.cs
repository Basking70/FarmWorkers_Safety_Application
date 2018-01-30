namespace FarmworkersWebAPI.Entities
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class FarmWorkerAppContext : DbContext
    {
        public FarmWorkerAppContext()
            : base("name=FarmWorkersEntities")
        {
        }

        public virtual DbSet<EducationalContent> EducationalContents { get; set; }
        public virtual DbSet<Farm> Farms { get; set; }
        public virtual DbSet<LoginCredential> LoginCredentials { get; set; }
        public virtual DbSet<Quiz> Quizs { get; set; }
        public virtual DbSet<QuizAnswer> QuizAnswers { get; set; }
        public virtual DbSet<QuizQuestion> QuizQuestions { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserCommunicationPreference> UserCommunicationPreferences { get; set; }
        public virtual DbSet<UserEducationalContent> UserEducationalContents { get; set; }
        public virtual DbSet<UserEmergencyContact> UserEmergencyContacts { get; set; }
        public virtual DbSet<UserFarm> UserFarms { get; set; }
        public virtual DbSet<UserPermission> UserPermissions { get; set; }
        public virtual DbSet<UserSecurityRole> UserSecurityRoles { get; set; }
        public virtual DbSet<WeatherHistory> WeatherHistories { get; set; }
        public virtual DbSet<QuizAttemptsUser> QuizAttemptsUsers { get; set; }
        public virtual DbSet<QuizEducationalContent> QuizEducationalContents { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EducationalContent>()
                .HasMany(e => e.UserEducationalContents)
                .WithRequired(e => e.EducationalContent)
                .HasForeignKey(e => e.EducationalContentID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EducationalContent>()
                .HasMany(e => e.Quizs)
                .WithMany(e => e.EducationalContents);
                //.Map(m => m.ToTable("QuizEducationalContent").MapLeftKey("EducationalContentID").MapRightKey(new[] { "QuizID", "QuizVersion" }));

            modelBuilder.Entity<Farm>()
                .HasMany(e => e.UserFarms)
                .WithRequired(e => e.Farm)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Farm>()
                .HasMany(e => e.WeatherHistories)
                .WithRequired(e => e.Farm)
                .HasForeignKey(e => e.WeatherLocation)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Quiz>()
                .HasMany(e => e.QuizAttemptsUsers)
                .WithRequired(e => e.Quiz)
                .HasForeignKey(e => new { e.QuizID, e.QuizVersion })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Quiz>()
                .HasMany(e => e.QuizQuestions)
                .WithRequired(e => e.Quiz)
                .HasForeignKey(e => new { e.QuizID, e.QuizVersion })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<QuizQuestion>()
                .HasMany(e => e.QuizAnswers)
                .WithRequired(e => e.QuizQuestion)
                .HasForeignKey(e => new { e.QuizID, e.QuizVersion, e.QuestionID })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserCommunicationPreferences)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserFarms)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.QuizAttemptsUsers)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserEducationalContents)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserEmergencyContacts)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserPermission>()
                .HasMany(e => e.UserSecurityRoles)
                .WithMany(e => e.UserPermissions)
                .Map(m => m.ToTable("UserSecurityRolePermission").MapLeftKey("IDPermission").MapRightKey("IDUserSecurityRole"));

            modelBuilder.Entity<QuizAttemptsUser>()
                .Property(e => e.UserAnswers)
                .IsUnicode(false);
        }
    }
}
