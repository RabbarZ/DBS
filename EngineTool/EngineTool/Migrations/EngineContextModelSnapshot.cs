﻿// <auto-generated />
using System;
using EngineTool;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EngineTool.Migrations
{
    [DbContext(typeof(EngineContext))]
    partial class EngineContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("EngineGame", b =>
                {
                    b.Property<Guid>("EnginesId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("GamesId")
                        .HasColumnType("char(36)");

                    b.HasKey("EnginesId", "GamesId");

                    b.HasIndex("GamesId");

                    b.ToTable("EngineGame");
                });

            modelBuilder.Entity("EngineTool.Entities.Engine", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("IgdbId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasAlternateKey("IgdbId");

                    b.ToTable("Engine");
                });

            modelBuilder.Entity("EngineTool.Entities.Game", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("IgdbId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<int>("SteamId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasAlternateKey("IgdbId");

                    b.HasAlternateKey("SteamId");

                    b.ToTable("Game");
                });

            modelBuilder.Entity("EngineTool.Entities.PlayerStats", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("GameId")
                        .HasColumnType("char(36)");

                    b.Property<int>("PlayerCount")
                        .HasColumnType("int");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasAlternateKey("GameId", "Timestamp");

                    b.ToTable("PlayerStats");
                });

            modelBuilder.Entity("EngineTool.Entities.Rating", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("GameId")
                        .HasColumnType("char(36)");

                    b.Property<int>("Score")
                        .HasColumnType("int");

                    b.Property<string>("ScoreDescription")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasAlternateKey("GameId", "Timestamp");

                    b.ToTable("Rating");
                });

            modelBuilder.Entity("EngineGame", b =>
                {
                    b.HasOne("EngineTool.Entities.Engine", null)
                        .WithMany()
                        .HasForeignKey("EnginesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EngineTool.Entities.Game", null)
                        .WithMany()
                        .HasForeignKey("GamesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EngineTool.Entities.PlayerStats", b =>
                {
                    b.HasOne("EngineTool.Entities.Game", "Game")
                        .WithMany("PlayerStats")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");
                });

            modelBuilder.Entity("EngineTool.Entities.Rating", b =>
                {
                    b.HasOne("EngineTool.Entities.Game", "Game")
                        .WithMany("Ratings")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");
                });

            modelBuilder.Entity("EngineTool.Entities.Game", b =>
                {
                    b.Navigation("PlayerStats");

                    b.Navigation("Ratings");
                });
#pragma warning restore 612, 618
        }
    }
}
