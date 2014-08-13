using System;

namespace SoftwarePassion.Common.Core
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

        /// <summary>
        /// Return true if this instance has a value, otherwise false is returned.
        /// </summary>
        public bool HasValue {get { return OptionType == OptionType.Some; }}

        /// <summary>
        /// Gets the value, if it is valid.
        /// </summary>
        /// <value>The value.</value>
        /// <exception cref="InvalidOperationException">When the options is a None-option.</exception>
        public TOptionType Value { get { return PerformValue; } }

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

        override protected TOptionType PerformValue { get { return value; } }

        private readonly TOptionType value;
    }
}