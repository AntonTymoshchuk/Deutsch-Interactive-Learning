using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using System.Globalization;
using Microsoft.Speech.Synthesis;
using Microsoft.Speech.Recognition;

namespace Deutsch_Interactive_Learning.Containers
{
    class LetterCombinationsTesting : ViewControls.ContainerControls.Container
    {
        private Button TestControlButton;
        private Panel LetterCombinationPanel;
        private Label LetterCombinationLabel;
        private Label LetterCombinationExampleWordLabel;
        private Label TranslatedExampleWordLabel;

        private bool TestStarted = false;
        private bool TestIsOn = false;
        private Thread LetterCombinationsTester;
        private string LetterCombinationsExample = "ch ch chs ck ph qu sch sp st tsch ig aa ah ee eh ä äh ie ih ieh äu eu";
        private string[] LetterCombinations;
        private string LetterCombinationExampleWordsExample = "nach|mich|wachsen|wecken|Ralph|Quelle|Schule|Sport|Stunde|deutsch|lustig|Staat|zahlen|See|sehr|spät|wählen|sie|ihm|sieht|Säule|Freund";
        private string[] LetterCombinationExampleWords;
        private string TranslatedExampleWordsExample = "to|me|to grow|wake up|Ralph|source|school|sport|hour|German|funny|State|pay|lake|very|late|choose|she|him|looks|pillar|friend";
        private string[] TranslatedExampleWords;
        private CultureInfo cultureInfo = new CultureInfo("de-de");
        private SpeechRecognitionEngine speechRecognitionEngine;

        private FlatDesign.Colors Colors;

        public LetterCombinationsTesting(Form ParentForm, FlatDesign.Colors Colors)
        {
            this.Colors = Colors;

            CreateContainer(
                ParentForm,
                Colors,
                "LetterCombinations",
                "",
                new Bitmap(@"..\..\..\downloads\Icons\LetterCombinations.png"),
                "Origins",
                new Bitmap(@"..\..\..\downloads\Icons\Origins.png"),
                "Testing");

            OnHide = DisposeSpeechInstruments;

            LetterCombinations = LetterCombinationsExample.Split(' ');
            LetterCombinationExampleWords = LetterCombinationExampleWordsExample.Split('|');
            TranslatedExampleWords = TranslatedExampleWordsExample.Split('|');

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

            LetterCombinationPanel = new Panel();
            LetterCombinationPanel.TabIndex = 0;
            LetterCombinationPanel.TabStop = false;
            LetterCombinationPanel.Location = new Point(90, TestControlButton.Bottom + 12);
            LetterCombinationPanel.Size = new Size(ContainerSize.Width - 180, 400);
            LetterCombinationPanel.BorderStyle = BorderStyle.None;
            LetterCombinationPanel.BackColor = Colors.Background;
            LetterCombinationPanel.Visible = false;

            LetterCombinationLabel = new Label();
            LetterCombinationLabel.TabIndex = 0;
            LetterCombinationLabel.Location = new Point(0, 0);
            LetterCombinationLabel.Size = new Size(LetterCombinationPanel.Width, 300);
            LetterCombinationLabel.BorderStyle = BorderStyle.None;
            LetterCombinationLabel.BackColor = Colors.Background;
            LetterCombinationLabel.Font = new Font("Calibri", 180);
            LetterCombinationLabel.ForeColor = Colors.Constant;
            LetterCombinationLabel.TextAlign = ContentAlignment.MiddleCenter;

            LetterCombinationExampleWordLabel = new Label();
            LetterCombinationExampleWordLabel.TabIndex = 0;
            LetterCombinationExampleWordLabel.Location = new Point(70, LetterCombinationLabel.Bottom);
            LetterCombinationExampleWordLabel.Size = new Size((LetterCombinationPanel.Width - 70) / 2, 100);
            LetterCombinationExampleWordLabel.BorderStyle = BorderStyle.None;
            LetterCombinationExampleWordLabel.BackColor = Colors.Background;
            LetterCombinationExampleWordLabel.Font = new Font("Calibri", 38);
            LetterCombinationExampleWordLabel.ForeColor = Colors.Constant;
            LetterCombinationExampleWordLabel.TextAlign = ContentAlignment.MiddleLeft;

            TranslatedExampleWordLabel = new Label();
            TranslatedExampleWordLabel.TabIndex = 0;
            TranslatedExampleWordLabel.Location = new Point(LetterCombinationExampleWordLabel.Right, LetterCombinationLabel.Bottom);
            TranslatedExampleWordLabel.Size = new Size((LetterCombinationPanel.Width - 70) / 2, 100);
            TranslatedExampleWordLabel.BorderStyle = BorderStyle.None;
            TranslatedExampleWordLabel.BackColor = Colors.Background;
            TranslatedExampleWordLabel.Font = new Font("Calibri", 38);
            TranslatedExampleWordLabel.ForeColor = Colors.Constant;
            TranslatedExampleWordLabel.TextAlign = ContentAlignment.MiddleLeft;

            LetterCombinationPanel.Controls.Add(LetterCombinationLabel);
            LetterCombinationPanel.Controls.Add(LetterCombinationExampleWordLabel);
            LetterCombinationPanel.Controls.Add(TranslatedExampleWordLabel);

            AddControl(TestControlButton);
            AddControl(LetterCombinationPanel);
        }

