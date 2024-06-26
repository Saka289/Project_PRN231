﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Web.Services.PaymentAPI.Data;

#nullable disable

namespace Web.Services.PaymentAPI.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240303160434_Initial_PaymentDB")]
    partial class Initial_PaymentDB
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.25")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Web.Services.PaymentAPI.Models.Payments", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("isPayed")
                        .HasColumnType("bit");

                    b.Property<Guid>("orderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("paymentStatus")
                        .HasColumnType("int");

                    b.Property<decimal>("refund")
                        .HasColumnType("decimal(10,2)");

                    b.HasKey("id");

                    b.ToTable("Payments");
                });
#pragma warning restore 612, 618
        }
    }
}
