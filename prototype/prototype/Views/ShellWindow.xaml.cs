using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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
using prototype.ViewModels;

namespace NewTerminal
{
    /// <summary>
    /// Логика взаимодействия для ShellWindow.xaml
    /// </summary>
    public partial class ShellWindow : Window
    {
        public ShellWindow(/*ShellWindowViewModel viewModel*/)
        {
            InitializeComponent();
            ViewModel = new MainWindowViewModel();
            SourceInitialized += OnSourceInitialized;
            SizeChanged += ShellWindow_SizeChanged;
        }

        #region Implementation of IView<out ShellWindowViewModel>
        public MainWindowViewModel ViewModel
        {
            get => (MainWindowViewModel) DataContext;
            set => DataContext = value;
        }
        #endregion


        #region Implementation of IRegionMemberLifetime

        public bool KeepAlive => false;

        #endregion

        private void ButtonMinimizeWindow_OnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void ButtonExpandWindow_OnClick(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                var newHeight = ActualHeight / 1.5;
                var newWidth = ActualWidth / 1.5;
                if (newHeight >= MinHeight && newWidth >= MinWidth)
                {
                    Width = newWidth;
                    Height = newHeight;
                }
            }
            else WindowState = WindowState.Maximized;
        }

        private void ButtonCloseWindow_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ShellWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //dockingContentControl.Height = e.NewSize.Height - 110;
            //dockingContentControl.Width = e.NewSize.Width;
        }

        // Nearest monitor to window

        const int MONITOR_DEFAULTTONEAREST = 2;

        // To get a handle to the specified monitor

        [DllImport("user32.dll")]
        static extern IntPtr MonitorFromWindow(IntPtr hwnd, int dwFlags);

        // Rectangle (used by MONITORINFOEX)

        [StructLayout(LayoutKind.Sequential)]

        public struct RECT

        {

            public int Left;

            public int Top;

            public int Right;

            public int Bottom;

        }

        // Monitor information (used by GetMonitorInfo())

        [StructLayout(LayoutKind.Sequential)]

        public class MONITORINFOEX

        {

            public int cbSize;

            public RECT rcMonitor; // Total area

            public RECT rcWork; // Working area

            public int dwFlags;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x20)]

            public char[] szDevice;

        }

        // To get the working area of the specified monitor

        [DllImport("user32.dll")]
        public static extern bool GetMonitorInfo(HandleRef hmonitor, [In, Out] MONITORINFOEX monitorInfo);

        void OnSourceInitialized(object sender, EventArgs e)
        {
            // Make window borderless

            this.WindowStyle = WindowStyle.None;

            this.ResizeMode = ResizeMode.NoResize;

            // Get handle for nearest monitor to this window

            WindowInteropHelper wih = new WindowInteropHelper(this);

            IntPtr hMonitor = MonitorFromWindow(wih.Handle, MONITOR_DEFAULTTONEAREST);

            // Get monitor info

            MONITORINFOEX monitorInfo = new MONITORINFOEX();

            monitorInfo.cbSize = Marshal.SizeOf(monitorInfo);

            GetMonitorInfo(new HandleRef(this, hMonitor), monitorInfo);

            // Create working area dimensions, converted to DPI-independent values

            HwndSource source = HwndSource.FromHwnd(wih.Handle);

            if (source == null) return; // Should never be null

            if (source.CompositionTarget == null) return; // Should never be null

            Matrix matrix = source.CompositionTarget.TransformFromDevice;

            RECT workingArea = monitorInfo.rcWork;

            Point dpiIndependentSize =

                matrix.Transform(

                new Point(

                    workingArea.Right - workingArea.Left,

                    workingArea.Bottom - workingArea.Top));

            // Maximize the window to the device-independent working area ie

            // the area without the taskbar.

            // NOTE - window state must be set to Maximized as this adds certain

            // maximized behaviors eg you can't move a window while it is maximized,

            // such as by calling Window.DragMove

            this.Top = 0;

            this.Left = 0;

            this.MaxWidth = dpiIndependentSize.X;

            this.MaxHeight = dpiIndependentSize.Y;

            this.WindowState = WindowState.Maximized;

        }
    }
}
