using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwarePassion.Common.ErrorHandling
{
    /// <summary>
    /// TruncatedDataException is thrown when the database reports that "string or binary data would be truncated.
    /// </summary>
    [Serializable]
    public class TruncatedDataException : UnrecoverableException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruncatedDataException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public TruncatedDataException(string message)
            : base(message)
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="TruncatedDataException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public TruncatedDataException(string message, Exception innerException)
            : base(message, innerException)
        {}
    }
}