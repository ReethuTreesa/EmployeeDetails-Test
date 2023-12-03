using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Common
{
    public static class Global
    {
        public static bool IsConsoleApp { get; set; }
        static Global()
        {
            IsConsoleApp = false;
        }

        /// <summary>
        /// Writes to the console and to the vs output window
        /// </summary>
        /// <param name="text"></param>
        public static void WriteLine(string text)
        {
#if DEBUG
            if (IsConsoleApp) { Console.WriteLine(text); }
            Debug.WriteLine(text);
#endif
        }
    }
}
