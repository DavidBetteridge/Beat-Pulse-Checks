using BeatPulse.Core;
using Microsoft.Azure.Management.Relay;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BeatPulse.AzureManagementServiceBus
{
    public class AzureServiceBusManagementLiveness : IBeatPulseLiveness
    {
        readonly string _clientID;
        readonly string _clientSecret;
        readonly string _tenantID;
        public AzureServiceBusManagementLiveness(string clientID, string clientSecret, string tenantID)
        {
            _clientID = clientID;
            _clientSecret = clientSecret;
            _tenantID = tenantID;
        }

        public async Task<LivenessResult> IsHealthy(LivenessExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var tokenCredentials = await GetTokenCredentials(_clientID, _clientSecret, _tenantID);
                var relayManagementClient = new RelayManagementClient(tokenCredentials);

                return LivenessResult.Healthy();
            }
            catch (Exception ex)
            {
                return LivenessResult.UnHealthy(ex);
            }
        }

        private static async Task<TokenCredentials> GetTokenCredentials(string clientID, string clientSecret, string tenantID)
        {
            var authContextURL = "https://login.windows.net/" + tenantID;
            var authenticationContext = new AuthenticationContext(authContextURL);
            var clientCredential = new ClientCredential(clientID, clientSecret);
            var result = await authenticationContext.AcquireTokenAsync(resource: "https://management.azure.com/", clientCredential: clientCredential);
            if (result == null)
            {
                throw new InvalidOperationException("Failed to obtain the JWT token");
            }

            return new TokenCredentials(result.AccessToken);
        }

    }
}