        private void TestControlButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (TestStarted == false && TestIsOn == false)
            {
                TestControlButton.Text = "Pause";
                TestStarted = true;
                TestIsOn = true;
                LetterCombinationsTester = new Thread(Testing);
                LetterCombinationsTester.IsBackground = true;
                LetterCombinationsTester.Start();
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
            LetterCombinationPanel.Invoke((MethodInvoker)(delegate { LetterCombinationPanel.Visible = true; }));
            LetterCombinationLabel.Invoke((MethodInvoker)(delegate { LetterCombinationLabel.Font = new Font("Calibri", 180); }));

            for (int i = 0; i < LetterCombinations.Length; i++)
            {
                if (TestIsOn == true)
                {
                    LetterCombinationLabel.Invoke((MethodInvoker)(delegate
                    {
                        LetterCombinationLabel.ForeColor = Colors.Constant;
                        LetterCombinationLabel.Text = LetterCombinations[i];
                    }));
                    LetterCombinationExampleWordLabel.Invoke((MethodInvoker)(delegate
                    {
                        LetterCombinationExampleWordLabel.ForeColor = Colors.Constant;
                        LetterCombinationExampleWordLabel.Text = LetterCombinationExampleWords[i];
                    }));
                    TranslatedExampleWordLabel.Invoke((MethodInvoker)(delegate { TranslatedExampleWordLabel.Text = TranslatedExampleWords[i]; }));
                    SetExampleWordToRecognize(LetterCombinationExampleWords[i]);
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

            LetterCombinationLabel.Invoke((MethodInvoker)(delegate
            {
                LetterCombinationLabel.ForeColor = Colors.Constant;
                LetterCombinationLabel.Font = new Font("Calibri", 36);
                LetterCombinationLabel.Text = "Congratulations!!!\n Your test is complete!";
            }));
            LetterCombinationExampleWordLabel.Invoke((MethodInvoker)(delegate
            {
                LetterCombinationExampleWordLabel.ForeColor = Colors.Constant;
                LetterCombinationExampleWordLabel.Text = "";
            }));
            TranslatedExampleWordLabel.Invoke((MethodInvoker)(delegate { TranslatedExampleWordLabel.Text = ""; }));

            TestIsOn = false;
            TestStarted = false;
            TestControlButton.Invoke((MethodInvoker)(delegate { TestControlButton.Text = "Start"; }));
        }

        private void SetExampleWordToRecognize(string Word)
        {
            speechRecognitionEngine = new SpeechRecognitionEngine(cultureInfo);
            speechRecognitionEngine.SetInputToDefaultAudioDevice();
            speechRecognitionEngine.SpeechRecognized += SpeechRecognitionEngine_SpeechRecognized;
            GrammarBuilder grammarBuilder = new GrammarBuilder();
            grammarBuilder.Culture = cultureInfo;
            grammarBuilder.Append(Word);
            Grammar grammar = new Grammar(grammarBuilder);
            speechRecognitionEngine.LoadGrammar(grammar);
            speechRecognitionEngine.Recognize();
        }

        private void SpeechRecognitionEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            LetterCombinationLabel.Invoke((MethodInvoker)(delegate { LetterCombinationLabel.ForeColor = Color.Red; }));
            LetterCombinationExampleWordLabel.Invoke((MethodInvoker)(delegate { LetterCombinationExampleWordLabel.ForeColor = Color.Red; }));
            Thread.Sleep(500);
        }

        private void DisposeSpeechInstruments()
        {
            if (speechRecognitionEngine != null)
                speechRecognitionEngine.Dispose();
        }
    }
}
