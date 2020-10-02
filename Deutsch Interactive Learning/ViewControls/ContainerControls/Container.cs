using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using Microsoft.Speech.Synthesis;

namespace Deutsch_Interactive_Learning.ViewControls.ContainerControls
{
    public class Container
    {
        private Panel GroundPanel;
        private Panel HeaderPanel;
        private Button BackButton;
        private Label CaptionLabel;
        private Label OverviewLabel;
        private Panel ControlPanel;

        private List<Thread> Threads;

        private NavigationControls.Curtain Curtain;

        private string name;
        private Bitmap image;
        private string curtainItemName;
        private Bitmap curtainItemImage;
        private string stripeItemName;
        private Pointer pointer;

        protected delegate void ContainerEventHandler();
        protected ContainerEventHandler OnHide;

        public string Name
        {
            get { return name; }
        }

        public Bitmap Image
        {
            get { return image; }
        }

        public string CurtainItemName
        {
            get { return curtainItemName; }
        }

        public Bitmap CurtainItemImage
        {
            get { return curtainItemImage; }
        }

        public string StripeItemName
        {
            get { return stripeItemName; }
        }

        public Pointer Pointer
        {
            get { return pointer; }
        }

        public bool Active
        {
            get { return GroundPanel.Visible; }
        }

        public Size ContainerSize
        {
            get { return ControlPanel.Size; }
        }

        public void CreateContainer(Form ParentForm, FlatDesign.Colors Colors, string Name, string Overview, Bitmap Image, string CurtainItemName, Bitmap CurtainItemImage, string StripeItemName)
        {
            name = Name;
            image = Image;
            curtainItemName = CurtainItemName;
            curtainItemImage = CurtainItemImage;
            stripeItemName = StripeItemName;
            pointer = new Pointer(this, Colors);
            pointer.Button.MouseClick += Control_MouseClick;
            Threads = new List<Thread>();

            GroundPanel = new Panel();
            GroundPanel.TabIndex = 0;
            GroundPanel.TabStop = false;
            GroundPanel.Location = new Point(51, 31);
            GroundPanel.Size = new Size(ParentForm.Width - 52, ParentForm.Height - 32);
            GroundPanel.BorderStyle = BorderStyle.None;
            GroundPanel.BackColor = Colors.Background;
            GroundPanel.Visible = false;
            GroundPanel.MouseClick += Control_MouseClick;

            HeaderPanel = new Panel();
            HeaderPanel.TabIndex = 0;
            HeaderPanel.TabStop = false;
            HeaderPanel.Location = new Point(0, 0);
            HeaderPanel.Size = new Size(GroundPanel.Width, 50);
            HeaderPanel.BorderStyle = BorderStyle.None;
            HeaderPanel.BackColor = Colors.Origin;
            HeaderPanel.MouseClick += Control_MouseClick;

            BackButton = new Button();
            BackButton.TabIndex = 0;
            BackButton.TabStop = false;
            BackButton.Location = new Point(0, 0);
            BackButton.Size = new Size(HeaderPanel.Height, HeaderPanel.Height);
            BackButton.FlatStyle = FlatStyle.Flat;
            BackButton.BackColor = HeaderPanel.BackColor;
            BackButton.FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
            BackButton.FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
            BackButton.FlatAppearance.BorderSize = 0;
            BackButton.Image = new Bitmap(@"..\..\..\downloads\Icons\Back.png");
            BackButton.ImageAlign = ContentAlignment.MiddleCenter;
            BackButton.MouseClick += BackButton_MouseClick;

            CaptionLabel = new Label();
            CaptionLabel.TabIndex = 0;
            CaptionLabel.Location = new Point(BackButton.Right, 4);
            CaptionLabel.AutoSize = true;
            CaptionLabel.BorderStyle = BorderStyle.None;
            CaptionLabel.FlatStyle = FlatStyle.Flat;
            CaptionLabel.BackColor = HeaderPanel.BackColor;
            CaptionLabel.Font = new Font("Calibri", 24);
            CaptionLabel.ForeColor = Colors.Constant;
            CaptionLabel.Text = Name;
            CaptionLabel.TextAlign = ContentAlignment.MiddleLeft;
            CaptionLabel.MouseClick += Control_MouseClick;

            OverviewLabel = new Label();
            OverviewLabel.TabIndex = 0;
            OverviewLabel.Location = new Point(200, 0);
            OverviewLabel.Size = new Size(HeaderPanel.Width - 200, HeaderPanel.Height);
            OverviewLabel.BorderStyle = BorderStyle.None;
            OverviewLabel.FlatStyle = FlatStyle.Flat;
            OverviewLabel.BackColor = HeaderPanel.BackColor;
            OverviewLabel.Font = new Font("Calibri", 12);
            OverviewLabel.ForeColor = Colors.Constant;
            OverviewLabel.Text = Overview;
            OverviewLabel.TextAlign = ContentAlignment.MiddleLeft;
            OverviewLabel.MouseClick += Control_MouseClick;

            ControlPanel = new Panel();
            ControlPanel.TabIndex = 0;
            ControlPanel.TabStop = false;
            ControlPanel.Location = new Point(0, HeaderPanel.Height);
            ControlPanel.Size = new Size(GroundPanel.Width, GroundPanel.Height - HeaderPanel.Height);
            ControlPanel.BorderStyle = BorderStyle.None;
            ControlPanel.BackColor = Colors.Background;
            ControlPanel.MouseClick += Control_MouseClick;

            HeaderPanel.Controls.Add(BackButton);
            HeaderPanel.Controls.Add(CaptionLabel);
            HeaderPanel.Controls.Add(OverviewLabel);

            GroundPanel.Controls.Add(HeaderPanel);
            GroundPanel.Controls.Add(ControlPanel);

            ParentForm.Controls.Add(GroundPanel);
        }

        private void Control_MouseClick(object sender, MouseEventArgs e)
        {
            Curtain.Hide();
        }

        public void SetOriginCurtain(NavigationControls.Curtain Curtain)
        {
            this.Curtain = Curtain;
        }

        private void BackButton_MouseClick(object sender, MouseEventArgs e)
        {
            Hide();
        }

        public void Show()
        {
            GroundPanel.Visible = true;
        }

        public void Hide()
        {
            OnHide();
            GroundPanel.Visible = false;
            for (int i = 0; i < Threads.Count; i++)
            {
                try { Threads[i].Abort(); }
                catch { continue; }
            }
        }

        protected void AddControl(Control Control)
        {
            ControlPanel.Controls.Add(Control);
            Control.MouseClick += Control_MouseClick;
        }

        protected void AddThread(Thread Thread, bool Start)
        {
            Thread.IsBackground = true;
            Threads.Add(Thread);
            if (Start == true)
                Thread.Start();
        }
    }
}
