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
    class AlphabetPracticing : ViewControls.ContainerControls.Container
    {
        private Button[] LetterButtons;
        private Label LetterLabel;
        private Button NextLetterButton;
        private Button PreviusLetterButton;

        private string AlphabetTemplate = "Aa Bb Cc Dd Ee Ff Gg Hh Jj Kk Ll Mm Nn Oo Pp Qq Rr Ss Tt Uu Vv Ww Xx Yy Zz Ää Öö Üü ẞ";
        private int Position = 0;
        private SpeechSynthesizer speechSynthesizer;
        private SpeechRecognitionEngine speechRecognitionEngine;
        private CultureInfo cultureInfo = new CultureInfo("de-de");

        private FlatDesign.Colors Colors;

        public AlphabetPracticing(Form ParentForm, FlatDesign.Colors Colors)
        {
            this.Colors = Colors;
            CreateContainer(
                ParentForm,
                Colors,
                "Alphabet",
                "You can practice your pronuncation of letters. or click on a letter to hear it, and then repeat after dictor.",
                new Bitmap(@"..\..\..\downloads\Icons\Alphabet.png"),
                "Origins",
                new Bitmap(@"..\..\..\downloads\Icons\Origins.png"),
                "Practicing");

            OnHide = DisposeSpeechInstruments;

            string[] Letters = AlphabetTemplate.Split(' ');
            LetterButtons = new Button[Letters.Length];

            for (int i = 0; i < 15; i++)
            {
                LetterButtons[i] = new Button();
                LetterButtons[i].TabIndex = 0;
                LetterButtons[i].TabStop = false;
                if (i == 0)
                    LetterButtons[i].Location = new Point(11, 11);
                else
                    LetterButtons[i].Location = new Point(LetterButtons[i - 1].Right + 2, 11);
                LetterButtons[i].Size = new Size(50, 50);
                LetterButtons[i].FlatStyle = FlatStyle.Flat;
                LetterButtons[i].BackColor = Colors.Origin;
                LetterButtons[i].FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
                LetterButtons[i].FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
                LetterButtons[i].FlatAppearance.BorderSize = 0;
                LetterButtons[i].Font = new Font("Calibri", 14);
                LetterButtons[i].ForeColor = Colors.Constant;
                LetterButtons[i].Text = Letters[i];
                LetterButtons[i].TextAlign = ContentAlignment.MiddleCenter;
                LetterButtons[i].MouseClick += LetterButton_MouseClick;
            }
            for (int i = 15; i < LetterButtons.Length; i++)
            {
                LetterButtons[i] = new Button();
                LetterButtons[i].TabIndex = 0;
                LetterButtons[i].TabStop = false;
                if (i == 15)
                    LetterButtons[i].Location = new Point(36, 63);
                else
                    LetterButtons[i].Location = new Point(LetterButtons[i - 1].Right + 2, 63);
                LetterButtons[i].Size = new Size(50, 50);
                LetterButtons[i].FlatStyle = FlatStyle.Flat;
                LetterButtons[i].BackColor = Colors.Origin;
                LetterButtons[i].FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
                LetterButtons[i].FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
                LetterButtons[i].FlatAppearance.BorderSize = 0;
                LetterButtons[i].Font = new Font("Calibri", 14);
                LetterButtons[i].ForeColor = Colors.Constant;
                LetterButtons[i].Text = Letters[i];
                LetterButtons[i].TextAlign = ContentAlignment.MiddleCenter;
                LetterButtons[i].MouseClick += LetterButton_MouseClick;
            }

            LetterLabel = new Label();
            LetterLabel.TabIndex = 0;
            LetterLabel.Location = new Point(50, 115);
            LetterLabel.Size = new Size(ContainerSize.Width - 100, ContainerSize.Height - 117);
            LetterLabel.BorderStyle = BorderStyle.None;
            LetterLabel.FlatStyle = FlatStyle.Flat;
            LetterLabel.BackColor = Colors.Background;
            LetterLabel.Font = new Font("Calibri light", 200);
            LetterLabel.ForeColor = Colors.Constant;
            LetterLabel.TextAlign = ContentAlignment.MiddleCenter;
            LetterLabel.MouseClick += LetterLabel_MouseClick;

            NextLetterButton = new Button();
            NextLetterButton.TabIndex = 0;
            NextLetterButton.TabStop = false;
            NextLetterButton.Size = new Size(70, 100);
            NextLetterButton.Location = new Point(LetterLabel.Width - NextLetterButton.Width, LetterLabel.Height / 2 - NextLetterButton.Height / 2);
            NextLetterButton.FlatStyle = FlatStyle.Flat;
            NextLetterButton.BackColor = Colors.Origin;
            NextLetterButton.FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
            NextLetterButton.FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
            NextLetterButton.FlatAppearance.BorderSize = 0;
            NextLetterButton.Image = new Bitmap(@"..\..\..\downloads\Icons\Next.png");
            NextLetterButton.ImageAlign = ContentAlignment.MiddleCenter;
            NextLetterButton.Visible = false;
            NextLetterButton.MouseClick += NextLetterButton_MouseClick;

            PreviusLetterButton = new Button();
            PreviusLetterButton.TabIndex = 0;
            PreviusLetterButton.TabStop = false;
            PreviusLetterButton.Size = new Size(70, 100);
            PreviusLetterButton.Location = new Point(0, LetterLabel.Height / 2 - PreviusLetterButton.Height / 2);
            PreviusLetterButton.FlatStyle = FlatStyle.Flat;
            PreviusLetterButton.BackColor = Colors.Origin;
            PreviusLetterButton.FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
            PreviusLetterButton.FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
            PreviusLetterButton.FlatAppearance.BorderSize = 0;
            PreviusLetterButton.Image = new Bitmap(@"..\..\..\downloads\Icons\Previus.png");
            PreviusLetterButton.ImageAlign = ContentAlignment.MiddleCenter;
            PreviusLetterButton.Visible = false;
            PreviusLetterButton.MouseClick += PreviusLetterButton_MouseClick;

            LetterLabel.Controls.Add(NextLetterButton);
            LetterLabel.Controls.Add(PreviusLetterButton);

            for (int i = 0; i < LetterButtons.Length; i++)
                AddControl(LetterButtons[i]);
            AddControl(LetterLabel);
        }

        private void LetterLabel_MouseClick(object sender, MouseEventArgs e)
        {
            speechSynthesizer = new SpeechSynthesizer();
            speechSynthesizer.SetOutputToDefaultAudioDevice();

            LetterLabel.ForeColor = Colors.Constant;
            if (LetterLabel.Text == "ẞ")
                speechSynthesizer.Speak("Sz");
            else
                speechSynthesizer.Speak(LetterLabel.Text.ToCharArray()[0].ToString());

            SetLetterToRecognize(LetterLabel.Text.ToCharArray()[0].ToString());
        }

        private void NextLetterButton_MouseClick(object sender, MouseEventArgs e)
        {
            LetterLabel.ForeColor = Colors.Constant;
            if (Position < LetterButtons.Length - 1)
            {
                Position++;
                LetterLabel.Text = LetterButtons[Position].Text;
            }
            NextLetterButton.Visible = true;
            PreviusLetterButton.Visible = true;
            if (Position == LetterButtons.Length - 1)
                NextLetterButton.Visible = false;
        }

        private void PreviusLetterButton_MouseClick(object sender, MouseEventArgs e)
        {
            LetterLabel.ForeColor = Colors.Constant;
            if (Position > 0)
            {
                Position--;
                LetterLabel.Text = LetterButtons[Position].Text;
            }
            NextLetterButton.Visible = true;
            PreviusLetterButton.Visible = true;
            if (Position == 0)
                PreviusLetterButton.Visible = false;
        }

        private void LetterButton_MouseClick(object sender, MouseEventArgs e)
        {
            LetterLabel.ForeColor = Colors.Constant;
            Button LetterButton = sender as Button;
            for (int i = 0; i < LetterButtons.Length; i++)
            {
                if (LetterButtons[i].Text == LetterButton.Text)
                    Position = i;
            }
            LetterLabel.Text = LetterButton.Text;
            if (Position > 0 && Position < LetterButtons.Length - 1)
            {
                NextLetterButton.Visible = true;
                PreviusLetterButton.Visible = true;
            }
            else if (Position == 0)
            {
                NextLetterButton.Visible = true;
                PreviusLetterButton.Visible = false;
            }
            else if (Position == LetterButtons.Length - 1)
            {
                NextLetterButton.Visible = false;
                PreviusLetterButton.Visible = true;
            }
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
            speechRecognitionEngine.RecognizeAsync(RecognizeMode.Multiple);
        }

        private void SpeechRecognitionEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence * 100 >= 90)
                LetterLabel.ForeColor = Color.Gold;
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
