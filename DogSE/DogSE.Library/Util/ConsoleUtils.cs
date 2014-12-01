using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace DogSE.Library.Util
{
    /// <summary>
    /// 控制台的一些辅助类
    /// </summary>
    public class ConsoleUtils
    {

        /// <summary>
        /// 控制台的窗口句柄
        /// </summary>
        public static IntPtr ConsoleHwnd = GetConsoleWindow(); // 控制台的窗口句柄

        [DllImport("kernel32.dll")]
        private extern static IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        private extern static IntPtr GetSystemMenu(IntPtr hWnd, IntPtr bRevert);
        [DllImport("user32.dll")]
        private extern static int RemoveMenu(IntPtr hMenu, int iPos, int iFlags);
        [DllImport("user32.dll")]
        private extern static bool DrawMenuBar(IntPtr hWnd);



        /// <summary>
        /// 去除关闭按扭，防止控制台程序被误关闭
        /// </summary>
        public static void RemoveSystemCloseMenu()
        {
            const int SC_CLOSE = 0xF060;
            IntPtr closeMenu = GetSystemMenu(ConsoleHwnd, IntPtr.Zero);
            RemoveMenu(closeMenu, SC_CLOSE, 0x0);
            DrawMenuBar(ConsoleHwnd);
        }

    }
}
