using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfApp2.Model
{
    public static class AppData
    {
        public static Frame MainFrame { get; set; }
        public static TaskBankEntities db = new TaskBankEntities();
        //public static InstrEntities dbold = new InstrEntities();
        public static Users CurrentUser = new Users();
    }
}
