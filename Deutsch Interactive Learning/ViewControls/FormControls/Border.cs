using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Deutsch_Interactive_Learning.ViewControls.FormControls
{
    public class Border
    {
        private Panel[] BorderPanels = new Panel[4];

        public Border(Form ParentForm, FlatDesign.Colors Colors)
        {
            BorderPanels[0] = new Panel();
            BorderPanels[0].TabIndex = 0;
            BorderPanels[0].TabStop = false;
            BorderPanels[0].Location = new Point(0, 0);
            BorderPanels[0].Size = new Size(ParentForm.Width, 1);
            BorderPanels[0].BorderStyle = BorderStyle.None;
            BorderPanels[0].BackColor = Colors.Constant;

            BorderPanels[1] = new Panel();
            BorderPanels[1].TabIndex = 0;
            BorderPanels[1].TabStop = false;
            BorderPanels[1].Location = new Point(ParentForm.Width - 1, 0);
            BorderPanels[1].Size = new Size(1, ParentForm.Height);
            BorderPanels[1].BorderStyle = BorderStyle.None;
            BorderPanels[1].BackColor = Colors.Constant;

            BorderPanels[2] = new Panel();
            BorderPanels[2].TabIndex = 0;
            BorderPanels[2].TabStop = false;
            BorderPanels[2].Location = new Point(0, ParentForm.Height - 1);
            BorderPanels[2].Size = new Size(ParentForm.Width, 1);
            BorderPanels[2].BorderStyle = BorderStyle.None;
            BorderPanels[2].BackColor = Colors.Constant;

            BorderPanels[3] = new Panel();
            BorderPanels[3].TabIndex = 0;
            BorderPanels[3].TabStop = false;
            BorderPanels[3].Location = new Point(0, 0);
            BorderPanels[3].Size = new Size(1, ParentForm.Height);
            BorderPanels[3].BorderStyle = BorderStyle.None;
            BorderPanels[3].BackColor = Colors.Constant;

            for (int i = 0; i < BorderPanels.Length; i++)
                ParentForm.Controls.Add(BorderPanels[i]);
        }
    }
}
