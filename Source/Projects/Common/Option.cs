using System;

namespace SoftwarePassion.Common
{
    /// <summary>
    /// A functional option implementation. Inspired
    /// by Tomas Petricek's Real-World Functional Programming.
    /// Used instead of returning null. The Option type forces
    /// you to consider that a return value may be invalid.
    /// </summary>
    public static class Option
    {
        /// <summary>
        /// Create an option type with no value.
        /// </summary>
        /// <typeparam name="TOptionType">The underlying type of the option.</typeparam>
        /// <returns>A <see cref="None&lt;TOptionType&gt;"/>.</returns>
        public static Option<TOptionType> None<TOptionType>() 
        {
            return new None<TOptionType>();
        }

        /// <summary>
        /// Creates an option type with a value.
        /// </summary>
        /// <typeparam name="TOptionType">The underlying type of the option.</typeparam>
        /// <param name="value">The value of the option.</param>
        /// <returns>A <see cref="Some&lt;TOptionType&gt;"/>.</returns>
        public static Option<TOptionType> Some<TOptionType>(TOptionType value) 
        {
            return new Some<TOptionType>(value);
        }
    }

    /// <summary>
    /// A functional option implementation. Inspired
    /// by Tomas Petricek's Real-World Functional Programming.
    /// Used instead of returning null. The Option type forces
    /// you to consider that a return value may be invalid.
    /// </summary>
    /// <typeparam name="TOptionType">The type to make optional.</typeparam>
    public abstract class Option<TOptionType> 
    {
        internal Option(OptionType optionType)
        {
            this.optionType = optionType;
        }

        /// <summary>
        /// Gets the type of the option.
        /// </summary>
        public OptionType OptionType { get { return optionType; } }

        /// Return true if this instance does NOT have a value, otherwise false is returned.
        public bool IsNone { get { return OptionType == OptionType.None; } }

        /// Return true if this instance has a value, otherwise false is returned.
        public bool IsSome { get { return OptionType == OptionType.Some; } }

        /// <summary>
        /// Gets the value, if it is valid.
        /// </summary>
        /// <value>The value.</value>
        /// <exception cref="InvalidOperationException">When the options is a None-option.</exception>
        public TOptionType Value { get { return PerformValue; } }

        /// <summary>
        /// Compares this and the obj. If either is None they are considered
        /// different. If both are Some then their respective values are
        /// compared.
        /// </summary>
        /// <param name="obj">The other Option to compare with.</param>
        /// <returns>If considered equal, true is returned. Otherwise false
        /// is returned.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (!(obj is Option<TOptionType>))
                return false;

            if (this.IsNone)
                return false;

            return object.Equals(((Option<TOptionType>) obj).Value, this.Value);            
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in 
        /// hashing algorithms and data structures like a hash table.
        /// If the instance is a None, the default Hash Code is returned.
        /// If the instance is a Some, the hash code of the Value is
        /// returned.</returns>
        public override int GetHashCode()
        {
            if (this.IsNone)
                return base.GetHashCode();

            return Value.GetHashCode();
        }

        /// <summary>
        /// Performs an explicit conversion from TOptionType to Option&lt;TOptionType&gt;.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Option<TOptionType>(TOptionType value)
        {
            if (object.Equals(null, value))
                return Option.None<TOptionType>();

            return Option.Some(value);
        }

        /// <summary>
        /// Returns the value.
        /// </summary>
        /// <exception cref="InvalidOperationException">When the options is a None-option.</exception>
        protected abstract TOptionType PerformValue { get; }

        private readonly OptionType optionType;
    }

    /// <summary>
    /// The option type of an Option.
    /// </summary>
    public enum OptionType
    {
        /// <summary>
        /// The option has a value.
        /// </summary>
        Some, 

        /// <summary>
        /// The option does NOT have a value.
        /// </summary>
        None
    };


    /// <summary>
    /// The None implementation of the <see cref="Option"/> type. Use this
    /// when there is no value.
    /// </summary>
    /// <typeparam name="TOptionType">The underlying type of the option.</typeparam>
    public class None<TOptionType> : Option<TOptionType> 
    {
        internal None() : base(OptionType.None) { }

        /// <summary>
        /// Always throws <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">Always, since a None
        /// cannot have a Value.</exception>
        protected override TOptionType PerformValue
        {
            get { throw new InvalidOperationException("None does not have a value."); }
        }
    }

    /// <summary>
    /// The Some implementation of the <see cref="Option"/>. Use this
    /// when there is a value.
    /// </summary>
    /// <typeparam name="TOptionType">The underlying type of the option.</typeparam>
    public class Some<TOptionType> : Option<TOptionType>
    {
        internal Some(TOptionType value)
            : base(OptionType.Some)
        {
            this.value = value;
        }

        /// <summary>
        /// Returns the value of the Option.
        /// </summary>
        override protected TOptionType PerformValue { get { return value; } }

        private readonly TOptionType value;
    }
}