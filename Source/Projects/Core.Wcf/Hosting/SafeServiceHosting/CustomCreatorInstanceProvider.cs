using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Core.Wcf.Hosting.SafeServiceHosting
{
    /// <summary>
    /// IInstanceProvider used by CustomCreatorServiceHost to create the actual instance for WCF.
    /// </summary>
    internal class CustomCreatorInstanceProvider : IInstanceProvider, IContractBehavior
    {
        public CustomCreatorInstanceProvider(Func<object> instanceCreator)
        {
            if (instanceCreator == null)
            {
                throw new ArgumentNullException("instanceCreator");
            }

            instanceProvider = instanceCreator;
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            return instanceProvider();
        }

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            return GetInstance(instanceContext); 
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {            
        }

        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        {
        }

        public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, DispatchRuntime dispatchRuntime)
        {
            dispatchRuntime.InstanceProvider = this;
        }

        public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
        }

        public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        private readonly Func<object> instanceProvider;
    }
}