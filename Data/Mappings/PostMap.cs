using Blog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Data.Mappings
{
    public class PostMap : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("Post");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            builder.Property("Title")
                .IsRequired()
                .HasColumnName("Title")
                .HasColumnType("VARCHAR")
                .HasMaxLength(160);

            builder.Property("Summary")
                .IsRequired()
                .HasColumnName("Summary")
                .HasColumnType("VARCHAR")
                .HasMaxLength(255);

            builder.Property("Body")
                .IsRequired()
                .HasColumnName("Body")
                .HasColumnType("TEXT");

            builder.Property("Slug")
                .IsRequired()
                .HasColumnName("Slug")
                .HasColumnType("VARCHAR")
                .HasMaxLength(80);

            builder.Property("CreateDate")
                .IsRequired()
                .HasColumnName("CreateDate")
                .HasColumnType("SMALLDATETIME")
                .HasDefaultValueSql("GETDATE()");
            // .HasDefaultValue(DateTime.Now.ToUniversalTime());

            builder.Property("LastUpdateDate")
                .IsRequired()
                .HasColumnName("LastUpdateDate")
                .HasColumnType("SMALLDATETIME")
                .HasDefaultValue(DateTime.Now.ToUniversalTime());

            builder.HasIndex(p => p.Slug, "IX_Post_Slug")
                .IsUnique();

            // Relations
            builder.HasMany(p => p.Tags)
                .WithMany(p => p.Posts)
                .UsingEntity<Dictionary<string, object>>(
                    "PostTag",
                    post => post.HasOne<Tag>()
                        .WithMany()
                        .HasForeignKey("PostId")
                        .HasConstraintName("FK_PostTag_PostId")
                        .OnDelete(DeleteBehavior.Cascade),
                    tag => tag.HasOne<Post>()
                        .WithMany()
                        .HasForeignKey("TagId")
                        .HasConstraintName("FK_PostTag_TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                );
        }
    }
}