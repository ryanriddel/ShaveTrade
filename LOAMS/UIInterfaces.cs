using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using MktSrvcAPI;

namespace LOAMS
{
    enum WindowType
    {
        Level2,
        Spread,
        OrderViewer
    }

    public abstract class Window
    {
        public static Dictionary<Guid, Window> windowList = new Dictionary<Guid, Window>();
        List<WindowUserControl> userControlList = new List<WindowUserControl>();

        public event WindowDataReceivedEventHandler WindowDataReceived = delegate { };
        public event WindowUserControlDataReceivedEventHandler WindowUserControlDataReceived = delegate { };
        Guid _windowID = Guid.NewGuid();
        WindowType _windowType;

        Window(WindowType windowType)
        {
            _windowType = windowType;
            WindowDataReceived += DataReceivedFromWindow;
            windowList[_windowID] = this;
        }

        private void SendDataToWindow(object data, Window window)
        {
            window.WindowDataReceived(data, this);
            
        }

        protected abstract void DataReceivedFromWindow(object data, Window window);

        private void SendDataToWindowUserControl(object data, WindowUserControl control)
        {
        }

        protected abstract void DataReceivedFromWindowUserControl(object data, WindowUserControl control);
    }

    public abstract class WindowUserControl
    {
        public static Dictionary<Guid, WindowUserControl> windowUserControlDict = new Dictionary<Guid, WindowUserControl>();

        public event WindowUserControlDataReceivedEventHandler WindowUserControlDataReceived = delegate { };

        Window _parent;

        Guid _windowUserControlID = Guid.NewGuid();

        WindowUserControl(Window parent)
        {
            _parent = parent;
            WindowUserControlDataReceived += DataReceivedFromWindowUserControl;
            windowUserControlDict[_windowUserControlID] = this;
        }

        private void SendDataToWindowUserControl(object data, WindowUserControl windowUserControl)
        {
            windowUserControl.WindowUserControlDataReceived(data, windowUserControl);
        }

        protected abstract void DataReceivedFromWindowUserControl(object data, WindowUserControl windowUserControl);
        
        
    }



    public delegate void WindowDataReceivedEventHandler(object data, Window sender);
    public delegate void WindowUserControlDataReceivedEventHandler(object data, WindowUserControl sender);

}
