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
    class ClockTesting : ViewControls.ContainerControls.Container
    {
        private Button TestControlButton;
        private Label ClockLabel;

        private string TimeNumberExample = "07:00 08:30 09:15 09:45 10:00 11:05 13:55 14:02 15:25 16:35 18:40 19:20 21:37 22:00";
        private string[] TimeNumbers;
        private string TimeExpressionExample = "Sieben Uhr|Halb neun|Fünfzehn nach neun|Fünfzehn vor zehn|Zehn Uhr|Fünf nach elf|Fünf vor vierzehn|Zwei nach dreizehn|Fünfundzwanzig nach sechzehn|Fünfundzwanzig vor siebzehn|Zwanzig vor neunzehn|Zwanzig nach zwanzig|Dreiundzwanzig vor einundzwanzig|Zweiundzwanzig Uhr";
        private string[] TimeExpressions;
        private int Position = -1;
        private bool TestStarted = false;
        private bool TestIsOn = false;
        private Thread ClockTester;
        private CultureInfo cultureInfo = new CultureInfo("de-de");
        private SpeechRecognitionEngine speechRecognitionEngine;

        private FlatDesign.Colors Colors;

        public ClockTesting(Form ParentForm, FlatDesign.Colors Colors)
        {
            this.Colors = Colors;

            CreateContainer(
                ParentForm,
                Colors,
                "Clock",
                "",
                null,
                "Words",
                null,
                "Testing");

            OnHide = DisposeSpeechInstruments;

            TimeNumbers = TimeNumberExample.Split(' ');
            TimeExpressions = TimeExpressionExample.Split('|');

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

            ClockLabel = new Label();
            ClockLabel.TabIndex = 0;
            ClockLabel.Location = new Point(0, TestControlButton.Bottom);
            ClockLabel.Size = new Size(ContainerSize.Width, ContainerSize.Height - TestControlButton.Bottom);
            ClockLabel.FlatStyle = FlatStyle.Flat;
            ClockLabel.BorderStyle = BorderStyle.None;
            ClockLabel.BackColor = Colors.Background;
            ClockLabel.Font = new Font("Calibri", 148);
            ClockLabel.ForeColor = Colors.Constant;
            ClockLabel.TextAlign = ContentAlignment.MiddleCenter;

            AddControl(TestControlButton);
            AddControl(ClockLabel);
        }

        private void TestControlButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (TestStarted == false && TestIsOn == false)
            {
                TestControlButton.Text = "Pause";
                TestStarted = true;
                TestIsOn = true;
                ClockTester = new Thread(Testing);
                ClockTester.IsBackground = true;
                ClockTester.Start();
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
            ClockLabel.Invoke((MethodInvoker)(delegate { ClockLabel.Font = new Font("Calibri", 148); }));

            for (int i = 0; i < TimeNumbers.Length; i++)
            {
                if (TestIsOn == true)
                {
                    Position++;
                    ClockLabel.Invoke((MethodInvoker)(delegate
                    {
                        ClockLabel.ForeColor = Colors.Constant;
                        ClockLabel.Text = TimeNumbers[i];
                    }));
                    SetTimeExpresssionToRecognize();
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

            ClockLabel.Invoke((MethodInvoker)(delegate
            {
                ClockLabel.ForeColor = Colors.Constant;
                ClockLabel.Text = "";
                ClockLabel.Font = new Font("Calibri", 36);
                ClockLabel.Text = "Congratulations!!!\n Your test is complete!";
            }));

            TestIsOn = false;
            TestStarted = false;
            TestControlButton.Invoke((MethodInvoker)(delegate { TestControlButton.Text = "Start"; }));
        }

        private void SetTimeExpresssionToRecognize()
        {
            speechRecognitionEngine = new SpeechRecognitionEngine();
            speechRecognitionEngine.SetInputToDefaultAudioDevice();
            speechRecognitionEngine.SpeechRecognized += SpeechRecognitionEngine_SpeechRecognized;
            GrammarBuilder grammarBuilder = new GrammarBuilder();
            grammarBuilder.Culture = cultureInfo;
            grammarBuilder.Append(TimeExpressions[Position]);
            Grammar grammar = new Grammar(grammarBuilder);
            speechRecognitionEngine.LoadGrammar(grammar);
            speechRecognitionEngine.Recognize();
        }

        private void SpeechRecognitionEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            ClockLabel.Invoke((MethodInvoker)(delegate { ClockLabel.ForeColor = Color.Red; }));
            Thread.Sleep(500);
        }

        private void DisposeSpeechInstruments()
        {
            if (speechRecognitionEngine != null)
                speechRecognitionEngine.Dispose();
        }
    }
}
