using System;
using System.ServiceModel;
using SoftwarePassion.Common.Core.Wcf.Extensions;

namespace SoftwarePassion.Common.Core.Wcf.Hosting.SafeServiceHosting
{
    internal class SafeServiceHostInstanceImplementation : ISafeServiceHostImplementation, IDisposable
    {
        public SafeServiceHostInstanceImplementation(object singletonInstance, params Uri[] baseAddresses)
        {
            if (singletonInstance == null)
            {
                throw new ArgumentNullException("singletonInstance");
            }

            this.singletonInstance = singletonInstance;
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

            serviceHost = new ServiceHost(singletonInstance, baseAddresses);
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

        private readonly object singletonInstance;
        private readonly Uri[] baseAddresses;
        private ServiceHost serviceHost;
    }
}