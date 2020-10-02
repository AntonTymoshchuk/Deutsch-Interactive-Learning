using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Globalization;
using Microsoft.Speech.Recognition;

namespace Deutsch_Interactive_Learning.Containers
{
    class WeekTesting : ViewControls.ContainerControls.Container
    {
        private Button TestControlButton;
        private Label DayNumberLabel;

        private string GermanWeekExample = "Sonntag Montag Dienstag Mittwoch Donnerstag Freitag Sonnabend";
        private string[] GermanDays;
        private string EnglishWeekExample = "Sunday Monday Tuesday Wednesday Thursday Friday Saturday";
        private string[] EnglishDays;
        private int Position = -1;
        private bool TestStarted = false;
        private bool TestIsOn = false;
        private Thread WeekTester;
        private CultureInfo cultureInfo = new CultureInfo("de-de");
        private SpeechRecognitionEngine speechRecognitionEngine;

        private FlatDesign.Colors Colors;

        public WeekTesting(Form ParentForm, FlatDesign.Colors Colors)
        {
            this.Colors = Colors;

            CreateContainer(
                ParentForm,
                Colors,
                "Week",
                "",
                null,
                "Words",
                null,
                "Testing");

            OnHide = DisposeSpeechSynthesizer;

            GermanDays = GermanWeekExample.Split(' ');
            EnglishDays = EnglishWeekExample.Split(' ');

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

            DayNumberLabel = new Label();
            DayNumberLabel.TabIndex = 0;
            DayNumberLabel.Location = new Point(0, TestControlButton.Bottom);
            DayNumberLabel.Size = new Size(ContainerSize.Width, ContainerSize.Height - TestControlButton.Bottom);
            DayNumberLabel.FlatStyle = FlatStyle.Flat;
            DayNumberLabel.BorderStyle = BorderStyle.None;
            DayNumberLabel.BackColor = Colors.Background;
            DayNumberLabel.Font = new Font("Calibri", 80);
            DayNumberLabel.ForeColor = Colors.Constant;
            DayNumberLabel.TextAlign = ContentAlignment.MiddleCenter;

            AddControl(TestControlButton);
            AddControl(DayNumberLabel);
        }

        private void TestControlButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (TestStarted == false && TestIsOn == false)
            {
                TestControlButton.Text = "Pause";
                TestStarted = true;
                TestIsOn = true;
                WeekTester = new Thread(Testing);
                WeekTester.IsBackground = true;
                WeekTester.Start();
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
            DayNumberLabel.Invoke((MethodInvoker)(delegate { DayNumberLabel.Font = new Font("Calibri", 80); }));

            for (int i = 0; i < GermanDays.Length; i++)
            {
                if (TestIsOn == true)
                {
                    Position++;
                    DayNumberLabel.Invoke((MethodInvoker)(delegate
                    {
                        DayNumberLabel.ForeColor = Colors.Constant;
                        DayNumberLabel.Text = EnglishDays[Position];
                    }));
                    SetGermanDayToRecognize();
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

            DayNumberLabel.Invoke((MethodInvoker)(delegate
            {
                DayNumberLabel.ForeColor = Colors.Constant;
                DayNumberLabel.Text = "";
                DayNumberLabel.Font = new Font("Calibri", 36);
                DayNumberLabel.Text = "Congratulations!!!\n Your test is complete!";
            }));

            TestIsOn = false;
            TestStarted = false;
            TestControlButton.Invoke((MethodInvoker)(delegate { TestControlButton.Text = "Start"; }));
        }

        private void SetGermanDayToRecognize()
        {
            speechRecognitionEngine = new SpeechRecognitionEngine();
            speechRecognitionEngine.SetInputToDefaultAudioDevice();
            speechRecognitionEngine.SpeechRecognized += SpeechRecognitionEngine_SpeechRecognized;
            GrammarBuilder grammarBuilder = new GrammarBuilder();
            grammarBuilder.Culture = cultureInfo;
            grammarBuilder.Append(GermanDays[Position]);
            Grammar grammar = new Grammar(grammarBuilder);
            speechRecognitionEngine.LoadGrammar(grammar);
            speechRecognitionEngine.Recognize();
        }

        private void SpeechRecognitionEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            DayNumberLabel.Invoke((MethodInvoker)(delegate { DayNumberLabel.ForeColor = Color.Red; }));
            Thread.Sleep(500);
        }

        private void DisposeSpeechSynthesizer()
        {
            if (speechRecognitionEngine != null)
                speechRecognitionEngine.Dispose();
        }
    }
}
