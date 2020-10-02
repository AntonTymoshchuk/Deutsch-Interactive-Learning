using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Threading;

namespace Deutsch_Interactive_Learning.Containers
{
    public class Editor : ViewControls.ContainerControls.Container
    {
        private Label NameLabel;
        private TextBox NameTextBox;
        private Label OverviewLabel;
        private TextBox OverviewTextBox;
        private Button ContinueButton;

        private int CustomLabelCount = 0;
        private int CustomTextBoxCount = 0;
        private int CustomPictureBoxCount = 0;

        private string ProjectName;
        private string ProjectOverview;
        private List<Control> CustomControls;

        private bool DragStarted = false;
        private int DragStartX, DragStartY;

        private bool ResizeStarted = false;
        private int ResizeStartX, ResizeStartY;

        private Panel InstrumentsPanel;
        private Button AddCustomLabelButton;
        private Button AddCustomTextBoxButton;
        private Button AddCustomPictureBoxButton;

        private Panel PageGroundPanel;
        private Button NextPageButton;
        private Button PreviusPageButton;

        private Form ParentForm;
        private FlatDesign.Colors Colors;

        public Editor(Form ParentForm, FlatDesign.Colors Colors)
        {
            this.ParentForm = ParentForm;
            this.Colors = Colors;

            CreateContainer(
                ParentForm,
                Colors,
                "Editor",
                "",
                null,
                "Editor",
                null,
                "Creating");

            CustomControls = new List<Control>();

            NameLabel = new Label();
            NameLabel.TabIndex = 0;
            NameLabel.Location = new Point(10, 10);
            NameLabel.AutoSize = true;
            NameLabel.Width = 200;
            NameLabel.FlatStyle = FlatStyle.Flat;
            NameLabel.BorderStyle = BorderStyle.None;
            NameLabel.BackColor = Colors.Background;
            NameLabel.Font = new Font("Calibri", 24);
            NameLabel.ForeColor = Colors.Constant;
            NameLabel.Text = "Name : ";
            NameLabel.TextAlign = ContentAlignment.MiddleLeft;

            NameTextBox = new TextBox();
            NameTextBox.TabIndex = 0;
            NameTextBox.TabStop = false;
            NameTextBox.Location = new Point(NameLabel.Right, 10);
            NameTextBox.Width = ContainerSize.Width - NameLabel.Width - 20;
            NameTextBox.BorderStyle = BorderStyle.None;
            NameTextBox.BackColor = Color.White;
            NameTextBox.Font = new Font("Calibri", 24);
            NameTextBox.ForeColor = Colors.Background;
            NameTextBox.TextAlign = HorizontalAlignment.Left;

            OverviewLabel = new Label();
            OverviewLabel.TabIndex = 0;
            OverviewLabel.Location = new Point(10, 65);
            OverviewLabel.AutoSize = true;
            OverviewLabel.Width = 200;
            OverviewLabel.FlatStyle = FlatStyle.Flat;
            OverviewLabel.BorderStyle = BorderStyle.None;
            OverviewLabel.BackColor = Colors.Background;
            OverviewLabel.Font = new Font("Calibri", 24);
            OverviewLabel.ForeColor = Colors.Constant;
            OverviewLabel.Text = "Overview : ";
            OverviewLabel.TextAlign = ContentAlignment.MiddleLeft;

            OverviewTextBox = new TextBox();
            OverviewTextBox.TabIndex = 0;
            OverviewTextBox.TabStop = false;
            OverviewTextBox.Location = new Point(10, 110);
            OverviewTextBox.Size = new Size(ContainerSize.Width - 20, 300);
            OverviewTextBox.Multiline = true;
            OverviewTextBox.BorderStyle = BorderStyle.None;
            OverviewTextBox.BackColor = Color.White;
            OverviewTextBox.Font = new Font("Calibri", 24);
            OverviewTextBox.ForeColor = Colors.Background;
            OverviewTextBox.TextAlign = HorizontalAlignment.Left;

            ContinueButton = new Button();
            ContinueButton.TabIndex = 0;
            ContinueButton.TabStop = false;
            ContinueButton.Size = new Size(300, 70);
            ContinueButton.Location = new Point(ContainerSize.Width / 2 - ContinueButton.Width / 2, OverviewTextBox.Bottom + 50);
            ContinueButton.FlatStyle = FlatStyle.Flat;
            ContinueButton.BackColor = Colors.Origin;
            ContinueButton.FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
            ContinueButton.FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
            ContinueButton.FlatAppearance.BorderSize = 0;
            ContinueButton.Font = new Font("Calibri", 24);
            ContinueButton.ForeColor = Colors.Constant;
            ContinueButton.Text = "Continue";
            ContinueButton.TextAlign = ContentAlignment.MiddleCenter;
            ContinueButton.MouseClick += ContinueButton_MouseClick;

            InstrumentsPanel = new Panel();
            InstrumentsPanel.TabIndex = 0;
            InstrumentsPanel.TabStop = false;
            InstrumentsPanel.Location = new Point(ContainerSize.Width / 2 - 75, 10);
            InstrumentsPanel.Size = new Size(150, 50);
            InstrumentsPanel.BorderStyle = BorderStyle.None;
            InstrumentsPanel.BackColor = Colors.Background;
            InstrumentsPanel.Visible = false;

            AddCustomLabelButton = new Button();
            AddCustomLabelButton.TabIndex = 0;
            AddCustomLabelButton.TabStop = false;
            AddCustomLabelButton.Location = new Point(0, 0);
            AddCustomLabelButton.Size = new Size(50, 50);
            AddCustomLabelButton.FlatStyle = FlatStyle.Flat;
            AddCustomLabelButton.BackColor = Colors.Curtain;
            AddCustomLabelButton.FlatAppearance.MouseOverBackColor = Colors.CurtainButtonMouseOver;
            AddCustomLabelButton.FlatAppearance.MouseDownBackColor = Colors.CurtainButtonMouseDown;
            AddCustomLabelButton.FlatAppearance.BorderSize = 0;
            AddCustomLabelButton.Image = new Bitmap(@"..\..\..\downloads\Icons\AddText.png");
            AddCustomLabelButton.ImageAlign = ContentAlignment.MiddleCenter;
            AddCustomLabelButton.MouseClick += AddCustomLabelButton_MouseClick;

            AddCustomTextBoxButton = new Button();
            AddCustomTextBoxButton.TabIndex = 0;
            AddCustomTextBoxButton.TabStop = false;
            AddCustomTextBoxButton.Location = new Point(50, 0);
            AddCustomTextBoxButton.Size = new Size(50, 50);
            AddCustomTextBoxButton.FlatStyle = FlatStyle.Flat;
            AddCustomTextBoxButton.BackColor = Colors.Curtain;
            AddCustomTextBoxButton.FlatAppearance.MouseOverBackColor = Colors.CurtainButtonMouseOver;
            AddCustomTextBoxButton.FlatAppearance.MouseDownBackColor = Colors.CurtainButtonMouseDown;
            AddCustomTextBoxButton.FlatAppearance.BorderSize = 0;
            AddCustomTextBoxButton.Image = new Bitmap(@"..\..\..\downloads\Icons\AddInput.png");
            AddCustomTextBoxButton.ImageAlign = ContentAlignment.MiddleCenter;
            AddCustomTextBoxButton.MouseClick += AddCustomTextBoxButton_MouseClick;

            AddCustomPictureBoxButton = new Button();
            AddCustomPictureBoxButton.TabIndex = 0;
            AddCustomPictureBoxButton.TabStop = false;
            AddCustomPictureBoxButton.Location = new Point(100, 0);
            AddCustomPictureBoxButton.Size = new Size(50, 50);
            AddCustomPictureBoxButton.FlatStyle = FlatStyle.Flat;
            AddCustomPictureBoxButton.BackColor = Colors.Curtain;
            AddCustomPictureBoxButton.FlatAppearance.MouseOverBackColor = Colors.CurtainButtonMouseOver;
            AddCustomPictureBoxButton.FlatAppearance.MouseDownBackColor = Colors.CurtainButtonMouseDown;
            AddCustomPictureBoxButton.FlatAppearance.BorderSize = 0;
            AddCustomPictureBoxButton.Image = new Bitmap(@"..\..\..\downloads\Icons\AddPicture.png");
            AddCustomPictureBoxButton.ImageAlign = ContentAlignment.MiddleCenter;
            AddCustomPictureBoxButton.MouseClick += AddCustomPictureBoxButton_MouseClick;

            InstrumentsPanel.Controls.Add(AddCustomLabelButton);
            InstrumentsPanel.Controls.Add(AddCustomTextBoxButton);
            InstrumentsPanel.Controls.Add(AddCustomPictureBoxButton);

            PageGroundPanel = new Panel();
            PageGroundPanel.TabIndex = 0;
            PageGroundPanel.TabStop = false;
            PageGroundPanel.Location = new Point(50, 50);
            PageGroundPanel.Size = new Size(ContainerSize.Width - 100, ContainerSize.Height - 100);
            PageGroundPanel.BorderStyle = BorderStyle.None;
            PageGroundPanel.BackColor = Colors.Background;
            PageGroundPanel.Visible = false;

            NextPageButton = new Button();
            NextPageButton.TabIndex = 0;
            NextPageButton.TabStop = false;
            NextPageButton.Location = new Point(PageGroundPanel.Width - 75, PageGroundPanel.Height / 2 - 50);
            NextPageButton.Size = new Size(75, 100);
            NextPageButton.FlatStyle = FlatStyle.Flat;
            NextPageButton.BackColor = Colors.Origin;
            NextPageButton.FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
            NextPageButton.FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
            NextPageButton.FlatAppearance.BorderSize = 0;
            NextPageButton.Image = new Bitmap(@"..\..\..\downloads\Icons\Next.png");
            NextPageButton.ImageAlign = ContentAlignment.MiddleCenter;

            PreviusPageButton = new Button();
            PreviusPageButton.TabIndex = 0;
            PreviusPageButton.TabStop = false;
            PreviusPageButton.Location = new Point(0, PageGroundPanel.Height / 2 - 50);
            PreviusPageButton.Size = new Size(75, 100);
            PreviusPageButton.FlatStyle = FlatStyle.Flat;
            PreviusPageButton.BackColor = Colors.Origin;
            PreviusPageButton.FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
            PreviusPageButton.FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
            PreviusPageButton.FlatAppearance.BorderSize = 0;
            PreviusPageButton.Image = new Bitmap(@"..\..\..\downloads\Icons\Previus.png");
            PreviusPageButton.ImageAlign = ContentAlignment.MiddleCenter;

            PageGroundPanel.Controls.Add(NextPageButton);
            PageGroundPanel.Controls.Add(PreviusPageButton);

            AddControl(NameLabel);
            AddControl(NameTextBox);
            AddControl(OverviewLabel);
            AddControl(OverviewTextBox);
            AddControl(ContinueButton);
            AddControl(InstrumentsPanel);
            AddControl(PageGroundPanel);
        }

