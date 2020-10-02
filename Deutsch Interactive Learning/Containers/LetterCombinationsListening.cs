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
    class LetterCombinationsListening : ViewControls.ContainerControls.Container
    {
        private List<Button> LetterCombinationButtons;
        private Panel LetterCombinationPanel;
        private Label LetterCombinationLabel;
        private Label LetterCombinationExampleWordLabel;
        private Label TranslatedExampleWordLabel;

        private string LetterCombinationsExample = "ch ch chs ck ph qu sch sp st tsch ig aa ah ee eh ä äh ie ih ieh äu eu";
        private string LetterCombinationExampleWordsExample = "nach|mich|wachsen|wecken|Ralph|Quelle|Schule|Sport|Stunde|deutsch|lustig|Staat|zahlen|See|sehr|spät|wählen|sie|ihm|sieht|Säule|Freund";
        private string[] LetterCombinationExampleWords;
        private string TranslatedExampleWordsExample = "to|me|to grow|wake up|Ralph|source|school|sport|hour|German|funny|State|pay|lake|very|late|choose|she|him|looks|pillar|friend";
        private string[] TranslatedExampleWords;
        private SpeechSynthesizer speechSynthesizer;

        private FlatDesign.Colors Colors;

        public LetterCombinationsListening(Form ParentForm, FlatDesign.Colors Colors)
        {
            this.Colors = Colors;

            CreateContainer(
                ParentForm,
                Colors,
                "Letter combinations",
                "",
                new Bitmap(@"..\..\..\downloads\Icons\LetterCombinations.png"),
                "Origins",
                new Bitmap(@"..\..\..\downloads\Icons\Origins.png"),
                "Listening");

            OnHide = DisposeSpeechSynthesizer;

            string[] LetterCombinations = LetterCombinationsExample.Split(' ');
            LetterCombinationExampleWords = LetterCombinationExampleWordsExample.Split('|');
            TranslatedExampleWords = TranslatedExampleWordsExample.Split('|');

            LetterCombinationButtons = new List<Button>();
            for (int i = 0; i < LetterCombinations.Length / 2; i++)
            {
                LetterCombinationButtons.Add(new Button());
                LetterCombinationButtons[i].TabIndex = 0;
                LetterCombinationButtons[i].TabStop = false;
                if (i == 0)
                    LetterCombinationButtons[i].Location = new Point(10, 12);
                else
                    LetterCombinationButtons[i].Location = new Point(10, LetterCombinationButtons[i - 1].Bottom + 2);
                LetterCombinationButtons[i].Size = new Size(70, 48);
                LetterCombinationButtons[i].FlatStyle = FlatStyle.Flat;
                LetterCombinationButtons[i].BackColor = Colors.Origin;
                LetterCombinationButtons[i].FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
                LetterCombinationButtons[i].FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
                LetterCombinationButtons[i].FlatAppearance.BorderSize = 0;
                LetterCombinationButtons[i].Font = new Font("Calibri", 18);
                LetterCombinationButtons[i].ForeColor = Colors.Constant;
                LetterCombinationButtons[i].Text = LetterCombinations[i];
                LetterCombinationButtons[i].TextAlign = ContentAlignment.MiddleCenter;
                LetterCombinationButtons[i].MouseClick += LetterCombinationButton_MouseClick;
            }

            for (int i = LetterCombinations.Length / 2; i < LetterCombinations.Length; i++)
            {
                LetterCombinationButtons.Add(new Button());
                LetterCombinationButtons[i].TabIndex = 0;
                LetterCombinationButtons[i].TabStop = false;
                if (i == LetterCombinations.Length / 2)
                    LetterCombinationButtons[i].Location = new Point(ContainerSize.Width - 80, 12);
                else
                    LetterCombinationButtons[i].Location = new Point(ContainerSize.Width - 80, LetterCombinationButtons[i - 1].Bottom + 2);
                LetterCombinationButtons[i].Size = new Size(70, 48);
                LetterCombinationButtons[i].FlatStyle = FlatStyle.Flat;
                LetterCombinationButtons[i].BackColor = Colors.Origin;
                LetterCombinationButtons[i].FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
                LetterCombinationButtons[i].FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
                LetterCombinationButtons[i].FlatAppearance.BorderSize = 0;
                LetterCombinationButtons[i].Font = new Font("Calibri", 18);
                LetterCombinationButtons[i].ForeColor = Colors.Constant;
                LetterCombinationButtons[i].Text = LetterCombinations[i];
                LetterCombinationButtons[i].TextAlign = ContentAlignment.MiddleCenter;
                LetterCombinationButtons[i].MouseClick += LetterCombinationButton_MouseClick;
            }

            LetterCombinationPanel = new Panel();
            LetterCombinationPanel.TabIndex = 0;
            LetterCombinationPanel.TabStop = false;
            LetterCombinationPanel.Location = new Point(LetterCombinationButtons[0].Right + 50, LetterCombinationButtons[0].Top);
            LetterCombinationPanel.Size = new Size(LetterCombinationButtons[LetterCombinations.Length / 2].Left - 50 - LetterCombinationPanel.Left, ContainerSize.Height - 24);
            LetterCombinationPanel.BorderStyle = BorderStyle.None;
            LetterCombinationPanel.BackColor = Colors.Background;
            LetterCombinationPanel.Visible = false;

            LetterCombinationLabel = new Label();
            LetterCombinationLabel.TabIndex = 0;
            LetterCombinationLabel.Location = new Point(0, 50);
            LetterCombinationLabel.Size = new Size(LetterCombinationPanel.Width, 120);
            LetterCombinationLabel.BorderStyle = BorderStyle.None;
            LetterCombinationLabel.BackColor = Colors.Background;
            LetterCombinationLabel.Font = new Font("Calibri", 60);
            LetterCombinationLabel.ForeColor = Colors.Constant;
            LetterCombinationLabel.TextAlign = ContentAlignment.MiddleCenter;

            LetterCombinationExampleWordLabel = new Label();
            LetterCombinationExampleWordLabel.TabIndex = 0;
            LetterCombinationExampleWordLabel.Location = new Point(0, LetterCombinationLabel.Bottom);
            LetterCombinationExampleWordLabel.Size = new Size(LetterCombinationPanel.Width, 200);
            LetterCombinationExampleWordLabel.BorderStyle = BorderStyle.None;
            LetterCombinationExampleWordLabel.BackColor = Colors.Background;
            LetterCombinationExampleWordLabel.Font = new Font("Calibri", 80);
            LetterCombinationExampleWordLabel.ForeColor = Colors.Constant;
            LetterCombinationExampleWordLabel.TextAlign = ContentAlignment.MiddleCenter;
            LetterCombinationExampleWordLabel.MouseClick += LetterCombinationExampleWordLabel_MouseClick;

            TranslatedExampleWordLabel = new Label();
            TranslatedExampleWordLabel.TabIndex = 0;
            TranslatedExampleWordLabel.Location = new Point(0, LetterCombinationExampleWordLabel.Bottom);
            TranslatedExampleWordLabel.Size = new Size(LetterCombinationPanel.Width, 100);
            TranslatedExampleWordLabel.BorderStyle = BorderStyle.None;
            TranslatedExampleWordLabel.BackColor = Colors.Background;
            TranslatedExampleWordLabel.Font = new Font("Calibri", 50);
            TranslatedExampleWordLabel.ForeColor = Colors.Constant;
            TranslatedExampleWordLabel.TextAlign = ContentAlignment.MiddleCenter;

            LetterCombinationPanel.Controls.Add(LetterCombinationLabel);
            LetterCombinationPanel.Controls.Add(LetterCombinationExampleWordLabel);
            LetterCombinationPanel.Controls.Add(TranslatedExampleWordLabel);

            for (int i = 0; i < LetterCombinationButtons.Count; i++)
                AddControl(LetterCombinationButtons[i]);
            AddControl(LetterCombinationPanel);
        }

        private void LetterCombinationExampleWordLabel_MouseClick(object sender, MouseEventArgs e)
        {
            speechSynthesizer = new SpeechSynthesizer();
            speechSynthesizer.SetOutputToDefaultAudioDevice();
            Thread thread = new Thread(() => { speechSynthesizer.Speak(LetterCombinationExampleWordLabel.Text); });
            thread.IsBackground = true;
            thread.Start();
        }

        private void LetterCombinationButton_MouseClick(object sender, MouseEventArgs e)
        {
            Button LetterCombinationButton = sender as Button;
            int Position = 0;
            for (int i = 0; i < LetterCombinationButtons.Count; i++)
            {
                if (LetterCombinationButton == LetterCombinationButtons[i])
                    break;
                Position++;
            }
            LetterCombinationPanel.Visible = true;
            LetterCombinationLabel.Text = LetterCombinationButton.Text;
            LetterCombinationExampleWordLabel.Text = LetterCombinationExampleWords[Position];
            TranslatedExampleWordLabel.Text = TranslatedExampleWords[Position];
        }

        private void DisposeSpeechSynthesizer()
        {
            if (speechSynthesizer != null)
                speechSynthesizer.Dispose();
        }
    }
}
