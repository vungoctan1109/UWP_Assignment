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

namespace UWP_Assignment.Service
{
    internal class SongService
    {
        private AccountService accountService;

        public async Task<List<Song>> GetListSongAsync()
        {
            List<Song> listSong = new List<Song>();
            using (HttpClient httpClient = new HttpClient())
            {
                var result = await httpClient.GetAsync($"{ApiConfig.ApiDomain}{ApiConfig.ListSongPath}");
                var content = await result.Content.ReadAsStringAsync();
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    listSong = JsonConvert.DeserializeObject<List<Song>>(content);
                }
                else
                {
                    Debug.WriteLine("Error 500");
                }
            }
            return listSong;
        }

        public async Task<List<Song>> GetMySongAsync()
        {
            List<Song> listSong = new List<Song>();
            using (HttpClient httpClient = new HttpClient())
            {
                accountService = new AccountService();
                Credential token = await accountService.LoadToken();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.access_token}");

                var result = await httpClient.GetAsync($"{ApiConfig.ApiDomain}{ApiConfig.MineListPath}");
                var content = await result.Content.ReadAsStringAsync();
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    listSong = JsonConvert.DeserializeObject<List<Song>>(content);
                }
                else
                {
                    Debug.WriteLine("Error 500");
                }
            }
            return listSong;
        }

        public async Task<bool> CreatSongAsync(SongUpload songUpload)
        {
            var jsonString = JsonConvert.SerializeObject(songUpload);
            Debug.WriteLine(jsonString);
            using (HttpClient httpClient = new HttpClient())
            {
                HttpContent contentToSend = new StringContent(jsonString, Encoding.UTF8, ApiConfig.MediaType);
                var result = await httpClient.PostAsync($"{ApiConfig.ApiDomain}{ApiConfig.ListSongPath}", contentToSend);
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

        public async Task<bool> CreatMySongAsync(SongUpload songUpload)
        {
            var jsonString = JsonConvert.SerializeObject(songUpload);
            Debug.WriteLine(jsonString);
            using (HttpClient httpClient = new HttpClient())
            {
                accountService = new AccountService();
                Credential token = await accountService.LoadToken();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.access_token}");
                HttpContent contentToSend = new StringContent(jsonString, Encoding.UTF8, ApiConfig.MediaType);
                var result = await httpClient.PostAsync($"{ApiConfig.ApiDomain}{ApiConfig.MineListPath}", contentToSend);
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
    }
}