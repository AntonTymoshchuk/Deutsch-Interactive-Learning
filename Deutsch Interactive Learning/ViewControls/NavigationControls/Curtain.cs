using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;

namespace Deutsch_Interactive_Learning.ViewControls.NavigationControls
{
    public class Curtain
    {
        private Panel GroundPanel;
        private Button HamburgerButton;
        private Label CaptionLabel;
        private Panel SelectorPanel;
        private List<PictureBox> ItemPictureBoxes = new List<PictureBox>();
        private List<Button> ItemButtons = new List<Button>();

        private List<ContainerControls.Container> Containers;

        private const int ItemControlHeight = 50;

        private Thread CurtainEngine;
        private bool CurtainShown = false;

        private Thread SelectorEngine;
        private int CurrentSelectorTop;
        private int TargetSelectorTop;
        private bool SelectorLocked = false;
        
        private List<Stripe> ItemStripes = new List<Stripe>();
        private int CurrentItemStripe = 0;
        private int TargetItemStripe = 0;

        private Form ParentForm;
        private FlatDesign.Colors Colors;

        public Curtain(Form ParentForm, FlatDesign.Colors Colors)
        {
            this.ParentForm = ParentForm;
            this.Colors = Colors;

            GroundPanel = new Panel();
            GroundPanel.TabIndex = 0;
            GroundPanel.TabStop = false;
            GroundPanel.Location = new Point(1, 31);
            GroundPanel.Size = new Size(ItemControlHeight, ParentForm.Height - 32);
            GroundPanel.BorderStyle = BorderStyle.None;
            GroundPanel.BackColor = Colors.Curtain;

            HamburgerButton = new Button();
            HamburgerButton.TabIndex = 0;
            HamburgerButton.TabStop = false;
            HamburgerButton.Location = new Point(0, 0);
            HamburgerButton.Size = new Size(GroundPanel.Width, GroundPanel.Width);
            HamburgerButton.FlatStyle = FlatStyle.Flat;
            HamburgerButton.BackColor = GroundPanel.BackColor;
            HamburgerButton.FlatAppearance.MouseOverBackColor = Colors.CurtainButtonMouseOver;
            HamburgerButton.FlatAppearance.MouseDownBackColor = Colors.CurtainButtonMouseDown;
            HamburgerButton.FlatAppearance.BorderSize = 0;
            HamburgerButton.Image = new Bitmap(@"..\..\..\downloads\Icons\Hamburger.png");
            HamburgerButton.ImageAlign = ContentAlignment.MiddleCenter;
            HamburgerButton.MouseClick += HamburgerButton_MouseClick;

            CaptionLabel = new Label();
            CaptionLabel.TabIndex = 0;
            CaptionLabel.Location = new Point(50, 0);
            CaptionLabel.Size = new Size(300, 50);
            CaptionLabel.FlatStyle = FlatStyle.Flat;
            CaptionLabel.BorderStyle = BorderStyle.None;
            CaptionLabel.BackColor = GroundPanel.BackColor;
            CaptionLabel.Font = new Font("Calibri", 20);
            CaptionLabel.ForeColor = Colors.Constant;
            CaptionLabel.Text = "Menu";
            CaptionLabel.TextAlign = ContentAlignment.MiddleLeft;

            GroundPanel.Controls.Add(HamburgerButton);
            GroundPanel.Controls.Add(CaptionLabel);

            ParentForm.Controls.Add(GroundPanel);
        }

        private void HamburgerButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (CurtainShown == false)
                Show();
            else if (CurtainShown == true)
                Hide();
        }

        private void CurtainShowing()
        {
            CurtainShown = true;
            for (int i = GroundPanel.Width; i < 350; i += 30)
            {
                GroundPanel.Invoke((MethodInvoker)(delegate { GroundPanel.Width += 30; }));
                Thread.Sleep(1);
            }
        }

        public void Show()
        {
            CurtainEngine = new Thread(CurtainShowing);
            CurtainEngine.Start();
        }

        private void CurtainHiding()
        {
            CurtainShown = false;
            for (int i = GroundPanel.Width; i > 50; i -= 30)
            {
                GroundPanel.Invoke((MethodInvoker)(delegate { GroundPanel.Width -= 30; }));
                Thread.Sleep(1);
            }
        }

        public void Hide()
        {
            CurtainEngine = new Thread(CurtainHiding);
            CurtainEngine.Start();
        }

