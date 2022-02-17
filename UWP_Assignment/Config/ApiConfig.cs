using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP_Assignment.Config
{
    internal class ApiConfig
    {
        public static string ApiDomain = "https://music-i-like.herokuapp.com";
        public static string AccountPath = "/api/v1/accounts";
        public static string AuthenPath = "/api/v1/accounts/authentication";
        public static string ListSongPath = "/api/v1/songs";
        public static string MineListPath = "/api/v1/songs/mine";
        public static string MediaType = "application/json";
    }
}