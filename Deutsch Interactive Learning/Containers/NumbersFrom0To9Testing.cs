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

namespace Deutsch_Interactive_Learning.Containers
{
    class NumbersFrom0To9Testing : ViewControls.ContainerControls.Container
    {
        private Button TestControlButton;
        private Label NumberLabel;

        private bool TestStarted = false;
        private bool TestIsOn = false;
        private Thread NumbersTester;
        private string NumberWordsExample = "Null Eins Zwei Drei Vier Fünf Sechs Sieben Acht Neun";
        private string[] NumberWords;
        private CultureInfo cultureInfo = new CultureInfo("de-de");
        private SpeechRecognitionEngine speechRecognitionEngine;

        private FlatDesign.Colors Colors;

        public NumbersFrom0To9Testing(Form ParentForm, FlatDesign.Colors Colors)
        {
            this.Colors = Colors;

            CreateContainer(
                ParentForm,
                Colors,
                "Numbers from 0 to 9",
                "",
                new Bitmap(@"..\..\..\downloads\Icons\NumbersFrom0To9.png"),
                "Origins",
                new Bitmap(@"..\..\..\downloads\Icons\Origins.png"),
                "Testing");

            OnHide = DisposeSpeechRecognitionEngine;

            NumberWords = NumberWordsExample.Split(' ');

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

            NumberLabel = new Label();
            NumberLabel.TabIndex = 0;
            NumberLabel.Location = new Point(20, TestControlButton.Bottom + 20);
            NumberLabel.Size = new Size(ContainerSize.Width - 40, ContainerSize.Height - TestControlButton.Height - 60);
            NumberLabel.BorderStyle = BorderStyle.None;
            NumberLabel.BackColor = Colors.Background;
            NumberLabel.Font = new Font("Calibri", 200);
            NumberLabel.ForeColor = Colors.Constant;
            NumberLabel.TextAlign = ContentAlignment.MiddleCenter;

            AddControl(TestControlButton);
            AddControl(NumberLabel);
        }

        private void TestControlButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (TestStarted == false && TestIsOn == false)
            {
                TestControlButton.Text = "Pause";
                TestStarted = true;
                TestIsOn = true;
                NumbersTester = new Thread(Testing);
                NumbersTester.IsBackground = true;
                NumbersTester.Start();
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
            NumberLabel.Invoke((MethodInvoker)(delegate
            {
                NumberLabel.ForeColor = Colors.Constant;
                NumberLabel.Font = new Font("Calibri", 200);
            }));

            for (int i = 0; i < 10; i++)
            {
                if (TestIsOn == true)
                {
                    NumberLabel.Invoke((MethodInvoker)(delegate
                    {
                        NumberLabel.ForeColor = Colors.Constant;
                        NumberLabel.Text = Convert.ToString(i);
                    }));
                    SetNumberToRecognize(NumberWords[i]);
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

            NumberLabel.Invoke((MethodInvoker)(delegate
            {
                NumberLabel.ForeColor = Colors.Constant;
                NumberLabel.Font = new Font("Calibri", 36);
                NumberLabel.Text = "Congratulations!!!\n Your test is complete!";
            }));

            TestIsOn = false;
            TestStarted = false;
            TestControlButton.Invoke((MethodInvoker)(delegate { TestControlButton.Text = "Start"; }));
        }

        private void SetNumberToRecognize(string Number)
        {
            speechRecognitionEngine = new SpeechRecognitionEngine(cultureInfo);
            speechRecognitionEngine.SetInputToDefaultAudioDevice();
            speechRecognitionEngine.SpeechRecognized += SpeechRecognitionEngine_SpeechRecognized;
            GrammarBuilder grammarBuilder = new GrammarBuilder();
            grammarBuilder.Culture = cultureInfo;
            grammarBuilder.Append(Number);
            Grammar grammar = new Grammar(grammarBuilder);
            speechRecognitionEngine.LoadGrammar(grammar);
            speechRecognitionEngine.Recognize();
        }

        private void SpeechRecognitionEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            NumberLabel.Invoke((MethodInvoker)(delegate { NumberLabel.ForeColor = Color.Red; }));
            Thread.Sleep(500);
        }

        private void DisposeSpeechRecognitionEngine()
        {
            if (speechRecognitionEngine != null)
                speechRecognitionEngine.Dispose();
        }
    }
}
