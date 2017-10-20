using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

//source: https://www.codeproject.com/Articles/31376/Making-a-C-screensaver
//source: https://www.harding.edu/fmccown/screensaver/screensaver.html

namespace CoalaBlurryScreen
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length > 0)
            {
                if (args[0].ToLower().Trim().Substring(0, 2) == "/s") //show
                {
                    //show the screen saver
                    ShowScreenSaver(); //this is the good stuff
                }
                else if (args[0].ToLower().Trim().Substring(0, 2) == "/p") //preview
                {
                    //preview the screen saver
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    //args[1] is the handle to the preview window
                    Application.Run(new ScreenSaverForm(new IntPtr(long.Parse(args[1]))));
                }
                else if (args[0].ToLower().Trim().Substring(0, 2) == "/c") //configure
                {
                    //configure the screen saver
                    /*Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new ConfigureForm()); //this is the good stuff*/

                    //nothing to configure
                    MessageBox.Show("This screensaver has no options that you can set",
                        "My Cool Screen Saver",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            else
            {
                //no arguments were passed (we should probably show the screen saver)
                ShowScreenSaver(); //this is the good stuff
            }

            Application.Run(new ScreenSaverForm());
        }

        static void ShowScreenSaver()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            foreach (Screen screen in Screen.AllScreens)
            {
                ScreenSaverForm screensaver = new ScreenSaverForm(screen.Bounds);
                screensaver.Show();
            }

            Application.Run();
        }
    }
}
