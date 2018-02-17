using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;


namespace MyMemory
{

    public interface IWindowPlacementStore
    {
        void Save(Windowplacement place);
        Windowplacement? Placement { get; }
        bool PlacementIsValid { get; }

        event EventHandler UserStoreChanged;
    }


    public class WindowPlacementManager
    {

        private readonly IWindowPlacementStore _store;
        private Window _window;


        public WindowPlacementManager(IWindowPlacementStore store)
        {
            _store = store;
            _store.UserStoreChanged += (sender, args) =>
            {
                if (_window != null)
                    WindowOnSourceInitialized(_window, null);
            };
        }
        

        public void Attach(Window window)
        {
            if (_window != null && !Equals(window, _window)) Detach();
            _window = window;
            Attach();
        }


        private void Attach()
        {
            if (_window == null) return;

            _window.Closing += WindowOnClosing;
            _window.SourceInitialized += WindowOnSourceInitialized;
        }


        private void Detach()
        {
            if (_window == null) return;

            _window.Closing -= WindowOnClosing;
            _window.SourceInitialized -= WindowOnSourceInitialized;
        }


        private void WindowOnSourceInitialized(object sender, EventArgs eventArgs)
        {
            if (_store.PlacementIsValid)
            {
                // ReSharper disable PossibleInvalidOperationException
                WindowPlacement.SetPlacement(new WindowInteropHelper((Window) sender).Handle, _store.Placement.Value);
                // ReSharper restore PossibleInvalidOperationException
            }
        }


        private void WindowOnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            _store.Save(WindowPlacement.GetPlacement(new WindowInteropHelper((Window)sender).Handle));
        }
    }



    // RECT structure required by WINDOWPLACEMENT structure
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Rect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        public Rect(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }
    }


    // POINT structure required by WINDOWPLACEMENT structure
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Point
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }


    // WINDOWPLACEMENT stores the position, size, and state of a window
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Windowplacement
    {
        public int length;
        public int flags;
        public int showCmd;
        public Point minPosition;
        public Point maxPosition;
        public Rect normalPosition;
    }


    public static class WindowPlacement
    {

        [DllImport("user32.dll")]
        private static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref Windowplacement lpwndpl);


        [DllImport("user32.dll")]
        private static extern bool GetWindowPlacement(IntPtr hWnd, out Windowplacement lpwndpl);


        private const int SwShownormal = 1;
        private const int SwShowminimized = 2;


        public static void SetPlacement(IntPtr windowHandle, Windowplacement placement)
        {
            try
            {
                placement.length = Marshal.SizeOf(typeof(Windowplacement));
                placement.flags = 0;
                placement.showCmd = (placement.showCmd == SwShowminimized ? SwShownormal : placement.showCmd);
                SetWindowPlacement(windowHandle, ref placement);
            }
            catch (InvalidOperationException exception)
            {
                // todo: NLOG
                //LogManager.GetCurrentClassLogger().Error(exception);
            }
        }


        public static Windowplacement GetPlacement(IntPtr windowHandle)
        {
            GetWindowPlacement(windowHandle, out Windowplacement placement);
            return placement;
        }
    }
}
