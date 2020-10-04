using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KubernetKV.Utils
{
    public class KVUtils
    {
        public async Task<string> GetValuesFromKV(string urlKeyVault, string key) 
        {
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            var keyVaultClient = new KeyVaultClient(
                new KeyVaultClient.AuthenticationCallback(
                    azureServiceTokenProvider.KeyVaultTokenCallback));

            //https://kvforaks.vault.azure.net/secrets/secret1/1a04e39bdf5345868901306c78502917
            var url = urlKeyVault + key;

            var value = await keyVaultClient.GetSecretAsync(url);

            return value.Value;
        }

        public async Task<IEnumerable<string>> GetAllSecretsFromKV(string urlKeyVault)
        {
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            var keyVaultClient = new KeyVaultClient(
                new KeyVaultClient.AuthenticationCallback(
                    azureServiceTokenProvider.KeyVaultTokenCallback));

            var secrets = await keyVaultClient.GetSecretsAsync(urlKeyVault);

            var enumerable = secrets.AsEnumerable().Select( s=> s.Id);

            return enumerable;
        }
    }
}
