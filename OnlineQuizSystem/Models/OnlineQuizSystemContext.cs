using Microsoft.EntityFrameworkCore;
using OnlineQuizSystem.Models.ViewModels;

namespace OnlineQuizSystem.Models
{
    public partial class OnlineQuizSystemContext : DbContext
    {
        public OnlineQuizSystemContext()
        {
        }

        public OnlineQuizSystemContext(DbContextOptions<OnlineQuizSystemContext> options)
            : base(options)
        {
        }

        public virtual DbSet<GlobalExceptionLog> GlobalExceptionLogs { get; set; } = null!;
        public virtual DbSet<Group> Groups { get; set; } = null!;
        public virtual DbSet<Message> Messages { get; set; } = null!;
        public virtual DbSet<Question> Questions { get; set; } = null!;
        public virtual DbSet<QuestionsWithAnswer> QuestionsWithAnswers { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserQuestion> UserQuestions { get; set; } = null!;
        public virtual DbSet<UserScoreVM> UserScores { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                throw new NotImplementedException();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8_general_ci")
                .HasCharSet("utf8mb3");

            modelBuilder.Entity<GlobalExceptionLog>(entity =>
            {
                entity.HasKey(e => e.ExceptionId)
                    .HasName("PRIMARY");

                entity.ToTable("globalexceptionlogs");

                entity.Property(e => e.ExceptionId)
                    .ValueGeneratedNever()
                    .HasColumnName("ExceptionID");

                entity.Property(e => e.Detail)
                    .HasMaxLength(1024)
                    .HasColumnName("detail");

                entity.Property(e => e.Error).HasMaxLength(128);
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.ToTable("groups");

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.Property(e => e.Link).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(16);

                entity.Property(e => e.Pass).HasMaxLength(16);
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("messages");

                entity.Property(e => e.MessageId).HasColumnName("MessageID");

                entity.Property(e => e.Content).HasMaxLength(1024);

                entity.Property(e => e.SentDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("questions");

                entity.HasIndex(e => e.Path, "soru_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.QuestionId).HasColumnName("QuestionID");

                entity.Property(e => e.Path).HasMaxLength(128);
            });

            modelBuilder.Entity<QuestionsWithAnswer>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("questionswithanswers");

                entity.Property(e => e.CorrectAnswers).HasColumnName("correct_answers");

                entity.Property(e => e.Question).HasColumnName("question");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Phone)
                    .HasName("PRIMARY");

                entity.ToTable("users");

                entity.Property(e => e.Phone).ValueGeneratedNever();

                entity.Property(e => e.FirstName).HasMaxLength(16);

                entity.Property(e => e.Job).HasMaxLength(64);

                entity.Property(e => e.LastActivity)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.LastName).HasMaxLength(16);

                entity.Property(e => e.Program).HasMaxLength(64);

                entity.Property(e => e.Registration)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<UserQuestion>(entity =>
            {
                entity.ToTable("userquestions");

                entity.HasIndex(e => e.Question, "fk_userQuestions_questions1_idx");

                entity.HasIndex(e => e.Phone, "fk_userQuestions_users1_idx");

                entity.HasIndex(e => new { e.Phone, e.Question }, "userques_unique")
                    .IsUnique();

                entity.Property(e => e.UserQuestionId).HasColumnName("UserQuestionID");

                entity.HasOne(d => d.PhoneNavigation)
                    .WithMany(p => p.Userquestions)
                    .HasForeignKey(d => d.Phone)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_userQuestions_users1");

                entity.HasOne(d => d.QuestionNavigation)
                    .WithMany(p => p.Userquestions)
                    .HasForeignKey(d => d.Question)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_userQuestions_questions1");
            });

            modelBuilder.Entity<UserScoreVM>(entity =>
            {
                entity.HasNoKey()
                    .ToSqlQuery("call GetTopListStatic()");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
