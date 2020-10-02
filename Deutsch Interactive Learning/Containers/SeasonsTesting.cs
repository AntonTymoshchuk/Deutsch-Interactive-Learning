using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using System.Globalization;
using Microsoft.Speech.Recognition;
using Microsoft.Speech.Synthesis;

namespace Deutsch_Interactive_Learning.Containers
{
    class SeasonsTesting : ViewControls.ContainerControls.Container
    {
        private Button TestControlButton;
        private PictureBox SeasonPictureBox;
        private Label CongratulationsLabel;

        private string GermanSeasonExample = "Winter Frühling Sommer Herbst";
        private string[] GermanSeasons;
        private string EnglishSeasonExample = "Winter Spring Summer Autumn";
        private string[] EnglishSeasons;
        private Bitmap[] SeasonImages;
        private Bitmap[] RedSeasonImages;
        private int Position = -1;
        private bool TestStarted = false;
        private bool TestIsOn = false;
        private Thread SeasonsTester;
        private CultureInfo cultureInfo = new CultureInfo("de-de");
        private SpeechRecognitionEngine speechRecognitionEngine;

        private FlatDesign.Colors Colors;

        public SeasonsTesting(Form ParentForm, FlatDesign.Colors Colors)
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
                "Testing");

            OnHide = DisposeSpeechInstruments;

            GermanSeasons = GermanSeasonExample.Split(' ');
            EnglishSeasons = EnglishSeasonExample.Split(' ');
            SeasonImages = new Bitmap[EnglishSeasons.Length];
            for (int i = 0; i < SeasonImages.Length; i++)
                SeasonImages[i] = new Bitmap(@"..\..\..\downloads\Icons\" + EnglishSeasons[i] + ".png");
            RedSeasonImages = new Bitmap[EnglishSeasons.Length];
            for (int i = 0; i < RedSeasonImages.Length; i++)
                RedSeasonImages[i] = new Bitmap(@"..\..\..\downloads\Icons\" + EnglishSeasons[i] + "Red.png");

            TestControlButton = new Button();
            TestControlButton.TabIndex = 0;
            TestControlButton.TabStop = false;
            TestControlButton.Location = new Point(20, 20);
            TestControlButton.Size = new Size(120, 50);
            TestControlButton.FlatStyle = FlatStyle.Flat;
            TestControlButton.BackColor = Colors.Origin;
            TestControlButton.FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
            TestControlButton.FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
            TestControlButton.FlatAppearance.BorderSize = 0;
            TestControlButton.Font = new Font("Calibri", 20);
            TestControlButton.ForeColor = Colors.Constant;
            TestControlButton.Text = "Start";
            TestControlButton.TextAlign = ContentAlignment.MiddleCenter;
            TestControlButton.MouseClick += TestControlButton_MouseClick;

            SeasonPictureBox = new PictureBox();
            SeasonPictureBox.Location = new Point(0, TestControlButton.Bottom);
            SeasonPictureBox.Size = new Size(ContainerSize.Width, ContainerSize.Height - TestControlButton.Bottom);
            SeasonPictureBox.BorderStyle = BorderStyle.None;
            SeasonPictureBox.BackColor = Colors.Background;
            SeasonPictureBox.SizeMode = PictureBoxSizeMode.CenterImage;

            CongratulationsLabel = new Label();
            CongratulationsLabel.TabIndex = 0;
            CongratulationsLabel.Location = SeasonPictureBox.Location;
            CongratulationsLabel.Size = SeasonPictureBox.Size;
            CongratulationsLabel.FlatStyle = FlatStyle.Flat;
            CongratulationsLabel.BorderStyle = BorderStyle.None;
            CongratulationsLabel.BackColor = Colors.Background;
            CongratulationsLabel.Font = new Font("Calibri", 36);
            CongratulationsLabel.ForeColor = Colors.Constant;
            CongratulationsLabel.Text = "Congratulations!!!\n Your test is complete!";
            CongratulationsLabel.TextAlign = ContentAlignment.MiddleCenter;

            AddControl(TestControlButton);
            AddControl(SeasonPictureBox);
            AddControl(CongratulationsLabel);
        }

        private void TestControlButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (TestStarted == false && TestIsOn == false)
            {
                TestControlButton.Text = "Pause";
                TestStarted = true;
                TestIsOn = true;
                SeasonsTester = new Thread(Testing);
                SeasonsTester.IsBackground = true;
                SeasonsTester.Start();
            }
            else if (TestStarted == true && TestIsOn == true)
            {
                TestControlButton.Text = "Resume";
                TestIsOn = false;
            }
            else if (TestStarted == true && TestIsOn == false)
            {
                TestControlButton.Text = "Pause";
                TestIsOn = true;
            }
        }

        private void Testing()
        {
            Position = -1;
            SeasonPictureBox.Invoke((MethodInvoker)(delegate { SeasonPictureBox.Visible = true; }));

            for (int i = 0; i < SeasonImages.Length; i++)
            {
                if (TestIsOn == true)
                {
                    Position++;
                    SeasonPictureBox.Invoke((MethodInvoker)(delegate { SeasonPictureBox.Image = SeasonImages[Position]; }));
                    SetSeasonToRecognize();
                }
                else
                {
                    while (true)
                    {
                        if (TestIsOn == true)
                            break;
                        Thread.Sleep(1);
                    }
                }
            }

            SeasonPictureBox.Invoke((MethodInvoker)(delegate { SeasonPictureBox.Visible = false; }));

            TestIsOn = false;
            TestStarted = false;
            TestControlButton.Invoke((MethodInvoker)(delegate { TestControlButton.Text = "Start"; }));
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
            speechRecognitionEngine.Recognize();
        }

        private void SpeechRecognitionEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            SeasonPictureBox.Invoke((MethodInvoker)(delegate { SeasonPictureBox.Image = RedSeasonImages[Position]; }));
            Thread.Sleep(500);
        }

        private void DisposeSpeechInstruments()
        {
            if (speechRecognitionEngine != null)
                speechRecognitionEngine.Dispose();
        }
    }
}
