using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;




namespace 逻辑符快速输入
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern UInt32 GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("User32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto)]
        public static extern int SendTextMessage(
            IntPtr hWnd,
            int Msg,
            int wParam,
            string lParam
        );

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetForegroundWindow();

        public struct tagRECT
        {
            Int32 left;
            Int32 top;
            Int32 right;
            Int32 bottom;
        };

        public struct tagGUITHREADINFO
        {
            public int cbSize;
            public int flags;
            public IntPtr hWndActive;
            public IntPtr hWndFocus;
            public IntPtr hWndCapture;
            public IntPtr hWndMenuOwner;
            public IntPtr hWndMoveSize;
            public IntPtr hWndCaret;
            public tagRECT rcCaret;
        };

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetGUIThreadInfo(int idThread, ref tagGUITHREADINFO pgui);


        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button == null)
                return;

            string s = button.Content as string;

            Debug.WriteLine(s);
            string t = Clipboard.GetText();
            Clipboard.SetText(s);
            System.Windows.Forms.SendKeys.SendWait("^{v}");
            Clipboard.SetText(t);

        }

        private void mainwindows_Loaded(object sender, RoutedEventArgs e)
        {
            //以下代码不能放到构造函数里，否则窗体丙柄为0
            WindowInteropHelper wndHelper = new WindowInteropHelper(this);
            IntPtr HWND = wndHelper.Handle;
            int GWL_EXSTYLE = -20;

            //GetWindowLong(HWND, GWL_EXSTYLE);

            SetWindowLong(HWND, GWL_EXSTYLE, (IntPtr)(0x8000000)); //让当前窗体不**输入焦点
        }
    }
}
