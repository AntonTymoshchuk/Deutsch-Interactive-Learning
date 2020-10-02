using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Deutsch_Interactive_Learning.ViewControls.FormControls
{
    public class Header
    {
        private Panel GroundPanel;
        private PictureBox ImagePictureBox;
        private Label CaptionLabel;
        private Button MinimizeButton;
        private Button CloseButton;

        private bool DragStarted = false;
        private int DragStartX, DragStartY;

        private Form ParentForm;

        public Header(Form ParentForm, FlatDesign.Colors Colors)
        {
            this.ParentForm = ParentForm;

            GroundPanel = new Panel();
            GroundPanel.TabIndex = 0;
            GroundPanel.TabStop = false;
            GroundPanel.Location = new Point(1, 1);
            GroundPanel.Size = new Size(ParentForm.Width - 2, 30);
            GroundPanel.BorderStyle = BorderStyle.None;
            GroundPanel.BackColor = Colors.Origin;
            GroundPanel.MouseDown += Control_MouseDown;
            GroundPanel.MouseMove += Control_MouseMove;
            GroundPanel.MouseUp += Control_MouseUp;

            ImagePictureBox = new PictureBox();
            ImagePictureBox.Location = new Point(3, 0);
            ImagePictureBox.Size = new Size(GroundPanel.Height, GroundPanel.Height);
            ImagePictureBox.BorderStyle = BorderStyle.None;
            ImagePictureBox.BackColor = GroundPanel.BackColor;
            ImagePictureBox.Image = ParentForm.Icon.ToBitmap();
            ImagePictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
            ImagePictureBox.Visible = false;
            ImagePictureBox.MouseDown += Control_MouseDown;
            ImagePictureBox.MouseMove += Control_MouseMove;
            ImagePictureBox.MouseUp += Control_MouseUp;

            CaptionLabel = new Label();
            CaptionLabel.TabIndex = 0;
            CaptionLabel.Location = new Point(4, 4);
            CaptionLabel.AutoSize = true;
            CaptionLabel.FlatStyle = FlatStyle.Flat;
            CaptionLabel.BorderStyle = BorderStyle.None;
            CaptionLabel.BackColor = GroundPanel.BackColor;
            CaptionLabel.Font = new Font("Calibri", 12);
            CaptionLabel.ForeColor = Colors.Constant;
            CaptionLabel.Text = ParentForm.Text;
            CaptionLabel.TextAlign = ContentAlignment.MiddleLeft;
            CaptionLabel.MouseDown += Control_MouseDown;
            CaptionLabel.MouseMove += Control_MouseMove;
            CaptionLabel.MouseUp += Control_MouseUp;

            MinimizeButton = new Button();
            MinimizeButton.TabIndex = 0;
            MinimizeButton.TabStop = false;
            MinimizeButton.Location = new Point(GroundPanel.Width - GroundPanel.Height * 2, 0);
            MinimizeButton.Size = new Size(GroundPanel.Height, GroundPanel.Height);
            MinimizeButton.FlatStyle = FlatStyle.Flat;
            MinimizeButton.BackColor = GroundPanel.BackColor;
            MinimizeButton.FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
            MinimizeButton.FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
            MinimizeButton.FlatAppearance.BorderSize = 0;
            MinimizeButton.Image = new Bitmap(@"..\..\..\downloads\Icons\Minimize.png");
            MinimizeButton.ImageAlign = ContentAlignment.MiddleCenter;
            MinimizeButton.MouseClick += MinimizeButton_MouseClick;

            CloseButton = new Button();
            CloseButton.TabIndex = 0;
            CloseButton.TabStop = false;
            CloseButton.Location = new Point(GroundPanel.Width - GroundPanel.Height, 0);
            CloseButton.Size = new Size(GroundPanel.Height, GroundPanel.Height);
            CloseButton.FlatStyle = FlatStyle.Flat;
            CloseButton.BackColor = GroundPanel.BackColor;
            CloseButton.FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
            CloseButton.FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
            CloseButton.FlatAppearance.BorderSize = 0;
            CloseButton.Image = new Bitmap(@"..\..\..\downloads\Icons\Close.png");
            CloseButton.ImageAlign = ContentAlignment.MiddleCenter;
            CloseButton.MouseClick += CloseButton_MouseClick;

            GroundPanel.Controls.Add(ImagePictureBox);
            GroundPanel.Controls.Add(CaptionLabel);
            GroundPanel.Controls.Add(MinimizeButton);
            GroundPanel.Controls.Add(CloseButton);

            ParentForm.Controls.Add(GroundPanel);
        }
        
        private void Control_MouseDown(object sender, MouseEventArgs e)
        {
            DragStarted = true;
            DragStartX = e.X;
            DragStartY = e.Y;
        }

        private void Control_MouseMove(object sender, MouseEventArgs e)
        {
            if (DragStarted == true)
            {
                ParentForm.Left = ParentForm.Left + e.X - DragStartX;
                ParentForm.Top = ParentForm.Top + e.Y - DragStartY;
            }
        }

        private void Control_MouseUp(object sender, MouseEventArgs e)
        {
            DragStarted = false;
        }

        private void MinimizeButton_MouseClick(object sender, MouseEventArgs e)
        {
            ParentForm.WindowState = FormWindowState.Minimized;
        }

        private void CloseButton_MouseClick(object sender, MouseEventArgs e)
        {
            ParentForm.Close();
        }
    }
}