        private void ContinueButton_MouseClick(object sender, MouseEventArgs e)
        {
            ProjectName = NameTextBox.Text;
            ProjectOverview = OverviewTextBox.Text;

            NameLabel.Visible = false;
            NameTextBox.Visible = false;
            OverviewLabel.Visible = false;
            OverviewTextBox.Visible = false;

            InstrumentsPanel.Visible = true;
            PageGroundPanel.Visible = true;
        }

        private void AddCustomLabelButton_MouseClick(object sender, MouseEventArgs e)
        {
            CustomLabelCount++;
            Label CustomLabel = new Label();
            CustomLabel.Name = "Text" + Convert.ToString(CustomLabelCount);
            CustomLabel.TabIndex = 0;
            CustomLabel.Location = new Point(5, 5);
            CustomLabel.Size = new Size(150, 50);
            CustomLabel.FlatStyle = FlatStyle.Flat;
            CustomLabel.BorderStyle = BorderStyle.None;
            CustomLabel.BackColor = Colors.Background;
            CustomLabel.Font = new Font("Calibri", 16);
            CustomLabel.ForeColor = Colors.Constant;
            CustomLabel.Text = "Text" + Convert.ToString(CustomLabelCount);
            CustomLabel.TextAlign = ContentAlignment.MiddleCenter;
            CustomLabel.MouseClick += CustomLabel_MouseClick;
            CustomLabel.MouseDown += CustomControl_MouseDown;
            CustomLabel.MouseMove += CustomControl_MouseMove;
            CustomLabel.MouseUp += CustomControl_MouseUp;
            CustomLabel.Resize += CustomControl_Resize;

            SetBorders(CustomLabel);
            CustomControls.Add(CustomLabel);
            PageGroundPanel.Controls.Add(CustomLabel);
        }

