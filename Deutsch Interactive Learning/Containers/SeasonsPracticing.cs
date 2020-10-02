using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Globalization;
using Microsoft.Speech.Recognition;
using Microsoft.Speech.Synthesis;

namespace Deutsch_Interactive_Learning.Containers
{
    class SeasonsPracticing : ViewControls.ContainerControls.Container
    {
        private Panel SeasonGroundPanel;
        private PictureBox SeasonPictureBox;
        private Label GermanSeasonLabel;
        private Label EnglishSeasonLabel;
        private Button NextSeasonButton;
        private Button PreviusSeasonButton;

        private string GermanSeasonExample = "Winter Frühling Sommer Herbst";
        private string[] GermanSeasons;
        private string EnglishSeasonExample = "Winter Spring Summer Autumn";
        private string[] EnglishSeasons;
        private Bitmap[] SeasonImages;
        private Bitmap[] GoldSeasonImages;
        private int Position = 0;
        private CultureInfo cultureInfo = new CultureInfo("de-de");
        private SpeechRecognitionEngine speechRecognitionEngine;
        private SpeechSynthesizer speechSynthesizer;

        private FlatDesign.Colors Colors;

        public SeasonsPracticing(Form ParentForm, FlatDesign.Colors Colors)
        {
            this.Colors = Colors;

            CreateContainer(
                ParentForm,
                Colors,
                "Seasons",
                "",
                null,
                "Words",
                null,
                "Practicing");

            OnHide = DisposeSpeechInstruments;

            GermanSeasons = GermanSeasonExample.Split(' ');
            EnglishSeasons = EnglishSeasonExample.Split(' ');
            SeasonImages = new Bitmap[EnglishSeasons.Length];
            for (int i = 0; i < SeasonImages.Length; i++)
                SeasonImages[i] = new Bitmap(@"..\..\..\downloads\Icons\" + EnglishSeasons[i] + ".png");
            GoldSeasonImages = new Bitmap[EnglishSeasons.Length];
            for (int i = 0; i < GoldSeasonImages.Length; i++)
                GoldSeasonImages[i] = new Bitmap(@"..\..\..\downloads\Icons\" + EnglishSeasons[i] + "Gold.png");

            SeasonGroundPanel = new Panel();
            SeasonGroundPanel.TabIndex = 0;
            SeasonGroundPanel.TabStop = false;
            SeasonGroundPanel.Location = new Point(50, 50);
            SeasonGroundPanel.Size = new Size(ContainerSize.Width - 100, ContainerSize.Width - 100);
            SeasonGroundPanel.BorderStyle = BorderStyle.None;
            SeasonGroundPanel.BackColor = Colors.Background;

            SeasonPictureBox = new PictureBox();
            SeasonPictureBox.Location = new Point(140, 130);
            SeasonPictureBox.Size = new Size(120, 120);
            SeasonPictureBox.BorderStyle = BorderStyle.None;
            SeasonPictureBox.BackColor = Colors.Background;
            SeasonPictureBox.Image = SeasonImages[0];
            SeasonPictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
            SeasonPictureBox.MouseClick += SeasonPictureBox_MouseClick;

            GermanSeasonLabel = new Label();
            GermanSeasonLabel.TabIndex = 0;
            GermanSeasonLabel.Location = new Point(SeasonPictureBox.Right + 5, SeasonPictureBox.Top);
            GermanSeasonLabel.Size = new Size(350, 120);
            GermanSeasonLabel.FlatStyle = FlatStyle.Flat;
            GermanSeasonLabel.BorderStyle = BorderStyle.None;
            GermanSeasonLabel.BackColor = Colors.Background;
            GermanSeasonLabel.Font = new Font("Calibri", 60);
            GermanSeasonLabel.ForeColor = Colors.Constant;
            GermanSeasonLabel.Text = GermanSeasons[0];
            GermanSeasonLabel.TextAlign = ContentAlignment.MiddleLeft;
            GermanSeasonLabel.MouseClick += SeasonLabel_MouseClick;

            EnglishSeasonLabel = new Label();
            EnglishSeasonLabel.TabIndex = 0;
            EnglishSeasonLabel.Location = new Point(SeasonPictureBox.Right + 5, GermanSeasonLabel.Bottom);
            EnglishSeasonLabel.Size = new Size(350, 120);
            EnglishSeasonLabel.FlatStyle = FlatStyle.Flat;
            EnglishSeasonLabel.BorderStyle = BorderStyle.None;
            EnglishSeasonLabel.BackColor = Colors.Background;
            EnglishSeasonLabel.Font = new Font("Calibri", 40);
            EnglishSeasonLabel.ForeColor = Colors.Constant;
            EnglishSeasonLabel.Text = EnglishSeasons[0];
            EnglishSeasonLabel.TextAlign = ContentAlignment.MiddleLeft;
            EnglishSeasonLabel.MouseClick += SeasonLabel_MouseClick;

            NextSeasonButton = new Button();
            NextSeasonButton.TabIndex = 0;
            NextSeasonButton.TabStop = false;
            NextSeasonButton.Location = new Point(SeasonGroundPanel.Width - 70, 200);
            NextSeasonButton.Size = new Size(70, 100);
            NextSeasonButton.FlatStyle = FlatStyle.Flat;
            NextSeasonButton.BackColor = Colors.Origin;
            NextSeasonButton.FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
            NextSeasonButton.FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
            NextSeasonButton.FlatAppearance.BorderSize = 0;
            NextSeasonButton.Image = new Bitmap(@"..\..\..\downloads\Icons\Next.png");
            NextSeasonButton.ImageAlign = ContentAlignment.MiddleCenter;
            NextSeasonButton.MouseClick += NextSeasonButton_MouseClick;

            PreviusSeasonButton = new Button();
            PreviusSeasonButton.TabIndex = 0;
            PreviusSeasonButton.TabStop = false;
            PreviusSeasonButton.Location = new Point(0, 200);
            PreviusSeasonButton.Size = new Size(70, 100);
            PreviusSeasonButton.FlatStyle = FlatStyle.Flat;
            PreviusSeasonButton.BackColor = Colors.Origin;
            PreviusSeasonButton.FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
            PreviusSeasonButton.FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
            PreviusSeasonButton.FlatAppearance.BorderSize = 0;
            PreviusSeasonButton.Image = new Bitmap(@"..\..\..\downloads\Icons\Previus.png");
            PreviusSeasonButton.ImageAlign = ContentAlignment.MiddleCenter;
            PreviusSeasonButton.Visible = false;
            PreviusSeasonButton.MouseClick += PreviusSeasonButton_MouseClick;

            SeasonGroundPanel.Controls.Add(SeasonPictureBox);
            SeasonGroundPanel.Controls.Add(GermanSeasonLabel);
            SeasonGroundPanel.Controls.Add(EnglishSeasonLabel);
            SeasonGroundPanel.Controls.Add(NextSeasonButton);
            SeasonGroundPanel.Controls.Add(PreviusSeasonButton);

            AddControl(SeasonGroundPanel);
        }

        private void SeasonPictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            speechSynthesizer = new SpeechSynthesizer();
            speechSynthesizer.SetOutputToDefaultAudioDevice();
            speechSynthesizer.Speak(GermanSeasons[Position]);
            SetSeasonToRecognize();
        }

        private void SeasonLabel_MouseClick(object sender, MouseEventArgs e)
        {
            speechSynthesizer = new SpeechSynthesizer();
            speechSynthesizer.SetOutputToDefaultAudioDevice();
            speechSynthesizer.Speak(GermanSeasons[Position]);
            SetSeasonToRecognize();
        }

        private void NextSeasonButton_MouseClick(object sender, MouseEventArgs e)
        {
            GermanSeasonLabel.ForeColor = Colors.Constant;
            if (Position < GermanSeasons.Length - 1)
            {
                Position++;
                GermanSeasonLabel.Text = GermanSeasons[Position];
                SeasonPictureBox.Image = SeasonImages[Position];
                EnglishSeasonLabel.Text = EnglishSeasons[Position];
            }
            NextSeasonButton.Visible = true;
            PreviusSeasonButton.Visible = true;
            if (Position == GermanSeasons.Length - 1)
                NextSeasonButton.Visible = false;
        }

        private void PreviusSeasonButton_MouseClick(object sender, MouseEventArgs e)
        {
            GermanSeasonLabel.ForeColor = Colors.Constant;
            if (Position > 0)
            {
                Position--;
                GermanSeasonLabel.Text = GermanSeasons[Position];
                SeasonPictureBox.Image = SeasonImages[Position];
                EnglishSeasonLabel.Text = EnglishSeasons[Position];
            }
            NextSeasonButton.Visible = true;
            PreviusSeasonButton.Visible = true;
            if (Position == 0)
                PreviusSeasonButton.Visible = false;
        }

        private void SetSeasonToRecognize()
        {
            speechRecognitionEngine = new SpeechRecognitionEngine();
            speechRecognitionEngine.SetInputToDefaultAudioDevice();
            speechRecognitionEngine.SpeechRecognized += SpeechRecognitionEngine_SpeechRecognized;
            GrammarBuilder grammarBuilder = new GrammarBuilder();
            grammarBuilder.Culture = cultureInfo;
            grammarBuilder.Append(GermanSeasons[Position]);
            Grammar grammar = new Grammar(grammarBuilder);
            speechRecognitionEngine.LoadGrammar(grammar);
            speechRecognitionEngine.RecognizeAsync(RecognizeMode.Multiple);
        }

        private void SpeechRecognitionEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence * 100 >= 90)
            {
                GermanSeasonLabel.ForeColor = Color.Gold;
                SeasonPictureBox.Image = GoldSeasonImages[Position];
            }
        }

        private void DisposeSpeechInstruments()
        {
            if (speechSynthesizer != null)
                speechSynthesizer.Dispose();
            if (speechRecognitionEngine != null)
                speechRecognitionEngine.Dispose();
        }
    }
}
