using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Deutsch_Interactive_Learning.ViewControls.ContainerControls
{
    public class Pointer
    {
        private Button PointerButton;
        private PictureBox ImagePictureBox;

        private Container TargetContainer;
        private FlatDesign.Colors Colors;

        public Button Button
        {
            get { return PointerButton; }
        }

        public Pointer(Container TargetContainer, FlatDesign.Colors Colors)
        {
            this.TargetContainer = TargetContainer;
            this.Colors = Colors;

            PointerButton = new Button();
            PointerButton.TabIndex = 0;
            PointerButton.TabStop = false;
            PointerButton.Left = 0;
            PointerButton.Height = 58;
            PointerButton.FlatStyle = FlatStyle.Flat;
            PointerButton.BackColor = Colors.Origin;
            PointerButton.FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
            PointerButton.FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
            PointerButton.FlatAppearance.BorderSize = 0;
            PointerButton.Font = new Font("Calibri", 24);
            PointerButton.ForeColor = Colors.Constant;
            PointerButton.Text = "        " + TargetContainer.Name;
            PointerButton.TextAlign = ContentAlignment.MiddleLeft;
            PointerButton.MouseClick += PointerButton_MouseClick;

            ImagePictureBox = new PictureBox();
            ImagePictureBox.Location = new Point(0, 0);
            ImagePictureBox.Size = new Size(PointerButton.Height, PointerButton.Height);
            ImagePictureBox.BorderStyle = BorderStyle.None;
            ImagePictureBox.BackColor = Color.Transparent;
            ImagePictureBox.Image = TargetContainer.Image;
            ImagePictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
            ImagePictureBox.MouseEnter += ImagePictureBox_MouseEnter;
            ImagePictureBox.MouseDown += ImagePictureBox_MouseDown;
            ImagePictureBox.MouseUp += PointerButton_MouseClick;
            ImagePictureBox.MouseLeave += ImagePictureBox_MouseLeave;
            PointerButton.Controls.Add(ImagePictureBox);
        }

        private void PointerButton_MouseClick(object sender, MouseEventArgs e)
        {
            TargetContainer.Show();
        }

        private void ImagePictureBox_MouseEnter(object sender, EventArgs e)
        {
            ImagePictureBox.BackColor = Colors.OriginButtonMouseOver;
            PointerButton.BackColor = Colors.OriginButtonMouseOver;
        }

        private void ImagePictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            ImagePictureBox.BackColor = Colors.OriginButtonMouseDown;
            PointerButton.BackColor = Colors.OriginButtonMouseDown;
        }

        private void ImagePictureBox_MouseLeave(object sender, EventArgs e)
        {
            ImagePictureBox.BackColor = Color.Transparent;
            PointerButton.BackColor = Colors.Origin;
        }
    }
}
