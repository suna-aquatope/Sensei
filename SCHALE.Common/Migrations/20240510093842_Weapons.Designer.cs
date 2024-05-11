﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SCHALE.Common.Database;

#nullable disable

namespace SCHALE.Common.Migrations
{
    [DbContext(typeof(SCHALEContext))]
    [Migration("20240510093842_Weapons")]
    partial class Weapons
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SCHALE.Common.Database.AccountDB", b =>
                {
                    b.Property<long>("ServerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("ServerId"));

                    b.Property<DateTime?>("BirthDay")
                        .HasColumnType("datetime2");

                    b.Property<string>("CallName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CallNameUpdateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("DevId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Exp")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("LastConnectTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<DateTime?>("LinkRewardDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("LobbyMode")
                        .HasColumnType("int");

                    b.Property<long>("MemoryLobbyUniqueId")
                        .HasColumnType("bigint");

                    b.Property<string>("Nickname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("PublisherAccountId")
                        .HasColumnType("bigint");

                    b.Property<int>("RepresentCharacterServerId")
                        .HasColumnType("int");

                    b.Property<int?>("RetentionDays")
                        .HasColumnType("int");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<int?>("UnReadMailCount")
                        .HasColumnType("int");

                    b.Property<int?>("VIPLevel")
                        .HasColumnType("int");

                    b.HasKey("ServerId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("SCHALE.Common.Database.CharacterDB", b =>
                {
                    b.Property<long>("ServerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("ServerId"));

                    b.Property<long>("AccountServerId")
                        .HasColumnType("bigint");

                    b.Property<string>("EquipmentServerIds")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EquipmentSlotAndDBIds")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ExSkillLevel")
                        .HasColumnType("int");

                    b.Property<long>("Exp")
                        .HasColumnType("bigint");

                    b.Property<int>("ExtraPassiveSkillLevel")
                        .HasColumnType("int");

                    b.Property<long>("FavorExp")
                        .HasColumnType("bigint");

                    b.Property<int>("FavorRank")
                        .HasColumnType("int");

                    b.Property<bool>("IsFavorite")
                        .HasColumnType("bit");

                    b.Property<bool>("IsLocked")
                        .HasColumnType("bit");

                    b.Property<int>("LeaderSkillLevel")
                        .HasColumnType("int");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<int>("PassiveSkillLevel")
                        .HasColumnType("int");

                    b.Property<string>("PotentialStats")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PublicSkillLevel")
                        .HasColumnType("int");

                    b.Property<int>("StarGrade")
                        .HasColumnType("int");

                    b.Property<long>("UniqueId")
                        .HasColumnType("bigint");

                    b.HasKey("ServerId");

                    b.HasIndex("AccountServerId");

                    b.ToTable("Characters");
                });

            modelBuilder.Entity("SCHALE.Common.Database.EchelonDB", b =>
                {
                    b.Property<long>("ServerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("ServerId"));

                    b.Property<long>("AccountServerId")
                        .HasColumnType("bigint");

                    b.Property<long>("EchelonNumber")
                        .HasColumnType("bigint");

                    b.Property<int>("EchelonType")
                        .HasColumnType("int");

                    b.Property<int>("ExtensionType")
                        .HasColumnType("int");

                    b.Property<long>("LeaderServerId")
                        .HasColumnType("bigint");

                    b.Property<string>("MainSlotServerIds")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SkillCardMulliganCharacterIds")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SupportSlotServerIds")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("TSSInteractionServerId")
                        .HasColumnType("bigint");

                    b.Property<int>("UsingFlag")
                        .HasColumnType("int");

                    b.HasKey("ServerId");

                    b.HasIndex("AccountServerId");

                    b.ToTable("Echelons");
                });

            modelBuilder.Entity("SCHALE.Common.Database.EquipmentDB", b =>
                {
                    b.Property<long>("ServerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("ServerId"));

                    b.Property<long>("AccountServerId")
                        .HasColumnType("bigint");

                    b.Property<long>("BoundCharacterServerId")
                        .HasColumnType("bigint");

                    b.Property<long>("Exp")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsLocked")
                        .HasColumnType("bit");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<long>("StackCount")
                        .HasColumnType("bigint");

                    b.Property<int>("Tier")
                        .HasColumnType("int");

                    b.Property<long>("UniqueId")
                        .HasColumnType("bigint");

                    b.HasKey("ServerId");

                    b.HasIndex("AccountServerId");

                    b.ToTable("Equipment");
                });

            modelBuilder.Entity("SCHALE.Common.Database.ItemDB", b =>
                {
                    b.Property<long>("ServerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("ServerId"));

                    b.Property<long>("AccountServerId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsLocked")
                        .HasColumnType("bit");

                    b.Property<long>("StackCount")
                        .HasColumnType("bigint");

                    b.Property<long>("UniqueId")
                        .HasColumnType("bigint");

                    b.HasKey("ServerId");

                    b.HasIndex("AccountServerId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("SCHALE.Common.Database.MissionProgressDB", b =>
                {
                    b.Property<long>("ServerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("ServerId"));

                    b.Property<long>("AccountServerId")
                        .HasColumnType("bigint");

                    b.Property<bool>("Complete")
                        .HasColumnType("bit");

                    b.Property<long>("MissionUniqueId")
                        .HasColumnType("bigint");

                    b.Property<string>("ProgressParameters")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.HasKey("ServerId");

                    b.HasIndex("AccountServerId");

                    b.ToTable("MissionProgresses");
                });

            modelBuilder.Entity("SCHALE.Common.Database.Models.AccountTutorial", b =>
                {
                    b.Property<long>("AccountServerId")
                        .HasColumnType("bigint");

                    b.Property<string>("TutorialIds")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AccountServerId");

                    b.ToTable("AccountTutorials");
                });

            modelBuilder.Entity("SCHALE.Common.Database.Models.GuestAccount", b =>
                {
                    b.Property<long>("Uid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Uid"));

                    b.Property<string>("DeviceId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Uid");

                    b.ToTable("GuestAccounts");
                });

            modelBuilder.Entity("SCHALE.Common.Database.WeaponDB", b =>
                {
                    b.Property<long>("ServerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("ServerId"));

                    b.Property<long>("AccountServerId")
                        .HasColumnType("bigint");

                    b.Property<long>("BoundCharacterServerId")
                        .HasColumnType("bigint");

                    b.Property<long>("Exp")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsLocked")
                        .HasColumnType("bit");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<int>("StarGrade")
                        .HasColumnType("int");

                    b.Property<long>("UniqueId")
                        .HasColumnType("bigint");

                    b.HasKey("ServerId");

                    b.HasIndex("AccountServerId");

                    b.ToTable("Weapons");
                });

            modelBuilder.Entity("SCHALE.Common.Database.CharacterDB", b =>
                {
                    b.HasOne("SCHALE.Common.Database.AccountDB", "Account")
                        .WithMany("Characters")
                        .HasForeignKey("AccountServerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("SCHALE.Common.Database.EchelonDB", b =>
                {
                    b.HasOne("SCHALE.Common.Database.AccountDB", "Account")
                        .WithMany("Echelons")
                        .HasForeignKey("AccountServerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("SCHALE.Common.Database.EquipmentDB", b =>
                {
                    b.HasOne("SCHALE.Common.Database.AccountDB", "Account")
                        .WithMany("Equipment")
                        .HasForeignKey("AccountServerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("SCHALE.Common.Database.ItemDB", b =>
                {
                    b.HasOne("SCHALE.Common.Database.AccountDB", "Account")
                        .WithMany("Items")
                        .HasForeignKey("AccountServerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("SCHALE.Common.Database.MissionProgressDB", b =>
                {
                    b.HasOne("SCHALE.Common.Database.AccountDB", "Account")
                        .WithMany("MissionProgresses")
                        .HasForeignKey("AccountServerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("SCHALE.Common.Database.WeaponDB", b =>
                {
                    b.HasOne("SCHALE.Common.Database.AccountDB", "Account")
                        .WithMany("Weapons")
                        .HasForeignKey("AccountServerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("SCHALE.Common.Database.AccountDB", b =>
                {
                    b.Navigation("Characters");

                    b.Navigation("Echelons");

                    b.Navigation("Equipment");

                    b.Navigation("Items");

                    b.Navigation("MissionProgresses");

                    b.Navigation("Weapons");
                });
#pragma warning restore 612, 618
        }
    }
}
