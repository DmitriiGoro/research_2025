﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ThomasonAlgorithm.Models;

#nullable disable

namespace ThomasonAlgorithm.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250216122056_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ThomasonAlgorithm.Models.Experiment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("K")
                        .HasColumnType("integer");

                    b.Property<int>("MaxChordLength")
                        .HasColumnType("integer");

                    b.Property<double>("TimeToFindFirstCycle")
                        .HasColumnType("double precision");

                    b.Property<double>("TimeToFindSecondCycle")
                        .HasColumnType("double precision");

                    b.Property<int>("VerticesNumber")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Experiments");
                });
#pragma warning restore 612, 618
        }
    }
}
