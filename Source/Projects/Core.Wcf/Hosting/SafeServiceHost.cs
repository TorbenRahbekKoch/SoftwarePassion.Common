using System;
using SoftwarePassion.Common.Core.Wcf.Hosting.SafeServiceHosting;

namespace SoftwarePassion.Common.Core.Wcf.Hosting
{
    /// <summary>
    /// A ServiceHost wrapper, which automatically tries to reinstantiate when
    /// a HostFaulted event occurs.
    /// This is intended as a drop-in replacement for the standard ServiceHost.
    /// </summary>
    public class SafeServiceHost : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SafeServiceHost"/> class, using a Type for determining the host.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="baseAddresses">The base addresses.</param>
        public SafeServiceHost(Type serviceType, params Uri[] baseAddresses)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }

            instanceCreator = () => new SafeServiceHostTypeImplementation(serviceType, baseAddresses);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SafeServiceHost"/> class, using the given singleton as host.
        /// </summary>
        /// <param name="singletonInstance">The singleton instance.</param>
        /// <param name="baseAddresses">The base addresses.</param>
        public SafeServiceHost(object singletonInstance, params Uri[] baseAddresses)
        {
            if (singletonInstance == null)
            {
                throw new ArgumentNullException("singletonInstance");
            }

            instanceCreator = () => new SafeServiceHostInstanceImplementation(singletonInstance, baseAddresses);
        }

        /// <summary>
        /// Use this constructor when each instantiation needs some specific parameters, instead of having a singleton.
        /// </summary>
        /// <param name="customCreator">The custom creator.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="baseAddresses">The base addresses.</param>
        public SafeServiceHost(Func<object> customCreator, Type serviceType, params Uri[] baseAddresses)
        {
            if (customCreator == null)
            {
                throw new ArgumentNullException("customCreator");
            }

            instanceCreator = () => new SafeServiceHostCustomCreatorImplementation(customCreator, serviceType, baseAddresses);
        }

        /// <summary>
        /// Directs the host to start listening.
        /// </summary>
        public void Open()
        {
            Listen();
        }

        /// <summary>
        /// Directs the host to stop listening.
        /// </summary>
        public void Close()
        {
            StopListen();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Listen()
        {
            StopListen();

            instance = instanceCreator();
            instance.Faulted += HostFaulted;
            instance.Listen();
        }

        private void StopListen()
        {
            try
            {
                if (instance != null)
                {
                    instance.Faulted -= HostFaulted;
                    instance.StopListen();
                    DisposeInstanceIfNeeded();
                    instance = null;
                }
            }
            catch (Exception)
            {
            }
        }

        private void HostFaulted(object sender, EventArgs e)
        {
            Listen();
        }

        private void DisposeInstanceIfNeeded()
        {
            if (instance == null)
            {
                return;
            }

            if (!(instance is IDisposable))
            {
                return;
            }

            ((IDisposable)instance).Dispose();
        }

        private void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            try
            {
                StopListen();
            }
            catch (Exception)
            {
            }
        }

        private readonly Func<ISafeServiceHostImplementation> instanceCreator;
        private ISafeServiceHostImplementation instance;
    }
}