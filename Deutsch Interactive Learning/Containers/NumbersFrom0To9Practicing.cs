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
    class NumbersFrom0To9Practicing : ViewControls.ContainerControls.Container
    {
        private Button[] NumberButtons;
        private Panel NumberPanel;
        private Label NumberLabel;
        private Label NumberWordLabel;
        private Button NextNumberButton;
        private Button PreviusNumberButton;

        private string NumberWordsExample = "Null Eins Zwei Drei Vier Fünf Sechs Sieben Acht Neun";
        private string[] NumberWords;
        private int Position = 0;
        private CultureInfo cultureInfo = new CultureInfo("de-de");
        private SpeechRecognitionEngine speechRecognitionEngine;
        private SpeechSynthesizer speechSynthesizer;

        private FlatDesign.Colors Colors;

        public NumbersFrom0To9Practicing(Form ParentForm, FlatDesign.Colors Colors)
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
                "Practicing");

            OnHide = DisposeSpeechInstruments;

            NumberWords = NumberWordsExample.Split(' ');

            NumberButtons = new Button[10];
            for (int i = 0; i < 10; i++)
            {
                NumberButtons[i] = new Button();
                NumberButtons[i].TabIndex = 0;
                NumberButtons[i].TabStop = false;
                if (i == 0)
                    NumberButtons[i].Location = new Point(10, 10);
                else
                    NumberButtons[i].Location = new Point(NumberButtons[i - 1].Right + 10, 10);
                NumberButtons[i].Size = new Size(69, 69);
                NumberButtons[i].FlatStyle = FlatStyle.Flat;
                NumberButtons[i].BackColor = Colors.Origin;
                NumberButtons[i].FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
                NumberButtons[i].FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
                NumberButtons[i].FlatAppearance.BorderSize = 0;
                NumberButtons[i].Font = new Font("Calibri", 24);
                NumberButtons[i].ForeColor = Colors.Constant;
                NumberButtons[i].Text = Convert.ToString(i);
                NumberButtons[i].TextAlign = ContentAlignment.MiddleCenter;
                NumberButtons[i].MouseClick += NumberButton_MouseClick;
            }

            NumberPanel = new Panel();
            NumberPanel.TabIndex = 0;
            NumberPanel.TabStop = false;
            NumberPanel.Location = new Point(50, NumberButtons[0].Bottom + 10);
            NumberPanel.Size = new Size(ContainerSize.Width - 100, ContainerSize.Height - (NumberButtons[0].Height + 20));
            NumberPanel.BorderStyle = BorderStyle.None;
            NumberPanel.BackColor = Colors.Background;
            NumberPanel.Visible = false;

            NumberLabel = new Label();
            NumberLabel.TabIndex = 0;
            NumberLabel.Location = new Point(0, 0);
            NumberLabel.Size = new Size(NumberPanel.Width, 300);
            NumberLabel.BorderStyle = BorderStyle.None;
            NumberLabel.BackColor = Colors.Background;
            NumberLabel.Font = new Font("Calibri", 160);
            NumberLabel.ForeColor = Colors.Constant;
            NumberLabel.TextAlign = ContentAlignment.BottomCenter;

            NumberWordLabel = new Label();
            NumberWordLabel.TabIndex = 0;
            NumberWordLabel.Location = new Point(0, NumberLabel.Bottom);
            NumberWordLabel.Size = new Size(NumberPanel.Width, NumberPanel.Height - NumberLabel.Height);
            NumberWordLabel.BorderStyle = BorderStyle.None;
            NumberWordLabel.BackColor = Colors.Background;
            NumberWordLabel.Font = new Font("Calibri", 80);
            NumberWordLabel.ForeColor = Colors.Constant;
            NumberWordLabel.TextAlign = ContentAlignment.TopCenter;
            NumberWordLabel.MouseClick += NumberWordLabel_MouseClick;

            NextNumberButton = new Button();
            NextNumberButton.TabIndex = 0;
            NextNumberButton.TabStop = false;
            NextNumberButton.Size = new Size(70, 100);
            NextNumberButton.Location = new Point(NumberPanel.Width - NextNumberButton.Width, NumberPanel.Height / 2 - NextNumberButton.Height / 2);
            NextNumberButton.FlatStyle = FlatStyle.Flat;
            NextNumberButton.BackColor = Colors.Origin;
            NextNumberButton.FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
            NextNumberButton.FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
            NextNumberButton.FlatAppearance.BorderSize = 0;
            NextNumberButton.Image = new Bitmap(@"..\..\..\downloads\Icons\Next.png");
            NextNumberButton.ImageAlign = ContentAlignment.MiddleCenter;
            NextNumberButton.MouseClick += NextNumberButton_MouseClick;

            PreviusNumberButton = new Button();
            PreviusNumberButton.TabIndex = 0;
            PreviusNumberButton.TabStop = false;
            PreviusNumberButton.Size = new Size(70, 100);
            PreviusNumberButton.Location = new Point(0, NumberPanel.Height / 2 - PreviusNumberButton.Height / 2);
            PreviusNumberButton.FlatStyle = FlatStyle.Flat;
            PreviusNumberButton.BackColor = Colors.Origin;
            PreviusNumberButton.FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
            PreviusNumberButton.FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
            PreviusNumberButton.FlatAppearance.BorderSize = 0;
            PreviusNumberButton.Image = new Bitmap(@"..\..\..\downloads\Icons\Previus.png");
            PreviusNumberButton.ImageAlign = ContentAlignment.MiddleCenter;
            PreviusNumberButton.MouseClick += PreviusNumberButton_MouseClick;

            NumberLabel.Controls.Add(NextNumberButton);
            NumberLabel.Controls.Add(PreviusNumberButton);

            NumberPanel.Controls.Add(NumberLabel);
            NumberPanel.Controls.Add(NumberWordLabel);

            for (int i = 0; i < 10; i++)
                AddControl(NumberButtons[i]);
            AddControl(NumberPanel);
        }

        private void NextNumberButton_MouseClick(object sender, MouseEventArgs e)
        {
            NumberLabel.ForeColor = Colors.Constant;
            NumberWordLabel.ForeColor = Colors.Constant;

            if (Position < NumberButtons.Length - 1)
            {
                Position++;
                NumberLabel.Text = NumberButtons[Position].Text;
                NumberWordLabel.Text = NumberWords[Position];
            }
            NextNumberButton.Visible = true;
            PreviusNumberButton.Visible = true;
            if (Position == NumberButtons.Length - 1)
                NextNumberButton.Visible = false;
        }

        private void PreviusNumberButton_MouseClick(object sender, MouseEventArgs e)
        {
            NumberLabel.ForeColor = Colors.Constant;
            NumberWordLabel.ForeColor = Colors.Constant;

            if (Position > 0)
            {
                Position--;
                NumberLabel.Text = NumberButtons[Position].Text;
                NumberWordLabel.Text = NumberWords[Position];
            }
            NextNumberButton.Visible = true;
            PreviusNumberButton.Visible = true;
            if (Position == 0)
                PreviusNumberButton.Visible = false;
        }

        private void NumberButton_MouseClick(object sender, MouseEventArgs e)
        {
            NumberLabel.ForeColor = Colors.Constant;
            NumberWordLabel.ForeColor = Colors.Constant;

            Button NumberButton = sender as Button;
            NumberPanel.Visible = true;
            NumberLabel.Text = NumberButton.Text;
            NumberWordLabel.Text = NumberWords[int.Parse(NumberButton.Text)];
            Position = int.Parse(NumberButton.Text);

            if (Position > 0 && Position < NumberButtons.Length - 1)
            {
                NextNumberButton.Visible = true;
                PreviusNumberButton.Visible = true;
            }
            else if (Position == 0)
            {
                NextNumberButton.Visible = true;
                PreviusNumberButton.Visible = false;
            }
            else if (Position == NumberButtons.Length - 1)
            {
                NextNumberButton.Visible = false;
                PreviusNumberButton.Visible = true;
            }
        }

        private void NumberWordLabel_MouseClick(object sender, MouseEventArgs e)
        {
            speechSynthesizer = new SpeechSynthesizer();
            speechSynthesizer.SetOutputToDefaultAudioDevice();
            speechSynthesizer.Speak(NumberButtons[Position].Text);
            SetNumberToRecognize();
        }

        private void SetNumberToRecognize()
        {
            speechRecognitionEngine = new SpeechRecognitionEngine(cultureInfo);
            speechRecognitionEngine.SetInputToDefaultAudioDevice();
            speechRecognitionEngine.SpeechRecognized += SpeechRecognitionEngine_SpeechRecognized;
            GrammarBuilder grammarBuilder = new GrammarBuilder();
            grammarBuilder.Culture = cultureInfo;
            grammarBuilder.Append(NumberWords[Position]);
            Grammar grammar = new Grammar(grammarBuilder);
            speechRecognitionEngine.LoadGrammar(grammar);
            speechRecognitionEngine.RecognizeAsync(RecognizeMode.Multiple);
        }

        private void SpeechRecognitionEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence * 100 >= 90)
            {
                NumberLabel.ForeColor = Color.Gold;
                NumberWordLabel.ForeColor = Color.Gold;
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
