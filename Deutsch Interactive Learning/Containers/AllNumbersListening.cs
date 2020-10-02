using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using Microsoft.Speech.Synthesis;

namespace Deutsch_Interactive_Learning.Containers
{
    class AllNumbersListening : ViewControls.ContainerControls.Container
    {
        private Button[] NumberButtons;
        private Panel NumberPanel;
        private Label NumberLabel;
        private Label NumberWordLabel;
        private Label NumberWordPartsLabel;
        private Button BackToNumbersButton;
        private Button PreviusNumberButton;
        private Button NextNumberButton;

        private string NumberWordsExample = "zehn elf zwölf dreizehn vierzehn fünfzehn sechzehn siebzehn achtzehn neunzehn zwanzig einundzwanzig zweiundzwanzig dreiundzwanzig vierundzwanzig fünfundzwanzig sechsundzwanzig siebenundzwanzig achtundzwanzig neunundzwanzig dreiβig einunddreiβig zweiunddreißig dreiunddreißig vierunddreißig fünfunddreißig sechsunddreißig siebenunddreißig achtunddreißig neununddreißig vierzig einundvierzig zweiundvierzig dreiundvierzig vierundvierzig fünfundvierzig sechsundvierzig siebenundvierzig achtundvierzig neunundvierzig fünfzig einundfünfzig zweiundfünfzig dreiundfünfzig vierundfünfzig fünfundfünfzig sechsundfünfzig siebenundfünfzig achtundfünfzig neunundfünfzig sechzig einundsechzig zweiundsechzig dreiundsechzig vierundsechzig fünfundsechzig sechsundsechzig siebenundsechzig achtundsechzig neunundsechzig siebzig einundsiebzig zweiundsiebzig dreiundsiebzig vierundsiebzig fünfundsiebzig sechsundsiebzig siebenundsiebzig achtundsiebzig neunundsiebzig achtzig einundachtzig zweiundachtzig dreiundachtzig vierundachtzig fünfundachtzig sechsundachtzig siebenundachtzig achtundachtzig neunundachtzig neunzig einundneunzig zweiundneunzig dreiundneunzig vierundneunzig fünfundneunzig sechsundneunzig siebenundneunzig achtundneunzig neunundneunzig einhundert zweihundert dreihundert vierhundert fünfhundert sechshundert siebenhundert achthundert neunhundert tausend";
        private string[] NumberWords;
        private int Position;
        private SpeechSynthesizer speechSynthesizer;

        private FlatDesign.Colors Colors;

        public AllNumbersListening(Form ParentForm, FlatDesign.Colors Colors)
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
                "Listening");

            OnHide = DisposeSpeechSynthersizer;

            NumberWords = NumberWordsExample.Split(' ');

