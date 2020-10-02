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
    class ClockListening : ViewControls.ContainerControls.Container
    {
        private Label VorLabel, NachLabel;
        private Label ClockLabel;
        private Button NextClockButton;
        private Button PreviusClockButton;
        private Label ExpressionLabel;

        private string TimeNumberExample = "07:00 08:30 09:15 09:45 10:00 11:05 13:55 14:02 15:25 16:35 18:40 19:20 21:37 22:00";
        private string[] TimeNumbers;
        private string TimeExpressionExample = "Sieben Uhr|Halb neun|Fünfzehn nach neun|Fünfzehn vor zehn|Zehn Uhr|Fünf nach elf|Fünf vor vierzehn|Zwei nach dreizehn|Fünfundzwanzig nach sechzehn|Fünfundzwanzig vor siebzehn|Zwanzig vor neunzehn|Zwanzig nach zwanzig|Dreiundzwanzig vor einundzwanzig|Zweiundzwanzig Uhr";
        private string[] TimeExpressions;
        private int Position = 0;
        private SpeechSynthesizer speechSynthesizer;

        private FlatDesign.Colors Colors;

        public ClockListening(Form ParentForm, FlatDesign.Colors Colors)
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
                "Listening");

            OnHide = DisposeSpeechSynthesizer;

            TimeNumbers = TimeNumberExample.Split(' ');
            TimeExpressions = TimeExpressionExample.Split('|');

            VorLabel = new Label();
            VorLabel.TabIndex = 0;
            VorLabel.Location = new Point(20, 3);
            VorLabel.AutoSize = true;
            VorLabel.FlatStyle = FlatStyle.Flat;
            VorLabel.BorderStyle = BorderStyle.None;
            VorLabel.BackColor = Colors.Background;
            VorLabel.Font = new Font("Calibri", 36);
            VorLabel.ForeColor = Colors.Constant;
            VorLabel.Text = "Vor - Before";
            VorLabel.TextAlign = ContentAlignment.MiddleLeft;

            NachLabel = new Label();
            NachLabel.TabIndex = 0;
            NachLabel.Location = new Point(ContainerSize.Width - 220, 3);
            NachLabel.AutoSize = true;
            NachLabel.FlatStyle = FlatStyle.Flat;
            NachLabel.BorderStyle = BorderStyle.None;
            NachLabel.BackColor = Colors.Background;
            NachLabel.Font = new Font("Calibri", 36);
            NachLabel.ForeColor = Colors.Constant;
            NachLabel.Text = "Nach - To";
            NachLabel.TextAlign = ContentAlignment.MiddleRight;

            ClockLabel = new Label();
            ClockLabel.TabIndex = 0;
            ClockLabel.Location = new Point(50, 50);
            ClockLabel.Size = new Size(ContainerSize.Width - 100, 300);
            ClockLabel.FlatStyle = FlatStyle.Flat;
            ClockLabel.BorderStyle = BorderStyle.None;
            ClockLabel.BackColor = Colors.Background;
            ClockLabel.Font = new Font("Calibri", 148);
            ClockLabel.ForeColor = Colors.Constant;
            ClockLabel.Text = TimeNumbers[0];
            ClockLabel.TextAlign = ContentAlignment.MiddleCenter;
            ClockLabel.MouseClick += ClockLabel_MouseClick;

            NextClockButton = new Button();
            NextClockButton.TabIndex = 0;
            NextClockButton.TabStop = false;
            NextClockButton.Location = new Point(ClockLabel.Width - 70, 100);
            NextClockButton.Size = new Size(70, 100);
            NextClockButton.FlatStyle = FlatStyle.Flat;
            NextClockButton.BackColor = Colors.Origin;
            NextClockButton.FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
            NextClockButton.FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
            NextClockButton.FlatAppearance.BorderSize = 0;
            NextClockButton.Image = new Bitmap(@"..\..\..\downloads\Icons\Next.png");
            NextClockButton.ImageAlign = ContentAlignment.MiddleCenter;
            NextClockButton.MouseClick += NextClockButton_MouseClick;

            PreviusClockButton = new Button();
            PreviusClockButton.TabIndex = 0;
            PreviusClockButton.TabStop = false;
            PreviusClockButton.Location = new Point(0, 100);
            PreviusClockButton.Size = new Size(70, 100);
            PreviusClockButton.FlatStyle = FlatStyle.Flat;
            PreviusClockButton.BackColor = Colors.Origin;
            PreviusClockButton.FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
            PreviusClockButton.FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
            PreviusClockButton.FlatAppearance.BorderSize = 0;
            PreviusClockButton.Image = new Bitmap(@"..\..\..\downloads\Icons\Previus.png");
            PreviusClockButton.ImageAlign = ContentAlignment.MiddleCenter;
            PreviusClockButton.MouseClick += PreviusClockButton_MouseClick;
            PreviusClockButton.Visible = false;

            ExpressionLabel = new Label();
            ExpressionLabel.TabIndex = 0;
            ExpressionLabel.Location = new Point(0, 350);
            ExpressionLabel.Size = new Size(ContainerSize.Width, 100);
            ExpressionLabel.FlatStyle = FlatStyle.Flat;
            ExpressionLabel.BorderStyle = BorderStyle.None;
            ExpressionLabel.BackColor = Colors.Background;
            ExpressionLabel.Font = new Font("Calibri", 36);
            ExpressionLabel.ForeColor = Colors.Constant;
            ExpressionLabel.Text = TimeExpressions[0];
            ExpressionLabel.TextAlign = ContentAlignment.MiddleCenter;
            ExpressionLabel.MouseClick += ExpressionLabel_MouseClick;

            ClockLabel.Controls.Add(NextClockButton);
            ClockLabel.Controls.Add(PreviusClockButton);

            AddControl(VorLabel);
            AddControl(NachLabel);
            AddControl(ClockLabel);
            AddControl(ExpressionLabel);
        }
        
        private void NextClockButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (Position < TimeNumbers.Length - 1)
            {
                Position++;
                ClockLabel.Text = TimeNumbers[Position];
                ExpressionLabel.Text = TimeExpressions[Position];
            }
            NextClockButton.Visible = true;
            PreviusClockButton.Visible = true;
            if (Position == TimeNumbers.Length - 1)
                NextClockButton.Visible = false;
        }

        private void PreviusClockButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (Position > 0)
            {
                Position--;
                ClockLabel.Text = TimeNumbers[Position];
                ExpressionLabel.Text = TimeExpressions[Position];
            }
            NextClockButton.Visible = true;
            PreviusClockButton.Visible = true;
            if (Position == 0)
                PreviusClockButton.Visible = false;
        }

        private void ClockLabel_MouseClick(object sender, MouseEventArgs e)
        {
            speechSynthesizer = new SpeechSynthesizer();
            speechSynthesizer.SetOutputToDefaultAudioDevice();
            Thread thread = new Thread(() => { speechSynthesizer.Speak(TimeExpressions[Position]); });
            thread.IsBackground = true;
            thread.Start();
        }

        private void ExpressionLabel_MouseClick(object sender, MouseEventArgs e)
        {
            speechSynthesizer = new SpeechSynthesizer();
            speechSynthesizer.SetOutputToDefaultAudioDevice();
            Thread thread = new Thread(() => { speechSynthesizer.Speak(TimeExpressions[Position]); });
            thread.IsBackground = true;
            thread.Start();
        }

        private void DisposeSpeechSynthesizer()
        {
            if (speechSynthesizer != null)
                speechSynthesizer.Dispose();
        }
    }
}
