using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using Microsoft.Speech.Synthesis;

namespace Deutsch_Interactive_Learning.Containers
{
    public class Reader : ViewControls.ContainerControls.Container
    {
        private SpeechSynthesizer speechSynthesizer;

        private Button ReadButton;
        private Button FontButton;
        private Button ColorButton;
        private Button[] TextAlignButtons = new Button[3];
        private Button UndoButton;
        private TextBox InputTextBox;

        public Reader(Form ParentForm, FlatDesign.Colors Colors)
        {
            CreateContainer(
                ParentForm,
                Colors,
                "Reader",
                "Simple text editor with an ability to read text.",
                new Bitmap(@"..\..\..\downloads\Icons\Reader.png"),
                "Instruments",
                new Bitmap(@"..\..\..\downloads\Icons\Instruments.png"),
                "Speech");

            OnHide = DisposeSpeechSynthesizer;

            ReadButton = new Button();
            ReadButton.TabIndex = 0;
            ReadButton.TabStop = false;
            ReadButton.Location = new Point(5, 5);
            ReadButton.Size = new Size(100, 60);
            ReadButton.FlatStyle = FlatStyle.Flat;
            ReadButton.BackColor = Colors.Origin;
            ReadButton.FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
            ReadButton.FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
            ReadButton.FlatAppearance.BorderSize = 0;
            ReadButton.Font = new Font("Calibri", 24);
            ReadButton.ForeColor = Colors.Constant;
            ReadButton.Text = "Read";
            ReadButton.TextAlign = ContentAlignment.MiddleCenter;
            ReadButton.MouseClick += ReadButton_MouseClick;

            FontButton = new Button();
            FontButton.TabIndex = 0;
            FontButton.TabStop = false;
            FontButton.Location = new Point(ReadButton.Right + 5, 5);
            FontButton.Size = new Size(100, 60);
            FontButton.FlatStyle = FlatStyle.Flat;
            FontButton.BackColor = Colors.Origin;
            FontButton.FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
            FontButton.FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
            FontButton.FlatAppearance.BorderSize = 0;
            FontButton.Font = new Font("Calibri", 24);
            FontButton.ForeColor = Colors.Constant;
            FontButton.Text = "Font";
            FontButton.TextAlign = ContentAlignment.MiddleCenter;
            FontButton.MouseClick += FontButton_MouseClick;

            ColorButton = new Button();
            ColorButton.TabIndex = 0;
            ColorButton.TabStop = false;
            ColorButton.Location = new Point(FontButton.Right + 5, 5);
            ColorButton.Size = new Size(100, 60);
            ColorButton.FlatStyle = FlatStyle.Flat;
            ColorButton.BackColor = Colors.Origin;
            ColorButton.FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
            ColorButton.FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
            ColorButton.FlatAppearance.BorderSize = 0;
            ColorButton.Font = new Font("Calibri", 24);
            ColorButton.ForeColor = Colors.Constant;
            ColorButton.Text = "Color";
            ColorButton.TextAlign = ContentAlignment.MiddleCenter;
            ColorButton.MouseClick += ColorButton_MouseClick;

            TextAlignButtons[0] = new Button();
            TextAlignButtons[0].TabIndex = 0;
            TextAlignButtons[0].TabStop = false;
            TextAlignButtons[0].Location = new Point(ColorButton.Right + 5, 5);
            TextAlignButtons[0].Size = new Size(60, 60);
            TextAlignButtons[0].FlatStyle = FlatStyle.Flat;
            TextAlignButtons[0].BackColor = Colors.Origin;
            TextAlignButtons[0].FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
            TextAlignButtons[0].FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
            TextAlignButtons[0].FlatAppearance.BorderSize = 0;
            TextAlignButtons[0].Image = new Bitmap(@"..\..\..\downloads\Icons\TextAlignLeft.png");
            TextAlignButtons[0].ImageAlign = ContentAlignment.MiddleCenter;
            TextAlignButtons[0].MouseClick += TextAlignLeftButton_MouseClick;

            TextAlignButtons[1] = new Button();
            TextAlignButtons[1].TabIndex = 0;
            TextAlignButtons[1].TabStop = false;
            TextAlignButtons[1].Location = new Point(TextAlignButtons[0].Right + 5, 5);
            TextAlignButtons[1].Size = new Size(60, 60);
            TextAlignButtons[1].FlatStyle = FlatStyle.Flat;
            TextAlignButtons[1].BackColor = Colors.Origin;
            TextAlignButtons[1].FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
            TextAlignButtons[1].FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
            TextAlignButtons[1].FlatAppearance.BorderSize = 0;
            TextAlignButtons[1].Image = new Bitmap(@"..\..\..\downloads\Icons\TextAlignCenter.png");
            TextAlignButtons[1].ImageAlign = ContentAlignment.MiddleCenter;
            TextAlignButtons[1].MouseClick += TextAlignCenterButton_MouseClick;

            TextAlignButtons[2] = new Button();
            TextAlignButtons[2].TabIndex = 0;
            TextAlignButtons[2].TabStop = false;
            TextAlignButtons[2].Location = new Point(TextAlignButtons[1].Right + 5, 5);
            TextAlignButtons[2].Size = new Size(60, 60);
            TextAlignButtons[2].FlatStyle = FlatStyle.Flat;
            TextAlignButtons[2].BackColor = Colors.Origin;
            TextAlignButtons[2].FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
            TextAlignButtons[2].FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
            TextAlignButtons[2].FlatAppearance.BorderSize = 0;
            TextAlignButtons[2].Image = new Bitmap(@"..\..\..\downloads\Icons\TextAlignRight.png");
            TextAlignButtons[2].ImageAlign = ContentAlignment.MiddleCenter;
            TextAlignButtons[2].MouseClick += TextAlignRightButton_MouseClick;

            UndoButton = new Button();
            UndoButton.TabIndex = 0;
            UndoButton.TabStop = false;
            UndoButton.Location = new Point(TextAlignButtons[2].Right + 5, 5);
            UndoButton.Size = new Size(60, 60);
            UndoButton.FlatStyle = FlatStyle.Flat;
            UndoButton.BackColor = Colors.Origin;
            UndoButton.FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
            UndoButton.FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
            UndoButton.FlatAppearance.BorderSize = 0;
            UndoButton.Image= new Bitmap(@"..\..\..\downloads\Icons\ReaderUndo.png");
            UndoButton.ImageAlign = ContentAlignment.MiddleCenter;
            UndoButton.MouseClick += UndoButton_MouseClick;

            InputTextBox = new TextBox();
            InputTextBox.TabIndex = 0;
            InputTextBox.TabStop = false;
            InputTextBox.Location = new Point(5, ReadButton.Bottom + 5);
            InputTextBox.Size = new Size(ContainerSize.Width - 10, ContainerSize.Height - 15 - ReadButton.Height);
            InputTextBox.BorderStyle = BorderStyle.None;
            InputTextBox.BackColor = Colors.Origin;
            InputTextBox.Font = new Font("Calibri", 14);
            InputTextBox.Multiline = true;
            InputTextBox.ForeColor = Colors.Constant;
            InputTextBox.TextAlign = HorizontalAlignment.Left;
            InputTextBox.ScrollBars = ScrollBars.None;

            AddControl(ReadButton);
            AddControl(FontButton);
            AddControl(ColorButton);
            for (int i = 0; i < 3; i++)
                AddControl(TextAlignButtons[i]);
            AddControl(UndoButton);
            AddControl(InputTextBox);
        }

