using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace Deutsch_Interactive_Learning.EditorForms
{
    public partial class CustomPictureBoxForm : Form
    {
        private ViewControls.FormControls.Border Border;
        private ViewControls.FormControls.Header Header;
        private FlatDesign.Colors Colors;
        private Panel ContentPanel;

        private Label CoordinateXLabel;
        private Button CoordinateXPlusButton;
        private Button CoordinateXMinusButton;

        private Label CoordinateYLabel;
        private Button CoordinateYPlusButton;
        private Button CoordinateYMinusButton;

        private Label WidthLabel;
        private Button WidthPlusButton;
        private Button WidthMinusButton;

        private Label HeightLabel;
        private Button HeightPlusButton;
        private Button HeightMinusButton;

        private Button DeleteButton;
        private Button ResetButton;

        private Thread OutsideChangeChecker;
        private int CustomPictureBoxLeft, CustomPictureBoxTop;
        private int CustomPictureBoxWidth, CustomPictureBoxHeight;

        private PictureBox CustomPictureBox;

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        [DllImport("dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);

        [DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        [DllImport("dwmapi.dll")]
        public static extern int DwmIsCompositionEnabled(ref int pfEnabled);

        private bool m_aeroEnabled;
        private const int CS_DROPSHADOW = 0x00020000;
        private const int WM_NCPAINT = 0x0085;
        private const int WM_ACTIVATEAPP = 0x001C;

        public struct MARGINS
        {
            public int leftWidth;
            public int rightWidth;
            public int topHeight;
            public int bottomHeight;
        }

        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        protected override CreateParams CreateParams
        {
            get
            {
                m_aeroEnabled = CheckAeroEnabled();

                CreateParams cp = base.CreateParams;
                if (!m_aeroEnabled)
                    cp.ClassStyle |= CS_DROPSHADOW;

                return cp;
            }
        }

        private bool CheckAeroEnabled()
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                int enabled = 0;
                DwmIsCompositionEnabled(ref enabled);
                return (enabled == 1) ? true : false;
            }
            return false;
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCPAINT:
                    if (m_aeroEnabled)
                    {
                        var v = 2;
                        DwmSetWindowAttribute(this.Handle, 2, ref v, 4);
                        MARGINS margins = new MARGINS()
                        {
                            bottomHeight = 1,
                            leftWidth = 1,
                            rightWidth = 1,
                            topHeight = 1
                        };
                        DwmExtendFrameIntoClientArea(this.Handle, ref margins);

                    }
                    break;
                default:
                    break;
            }
            base.WndProc(ref m);

            if (m.Msg == WM_NCHITTEST && (int)m.Result == HTCLIENT)
                m.Result = (IntPtr)HTCAPTION;
        }

        public CustomPictureBoxForm(PictureBox CustomPictureBox)
        {
            this.CustomPictureBox = CustomPictureBox;

            CustomPictureBoxLeft = CustomPictureBox.Left;
            CustomPictureBoxTop = CustomPictureBox.Top;
            CustomPictureBoxWidth = CustomPictureBox.Width;
            CustomPictureBoxHeight = CustomPictureBox.Height;

            InitializeComponent();
            this.Name = "CustomLabelForm";
            this.Text = CustomPictureBox.Name + " settings";
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(322, 182);
            this.FormClosing += CustomPictureBoxForm_FormClosing;

            Colors = new FlatDesign.Colors(Color.FromArgb(50, 205, 50), 10);

            Border = new ViewControls.FormControls.Border(this, Colors);
            Header = new ViewControls.FormControls.Header(this, Colors);

            ContentPanel = new Panel();
            ContentPanel.TabIndex = 0;
            ContentPanel.TabStop = false;
            ContentPanel.Location = new Point(1, 31);
            ContentPanel.Size = new Size(this.Width - 2, this.Height - 32);
            ContentPanel.BorderStyle = BorderStyle.None;
            ContentPanel.BackColor = Colors.Origin;
            this.Controls.Add(ContentPanel);

            OutsideChangeChecker = new Thread(CheckForOutsideChanges);
            OutsideChangeChecker.IsBackground = true;
            OutsideChangeChecker.Start();

            InitializeControls();
        }

        private void CustomPictureBoxForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            OutsideChangeChecker.Abort();
        }

        private void InitializeControls()
        {
            CoordinateXLabel = new Label();
            CoordinateXLabel.TabIndex = 0;
            CoordinateXLabel.Location = new Point(10, 10);
            CoordinateXLabel.Size = new Size(this.Width - 20, 30);
            CoordinateXLabel.FlatStyle = FlatStyle.Flat;
            CoordinateXLabel.BorderStyle = BorderStyle.None;
            CoordinateXLabel.BackColor = Colors.Origin;
            CoordinateXLabel.Font = new Font("Calibri", 14);
            CoordinateXLabel.ForeColor = Colors.Constant;
            CoordinateXLabel.Text = "       X coordinate : " + Convert.ToString(CustomPictureBox.Left);
            CoordinateXLabel.TextAlign = ContentAlignment.MiddleLeft;
            CoordinateXLabel.Image = new Bitmap(@"..\..\..\downloads\Icons\CoordinateX.png");
            CoordinateXLabel.ImageAlign = ContentAlignment.MiddleLeft;

            CoordinateXPlusButton = new Button();
            CoordinateXPlusButton.TabIndex = 0;
            CoordinateXPlusButton.TabStop = false;
            CoordinateXPlusButton.Location = new Point(CoordinateXLabel.Width - CoordinateXLabel.Height * 2, 0);
            CoordinateXPlusButton.Size = new Size(CoordinateXLabel.Height, CoordinateXLabel.Height);
            CoordinateXPlusButton.FlatStyle = FlatStyle.Flat;
            CoordinateXPlusButton.BackColor = Colors.Origin;
            CoordinateXPlusButton.FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
            CoordinateXPlusButton.FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
            CoordinateXPlusButton.FlatAppearance.BorderSize = 0;
            CoordinateXPlusButton.Image = new Bitmap(@"..\..\..\downloads\Icons\Plus.png");
            CoordinateXPlusButton.ImageAlign = ContentAlignment.MiddleCenter;
            CoordinateXPlusButton.MouseClick += LocationXPlusButton_MouseClick;

            CoordinateXMinusButton = new Button();
            CoordinateXMinusButton.TabIndex = 0;
            CoordinateXMinusButton.TabStop = false;
            CoordinateXMinusButton.Location = new Point(CoordinateXLabel.Width - CoordinateXLabel.Height, 0);
            CoordinateXMinusButton.Size = new Size(CoordinateXLabel.Height, CoordinateXLabel.Height);
            CoordinateXMinusButton.FlatStyle = FlatStyle.Flat;
            CoordinateXMinusButton.BackColor = Colors.Origin;
            CoordinateXMinusButton.FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
            CoordinateXMinusButton.FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
            CoordinateXMinusButton.FlatAppearance.BorderSize = 0;
            CoordinateXMinusButton.Image = new Bitmap(@"..\..\..\downloads\Icons\Minus.png");
            CoordinateXMinusButton.ImageAlign = ContentAlignment.MiddleCenter;
            CoordinateXMinusButton.MouseClick += LocationXMinusButton_MouseClick;

            CoordinateYLabel = new Label();
            CoordinateYLabel.TabIndex = 0;
            CoordinateYLabel.Location = new Point(10, CoordinateXLabel.Bottom + 2);
            CoordinateYLabel.Size = new Size(this.Width - 20, 30);
            CoordinateYLabel.FlatStyle = FlatStyle.Flat;
            CoordinateYLabel.BorderStyle = BorderStyle.None;
            CoordinateYLabel.BackColor = Colors.Origin;
            CoordinateYLabel.Font = new Font("Calibri", 14);
            CoordinateYLabel.ForeColor = Colors.Constant;
            CoordinateYLabel.Text = "       Y coordinate : " + Convert.ToString(CustomPictureBox.Top);
            CoordinateYLabel.TextAlign = ContentAlignment.MiddleLeft;
            CoordinateYLabel.Image = new Bitmap(@"..\..\..\downloads\Icons\CoordinateY.png");
            CoordinateYLabel.ImageAlign = ContentAlignment.MiddleLeft;

            CoordinateYPlusButton = new Button();
            CoordinateYPlusButton.TabIndex = 0;
            CoordinateYPlusButton.TabStop = false;
            CoordinateYPlusButton.Location = new Point(CoordinateYLabel.Width - CoordinateYLabel.Height * 2, 0);
            CoordinateYPlusButton.Size = new Size(CoordinateYLabel.Height, CoordinateYLabel.Height);
            CoordinateYPlusButton.FlatStyle = FlatStyle.Flat;
            CoordinateYPlusButton.BackColor = Colors.Origin;
            CoordinateYPlusButton.FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
            CoordinateYPlusButton.FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
            CoordinateYPlusButton.FlatAppearance.BorderSize = 0;
            CoordinateYPlusButton.Image = new Bitmap(@"..\..\..\downloads\Icons\Plus.png");
            CoordinateYPlusButton.ImageAlign = ContentAlignment.MiddleCenter;
            CoordinateYPlusButton.MouseClick += LocationYPlusButton_MouseClick;

            CoordinateYMinusButton = new Button();
            CoordinateYMinusButton.TabIndex = 0;
            CoordinateYMinusButton.TabStop = false;
            CoordinateYMinusButton.Location = new Point(CoordinateYLabel.Width - CoordinateYLabel.Height, 0);
            CoordinateYMinusButton.Size = new Size(CoordinateYLabel.Height, CoordinateYLabel.Height);
            CoordinateYMinusButton.FlatStyle = FlatStyle.Flat;
            CoordinateYMinusButton.BackColor = Colors.Origin;
            CoordinateYMinusButton.FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
            CoordinateYMinusButton.FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
            CoordinateYMinusButton.FlatAppearance.BorderSize = 0;
            CoordinateYMinusButton.Image = new Bitmap(@"..\..\..\downloads\Icons\Minus.png");
            CoordinateYMinusButton.ImageAlign = ContentAlignment.MiddleCenter;
            CoordinateYMinusButton.MouseClick += LocationYMinusButton_MouseClick;

            WidthLabel = new Label();
            WidthLabel.TabIndex = 0;
            WidthLabel.Location = new Point(10, CoordinateYLabel.Bottom + 2);
            WidthLabel.Size = new Size(this.Width - 20, 30);
            WidthLabel.FlatStyle = FlatStyle.Flat;
            WidthLabel.BorderStyle = BorderStyle.None;
            WidthLabel.BackColor = Colors.Origin;
            WidthLabel.Font = new Font("Calibri", 14);
            WidthLabel.ForeColor = Colors.Constant;
            WidthLabel.Text = "       Width : " + Convert.ToString(CustomPictureBox.Width);
            WidthLabel.TextAlign = ContentAlignment.MiddleLeft;
            WidthLabel.Image = new Bitmap(@"..\..\..\downloads\Icons\Width.png");
            WidthLabel.ImageAlign = ContentAlignment.MiddleLeft;

            WidthPlusButton = new Button();
            WidthPlusButton.TabIndex = 0;
            WidthPlusButton.TabStop = false;
            WidthPlusButton.Location = new Point(WidthLabel.Width - WidthLabel.Height * 2, 0);
            WidthPlusButton.Size = new Size(WidthLabel.Height, WidthLabel.Height);
            WidthPlusButton.FlatStyle = FlatStyle.Flat;
            WidthPlusButton.BackColor = Colors.Origin;
            WidthPlusButton.FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
            WidthPlusButton.FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
            WidthPlusButton.FlatAppearance.BorderSize = 0;
            WidthPlusButton.Image = new Bitmap(@"..\..\..\downloads\Icons\Plus.png");
            WidthPlusButton.ImageAlign = ContentAlignment.MiddleCenter;
            WidthPlusButton.MouseClick += WidthPlusButton_MouseClick;

            WidthMinusButton = new Button();
            WidthMinusButton.TabIndex = 0;
            WidthMinusButton.TabStop = false;
            WidthMinusButton.Location = new Point(WidthLabel.Width - WidthLabel.Height, 0);
            WidthMinusButton.Size = new Size(WidthLabel.Height, WidthLabel.Height);
            WidthMinusButton.FlatStyle = FlatStyle.Flat;
            WidthMinusButton.BackColor = Colors.Origin;
            WidthMinusButton.FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
            WidthMinusButton.FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
            WidthMinusButton.FlatAppearance.BorderSize = 0;
            WidthMinusButton.Image = new Bitmap(@"..\..\..\downloads\Icons\Minus.png");
            WidthMinusButton.ImageAlign = ContentAlignment.MiddleCenter;
            WidthMinusButton.MouseClick += WidthMinusButton_MouseClick;

            HeightLabel = new Label();
            HeightLabel.TabIndex = 0;
            HeightLabel.Location = new Point(10, WidthLabel.Bottom + 2);
            HeightLabel.Size = new Size(this.Width - 20, 30);
            HeightLabel.FlatStyle = FlatStyle.Flat;
            HeightLabel.BorderStyle = BorderStyle.None;
            HeightLabel.BackColor = Colors.Origin;
            HeightLabel.Font = new Font("Calibri", 14);
            HeightLabel.ForeColor = Colors.Constant;
            HeightLabel.Text = "       Height : " + Convert.ToString(CustomPictureBox.Height);
            HeightLabel.TextAlign = ContentAlignment.MiddleLeft;
            HeightLabel.Image = new Bitmap(@"..\..\..\downloads\Icons\Height.png");
            HeightLabel.ImageAlign = ContentAlignment.MiddleLeft;

            HeightPlusButton = new Button();
            HeightPlusButton.TabIndex = 0;
            HeightPlusButton.TabStop = false;
            HeightPlusButton.Location = new Point(HeightLabel.Width - HeightLabel.Height * 2, 0);
            HeightPlusButton.Size = new Size(HeightLabel.Height, HeightLabel.Height);
            HeightPlusButton.FlatStyle = FlatStyle.Flat;
            HeightPlusButton.BackColor = Colors.Origin;
            HeightPlusButton.FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
            HeightPlusButton.FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
            HeightPlusButton.FlatAppearance.BorderSize = 0;
            HeightPlusButton.Image = new Bitmap(@"..\..\..\downloads\Icons\Plus.png");
            HeightPlusButton.ImageAlign = ContentAlignment.MiddleCenter;
            HeightPlusButton.MouseClick += HeightPlusButton_MouseClick;

            HeightMinusButton = new Button();
            HeightMinusButton.TabIndex = 0;
            HeightMinusButton.TabStop = false;
            HeightMinusButton.Location = new Point(HeightLabel.Width - HeightLabel.Height, 0);
            HeightMinusButton.Size = new Size(HeightLabel.Height, HeightLabel.Height);
            HeightMinusButton.FlatStyle = FlatStyle.Flat;
            HeightMinusButton.BackColor = Colors.Origin;
            HeightMinusButton.FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
            HeightMinusButton.FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
            HeightMinusButton.FlatAppearance.BorderSize = 0;
            HeightMinusButton.Image = new Bitmap(@"..\..\..\downloads\Icons\Minus.png");
            HeightMinusButton.ImageAlign = ContentAlignment.MiddleCenter;
            HeightMinusButton.MouseClick += HeightMinusButton_MouseClick;

            CoordinateXLabel.Controls.Add(CoordinateXPlusButton);
            CoordinateXLabel.Controls.Add(CoordinateXMinusButton);

            CoordinateYLabel.Controls.Add(CoordinateYPlusButton);
            CoordinateYLabel.Controls.Add(CoordinateYMinusButton);

            WidthLabel.Controls.Add(WidthPlusButton);
            WidthLabel.Controls.Add(WidthMinusButton);

            HeightLabel.Controls.Add(HeightPlusButton);
            HeightLabel.Controls.Add(HeightMinusButton);
            
            ContentPanel.Controls.Add(CoordinateXLabel);
            ContentPanel.Controls.Add(CoordinateYLabel);
            ContentPanel.Controls.Add(WidthLabel);
            ContentPanel.Controls.Add(HeightLabel);
        }

        private void LocationXPlusButton_MouseClick(object sender, MouseEventArgs e)
        {
            CustomPictureBox.Left++;
            CoordinateXLabel.Text = "       X coordinate : " + Convert.ToString(CustomPictureBox.Left);
        }

        private void LocationXMinusButton_MouseClick(object sender, MouseEventArgs e)
        {
            CustomPictureBox.Left--;
            CoordinateXLabel.Text = "       X coordinate : " + Convert.ToString(CustomPictureBox.Left);
        }

        private void LocationYPlusButton_MouseClick(object sender, MouseEventArgs e)
        {
            CustomPictureBox.Top++;
            CoordinateYLabel.Text = "       Y coordinate : " + Convert.ToString(CustomPictureBox.Top);
        }

        private void LocationYMinusButton_MouseClick(object sender, MouseEventArgs e)
        {
            CustomPictureBox.Top--;
            CoordinateYLabel.Text = "       Y coordinate : " + Convert.ToString(CustomPictureBox.Top);
        }

        private void WidthPlusButton_MouseClick(object sender, MouseEventArgs e)
        {
            CustomPictureBox.Width++;
            WidthLabel.Text = "       Width : " + Convert.ToString(CustomPictureBox.Width);
        }

        private void WidthMinusButton_MouseClick(object sender, MouseEventArgs e)
        {
            CustomPictureBox.Width--;
            WidthLabel.Text = "       Width : " + Convert.ToString(CustomPictureBox.Width);
        }

        private void HeightPlusButton_MouseClick(object sender, MouseEventArgs e)
        {
            CustomPictureBox.Height++;
            HeightLabel.Text = "       Height : " + Convert.ToString(CustomPictureBox.Height);
        }

        private void HeightMinusButton_MouseClick(object sender, MouseEventArgs e)
        {
            CustomPictureBox.Height--;
            HeightLabel.Text = "       Height : " + Convert.ToString(CustomPictureBox.Height);
        }

        private void CheckForOutsideChanges()
        {
            while (true)
            {
                if (CustomPictureBox.Left != CustomPictureBoxLeft)
                {
                    CustomPictureBoxLeft = CustomPictureBox.Left;
                    CoordinateXLabel.Invoke((MethodInvoker)(delegate { CoordinateXLabel.Text = "       X coordinate : " + Convert.ToString(CustomPictureBox.Left); }));
                }
                if (CustomPictureBox.Top != CustomPictureBoxTop)
                {
                    CustomPictureBoxTop = CustomPictureBox.Top;
                    CoordinateYLabel.Invoke((MethodInvoker)(delegate { CoordinateYLabel.Text = "       Y coordinate : " + Convert.ToString(CustomPictureBox.Top); }));
                }
                if (CustomPictureBoxWidth != CustomPictureBox.Width)
                {
                    CustomPictureBoxWidth = CustomPictureBox.Width;
                    WidthLabel.Invoke((MethodInvoker)(delegate { WidthLabel.Text = "       Width : " + Convert.ToString(CustomPictureBox.Width); }));
                }
                if (CustomPictureBoxHeight != CustomPictureBox.Height)
                {
                    CustomPictureBoxHeight = CustomPictureBox.Height;
                    HeightLabel.Invoke((MethodInvoker)(delegate { HeightLabel.Text = "       Height : " + Convert.ToString(CustomPictureBox.Height); }));
                }
                Thread.Sleep(1);
            }
        }
    }
}