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
    class AlphabetTesting : ViewControls.ContainerControls.Container
    {
        private Button TestControlButton;
        private Label LetterLabel;

        private string AlphabetTemplate = "Aa Bb Cc Dd Ee Ff Gg Hh Jj Kk Ll Mm Nn Oo Pp Qq Rr Ss Tt Uu Vv Ww Xx Yy Zz Ää Öö Üü ẞ";
        private string[] Letters;
        private bool TestStarted = false;
        private bool TestIsOn = false;
        private Thread AlphabetTester;
        private SpeechRecognitionEngine speechRecognitionEngine;
        private CultureInfo cultureInfo = new CultureInfo("de-de");

        private FlatDesign.Colors Colors;

        public AlphabetTesting(Form ParentForm, FlatDesign.Colors Colors)
        {
            this.Colors = Colors;
            
            CreateContainer(
                ParentForm,
                Colors,
                "Alphabet",
                "Alphabet testing.",
                new Bitmap(@"..\..\..\downloads\Icons\Alphabet.png"),
                "Origins",
                new Bitmap(@"..\..\..\downloads\Icons\Origins.png"),
                "Testing");
            OnHide = DisposeSpeechInstruments;

            Letters = AlphabetTemplate.Split(' ');

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

            LetterLabel = new Label();
            LetterLabel.TabIndex = 0;
            LetterLabel.Location = new Point(0, TestControlButton.Bottom);
            LetterLabel.Size = new Size(ContainerSize.Width, ContainerSize.Height - (TestControlButton.Height + TestControlButton.Top));
            LetterLabel.BorderStyle = BorderStyle.None;
            LetterLabel.FlatStyle = FlatStyle.Flat;
            LetterLabel.BackColor = Colors.Background;
            LetterLabel.Font = new Font("Calibri light", 200);
            LetterLabel.ForeColor = Colors.Constant;
            LetterLabel.TextAlign = ContentAlignment.MiddleCenter;

            AddControl(TestControlButton);
            AddControl(LetterLabel);
        }

        private void TestControlButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (TestStarted == false && TestIsOn == false)
            {
                TestControlButton.Text = "Pause";
                TestStarted = true;
                TestIsOn = true;
                AlphabetTester = new Thread(Testing);
                AlphabetTester.IsBackground = true;
                AlphabetTester.Start();
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
            LetterLabel.Invoke((MethodInvoker)(delegate { LetterLabel.Font = new Font("Calibri light", 200); }));

            for (int i = 0; i < Letters.Length; i++)
            {
                if (TestIsOn == true)
                {
                    LetterLabel.Invoke((MethodInvoker)(delegate
                    {
                        LetterLabel.ForeColor = Colors.Constant;
                        LetterLabel.Text = Letters[i];
                    }));
                    SetLetterToRecognize(LetterLabel.Text.ToCharArray()[0].ToString());
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

            LetterLabel.Invoke((MethodInvoker)(delegate
            {
                LetterLabel.ForeColor = Colors.Constant;
                LetterLabel.Text = "";
                LetterLabel.Font = new Font("Calibri", 36);
                LetterLabel.Text = "Congratulations!!!\n Your test is complete!";
            }));

            TestIsOn = false;
            TestStarted = false;
            TestControlButton.Invoke((MethodInvoker)(delegate { TestControlButton.Text = "Start"; }));
        }

        private void SetLetterToRecognize(string Letter)
        {
            speechRecognitionEngine = new SpeechRecognitionEngine(cultureInfo);
            speechRecognitionEngine.SetInputToDefaultAudioDevice();
            speechRecognitionEngine.SpeechRecognized += SpeechRecognitionEngine_SpeechRecognized;
            GrammarBuilder grammarBuilder = new GrammarBuilder();
            grammarBuilder.Culture = cultureInfo;
            grammarBuilder.Append(Letter);
            Grammar grammar = new Grammar(grammarBuilder);
            speechRecognitionEngine.LoadGrammar(grammar);
            speechRecognitionEngine.Recognize();
        }

        private void SpeechRecognitionEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            LetterLabel.Invoke((MethodInvoker)(delegate { LetterLabel.ForeColor = Color.Red; }));
            Thread.Sleep(500);
        }

        private void DisposeSpeechInstruments()
        {
            if (speechRecognitionEngine != null)
                speechRecognitionEngine.Dispose();
        }
    }
}
