using BeatPulse;
using BeatPulse.AzureManagementServiceBus;
using BeatPulse.Core;

namespace BeatPulse
{
    public static class BeatPulseContextExtensions
    {
        public static BeatPulseContext AddAzureServiceBusManagement(this BeatPulseContext context, string clientID, string clientSecret, string tenantID, string name = nameof(AzureServiceBusManagementLiveness), string defaultPath = "azureservicebusmanagementliveness")
        {
            return context.AddLiveness(name, setup =>
            {
                setup.UsePath(defaultPath);
                setup.UseLiveness(new AzureServiceBusManagementLiveness(clientID, clientSecret, tenantID));
            });
        }
    }
}