            NumberButtons = new Button[NumberWords.Length];
            int Position = 0;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    NumberButtons[Position] = new Button();
                    NumberButtons[Position].TabIndex = 0;
                    NumberButtons[Position].TabStop = false;
                    if (i == 0 && j == 0)
                        NumberButtons[Position].Location = new Point(12, 13);
                    else if (i > 0 && j == 0)
                        NumberButtons[Position].Location = new Point(NumberButtons[Position - 10].Right + 5, 13);
                    else if (i == 0 && j > 0)
                        NumberButtons[Position].Location = new Point(12, NumberButtons[Position - 1].Bottom + 5);
                    else if (i > 0 && j > 0)
                        NumberButtons[Position].Location = new Point(NumberButtons[Position - 10].Right + 5, NumberButtons[Position - 1].Bottom + 5);
                    NumberButtons[Position].Size = new Size(73, 50);
                    NumberButtons[Position].FlatStyle = FlatStyle.Flat;
                    NumberButtons[Position].BackColor = Colors.Origin;
                    NumberButtons[Position].FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
                    NumberButtons[Position].FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
                    NumberButtons[Position].FlatAppearance.BorderSize = 0;
                    NumberButtons[Position].Font = new Font("Calibri", 16);
                    NumberButtons[Position].ForeColor = Colors.Constant;
                    NumberButtons[Position].TextAlign = ContentAlignment.MiddleCenter;
                    NumberButtons[Position].MouseClick += NumberButton_MouseClick;
                    Position++;
                }
            }
            for (int i = 0; i < 90; i++)
                NumberButtons[i].Text = Convert.ToString(i + 10);
            int Hunderd = 100;
            for (int i = 90; i < 100; i++)
            {
                NumberButtons[i].Text = Convert.ToString(Hunderd);
                Hunderd += 100;
            }

            NumberPanel = new Panel();
            NumberPanel.TabIndex = 0;
            NumberPanel.TabStop = false;
            NumberPanel.Location = new Point(50, 50);
            NumberPanel.Size = new Size(ContainerSize.Width - 100, ContainerSize.Height - 100);
            NumberPanel.BorderStyle = BorderStyle.None;
            NumberPanel.BackColor = Colors.Background;
            NumberPanel.Visible = false;

            BackToNumbersButton = new Button();
            BackToNumbersButton.TabIndex = 0;
            BackToNumbersButton.TabStop = false;
            BackToNumbersButton.Location = new Point(0, 0);
            BackToNumbersButton.Size = new Size(70, 70);
            BackToNumbersButton.FlatStyle = FlatStyle.Flat;
            BackToNumbersButton.BackColor = Colors.Origin;
            BackToNumbersButton.FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
            BackToNumbersButton.FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
            BackToNumbersButton.FlatAppearance.BorderSize = 0;
            BackToNumbersButton.Image = new Bitmap(@"..\..\..\downloads\Icons\BackToNumbers.png");
            BackToNumbersButton.ImageAlign = ContentAlignment.MiddleCenter;
            BackToNumbersButton.MouseClick += BackToNumbersButton_MouseClick;

            NumberLabel = new Label();
            NumberLabel.TabIndex = 0;
            NumberLabel.Location = new Point(70, 0);
            NumberLabel.Size = new Size(NumberPanel.Width - 140, 270);
            NumberLabel.FlatStyle = FlatStyle.Flat;
            NumberLabel.BorderStyle = BorderStyle.None;
            NumberLabel.BackColor = Colors.Background;
            NumberLabel.Font = new Font("Calibri", 160);
            NumberLabel.ForeColor = Colors.Constant;
            NumberLabel.TextAlign = ContentAlignment.MiddleCenter;

            NumberWordLabel = new Label();
            NumberWordLabel.TabIndex = 0;
            NumberWordLabel.Location = new Point(70, NumberLabel.Bottom);
            NumberWordLabel.Size = new Size(NumberPanel.Width - 140, 100);
            NumberWordLabel.FlatStyle = FlatStyle.Flat;
            NumberWordLabel.BorderStyle = BorderStyle.None;
            NumberWordLabel.BackColor = Colors.Background;
            NumberWordLabel.Font = new Font("Calibri", 50);
            NumberWordLabel.ForeColor = Colors.Constant;
            NumberWordLabel.TextAlign = ContentAlignment.MiddleCenter;
            NumberWordLabel.MouseClick += NumberWordLabel_MouseClick;

            NumberWordPartsLabel = new Label();
            NumberWordPartsLabel.TabIndex = 0;
            NumberWordPartsLabel.Location = new Point(0, NumberWordLabel.Bottom);
            NumberWordPartsLabel.Size = new Size(NumberPanel.Width, 100);
            NumberWordPartsLabel.FlatStyle = FlatStyle.Flat;
            NumberWordPartsLabel.BorderStyle = BorderStyle.None;
            NumberWordPartsLabel.BackColor = Colors.Background;
            NumberWordPartsLabel.Font = new Font("Calibri", 40);
            NumberWordPartsLabel.ForeColor = Colors.Constant;
            NumberWordPartsLabel.TextAlign = ContentAlignment.MiddleCenter;
            NumberWordPartsLabel.MouseClick += NumberWordPartsLabel_MouseClick;

            PreviusNumberButton = new Button();
            PreviusNumberButton.TabIndex = 0;
            PreviusNumberButton.TabStop = false;
            PreviusNumberButton.Location = new Point(0, 220);
            PreviusNumberButton.Size = new Size(70, 100);
            PreviusNumberButton.FlatStyle = FlatStyle.Flat;
            PreviusNumberButton.BackColor = Colors.Origin;
            PreviusNumberButton.FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
            PreviusNumberButton.FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
            PreviusNumberButton.FlatAppearance.BorderSize = 0;
            PreviusNumberButton.Image = new Bitmap(@"..\..\..\downloads\Icons\Previus.png");
            PreviusNumberButton.ImageAlign = ContentAlignment.MiddleCenter;
            PreviusNumberButton.MouseClick += PreviusNumberButton_MouseClick;

            NextNumberButton = new Button();
            NextNumberButton.TabIndex = 0;
            NextNumberButton.TabStop = false;
            NextNumberButton.Location = new Point(NumberPanel.Width - 70, 220);
            NextNumberButton.Size = new Size(70, 100);
            NextNumberButton.FlatStyle = FlatStyle.Flat;
            NextNumberButton.BackColor = Colors.Origin;
            NextNumberButton.FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
            NextNumberButton.FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
            NextNumberButton.FlatAppearance.BorderSize = 0;
            NextNumberButton.Image = new Bitmap(@"..\..\..\downloads\Icons\Next.png");
            NextNumberButton.ImageAlign = ContentAlignment.MiddleCenter;
            NextNumberButton.MouseClick += NextNumberButton_MouseClick;

            NumberPanel.Controls.Add(BackToNumbersButton);
            NumberPanel.Controls.Add(NumberLabel);
            NumberPanel.Controls.Add(NumberWordPartsLabel);
            NumberPanel.Controls.Add(NumberWordLabel);
            NumberPanel.Controls.Add(PreviusNumberButton);
            NumberPanel.Controls.Add(NextNumberButton);

            AddControl(NumberPanel);
            for (int i = 0; i < NumberButtons.Length; i++)
                AddControl(NumberButtons[i]);
        }
        
        private void NumberButton_MouseClick(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < NumberButtons.Length; i++)
                NumberButtons[i].Visible = false;

            Button NumberButton = sender as Button;
            NumberPanel.Visible = true;
            NumberLabel.Text = NumberButton.Text;

            Position = int.Parse(NumberButton.Text);
            if (Position >= 10 && Position <= 99)
                Position -= 10;
            else if (Position >= 100 && Position <= 1000)
                Position = 90 + (Position / 100 - 1);

            NumberWordLabel.Text = NumberWords[Position];

            string SeparatedNumberWord = "";
            if (int.Parse(NumberButton.Text) >= 13 && int.Parse(NumberButton.Text) <= 19)
            {
                for (int i = 0; i < NumberWords[Position].Length - 4; i++)
                    SeparatedNumberWord += NumberWords[Position].ToCharArray()[i];
                SeparatedNumberWord += " + zehn";
            }
            else if (int.Parse(NumberButton.Text) > 10 && int.Parse(NumberButton.Text) != 30 && int.Parse(NumberButton.Text) < 100 && int.Parse(NumberButton.Text) % 10 == 0)
            {
                for (int i = 0; i < NumberWords[Position].Length - 3; i++)
                    SeparatedNumberWord += NumberWords[Position].ToCharArray()[i];
                SeparatedNumberWord += " + zig";
            }
            else if (int.Parse(NumberButton.Text) == 30)
            {
                for (int i = 0; i < NumberWords[Position].Length - 3; i++)
                    SeparatedNumberWord += NumberWords[Position].ToCharArray()[i];
                SeparatedNumberWord += " + ßig";
            }
            else if (int.Parse(NumberButton.Text) >= 100 && int.Parse(NumberButton.Text) < 1000 && int.Parse(NumberButton.Text) % 100 == 0)
            {
                for (int i = 0; i < NumberWords[Position].Length - 7; i++)
                    SeparatedNumberWord += NumberWords[Position].ToCharArray()[i];
                SeparatedNumberWord += " + hundert";
            }
            else if (int.Parse(NumberButton.Text.ToCharArray()[1].ToString()) != 0 && int.Parse(NumberButton.Text) != 11 && int.Parse(NumberButton.Text) != 12)
            {
                int IndexOfUnd = 0;
                char[] Chars = NumberWords[Position].ToCharArray();
                for (int i = 0; i < Chars.Length - 3; i++)
                {
                    if (Chars[i] == 'u' && Chars[i + 1] == 'n' && Chars[i + 2] == 'd')
                        IndexOfUnd = i;
                }
                for (int i = 0; i < IndexOfUnd; i++)
                    SeparatedNumberWord += Chars[i];
                SeparatedNumberWord += " + und + ";
                for (int i = IndexOfUnd + 3; i < Chars.Length - 3; i++)
                    SeparatedNumberWord += Chars[i];
                SeparatedNumberWord += " + ";
                for (int i = Chars.Length - 3; i < Chars.Length; i++)
                    SeparatedNumberWord += Chars[i];
                //SeparatedNumberWord += " + zig";
            }

            if (SeparatedNumberWord == "")
            {
                NumberWordPartsLabel.Visible = false;
                NumberWordLabel.Height = 200;
                NumberWordLabel.Font = new Font("Calibri", 100);
            }
            else if (SeparatedNumberWord != "")
            {
                NumberWordPartsLabel.Visible = true;
                NumberWordLabel.Height = 100;
                NumberWordLabel.Font = new Font("Calibri", 50);
            }
            NumberWordPartsLabel.Text = SeparatedNumberWord;

            if (NumberButton.Text == "10")
            {
                PreviusNumberButton.Visible = false;
                NextNumberButton.Visible = true;
            }
            else if (NumberButton.Text == "1000")
            {
                PreviusNumberButton.Visible = true;
                NextNumberButton.Visible = false;
            }
            else
            {
                PreviusNumberButton.Visible = true;
                NextNumberButton.Visible = true;
            }
        }

        private void PreviusNumberButton_MouseClick(object sender, MouseEventArgs e)
        {
            Position--;
            NumberLabel.Text = NumberButtons[Position].Text;
            NumberWordLabel.Text = NumberWords[Position];

            string SeparatedNumberWord = "";
            if (int.Parse(NumberButtons[Position].Text) >= 13 && int.Parse(NumberButtons[Position].Text) <= 19)
            {
                for (int i = 0; i < NumberWords[Position].Length - 4; i++)
                    SeparatedNumberWord += NumberWords[Position].ToCharArray()[i];
                SeparatedNumberWord += " + zehn";
            }
            else if (int.Parse(NumberButtons[Position].Text) > 10 && int.Parse(NumberButtons[Position].Text) != 30 && int.Parse(NumberButtons[Position].Text) < 100 && int.Parse(NumberButtons[Position].Text) % 10 == 0)
            {
                for (int i = 0; i < NumberWords[Position].Length - 3; i++)
                    SeparatedNumberWord += NumberWords[Position].ToCharArray()[i];
                SeparatedNumberWord += " + zig";
            }
            else if (int.Parse(NumberButtons[Position].Text) == 30)
            {
                for (int i = 0; i < NumberWords[Position].Length - 3; i++)
                    SeparatedNumberWord += NumberWords[Position].ToCharArray()[i];
                SeparatedNumberWord += " + ßig";
            }
            else if (int.Parse(NumberButtons[Position].Text) >= 100 && int.Parse(NumberButtons[Position].Text) < 1000 && int.Parse(NumberButtons[Position].Text) % 100 == 0)
            {
                for (int i = 0; i < NumberWords[Position].Length - 7; i++)
                    SeparatedNumberWord += NumberWords[Position].ToCharArray()[i];
                SeparatedNumberWord += " + hundert";
            }
            else if (int.Parse(NumberButtons[Position].Text.ToCharArray()[1].ToString()) != 0 && int.Parse(NumberButtons[Position].Text) != 11 && int.Parse(NumberButtons[Position].Text) != 12)
            {
                int IndexOfUnd = 0;
                char[] Chars = NumberWords[Position].ToCharArray();
                for (int i = 0; i < Chars.Length - 3; i++)
                {
                    if (Chars[i] == 'u' && Chars[i + 1] == 'n' && Chars[i + 2] == 'd')
                        IndexOfUnd = i;
                }
                for (int i = 0; i < IndexOfUnd; i++)
                    SeparatedNumberWord += Chars[i];
                SeparatedNumberWord += " + und + ";
                for (int i = IndexOfUnd + 3; i < Chars.Length - 3; i++)
                    SeparatedNumberWord += Chars[i];
                SeparatedNumberWord += " + ";
                for (int i = Chars.Length - 3; i < Chars.Length; i++)
                    SeparatedNumberWord += Chars[i];
                //SeparatedNumberWord += " + zig";
            }

            if (SeparatedNumberWord == "")
            {
                NumberWordPartsLabel.Visible = false;
                NumberWordLabel.Height = 200;
                NumberWordLabel.Font = new Font("Calibri", 100);
            }
            else if (SeparatedNumberWord != "")
            {
                NumberWordPartsLabel.Visible = true;
                NumberWordLabel.Height = 100;
                NumberWordLabel.Font = new Font("Calibri", 50);
            }
            NumberWordPartsLabel.Text = SeparatedNumberWord;

            if (NumberButtons[Position].Text == "10")
            {
                PreviusNumberButton.Visible = false;
                NextNumberButton.Visible = true;
            }
            else if (NumberButtons[Position].Text == "1000")
            {
                PreviusNumberButton.Visible = true;
                NextNumberButton.Visible = false;
            }
            else
            {
                PreviusNumberButton.Visible = true;
                NextNumberButton.Visible = true;
            }
        }

        private void NextNumberButton_MouseClick(object sender, MouseEventArgs e)
        {
            Position++;
            NumberLabel.Text = NumberButtons[Position].Text;
            NumberWordLabel.Text = NumberWords[Position];

            string SeparatedNumberWord = "";
            if (int.Parse(NumberButtons[Position].Text) >= 13 && int.Parse(NumberButtons[Position].Text) <= 19)
            {
                for (int i = 0; i < NumberWords[Position].Length - 4; i++)
                    SeparatedNumberWord += NumberWords[Position].ToCharArray()[i];
                SeparatedNumberWord += " + zehn";
            }
            else if (int.Parse(NumberButtons[Position].Text) > 10 && int.Parse(NumberButtons[Position].Text) != 30 && int.Parse(NumberButtons[Position].Text) < 100 && int.Parse(NumberButtons[Position].Text) % 10 == 0)
            {
                for (int i = 0; i < NumberWords[Position].Length - 3; i++)
                    SeparatedNumberWord += NumberWords[Position].ToCharArray()[i];
                SeparatedNumberWord += " + zig";
            }
            else if (int.Parse(NumberButtons[Position].Text) == 30)
            {
                for (int i = 0; i < NumberWords[Position].Length - 3; i++)
                    SeparatedNumberWord += NumberWords[Position].ToCharArray()[i];
                SeparatedNumberWord += " + ßig";
            }
            else if (int.Parse(NumberButtons[Position].Text) >= 100 && int.Parse(NumberButtons[Position].Text) < 1000 && int.Parse(NumberButtons[Position].Text) % 100 == 0)
            {
                for (int i = 0; i < NumberWords[Position].Length - 7; i++)
                    SeparatedNumberWord += NumberWords[Position].ToCharArray()[i];
                SeparatedNumberWord += " + hundert";
            }
            else if (int.Parse(NumberButtons[Position].Text.ToCharArray()[1].ToString()) != 0 && int.Parse(NumberButtons[Position].Text) != 11 && int.Parse(NumberButtons[Position].Text) != 12)
            {
                int IndexOfUnd = 0;
                char[] Chars = NumberWords[Position].ToCharArray();
                for (int i = 0; i < Chars.Length - 3; i++)
                {
                    if (Chars[i] == 'u' && Chars[i + 1] == 'n' && Chars[i + 2] == 'd')
                        IndexOfUnd = i;
                }
                for (int i = 0; i < IndexOfUnd; i++)
                    SeparatedNumberWord += Chars[i];
                SeparatedNumberWord += " + und + ";
                for (int i = IndexOfUnd + 3; i < Chars.Length - 3; i++)
                    SeparatedNumberWord += Chars[i];
                SeparatedNumberWord += " + ";
                for (int i = Chars.Length - 3; i < Chars.Length; i++)
                    SeparatedNumberWord += Chars[i];
                //SeparatedNumberWord += " + zig";
            }

            if (SeparatedNumberWord == "")
            {
                NumberWordPartsLabel.Visible = false;
                NumberWordLabel.Height = 200;
                NumberWordLabel.Font = new Font("Calibri", 100);
            }
            else if (SeparatedNumberWord != "")
            {
                NumberWordPartsLabel.Visible = true;
                NumberWordLabel.Height = 100;
                NumberWordLabel.Font = new Font("Calibri", 50);
            }
            NumberWordPartsLabel.Text = SeparatedNumberWord;

            if (NumberButtons[Position].Text == "10")
            {
                PreviusNumberButton.Visible = false;
                NextNumberButton.Visible = true;
            }
            else if (NumberButtons[Position].Text == "1000")
            {
                PreviusNumberButton.Visible = true;
                NextNumberButton.Visible = false;
            }
            else
            {
                PreviusNumberButton.Visible = true;
                NextNumberButton.Visible = true;
            }
        }

        private void NumberWordLabel_MouseClick(object sender, MouseEventArgs e)
        {
            speechSynthesizer = new SpeechSynthesizer();
            speechSynthesizer.SetOutputToDefaultAudioDevice();
            Thread thread = new Thread(() => { speechSynthesizer.Speak(NumberWordLabel.Text); });
            thread.IsBackground = true;
            thread.Start();
        }

        private void NumberWordPartsLabel_MouseClick(object sender, MouseEventArgs e)
        {
            speechSynthesizer = new SpeechSynthesizer();
            speechSynthesizer.SetOutputToDefaultAudioDevice();
            string[] NumberWordParts = NumberWordPartsLabel.Text.Split('+');
            string NumberWordPartsToSpeak = "";
            for (int i = 0; i < NumberWordParts.Length; i++)
                NumberWordPartsToSpeak += NumberWordParts[i] + ". ";
            Thread thread = new Thread(() => { speechSynthesizer.Speak(NumberWordPartsToSpeak); });
            thread.IsBackground = true;
            thread.Start();
        }

        private void BackToNumbersButton_MouseClick(object sender, MouseEventArgs e)
        {
            NumberPanel.Visible = false;
            for (int i = 0; i < NumberButtons.Length; i++)
                NumberButtons[i].Visible = true;
        }

        private void DisposeSpeechSynthersizer()
        {
            if (speechSynthesizer != null)
                speechSynthesizer.Dispose();
        }
    }
}
