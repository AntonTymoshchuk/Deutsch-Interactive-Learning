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
    public partial class CustomLabelForm : Form
    {
        private ViewControls.FormControls.Border Border;
        private ViewControls.FormControls.Header Header;
        private FlatDesign.Colors Colors;
        private Panel ContentPanel;

        private Label FontSizeLabel;
        private Button FontSizePlusButton;
        private Button FontSizeMinusButton;
        private Button[] FontSizeButtons;

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
        private int CustomLabelLeft, CustomLabelTop;
        private int CustomLabelWidth, CustomLabelHeight;

        private Label CustomLabel;
        private Containers.Editor Editor;

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

        public CustomLabelForm(Label CustomLabel, Containers.Editor Editor)
        {
            this.CustomLabel = CustomLabel;
            this.Editor = Editor;

            CustomLabelLeft = CustomLabel.Left;
            CustomLabelTop = CustomLabel.Top;
            CustomLabelWidth = CustomLabel.Width;
            CustomLabelHeight = CustomLabel.Height;

            InitializeComponent();
            this.Name = "CustomLabelForm";
            this.Text = CustomLabel.Name + " settings";
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(322, 274);
            this.FormClosing += CustomLabelForm_FormClosing;

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

        private void CustomLabelForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            OutsideChangeChecker.Abort();
        }

        private void InitializeControls()
        {
            FontSizeLabel = new Label();
            FontSizeLabel.TabIndex = 0;
            FontSizeLabel.Location = new Point(10, 10);
            FontSizeLabel.Size = new Size(this.Width - 20, 30);
            FontSizeLabel.FlatStyle = FlatStyle.Flat;
            FontSizeLabel.BorderStyle = BorderStyle.None;
            FontSizeLabel.BackColor = Colors.Origin;
            FontSizeLabel.Font = new Font("Calibri", 14);
            FontSizeLabel.ForeColor = Colors.Constant;
            FontSizeLabel.Text = "       Font size : " + Convert.ToString(CustomLabel.Font.Size);
            FontSizeLabel.TextAlign = ContentAlignment.MiddleLeft;
            FontSizeLabel.Image = new Bitmap(@"..\..\..\downloads\Icons\FontSize.png");
            FontSizeLabel.ImageAlign = ContentAlignment.MiddleLeft;

            FontSizePlusButton = new Button();
            FontSizePlusButton.TabIndex = 0;
            FontSizePlusButton.TabStop = false;
            FontSizePlusButton.Location = new Point(FontSizeLabel.Width - FontSizeLabel.Height * 2, 0);
            FontSizePlusButton.Size = new Size(FontSizeLabel.Height, FontSizeLabel.Height);
            FontSizePlusButton.FlatStyle = FlatStyle.Flat;
            FontSizePlusButton.BackColor = Colors.Origin;
            FontSizePlusButton.FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
            FontSizePlusButton.FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
            FontSizePlusButton.FlatAppearance.BorderSize = 0;
            FontSizePlusButton.Image = new Bitmap(@"..\..\..\downloads\Icons\Plus.png");
            FontSizePlusButton.ImageAlign = ContentAlignment.MiddleCenter;
            FontSizePlusButton.MouseClick += FontSizePlusButton_MouseClick;

            FontSizeMinusButton = new Button();
            FontSizeMinusButton.TabIndex = 0;
            FontSizeMinusButton.TabStop = false;
            FontSizeMinusButton.Location = new Point(FontSizeLabel.Width - FontSizeLabel.Height, 0);
            FontSizeMinusButton.Size = new Size(FontSizeLabel.Height, FontSizeLabel.Height);
            FontSizeMinusButton.FlatStyle = FlatStyle.Flat;
            FontSizeMinusButton.BackColor = Colors.Origin;
            FontSizeMinusButton.FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
            FontSizeMinusButton.FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
            FontSizeMinusButton.FlatAppearance.BorderSize = 0;
            FontSizeMinusButton.Image = new Bitmap(@"..\..\..\downloads\Icons\Minus.png");
            FontSizeMinusButton.ImageAlign = ContentAlignment.MiddleCenter;
            FontSizeMinusButton.MouseClick += FontSizeMinusButton_MouseClick;

            int FontSize = 0;
            FontSizeButtons = new Button[8];
            for (int i = 0; i < FontSizeButtons.Length; i++)
            {
                FontSizeButtons[i] = new Button();
                FontSizeButtons[i].TabIndex = 0;
                FontSizeButtons[i].TabStop = false;
                if (i == 0)
                    FontSizeButtons[i].Location = new Point(10, FontSizeLabel.Bottom + 2);
                else
                    FontSizeButtons[i].Location = new Point(FontSizeButtons[i - 1].Right, FontSizeLabel.Bottom + 2);
                FontSizeButtons[i].Size = new Size(30, 30);
                FontSizeButtons[i].FlatStyle = FlatStyle.Flat;
                FontSizeButtons[i].BackColor = Colors.Origin;
                FontSizeButtons[i].FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
                FontSizeButtons[i].FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
                FontSizeButtons[i].FlatAppearance.BorderSize = 0;
                FontSizeButtons[i].Font = new Font("Calibri", 10);
                FontSizeButtons[i].ForeColor = Colors.Constant;
                FontSizeButtons[i].Text = Convert.ToString(FontSize += 12);
                FontSizeButtons[i].TextAlign = ContentAlignment.MiddleCenter;
                FontSizeButtons[i].MouseClick += FontSizeButton_MouseClick;
            }

            CoordinateXLabel = new Label();
            CoordinateXLabel.TabIndex = 0;
            CoordinateXLabel.Location = new Point(10, FontSizeLabel.Bottom + 34);
            CoordinateXLabel.Size = new Size(this.Width - 20, 30);
            CoordinateXLabel.FlatStyle = FlatStyle.Flat;
            CoordinateXLabel.BorderStyle = BorderStyle.None;
            CoordinateXLabel.BackColor = Colors.Origin;
            CoordinateXLabel.Font = new Font("Calibri", 14);
            CoordinateXLabel.ForeColor = Colors.Constant;
            CoordinateXLabel.Text = "       X coordinate : " + Convert.ToString(CustomLabel.Left);
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
            CoordinateYLabel.Text = "       Y coordinate : " + Convert.ToString(CustomLabel.Top);
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
            WidthLabel.Text = "       Width : " + Convert.ToString(CustomLabel.Width);
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
            HeightLabel.Text = "       Height : " + Convert.ToString(CustomLabel.Height);
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

            DeleteButton = new Button();
            DeleteButton.TabIndex = 0;
            DeleteButton.TabStop = false;
            DeleteButton.Location = new Point(10, HeightLabel.Bottom + 2);
            DeleteButton.Size = new Size(150, 30);
            DeleteButton.FlatStyle = FlatStyle.Flat;
            DeleteButton.BackColor = Colors.Origin;
            DeleteButton.FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
            DeleteButton.FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
            DeleteButton.FlatAppearance.BorderSize = 0;
            DeleteButton.Font = new Font("Calibri", 12);
            DeleteButton.ForeColor = Colors.Constant;
            DeleteButton.Text = "Delete";
            DeleteButton.TextAlign = ContentAlignment.MiddleCenter;
            DeleteButton.MouseClick += DeleteButton_MouseClick;

            ResetButton = new Button();
            ResetButton.TabIndex = 0;
            ResetButton.TabStop = false;
            ResetButton.Location = new Point(DeleteButton.Right, HeightLabel.Bottom + 2);
            ResetButton.Size = new Size(150, 30);
            ResetButton.FlatStyle = FlatStyle.Flat;
            ResetButton.BackColor = Colors.Origin;
            ResetButton.FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
            ResetButton.FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
            ResetButton.FlatAppearance.BorderSize = 0;
            ResetButton.Font = new Font("Calibri", 12);
            ResetButton.ForeColor = Colors.Constant;
            ResetButton.Text = "Reset";
            ResetButton.TextAlign = ContentAlignment.MiddleCenter;
            ResetButton.MouseClick += ResetButton_MouseClick;

            FontSizeLabel.Controls.Add(FontSizePlusButton);
            FontSizeLabel.Controls.Add(FontSizeMinusButton);

            CoordinateXLabel.Controls.Add(CoordinateXPlusButton);
            CoordinateXLabel.Controls.Add(CoordinateXMinusButton);

            CoordinateYLabel.Controls.Add(CoordinateYPlusButton);
            CoordinateYLabel.Controls.Add(CoordinateYMinusButton);

            WidthLabel.Controls.Add(WidthPlusButton);
            WidthLabel.Controls.Add(WidthMinusButton);

            HeightLabel.Controls.Add(HeightPlusButton);
            HeightLabel.Controls.Add(HeightMinusButton);

            ContentPanel.Controls.Add(FontSizeLabel);
            for (int i = 0; i < FontSizeButtons.Length; i++)
                ContentPanel.Controls.Add(FontSizeButtons[i]);
            ContentPanel.Controls.Add(CoordinateXLabel);
            ContentPanel.Controls.Add(CoordinateYLabel);
            ContentPanel.Controls.Add(WidthLabel);
            ContentPanel.Controls.Add(HeightLabel);
            ContentPanel.Controls.Add(DeleteButton);
            ContentPanel.Controls.Add(ResetButton);
        }
        
        private void FontSizePlusButton_MouseClick(object sender, MouseEventArgs e)
        {
            CustomLabel.Font = new Font("Calibri", CustomLabel.Font.Size + 1);
            FontSizeLabel.Text = "       Font size : " + Convert.ToString(CustomLabel.Font.Size);
        }

        private void FontSizeMinusButton_MouseClick(object sender, MouseEventArgs e)
        {
            CustomLabel.Font = new Font("Calibri", CustomLabel.Font.Size - 1);
            FontSizeLabel.Text = "       Font size : " + Convert.ToString(CustomLabel.Font.Size);
        }

        private void FontSizeButton_MouseClick(object sender, MouseEventArgs e)
        {
            Button FontSizeButton = sender as Button;
            CustomLabel.Font = new Font("Calibri", Convert.ToInt32(FontSizeButton.Text));
            FontSizeLabel.Text = "       Font size : " + Convert.ToString(CustomLabel.Font.Size);
        }

        private void LocationXPlusButton_MouseClick(object sender, MouseEventArgs e)
        {
            CustomLabel.Left++;
            CoordinateXLabel.Text = "       X coordinate : " + Convert.ToString(CustomLabel.Left);
        }

        private void LocationXMinusButton_MouseClick(object sender, MouseEventArgs e)
        {
            CustomLabel.Left--;
            CoordinateXLabel.Text = "       X coordinate : " + Convert.ToString(CustomLabel.Left);
        }

        private void LocationYPlusButton_MouseClick(object sender, MouseEventArgs e)
        {
            CustomLabel.Top++;
            CoordinateYLabel.Text = "       Y coordinate : " + Convert.ToString(CustomLabel.Top);
        }

        private void LocationYMinusButton_MouseClick(object sender, MouseEventArgs e)
        {
            CustomLabel.Top--;
            CoordinateYLabel.Text = "       Y coordinate : " + Convert.ToString(CustomLabel.Top);
        }

        private void WidthPlusButton_MouseClick(object sender, MouseEventArgs e)
        {
            CustomLabel.Width++;
            WidthLabel.Text = "       Width : " + Convert.ToString(CustomLabel.Width);
        }

        private void WidthMinusButton_MouseClick(object sender, MouseEventArgs e)
        {
            CustomLabel.Width--;
            WidthLabel.Text = "       Width : " + Convert.ToString(CustomLabel.Width);
        }

        private void HeightPlusButton_MouseClick(object sender, MouseEventArgs e)
        {
            CustomLabel.Height++;
            HeightLabel.Text = "       Height : " + Convert.ToString(CustomLabel.Height);
        }

        private void HeightMinusButton_MouseClick(object sender, MouseEventArgs e)
        {
            CustomLabel.Height--;
            HeightLabel.Text = "       Height : " + Convert.ToString(CustomLabel.Height);
        }

        private void DeleteButton_MouseClick(object sender, MouseEventArgs e)
        {
            CustomLabel.Dispose();
            Editor.RemoveFromEditor(CustomLabel);
            this.Close();
        }

        private void ResetButton_MouseClick(object sender, MouseEventArgs e)
        {
            CustomLabel.Location = new Point(5, 5);
            CustomLabel.Size = new Size(150, 50);
            CustomLabel.Font = new Font("Calibri", 16);
        }

        private void CheckForOutsideChanges()
        {
            while (true)
            {
                if (CustomLabel.Left != CustomLabelLeft)
                {
                    CustomLabelLeft = CustomLabel.Left;
                    CoordinateXLabel.Invoke((MethodInvoker)(delegate { CoordinateXLabel.Text = "       X coordinate : " + Convert.ToString(CustomLabel.Left); }));
                }
                if (CustomLabel.Top != CustomLabelTop)
                {
                    CustomLabelTop = CustomLabel.Top;
                    CoordinateYLabel.Invoke((MethodInvoker)(delegate { CoordinateYLabel.Text = "       Y coordinate : " + Convert.ToString(CustomLabel.Top); }));
                }
                if (CustomLabelWidth != CustomLabel.Width)
                {
                    CustomLabelWidth = CustomLabel.Width;
                    WidthLabel.Invoke((MethodInvoker)(delegate { WidthLabel.Text = "       Width : " + Convert.ToString(CustomLabel.Width); }));
                }
                if (CustomLabelHeight != CustomLabel.Height)
                {
                    CustomLabelHeight = CustomLabel.Height;
                    HeightLabel.Invoke((MethodInvoker)(delegate { HeightLabel.Text = "       Height : " + Convert.ToString(CustomLabel.Height); }));
                }
                Thread.Sleep(1);
            }
        }
    }
}
