using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwarePassion.Common.ErrorHandling
{
    /// <summary>
    /// <see cref="AuthorizationException"/> is thrown when the user (machine 
    /// or actual human) does not have access to perform the requested 
    /// operation. Whether this is because of incorrect username, password 
    /// or other reasons is not necessarily clear.
    /// </summary>
    [Serializable]
    public class AuthorizationException : UnrecoverableException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public AuthorizationException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public AuthorizationException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}