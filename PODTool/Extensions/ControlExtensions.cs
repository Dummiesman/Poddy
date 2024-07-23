using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PODTool.Extensions
{
    static class ControlExtensions
    {
        public static void EnableDoubleBuffering(this Control control)
        {
            SendMessage(control.Handle, TVM_SETEXTENDEDSTYLE, (IntPtr)TVS_EX_DOUBLEBUFFER, (IntPtr)TVS_EX_DOUBLEBUFFER);
        }

        private const int TVM_SETEXTENDEDSTYLE = 0x1100 + 44;
        private const int TVS_EX_DOUBLEBUFFER = 0x0004;

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
    }
}
