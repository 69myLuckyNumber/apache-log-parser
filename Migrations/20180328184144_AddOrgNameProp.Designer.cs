﻿// <auto-generated />
using ApacheLogParser.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace ApacheLogParser.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20180328184144_AddOrgNameProp")]
    partial class AddOrgNameProp
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ApacheLogParser.Core.Models.File", b =>
                {
                    b.Property<int>("FileId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FileName");

                    b.Property<string>("FilePath");

                    b.Property<int>("FileSize");

                    b.Property<int>("RequestId");

                    b.HasKey("FileId");

                    b.HasIndex("RequestId")
                        .IsUnique();

                    b.ToTable("Files");
                });

            modelBuilder.Entity("ApacheLogParser.Core.Models.Host", b =>
                {
                    b.Property<byte[]>("IPAddressBytes")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(16);

                    b.Property<string>("HostName");

                    b.Property<string>("OrgName");

                    b.HasKey("IPAddressBytes")
                        .HasName("PK_IPAddressBytes");

                    b.ToTable("Hosts");
                });

            modelBuilder.Entity("ApacheLogParser.Core.Models.Request", b =>
                {
                    b.Property<int>("RequestId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateTimeRequested");

                    b.Property<string>("RequestType");

                    b.Property<byte[]>("RequestorIPAddress");

                    b.Property<byte[]>("RequestorIPAddressBytes");

                    b.Property<int>("ResponseStatusCode");

                    b.HasKey("RequestId");

                    b.HasIndex("RequestorIPAddressBytes");

                    b.ToTable("Requests");
                });

            modelBuilder.Entity("ApacheLogParser.Core.Models.File", b =>
                {
                    b.HasOne("ApacheLogParser.Core.Models.Request", "Request")
                        .WithOne("RequestedFile")
                        .HasForeignKey("ApacheLogParser.Core.Models.File", "RequestId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ApacheLogParser.Core.Models.Request", b =>
                {
                    b.HasOne("ApacheLogParser.Core.Models.Host", "Requestor")
                        .WithMany("Requests")
                        .HasForeignKey("RequestorIPAddressBytes");
                });
#pragma warning restore 612, 618
        }
    }
}