        public void GetItemsFrom(List<ContainerControls.Container> Containers)
        {
            this.Containers = Containers;

            for (int i = 0; i < Containers.Count; i++)
                Containers[i].SetOriginCurtain(this);

            bool Exists = false;
            int Position = 0;

            List<List<ContainerControls.Container>> StripeContainers = new List<List<ContainerControls.Container>>();

            for (int i = 0; i < Containers.Count; i++)
            {
                Exists = false;
                Position = 0;
                for (int j = 0; j < ItemButtons.Count; j++)
                {
                    Position++;
                    if (ItemButtons[j].Text == Containers[i].CurtainItemName)
                    {
                        Exists = true;
                        break;
                    }
                }
                if (Exists == false)
                {
                    ItemPictureBoxes.Add(new PictureBox());
                    ItemPictureBoxes[Position].Location = new Point(0, HamburgerButton.Bottom + Position * GroundPanel.Width);
                    ItemPictureBoxes[Position].Size = new Size(GroundPanel.Width, GroundPanel.Width);
                    ItemPictureBoxes[Position].BorderStyle = BorderStyle.None;
                    ItemPictureBoxes[Position].BackColor = GroundPanel.BackColor;
                    ItemPictureBoxes[Position].Image = Containers[i].CurtainItemImage;
                    ItemPictureBoxes[Position].SizeMode = PictureBoxSizeMode.CenterImage;
                    ItemPictureBoxes[Position].MouseEnter += ItemPictureBox_MouseEnter;
                    ItemPictureBoxes[Position].MouseLeave += ItemPictureBox_MouseLeave;
                    ItemPictureBoxes[Position].MouseDown += ItemPictureBox_MouseDown;
                    ItemPictureBoxes[Position].MouseUp += ItemPictureBox_MouseUp;
                    GroundPanel.Controls.Add(ItemPictureBoxes[Position]);

                    ItemButtons.Add(new Button());
                    ItemButtons[Position].TabIndex = 0;
                    ItemButtons[Position].TabStop = false;
                    ItemButtons[Position].Location = new Point(ItemPictureBoxes[Position].Right, HamburgerButton.Bottom + Position * GroundPanel.Width);
                    ItemButtons[Position].Size = new Size(GroundPanel.Width + 300, GroundPanel.Width);
                    ItemButtons[Position].FlatStyle = FlatStyle.Flat;
                    ItemButtons[Position].BackColor = GroundPanel.BackColor;
                    ItemButtons[Position].FlatAppearance.MouseOverBackColor = Colors.CurtainButtonMouseOver;
                    ItemButtons[Position].FlatAppearance.MouseDownBackColor = Colors.CurtainButtonMouseDown;
                    ItemButtons[Position].FlatAppearance.BorderSize = 0;
                    ItemButtons[Position].Font = new Font("Calibri", 18);
                    ItemButtons[Position].ForeColor = Colors.Constant;
                    ItemButtons[Position].Text = Containers[i].CurtainItemName;
                    ItemButtons[Position].TextAlign = ContentAlignment.MiddleLeft;
                    ItemButtons[Position].MouseEnter += ItemButton_MouseEnter;
                    ItemButtons[Position].MouseLeave += ItemButton_MouseLeave;
                    ItemButtons[Position].MouseDown += ItemButton_MouseDown;
                    ItemButtons[Position].MouseUp += ItemButton_MouseUp;
                    GroundPanel.Controls.Add(ItemButtons[Position]);

                    ItemStripes.Add(new Stripe(ParentForm, this, Colors, ItemButtons[Position].Text));
                    ItemStripes[Position].HideStripe();
                    StripeContainers.Add(new List<ContainerControls.Container>());
                }
            }

            SelectorPanel = new Panel();
            SelectorPanel.TabIndex = 0;
            SelectorPanel.TabStop = false;
            SelectorPanel.Location = new Point(0, ItemButtons[0].Top);
            SelectorPanel.Size = new Size(4, ItemButtons[0].Height);
            SelectorPanel.BorderStyle = BorderStyle.None;
            SelectorPanel.BackColor = Colors.Constant;
            GroundPanel.Controls.Add(SelectorPanel);
            SelectorPanel.BringToFront();

            for (int i = 0; i < Containers.Count; i++)
            {
                for (int j = 0; j < ItemButtons.Count; j++)
                {
                    if (Containers[i].CurtainItemName == ItemButtons[j].Text)
                        StripeContainers[j].Add(Containers[i]);
                }
            }

            ItemStripes[0].ShowStripe();

            for (int i = 0; i < StripeContainers.Count; i++)
                ItemStripes[i].GetItemsFrom(StripeContainers[i]);

            GroundPanel.BringToFront();
        }
        
        private void ItemPictureBox_MouseEnter(object sender, EventArgs e)
        {
            PictureBox ItemPictureBox = sender as PictureBox;
            int Position = (ItemPictureBox.Bottom - HamburgerButton.Height) / ItemControlHeight - 1;
            ItemPictureBox.BackColor = Colors.CurtainButtonMouseOver;
            ItemButtons[Position].BackColor = Colors.CurtainButtonMouseOver;
        }

        private void ItemPictureBox_MouseLeave(object sender, EventArgs e)
        {
            PictureBox ItemPictureBox = sender as PictureBox;
            int Position = (ItemPictureBox.Bottom - HamburgerButton.Height) / ItemControlHeight - 1;
            ItemPictureBox.BackColor = GroundPanel.BackColor;
            ItemButtons[Position].BackColor = GroundPanel.BackColor;
        }

