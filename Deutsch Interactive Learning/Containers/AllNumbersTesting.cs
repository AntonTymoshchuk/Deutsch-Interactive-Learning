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
    class AllNumbersTesting : ViewControls.ContainerControls.Container
    {
        private Button TestControlButton;
        private Label NumberLabel;

        private bool TestStarted = false;
        private bool TestIsOn = false;
        private string NumberWordsExample = "zehn elf zwölf dreizehn vierzehn fünfzehn sechzehn siebzehn achtzehn neunzehn zwanzig einundzwanzig zweiundzwanzig dreiundzwanzig vierundzwanzig fünfundzwanzig sechsundzwanzig siebenundzwanzig achtundzwanzig neunundzwanzig dreiβig einunddreiβig zweiunddreißig dreiunddreißig vierunddreißig fünfunddreißig sechsunddreißig siebenunddreißig achtunddreißig neununddreißig vierzig einundvierzig zweiundvierzig dreiundvierzig vierundvierzig fünfundvierzig sechsundvierzig siebenundvierzig achtundvierzig neunundvierzig fünfzig einundfünfzig zweiundfünfzig dreiundfünfzig vierundfünfzig fünfundfünfzig sechsundfünfzig siebenundfünfzig achtundfünfzig neunundfünfzig sechzig einundsechzig zweiundsechzig dreiundsechzig vierundsechzig fünfundsechzig sechsundsechzig siebenundsechzig achtundsechzig neunundsechzig siebzig einundsiebzig zweiundsiebzig dreiundsiebzig vierundsiebzig fünfundsiebzig sechsundsiebzig siebenundsiebzig achtundsiebzig neunundsiebzig achtzig einundachtzig zweiundachtzig dreiundachtzig vierundachtzig fünfundachtzig sechsundachtzig siebenundachtzig achtundachtzig neunundachtzig neunzig einundneunzig zweiundneunzig dreiundneunzig vierundneunzig fünfundneunzig sechsundneunzig siebenundneunzig achtundneunzig neunundneunzig einhundert zweihundert dreihundert vierhundert fünfhundert sechshundert siebenhundert achthundert neunhundert tausend";
        private string[] NumberWords;
        private CultureInfo cultureInfo = new CultureInfo("de-de");
        private SpeechRecognitionEngine speechRecognitionEngine;

        private FlatDesign.Colors Colors;

        public AllNumbersTesting(Form ParentForm, FlatDesign.Colors Colors)
        {
            this.Colors = Colors;

            CreateContainer(
                ParentForm,
                Colors,
                "All numbers",
                "",
                new Bitmap(@"..\..\..\downloads\Icons\AllNumbers.png"),
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
                Thread thread = new Thread(() => { Testing(); });
                thread.Start();
                
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
            NumberLabel.ForeColor = Colors.Constant;
            NumberLabel.Font = new Font("Calibri", 200);

            for (int i = 0; i < 90; i++)
            {
                if (TestIsOn == true)
                {
                    NumberLabel.Invoke((MethodInvoker)(delegate
                    {
                        NumberLabel.ForeColor = Colors.Constant;
                        NumberLabel.Text = Convert.ToString(i + 10);
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

            int Number = 200;
            for (int i = 90; i < 100; i++)
            {
                if (TestIsOn == true)
                {
                    NumberLabel.ForeColor = Colors.Constant;
                    NumberLabel.Text = Convert.ToString(Number);
                    Number += 100;
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
            NumberLabel.ForeColor = Colors.Constant;
            NumberLabel.Font = new Font("Calibri", 36);
            NumberLabel.Text = "Congratulations!!!\n Your test is complete!";

            TestIsOn = false;
            TestStarted = false;
            TestControlButton.Text = "Start";
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
            speechRecognitionEngine.RecognizeAsync(RecognizeMode.Multiple);
        }

        private void SpeechRecognitionEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence >= 80)
            {
                NumberLabel.ForeColor = Color.Gold;
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
