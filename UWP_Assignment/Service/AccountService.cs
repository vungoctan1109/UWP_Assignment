using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UWP_Assignment.Config;
using UWP_Assignment.Entity;
using Windows.Storage;

namespace UWP_Assignment.Service
{
    internal class AccountService
    {
        private const string TokenFileName = "credential.txt";

        public async Task<bool> RegisterAsync(Account account)
        {
            var jsonString = JsonConvert.SerializeObject(account);
            using (HttpClient httpClient = new HttpClient())
            {
                HttpContent contentToSend = new StringContent(jsonString, Encoding.UTF8, ApiConfig.MediaType);
                var result = await httpClient.PostAsync($"{ApiConfig.ApiDomain}{ApiConfig.AccountPath}", contentToSend);
                if (result.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Error 500");
                    return false;
                }
            }
        }

        public async Task<Credential> LoginAsync(LoginInformation loginInformation)
        {
            var jsonString = JsonConvert.SerializeObject(loginInformation);
            using (HttpClient httpClient = new HttpClient())
            {
                HttpContent contentToSend = new StringContent(jsonString, Encoding.UTF8, ApiConfig.MediaType);
                var result = await httpClient.PostAsync($"{ApiConfig.ApiDomain}{ApiConfig.AuthenPath}", contentToSend);
                var content = await result.Content.ReadAsStringAsync();
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    SaveToken(content);
                    return JsonConvert.DeserializeObject<Credential>(content);
                }
                else
                {
                    Console.WriteLine("Error 500");
                }
            }
            return null;
        }

        public async Task<Account> GetLoggedInAccount()
        {
            Account account;
            Credential credential = await LoadToken();
            if (credential == null)
            {
                return null;
            }
            App.currentCredential = credential;
            account = await GetAccountInformation(credential.access_token);
            return account;
        }

        public async void SaveToken(string content)
        {
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile storageFile = await storageFolder.CreateFileAsync(TokenFileName, Windows.Storage.CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(storageFile, content);
        }

        public async Task<Credential> LoadToken()
        {
            try
            {
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                StorageFile storageFile = await storageFolder.GetFileAsync(TokenFileName);
                string tokenString = await FileIO.ReadTextAsync(storageFile);
                Credential credential = JsonConvert.DeserializeObject<Credential>(tokenString);
                return credential;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<Account> GetAccountInformation(string token)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                var result = await httpClient.GetAsync($"{ApiConfig.ApiDomain}{ApiConfig.AccountPath}");
                var content = await result.Content.ReadAsStringAsync();
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Account account = JsonConvert.DeserializeObject<Account>(content);
                    return account;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}