        private void ItemPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            PictureBox ItemPictureBox = sender as PictureBox;
            int Position = (ItemPictureBox.Bottom - HamburgerButton.Height) / ItemControlHeight - 1;
            ItemPictureBox.BackColor = Colors.CurtainButtonMouseDown;
            ItemButtons[Position].BackColor = Colors.CurtainButtonMouseDown;
        }

        private void ItemPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (SelectorLocked == false)
            {
                SelectorLocked = true;

                for (int i = 0; i < Containers.Count; i++)
                {
                    if (Containers[i].Active == true)
                        Containers[i].Hide();
                }

                PictureBox ItemPictureBox = sender as PictureBox;
                int Position = (ItemPictureBox.Bottom - HamburgerButton.Height) / ItemControlHeight - 1;
                ItemPictureBox.BackColor = Colors.CurtainButtonMouseOver;
                ItemButtons[Position].BackColor = Colors.CurtainButtonMouseOver;

                CurrentSelectorTop = SelectorPanel.Top;
                TargetSelectorTop = ItemPictureBox.Top;

                SelectorEngine = new Thread(SelectorReplace);
                SelectorEngine.Start();

                if (TargetSelectorTop > CurrentSelectorTop)
                    TargetItemStripe = CurrentItemStripe + (TargetSelectorTop - CurrentSelectorTop) / ItemControlHeight;
                else if (TargetSelectorTop < CurrentSelectorTop)
                    TargetItemStripe = CurrentItemStripe - (CurrentSelectorTop - TargetSelectorTop) / ItemControlHeight;

                ItemStripes[CurrentItemStripe].HideStripe();
                ItemStripes[TargetItemStripe].ShowStripe();
                CurrentItemStripe = TargetItemStripe;

                Hide();
            }
        }

        private void ItemButton_MouseEnter(object sender, EventArgs e)
        {
            Button ItemButton = sender as Button;
            int Position = (ItemButton.Bottom - HamburgerButton.Height) / ItemControlHeight - 1;
            ItemPictureBoxes[Position].BackColor = Colors.CurtainButtonMouseOver;
        }

        private void ItemButton_MouseLeave(object sender, EventArgs e)
        {
            Button ItemButton = sender as Button;
            int Position = (ItemButton.Bottom - HamburgerButton.Height) / ItemControlHeight - 1;
            ItemPictureBoxes[Position].BackColor = GroundPanel.BackColor;
        }

        private void ItemButton_MouseDown(object sender, MouseEventArgs e)
        {
            Button ItemButton = sender as Button;
            int Position = (ItemButton.Bottom - HamburgerButton.Height) / ItemControlHeight - 1;
            ItemPictureBoxes[Position].BackColor = Colors.CurtainButtonMouseDown;
        }

        private void ItemButton_MouseUp(object sender, MouseEventArgs e)
        {
            Button ItemButton = sender as Button;
            int Position = (ItemButton.Bottom - HamburgerButton.Height) / ItemControlHeight - 1;
            ItemPictureBoxes[Position].BackColor = Colors.CurtainButtonMouseOver;

            if (SelectorLocked == false)
            {
                SelectorLocked = true;

                for (int i = 0; i < Containers.Count; i++)
                {
                    if (Containers[i].Active == true)
                        Containers[i].Hide();
                }

                CurrentSelectorTop = SelectorPanel.Top;
                TargetSelectorTop = ItemButton.Top;

                SelectorEngine = new Thread(SelectorReplace);
                SelectorEngine.Start();

                if (TargetSelectorTop > CurrentSelectorTop)
                    TargetItemStripe = CurrentItemStripe + (TargetSelectorTop - CurrentSelectorTop) / ItemControlHeight;
                else if (TargetSelectorTop < CurrentSelectorTop)
                    TargetItemStripe = CurrentItemStripe - (CurrentSelectorTop - TargetSelectorTop) / ItemControlHeight;

                ItemStripes[CurrentItemStripe].HideStripe();
                ItemStripes[TargetItemStripe].ShowStripe();
                CurrentItemStripe = TargetItemStripe;

                Hide();
            }
        }

        private void SelectorReplace()
        {
            if (TargetSelectorTop > CurrentSelectorTop)
            {
                for (int i = CurrentSelectorTop; i < TargetSelectorTop; i += (TargetSelectorTop - CurrentSelectorTop) / 10)
                {
                    SelectorPanel.Invoke((MethodInvoker)(delegate { SelectorPanel.Top += (TargetSelectorTop - CurrentSelectorTop) / 10; }));
                    Thread.Sleep(1);
                }
            }
            else if (TargetSelectorTop < CurrentSelectorTop)
            {
                for (int i = CurrentSelectorTop; i > TargetSelectorTop; i -= (CurrentSelectorTop - TargetSelectorTop) / 10)
                {
                    SelectorPanel.Invoke((MethodInvoker)(delegate { SelectorPanel.Top -= (CurrentSelectorTop - TargetSelectorTop) / 10; }));
                    Thread.Sleep(1);
                }
            }
            SelectorLocked = false;
        }
    }
}
