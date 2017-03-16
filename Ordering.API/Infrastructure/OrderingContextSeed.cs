using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Ordering.API.Data;
using Ordering.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.API.Infrastructure
{
    public class OrderingContextSeed
    {
        public static async Task SeedAsync(IApplicationBuilder applicationBuilder)
        {
            var context = (OrderingContext)applicationBuilder
                .ApplicationServices.GetService(typeof(OrderingContext));

            using (context)
            {
                context.Database.Migrate();

                if (!context.Subscriptions.Any())
                {
                    foreach (var s in Subscription.GetAllSubscription())
                    {
                        context.Subscriptions.Add(s);
                    }

                    await context.SaveChangesAsync();
                }

                if (!context.OrderStatus.Any())
                {
                    context.OrderStatus.Add(OrderStatus.Canceled);
                    context.OrderStatus.Add(OrderStatus.InProcess);
                    context.OrderStatus.Add(OrderStatus.Completed);
                }

                if (!context.AvailableTime.Any())
                {
                    context.AvailableTime.Add(AvailableTime.Annual);
                    context.AvailableTime.Add(AvailableTime.SemiAnnual);
                    context.AvailableTime.Add(AvailableTime.Quarter);
                    context.AvailableTime.Add(AvailableTime.Month);
                    context.AvailableTime.Add(AvailableTime.Week);
                }

                await context.SaveChangesAsync();
            }
        }

    }
}
