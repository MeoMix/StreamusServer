using Streamus.Domain.Interfaces;

namespace Streamus.Domain
{
    public abstract class AbstractDomainEntity<T> : IAbstractDomainEntity<T>
    {
        public virtual T Id { get; set; }

        private int? OldHashCode;
        public override int GetHashCode()
        {
            // Once we have a hash code we'll never change it
            if (OldHashCode.HasValue)
                return OldHashCode.Value;

            //  The default of string is NULL not string.Empty
            bool thisIsTransient = typeof (T) == typeof (string) ? Equals(Id, string.Empty) : Equals(Id, default(T));

            // When this instance is transient, we use the base GetHashCode()
            // and remember it, so an instance can NEVER change its hash code.
            if (thisIsTransient)
            {
                OldHashCode = base.GetHashCode();
                return OldHashCode.Value;
            }
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            AbstractDomainEntity<T> other = obj as AbstractDomainEntity<T>;
            if (other == null)
                return false;

            // handle the case of comparing two NEW objects
            bool otherIsTransient = typeof(T) == typeof(string) ? Equals(other.Id, string.Empty) : Equals(other.Id, default(T));
            bool thisIsTransient = typeof(T) == typeof(string) ? Equals(Id, string.Empty) : Equals(Id, default(T));
            if (otherIsTransient && thisIsTransient)
                return ReferenceEquals(other, this);

            return other.Id.Equals(Id);
        }

    }
}