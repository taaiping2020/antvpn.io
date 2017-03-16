using Ordering.API.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ordering.API.Models
{
    public class AvailableTime
      : Enumeration
    {
        public static AvailableTime Annual = new AvailableTime(1, nameof(Annual).ToLowerInvariant());
        public static AvailableTime SemiAnnual = new AvailableTime(2, nameof(SemiAnnual).ToLowerInvariant());
        public static AvailableTime Quarter = new AvailableTime(3, nameof(Quarter).ToLowerInvariant());
        public static AvailableTime Month = new AvailableTime(4, nameof(Month).ToLowerInvariant());
        public static AvailableTime Week = new AvailableTime(5, nameof(Week).ToLowerInvariant());

        protected AvailableTime()
        {
        }

        public AvailableTime(int id, string name)
            : base(id, name)
        {
        }

        public static IEnumerable<AvailableTime> List()
        {
            return new[] { Annual, SemiAnnual, Quarter, Month, Week };
        }

        public static AvailableTime FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new ArgumentException($"Possible values for OrderStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static AvailableTime From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new ArgumentException($"Possible values for OrderStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}