﻿// <auto-generated />
using System;
using AddressBookAPI.Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AddressBookAPI.Migrations
{
    [DbContext(typeof(AddressBookContext))]
    partial class AddressBookContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AddressBookAPI.Entity.Models.address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("city")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("country")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("line1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("line2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("refTermId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("stateName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("userId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("zipCode")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("refTermId");

                    b.HasIndex("userId");

                    b.ToTable("Address");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            city = "vizag",
                            country = new Guid("ee3f90cd-2d51-40e8-a25b-f7c81f8e76b2"),
                            line1 = "s-1",
                            line2 = "s2",
                            refTermId = new Guid("f87b8232-f2d8-4286-ac13-422aa54194ce"),
                            stateName = "AndhraPradesh",
                            userId = new Guid("1e39c93c-6f67-4629-a013-ab7dc2c201b4"),
                            zipCode = "531116"
                        });
                });

            modelBuilder.Entity("AddressBookAPI.Entity.Models.asset", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("field")
                        .HasColumnType("varbinary(max)");

                    b.Property<Guid?>("userId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("userId");

                    b.ToTable("AssetDTO");
                });

            modelBuilder.Entity("AddressBookAPI.Entity.Models.email", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("emailAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("refTermId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("userId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("refTermId");

                    b.HasIndex("userId");

                    b.ToTable("Email");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            emailAddress = "psuryaraju5@gmail.com",
                            refTermId = new Guid("f87b8232-f2d8-4286-ac13-422aa54194ce"),
                            userId = new Guid("1e39c93c-6f67-4629-a013-ab7dc2c201b4")
                        });
                });

            modelBuilder.Entity("AddressBookAPI.Entity.Models.login", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("userName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Login");
                });

            modelBuilder.Entity("AddressBookAPI.Entity.Models.phone", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("phoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("refTermId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("userId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("refTermId");

                    b.HasIndex("userId");

                    b.ToTable("Phone");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            phoneNumber = "8142255769",
                            refTermId = new Guid("f87b8232-f2d8-4286-ac13-422aa54194ce"),
                            userId = new Guid("1e39c93c-6f67-4629-a013-ab7dc2c201b4")
                        });
                });

            modelBuilder.Entity("AddressBookAPI.Entity.Models.refSet", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("key")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("RefSet");

                    b.HasData(
                        new
                        {
                            Id = new Guid("7294ac28-c285-4476-89e9-0215d0cb96cd"),
                            description = "india ",
                            key = "INDIA"
                        },
                        new
                        {
                            Id = new Guid("b4005322-979c-4df5-96b2-16b2f6101006"),
                            description = "email",
                            key = "EMAIL_ADDRESS_TYPE"
                        },
                        new
                        {
                            Id = new Guid("4adab962-e8c7-489d-b9eb-2d76c8cc30a2"),
                            description = "usa",
                            key = "UNITED_STATES"
                        },
                        new
                        {
                            Id = new Guid("9406dc1f-7781-4a7c-9f21-4d0267fb35d3"),
                            description = "address ",
                            key = "ADDRESS_TYPE"
                        },
                        new
                        {
                            Id = new Guid("1b484930-bf78-4b07-afef-6e9260f31e7b"),
                            description = "country ",
                            key = "COUNTRY"
                        },
                        new
                        {
                            Id = new Guid("c8dc949e-47f7-4eac-a83d-d0ebc8031300"),
                            description = "phone",
                            key = "PHONE_NUMBER_TYPE"
                        });
                });

            modelBuilder.Entity("AddressBookAPI.Entity.Models.refTerm", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("key")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("RefTerm");

                    b.HasData(
                        new
                        {
                            Id = new Guid("12cf7780-9096-4855-a049-40476cead362"),
                            description = "work type",
                            key = "WORK"
                        },
                        new
                        {
                            Id = new Guid("f87b8232-f2d8-4286-ac13-422aa54194ce"),
                            description = "personal type",
                            key = "PERSONAL"
                        },
                        new
                        {
                            Id = new Guid("04cd138d-6ce7-4389-919c-6687cf7f011f"),
                            description = "alternate  type",
                            key = "ALTERNATE"
                        },
                        new
                        {
                            Id = new Guid("ee3f90cd-2d51-40e8-a25b-f7c81f8e76b2"),
                            description = "INDIA_TYPE",
                            key = "INDIA"
                        },
                        new
                        {
                            Id = new Guid("7aae5636-8a33-4cbd-8fbf-09d7c143ed6b"),
                            description = "USA_TYPE",
                            key = "USA"
                        },
                        new
                        {
                            Id = new Guid("05e92a12-1241-4a96-92d9-0206e500efee"),
                            description = "country",
                            key = "COUNTRY"
                        });
                });

            modelBuilder.Entity("AddressBookAPI.Entity.Models.setRefTerm", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("refSetId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("refTermId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("refSetId");

                    b.HasIndex("refTermId");

                    b.ToTable("SetRefTerm");

                    b.HasData(
                        new
                        {
                            Id = new Guid("46f05308-b5df-4ad5-a94b-2f9ba0aaa2db"),
                            refSetId = new Guid("b4005322-979c-4df5-96b2-16b2f6101006"),
                            refTermId = new Guid("f87b8232-f2d8-4286-ac13-422aa54194ce")
                        },
                        new
                        {
                            Id = new Guid("1202ff32-8983-4ef5-ab36-5151fbe5620b"),
                            refSetId = new Guid("b4005322-979c-4df5-96b2-16b2f6101006"),
                            refTermId = new Guid("12cf7780-9096-4855-a049-40476cead362")
                        },
                        new
                        {
                            Id = new Guid("b3e4c0da-3621-43f7-9549-84cdfe91b349"),
                            refSetId = new Guid("b4005322-979c-4df5-96b2-16b2f6101006"),
                            refTermId = new Guid("04cd138d-6ce7-4389-919c-6687cf7f011f")
                        },
                        new
                        {
                            Id = new Guid("d9428266-7e3a-4b58-9b8e-6724e5a00ee1"),
                            refSetId = new Guid("4adab962-e8c7-489d-b9eb-2d76c8cc30a2"),
                            refTermId = new Guid("7aae5636-8a33-4cbd-8fbf-09d7c143ed6b")
                        },
                        new
                        {
                            Id = new Guid("221a524f-15eb-421c-8b96-383a4bfa1e46"),
                            refSetId = new Guid("9406dc1f-7781-4a7c-9f21-4d0267fb35d3"),
                            refTermId = new Guid("f87b8232-f2d8-4286-ac13-422aa54194ce")
                        },
                        new
                        {
                            Id = new Guid("3fd0f674-e20e-4cd2-8512-8600c3af40ff"),
                            refSetId = new Guid("9406dc1f-7781-4a7c-9f21-4d0267fb35d3"),
                            refTermId = new Guid("12cf7780-9096-4855-a049-40476cead362")
                        },
                        new
                        {
                            Id = new Guid("0050c6c6-6ff7-486d-a2d4-75b7ebfaca0a"),
                            refSetId = new Guid("9406dc1f-7781-4a7c-9f21-4d0267fb35d3"),
                            refTermId = new Guid("04cd138d-6ce7-4389-919c-6687cf7f011f")
                        },
                        new
                        {
                            Id = new Guid("ecb24a03-2530-4c54-9f17-1e22900bd44d"),
                            refSetId = new Guid("1b484930-bf78-4b07-afef-6e9260f31e7b"),
                            refTermId = new Guid("05e92a12-1241-4a96-92d9-0206e500efee")
                        },
                        new
                        {
                            Id = new Guid("4d4e3d74-b45f-40ef-94ba-7d9d50b0fcf3"),
                            refSetId = new Guid("c8dc949e-47f7-4eac-a83d-d0ebc8031300"),
                            refTermId = new Guid("f87b8232-f2d8-4286-ac13-422aa54194ce")
                        },
                        new
                        {
                            Id = new Guid("d5cf481f-7d61-46e3-83b1-c36f1c419b2e"),
                            refSetId = new Guid("c8dc949e-47f7-4eac-a83d-d0ebc8031300"),
                            refTermId = new Guid("12cf7780-9096-4855-a049-40476cead362")
                        },
                        new
                        {
                            Id = new Guid("4ee96531-4dda-4d5b-9cb2-965bec063e72"),
                            refSetId = new Guid("c8dc949e-47f7-4eac-a83d-d0ebc8031300"),
                            refTermId = new Guid("04cd138d-6ce7-4389-919c-6687cf7f011f")
                        },
                        new
                        {
                            Id = new Guid("3580a95e-01a9-4298-93ec-57b57c155196"),
                            refSetId = new Guid("7294ac28-c285-4476-89e9-0215d0cb96cd"),
                            refTermId = new Guid("ee3f90cd-2d51-40e8-a25b-f7c81f8e76b2")
                        });
                });

            modelBuilder.Entity("AddressBookAPI.Entity.Models.user", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("firstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("lastName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("User");

                    b.HasData(
                        new
                        {
                            Id = new Guid("1e39c93c-6f67-4629-a013-ab7dc2c201b4"),
                            firstName = "surya",
                            lastName = "raju"
                        });
                });

            modelBuilder.Entity("AddressBookAPI.Entity.Models.address", b =>
                {
                    b.HasOne("AddressBookAPI.Entity.Models.refTerm", "refTerm")
                        .WithMany()
                        .HasForeignKey("refTermId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AddressBookAPI.Entity.Models.user", "user")
                        .WithMany("address")
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AddressBookAPI.Entity.Models.asset", b =>
                {
                    b.HasOne("AddressBookAPI.Entity.Models.user", null)
                        .WithMany("assetdto")
                        .HasForeignKey("userId");
                });

            modelBuilder.Entity("AddressBookAPI.Entity.Models.email", b =>
                {
                    b.HasOne("AddressBookAPI.Entity.Models.refTerm", "refTerm")
                        .WithMany()
                        .HasForeignKey("refTermId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AddressBookAPI.Entity.Models.user", "user")
                        .WithMany("email")
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AddressBookAPI.Entity.Models.phone", b =>
                {
                    b.HasOne("AddressBookAPI.Entity.Models.refTerm", "refTerm")
                        .WithMany()
                        .HasForeignKey("refTermId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AddressBookAPI.Entity.Models.user", "user")
                        .WithMany("phone")
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AddressBookAPI.Entity.Models.setRefTerm", b =>
                {
                    b.HasOne("AddressBookAPI.Entity.Models.refSet", "refSet")
                        .WithMany()
                        .HasForeignKey("refSetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AddressBookAPI.Entity.Models.refTerm", "refTerm")
                        .WithMany()
                        .HasForeignKey("refTermId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
