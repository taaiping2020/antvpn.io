using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Ordering.API.Data;
using Ordering.API.Models;

namespace Ordering.API.Migrations
{
    [DbContext(typeof(OrderingContext))]
    partial class OrderingContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:Sequence:ordering.buyerseq", "'buyerseq', 'ordering', '1', '10', '', '', 'Int64', 'False'")
                .HasAnnotation("SqlServer:Sequence:ordering.orderseq", "'orderseq', 'ordering', '1', '10', '', '', 'Int64', 'False'")
                .HasAnnotation("SqlServer:Sequence:ordering.paymentseq", "'paymentseq', 'ordering', '1', '10', '', '', 'Int64', 'False'")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Ordering.API.Models.AvailableTime", b =>
                {
                    b.Property<int>("Id")
                        .HasDefaultValue(1);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("availabletime","ordering");
                });

            modelBuilder.Entity("Ordering.API.Models.Buyer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:HiLoSequenceName", "buyerseq")
                        .HasAnnotation("SqlServer:HiLoSequenceSchema", "ordering")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.SequenceHiLo);

                    b.Property<string>("IdentityGuid")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.HasIndex("IdentityGuid")
                        .IsUnique();

                    b.ToTable("buyers","ordering");
                });

            modelBuilder.Entity("Ordering.API.Models.CardType", b =>
                {
                    b.Property<int>("Id")
                        .HasDefaultValue(1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.ToTable("cardtypes","ordering");
                });

            modelBuilder.Entity("Ordering.API.Models.GiftCard", b =>
                {
                    b.Property<string>("GiftCardKey")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("Created");

                    b.Property<string>("SubscriptionName");

                    b.HasKey("GiftCardKey");

                    b.HasIndex("SubscriptionName");

                    b.ToTable("giftcards","ordering");
                });

            modelBuilder.Entity("Ordering.API.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:HiLoSequenceName", "orderseq")
                        .HasAnnotation("SqlServer:HiLoSequenceSchema", "ordering")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.SequenceHiLo);

                    b.Property<int>("BuyerId");

                    b.Property<DateTime>("OrderDate");

                    b.Property<string>("OrderId");

                    b.Property<int>("OrderStatusId");

                    b.Property<int>("PaymentMethodId");

                    b.Property<string>("SubscriptionName");

                    b.HasKey("Id");

                    b.HasIndex("BuyerId");

                    b.HasIndex("OrderId");

                    b.HasIndex("OrderStatusId");

                    b.HasIndex("PaymentMethodId");

                    b.HasIndex("SubscriptionName");

                    b.ToTable("orders","ordering");
                });

            modelBuilder.Entity("Ordering.API.Models.OrderStatus", b =>
                {
                    b.Property<int>("Id")
                        .HasDefaultValue(1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.ToTable("orderstatus","ordering");
                });

            modelBuilder.Entity("Ordering.API.Models.PaymentMethod", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:HiLoSequenceName", "paymentseq")
                        .HasAnnotation("SqlServer:HiLoSequenceSchema", "ordering")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.SequenceHiLo);

                    b.Property<string>("Alias")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<int>("BuyerId");

                    b.Property<string>("CardHolderName")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<string>("CardNumber")
                        .IsRequired()
                        .HasMaxLength(25);

                    b.Property<int>("CardTypeId");

                    b.Property<DateTimeOffset>("Expiration");

                    b.HasKey("Id");

                    b.HasIndex("BuyerId");

                    b.HasIndex("CardTypeId");

                    b.ToTable("paymentmethods","ordering");
                });

            modelBuilder.Entity("Ordering.API.Models.Subscription", b =>
                {
                    b.Property<string>("Name")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("SubscriptionService");

                    b.HasKey("Name");

                    b.ToTable("subscriptions","ordering");
                });

            modelBuilder.Entity("Ordering.API.Models.AvailableTime", b =>
                {
                    b.HasOne("Ordering.API.Models.Subscription")
                        .WithOne("AvailableTime")
                        .HasForeignKey("Ordering.API.Models.AvailableTime", "Name")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Ordering.API.Models.GiftCard", b =>
                {
                    b.HasOne("Ordering.API.Models.Subscription", "Subscription")
                        .WithMany("GiftCards")
                        .HasForeignKey("SubscriptionName");
                });

            modelBuilder.Entity("Ordering.API.Models.Order", b =>
                {
                    b.HasOne("Ordering.API.Models.Buyer", "Buyer")
                        .WithMany()
                        .HasForeignKey("BuyerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Ordering.API.Models.Subscription")
                        .WithMany("Orders")
                        .HasForeignKey("OrderId");

                    b.HasOne("Ordering.API.Models.OrderStatus", "OrderStatus")
                        .WithMany()
                        .HasForeignKey("OrderStatusId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Ordering.API.Models.PaymentMethod", "PaymentMethod")
                        .WithMany()
                        .HasForeignKey("PaymentMethodId");

                    b.HasOne("Ordering.API.Models.Subscription", "Subscription")
                        .WithMany()
                        .HasForeignKey("SubscriptionName");
                });

            modelBuilder.Entity("Ordering.API.Models.PaymentMethod", b =>
                {
                    b.HasOne("Ordering.API.Models.Buyer")
                        .WithMany("PaymentMethods")
                        .HasForeignKey("BuyerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Ordering.API.Models.CardType", "CardType")
                        .WithMany()
                        .HasForeignKey("CardTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
