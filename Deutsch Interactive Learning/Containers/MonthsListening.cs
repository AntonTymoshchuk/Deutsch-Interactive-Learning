using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using Microsoft.Speech.Synthesis;

namespace Deutsch_Interactive_Learning.Containers
{
    class MonthsListening : ViewControls.ContainerControls.Container
    {
        private Panel MonthGroundPanel;
        private PictureBox SeasonPictureBox;
        private Label SeasonLabel;
        private Label GermanMonthLabel;
        private Label EnglishMonthLabel;
        private Button NextMonthButton;
        private Button PreviusMonthButton;

        private string GermanMonthExample = "Januar Februar März April Kann Juni Juli August September Oktober November Dezember";
        private string[] GermanMonths;
        private string EnglishMonthExample = "January February March April May June July August September October November December";
        private string[] EnglishMonths;
        private string GermanSeasonExample = "Winter Frühling Sommer Herbst";
        private string[] GermanSeasons;
        private string EnglishSeasonExample = "Winter Spring Summer Autumn";
        private string[] EnglishSeasons;
        private Bitmap[] SeasonImages;
        private int Position = 0;
        private SpeechSynthesizer speechSynthesizer;

        private FlatDesign.Colors Colors;

        public MonthsListening(Form ParentForm, FlatDesign.Colors Colors)
        {
            this.Colors = Colors;

            CreateContainer(
                ParentForm,
                Colors,
                "Months",
                "",
                null,
                "Words",
                null,
                "Listening");

            OnHide = DisposeSpeechSynthesizer;

            GermanMonths = GermanMonthExample.Split(' ');
            EnglishMonths = EnglishMonthExample.Split(' ');
            GermanSeasons = GermanSeasonExample.Split(' ');
            EnglishSeasons = EnglishSeasonExample.Split(' ');
            SeasonImages = new Bitmap[EnglishSeasons.Length];
            for (int i = 0; i < SeasonImages.Length; i++)
                SeasonImages[i] = new Bitmap(@"..\..\..\downloads\Icons\" + EnglishSeasons[i] + ".png");

            MonthGroundPanel = new Panel();
            MonthGroundPanel.TabIndex = 0;
            MonthGroundPanel.TabStop = false;
            MonthGroundPanel.Location = new Point(50, 50);
            MonthGroundPanel.Size = new Size(ContainerSize.Width - 100, ContainerSize.Height - 100);
            MonthGroundPanel.BorderStyle = BorderStyle.None;
            MonthGroundPanel.BackColor = Colors.Background;

            SeasonPictureBox = new PictureBox();
            SeasonPictureBox.Location = new Point(150, 10);
            SeasonPictureBox.Size = new Size(120, 120);
            SeasonPictureBox.BorderStyle = BorderStyle.None;
            SeasonPictureBox.BackColor = Colors.Background;
            SeasonPictureBox.Image = SeasonImages[0];
            SeasonPictureBox.SizeMode = PictureBoxSizeMode.CenterImage;

            SeasonLabel = new Label();
            SeasonLabel.TabIndex = 0;
            SeasonLabel.Location = new Point(SeasonPictureBox.Right + 10, 10);
            SeasonLabel.Size = new Size(MonthGroundPanel.Width - SeasonPictureBox.Right, 120);
            SeasonLabel.FlatStyle = FlatStyle.Flat;
            SeasonLabel.BorderStyle = BorderStyle.None;
            SeasonLabel.BackColor = Colors.Background;
            SeasonLabel.Font = new Font("Calibri", 50);
            SeasonLabel.ForeColor = Colors.Constant;
            SeasonLabel.Text = GermanSeasons[0];
            SeasonLabel.TextAlign = ContentAlignment.MiddleLeft;

            GermanMonthLabel = new Label();
            GermanMonthLabel.TabIndex = 0;
            GermanMonthLabel.Location = new Point(70, SeasonLabel.Bottom + 10);
            GermanMonthLabel.Size = new Size(MonthGroundPanel.Width - 140, 170);
            GermanMonthLabel.FlatStyle = FlatStyle.Flat;
            GermanMonthLabel.BorderStyle = BorderStyle.None;
            GermanMonthLabel.BackColor = Colors.Background;
            GermanMonthLabel.Font = new Font("Calibri", 80);
            GermanMonthLabel.ForeColor = Colors.Constant;
            GermanMonthLabel.Text = GermanMonths[0];
            GermanMonthLabel.TextAlign = ContentAlignment.MiddleCenter;
            GermanMonthLabel.MouseClick += GermanMonthLabel_MouseClick;

            EnglishMonthLabel = new Label();
            EnglishMonthLabel.TabIndex = 0;
            EnglishMonthLabel.Location = new Point(0, GermanMonthLabel.Bottom + 10);
            EnglishMonthLabel.Size = new Size(MonthGroundPanel.Width, 120);
            EnglishMonthLabel.FlatStyle = FlatStyle.Flat;
            EnglishMonthLabel.BorderStyle = BorderStyle.None;
            EnglishMonthLabel.BackColor = Colors.Background;
            EnglishMonthLabel.Font = new Font("Calibri", 40);
            EnglishMonthLabel.ForeColor = Colors.Constant;
            EnglishMonthLabel.Text = EnglishMonths[0];
            EnglishMonthLabel.TextAlign = ContentAlignment.MiddleCenter;

            NextMonthButton = new Button();
            NextMonthButton.TabIndex = 0;
            NextMonthButton.TabStop = false;
            NextMonthButton.Location = new Point(MonthGroundPanel.Width - 70, SeasonLabel.Bottom + 45);
            NextMonthButton.Size = new Size(70, 100);
            NextMonthButton.FlatStyle = FlatStyle.Flat;
            NextMonthButton.BackColor = Colors.Origin;
            NextMonthButton.FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
            NextMonthButton.FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
            NextMonthButton.FlatAppearance.BorderSize = 0;
            NextMonthButton.Image = new Bitmap(@"..\..\..\downloads\Icons\Next.png");
            NextMonthButton.ImageAlign = ContentAlignment.MiddleCenter;
            NextMonthButton.MouseClick += NextSeasonButton_MouseClick;

            PreviusMonthButton = new Button();
            PreviusMonthButton.TabIndex = 0;
            PreviusMonthButton.TabStop = false;
            PreviusMonthButton.Location = new Point(0, SeasonLabel.Bottom + 45);
            PreviusMonthButton.Size = new Size(70, 100);
            PreviusMonthButton.FlatStyle = FlatStyle.Flat;
            PreviusMonthButton.BackColor = Colors.Origin;
            PreviusMonthButton.FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
            PreviusMonthButton.FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
            PreviusMonthButton.FlatAppearance.BorderSize = 0;
            PreviusMonthButton.Image = new Bitmap(@"..\..\..\downloads\Icons\Previus.png");
            PreviusMonthButton.ImageAlign = ContentAlignment.MiddleCenter;
            PreviusMonthButton.Visible = false;
            PreviusMonthButton.MouseClick += PreviusSeasonButton_MouseClick;

            MonthGroundPanel.Controls.Add(SeasonPictureBox);
            MonthGroundPanel.Controls.Add(SeasonLabel);
            MonthGroundPanel.Controls.Add(GermanMonthLabel);
            MonthGroundPanel.Controls.Add(EnglishMonthLabel);
            MonthGroundPanel.Controls.Add(NextMonthButton);
            MonthGroundPanel.Controls.Add(PreviusMonthButton);

            AddControl(MonthGroundPanel);
        }

        private void GermanMonthLabel_MouseClick(object sender, MouseEventArgs e)
        {
            speechSynthesizer = new SpeechSynthesizer();
            speechSynthesizer.SetOutputToDefaultAudioDevice();
            Thread thread = new Thread(() => { speechSynthesizer.Speak(GermanMonths[Position]); });
            thread.IsBackground = true;
            thread.Start();
        }

        private void NextSeasonButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (Position < GermanMonths.Length - 1)
            {
                Position++;
                GermanMonthLabel.Text = GermanMonths[Position];
                EnglishMonthLabel.Text = EnglishMonths[Position];
                if (Position == 0 || Position == 1)
                {
                    SeasonPictureBox.Image = SeasonImages[0];
                    SeasonLabel.Text = GermanSeasons[0];
                }
                else if (Position >= 2 && Position <= 4)
                {
                    SeasonPictureBox.Image = SeasonImages[1];
                    SeasonLabel.Text = GermanSeasons[1];
                }
                else if (Position >= 5 && Position <= 7)
                {
                    SeasonPictureBox.Image = SeasonImages[2];
                    SeasonLabel.Text = GermanSeasons[2];
                }
                else if (Position >= 8 && Position <= 10)
                {
                    SeasonPictureBox.Image = SeasonImages[3];
                    SeasonLabel.Text = GermanSeasons[3];
                }
                else if (Position == 11)
                {
                    SeasonPictureBox.Image = SeasonImages[0];
                    SeasonLabel.Text = GermanSeasons[0];
                }
            }
            NextMonthButton.Visible = true;
            PreviusMonthButton.Visible = true;
            if (Position == GermanMonths.Length - 1)
                NextMonthButton.Visible = false;
        }

