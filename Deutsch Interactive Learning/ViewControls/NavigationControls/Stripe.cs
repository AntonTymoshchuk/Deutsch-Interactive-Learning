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
    public class Stripe
    {
        private Panel GroundPanel;
        private Panel StripePanel;
        private Panel ContentPanel;
        private Label CaptionLabel;
        private Panel SelectorPanel;
        private List<Button> ItemButtons = new List<Button>();

        private const int ItemButtonWidth = 100;

        private Thread SelectorEngine;
        private int CurrentSelectorLeft;
        private int TargetSelectorLeft;
        private bool SelectorLocked = false;

        private Thread ContentEngine;
        
        private List<ContainerControls.Content> ItemContents = new List<ContainerControls.Content>();
        private int CurrentItemContent = 0;
        private int TargetItemContent = 0;

        private Form ParentForm;
        private Curtain ParentCurtain;
        private FlatDesign.Colors Colors;

        public Stripe(Form ParentForm, Curtain ParentCurtain, FlatDesign.Colors Colors, string Caption)
        {
            this.ParentForm = ParentForm;
            this.ParentCurtain = ParentCurtain;
            this.Colors = Colors;

            GroundPanel = new Panel();
            GroundPanel.TabIndex = 0;
            GroundPanel.TabStop = false;
            GroundPanel.Location = new Point(51, 31);
            GroundPanel.Size = new Size(ParentForm.Width - 52, ParentForm.Height - 32);
            GroundPanel.BorderStyle = BorderStyle.None;
            GroundPanel.BackColor = Colors.Background;
            GroundPanel.MouseClick += Control_MouseClick;

            StripePanel = new Panel();
            StripePanel.TabIndex = 0;
            StripePanel.TabStop = false;
            StripePanel.Location = new Point(0, 0);
            StripePanel.Size = new Size(GroundPanel.Width, 50);
            StripePanel.BorderStyle = BorderStyle.None;
            StripePanel.BackColor = Colors.Origin;
            StripePanel.MouseClick += Control_MouseClick;

            ContentPanel = new Panel();
            ContentPanel.TabIndex = 0;
            ContentPanel.TabStop = false;
            ContentPanel.Location = new Point(0, 50);
            ContentPanel.Size = new Size(0, GroundPanel.Height - 50);
            ContentPanel.BorderStyle = BorderStyle.None;
            ContentPanel.BackColor = Colors.Background;
            ContentPanel.MouseClick += Control_MouseClick;

            CaptionLabel = new Label();
            CaptionLabel.TabIndex = 0;
            CaptionLabel.Location = new Point(3, 5);
            CaptionLabel.AutoSize = true;
            CaptionLabel.FlatStyle = FlatStyle.Flat;
            CaptionLabel.BorderStyle = BorderStyle.None;
            CaptionLabel.BackColor = StripePanel.BackColor;
            CaptionLabel.Font = new Font("Calibri", 24);
            CaptionLabel.ForeColor = Colors.Constant;
            CaptionLabel.Text = Caption;
            CaptionLabel.TextAlign = ContentAlignment.MiddleLeft;
            CaptionLabel.MouseClick += Control_MouseClick;

            StripePanel.Controls.Add(CaptionLabel);

            GroundPanel.Controls.Add(StripePanel);
            GroundPanel.Controls.Add(ContentPanel);

            ParentForm.Controls.Add(GroundPanel);
        }

        private void Control_MouseClick(object sender, MouseEventArgs e)
        {
            ParentCurtain.Hide();
        }

        public void ShowStripe()
        {
            GroundPanel.Visible = true;
        }

        public void HideStripe()
        {
            GroundPanel.Visible = false;
        }

        public void GetItemsFrom(List<ContainerControls.Container> Containers)
        {
            bool Exists = false;
            int Position = 0;

            List<List<ContainerControls.Container>> ContentContainers = new List<List<ContainerControls.Container>>();

            for (int i = 0; i < Containers.Count; i++)
            {
                Exists = false;
                Position = 0;
                for (int j = 0; j < ItemButtons.Count; j++)
                {
                    Position++;
                    if (ItemButtons[j].Text == Containers[i].StripeItemName)
                    {
                        Exists = true;
                        break;
                    }
                }
                if (Exists == false)
                {
                    ItemButtons.Add(new Button());
                    ItemButtons[Position].TabIndex = 0;
                    ItemButtons[Position].TabStop = false;
                    ItemButtons[Position].Size = new Size(100, StripePanel.Height);
                    ItemButtons[Position].FlatStyle = FlatStyle.Flat;
                    ItemButtons[Position].BackColor = StripePanel.BackColor;
                    ItemButtons[Position].FlatAppearance.MouseOverBackColor = StripePanel.BackColor;
                    ItemButtons[Position].FlatAppearance.MouseDownBackColor = StripePanel.BackColor;
                    ItemButtons[Position].FlatAppearance.BorderSize = 0;
                    ItemButtons[Position].Font = new Font("Calibri", 14);
                    ItemButtons[Position].ForeColor = Colors.Constant;
                    ItemButtons[Position].Text = Containers[i].StripeItemName;
                    ItemButtons[Position].TextAlign = ContentAlignment.MiddleCenter;
                    ItemButtons[Position].MouseEnter += ItemButton_MouseEnter;
                    ItemButtons[Position].MouseLeave += ItemButton_MouseLeave;
                    ItemButtons[Position].MouseClick += ItemButton_MouseClick;
                    ItemButtons[Position].MouseClick += Control_MouseClick;
                    StripePanel.Controls.Add(ItemButtons[Position]);
                    ContentPanel.Width += GroundPanel.Width;
                    ItemContents.Add(new ContainerControls.Content(ContentPanel, ParentCurtain, Colors, Position));
                    ContentContainers.Add(new List<ContainerControls.Container>());
                }
            }

            ItemButtons[0].Font = new Font("Calibri", 14, FontStyle.Bold);

            int TotalButtonsWidth = 0;
            for (int i = 0; i < ItemButtons.Count; i++)
                TotalButtonsWidth += ItemButtons[i].Width;

            ItemButtons[0].Location = new Point((StripePanel.Width - CaptionLabel.Width) / 2 - TotalButtonsWidth / 2 + CaptionLabel.Width, 0);
            for (int i = 1; i < ItemButtons.Count; i++)
                ItemButtons[i].Location = new Point(ItemButtons[i - 1].Right, 0);

            SelectorPanel = new Panel();
            SelectorPanel.TabIndex = 0;
            SelectorPanel.TabStop = false;
            SelectorPanel.Location = new Point(ItemButtons[0].Left, 46);
            SelectorPanel.Size = new Size(ItemButtons[0].Width, 4);
            SelectorPanel.BorderStyle = BorderStyle.None;
            SelectorPanel.BackColor = Colors.Constant;
            StripePanel.Controls.Add(SelectorPanel);
            SelectorPanel.MouseClick += Control_MouseClick;
            SelectorPanel.BringToFront();

            for (int i = 0; i < Containers.Count; i++)
            {
                for (int j = 0; j < ItemButtons.Count; j++)
                {
                    if (Containers[i].StripeItemName == ItemButtons[j].Text)
                        ContentContainers[j].Add(Containers[i]);
                }
            }

            for (int i = 0; i < ContentContainers.Count; i++)
                ItemContents[i].GetItemsFrom(ContentContainers[i]);
        }

        private void ItemButton_MouseEnter(object sender, EventArgs e)
        {
            Button ItemButton = sender as Button;
            ItemButton.Font = new Font("Calibri", 14, FontStyle.Bold);
        }

        private void ItemButton_MouseLeave(object sender, EventArgs e)
        {
            Button ItemButton = sender as Button;
            if (SelectorPanel.Left != ItemButton.Left)
                ItemButton.Font = new Font("Calibri", 14);
        }

        private void ItemButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (SelectorLocked == false)
            {
                SelectorLocked = true;
                Button ItemButton = sender as Button;
                CurrentSelectorLeft = SelectorPanel.Left;
                TargetSelectorLeft = ItemButton.Left;

                SelectorEngine = new Thread(SelectorReplace);
                SelectorEngine.Start();

                ContentEngine = new Thread(ContentReplace);
                ContentEngine.Start();

                if (TargetSelectorLeft > CurrentSelectorLeft)
                    TargetItemContent = CurrentItemContent + (TargetSelectorLeft - CurrentSelectorLeft) / ItemButtonWidth;
                else if (TargetSelectorLeft < CurrentSelectorLeft)
                    TargetItemContent = CurrentItemContent - (CurrentSelectorLeft - TargetSelectorLeft) / ItemButtonWidth;

                ItemButtons[CurrentItemContent].Font = new Font("Calibri", 14);
                ItemButtons[TargetItemContent].Font = new Font("Calibri", 14, FontStyle.Bold);
                CurrentItemContent = TargetItemContent;
            }
        }
        
        private void SelectorReplace()
        {
            if (TargetSelectorLeft > CurrentSelectorLeft)
            {
                for (int i = CurrentSelectorLeft; i < TargetSelectorLeft; i += (TargetSelectorLeft - CurrentSelectorLeft) / 10)
                {
                    SelectorPanel.Invoke((MethodInvoker)(delegate { SelectorPanel.Left += (TargetSelectorLeft - CurrentSelectorLeft) / 10; }));
                    Thread.Sleep(1);
                }
            }
            else if (TargetSelectorLeft < CurrentSelectorLeft)
            {
                for (int i = CurrentSelectorLeft; i > TargetSelectorLeft; i -= (CurrentSelectorLeft - TargetSelectorLeft) / 10)
                {
                    SelectorPanel.Invoke((MethodInvoker)(delegate { SelectorPanel.Left -= (CurrentSelectorLeft - TargetSelectorLeft) / 10; }));
                    Thread.Sleep(1);
                }
            }
            SelectorLocked = false;
        }

        private void ContentReplace()
        {
            int TargetStripeLeft;
            if (TargetSelectorLeft > CurrentSelectorLeft)
            {
                TargetStripeLeft = (ContentPanel.Width / ItemContents.Count) * ((TargetSelectorLeft - CurrentSelectorLeft) / ItemButtonWidth);
                for (int i = TargetStripeLeft; i > 0; i -= TargetStripeLeft / 10)
                {
                    ContentPanel.Invoke((MethodInvoker)(delegate { ContentPanel.Left -= TargetStripeLeft / 10; }));
                    Thread.Sleep(1);
                }
            }
            else if (TargetSelectorLeft < CurrentSelectorLeft)
            {
                TargetStripeLeft = (ContentPanel.Width / ItemContents.Count) * ((CurrentSelectorLeft - TargetSelectorLeft) / ItemButtonWidth);
                for (int i = 0; i < TargetStripeLeft; i += TargetStripeLeft / 10)
                {
                    ContentPanel.Invoke((MethodInvoker)(delegate { ContentPanel.Left += TargetStripeLeft / 10; }));
                    Thread.Sleep(1);
                }
            }
        }
    }
}
