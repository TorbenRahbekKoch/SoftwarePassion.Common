using System;

namespace Core.Wcf.Hosting.SafeServiceHosting
{
    internal interface ISafeServiceHostImplementation
    {
        event EventHandler Faulted;

        void Listen();

        void StopListen();
    }
}