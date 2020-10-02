using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using Microsoft.Speech.Synthesis;
using Microsoft.Speech.Recognition;

namespace Deutsch_Interactive_Learning
{
    public partial class MainForm : Form
    {
        private FlatDesign.Colors Colors;

        private ViewControls.FormControls.Border Border;
        private ViewControls.FormControls.Header Header;

        private Instruments.NavigationCreator NavigationCreator;

        public MainForm()
        {
            InitializeComponent();
            this.Name = "MainForm";
            this.Text = "Deutsch Interactive Learning";
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(852, 654);

            Colors = new FlatDesign.Colors(Color.FromArgb(50, 205, 50), 10);
            this.BackColor = Colors.Background;

            Border = new ViewControls.FormControls.Border(this, Colors);
            Header = new ViewControls.FormControls.Header(this, Colors);

            List<ViewControls.ContainerControls.Container> ContainerTemplates = new List<ViewControls.ContainerControls.Container>();

            ContainerTemplates.Add(new Containers.Reader(this, Colors));
            ContainerTemplates.Add(new Containers.AlphabetListening(this, Colors));
            ContainerTemplates.Add(new Containers.AlphabetPracticing(this, Colors));
            //ContainerTemplates.Add(new Containers.AlphabetTesting(this, Colors));
            ContainerTemplates.Add(new Containers.LetterCombinationsListening(this, Colors));
            ContainerTemplates.Add(new Containers.LetterCombinationsPracticing(this, Colors));
            //ContainerTemplates.Add(new Containers.LetterCombinationsTesting(this, Colors));
            ContainerTemplates.Add(new Containers.NumbersFrom0To9Listening(this, Colors));
            ContainerTemplates.Add(new Containers.NumbersFrom0To9Practicing(this, Colors));
            //ContainerTemplates.Add(new Containers.NumbersFrom0To9Testing(this, Colors));
            ContainerTemplates.Add(new Containers.AllNumbersListening(this, Colors));
            ContainerTemplates.Add(new Containers.AllNumbersPracticing(this, Colors));
            //ContainerTemplates.Add(new Containers.AllNumbersTesting(this, Colors));
            ContainerTemplates.Add(new Containers.ClockListening(this, Colors));
            ContainerTemplates.Add(new Containers.ClockPracticing(this, Colors));
            //ContainerTemplates.Add(new Containers.ClockTesting(this, Colors));
            ContainerTemplates.Add(new Containers.WeekListening(this, Colors));
            ContainerTemplates.Add(new Containers.WeekPracticing(this, Colors));
            //ContainerTemplates.Add(new Containers.WeekTesting(this, Colors));
            ContainerTemplates.Add(new Containers.SeasonsListening(this, Colors));
            ContainerTemplates.Add(new Containers.SeasonsPracticing(this, Colors));
            //ContainerTemplates.Add(new Containers.SeasonsTesting(this, Colors));
            ContainerTemplates.Add(new Containers.MonthsListening(this, Colors));
            ContainerTemplates.Add(new Containers.MonthsPracticing(this, Colors));
            //ContainerTemplates.Add(new Containers.MonthsTesting(this, Colors));
            ContainerTemplates.Add(new Containers.Editor(this, Colors));

            NavigationCreator = new Instruments.NavigationCreator();
            NavigationCreator.CreateNavigation(this, Colors, ContainerTemplates);

            EditorForm editorForm = new EditorForm();
            editorForm.Show();
        }
    }
}
