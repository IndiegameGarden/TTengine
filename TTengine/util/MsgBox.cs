using System;
using System.Runtime.InteropServices;

namespace TTengine.Util
{

    public class MsgBox
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int MessageBox(IntPtr hWnd, String text, String caption, uint type);

        /**
         * Shows a Windows MessageBox with title and content 'msg', returning true when
         * successfully shown and false if MsgBox could not be created/shown.
         */
        public static bool Show(String windowTitle, String msg) 
        {
            try{
                MessageBox(new IntPtr(0), msg, windowTitle, 0);
                return true;
            }catch(Exception) {
          	    return false;
            }
        }
    }
}
