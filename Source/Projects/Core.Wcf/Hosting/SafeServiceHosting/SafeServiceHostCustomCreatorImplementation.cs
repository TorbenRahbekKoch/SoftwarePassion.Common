using System;
using SoftwarePassion.Common.Core.Wcf.Extensions;

namespace SoftwarePassion.Common.Core.Wcf.Hosting.SafeServiceHosting
{
    internal class SafeServiceHostCustomCreatorImplementation : ISafeServiceHostImplementation, IDisposable
    {
        public SafeServiceHostCustomCreatorImplementation(Func<object> customCreator, Type serviceType, params Uri[] baseAddresses)
        {
            if (customCreator == null)
            {
                throw new ArgumentNullException("customCreator");
            }

            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }

            this.customCreator = customCreator;
            this.serviceType = serviceType;
            this.baseAddresses = baseAddresses;
        }

        public event EventHandler Faulted = (sender, e) => { };

        public void Listen()
        {
            StopListen();

            serviceHost = new CustomCreatorServiceHost(customCreator, serviceType, baseAddresses);
            serviceHost.Faulted += Faulted;
            serviceHost.Open();
        }

        public void StopListen()
        {
            if (serviceHost != null)
            {
                serviceHost.Faulted -= HostFaulted;
                serviceHost.CloseSafely();
                serviceHost = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void HostFaulted(object sender, EventArgs e)
        {
            Faulted(sender, e);
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

        private readonly Func<object> customCreator;
        private readonly Type serviceType;
        private readonly Uri[] baseAddresses;
        private CustomCreatorServiceHost serviceHost;
    }
}