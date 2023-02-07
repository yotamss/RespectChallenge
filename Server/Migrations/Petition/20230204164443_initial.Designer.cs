﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Respect.Server.Data;

#nullable disable

namespace Respect.Server.Migrations.Petition
{
    [DbContext(typeof(PetitionContext))]
    [Migration("20230204164443_initial")]
    partial class initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.2");

            modelBuilder.Entity("Respect.Server.Models.Petition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Petitions");
                });

            modelBuilder.Entity("Respect.Server.Models.Vote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("PetitionId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PetitionId");

                    b.ToTable("Vote");
                });

            modelBuilder.Entity("Respect.Server.Models.Vote", b =>
                {
                    b.HasOne("Respect.Server.Models.Petition", null)
                        .WithMany("Votes")
                        .HasForeignKey("PetitionId");
                });

            modelBuilder.Entity("Respect.Server.Models.Petition", b =>
                {
                    b.Navigation("Votes");
                });
#pragma warning restore 612, 618
        }
    }
}