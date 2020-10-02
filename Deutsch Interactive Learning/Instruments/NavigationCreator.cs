using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Deutsch_Interactive_Learning.Instruments
{
    class NavigationCreator
    {
        private ViewControls.NavigationControls.Curtain Curtain;

        public void CreateNavigation(Form Form, FlatDesign.Colors Colors, List<ViewControls.ContainerControls.Container> Containers)
        {
            Curtain = new ViewControls.NavigationControls.Curtain(Form, Colors);
            Curtain.GetItemsFrom(Containers);
        }
    }
}
