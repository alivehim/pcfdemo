using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Core.Utilites
{
    public class WindowsHelper
    {


        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        public extern static IntPtr FindWindow(string lpClassName, string lpWindowName);

        //[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        //public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);


        [DllImport("User32")]
        private static extern int ShowWindow(int hwnd, int nCmdShow);


        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hWnd, int msg, uint wParam, uint lParam);


        [DllImport("kernel32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        //消息发送API
        [DllImport("User32.dll", EntryPoint = "PostMessage")]
        public static extern int PostMessage(
            IntPtr hWnd,        // 信息发往的窗口的句柄
            int Msg,            // 消息ID
            int wParam,         // 参数1
            ulong lParam            // 参数2
        );

        public static void ShowWindow1(int hwnd)
        {
            ShowWindow(hwnd, 3);
        }
        public static void ShowWindow()
        {
            //IntPtr hwnd = GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName);
            //ShowWindow(hwnd.ToInt32(), 1);

            //SendMessage(hwnd, 0x0018, 0, 3);

            //           IntPtr OtherExeWnd = new IntPtr(0);
            //OtherExeWnd = FindWindow("HwndWrapper[UtilityTools;;9ce7a4bf-06df-472d-b179-8576264123bc]", null);
            ////判断这个窗体是否有效
            //if (OtherExeWnd != IntPtr.Zero)
            //{
            //    Console.WriteLine("找到窗口");
            //    ShowWindow(OtherExeWnd, 0);//0表示隐藏窗口
            //}
            //else
            //{
            //    Console.WriteLine("没有找到窗口");
            //}

            Process[] processRunning = Process.GetProcesses();

            var hWnd = processRunning.First(p => p.ProcessName.StartsWith("UtilityTools")).MainWindowHandle;
            WindowsHelper.ShowWindow1(hWnd.ToInt32());
            //foreach (Process pr in processRunning)
            //{
            //    if (pr.ProcessName == "notepad")
            //    {
            //        var hWnd = pr.MainWindowHandle;
            //        WindowsHelper.ShowWindow1(hWnd.ToInt32());
            //    }
            //}

        }
    }
}
