using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoalaBlurryScreen
{
    public partial class ScreenSaverForm : Form
    {
        #region Preview API's

        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);

        #endregion

        private bool previewMode = false;

        public ScreenSaverForm()
        {
            InitializeComponent();
        }

        public ScreenSaverForm(Rectangle Bounds)
        {
            InitializeComponent();
            this.Bounds = Bounds;
            //hide the cursor
            Cursor.Hide();
        }

        //This constructor is the handle to the select screen saver dialog preview window
        //It is used when in preview mode (/p)
        public ScreenSaverForm(IntPtr PreviewHandle)
        {
            InitializeComponent();

            //set the preview window as the parent of this window
            SetParent(this.Handle, PreviewHandle);

            //make this a child window, so when the select screensaver 
            //dialog closes, this will also close
            SetWindowLong(this.Handle, -16,
                  new IntPtr(GetWindowLong(this.Handle, -16) | 0x40000000));

            //set our window's size to the size of our window's new parent
            Rectangle ParentRect;
            GetClientRect(PreviewHandle, out ParentRect);
            this.Size = ParentRect.Size;

            //set our location at (0, 0)
            this.Location = new Point(0, 0);

            previewMode = true;
        }

        private Timer moveTimer = new Timer();
        private Random rand = new Random();
        private void ScreenSaverForm_Load(object sender, EventArgs e)
        {
            Cursor.Hide();
            TopMost = true;

            time.Text = DateTime.Now.ToString("hh:mm");
            Refresh();

            moveTimer.Interval = 3000;
            moveTimer.Tick += new EventHandler(moveTimer_Tick);
            moveTimer.Start();
        }

        private void moveTimer_Tick(object sender, System.EventArgs e)
        {
            // Move text to new location
            time.Left = rand.Next(Math.Max(1, Bounds.Width - time.Width));
            time.Top = rand.Next(Math.Max(1, Bounds.Height - time.Height));

            time.Text = DateTime.Now.ToString("hh:mm");
            Refresh();
        }

        private Point mouseLocation;

        private void ScreenSaverForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (!previewMode)
            {
                if (!mouseLocation.IsEmpty)
                {
                    // Terminate if mouse is moved a significant distance
                    if (Math.Abs(mouseLocation.X - e.X) > 5 ||
                        Math.Abs(mouseLocation.Y - e.Y) > 5)
                        Application.Exit();
                }

                // Update current mouse location
                mouseLocation = e.Location;
            }
        }

        private void ScreenSaverForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (!previewMode)
                Application.Exit();
        }

        private void ScreenSaverForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!previewMode)
                Application.Exit();
        }
    }
}
