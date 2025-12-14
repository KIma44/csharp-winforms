using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinForms
{
    internal class UserSession
    {
        public static bool IsLoggedIn = false;
        public static bool IsLoggedOut = false;

        public static int? UserId = null;
        public static string UserName = null;

        // ⭐ 비회원 일정 임시 저장소
        public static List<(DateTime date, string schedule, int cost)> GuestSchedules
            = new List<(DateTime, string, int)>();


    }
}
