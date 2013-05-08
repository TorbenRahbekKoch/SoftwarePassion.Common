using System;
using System.ServiceModel;
    
namespace Core.Wcf.Extensions
{
    /// <summary>
    /// Defines extension methods for types compatible with the
    /// <see cref="ICommunicationObject"/> interface.
    /// </summary>
    public static class CommunicationObjectExtensions
    {
        /// <summary>
        /// Tries to close the <see cref="ICommunicationObject"/>
        /// the recommended way. If this fails, an attempt is made
        /// to abort the object.
        /// </summary>
        /// <param name="communicationObject">The object to close safely.</param>
        public static void CloseSafely(this ICommunicationObject communicationObject)
        {
            if (communicationObject == null)
            {
                return;
            }

            try
            {
                communicationObject.Close();
            }
            catch (CommunicationException)
            {
                try
                {
                    communicationObject.Abort();
                }
                catch (Exception)
                {
                    try
                    {
                        // Nothing we can do here :-(
                    }
                    catch (Exception)
                    {                        
                    }
                }
            }
        }
    }
}
