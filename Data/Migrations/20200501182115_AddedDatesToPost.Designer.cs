﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyBlog.Data;

namespace MyBlog.Data.Migrations
{
    [DbContext(typeof(MyBlogContext))]
    [Migration("20200501182115_AddedDatesToPost")]
    partial class AddedDatesToPost
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3");

            modelBuilder.Entity("MyBlog.Data.Models.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(500);

                    b.Property<string>("FullBio")
                        .HasColumnType("TEXT");

                    b.Property<string>("JobTitle")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .HasColumnType("TEXT");

                    b.Property<string>("Permalink")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(500);

                    b.Property<string>("ShortBio")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("MyBlog.Data.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(500);

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("MyBlog.Data.Models.CategoryPost", b =>
                {
                    b.Property<int>("CategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PostId")
                        .HasColumnType("INTEGER");

                    b.HasKey("CategoryId", "PostId");

                    b.HasIndex("PostId");

                    b.ToTable("Categories_Posts");
                });

            modelBuilder.Entity("MyBlog.Data.Models.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AuthorId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Excerpt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastUpdateDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Permalink")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(500);

                    b.Property<string>("Tags")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(500);

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("MyBlog.Data.Models.CategoryPost", b =>
                {
                    b.HasOne("MyBlog.Data.Models.Category", "Category")
                        .WithMany("CategoryPosts")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyBlog.Data.Models.Post", "Post")
                        .WithMany("CategoryPosts")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MyBlog.Data.Models.Post", b =>
                {
                    b.HasOne("MyBlog.Data.Models.Author", "Author")
                        .WithMany("Posts")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
