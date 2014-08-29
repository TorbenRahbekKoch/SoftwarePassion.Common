using System;

namespace SoftwarePassion.Common.Core.Wcf.Hosting.SafeServiceHosting
{
    internal interface ISafeServiceHostImplementation
    {
        event EventHandler Faulted;

        void Listen();

        void StopListen();
    }
}