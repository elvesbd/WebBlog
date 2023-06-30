using Blog.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data.Mappings
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            builder.Property("Name")
                .IsRequired()
                .HasColumnName("Name")
                .HasColumnType("NVARCHAR")
                .HasMaxLength(80);

            builder.Property("Email")
                .IsRequired()
                .HasColumnName("Email")
                .HasColumnType("VARCHAR")
                .HasMaxLength(200);

            builder.Property("PasswordHash")
                .IsRequired()
                .HasColumnName("PasswordHash")
                .HasColumnType("VARCHAR")
                .HasMaxLength(255);

            builder.Property("Image")
                .IsRequired()
                .HasColumnName("Image")
                .HasColumnType("VARCHAR")
                .HasMaxLength(2000);

            builder.Property("Slug")
                .IsRequired()
                .HasColumnName("Slug")
                .HasColumnType("VARCHAR")
                .HasMaxLength(80);

            builder.Property("Bio")
                .IsRequired()
                .HasColumnName("Bio")
                .HasColumnType("TEXT");

            builder.HasIndex(u => u.Slug, "IX_User_Slug")
                .IsUnique();

            builder.HasMany(u => u.Roles)
                .WithMany(u => u.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserRole",
                    role => role.HasOne<Role>()
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .HasConstraintName("FK_UserRole_RoleId")
                        .OnDelete(DeleteBehavior.Cascade),
                    user => user.HasOne<User>()
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_UserRole_UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                );
        }
    }
}