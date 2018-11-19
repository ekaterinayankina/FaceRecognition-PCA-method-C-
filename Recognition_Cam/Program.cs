using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Recognition_Cam
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
			Form1 f = new Form1();
            Application.Run(f);
			Form1.capture.Dispose();
			Form1.faceCascade.Dispose();
			Form1.connection.Dispose();
			f.Image_in_work.Dispose();
			f.Dispose();
			
		}
    }
}
