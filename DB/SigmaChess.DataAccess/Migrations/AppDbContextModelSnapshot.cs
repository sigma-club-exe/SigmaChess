﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SigmaChess.DataAccess;

#nullable disable

namespace SigmaChess.DataAccess.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SigmaChess.Core.Models.GameSession", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("GameId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Player1TgId")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<string>("Player2TgId")
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<DateTime?>("StartedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Player1TgId");

                    b.HasIndex("Player2TgId");

                    b.ToTable("DbGameSession");
                });

            modelBuilder.Entity("SigmaChess.Core.Models.UserAuth", b =>
                {
                    b.Property<string>("TgId")
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<DateOnly>("AuthDate")
                        .HasColumnType("date");

                    b.Property<string>("Avatar")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TgUsername")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("TgId");

                    b.ToTable("DbUserAuth");
                });

            modelBuilder.Entity("SigmaChess.Core.Models.GameSession", b =>
                {
                    b.HasOne("SigmaChess.Core.Models.UserAuth", "Player1")
                        .WithMany()
                        .HasForeignKey("Player1TgId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SigmaChess.Core.Models.UserAuth", "Player2")
                        .WithMany()
                        .HasForeignKey("Player2TgId");

                    b.Navigation("Player1");

                    b.Navigation("Player2");
                });
#pragma warning restore 612, 618
        }
    }
}
