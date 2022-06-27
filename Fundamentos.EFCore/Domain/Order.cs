using CursoEFCore.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CursoEFCore.Domain
{
    public class Order
    {
        public Guid Id { get; private set; }
        public string Username { get; private set; }
        public Address Address { get; private set; }

        private Order()
        {
            // Only for EF
        }
    }

    public abstract class BaseValueObject
    {
        protected abstract IEnumerable<object?> GetEqualityComponents();

        protected static bool EqualOperator(BaseValueObject? left, BaseValueObject? right)
        {
            if (ReferenceEquals(left, objB: null) ^ ReferenceEquals(right, objB: null)) return false;

            return ReferenceEquals(left, objB: null) || left.Equals(right);
        }

        protected static bool NotEqualOperator(BaseValueObject left, BaseValueObject right)
        {
            return !(EqualOperator(left, right));
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != GetType()) return false;

            var other = (BaseValueObject)obj;

            return this.GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                  .Select(x => x != null
                                   ? x.GetHashCode()
                                   : 0)
                  .Aggregate((x, y) => x ^ y);
        }
    }

    public class Address : BaseValueObject
    {
        public string Country { get; private set; }
        public string City { get; private set; }

        public Address(string country, string city)
        {
            if (string.IsNullOrEmpty(country)) throw new ArgumentNullException(nameof(country));
            if (string.IsNullOrEmpty(city)) throw new ArgumentNullException(nameof(city));

            Country = country;
            City = city;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Country;
            yield return City;
        }
    }
}