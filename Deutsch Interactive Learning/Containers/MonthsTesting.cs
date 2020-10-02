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
    class MonthsTesting : ViewControls.ContainerControls.Container
    {
        private Button TestControlButton;
        private PictureBox SeasonPictureBox;
        private Label SeasonLabel;
        private Label MonthNumberLabel;

        private string GermanMonthExample = "Januar Februar März April Kann Juni Juli August September Oktober November Dezember";
        private string[] GermanMonths;
        private string GermanSeasonExample = "Winter Frühling Sommer Herbst";
        private string[] GermanSeasons;
        private string EnglishSeasonExample = "Winter Spring Summer Autumn";
        private string[] EnglishSeasons;
        private Bitmap[] SeasonImages;
        private Bitmap[] RedSeasonImages;
        private int Position = 0;
        private bool TestStarted = false;
        private bool TestIsOn = false;
        private Thread MonthsTester;
        private CultureInfo cultureInfo = new CultureInfo("de-de");
        private SpeechRecognitionEngine speechRecognitionEngine;

        private FlatDesign.Colors Colors;

        public MonthsTesting(Form ParentForm, FlatDesign.Colors Colors)
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
                "Testing");

            OnHide = DisposeSpeechRecognitionEngine;

            GermanMonths = GermanMonthExample.Split(' ');
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
            SeasonPictureBox.Location = new Point(200, TestControlButton.Bottom + 50);
            SeasonPictureBox.Size = new Size(120, 120);
            SeasonPictureBox.BorderStyle = BorderStyle.None;
            SeasonPictureBox.BackColor = Colors.Background;
            SeasonPictureBox.SizeMode = PictureBoxSizeMode.CenterImage;

            SeasonLabel = new Label();
            SeasonLabel.TabIndex = 0;
            SeasonLabel.Location = new Point(SeasonPictureBox.Right + 10, TestControlButton.Bottom + 50);
            SeasonLabel.Size = new Size(ContainerSize.Width - SeasonPictureBox.Right, 120);
            SeasonLabel.FlatStyle = FlatStyle.Flat;
            SeasonLabel.BorderStyle = BorderStyle.None;
            SeasonLabel.BackColor = Colors.Background;
            SeasonLabel.Font = new Font("Calibri", 50);
            SeasonLabel.ForeColor = Colors.Constant;
            SeasonLabel.TextAlign = ContentAlignment.MiddleLeft;

            MonthNumberLabel = new Label();
            MonthNumberLabel.TabIndex = 0;
            MonthNumberLabel.Location = new Point(0, SeasonPictureBox.Bottom);
            MonthNumberLabel.Size = new Size(ContainerSize.Width, 300);
            MonthNumberLabel.FlatStyle = FlatStyle.Flat;
            MonthNumberLabel.BorderStyle = BorderStyle.None;
            MonthNumberLabel.BackColor = Colors.Background;
            MonthNumberLabel.Font = new Font("Calibri", 200);
            MonthNumberLabel.ForeColor = Colors.Constant;
            MonthNumberLabel.TextAlign = ContentAlignment.MiddleCenter;
            MonthNumberLabel.Text = "1";

            SetMonthToRecognize();

            AddControl(TestControlButton);
            AddControl(SeasonPictureBox);
            AddControl(SeasonLabel);
            AddControl(MonthNumberLabel);
        }

        private void TestControlButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (TestStarted == false && TestIsOn == false)
            {
                TestControlButton.Text = "Pause";
                TestStarted = true;
                TestIsOn = true;
                MonthsTester = new Thread(Testing);
                MonthsTester.IsBackground = true;
                MonthsTester.Start();
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
            SeasonLabel.Invoke((MethodInvoker)(delegate { SeasonLabel.Visible = true; }));
            SeasonPictureBox.Invoke((MethodInvoker)(delegate { SeasonPictureBox.Image = SeasonImages[0]; }));
            SeasonLabel.Invoke((MethodInvoker)(delegate { SeasonLabel.ForeColor = Colors.Constant; }));
            MonthNumberLabel.Invoke((MethodInvoker)(delegate
            {
                MonthNumberLabel.Font = new Font("Calibri", 200);
                MonthNumberLabel.Top = 220;
            }));

            for (int i = 0; i < GermanMonths.Length; i++)
            {
                if (TestIsOn == true)
                {
                    Position++;
                    MonthNumberLabel.Invoke((MethodInvoker)(delegate
                    {
                        MonthNumberLabel.Text = Convert.ToString(Position + 1);
                        MonthNumberLabel.ForeColor = Colors.Constant;
                    }));
                    SeasonLabel.Invoke((MethodInvoker)(delegate { SeasonLabel.ForeColor = Colors.Constant; }));
                    if (Position == 0 || Position == 1)
                    {
                        SeasonPictureBox.Invoke((MethodInvoker)(delegate { SeasonPictureBox.Image = SeasonImages[0]; }));
                        SeasonLabel.Invoke((MethodInvoker)(delegate { SeasonLabel.Text = GermanSeasons[0]; }));
                    }
                    else if (Position >= 2 && Position <= 4)
                    {
                        SeasonPictureBox.Invoke((MethodInvoker)(delegate { SeasonPictureBox.Image = SeasonImages[1]; }));
                        SeasonLabel.Invoke((MethodInvoker)(delegate { SeasonLabel.Text = GermanSeasons[1]; }));
                    }
                    else if (Position >= 5 && Position <= 7)
                    {
                        SeasonPictureBox.Invoke((MethodInvoker)(delegate { SeasonPictureBox.Image = SeasonImages[2]; }));
                        SeasonLabel.Invoke((MethodInvoker)(delegate { SeasonLabel.Text = GermanSeasons[2]; }));
                    }
                    else if (Position >= 8 && Position <= 10)
                    {
                        SeasonPictureBox.Invoke((MethodInvoker)(delegate { SeasonPictureBox.Image = SeasonImages[3]; }));
                        SeasonLabel.Invoke((MethodInvoker)(delegate { SeasonLabel.Text = GermanSeasons[3]; }));
                    }
                    else if (Position == 11)
                    {
                        SeasonPictureBox.Invoke((MethodInvoker)(delegate { SeasonPictureBox.Image = SeasonImages[0]; }));
                        SeasonLabel.Invoke((MethodInvoker)(delegate { SeasonLabel.Text = GermanSeasons[0]; }));
                    }
                    SetMonthToRecognize();
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
            SeasonLabel.Invoke((MethodInvoker)(delegate { SeasonLabel.Visible = false; }));
            MonthNumberLabel.Invoke((MethodInvoker)(delegate
            {
                MonthNumberLabel.Top = 120;
                MonthNumberLabel.Font = new Font("Calibri", 36);
                MonthNumberLabel.ForeColor = Colors.Constant;
                MonthNumberLabel.Text = "Congratulations!!!\n Your test is complete!";
            }));

            TestIsOn = false;
            TestStarted = false;
            TestControlButton.Invoke((MethodInvoker)(delegate { TestControlButton.Text = "Start"; }));
        }

        private void SetMonthToRecognize()
        {
            speechRecognitionEngine = new SpeechRecognitionEngine(cultureInfo);
            speechRecognitionEngine.SetInputToDefaultAudioDevice();
            speechRecognitionEngine.SpeechRecognized += SpeechRecognitionEngine_SpeechRecognized;
            GrammarBuilder grammarBuilder = new GrammarBuilder();
            grammarBuilder.Culture = cultureInfo;
            grammarBuilder.Append(GermanMonths[Position]);
            Grammar grammar = new Grammar(grammarBuilder);
            speechRecognitionEngine.LoadGrammar(grammar);
            speechRecognitionEngine.RecognizeAsync(RecognizeMode.Multiple);
        }

        private void SpeechRecognitionEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence * 100 >= 75)
            {
                MonthNumberLabel.Invoke((MethodInvoker)(delegate { MonthNumberLabel.ForeColor = Color.Red; }));

                if (Position == 0 || Position == 1)
                    SeasonPictureBox.Invoke((MethodInvoker)(delegate { SeasonPictureBox.Image = RedSeasonImages[0]; }));
                else if (Position >= 2 && Position <= 4)
                    SeasonPictureBox.Invoke((MethodInvoker)(delegate { SeasonPictureBox.Image = RedSeasonImages[1]; }));
                else if (Position >= 5 && Position <= 7)
                    SeasonPictureBox.Invoke((MethodInvoker)(delegate { SeasonPictureBox.Image = RedSeasonImages[2]; }));
                else if (Position >= 8 && Position <= 10)
                    SeasonPictureBox.Invoke((MethodInvoker)(delegate { SeasonPictureBox.Image = RedSeasonImages[3]; }));
                else if (Position == 11)
                    SeasonPictureBox.Invoke((MethodInvoker)(delegate { SeasonPictureBox.Image = RedSeasonImages[0]; }));

                SeasonLabel.Invoke((MethodInvoker)(delegate { SeasonLabel.ForeColor = Color.Red; SeasonLabel.Text = Convert.ToString(e.Result.Confidence*100); }));
                Thread.Sleep(500);
            }
        }

        private void DisposeSpeechRecognitionEngine()
        {
            if (speechRecognitionEngine != null)
                speechRecognitionEngine.Dispose();
        }
    }
}
