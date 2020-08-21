using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace prosr.Parser
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
       "Design", "CA1036:Override methods on comparable types",
       Justification = "Since Enumeration is abstract, different concrete implementations must not be compareable.")]
    public abstract class Enumeration : IComparable
    {
        public string Name { get; private set; }

        public int Id { get; private set; }

        protected Enumeration(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }

        public static IEnumerable<T> GetAll<T>() where T : Enumeration
        {
            var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            return fields.Select(fld => fld.GetValue(null)).Cast<T>();
        }

        public static T FromString<T>(string name) where T : Enumeration
        {
            return GetAll<T>().Single(enmrtn => string.Equals(enmrtn.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        public static T FromValue<T>(int value) where T : Enumeration
        {
            return GetAll<T>().Single(enmrtn => enmrtn.Name.Equals(value));
        }

        public int CompareTo(object other)
        {
            if (!(other is Enumeration otherValue))
            {
                throw new InvalidCastException($"{nameof(other)} is not of type {nameof(Enumeration)}.");
            }

            if (GetType().Equals(otherValue.GetType()))
            {
                throw new InvalidCastException($"{nameof(otherValue)} is not of type {GetType().Name}.");
            }

            return Id.CompareTo(otherValue.Id);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Enumeration otherValue))
            {
                return false;
            }

            var typeMatches = GetType().Equals(otherValue.GetType());
            var valueMatches = Id.Equals(otherValue.Id);

            return typeMatches && valueMatches;
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
