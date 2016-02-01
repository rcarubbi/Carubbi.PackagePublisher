using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Carubbi.Utils.NativeWin32
{
    /// <summary>
    /// Expõe funcões da biblioteca nativa do Windows User32.dll
    /// </summary>
    public class User32Facade
    {

        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_CLOSE = 0xF060;

        private const int GWL_EXSTYLE = (-20);
        private const int WS_EX_TOOLWINDOW = 0x80;
        private const int WS_EX_APPWINDOW = 0x40000;

        public const int GW_HWNDFIRST = 0;
        public const int GW_HWNDLAST = 1;
        public const int GW_HWNDNEXT = 2;
        public const int GW_HWNDPREV = 3;
        public const int GW_OWNER = 4;
        public const int GW_CHILD = 5;

        /// <summary>
        /// Procura uma janela pelo nome
        /// </summary>
        /// <param name="lpClassName"></param>
        /// <param name="lpWindowName"></param>
        /// <returns>Handle Id da janela</returns>
        [DllImport("user32.dll")]
        public static extern int FindWindow(
            string lpClassName, // class name 
            string lpWindowName // window name 
        );


        /// <summary>
        /// Envia uma mensagem para uma determinada janela
        /// </summary>
        /// <param name="hWnd">handle to destination window</param>
        /// <param name="Msg">message</param>
        /// <param name="wParam">first message parameter</param>
        /// <param name="lParam">second message parameter</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int SendMessage(
            int hWnd,  
            uint Msg,  
            int wParam, 
            int lParam 
        );

        /// <summary>
        /// Traz uma janela para frente
        /// </summary>
        /// <param name="hWnd">handle to window</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int SetForegroundWindow(
            int hWnd // 
            );

    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        public delegate int EnumWindowsProcDelegate(int hWnd, int lParam);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lpEnumFunc"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32")]
        public static extern int EnumWindows(EnumWindowsProcDelegate lpEnumFunc, int lParam);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="h"></param>
        /// <param name="s"></param>
        /// <param name="nMaxCount"></param>
        [DllImport("User32.Dll")]
        public static extern void GetWindowText(int h, StringBuilder s, int nMaxCount);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        [DllImport("user32", EntryPoint = "GetWindowLongA")]
        public static extern int GetWindowLongPtr(int hwnd, int nIndex);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        [DllImport("user32")]
        public static extern int GetParent(int hwnd);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="wCmd"></param>
        /// <returns></returns>
        [DllImport("user32")]
        public static extern int GetWindow(int hwnd, int wCmd);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        [DllImport("user32")]
        public static extern int IsWindowVisible(int hwnd);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DllImport("user32")]
        public static extern int GetDesktopWindow();

    }
}