        private void ReadButton_MouseClick(object sender, MouseEventArgs e)
        {
            speechSynthesizer = new SpeechSynthesizer();
            speechSynthesizer.SetOutputToDefaultAudioDevice();
            if (InputTextBox.Text != "")
            {
                Thread thread = new Thread(() => { speechSynthesizer.Speak(InputTextBox.Text); });
                thread.IsBackground = true;
                thread.Start();
            }
        }

        private void FontButton_MouseClick(object sender, MouseEventArgs e)
        {
            AddThread(new Thread(() =>
            {
                FontDialog fontDialog = new FontDialog();
                fontDialog.Font = InputTextBox.Font;
                if (fontDialog.ShowDialog() == DialogResult.OK)
                    InputTextBox.Invoke((MethodInvoker)(delegate { InputTextBox.Font = fontDialog.Font; }));
            }), true);
        }

        private void ColorButton_MouseClick(object sender, MouseEventArgs e)
        {
            AddThread(new Thread(() =>
            {
                ColorDialog colorDialog = new ColorDialog();
                colorDialog.Color = InputTextBox.ForeColor;
                if (colorDialog.ShowDialog() == DialogResult.OK)
                    InputTextBox.Invoke((MethodInvoker)(delegate { InputTextBox.ForeColor = colorDialog.Color; }));
            }), true);
        }

        private void TextAlignLeftButton_MouseClick(object sender, MouseEventArgs e)
        {
            InputTextBox.TextAlign = HorizontalAlignment.Left;
        }

        private void TextAlignCenterButton_MouseClick(object sender, MouseEventArgs e)
        {
            InputTextBox.TextAlign = HorizontalAlignment.Center;
        }

        private void TextAlignRightButton_MouseClick(object sender, MouseEventArgs e)
        {
            InputTextBox.TextAlign = HorizontalAlignment.Right;
        }

        private void UndoButton_MouseClick(object sender, MouseEventArgs e)
        {
            InputTextBox.Undo();
        }

        private void DisposeSpeechSynthesizer()
        {
            if (speechSynthesizer != null)
                speechSynthesizer.Dispose();
        }
    }
}
