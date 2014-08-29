using System;
using System.ServiceModel;
using SoftwarePassion.Common.Core.Wcf.Extensions;

namespace SoftwarePassion.Common.Core.Wcf.Hosting.SafeServiceHosting
{
    internal class SafeServiceHostTypeImplementation : ISafeServiceHostImplementation, IDisposable
    {
        public SafeServiceHostTypeImplementation(Type serviceType, params Uri[] baseAddresses)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }

            this.serviceType = serviceType;
            this.baseAddresses = baseAddresses;
        }

        public event EventHandler Faulted = (sender, e) => { };

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Listen()
        {
            StopListen();

            serviceHost = new ServiceHost(serviceType, baseAddresses);
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

        private readonly Type serviceType;
        private readonly Uri[] baseAddresses;
        private ServiceHost serviceHost;
    }
}