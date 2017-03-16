using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.API.Data
{
    public class OrderingContext
      : DbContext

    {
        const string DEFAULT_SCHEMA = "ordering";

        public DbSet<Subscription> Subscriptions { get; set; }

        public DbSet<GiftCard> GiftCards { get; set; }

        public DbSet<PaymentMethod> Payments { get; set; }

        public DbSet<Buyer> Buyers { get; set; }

        public DbSet<CardType> CardTypes { get; set; }

        public DbSet<OrderStatus> OrderStatus { get; set; }

        public DbSet<AvailableTime> AvailableTime { get; set; }

        public OrderingContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Subscription>().HasKey(c => c.Name);
            modelBuilder.Entity<Subscription>(ConfigureSubscription);
            modelBuilder.Entity<GiftCard>(ConfigureGiftCard);
            //modelBuilder.Entity<GiftCard>().HasKey(c => c.Id);
            modelBuilder.Entity<AvailableTime>(ConfigureOrderAvailableTime);
            modelBuilder.Entity<PaymentMethod>(ConfigurePayment);
            modelBuilder.Entity<Order>(ConfigureOrder);
            modelBuilder.Entity<CardType>(ConfigureCardTypes);
            modelBuilder.Entity<OrderStatus>(ConfigureOrderStatus);
            modelBuilder.Entity<Buyer>(ConfigureBuyer);
        }

        void ConfigureSubscription(EntityTypeBuilder<Subscription> subscriptionConfiguration)
        {
            subscriptionConfiguration.ToTable("subscriptions", DEFAULT_SCHEMA);

            subscriptionConfiguration.HasKey(b => b.Name);

            //subscriptionConfiguration.Property(o => o.Id)
            //     .ForSqlServerUseSequenceHiLo("giftcardseq", DEFAULT_SCHEMA);

            subscriptionConfiguration.HasMany(o => o.Orders)
                .WithOne()
                .HasForeignKey("OrderId");

            var navigation = subscriptionConfiguration.Metadata.FindNavigation(nameof(Subscription.Orders));

            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            //subscriptionConfiguration.HasOne(o => o.GiftCard)
            //    .WithOne()
            //    .HasForeignKey("GiftCardId");
        }

        void ConfigureGiftCard(EntityTypeBuilder<GiftCard> giftCardConfiguration)
        {
            giftCardConfiguration.ToTable("giftcards", DEFAULT_SCHEMA);

            giftCardConfiguration.HasKey(b => b.GiftCardKey);

            //giftCardConfiguration.Property(o => o.Id)
            //     .ForSqlServerUseSequenceHiLo("giftcardseq", DEFAULT_SCHEMA);

            //giftCardConfiguration.HasOne(o => o.Order)
            //    .WithOne()
            //    .HasForeignKey("OrderId");

            //giftCardConfiguration.HasOne(o => o.Subscription)
            //    .WithMany()
            //    .HasForeignKey("SubscriptionId");
        }

        void ConfigureOrderAvailableTime(EntityTypeBuilder<AvailableTime> availableTimeConfiguration)
        {
            availableTimeConfiguration.ToTable("availabletime", DEFAULT_SCHEMA);

            availableTimeConfiguration.HasKey(o => o.Id);

            availableTimeConfiguration.Property(o => o.Id)
                .HasDefaultValue(1)
                .ValueGeneratedNever()
                .IsRequired();

            availableTimeConfiguration.Property(o => o.Name)
                //.HasMaxLength(200)
                .IsRequired();
        }

        void ConfigureBuyer(EntityTypeBuilder<Buyer> buyerConfiguration)
        {
            buyerConfiguration.ToTable("buyers", DEFAULT_SCHEMA);

            buyerConfiguration.HasKey(b => b.Id);

            buyerConfiguration.Property(b => b.Id)
                .ForSqlServerUseSequenceHiLo("buyerseq", DEFAULT_SCHEMA);

            buyerConfiguration.Property(b => b.IdentityGuid)
                .HasMaxLength(200)
                .IsRequired();

            buyerConfiguration.HasIndex("IdentityGuid")
              .IsUnique(true);

            buyerConfiguration.HasMany(b => b.PaymentMethods)
               .WithOne()
               .HasForeignKey("BuyerId")
               .OnDelete(DeleteBehavior.Cascade);

            var navigation = buyerConfiguration.Metadata.FindNavigation(nameof(Buyer.PaymentMethods));

            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        }

        void ConfigurePayment(EntityTypeBuilder<PaymentMethod> paymentConfiguration)
        {
            paymentConfiguration.ToTable("paymentmethods", DEFAULT_SCHEMA);

            paymentConfiguration.HasKey(b => b.Id);

            paymentConfiguration.Property(b => b.Id)
                .ForSqlServerUseSequenceHiLo("paymentseq", DEFAULT_SCHEMA);

            paymentConfiguration.Property<int>("BuyerId")
                .IsRequired();

            paymentConfiguration.Property<string>("CardHolderName")
                .HasMaxLength(200)
                .IsRequired();

            paymentConfiguration.Property<string>("Alias")
                .HasMaxLength(200)
                .IsRequired();

            paymentConfiguration.Property<string>("CardNumber")
                .HasMaxLength(25)
                .IsRequired();

            paymentConfiguration.Property<DateTimeOffset>("Expiration")
                .IsRequired();

            paymentConfiguration.Property<int>("CardTypeId")
                .IsRequired();

            paymentConfiguration.HasOne(p => p.CardType)
                .WithMany()
                .HasForeignKey("CardTypeId");
        }

        void ConfigureOrder(EntityTypeBuilder<Order> orderConfiguration)
        {
            orderConfiguration.ToTable("orders", DEFAULT_SCHEMA);

            orderConfiguration.HasKey(o => o.Id);

            orderConfiguration.Property(o => o.Id)
                .ForSqlServerUseSequenceHiLo("orderseq", DEFAULT_SCHEMA);

            orderConfiguration.Property<DateTime>("OrderDate").IsRequired();
            orderConfiguration.Property<int>("BuyerId").IsRequired();
            orderConfiguration.Property<int>("OrderStatusId").IsRequired();
            orderConfiguration.Property<int>("PaymentMethodId").IsRequired();

            //orderConfiguration.HasOne(o => o.GiftCard)
            //    .WithOne()
            //    .HasForeignKey("GiftCardId")
            //    .OnDelete(DeleteBehavior.Restrict);

            orderConfiguration.HasOne(o => o.PaymentMethod)
                .WithMany()
                .HasForeignKey("PaymentMethodId")
                .OnDelete(DeleteBehavior.Restrict);

            orderConfiguration.HasOne(o => o.Buyer)
                .WithMany()
                .HasForeignKey("BuyerId");

            orderConfiguration.HasOne(o => o.OrderStatus)
                .WithMany()
                .HasForeignKey("OrderStatusId");


            //orderConfiguration.HasOne(o => o.Subscription)
            //    .WithMany()
            //    .HasForeignKey("SubscriptionId");
        }


        void ConfigureOrderStatus(EntityTypeBuilder<OrderStatus> orderStatusConfiguration)
        {
            orderStatusConfiguration.ToTable("orderstatus", DEFAULT_SCHEMA);

            orderStatusConfiguration.HasKey(o => o.Id);

            orderStatusConfiguration.Property(o => o.Id)
                .HasDefaultValue(1)
                .ValueGeneratedNever()
                .IsRequired();

            orderStatusConfiguration.Property(o => o.Name)
                .HasMaxLength(200)
                .IsRequired();
        }

        void ConfigureCardTypes(EntityTypeBuilder<CardType> cardTypesConfiguration)
        {
            cardTypesConfiguration.ToTable("cardtypes", DEFAULT_SCHEMA);

            cardTypesConfiguration.HasKey(ct => ct.Id);

            cardTypesConfiguration.Property(ct => ct.Id)
                .HasDefaultValue(1)
                .ValueGeneratedNever()
                .IsRequired();

            cardTypesConfiguration.Property(ct => ct.Name)
                .HasMaxLength(200)
                .IsRequired();
        }
    }
}
