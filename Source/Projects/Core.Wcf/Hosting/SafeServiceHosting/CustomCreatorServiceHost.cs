using System;
using System.ServiceModel;

namespace SoftwarePassion.Common.Core.Wcf.Hosting.SafeServiceHosting
{
    /// <summary>
    /// A ServiceHost that uses a Func[object] creation function to 
    /// create the actual service host instance. 
    /// </summary>
    internal class CustomCreatorServiceHost : ServiceHost
    {
        public CustomCreatorServiceHost(Func<object> instanceCreator, Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
            if (instanceCreator == null)
            {
                throw new ArgumentNullException("instanceCreator");
            }

            var contracts = ImplementedContracts.Values;
            foreach (var c in contracts)
            {
                var instanceProvider = new CustomCreatorInstanceProvider(instanceCreator);
                c.Behaviors.Add(instanceProvider);
            }
        }
    }
}