        private void CustomLabel_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Label CustomLabel = sender as Label;
                EditorForms.CustomLabelForm CustomLabelForm = new EditorForms.CustomLabelForm(CustomLabel, this);
                CustomLabelForm.Show();
            }
        }

        private void AddCustomTextBoxButton_MouseClick(object sender, MouseEventArgs e)
        {
            CustomTextBoxCount++;
            TextBox CustomTextBox = new TextBox();
            CustomTextBox.Name = "Input" + Convert.ToString(CustomTextBoxCount);
            CustomTextBox.TabIndex = 0;
            CustomTextBox.TabStop = false;
            CustomTextBox.Location = new Point(5, 5);
            CustomTextBox.Size = new Size(150, 50);
            CustomTextBox.BorderStyle = BorderStyle.None;
            CustomTextBox.BackColor = Colors.Origin;
            CustomTextBox.Font = new Font("Calibri", 16);
            CustomTextBox.ForeColor = Colors.Constant;
            CustomTextBox.Text = "Input" + Convert.ToString(CustomTextBoxCount);
            CustomTextBox.TextAlign = HorizontalAlignment.Left;
            CustomTextBox.Multiline = true;
            CustomTextBox.MouseDown += CustomControl_MouseDown;
            CustomTextBox.MouseMove += CustomControl_MouseMove;
            CustomTextBox.MouseUp += CustomControl_MouseUp;
            CustomTextBox.Resize += CustomControl_Resize;

            SetBorders(CustomTextBox);
            CustomControls.Add(CustomTextBox);
            PageGroundPanel.Controls.Add(CustomTextBox);
        }

        private void AddCustomPictureBoxButton_MouseClick(object sender, MouseEventArgs e)
        {
            CustomPictureBoxCount++;
            PictureBox CustomPictureBox = new PictureBox();
            CustomPictureBox.Name = "Image" + Convert.ToString(CustomPictureBoxCount);
            CustomPictureBox.Location = new Point(5, 5);
            CustomPictureBox.Size = new Size(150, 150);
            CustomPictureBox.BorderStyle = BorderStyle.None;
            CustomPictureBox.BackColor = Colors.Background;
            CustomPictureBox.Image = new Bitmap(@"..\..\..\downloads\Icons\Image.png");
            CustomPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            CustomPictureBox.MouseClick += CustomPictureBox_MouseClick;
            CustomPictureBox.MouseDown += CustomControl_MouseDown;
            CustomPictureBox.MouseMove += CustomControl_MouseMove;
            CustomPictureBox.MouseUp += CustomControl_MouseUp;
            CustomPictureBox.Resize += CustomControl_Resize;

            SetBorders(CustomPictureBox);
            CustomControls.Add(CustomPictureBox);
            PageGroundPanel.Controls.Add(CustomPictureBox);
        }

        private void CustomPictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                PictureBox CustomPictureBox = sender as PictureBox;
                EditorForms.CustomPictureBoxForm CustomPictureBoxForm = new EditorForms.CustomPictureBoxForm(CustomPictureBox);
                CustomPictureBoxForm.Show();
            }
        }

        private void CustomControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                DragStarted = true;
                DragStartX = e.X;
                DragStartY = e.Y;

                Control CustomControl = sender as Control;
                CustomControl.BringToFront();
                InstrumentsPanel.BringToFront();
                NextPageButton.BringToFront();
                PreviusPageButton.BringToFront();
                CustomControl.Cursor = Cursors.SizeAll;
            }
        }

        private void CustomControl_MouseMove(object sender, MouseEventArgs e)
        {
            Control CustomControl = sender as Control;
            if (DragStarted == true)
            {
                CustomControl.Left = CustomControl.Left - DragStartX + e.X;
                CustomControl.Top = CustomControl.Top - DragStartY + e.Y;
            }
        }

        private void CustomControl_MouseUp(object sender, MouseEventArgs e)
        {
            DragStarted = false;

            Control CustomControl = sender as Control;
            CustomControl.Cursor = Cursors.Default;
        }

        private void CustomControl_Resize(object sender, EventArgs e)
        {
            Control CustomControl = sender as Control;

            CustomControl.Controls[0].Location = new Point(0, 0);
            CustomControl.Controls[0].Size = new Size(7, 7);

            CustomControl.Controls[1].Location = new Point(0, 7);
            CustomControl.Controls[1].Size = new Size(3, CustomControl.Height - 14);

            CustomControl.Controls[2].Location = new Point(0, CustomControl.Height - 7);
            CustomControl.Controls[2].Size = new Size(7, 7);

            CustomControl.Controls[3].Location = new Point(7, CustomControl.Height - 3);
            CustomControl.Controls[3].Size = new Size(CustomControl.Width - 14, 3);

            CustomControl.Controls[4].Location = new Point(CustomControl.Width - 7, CustomControl.Height - 7);
            CustomControl.Controls[4].Size = new Size(7, 7);

            CustomControl.Controls[5].Location = new Point(CustomControl.Width - 3, 7);
            CustomControl.Controls[5].Size = new Size(3, CustomControl.Height - 14);

            CustomControl.Controls[6].Location = new Point(CustomControl.Width - 7, 0);
            CustomControl.Controls[6].Size = new Size(7, 7);

            CustomControl.Controls[7].Location = new Point(7, 0);
            CustomControl.Controls[7].Size = new Size(CustomControl.Width - 14, 3);
        }

        private void SetBorders(Control CustomControl)
        {
            Panel LeftTopBorderPanel = new Panel();
            LeftTopBorderPanel.TabIndex = 0;
            LeftTopBorderPanel.TabStop = false;
            LeftTopBorderPanel.Location = new Point(0, 0);
            LeftTopBorderPanel.Size = new Size(7, 7);
            LeftTopBorderPanel.BorderStyle = BorderStyle.None;
            LeftTopBorderPanel.BackColor = Colors.Constant;
            LeftTopBorderPanel.Cursor = Cursors.SizeNWSE;
            LeftTopBorderPanel.MouseDown += BorderPanel_MouseDown;
            LeftTopBorderPanel.MouseMove += BorderPanel_MouseMove;
            LeftTopBorderPanel.MouseUp += BorderPanel_MouseUp;

            Panel LeftBorderPanel = new Panel();
            LeftBorderPanel.TabIndex = 0;
            LeftBorderPanel.TabStop = false;
            LeftBorderPanel.Location = new Point(0, 7);
            LeftBorderPanel.Size = new Size(3, CustomControl.Height - 14);
            LeftBorderPanel.BorderStyle = BorderStyle.None;
            LeftBorderPanel.BackColor = Colors.Constant;
            LeftBorderPanel.Cursor = Cursors.SizeWE;
            LeftBorderPanel.MouseDown += BorderPanel_MouseDown;
            LeftBorderPanel.MouseMove += BorderPanel_MouseMove;
            LeftBorderPanel.MouseUp += BorderPanel_MouseUp;

            Panel LeftBottomBorderPanel = new Panel();
            LeftBottomBorderPanel.TabIndex = 0;
            LeftBottomBorderPanel.TabStop = false;
            LeftBottomBorderPanel.Location = new Point(0, CustomControl.Height - 7);
            LeftBottomBorderPanel.Size = new Size(7, 7);
            LeftBottomBorderPanel.BorderStyle = BorderStyle.None;
            LeftBottomBorderPanel.BackColor = Colors.Constant;
            LeftBottomBorderPanel.Cursor = Cursors.SizeNESW;
            LeftBottomBorderPanel.MouseDown += BorderPanel_MouseDown;
            LeftBottomBorderPanel.MouseMove += BorderPanel_MouseMove;
            LeftBottomBorderPanel.MouseUp += BorderPanel_MouseUp;

            Panel BottomBorderPanel = new Panel();
            BottomBorderPanel.TabIndex = 0;
            BottomBorderPanel.TabStop = false;
            BottomBorderPanel.Location = new Point(7, CustomControl.Height - 3);
            BottomBorderPanel.Size = new Size(CustomControl.Width - 14, 3);
            BottomBorderPanel.BorderStyle = BorderStyle.None;
            BottomBorderPanel.BackColor = Colors.Constant;
            BottomBorderPanel.Cursor = Cursors.SizeNS;
            BottomBorderPanel.MouseDown += BorderPanel_MouseDown;
            BottomBorderPanel.MouseMove += BorderPanel_MouseMove;
            BottomBorderPanel.MouseUp += BorderPanel_MouseUp;

            Panel RightBottomBorderPanel = new Panel();
            RightBottomBorderPanel.TabIndex = 0;
            RightBottomBorderPanel.TabStop = false;
            RightBottomBorderPanel.Location = new Point(CustomControl.Width - 7, CustomControl.Height - 7);
            RightBottomBorderPanel.Size = new Size(7, 7);
            RightBottomBorderPanel.BorderStyle = BorderStyle.None;
            RightBottomBorderPanel.BackColor = Colors.Constant;
            RightBottomBorderPanel.Cursor = Cursors.SizeNWSE;
            RightBottomBorderPanel.MouseDown += BorderPanel_MouseDown;
            RightBottomBorderPanel.MouseMove += BorderPanel_MouseMove;
            RightBottomBorderPanel.MouseUp += BorderPanel_MouseUp;

            Panel RightBorderPanel = new Panel();
            RightBorderPanel.TabIndex = 0;
            RightBorderPanel.TabStop = false;
            RightBorderPanel.Location = new Point(CustomControl.Width - 3, 7);
            RightBorderPanel.Size = new Size(3, CustomControl.Height - 14);
            RightBorderPanel.BorderStyle = BorderStyle.None;
            RightBorderPanel.BackColor = Colors.Constant;
            RightBorderPanel.Cursor = Cursors.SizeWE;
            RightBorderPanel.MouseDown += BorderPanel_MouseDown;
            RightBorderPanel.MouseMove += BorderPanel_MouseMove;
            RightBorderPanel.MouseUp += BorderPanel_MouseUp;

            Panel RightTopBorderPanel = new Panel();
            RightTopBorderPanel.TabIndex = 0;
            RightTopBorderPanel.TabStop = false;
            RightTopBorderPanel.Location = new Point(CustomControl.Width - 7, 0);
            RightTopBorderPanel.Size = new Size(7, 7);
            RightTopBorderPanel.BorderStyle = BorderStyle.None;
            RightTopBorderPanel.BackColor = Colors.Constant;
            RightTopBorderPanel.Cursor = Cursors.SizeNESW;
            RightTopBorderPanel.MouseDown += BorderPanel_MouseDown;
            RightTopBorderPanel.MouseMove += BorderPanel_MouseMove;
            RightTopBorderPanel.MouseUp += BorderPanel_MouseUp;

            Panel TopBorderPanel = new Panel();
            TopBorderPanel.TabIndex = 0;
            TopBorderPanel.TabStop = false;
            TopBorderPanel.Location = new Point(7, 0);
            TopBorderPanel.Size = new Size(CustomControl.Width - 14, 3);
            TopBorderPanel.BorderStyle = BorderStyle.None;
            TopBorderPanel.BackColor = Colors.Constant;
            TopBorderPanel.Cursor = Cursors.SizeNS;
            TopBorderPanel.MouseDown += BorderPanel_MouseDown;
            TopBorderPanel.MouseMove += BorderPanel_MouseMove;
            TopBorderPanel.MouseUp += BorderPanel_MouseUp;

            CustomControl.Controls.Add(LeftTopBorderPanel);
            CustomControl.Controls.Add(LeftBorderPanel);
            CustomControl.Controls.Add(LeftBottomBorderPanel);
            CustomControl.Controls.Add(BottomBorderPanel);
            CustomControl.Controls.Add(RightBottomBorderPanel);
            CustomControl.Controls.Add(RightBorderPanel);
            CustomControl.Controls.Add(RightTopBorderPanel);
            CustomControl.Controls.Add(TopBorderPanel);
        }

        private void BorderPanel_MouseDown(object sender, MouseEventArgs e)
        {
            ResizeStarted = true;
            ResizeStartX = e.X;
            ResizeStartY = e.Y;
        }

        private void BorderPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (ResizeStarted == true)
            {
                for (int i = 0; i < CustomControls.Count; i++)
                {
                    if (sender == CustomControls[i].Controls[0])
                    {
                        CustomControls[i].Width += ResizeStartX - e.X;
                        CustomControls[i].Left -= ResizeStartX - e.X;
                        CustomControls[i].Height += ResizeStartY - e.Y;
                        CustomControls[i].Top -= ResizeStartY - e.Y;
                        return;
                    }

                    if (sender == CustomControls[i].Controls[1])
                    {
                        CustomControls[i].Width += ResizeStartX - e.X;
                        CustomControls[i].Left -= ResizeStartX - e.X;
                        return;
                    }

                    if (sender == CustomControls[i].Controls[2])
                    {
                        CustomControls[i].Width += ResizeStartX - e.X;
                        CustomControls[i].Left -= ResizeStartX - e.X;
                        CustomControls[i].Height += e.Y - ResizeStartY;
                        return;
                    }

                    if (sender == CustomControls[i].Controls[3])
                    {
                        CustomControls[i].Height += e.Y - ResizeStartY;
                        return;
                    }

                    if (sender == CustomControls[i].Controls[4])
                    {
                        CustomControls[i].Width += e.X - ResizeStartX;
                        CustomControls[i].Height += e.Y - ResizeStartY;
                        return;
                    }

                    if (sender == CustomControls[i].Controls[5])
                    {
                        CustomControls[i].Width += e.X - ResizeStartX;
                        return;
                    }

                    if (sender == CustomControls[i].Controls[6])
                    {
                        CustomControls[i].Width += e.X - ResizeStartX;
                        CustomControls[i].Height += ResizeStartY - e.Y;
                        CustomControls[i].Top -= ResizeStartY - e.Y;
                        return;
                    }

                    if (sender == CustomControls[i].Controls[7])
                    {
                        CustomControls[i].Height += ResizeStartY - e.Y;
                        CustomControls[i].Top -= ResizeStartY - e.Y;
                        return;
                    }
                }
            }
        }

        private void BorderPanel_MouseUp(object sender, MouseEventArgs e)
        {
            ResizeStarted = false;
        }

        public void RemoveFromEditor(Control CustomControl)
        {
            for (int i = 0; i < CustomControls.Count; i++)
            {
                if (CustomControls[i] == CustomControl)
                {
                    CustomControls.Remove(CustomControl);
                    break;
                }
            }
        }
    }
}
