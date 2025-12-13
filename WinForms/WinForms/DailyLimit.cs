using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinForms
{
    internal class DailyLimit
    {
        public int AddCount = 0;
        public int DeleteCount = 0;
        public int UpdateCount = 0;
        public DateTime Date = DateTime.Today;

        public void ResetIfNewDay()
        {
            if (Date != DateTime.Today)
            {
                AddCount = DeleteCount = UpdateCount = 0;
                Date = DateTime.Today;
            }
        }
    }
}
