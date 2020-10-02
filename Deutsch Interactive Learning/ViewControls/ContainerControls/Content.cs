using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;

namespace Deutsch_Interactive_Learning.ViewControls.ContainerControls
{
    public class Content
    {
        private Panel GroundPanel;
        private Panel PointerPanel;
        private Panel ScrollBarPanel;
        private Panel ScrollSliderPanel;

        private bool ScrollPermitted = false;
        private Thread ScrollEngine;
        private int Delta;
        private bool ScrollLocked = false;

        private FlatDesign.Colors Colors;
        private NavigationControls.Curtain ParentCurtain;

        public Content(Panel ParentPanel, NavigationControls.Curtain ParentCurtain, FlatDesign.Colors Colors, int Position)
        {
            this.Colors = Colors;
            this.ParentCurtain = ParentCurtain;

            GroundPanel = new Panel();
            GroundPanel.TabIndex = 0;
            GroundPanel.TabStop = false;
            GroundPanel.Location = new Point((ParentPanel.Width / (Position + 1)) * Position, 0);
            GroundPanel.Size = new Size(ParentPanel.Width / (Position + 1), ParentPanel.Height);
            GroundPanel.BorderStyle = BorderStyle.None;
            GroundPanel.BackColor = Colors.Background;
            GroundPanel.MouseWheel += GroundPanel_MouseWheel;
            GroundPanel.MouseClick += Control_MouseClick;

            PointerPanel = new Panel();
            PointerPanel.TabIndex = 0;
            PointerPanel.TabStop = false;
            PointerPanel.Location = new Point(5, 5);
            PointerPanel.Size = new Size(GroundPanel.Width - 10, 0);
            PointerPanel.BorderStyle = BorderStyle.None;
            PointerPanel.BackColor = Colors.Background;
            PointerPanel.MouseClick += Control_MouseClick;

            ScrollBarPanel = new Panel();
            ScrollBarPanel.TabIndex = 0;
            ScrollBarPanel.TabStop = false;
            ScrollBarPanel.Location = new Point(GroundPanel.Width - 10, 5);
            ScrollBarPanel.Size = new Size(5, GroundPanel.Height - 10);
            ScrollBarPanel.BorderStyle = BorderStyle.None;
            ScrollBarPanel.BackColor = Colors.Background;
            ScrollBarPanel.MouseClick += Control_MouseClick;

            ScrollSliderPanel = new Panel();
            ScrollSliderPanel.TabIndex = 0;
            ScrollSliderPanel.TabStop = false;
            ScrollSliderPanel.Location = new Point(0, 0);
            ScrollSliderPanel.Size = new Size(ScrollBarPanel.Width, ScrollBarPanel.Height);
            ScrollSliderPanel.BorderStyle = BorderStyle.None;
            ScrollSliderPanel.BackColor = Colors.Background;
            ScrollSliderPanel.MouseClick += Control_MouseClick;

            ScrollBarPanel.Controls.Add(ScrollSliderPanel);

            GroundPanel.Controls.Add(PointerPanel);
            GroundPanel.Controls.Add(ScrollBarPanel);

            ParentPanel.Controls.Add(GroundPanel);
        }

        private void Control_MouseClick(object sender, MouseEventArgs e)
        {
            ParentCurtain.Hide();
        }

        private void GroundPanel_MouseWheel(object sender, MouseEventArgs e)
        {
            if (ScrollSliderPanel.Height < ScrollBarPanel.Height)
            {
                Delta = e.Delta;
                ScrollEngine = new Thread(Scroll);
                ScrollEngine.Start();
            }
        }

        private void Scroll()
        {
            if (Delta < 0)
            {
                for (int i = 0; i < 68; i += 17)
                {
                    for (int j = 0; j < 17; j++)
                    {
                        if (ScrollSliderPanel.Bottom >= ScrollBarPanel.Height)
                            return;
                        ScrollSliderPanel.Invoke((MethodInvoker)(delegate { ScrollSliderPanel.Top++; }));
                        PointerPanel.Invoke((MethodInvoker)(delegate { PointerPanel.Top--; }));
                    }
                    Thread.Sleep(1);
                }
            }
            else if (Delta > 0)
            {
                for (int i = 0; i < 68; i += 17)
                {
                    for (int j = 0; j < 17; j++)
                    {
                        if (ScrollSliderPanel.Top <= 0)
                            return;
                        ScrollSliderPanel.Invoke((MethodInvoker)(delegate { ScrollSliderPanel.Top--; }));
                        PointerPanel.Invoke((MethodInvoker)(delegate { PointerPanel.Top++; }));
                    }
                    Thread.Sleep(1);
                }
            }
        }

        public void GetItemsFrom(List<ContainerControls.Container> Containers)
        {
            for (int i = 0; i < Containers.Count; i++)
                AddPointer(Containers[i].Pointer);
        }

        public void AddPointer(Pointer Pointer)
        {
            Pointer.Button.Width = PointerPanel.Width;
            PointerPanel.Height += Pointer.Button.Height + 5;
            Pointer.Button.Top = PointerPanel.Height - Pointer.Button.Height - 5;
            PointerPanel.Controls.Add(Pointer.Button);
            Pointer.Button.BringToFront();

            if (PointerPanel.Controls.Count > 9)
                ScrollSliderPanel.Height = ScrollBarPanel.Height - (PointerPanel.Controls.Count - 9) * 68;
            ScrollSliderPanel.Height += 20;
        }
    }
}
