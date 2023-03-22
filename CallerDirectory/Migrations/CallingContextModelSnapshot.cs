﻿// <auto-generated />
using System;
using CallerDirectory.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CallerDirectory.Migrations
{
    [DbContext(typeof(CallingContext))]
    partial class CallingContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("CallerDirectory.Models.CallRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<long?>("Caller")
                        .HasColumnType("bigint");

                    b.Property<float>("Cost")
                        .HasColumnType("float");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("VARCHAR(3)");

                    b.Property<DateTime>("EndDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<long>("Recipient")
                        .HasColumnType("bigint");

                    b.Property<string>("Reference")
                        .IsRequired()
                        .HasColumnType("VARCHAR(33)");

                    b.Property<DateTime>("StartDateTime")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("Caller");

                    b.HasIndex("EndDateTime");

                    b.HasIndex("Recipient");

                    b.HasIndex("Reference")
                        .IsUnique();

                    b.HasIndex("StartDateTime");

                    b.ToTable("CallRecords");
                });
#pragma warning restore 612, 618
        }
    }
}