        private void PreviusSeasonButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (Position > 0)
            {
                Position--;
                GermanMonthLabel.Text = GermanMonths[Position];
                EnglishMonthLabel.Text = EnglishMonths[Position];
                if (Position == 0 || Position == 1)
                {
                    SeasonPictureBox.Image = SeasonImages[0];
                    SeasonLabel.Text = GermanSeasons[0];
                }
                else if (Position >= 2 && Position <= 4)
                {
                    SeasonPictureBox.Image = SeasonImages[1];
                    SeasonLabel.Text = GermanSeasons[1];
                }
                else if (Position >= 5 && Position <= 7)
                {
                    SeasonPictureBox.Image = SeasonImages[2];
                    SeasonLabel.Text = GermanSeasons[2];
                }
                else if (Position >= 8 && Position <= 10)
                {
                    SeasonPictureBox.Image = SeasonImages[3];
                    SeasonLabel.Text = GermanSeasons[3];
                }
                else if (Position == 11)
                {
                    SeasonPictureBox.Image = SeasonImages[0];
                    SeasonLabel.Text = GermanSeasons[0];
                }
            }
            NextMonthButton.Visible = true;
            PreviusMonthButton.Visible = true;
            if (Position == 0)
                PreviusMonthButton.Visible = false;
        }

        private void DisposeSpeechSynthesizer()
        {
            if (speechSynthesizer != null)
                speechSynthesizer.Dispose();
        }
    }
}
