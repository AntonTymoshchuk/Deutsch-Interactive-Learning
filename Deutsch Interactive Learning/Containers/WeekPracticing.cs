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
using Microsoft.Speech.Synthesis;

namespace Deutsch_Interactive_Learning.Containers
{
    class WeekPracticing : ViewControls.ContainerControls.Container
    {
        private Label GermanDayLabel;
        private Label EnglishDayLabel;
        private Button NextDayButton;
        private Button PreviusDayButton;

        private string GermanWeekExample = "Sonntag Montag Dienstag Mittwoch Donnerstag Freitag Sonnabend";
        private string[] GermanDays;
        private string EnglishWeekExample = "Sunday Monday Tuesday Wednesday Thursday Friday Saturday";
        private string[] EnglishDays;
        private int Position = 0;
        private CultureInfo cultureInfo = new CultureInfo("de-de");
        private SpeechRecognitionEngine speechRecognitionEngine;
        private SpeechSynthesizer speechSynthesizer;

        private FlatDesign.Colors Colors;

        public WeekPracticing(Form ParentForm, FlatDesign.Colors Colors)
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
                "Practicing");

            OnHide = DisposeSpeechInstruments;

            GermanDays = GermanWeekExample.Split(' ');
            EnglishDays = EnglishWeekExample.Split(' ');

            GermanDayLabel = new Label();
            GermanDayLabel.TabIndex = 0;
            GermanDayLabel.Location = new Point(0, 100);
            GermanDayLabel.Size = new Size(ContainerSize.Width, 200);
            GermanDayLabel.FlatStyle = FlatStyle.Flat;
            GermanDayLabel.BorderStyle = BorderStyle.None;
            GermanDayLabel.BackColor = Colors.Background;
            GermanDayLabel.Font = new Font("Calibri", 80);
            GermanDayLabel.ForeColor = Colors.Constant;
            GermanDayLabel.Text = GermanDays[0];
            GermanDayLabel.TextAlign = ContentAlignment.MiddleCenter;
            GermanDayLabel.MouseClick += GermanDayLabel_MouseClick;

            NextDayButton = new Button();
            NextDayButton.TabIndex = 0;
            NextDayButton.TabStop = false;
            NextDayButton.Location = new Point(GermanDayLabel.Width - 100, 50);
            NextDayButton.Size = new Size(70, 100);
            NextDayButton.FlatStyle = FlatStyle.Flat;
            NextDayButton.BackColor = Colors.Origin;
            NextDayButton.FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
            NextDayButton.FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
            NextDayButton.FlatAppearance.BorderSize = 0;
            NextDayButton.Image = new Bitmap(@"..\..\..\downloads\Icons\Next.png");
            NextDayButton.ImageAlign = ContentAlignment.MiddleCenter;
            NextDayButton.MouseClick += NextDayButton_MouseClick;

            PreviusDayButton = new Button();
            PreviusDayButton.TabIndex = 0;
            PreviusDayButton.TabStop = false;
            PreviusDayButton.Location = new Point(30, 50);
            PreviusDayButton.Size = new Size(70, 100);
            PreviusDayButton.FlatStyle = FlatStyle.Flat;
            PreviusDayButton.BackColor = Colors.Origin;
            PreviusDayButton.FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
            PreviusDayButton.FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
            PreviusDayButton.FlatAppearance.BorderSize = 0;
            PreviusDayButton.Image = new Bitmap(@"..\..\..\downloads\Icons\Previus.png");
            PreviusDayButton.ImageAlign = ContentAlignment.MiddleCenter;
            PreviusDayButton.Visible = false;
            PreviusDayButton.MouseClick += PreviusDayButton_MouseClick;

            EnglishDayLabel = new Label();
            EnglishDayLabel.TabIndex = 0;
            EnglishDayLabel.Location = new Point(0, GermanDayLabel.Bottom);
            EnglishDayLabel.Size = new Size(ContainerSize.Width, 80);
            EnglishDayLabel.FlatStyle = FlatStyle.Flat;
            EnglishDayLabel.BorderStyle = BorderStyle.None;
            EnglishDayLabel.BackColor = Colors.Background;
            EnglishDayLabel.Font = new Font("Calibri", 50);
            EnglishDayLabel.ForeColor = Colors.Constant;
            EnglishDayLabel.Text = EnglishDays[0];
            EnglishDayLabel.TextAlign = ContentAlignment.MiddleCenter;

            GermanDayLabel.Controls.Add(NextDayButton);
            GermanDayLabel.Controls.Add(PreviusDayButton);

            AddControl(GermanDayLabel);
            AddControl(EnglishDayLabel);
        }

        private void GermanDayLabel_MouseClick(object sender, MouseEventArgs e)
        {
            speechSynthesizer = new SpeechSynthesizer();
            speechSynthesizer.SetOutputToDefaultAudioDevice();
            speechSynthesizer.Speak(GermanDays[Position]);
            SetGermanDayToRecognize();
        }

        private void NextDayButton_MouseClick(object sender, MouseEventArgs e)
        {
            GermanDayLabel.ForeColor = Colors.Constant;
            if (Position < GermanDays.Length - 1)
            {
                Position++;
                GermanDayLabel.Text = GermanDays[Position];
                EnglishDayLabel.Text = EnglishDays[Position];
            }
            NextDayButton.Visible = true;
            PreviusDayButton.Visible = true;
            if (Position == GermanDays.Length - 1)
                NextDayButton.Visible = false;
        }

        private void PreviusDayButton_MouseClick(object sender, MouseEventArgs e)
        {
            GermanDayLabel.ForeColor = Colors.Constant;
            if (Position > 0)
            {
                Position--;
                GermanDayLabel.Text = GermanDays[Position];
                EnglishDayLabel.Text = EnglishDays[Position];
            }
            NextDayButton.Visible = true;
            PreviusDayButton.Visible = true;
            if (Position == 0)
                PreviusDayButton.Visible = false;
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
            speechRecognitionEngine.RecognizeAsync(RecognizeMode.Multiple);
        }

        private void SpeechRecognitionEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence * 100 >= 90)
                GermanDayLabel.ForeColor = Color.Gold;
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
