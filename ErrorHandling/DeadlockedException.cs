using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwarePassion.Common.ErrorHandling
{
    /// <summary>
    /// Exception thrown when a deadlock situation is encountered.
    /// </summary>
    [Serializable]
    public class DeadlockedException : RecoverableException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeadlockedException" /> class.
        /// </summary>
        public DeadlockedException()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeadlockedException" /> class.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        public DeadlockedException(Exception innerException)
            : base(string.Empty, innerException)
        { }
    }
}
