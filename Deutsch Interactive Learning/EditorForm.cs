using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Deutsch_Interactive_Learning
{
    public partial class EditorForm : Form
    {
        private Button ProcessButton;
        private int ProcessPosition = 0;

        #region Start
        private Panel StartPanel;
        private Label NameLabel;
        private TextBox NameTextBox;
        private Label OverviewLabel;
        private TextBox OverviewTextBox;
        private Label CategoryLabel;
        private TextBox CategoryTextBox;
        #endregion

        #region Menu
        private Panel MenuPanel;
        private List<CustomElement> CustomElements;
        private CustomElement FocusedCustomElement;

        #region Background properties
        private Panel BackgroundPropertiesPanel;
        private Label BackgroundPropertiesLabel;
        private Button BackgroundColorButton;
        private Button BackgroundImageButton;
        #endregion

        #region Switches properties
        private Panel SwitchesPropertiesPanel;
        private Label SwitchesPropertiesLabel;
        private TextBox TopValueTextBox;
        private Button SwitchesBackColorButton;
        private Button SwitchesMouseOverBackColorButton;
        private Button SwitchesMouseDownBackColorButton;
        private Button SwitchesTransparentBackColorButton;
        #endregion

        #region Elements
        private Panel ElementsPanel;
        private Label ElementsLabel;
        private Button TextButton;
        private Button InputButton;
        private Button PictureButton;
        private Button PanelButton;
        private Button ButtonButton;
        #endregion

        #region Common properties
        private Panel CommonPropertiesPanel;
        private Label CommonPropertiesLabel;
        private TextBox PointXTextBox;
        private TextBox PointYTextBox;
        private TextBox WidthTextBox;
        private TextBox HeightTextBox;
        private Button BackColorButton;
        private Button TransparentBackColorButton;
        #endregion

        #region Text properties
        private Panel TextPropertiesPanel;
        private Label TextPropertiesLabel;
        private TextBox FontSizeTextBox;
        private Button FontSizePlusButton;
        private Button FontSizeMinusButton;
        private Button FontButton;
        private Button ForeColorButton;

        private Button BoldTextButton;
        private Button ItalicTextButton;
        private Button UnderlineTextButton;
        private Button StrikeoutTextButton;
        private Button TextAlignLeftButton;
        private Button TextAlignCenterButton;
        private Button TextAlignRightButton;

        private bool IsBold = false;
        private bool IsItalic = false;
        private bool IsUnderline = false;
        private bool IsStrikeout = false;
        #endregion

        #region Image properties
        private Panel ImagePropertiesPanel;
        private Label ImagePropertiesLabel;
        private Button SizeMode1Button;
        private Button SizeMode2Button;
        private Button SizeMode3Button;
        private Button SizeMode4Button;
        private Button SizeMode5Button;
        #endregion

        #region Z-index properties
        private Panel ZindexPropertiesPanel;
        private Label ZindexPropertiesLabel;
        private Button BringToFrontButton;
        private Button SendToBackButton;
        #endregion

        #region Interactive properties
        private Panel InteractivePropertiesPanel;
        private Label InteractivePropertiesLabel;
        private CheckBox ReadCheckBox;
        private CheckBox ListenCheckBox;
        #endregion

        #region Mouse properties
        private Panel MousePropertiesPanel;
        private Label MousePropertiesLabel;
        private Button MouseOverBackColorButton;
        private Button MouseDownBackColorButton;
        #endregion

        #region Actions
        private Panel ActionsPanel;
        private Label ActionsLabel;
        private Button DeleteButton;
        private Button CloneButton;
        #endregion

        #region Text content
        private Panel TextContentPanel;
        private Label TextContentLabel;
        private TextBox TextContentTextBox;
        #endregion

        #region Input content
        Panel InputContentPanel;
        Label InputContentLabel;
        Label NoInputContentLabel;
        #endregion

        #region Picture content
        Panel PictureContentPanel;
        Label PictureContentLabel;
        Button SelectPictureButton;
        #endregion
        
        #region Panel content
        Panel PanelContentPanel;
        Label PanelContentLabel;
        Label NoPanelContentLabel;
        #endregion
        
        #region Button content
        Panel ButtonContentPanel;
        Label ButtonContentLabel;
        TextBox TargetPageIdTextBox;
        #endregion

        #endregion

        #region Pages
        private Panel PagesPanel;
        private List<Page> Pages;
        private int PageIndex = -1;
        private List<Button> PageButtons;
        private Button AddPageButton;
        private List<CustomElement> DefaultCustomElements;
        #endregion

        #region Container
        private Panel ContainerPanel;
        private Button NextPageButton;
        private Button PreviusPageButton;
        private Panel[] BorderPanels;

        private bool BackgroundColor = true;

        private bool ResizeStarted = false;
        private int ResizeStartX, ResizeStartY;

        private bool DragStarted = false;
        private int DragStartX, DragStartY;
        #endregion

        private ViewControls.FormControls.Border Border;
        private ViewControls.FormControls.Header Header;
        private FlatDesign.Colors Colors;

        public EditorForm()
        {
            InitializeComponent();
            this.Name = "EditorForm";
            this.Text = "Editor";
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(852, 654);

            Colors = new FlatDesign.Colors(Color.FromArgb(50, 205, 50), 10);
            this.BackColor = Colors.Background;

            Pages = new List<Page>();
            PageButtons = new List<Button>();

            Border = new ViewControls.FormControls.Border(this, Colors);
            Header = new ViewControls.FormControls.Header(this, Colors);

            ProcessButton = new Button();
            ProcessButton.TabIndex = 0;
            ProcessButton.TabStop = false;
            ProcessButton.Location = new Point(276, 618);
            ProcessButton.Size = new Size(300, 30);
            ProcessButton.FlatStyle = FlatStyle.Flat;
            ProcessButton.BackColor = Colors.Curtain;
            ProcessButton.FlatAppearance.MouseOverBackColor = Colors.CurtainButtonMouseOver;
            ProcessButton.FlatAppearance.MouseDownBackColor = Colors.CurtainButtonMouseDown;
            ProcessButton.FlatAppearance.BorderSize = 0;
            ProcessButton.Font = new Font("Calibri", 14);
            ProcessButton.ForeColor = Colors.Constant;
            ProcessButton.Text = "Create";
            ProcessButton.TextAlign = ContentAlignment.MiddleCenter;
            ProcessButton.MouseClick += ProcessButton_MouseClick;
            this.Controls.Add(ProcessButton);

            InitializeStart();
            InitializeMenu();
            InitializePages();
            InitializeContainer();
        }

        private void ProcessButton_MouseClick(object sender, MouseEventArgs e)
        {
            switch (ProcessPosition)
            {
                case 0:
                    StartPanel.Visible = false;
                    MenuPanel.Visible = true;
                    ContainerPanel.Visible = true;
                    ProcessButton.Text = "Continue";
                    break;

                case 1:
                    ContainerPanel.Left = 125;
                    PagesPanel.Visible = true;
                    ProcessButton.Text = "Save";

                    DefaultCustomElements = new List<CustomElement>();
                    for (int i = 0; i < CustomElements.Count; i++)
                    {
                        DefaultCustomElements.Add(CustomElements[i]);
                        //DefaultCustomElements[i] = CustomElements[i];
                    }

                    AddPage();

                    BackgroundPropertiesPanel.Visible = false;
                    SwitchesPropertiesPanel.Visible = false;
                    ElementsPanel.Visible = false;

                    CommonPropertiesPanel.Visible = false;
                    CommonPropertiesPanel.Location = new Point(0, 0);

                    TextPropertiesPanel.Visible = false;
                    TextPropertiesPanel.Location = new Point(0, 0);

                    ImagePropertiesPanel.Visible = false;
                    ImagePropertiesPanel.Location = new Point(0, 0);

                    ZindexPropertiesPanel.Visible = false;
                    ZindexPropertiesPanel.Location = new Point(0, 0);

                    InteractivePropertiesPanel.Visible = false;
                    InteractivePropertiesPanel.Location = new Point(0, 0);

                    MousePropertiesPanel.Visible = false;
                    MousePropertiesPanel.Location = new Point(0, 0);

                    ActionsPanel.Visible = false;
                    ActionsPanel.Location = new Point(0, 0);
                    break;
            }
            ProcessPosition++;
        }

        private void InitializeStart()
        {
            StartPanel = new Panel();
            StartPanel.TabIndex = 0;
            StartPanel.TabStop = false;
            StartPanel.Location = new Point(226, 226);
            StartPanel.Size = new Size(400, 400);
            StartPanel.BorderStyle = BorderStyle.None;
            StartPanel.BackColor = Colors.Background;

            NameLabel = new Label();
            NameLabel.TabIndex = 0;
            NameLabel.Location = new Point(0, 0);
            NameLabel.Width = 400;
            NameLabel.FlatStyle = FlatStyle.Flat;
            NameLabel.BorderStyle = BorderStyle.None;
            NameLabel.BackColor = Colors.Background;
            NameLabel.Font = new Font("Calibri", 14);
            NameLabel.ForeColor = Colors.Constant;
            NameLabel.Text = "Name :";
            NameLabel.TextAlign = ContentAlignment.MiddleLeft;

            NameTextBox = new TextBox();
            NameTextBox.TabIndex = 0;
            NameTextBox.TabStop = false;
            NameTextBox.Location = new Point(0, NameLabel.Bottom);
            NameTextBox.Width = 400;
            NameTextBox.BorderStyle = BorderStyle.None;
            NameTextBox.BackColor = Colors.Origin;
            NameTextBox.Font = new Font("Calibri", 14);
            NameTextBox.ForeColor = Colors.Constant;
            NameTextBox.TextAlign = HorizontalAlignment.Left;

            OverviewLabel = new Label();
            OverviewLabel.TabIndex = 0;
            OverviewLabel.Location = new Point(0, NameTextBox.Bottom + 5);
            OverviewLabel.Width = 400;
            OverviewLabel.FlatStyle = FlatStyle.Flat;
            OverviewLabel.BorderStyle = BorderStyle.None;
            OverviewLabel.BackColor = Colors.Background;
            OverviewLabel.Font = new Font("Calibri", 14);
            OverviewLabel.ForeColor = Colors.Constant;
            OverviewLabel.Text = "Overview :";
            OverviewLabel.TextAlign = ContentAlignment.MiddleLeft;

            OverviewTextBox = new TextBox();
            OverviewTextBox.TabIndex = 0;
            OverviewTextBox.TabStop = false;
            OverviewTextBox.Location = new Point(0, OverviewLabel.Bottom);
            OverviewTextBox.Width = 400;
            OverviewTextBox.BorderStyle = BorderStyle.None;
            OverviewTextBox.BackColor = Colors.Origin;
            OverviewTextBox.Font = new Font("Calibri", 14);
            OverviewTextBox.ForeColor = Colors.Constant;
            OverviewTextBox.TextAlign = HorizontalAlignment.Left;

            CategoryLabel = new Label();
            CategoryLabel.TabIndex = 0;
            CategoryLabel.Location = new Point(0, OverviewTextBox.Bottom + 5);
            CategoryLabel.Width = 400;
            CategoryLabel.FlatStyle = FlatStyle.Flat;
            CategoryLabel.BorderStyle = BorderStyle.None;
            CategoryLabel.BackColor = Colors.Background;
            CategoryLabel.Font = new Font("Calibri", 14);
            CategoryLabel.ForeColor = Colors.Constant;
            CategoryLabel.Text = "Category :";
            CategoryLabel.TextAlign = ContentAlignment.MiddleLeft;

            CategoryTextBox = new TextBox();
            CategoryTextBox.TabIndex = 0;
            CategoryTextBox.TabStop = false;
            CategoryTextBox.Location = new Point(0, CategoryLabel.Bottom);
            CategoryTextBox.Width = 400;
            CategoryTextBox.BorderStyle = BorderStyle.None;
            CategoryTextBox.BackColor = Colors.Origin;
            CategoryTextBox.Font = new Font("Calibri", 14);
            CategoryTextBox.ForeColor = Colors.Constant;
            CategoryTextBox.TextAlign = HorizontalAlignment.Left;

            StartPanel.Controls.Add(NameLabel);
            StartPanel.Controls.Add(NameTextBox);
            StartPanel.Controls.Add(OverviewLabel);
            StartPanel.Controls.Add(OverviewTextBox);
            StartPanel.Controls.Add(CategoryLabel);
            StartPanel.Controls.Add(CategoryTextBox);

            this.Controls.Add(StartPanel);
        }

        private void InitializeMenu()
        {
            MenuPanel = new Panel();
            MenuPanel.TabIndex = 0;
            MenuPanel.TabStop = false;
            MenuPanel.Location = new Point(1, 31);
            MenuPanel.Size = new Size(this.Width - 2, 95);
            MenuPanel.BorderStyle = BorderStyle.None;
            MenuPanel.BackColor = Colors.Curtain;

            #region Background properties
            BackgroundPropertiesPanel = new Panel();
            BackgroundPropertiesPanel.TabIndex = 0;
            BackgroundPropertiesPanel.TabStop = false;
            BackgroundPropertiesPanel.Location = new Point(120, 0);
            BackgroundPropertiesPanel.Size = new Size(120, 90);
            BackgroundPropertiesPanel.BorderStyle = BorderStyle.None;
            BackgroundPropertiesPanel.BackColor = Colors.Curtain;

            BackgroundPropertiesLabel = new Label();
            BackgroundPropertiesLabel.TabIndex = 0;
            BackgroundPropertiesLabel.Location = new Point(0, 0);
            BackgroundPropertiesLabel.Size = new Size(BackgroundPropertiesPanel.Width, 30);
            BackgroundPropertiesLabel.FlatStyle = FlatStyle.Flat;
            BackgroundPropertiesLabel.BorderStyle = BorderStyle.None;
            BackgroundPropertiesLabel.BackColor = Colors.Curtain;
            BackgroundPropertiesLabel.Font = new Font("Calibri", 14);
            BackgroundPropertiesLabel.ForeColor = Colors.Constant;
            BackgroundPropertiesLabel.Text = "Background";
            BackgroundPropertiesLabel.TextAlign = ContentAlignment.MiddleCenter;

            BackgroundColorButton = new Button();
            BackgroundColorButton.TabIndex = 0;
            BackgroundColorButton.TabStop = false;
            BackgroundColorButton.Location = new Point(0, BackgroundPropertiesLabel.Bottom);
            BackgroundColorButton.Size = new Size(60, 60);
            BackgroundColorButton.FlatStyle = FlatStyle.Flat;
            BackgroundColorButton.BackColor = Colors.Curtain;
            BackgroundColorButton.FlatAppearance.MouseOverBackColor = Colors.CurtainButtonMouseOver;
            BackgroundColorButton.FlatAppearance.MouseDownBackColor = Colors.CurtainButtonMouseDown;
            BackgroundColorButton.FlatAppearance.BorderSize = 0;
            BackgroundColorButton.Image = new Bitmap(@"..\..\..\downloads\Icons\BackgroundColor.png");
            BackgroundColorButton.ImageAlign = ContentAlignment.TopCenter;
            BackgroundColorButton.Font = new Font("Calibri", 10);
            BackgroundColorButton.ForeColor = Colors.Constant;
            BackgroundColorButton.Text = "Color";
            BackgroundColorButton.TextAlign = ContentAlignment.BottomCenter;
            BackgroundColorButton.MouseClick += BackgroundColorButton_MouseClick;

            BackgroundImageButton = new Button();
            BackgroundImageButton.TabIndex = 0;
            BackgroundImageButton.TabStop = false;
            BackgroundImageButton.Location = new Point(60, BackgroundPropertiesLabel.Bottom);
            BackgroundImageButton.Size = new Size(60, 60);
            BackgroundImageButton.FlatStyle = FlatStyle.Flat;
            BackgroundImageButton.BackColor = Colors.Curtain;
            BackgroundImageButton.FlatAppearance.MouseOverBackColor = Colors.CurtainButtonMouseOver;
            BackgroundImageButton.FlatAppearance.MouseDownBackColor = Colors.CurtainButtonMouseDown;
            BackgroundImageButton.FlatAppearance.BorderSize = 0;
            BackgroundImageButton.Image = new Bitmap(@"..\..\..\downloads\Icons\BackgroundImage.png");
            BackgroundImageButton.ImageAlign = ContentAlignment.TopCenter;
            BackgroundImageButton.Font = new Font("Calibri", 10);
            BackgroundImageButton.ForeColor = Colors.Constant;
            BackgroundImageButton.Text = "Image";
            BackgroundImageButton.TextAlign = ContentAlignment.BottomCenter;
            BackgroundImageButton.MouseClick += BackgroundImageButton_MouseClick;

            BackgroundPropertiesPanel.Controls.Add(BackgroundPropertiesLabel);
            BackgroundPropertiesPanel.Controls.Add(BackgroundColorButton);
            BackgroundPropertiesPanel.Controls.Add(BackgroundImageButton);
            #endregion

            #region Switches properties
            SwitchesPropertiesPanel = new Panel();
            SwitchesPropertiesPanel.TabIndex = 0;
            SwitchesPropertiesPanel.TabStop = false;
            SwitchesPropertiesPanel.Location = new Point(BackgroundPropertiesPanel.Right + 30, 0);
            SwitchesPropertiesPanel.Size = new Size(120, 90);
            SwitchesPropertiesPanel.BorderStyle = BorderStyle.None;
            SwitchesPropertiesPanel.BackColor = Colors.Curtain;

            SwitchesPropertiesLabel = new Label();
            SwitchesPropertiesLabel.TabIndex = 0;
            SwitchesPropertiesLabel.Location = new Point(0, 0);
            SwitchesPropertiesLabel.Size = new Size(SwitchesPropertiesPanel.Width, 30);
            SwitchesPropertiesLabel.FlatStyle = FlatStyle.Flat;
            SwitchesPropertiesLabel.BorderStyle = BorderStyle.None;
            SwitchesPropertiesLabel.BackColor = Colors.Curtain;
            SwitchesPropertiesLabel.Font = new Font("Calibri", 14);
            SwitchesPropertiesLabel.ForeColor = Colors.Constant;
            SwitchesPropertiesLabel.Text = "Switches";
            SwitchesPropertiesLabel.TextAlign = ContentAlignment.MiddleCenter;

            PictureBox TopValuePictureBox = new PictureBox();
            TopValuePictureBox.Location = new Point(3, SwitchesPropertiesLabel.Bottom + 3);
            TopValuePictureBox.Size = new Size(24, 24);
            TopValuePictureBox.BorderStyle = BorderStyle.None;
            TopValuePictureBox.BackColor = Colors.Curtain;
            TopValuePictureBox.Image = new Bitmap(@"..\..\..\downloads\Icons\CoordinateY.png");
            TopValuePictureBox.SizeMode = PictureBoxSizeMode.CenterImage;

            TopValueTextBox = new TextBox();
            TopValueTextBox.TabIndex = 0;
            TopValueTextBox.TabStop = false;
            TopValueTextBox.Location = new Point(TopValuePictureBox.Right + 4, TopValuePictureBox.Top);
            TopValueTextBox.Width = 40;
            TopValueTextBox.BorderStyle = BorderStyle.None;
            TopValueTextBox.BackColor = Colors.Curtain;
            TopValueTextBox.Font = new Font("Calibri", 14);
            TopValueTextBox.ForeColor = Colors.Constant;
            TopValueTextBox.Text = "189";
            TopValueTextBox.TextAlign = HorizontalAlignment.Left;
            TopValueTextBox.TextChanged += TopValueTextBox_TextChanged;

            SwitchesBackColorButton = new Button();
            SwitchesBackColorButton.TabIndex = 0;
            SwitchesBackColorButton.TabStop = false;
            SwitchesBackColorButton.Location = new Point(0, 60);
            SwitchesBackColorButton.Size = new Size(30, 30);
            SwitchesBackColorButton.FlatStyle = FlatStyle.Flat;
            SwitchesBackColorButton.BackColor = Colors.Curtain;
            SwitchesBackColorButton.FlatAppearance.MouseOverBackColor = Colors.CurtainButtonMouseOver;
            SwitchesBackColorButton.FlatAppearance.MouseDownBackColor = Colors.CurtainButtonMouseDown;
            SwitchesBackColorButton.FlatAppearance.BorderSize = 0;
            SwitchesBackColorButton.Image = new Bitmap(@"..\..\..\downloads\Icons\Color.png");
            SwitchesBackColorButton.ImageAlign = ContentAlignment.MiddleCenter;
            SwitchesBackColorButton.MouseClick += SwitchesBackColorButton_MouseClick;

            SwitchesMouseOverBackColorButton = new Button();
            SwitchesMouseOverBackColorButton.TabIndex = 0;
            SwitchesMouseOverBackColorButton.TabStop = false;
            SwitchesMouseOverBackColorButton.Location = new Point(30, 60);
            SwitchesMouseOverBackColorButton.Size = new Size(30, 30);
            SwitchesMouseOverBackColorButton.FlatStyle = FlatStyle.Flat;
            SwitchesMouseOverBackColorButton.BackColor = Colors.Curtain;
            SwitchesMouseOverBackColorButton.FlatAppearance.MouseOverBackColor = Colors.CurtainButtonMouseOver;
            SwitchesMouseOverBackColorButton.FlatAppearance.MouseDownBackColor = Colors.CurtainButtonMouseDown;
            SwitchesMouseOverBackColorButton.FlatAppearance.BorderSize = 0;
            SwitchesMouseOverBackColorButton.Image = new Bitmap(@"..\..\..\downloads\Icons\MouseOver.png");
            SwitchesMouseOverBackColorButton.ImageAlign = ContentAlignment.MiddleCenter;
            SwitchesMouseOverBackColorButton.MouseClick += SwitchesMouseOverBackColorButton_MouseClick;

            SwitchesMouseDownBackColorButton = new Button();
            SwitchesMouseDownBackColorButton.TabIndex = 0;
            SwitchesMouseDownBackColorButton.TabStop = false;
            SwitchesMouseDownBackColorButton.Location = new Point(60, 60);
            SwitchesMouseDownBackColorButton.Size = new Size(30, 30);
            SwitchesMouseDownBackColorButton.FlatStyle = FlatStyle.Flat;
            SwitchesMouseDownBackColorButton.BackColor = Colors.Curtain;
            SwitchesMouseDownBackColorButton.FlatAppearance.MouseOverBackColor = Colors.CurtainButtonMouseOver;
            SwitchesMouseDownBackColorButton.FlatAppearance.MouseDownBackColor = Colors.CurtainButtonMouseDown;
            SwitchesMouseDownBackColorButton.FlatAppearance.BorderSize = 0;
            SwitchesMouseDownBackColorButton.Image = new Bitmap(@"..\..\..\downloads\Icons\MouseDown.png");
            SwitchesMouseDownBackColorButton.ImageAlign = ContentAlignment.MiddleCenter;
            SwitchesMouseDownBackColorButton.MouseClick += SwitchesMouseDownBackColorButton_MouseClick;

            SwitchesTransparentBackColorButton = new Button();
            SwitchesTransparentBackColorButton.TabIndex = 0;
            SwitchesTransparentBackColorButton.TabStop = false;
            SwitchesTransparentBackColorButton.Location = new Point(90, 60);
            SwitchesTransparentBackColorButton.Size = new Size(30, 30);
            SwitchesTransparentBackColorButton.FlatStyle = FlatStyle.Flat;
            SwitchesTransparentBackColorButton.BackColor = Colors.Curtain;
            SwitchesTransparentBackColorButton.FlatAppearance.MouseOverBackColor = Colors.CurtainButtonMouseOver;
            SwitchesTransparentBackColorButton.FlatAppearance.MouseDownBackColor = Colors.CurtainButtonMouseDown;
            SwitchesTransparentBackColorButton.FlatAppearance.BorderSize = 0;
            SwitchesTransparentBackColorButton.Image = new Bitmap(@"..\..\..\Downloads\Icons\Transparent.png");
            SwitchesTransparentBackColorButton.ImageAlign = ContentAlignment.MiddleCenter;
            SwitchesTransparentBackColorButton.MouseClick += SwitchesTransparentColorButton_MouseClick;

            SwitchesPropertiesPanel.Controls.Add(SwitchesPropertiesLabel);
            SwitchesPropertiesPanel.Controls.Add(TopValuePictureBox);
            SwitchesPropertiesPanel.Controls.Add(TopValueTextBox);
            SwitchesPropertiesPanel.Controls.Add(SwitchesBackColorButton);
            SwitchesPropertiesPanel.Controls.Add(SwitchesMouseOverBackColorButton);
            SwitchesPropertiesPanel.Controls.Add(SwitchesMouseDownBackColorButton);
            SwitchesPropertiesPanel.Controls.Add(SwitchesTransparentBackColorButton);
            #endregion

            #region Elements
            ElementsPanel = new Panel();
            ElementsPanel.TabIndex = 0;
            ElementsPanel.TabStop = false;
            ElementsPanel.Location = new Point(SwitchesPropertiesPanel.Right + 30, 0);
            ElementsPanel.Size = new Size(300, MenuPanel.Height);
            ElementsPanel.BorderStyle = BorderStyle.None;
            ElementsPanel.BackColor = Colors.Curtain;

            ElementsLabel = new Label();
            ElementsLabel.TabIndex = 0;
            ElementsLabel.Location = new Point(0, 0);
            ElementsLabel.Size = new Size(ElementsPanel.Width, 30);
            ElementsLabel.FlatStyle = FlatStyle.Flat;
            ElementsLabel.BorderStyle = BorderStyle.None;
            ElementsLabel.BackColor = Colors.Curtain;
            ElementsLabel.Font = new Font("Calibri", 14);
            ElementsLabel.ForeColor = Colors.Constant;
            ElementsLabel.Text = "Elements";
            ElementsLabel.TextAlign = ContentAlignment.MiddleCenter;

            TextButton = new Button();
            TextButton.TabIndex = 0;
            TextButton.TabStop = false;
            TextButton.Location = new Point(0, ElementsLabel.Bottom);
            TextButton.Size = new Size(60, 60);
            TextButton.FlatStyle = FlatStyle.Flat;
            TextButton.BackColor = Colors.Curtain;
            TextButton.FlatAppearance.MouseOverBackColor = Colors.CurtainButtonMouseOver;
            TextButton.FlatAppearance.MouseDownBackColor = Colors.CurtainButtonMouseDown;
            TextButton.FlatAppearance.BorderSize = 0;
            TextButton.Image = new Bitmap(@"..\..\..\downloads\Icons\AddText.png");
            TextButton.ImageAlign = ContentAlignment.TopCenter;
            TextButton.Font = new Font("Calibri", 10);
            TextButton.ForeColor = Colors.Constant;
            TextButton.Text = "Text";
            TextButton.TextAlign = ContentAlignment.BottomCenter;
            TextButton.MouseClick += TextButton_MouseClick;

            InputButton = new Button();
            InputButton.TabIndex = 0;
            InputButton.TabStop = false;
            InputButton.Location = new Point(60, ElementsLabel.Bottom);
            InputButton.Size = new Size(60, 60);
            InputButton.FlatStyle = FlatStyle.Flat;
            InputButton.BackColor = Colors.Curtain;
            InputButton.FlatAppearance.MouseOverBackColor = Colors.CurtainButtonMouseOver;
            InputButton.FlatAppearance.MouseDownBackColor = Colors.CurtainButtonMouseDown;
            InputButton.FlatAppearance.BorderSize = 0;
            InputButton.Image = new Bitmap(@"..\..\..\downloads\Icons\AddInput.png");
            InputButton.ImageAlign = ContentAlignment.TopCenter;
            InputButton.Font = new Font("Calibri", 10);
            InputButton.ForeColor = Colors.Constant;
            InputButton.Text = "Input";
            InputButton.TextAlign = ContentAlignment.BottomCenter;
            InputButton.MouseClick += InputButton_MouseClick;

            PictureButton = new Button();
            PictureButton.TabIndex = 0;
            PictureButton.TabStop = false;
            PictureButton.Location = new Point(120, ElementsLabel.Bottom);
            PictureButton.Size = new Size(60, 60);
            PictureButton.FlatStyle = FlatStyle.Flat;
            PictureButton.BackColor = Colors.Curtain;
            PictureButton.FlatAppearance.MouseOverBackColor = Colors.CurtainButtonMouseOver;
            PictureButton.FlatAppearance.MouseDownBackColor = Colors.CurtainButtonMouseDown;
            PictureButton.FlatAppearance.BorderSize = 0;
            PictureButton.Image = new Bitmap(@"..\..\..\downloads\Icons\AddPicture.png");
            PictureButton.ImageAlign = ContentAlignment.TopCenter;
            PictureButton.Font = new Font("Calibri", 10);
            PictureButton.ForeColor = Colors.Constant;
            PictureButton.Text = "Picture";
            PictureButton.TextAlign = ContentAlignment.BottomCenter;
            PictureButton.MouseClick += PictureButton_MouseClick;

            PanelButton = new Button();
            PanelButton.TabIndex = 0;
            PanelButton.TabStop = false;
            PanelButton.Location = new Point(180, ElementsLabel.Bottom);
            PanelButton.Size = new Size(60, 60);
            PanelButton.FlatStyle = FlatStyle.Flat;
            PanelButton.BackColor = Colors.Curtain;
            PanelButton.FlatAppearance.MouseOverBackColor = Colors.CurtainButtonMouseOver;
            PanelButton.FlatAppearance.MouseDownBackColor = Colors.CurtainButtonMouseDown;
            PanelButton.FlatAppearance.BorderSize = 0;
            PanelButton.Image = new Bitmap(@"..\..\..\downloads\Icons\AddPanel.png");
            PanelButton.ImageAlign = ContentAlignment.TopCenter;
            PanelButton.Font = new Font("Calibri", 10);
            PanelButton.ForeColor = Colors.Constant;
            PanelButton.Text = "Panel";
            PanelButton.TextAlign = ContentAlignment.BottomCenter;
            PanelButton.MouseClick += PanelButton_MouseClick;

            ButtonButton = new Button();
            ButtonButton.TabIndex = 0;
            ButtonButton.TabStop = false;
            ButtonButton.Location = new Point(240, ElementsLabel.Bottom);
            ButtonButton.Size = new Size(60, 60);
            ButtonButton.FlatStyle = FlatStyle.Flat;
            ButtonButton.BackColor = Colors.Curtain;
            ButtonButton.FlatAppearance.MouseOverBackColor = Colors.CurtainButtonMouseOver;
            ButtonButton.FlatAppearance.MouseDownBackColor = Colors.CurtainButtonMouseDown;
            ButtonButton.FlatAppearance.BorderSize = 0;
            ButtonButton.Image = new Bitmap(@"..\..\..\downloads\Icons\AddButton.png");
            ButtonButton.ImageAlign = ContentAlignment.TopCenter;
            ButtonButton.Font = new Font("Calibri", 10);
            ButtonButton.ForeColor = Colors.Constant;
            ButtonButton.Text = "Button";
            ButtonButton.TextAlign = ContentAlignment.BottomCenter;
            ButtonButton.MouseClick += ButtonButton_MouseClick;

            ElementsPanel.Controls.Add(ElementsLabel);
            ElementsPanel.Controls.Add(TextButton);
            ElementsPanel.Controls.Add(InputButton);
            ElementsPanel.Controls.Add(PictureButton);
            ElementsPanel.Controls.Add(PanelButton);
            ElementsPanel.Controls.Add(ButtonButton);
            #endregion

            #region Common properties
            CommonPropertiesPanel = new Panel();
            CommonPropertiesPanel.TabIndex = 0;
            CommonPropertiesPanel.TabStop = false;
            CommonPropertiesPanel.Location = new Point(0, 0);
            CommonPropertiesPanel.Size = new Size(166, 90);
            CommonPropertiesPanel.BorderStyle = BorderStyle.None;
            CommonPropertiesPanel.BackColor = Colors.Curtain;
            CommonPropertiesPanel.Visible = false;

            CommonPropertiesLabel = new Label();
            CommonPropertiesLabel.TabIndex = 0;
            CommonPropertiesLabel.Location = new Point(0, 0);
            CommonPropertiesLabel.Size = new Size(CommonPropertiesPanel.Width, 30);
            CommonPropertiesLabel.FlatStyle = FlatStyle.Flat;
            CommonPropertiesLabel.BorderStyle = BorderStyle.None;
            CommonPropertiesLabel.BackColor = Colors.Curtain;
            CommonPropertiesLabel.Font = new Font("Calibri", 14);
            CommonPropertiesLabel.ForeColor = Colors.Constant;
            CommonPropertiesLabel.Text = "Common";
            CommonPropertiesLabel.TextAlign = ContentAlignment.MiddleCenter;

            PictureBox PointXPictureBox = new PictureBox();
            PointXPictureBox.Location = new Point(0, CommonPropertiesLabel.Bottom + 4);
            PointXPictureBox.Size = new Size(24, 24);
            PointXPictureBox.BorderStyle = BorderStyle.None;
            PointXPictureBox.BackColor = Colors.Curtain;
            PointXPictureBox.Image = new Bitmap(@"..\..\..\downloads\Icons\CoordinateX.png");
            PointXPictureBox.SizeMode = PictureBoxSizeMode.CenterImage;

            PointXTextBox = new TextBox();
            PointXTextBox.TabIndex = 0;
            PointXTextBox.TabStop = false;
            PointXTextBox.Location = new Point(PointXPictureBox.Right + 4, PointXPictureBox.Top);
            PointXTextBox.Width = 40;
            PointXTextBox.BorderStyle = BorderStyle.None;
            PointXTextBox.BackColor = Colors.Curtain;
            PointXTextBox.Font = new Font("Calibri", 14);
            PointXTextBox.ForeColor = Colors.Constant;
            PointXTextBox.TextAlign = HorizontalAlignment.Left;
            PointXTextBox.TextChanged += PointXTextBox_TextChanged;

            PictureBox PointYPictureBox = new PictureBox();
            PointYPictureBox.Location = new Point(PointXTextBox.Right, PointXTextBox.Top);
            PointYPictureBox.Size = new Size(24, 24);
            PointYPictureBox.BorderStyle = BorderStyle.None;
            PointYPictureBox.BackColor = Colors.Curtain;
            PointYPictureBox.Image = new Bitmap(@"..\..\..\downloads\Icons\CoordinateY.png");
            PointYPictureBox.SizeMode = PictureBoxSizeMode.CenterImage;

            PointYTextBox = new TextBox();
            PointYTextBox.TabIndex = 0;
            PointYTextBox.TabStop = false;
            PointYTextBox.Location = new Point(PointYPictureBox.Right + 4, PointYPictureBox.Top);
            PointYTextBox.Width = 40;
            PointYTextBox.BorderStyle = BorderStyle.None;
            PointYTextBox.BackColor = Colors.Curtain;
            PointYTextBox.Font = new Font("Calibri", 14);
            PointYTextBox.ForeColor = Colors.Constant;
            PointYTextBox.TextAlign = HorizontalAlignment.Left;
            PointYTextBox.TextChanged += PointYTextBox_TextChanged;

            PictureBox WidthPictureBox = new PictureBox();
            WidthPictureBox.Location = new Point(0, PointXPictureBox.Bottom + 5);
            WidthPictureBox.Size = new Size(24, 24);
            WidthPictureBox.BorderStyle = BorderStyle.None;
            WidthPictureBox.BackColor = Colors.Curtain;
            WidthPictureBox.Image = new Bitmap(@"..\..\..\downloads\Icons\Width.png");
            WidthPictureBox.SizeMode = PictureBoxSizeMode.CenterImage;

            WidthTextBox = new TextBox();
            WidthTextBox.TabIndex = 0;
            WidthTextBox.TabStop = false;
            WidthTextBox.Location = new Point(WidthPictureBox.Right + 4, WidthPictureBox.Top);
            WidthTextBox.Width = 40;
            WidthTextBox.BorderStyle = BorderStyle.None;
            WidthTextBox.BackColor = Colors.Curtain;
            WidthTextBox.Font = new Font("Calibri", 14);
            WidthTextBox.ForeColor = Colors.Constant;
            WidthTextBox.TextAlign = HorizontalAlignment.Left;
            WidthTextBox.TextChanged += WidthTextBox_TextChanged;

            PictureBox HeightPictureBox = new PictureBox();
            HeightPictureBox.Location = new Point(WidthTextBox.Right, WidthTextBox.Top);
            HeightPictureBox.Size = new Size(24, 24);
            HeightPictureBox.BorderStyle = BorderStyle.None;
            HeightPictureBox.BackColor = Colors.Curtain;
            HeightPictureBox.Image = new Bitmap(@"..\..\..\downloads\Icons\Height.png");
            HeightPictureBox.SizeMode = PictureBoxSizeMode.CenterImage;

            HeightTextBox = new TextBox();
            HeightTextBox.TabIndex = 0;
            HeightTextBox.TabStop = false;
            HeightTextBox.Location = new Point(HeightPictureBox.Right + 4, HeightPictureBox.Top);
            HeightTextBox.Width = 40;
            HeightTextBox.BorderStyle = BorderStyle.None;
            HeightTextBox.BackColor = Colors.Curtain;
            HeightTextBox.Font = new Font("Calibri", 14);
            HeightTextBox.ForeColor = Colors.Constant;
            HeightTextBox.TextAlign = HorizontalAlignment.Left;
            HeightTextBox.TextChanged += HeightTextBox_TextChanged;

            BackColorButton = new Button();
            BackColorButton.TabIndex = 0;
            BackColorButton.TabStop = false;
            BackColorButton.Location = new Point(PointYTextBox.Right, CommonPropertiesLabel.Bottom);
            BackColorButton.Size = new Size(30, 30);
            BackColorButton.FlatStyle = FlatStyle.Flat;
            BackColorButton.BackColor = Colors.Curtain;
            BackColorButton.FlatAppearance.MouseOverBackColor = Colors.CurtainButtonMouseOver;
            BackColorButton.FlatAppearance.MouseDownBackColor = Colors.CurtainButtonMouseDown;
            BackColorButton.FlatAppearance.BorderSize = 0;
            BackColorButton.Image = new Bitmap(@"..\..\..\downloads\Icons\Color.png");
            BackColorButton.ImageAlign = ContentAlignment.MiddleCenter;
            BackColorButton.MouseClick += ColorButton_MouseClick;

            TransparentBackColorButton = new Button();
            TransparentBackColorButton.TabIndex = 0;
            TransparentBackColorButton.TabStop = false;
            TransparentBackColorButton.Location = new Point(HeightTextBox.Right, BackColorButton.Bottom);
            TransparentBackColorButton.Size = new Size(30, 30);
            TransparentBackColorButton.FlatStyle = FlatStyle.Flat;
            TransparentBackColorButton.BackColor = Colors.Curtain;
            TransparentBackColorButton.FlatAppearance.MouseOverBackColor = Colors.CurtainButtonMouseOver;
            TransparentBackColorButton.FlatAppearance.MouseDownBackColor = Colors.CurtainButtonMouseDown;
            TransparentBackColorButton.FlatAppearance.BorderSize = 0;
            TransparentBackColorButton.Image = new Bitmap(@"..\..\..\downloads\Icons\Transparent.png");
            TransparentBackColorButton.ImageAlign = ContentAlignment.MiddleCenter;
            TransparentBackColorButton.MouseClick += TransparentBackColorButton_MouseClick;

            CommonPropertiesPanel.Controls.Add(CommonPropertiesLabel);
            CommonPropertiesPanel.Controls.Add(PointXPictureBox);
            CommonPropertiesPanel.Controls.Add(PointXTextBox);
            CommonPropertiesPanel.Controls.Add(PointYPictureBox);
            CommonPropertiesPanel.Controls.Add(PointYTextBox);
            CommonPropertiesPanel.Controls.Add(WidthPictureBox);
            CommonPropertiesPanel.Controls.Add(WidthTextBox);
            CommonPropertiesPanel.Controls.Add(HeightPictureBox);
            CommonPropertiesPanel.Controls.Add(HeightTextBox);
            CommonPropertiesPanel.Controls.Add(BackColorButton);
            CommonPropertiesPanel.Controls.Add(TransparentBackColorButton);
            #endregion

            #region Text properties
            TextPropertiesPanel = new Panel();
            TextPropertiesPanel.TabIndex = 0;
            TextPropertiesPanel.TabStop = false;
            TextPropertiesPanel.Location = new Point(0, 0);
            TextPropertiesPanel.Size = new Size(210, 90);
            TextPropertiesPanel.BorderStyle = BorderStyle.None;
            TextPropertiesPanel.BackColor = Colors.Curtain;
            TextPropertiesPanel.Visible = false;

            TextPropertiesLabel = new Label();
            TextPropertiesLabel.TabIndex = 0;
            TextPropertiesLabel.Location = new Point(0, 0);
            TextPropertiesLabel.Size = new Size(TextPropertiesPanel.Width, 30);
            TextPropertiesLabel.FlatStyle = FlatStyle.Flat;
            TextPropertiesLabel.BorderStyle = BorderStyle.None;
            TextPropertiesLabel.BackColor = Colors.Curtain;
            TextPropertiesLabel.Font = new Font("Calibri", 14);
            TextPropertiesLabel.ForeColor = Colors.Constant;
            TextPropertiesLabel.Text = "Font";
            TextPropertiesLabel.TextAlign = ContentAlignment.MiddleCenter;

            PictureBox FontSizePictureBox = new PictureBox();
            FontSizePictureBox.Location = new Point(0, TextPropertiesLabel.Bottom + 3);
            FontSizePictureBox.Size = new Size(24, 24);
            FontSizePictureBox.BorderStyle = BorderStyle.None;
            FontSizePictureBox.BackColor = Colors.Curtain;
            FontSizePictureBox.Image = new Bitmap(@"..\..\..\downloads\Icons\FontSize.png");
            FontSizePictureBox.SizeMode = PictureBoxSizeMode.CenterImage;

            FontSizeTextBox = new TextBox();
            FontSizeTextBox.TabIndex = 0;
            FontSizeTextBox.TabStop = false;
            FontSizeTextBox.Location = new Point(FontSizePictureBox.Right + 6, FontSizePictureBox.Top + 1);
            FontSizeTextBox.Width = 60;
            FontSizeTextBox.BorderStyle = BorderStyle.None;
            FontSizeTextBox.BackColor = Colors.Curtain;
            FontSizeTextBox.Font = new Font("Calibri", 14);
            FontSizeTextBox.ForeColor = Colors.Constant;
            FontSizeTextBox.TextAlign = HorizontalAlignment.Left;
            FontSizeTextBox.TextChanged += FontSizeTextBox_TextChanged;

            FontSizePlusButton = new Button();
            FontSizePlusButton.TabIndex = 0;
            FontSizePlusButton.TabStop = false;
            FontSizePlusButton.Location = new Point(FontSizeTextBox.Right, TextPropertiesLabel.Bottom);
            FontSizePlusButton.Size = new Size(30, 30);
            FontSizePlusButton.FlatStyle = FlatStyle.Flat;
            FontSizePlusButton.BackColor = Colors.Curtain;
            FontSizePlusButton.FlatAppearance.MouseOverBackColor = Colors.CurtainButtonMouseOver;
            FontSizePlusButton.FlatAppearance.MouseDownBackColor = Colors.CurtainButtonMouseDown;
            FontSizePlusButton.FlatAppearance.BorderSize = 0;
            FontSizePlusButton.Image = new Bitmap(@"..\..\..\downloads\Icons\FontSizePlus.png");
            FontSizePlusButton.ImageAlign = ContentAlignment.MiddleCenter;
            FontSizePlusButton.MouseClick += FontSizePlusButton_MouseClick;

            FontSizeMinusButton = new Button();
            FontSizeMinusButton.TabIndex = 0;
            FontSizeMinusButton.TabStop = false;
            FontSizeMinusButton.Location = new Point(FontSizePlusButton.Right, TextPropertiesLabel.Bottom);
            FontSizeMinusButton.Size = new Size(30, 30);
            FontSizeMinusButton.FlatStyle = FlatStyle.Flat;
            FontSizeMinusButton.BackColor = Colors.Curtain;
            FontSizeMinusButton.FlatAppearance.MouseOverBackColor = Colors.CurtainButtonMouseOver;
            FontSizeMinusButton.FlatAppearance.MouseDownBackColor = Colors.CurtainButtonMouseDown;
            FontSizeMinusButton.FlatAppearance.BorderSize = 0;
            FontSizeMinusButton.Image = new Bitmap(@"..\..\..\downloads\Icons\FontSizeMinus.png");
            FontSizeMinusButton.ImageAlign = ContentAlignment.MiddleCenter;
            FontSizeMinusButton.MouseClick += FontSizeMinusButton_MouseClick;

            FontButton = new Button();
            FontButton.TabIndex = 0;
            FontButton.TabStop = false;
            FontButton.Location = new Point(FontSizeMinusButton.Right, TextPropertiesLabel.Bottom);
            FontButton.Size = new Size(30, 30);
            FontButton.FlatStyle = FlatStyle.Flat;
            FontButton.BackColor = Colors.Curtain;
            FontButton.FlatAppearance.MouseOverBackColor = Colors.CurtainButtonMouseOver;
            FontButton.FlatAppearance.MouseDownBackColor = Colors.CurtainButtonMouseDown;
            FontButton.FlatAppearance.BorderSize = 0;
            FontButton.Image = new Bitmap(@"..\..\..\downloads\Icons\Font.png");
            FontButton.ImageAlign = ContentAlignment.MiddleCenter;
            FontButton.MouseClick += FontButton_MouseClick;

            ForeColorButton = new Button();
            ForeColorButton.TabIndex = 0;
            ForeColorButton.TabStop = false;
            ForeColorButton.Location = new Point(FontButton.Right, TextPropertiesLabel.Bottom);
            ForeColorButton.Size = new Size(30, 30);
            ForeColorButton.FlatStyle = FlatStyle.Flat;
            ForeColorButton.BackColor = Colors.Curtain;
            ForeColorButton.FlatAppearance.MouseOverBackColor = Colors.CurtainButtonMouseOver;
            ForeColorButton.FlatAppearance.MouseDownBackColor = Colors.CurtainButtonMouseDown;
            ForeColorButton.FlatAppearance.BorderSize = 0;
            ForeColorButton.Image = new Bitmap(@"..\..\..\downloads\Icons\Color.png");
            ForeColorButton.ImageAlign = ContentAlignment.MiddleCenter;
            ForeColorButton.MouseClick += ForeColorButton_MouseClick;

            BoldTextButton = new Button();
            BoldTextButton.TabIndex = 0;
            BoldTextButton.TabStop = false;
            BoldTextButton.Location = new Point(0, ForeColorButton.Bottom);
            BoldTextButton.Size = new Size(30, 30);
            BoldTextButton.FlatStyle = FlatStyle.Flat;
            BoldTextButton.BackColor = Colors.Curtain;
            BoldTextButton.FlatAppearance.MouseOverBackColor = Colors.CurtainButtonMouseOver;
            BoldTextButton.FlatAppearance.MouseDownBackColor = Colors.CurtainButtonMouseDown;
            BoldTextButton.FlatAppearance.BorderSize = 0;
            BoldTextButton.Image = new Bitmap(@"..\..\..\downloads\Icons\Bold.png");
            BoldTextButton.ImageAlign = ContentAlignment.MiddleCenter;
            BoldTextButton.MouseClick += BoldTextButton_MouseClick;

            ItalicTextButton = new Button();
            ItalicTextButton.TabIndex = 0;
            ItalicTextButton.TabStop = false;
            ItalicTextButton.Location = new Point(BoldTextButton.Right, ForeColorButton.Bottom);
            ItalicTextButton.Size = new Size(30, 30);
            ItalicTextButton.FlatStyle = FlatStyle.Flat;
            ItalicTextButton.BackColor = Colors.Curtain;
            ItalicTextButton.FlatAppearance.MouseOverBackColor = Colors.CurtainButtonMouseOver;
            ItalicTextButton.FlatAppearance.MouseDownBackColor = Colors.CurtainButtonMouseDown;
            ItalicTextButton.FlatAppearance.BorderSize = 0;
            ItalicTextButton.Image = new Bitmap(@"..\..\..\downloads\Icons\Italic.png");
            ItalicTextButton.ImageAlign = ContentAlignment.MiddleCenter;
            ItalicTextButton.MouseClick += ItalicTextButton_MouseClick;

            UnderlineTextButton = new Button();
            UnderlineTextButton.TabIndex = 0;
            UnderlineTextButton.TabStop = false;
            UnderlineTextButton.Location = new Point(ItalicTextButton.Right, ForeColorButton.Bottom);
            UnderlineTextButton.Size = new Size(30, 30);
            UnderlineTextButton.FlatStyle = FlatStyle.Flat;
            UnderlineTextButton.BackColor = Colors.Curtain;
            UnderlineTextButton.FlatAppearance.MouseOverBackColor = Colors.CurtainButtonMouseOver;
            UnderlineTextButton.FlatAppearance.MouseDownBackColor = Colors.CurtainButtonMouseDown;
            UnderlineTextButton.FlatAppearance.BorderSize = 0;
            UnderlineTextButton.Image = new Bitmap(@"..\..\..\downloads\Icons\Underline.png");
            UnderlineTextButton.ImageAlign = ContentAlignment.MiddleCenter;
            UnderlineTextButton.MouseClick += UnderlineTextButton_MouseClick;

            StrikeoutTextButton = new Button();
            StrikeoutTextButton.TabIndex = 0;
            StrikeoutTextButton.TabStop = false;
            StrikeoutTextButton.Location = new Point(UnderlineTextButton.Right, ForeColorButton.Bottom);
            StrikeoutTextButton.Size = new Size(30, 30);
            StrikeoutTextButton.FlatStyle = FlatStyle.Flat;
            StrikeoutTextButton.BackColor = Colors.Curtain;
            StrikeoutTextButton.FlatAppearance.MouseOverBackColor = Colors.CurtainButtonMouseOver;
            StrikeoutTextButton.FlatAppearance.MouseDownBackColor = Colors.CurtainButtonMouseDown;
            StrikeoutTextButton.FlatAppearance.BorderSize = 0;
            StrikeoutTextButton.Image = new Bitmap(@"..\..\..\downloads\Icons\Strikeout.png");
            StrikeoutTextButton.ImageAlign = ContentAlignment.MiddleCenter;
            StrikeoutTextButton.MouseClick += StrikeoutTextButton_MouseClick;

            TextAlignLeftButton = new Button();
            TextAlignLeftButton.TabIndex = 0;
            TextAlignLeftButton.TabStop = false;
            TextAlignLeftButton.Location = new Point(StrikeoutTextButton.Right, ForeColorButton.Bottom);
            TextAlignLeftButton.Size = new Size(30, 30);
            TextAlignLeftButton.FlatStyle = FlatStyle.Flat;
            TextAlignLeftButton.BackColor = Colors.Curtain;
            TextAlignLeftButton.FlatAppearance.MouseOverBackColor = Colors.CurtainButtonMouseOver;
            TextAlignLeftButton.FlatAppearance.MouseDownBackColor = Colors.CurtainButtonMouseDown;
            TextAlignLeftButton.FlatAppearance.BorderSize = 0;
            TextAlignLeftButton.Image = new Bitmap(@"..\..\..\downloads\Icons\EditorTextAlignLeft.png");
            TextAlignLeftButton.ImageAlign = ContentAlignment.MiddleCenter;
            TextAlignLeftButton.MouseClick += TextAlignLeftButton_MouseClick;

            TextAlignCenterButton = new Button();
            TextAlignCenterButton.TabIndex = 0;
            TextAlignCenterButton.TabStop = false;
            TextAlignCenterButton.Location = new Point(TextAlignLeftButton.Right, ForeColorButton.Bottom);
            TextAlignCenterButton.Size = new Size(30, 30);
            TextAlignCenterButton.FlatStyle = FlatStyle.Flat;
            TextAlignCenterButton.BackColor = Colors.Curtain;
            TextAlignCenterButton.FlatAppearance.MouseOverBackColor = Colors.CurtainButtonMouseOver;
            TextAlignCenterButton.FlatAppearance.MouseDownBackColor = Colors.CurtainButtonMouseDown;
            TextAlignCenterButton.FlatAppearance.BorderSize = 0;
            TextAlignCenterButton.Image = new Bitmap(@"..\..\..\downloads\Icons\EditorTextAlignCenter.png");
            TextAlignCenterButton.ImageAlign = ContentAlignment.MiddleCenter;
            TextAlignCenterButton.MouseClick += TextAlignCenterButton_MouseClick;

            TextAlignRightButton = new Button();
            TextAlignRightButton.TabIndex = 0;
            TextAlignRightButton.TabStop = false;
            TextAlignRightButton.Location = new Point(TextAlignCenterButton.Right, ForeColorButton.Bottom);
            TextAlignRightButton.Size = new Size(30, 30);
            TextAlignRightButton.FlatStyle = FlatStyle.Flat;
            TextAlignRightButton.BackColor = Colors.Curtain;
            TextAlignRightButton.FlatAppearance.MouseOverBackColor = Colors.CurtainButtonMouseOver;
            TextAlignRightButton.FlatAppearance.MouseDownBackColor = Colors.CurtainButtonMouseDown;
            TextAlignRightButton.FlatAppearance.BorderSize = 0;
            TextAlignRightButton.Image = new Bitmap(@"..\..\..\downloads\Icons\EditorTextAlignRight.png");
            TextAlignRightButton.ImageAlign = ContentAlignment.MiddleCenter;
            TextAlignRightButton.MouseClick += TextAlignRightButton_MouseClick;

            TextPropertiesPanel.Controls.Add(TextPropertiesLabel);
            TextPropertiesPanel.Controls.Add(FontSizePictureBox);
            TextPropertiesPanel.Controls.Add(FontSizeTextBox);
            TextPropertiesPanel.Controls.Add(FontSizePlusButton);
            TextPropertiesPanel.Controls.Add(FontSizeMinusButton);
            TextPropertiesPanel.Controls.Add(FontButton);
            TextPropertiesPanel.Controls.Add(ForeColorButton);
            TextPropertiesPanel.Controls.Add(BoldTextButton);
            TextPropertiesPanel.Controls.Add(ItalicTextButton);
            TextPropertiesPanel.Controls.Add(UnderlineTextButton);
            TextPropertiesPanel.Controls.Add(StrikeoutTextButton);
            TextPropertiesPanel.Controls.Add(TextAlignLeftButton);
            TextPropertiesPanel.Controls.Add(TextAlignCenterButton);
            TextPropertiesPanel.Controls.Add(TextAlignRightButton);
            #endregion

            #region Image properties
            ImagePropertiesPanel = new Panel();
            ImagePropertiesPanel.TabIndex = 0;
            ImagePropertiesPanel.TabStop = false;
            ImagePropertiesPanel.Location = new Point(0, 0);
            ImagePropertiesPanel.Size = new Size(300, 90);
            ImagePropertiesPanel.BorderStyle = BorderStyle.None;
            ImagePropertiesPanel.BackColor = Colors.Curtain;
            ImagePropertiesPanel.Visible = false;

            ImagePropertiesLabel = new Label();
            ImagePropertiesLabel.TabIndex = 0;
            ImagePropertiesLabel.Location = new Point(0, 0);
            ImagePropertiesLabel.Size = new Size(ImagePropertiesPanel.Width, 30);
            ImagePropertiesLabel.FlatStyle = FlatStyle.Flat;
            ImagePropertiesLabel.BorderStyle = BorderStyle.None;
            ImagePropertiesLabel.BackColor = Colors.Curtain;
            ImagePropertiesLabel.Font = new Font("Calibri", 14);
            ImagePropertiesLabel.ForeColor = Colors.Constant;
            ImagePropertiesLabel.Text = "Picture size";
            ImagePropertiesLabel.TextAlign = ContentAlignment.MiddleCenter;

            SizeMode1Button = new Button();
            SizeMode1Button.TabIndex = 0;
            SizeMode1Button.TabStop = false;
            SizeMode1Button.Location = new Point(0, ImagePropertiesLabel.Bottom);
            SizeMode1Button.Size = new Size(60, 60);
            SizeMode1Button.FlatStyle = FlatStyle.Flat;
            SizeMode1Button.BackColor = Colors.Curtain;
            SizeMode1Button.FlatAppearance.MouseOverBackColor = Colors.CurtainButtonMouseOver;
            SizeMode1Button.FlatAppearance.MouseDownBackColor = Colors.CurtainButtonMouseDown;
            SizeMode1Button.FlatAppearance.BorderSize = 0;
            SizeMode1Button.Image = new Bitmap(@"..\..\..\downloads\Icons\SizeModeAuto.png");
            SizeMode1Button.ImageAlign = ContentAlignment.TopCenter;
            SizeMode1Button.Font = new Font("Calibri", 10);
            SizeMode1Button.ForeColor = Colors.Constant;
            SizeMode1Button.Text = "Auto";
            SizeMode1Button.TextAlign = ContentAlignment.BottomCenter;
            SizeMode1Button.MouseClick += SizeMode1Button_MouseClick;

            SizeMode2Button = new Button();
            SizeMode2Button.TabIndex = 0;
            SizeMode2Button.TabStop = false;
            SizeMode2Button.Location = new Point(60, ImagePropertiesLabel.Bottom);
            SizeMode2Button.Size = new Size(60, 60);
            SizeMode2Button.FlatStyle = FlatStyle.Flat;
            SizeMode2Button.BackColor = Colors.Curtain;
            SizeMode2Button.FlatAppearance.MouseOverBackColor = Colors.CurtainButtonMouseOver;
            SizeMode2Button.FlatAppearance.MouseDownBackColor = Colors.CurtainButtonMouseDown;
            SizeMode2Button.FlatAppearance.BorderSize = 0;
            SizeMode2Button.Image = new Bitmap(@"..\..\..\downloads\Icons\SizeModeCenter.png");
            SizeMode2Button.ImageAlign = ContentAlignment.TopCenter;
            SizeMode2Button.Font = new Font("Calibri", 10);
            SizeMode2Button.ForeColor = Colors.Constant;
            SizeMode2Button.Text = "Center";
            SizeMode2Button.TextAlign = ContentAlignment.BottomCenter;
            SizeMode2Button.MouseClick += SizeMode2Button_MouseClick;

            SizeMode3Button = new Button();
            SizeMode3Button.TabIndex = 0;
            SizeMode3Button.TabStop = false;
            SizeMode3Button.Location = new Point(120, ImagePropertiesLabel.Bottom);
            SizeMode3Button.Size = new Size(60, 60);
            SizeMode3Button.FlatStyle = FlatStyle.Flat;
            SizeMode3Button.BackColor = Colors.Curtain;
            SizeMode3Button.FlatAppearance.MouseOverBackColor = Colors.CurtainButtonMouseOver;
            SizeMode3Button.FlatAppearance.MouseDownBackColor = Colors.CurtainButtonMouseDown;
            SizeMode3Button.FlatAppearance.BorderSize = 0;
            SizeMode3Button.Image = new Bitmap(@"..\..\..\downloads\Icons\SizeModeNormal.png");
            SizeMode3Button.ImageAlign = ContentAlignment.TopCenter;
            SizeMode3Button.Font = new Font("Calibri", 10);
            SizeMode3Button.ForeColor = Colors.Constant;
            SizeMode3Button.Text = "Normal";
            SizeMode3Button.TextAlign = ContentAlignment.BottomCenter;
            SizeMode3Button.MouseClick += SizeMode3Button_MouseClick;

            SizeMode4Button = new Button();
            SizeMode4Button.TabIndex = 0;
            SizeMode4Button.TabStop = false;
            SizeMode4Button.Location = new Point(180, ImagePropertiesLabel.Bottom);
            SizeMode4Button.Size = new Size(60, 60);
            SizeMode4Button.FlatStyle = FlatStyle.Flat;
            SizeMode4Button.BackColor = Colors.Curtain;
            SizeMode4Button.FlatAppearance.MouseOverBackColor = Colors.CurtainButtonMouseOver;
            SizeMode4Button.FlatAppearance.MouseDownBackColor = Colors.CurtainButtonMouseDown;
            SizeMode4Button.FlatAppearance.BorderSize = 0;
            SizeMode4Button.Image = new Bitmap(@"..\..\..\downloads\Icons\SizeModeStretch.png");
            SizeMode4Button.ImageAlign = ContentAlignment.TopCenter;
            SizeMode4Button.Font = new Font("Calibri", 10);
            SizeMode4Button.ForeColor = Colors.Constant;
            SizeMode4Button.Text = "Stretch";
            SizeMode4Button.TextAlign = ContentAlignment.BottomCenter;
            SizeMode4Button.MouseClick += SizeMode4Button_MouseClick;

            SizeMode5Button = new Button();
            SizeMode5Button.TabIndex = 0;
            SizeMode5Button.TabStop = false;
            SizeMode5Button.Location = new Point(240, ImagePropertiesLabel.Bottom);
            SizeMode5Button.Size = new Size(60, 60);
            SizeMode5Button.FlatStyle = FlatStyle.Flat;
            SizeMode5Button.BackColor = Colors.Curtain;
            SizeMode5Button.FlatAppearance.MouseOverBackColor = Colors.CurtainButtonMouseOver;
            SizeMode5Button.FlatAppearance.MouseDownBackColor = Colors.CurtainButtonMouseDown;
            SizeMode5Button.FlatAppearance.BorderSize = 0;
            SizeMode5Button.Image = new Bitmap(@"..\..\..\downloads\Icons\SizeModeZoom.png");
            SizeMode5Button.ImageAlign = ContentAlignment.TopCenter;
            SizeMode5Button.Font = new Font("Calibri", 10);
            SizeMode5Button.ForeColor = Colors.Constant;
            SizeMode5Button.Text = "Zoom";
            SizeMode5Button.TextAlign = ContentAlignment.BottomCenter;
            SizeMode5Button.MouseClick += SizeMode5Button_MouseClick;

            ImagePropertiesPanel.Controls.Add(ImagePropertiesLabel);
            ImagePropertiesPanel.Controls.Add(SizeMode1Button);
            ImagePropertiesPanel.Controls.Add(SizeMode2Button);
            ImagePropertiesPanel.Controls.Add(SizeMode3Button);
            ImagePropertiesPanel.Controls.Add(SizeMode4Button);
            ImagePropertiesPanel.Controls.Add(SizeMode5Button);
            #endregion

            #region Z-index properties
            ZindexPropertiesPanel = new Panel();
            ZindexPropertiesPanel.TabIndex = 0;
            ZindexPropertiesPanel.TabStop = false;
            ZindexPropertiesPanel.Location = new Point(0, 0);
            ZindexPropertiesPanel.Size = new Size(120, 90);
            ZindexPropertiesPanel.BorderStyle = BorderStyle.None;
            ZindexPropertiesPanel.BackColor = Colors.Curtain;
            ZindexPropertiesPanel.Visible = false;

            ZindexPropertiesLabel = new Label();
            ZindexPropertiesLabel.TabIndex = 0;
            ZindexPropertiesLabel.Location = new Point(0, 0);
            ZindexPropertiesLabel.Size = new Size(ZindexPropertiesPanel.Width, 30);
            ZindexPropertiesLabel.FlatStyle = FlatStyle.Flat;
            ZindexPropertiesLabel.BorderStyle = BorderStyle.None;
            ZindexPropertiesLabel.BackColor = Colors.Curtain;
            ZindexPropertiesLabel.Font = new Font("Calibri", 14);
            ZindexPropertiesLabel.ForeColor = Colors.Constant;
            ZindexPropertiesLabel.Text = "Z-index";
            ZindexPropertiesLabel.TextAlign = ContentAlignment.MiddleCenter;

            BringToFrontButton = new Button();
            BringToFrontButton.TabIndex = 0;
            BringToFrontButton.TabStop = false;
            BringToFrontButton.Location = new Point(0, ZindexPropertiesLabel.Bottom);
            BringToFrontButton.Size = new Size(60, 60);
            BringToFrontButton.FlatStyle = FlatStyle.Flat;
            BringToFrontButton.BackColor = Colors.Curtain;
            BringToFrontButton.FlatAppearance.MouseOverBackColor = Colors.CurtainButtonMouseOver;
            BringToFrontButton.FlatAppearance.MouseDownBackColor = Colors.CurtainButtonMouseDown;
            BringToFrontButton.FlatAppearance.BorderSize = 0;
            BringToFrontButton.Image = new Bitmap(@"..\..\..\downloads\Icons\BringToFront.png");
            BringToFrontButton.ImageAlign = ContentAlignment.TopCenter;
            BringToFrontButton.Font = new Font("Calibri", 10);
            BringToFrontButton.ForeColor = Colors.Constant;
            BringToFrontButton.Text = "Front";
            BringToFrontButton.TextAlign = ContentAlignment.BottomCenter;
            BringToFrontButton.MouseClick += BringToFrontButton_MouseClick;

            SendToBackButton = new Button();
            SendToBackButton.TabIndex = 0;
            SendToBackButton.TabStop = false;
            SendToBackButton.Location = new Point(60, ZindexPropertiesLabel.Bottom);
            SendToBackButton.Size = new Size(60, 60);
            SendToBackButton.FlatStyle = FlatStyle.Flat;
            SendToBackButton.BackColor = Colors.Curtain;
            SendToBackButton.FlatAppearance.MouseOverBackColor = Colors.CurtainButtonMouseOver;
            SendToBackButton.FlatAppearance.MouseDownBackColor = Colors.CurtainButtonMouseDown;
            SendToBackButton.FlatAppearance.BorderSize = 0;
            SendToBackButton.Image = new Bitmap(@"..\..\..\downloads\Icons\SendToBack.png");
            SendToBackButton.ImageAlign = ContentAlignment.TopCenter;
            SendToBackButton.Font = new Font("Calibri", 10);
            SendToBackButton.ForeColor = Colors.Constant;
            SendToBackButton.Text = "Back";
            SendToBackButton.TextAlign = ContentAlignment.BottomCenter;
            SendToBackButton.MouseClick += SendToBackButton_MouseClick;

            ZindexPropertiesPanel.Controls.Add(ZindexPropertiesLabel);
            ZindexPropertiesPanel.Controls.Add(BringToFrontButton);
            ZindexPropertiesPanel.Controls.Add(SendToBackButton);
            #endregion

            #region Interactive properties
            InteractivePropertiesPanel = new Panel();
            InteractivePropertiesPanel.TabIndex = 0;
            InteractivePropertiesPanel.TabStop = false;
            InteractivePropertiesPanel.Location = new Point(0, 0);
            InteractivePropertiesPanel.Size = new Size(72, 90);
            InteractivePropertiesPanel.BorderStyle = BorderStyle.None;
            InteractivePropertiesPanel.BackColor = Colors.Curtain;
            InteractivePropertiesPanel.Visible = false;

            InteractivePropertiesLabel = new Label();
            InteractivePropertiesLabel.TabIndex = 0;
            InteractivePropertiesLabel.Location = new Point(0, 0);
            InteractivePropertiesLabel.Size = new Size(InteractivePropertiesPanel.Width, 30);
            InteractivePropertiesLabel.FlatStyle = FlatStyle.Flat;
            InteractivePropertiesLabel.BorderStyle = BorderStyle.None;
            InteractivePropertiesLabel.BackColor = Colors.Curtain;
            InteractivePropertiesLabel.Font = new Font("Calibri", 14);
            InteractivePropertiesLabel.ForeColor = Colors.Constant;
            InteractivePropertiesLabel.Text = "Tasks";
            InteractivePropertiesLabel.TextAlign = ContentAlignment.MiddleCenter;

            ReadCheckBox = new CheckBox();
            ReadCheckBox.TabIndex = 0;
            ReadCheckBox.TabStop = false;
            ReadCheckBox.Location = new Point(0, InteractivePropertiesLabel.Bottom);
            ReadCheckBox.Size = new Size(InteractivePropertiesPanel.Width, 30);
            ReadCheckBox.FlatStyle = FlatStyle.Flat;
            ReadCheckBox.BackColor = Colors.Curtain;
            ReadCheckBox.FlatAppearance.MouseOverBackColor = Colors.CurtainButtonMouseOver;
            ReadCheckBox.FlatAppearance.MouseDownBackColor = Colors.CurtainButtonMouseDown;
            ReadCheckBox.FlatAppearance.BorderSize = 0;
            ReadCheckBox.Font = new Font("Calibri", 13);
            ReadCheckBox.ForeColor = Colors.Constant;
            ReadCheckBox.Text = "Read";
            ReadCheckBox.TextAlign = ContentAlignment.MiddleLeft;
            ReadCheckBox.CheckedChanged += ReadCheckBox_CheckedChanged;

            ListenCheckBox = new CheckBox();
            ListenCheckBox.TabIndex = 0;
            ListenCheckBox.TabStop = false;
            ListenCheckBox.Location = new Point(0, ReadCheckBox.Bottom);
            ListenCheckBox.Size = new Size(InteractivePropertiesPanel.Width, 30);
            ListenCheckBox.FlatStyle = FlatStyle.Flat;
            ListenCheckBox.BackColor = Colors.Curtain;
            ListenCheckBox.FlatAppearance.MouseOverBackColor = Colors.CurtainButtonMouseOver;
            ListenCheckBox.FlatAppearance.MouseDownBackColor = Colors.CurtainButtonMouseDown;
            ListenCheckBox.FlatAppearance.BorderSize = 0;
            ListenCheckBox.Font = new Font("Calibri", 13);
            ListenCheckBox.ForeColor = Colors.Constant;
            ListenCheckBox.Text = "Listen";
            ListenCheckBox.TextAlign = ContentAlignment.MiddleLeft;
            ListenCheckBox.CheckedChanged += ListenCheckBox_CheckedChanged;

            InteractivePropertiesPanel.Controls.Add(InteractivePropertiesLabel);
            InteractivePropertiesPanel.Controls.Add(ReadCheckBox);
            InteractivePropertiesPanel.Controls.Add(ListenCheckBox);
            #endregion

            #region Mouse properties
            MousePropertiesPanel = new Panel();
            MousePropertiesPanel.TabIndex = 0;
            MousePropertiesPanel.TabStop = false;
            MousePropertiesPanel.Location = new Point(0, 0);
            MousePropertiesPanel.Size = new Size(120, 90);
            MousePropertiesPanel.BorderStyle = BorderStyle.None;
            MousePropertiesPanel.BackColor = Colors.Curtain;
            MousePropertiesPanel.Visible = false;

            MousePropertiesLabel = new Label();
            MousePropertiesLabel.TabIndex = 0;
            MousePropertiesLabel.Location = new Point(0, 0);
            MousePropertiesLabel.Size = new Size(MousePropertiesPanel.Width, 30);
            MousePropertiesLabel.FlatStyle = FlatStyle.Flat;
            MousePropertiesLabel.BorderStyle = BorderStyle.None;
            MousePropertiesLabel.BackColor = Colors.Curtain;
            MousePropertiesLabel.Font = new Font("Calibri", 14);
            MousePropertiesLabel.ForeColor = Colors.Constant;
            MousePropertiesLabel.Text = "Mouse";
            MousePropertiesLabel.TextAlign = ContentAlignment.MiddleCenter;

            MouseOverBackColorButton = new Button();
            MouseOverBackColorButton.TabIndex = 0;
            MouseOverBackColorButton.TabStop = false;
            MouseOverBackColorButton.Location = new Point(0, MousePropertiesLabel.Bottom);
            MouseOverBackColorButton.Size = new Size(60, 60);
            MouseOverBackColorButton.FlatStyle = FlatStyle.Flat;
            MouseOverBackColorButton.BackColor = Colors.Curtain;
            MouseOverBackColorButton.FlatAppearance.MouseOverBackColor = Colors.CurtainButtonMouseOver;
            MouseOverBackColorButton.FlatAppearance.MouseDownBackColor = Colors.CurtainButtonMouseDown;
            MouseOverBackColorButton.FlatAppearance.BorderSize = 0;
            MouseOverBackColorButton.Image = new Bitmap(@"..\..\..\downloads\Icons\ButtonMouseOver.png");
            MouseOverBackColorButton.ImageAlign = ContentAlignment.TopCenter;
            MouseOverBackColorButton.Font = new Font("Calibri", 10);
            MouseOverBackColorButton.ForeColor = Colors.Constant;
            MouseOverBackColorButton.Text = "Hover";
            MouseOverBackColorButton.TextAlign = ContentAlignment.BottomCenter;
            MouseOverBackColorButton.MouseClick += MouseOverBackColorButton_MouseClick;

            MouseDownBackColorButton = new Button();
            MouseDownBackColorButton.TabIndex = 0;
            MouseDownBackColorButton.TabStop = false;
            MouseDownBackColorButton.Location = new Point(60, MousePropertiesLabel.Bottom);
            MouseDownBackColorButton.Size = new Size(60, 60);
            MouseDownBackColorButton.FlatStyle = FlatStyle.Flat;
            MouseDownBackColorButton.BackColor = Colors.Curtain;
            MouseDownBackColorButton.FlatAppearance.MouseOverBackColor = Colors.CurtainButtonMouseOver;
            MouseDownBackColorButton.FlatAppearance.MouseDownBackColor = Colors.CurtainButtonMouseDown;
            MouseDownBackColorButton.FlatAppearance.BorderSize = 0;
            MouseDownBackColorButton.Image = new Bitmap(@"..\..\..\downloads\Icons\ButtonMouseDown.png");
            MouseDownBackColorButton.ImageAlign = ContentAlignment.TopCenter;
            MouseDownBackColorButton.Font = new Font("Calibri", 10);
            MouseDownBackColorButton.ForeColor = Colors.Constant;
            MouseDownBackColorButton.Text = "Click";
            MouseDownBackColorButton.TextAlign = ContentAlignment.BottomCenter;
            MouseDownBackColorButton.MouseClick += MouseDownBackColorButton_MouseClick;

            MousePropertiesPanel.Controls.Add(MousePropertiesLabel);
            MousePropertiesPanel.Controls.Add(MouseOverBackColorButton);
            MousePropertiesPanel.Controls.Add(MouseDownBackColorButton);
            #endregion

            #region Actions
            ActionsPanel = new Panel();
            ActionsPanel.TabIndex = 0;
            ActionsPanel.TabStop = false;
            ActionsPanel.Location = new Point(0, 0);
            ActionsPanel.Size = new Size(80, 90);
            ActionsPanel.BorderStyle = BorderStyle.None;
            ActionsPanel.BackColor = Colors.Curtain;
            ActionsPanel.Visible = false;

            ActionsLabel = new Label();
            ActionsLabel.TabIndex = 0;
            ActionsLabel.Location = new Point(0, 0);
            ActionsLabel.Size = new Size(ActionsPanel.Width, 30);
            ActionsLabel.FlatStyle = FlatStyle.Flat;
            ActionsLabel.BorderStyle = BorderStyle.None;
            ActionsLabel.BackColor = Colors.Curtain;
            ActionsLabel.Font = new Font("Calibri", 14);
            ActionsLabel.ForeColor = Colors.Constant;
            ActionsLabel.Text = "Actions";
            ActionsLabel.TextAlign = ContentAlignment.MiddleCenter;

            DeleteButton = new Button();
            DeleteButton.TabIndex = 0;
            DeleteButton.TabStop = false;
            DeleteButton.Location = new Point(0, ActionsLabel.Bottom);
            DeleteButton.Size = new Size(80, 30);
            DeleteButton.FlatStyle = FlatStyle.Flat;
            DeleteButton.BackColor = Colors.Curtain;
            DeleteButton.FlatAppearance.MouseOverBackColor = Colors.CurtainButtonMouseOver;
            DeleteButton.FlatAppearance.MouseDownBackColor = Colors.CurtainButtonMouseDown;
            DeleteButton.FlatAppearance.BorderSize = 0;
            DeleteButton.Font = new Font("Calibri", 13);
            DeleteButton.ForeColor = Colors.Constant;
            DeleteButton.Text = "Delete";
            DeleteButton.TextAlign = ContentAlignment.MiddleCenter;
            DeleteButton.MouseClick += DeleteButton_MouseClick;

            CloneButton = new Button();
            CloneButton.TabIndex = 0;
            CloneButton.TabStop = false;
            CloneButton.Location = new Point(0, DeleteButton.Bottom);
            CloneButton.Size = new Size(80, 30);
            CloneButton.FlatStyle = FlatStyle.Flat;
            CloneButton.BackColor = Colors.Curtain;
            CloneButton.FlatAppearance.MouseOverBackColor = Colors.CurtainButtonMouseOver;
            CloneButton.FlatAppearance.MouseDownBackColor = Colors.CurtainButtonMouseDown;
            CloneButton.FlatAppearance.BorderSize = 0;
            CloneButton.Font = new Font("Calibri", 13);
            CloneButton.ForeColor = Colors.Constant;
            CloneButton.Text = "Clone";
            CloneButton.TextAlign = ContentAlignment.MiddleCenter;
            CloneButton.MouseClick += CloneButton_MouseClick;

            ActionsPanel.Controls.Add(ActionsLabel);
            ActionsPanel.Controls.Add(DeleteButton);
            ActionsPanel.Controls.Add(CloneButton);
            #endregion

            #region Text content
            TextContentPanel = new Panel();
            TextContentPanel.TabIndex = 0;
            TextContentPanel.TabStop = false;
            TextContentPanel.Location = new Point(0, 0);
            TextContentPanel.Size = new Size(MenuPanel.Width, 90);
            TextContentPanel.BorderStyle = BorderStyle.None;
            TextContentPanel.BackColor = Colors.Curtain;
            TextContentPanel.Visible = false;

            TextContentLabel = new Label();
            TextContentLabel.TabIndex = 0;
            TextContentLabel.Location = new Point(0, 12);
            TextContentLabel.Size = new Size(TextContentPanel.Width, 30);
            TextContentLabel.FlatStyle = FlatStyle.Flat;
            TextContentLabel.BorderStyle = BorderStyle.None;
            TextContentLabel.BackColor = Colors.Curtain;
            TextContentLabel.Font = new Font("Calibri", 14);
            TextContentLabel.ForeColor = Colors.Constant;
            TextContentLabel.Text = "Text";
            TextContentLabel.TextAlign = ContentAlignment.MiddleCenter;

            TextContentTextBox = new TextBox();
            TextContentTextBox.TabIndex = 0;
            TextContentTextBox.TabStop = false;
            TextContentTextBox.Location = new Point(0, TextContentLabel.Bottom);
            TextContentTextBox.Width = TextContentPanel.Width;
            TextContentTextBox.BorderStyle = BorderStyle.None;
            TextContentTextBox.BackColor = Colors.Curtain;
            TextContentTextBox.Font = new Font("Calibri", 24);
            TextContentTextBox.ForeColor = Colors.Constant;
            TextContentTextBox.Text = "Text";
            TextContentTextBox.TextAlign = HorizontalAlignment.Center;
            TextContentTextBox.TextChanged += TextContentTextBox_TextChanged;

            TextContentPanel.Controls.Add(TextContentLabel);
            TextContentPanel.Controls.Add(TextContentTextBox);
            #endregion

            #region Input content
            InputContentPanel = new Panel();
            InputContentPanel.TabIndex = 0;
            InputContentPanel.TabStop = false;
            InputContentPanel.Location = new Point(0, 0);
            InputContentPanel.Size = new Size(MenuPanel.Width, 90);
            InputContentPanel.BorderStyle = BorderStyle.None;
            InputContentPanel.BackColor = Colors.Curtain;
            InputContentPanel.Visible = false;

            InputContentLabel = new Label();
            InputContentLabel.TabIndex = 0;
            InputContentLabel.Location = new Point(0, 12);
            InputContentLabel.Size = new Size(InputContentPanel.Width, 30);
            InputContentLabel.FlatStyle = FlatStyle.Flat;
            InputContentLabel.BorderStyle = BorderStyle.None;
            InputContentLabel.BackColor = Colors.Curtain;
            InputContentLabel.Font = new Font("Calibri", 14);
            InputContentLabel.ForeColor = Colors.Constant;
            InputContentLabel.Text = "Input";
            InputContentLabel.TextAlign = ContentAlignment.MiddleCenter;

            NoInputContentLabel = new Label();
            NoInputContentLabel.TabIndex = 0;
            NoInputContentLabel.Location = new Point(0, InputContentLabel.Bottom);
            NoInputContentLabel.Size = new Size(InputContentPanel.Width, 45);
            NoInputContentLabel.FlatStyle = FlatStyle.Flat;
            NoInputContentLabel.BorderStyle = BorderStyle.None;
            NoInputContentLabel.BackColor = Colors.Curtain;
            NoInputContentLabel.Font = new Font("Calibri", 24);
            NoInputContentLabel.ForeColor = Colors.Constant;
            NoInputContentLabel.Text = "No content";
            NoInputContentLabel.TextAlign = ContentAlignment.TopCenter;

            InputContentPanel.Controls.Add(InputContentLabel);
            InputContentPanel.Controls.Add(NoInputContentLabel);
            #endregion

            #region Picture content
            PictureContentPanel = new Panel();
            PictureContentPanel.TabIndex = 0;
            PictureContentPanel.TabStop = false;
            PictureContentPanel.Location = new Point(0, 0);
            PictureContentPanel.Size = new Size(MenuPanel.Width, 90);
            PictureContentPanel.BorderStyle = BorderStyle.None;
            PictureContentPanel.BackColor = Colors.Curtain;
            PictureContentPanel.Visible = false;

            PictureContentLabel = new Label();
            PictureContentLabel.TabIndex = 0;
            PictureContentLabel.Location = new Point(0, 12);
            PictureContentLabel.Size = new Size(MenuPanel.Width, 30);
            PictureContentLabel.FlatStyle = FlatStyle.Flat;
            PictureContentLabel.BorderStyle = BorderStyle.None;
            PictureContentLabel.BackColor = Colors.Curtain;
            PictureContentLabel.Font = new Font("Calibri", 14);
            PictureContentLabel.ForeColor = Colors.Constant;
            PictureContentLabel.Text = "Picture";
            PictureContentLabel.TextAlign = ContentAlignment.MiddleCenter;

            SelectPictureButton = new Button();
            SelectPictureButton.TabIndex = 0;
            SelectPictureButton.TabStop = false;
            SelectPictureButton.Location = new Point(MenuPanel.Width / 2 - 150, PictureContentLabel.Bottom);
            SelectPictureButton.Size = new Size(300, 40);
            SelectPictureButton.FlatStyle = FlatStyle.Flat;
            SelectPictureButton.BackColor = Colors.Curtain;
            SelectPictureButton.FlatAppearance.MouseOverBackColor = Colors.CurtainButtonMouseOver;
            SelectPictureButton.FlatAppearance.MouseDownBackColor = Colors.CurtainButtonMouseDown;
            SelectPictureButton.FlatAppearance.BorderSize = 0;
            SelectPictureButton.Font = new Font("Calibri", 20);
            SelectPictureButton.ForeColor = Colors.Constant;
            SelectPictureButton.Text = "Select picture";
            SelectPictureButton.TextAlign = ContentAlignment.MiddleCenter;
            SelectPictureButton.MouseClick += SelectPictureButton_MouseClick;

            PictureContentPanel.Controls.Add(PictureContentLabel);
            PictureContentPanel.Controls.Add(SelectPictureButton);
            #endregion

            #region Panel content
            PanelContentPanel = new Panel();
            PanelContentPanel.TabIndex = 0;
            PanelContentPanel.TabStop = false;
            PanelContentPanel.Location = new Point(0, 0);
            PanelContentPanel.Size = new Size(MenuPanel.Width, 90);
            PanelContentPanel.BorderStyle = BorderStyle.None;
            PanelContentPanel.BackColor = Colors.Curtain;
            PanelContentPanel.Visible = false;

            PanelContentLabel = new Label();
            PanelContentLabel.TabIndex = 0;
            PanelContentLabel.Location = new Point(0, 12);
            PanelContentLabel.Size = new Size(PanelContentPanel.Width, 30);
            PanelContentLabel.FlatStyle = FlatStyle.Flat;
            PanelContentLabel.BorderStyle = BorderStyle.None;
            PanelContentLabel.BackColor = Colors.Curtain;
            PanelContentLabel.Font = new Font("Calibri", 14);
            PanelContentLabel.ForeColor = Colors.Constant;
            PanelContentLabel.Text = "Panel";
            PanelContentLabel.TextAlign = ContentAlignment.MiddleCenter;

            NoPanelContentLabel = new Label();
            NoPanelContentLabel.TabIndex = 0;
            NoPanelContentLabel.Location = new Point(0, InputContentLabel.Bottom);
            NoPanelContentLabel.Size = new Size(PanelContentPanel.Width, 45);
            NoPanelContentLabel.FlatStyle = FlatStyle.Flat;
            NoPanelContentLabel.BorderStyle = BorderStyle.None;
            NoPanelContentLabel.BackColor = Colors.Curtain;
            NoPanelContentLabel.Font = new Font("Calibri", 24);
            NoPanelContentLabel.ForeColor = Colors.Constant;
            NoPanelContentLabel.Text = "No content";
            NoPanelContentLabel.TextAlign = ContentAlignment.TopCenter;

            PanelContentPanel.Controls.Add(PanelContentLabel);
            PanelContentPanel.Controls.Add(NoPanelContentLabel);
            #endregion

            #region Button content
            ButtonContentPanel = new Panel();
            ButtonContentPanel.TabIndex = 0;
            ButtonContentPanel.TabStop = false;
            ButtonContentPanel.Location = new Point(0, 0);
            ButtonContentPanel.Size = new Size(MenuPanel.Width, 90);
            ButtonContentPanel.BorderStyle = BorderStyle.None;
            ButtonContentPanel.BackColor = Colors.Curtain;
            ButtonContentPanel.Visible = false;

            ButtonContentLabel = new Label();
            ButtonContentLabel.TabIndex = 0;
            ButtonContentLabel.Location = new Point(0, 12);
            ButtonContentLabel.Size = new Size(ButtonContentPanel.Width, 30);
            ButtonContentLabel.FlatStyle = FlatStyle.Flat;
            ButtonContentLabel.BorderStyle = BorderStyle.None;
            ButtonContentLabel.BackColor = Colors.Curtain;
            ButtonContentLabel.Font = new Font("Calibri", 14);
            ButtonContentLabel.ForeColor = Colors.Constant;
            ButtonContentLabel.Text = "Target page Id";
            ButtonContentLabel.TextAlign = ContentAlignment.MiddleCenter;

            TargetPageIdTextBox = new TextBox();
            TargetPageIdTextBox.TabIndex = 0;
            TargetPageIdTextBox.TabStop = false;
            TargetPageIdTextBox.Location = new Point(MenuPanel.Width / 2 - 50, ButtonContentLabel.Bottom);
            TargetPageIdTextBox.Width = 100;
            TargetPageIdTextBox.BorderStyle = BorderStyle.None;
            TargetPageIdTextBox.BackColor = Colors.Curtain;
            TargetPageIdTextBox.Font = new Font("Calibri", 24);
            TargetPageIdTextBox.ForeColor = Colors.Constant;
            TargetPageIdTextBox.Text = "0";
            TargetPageIdTextBox.TextAlign = HorizontalAlignment.Center;
            TargetPageIdTextBox.TextChanged += TargetPageIdTextBox_TextChanged;

            ButtonContentPanel.Controls.Add(ButtonContentLabel);
            ButtonContentPanel.Controls.Add(TargetPageIdTextBox);
            #endregion

            MenuPanel.Controls.Add(BackgroundPropertiesPanel);
            MenuPanel.Controls.Add(SwitchesPropertiesPanel);
            MenuPanel.Controls.Add(ElementsPanel);
            MenuPanel.Controls.Add(CommonPropertiesPanel);
            MenuPanel.Controls.Add(TextPropertiesPanel);
            MenuPanel.Controls.Add(ImagePropertiesPanel);
            MenuPanel.Controls.Add(ZindexPropertiesPanel);
            MenuPanel.Controls.Add(InteractivePropertiesPanel);
            MenuPanel.Controls.Add(MousePropertiesPanel);
            MenuPanel.Controls.Add(ActionsPanel);
            MenuPanel.Controls.Add(TextContentPanel);
            MenuPanel.Controls.Add(InputContentPanel);
            MenuPanel.Controls.Add(PictureContentPanel);
            MenuPanel.Controls.Add(PanelContentPanel);
            MenuPanel.Controls.Add(ButtonContentPanel);

            MenuPanel.Visible = false;
            this.Controls.Add(MenuPanel);
        }
        
        #region Background properties
        private void BackgroundColorButton_MouseClick(object sender, MouseEventArgs e)
        {
            BackgroundColor = true;
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.Color = ContainerPanel.BackColor;
            if (colorDialog.ShowDialog() == DialogResult.OK)
                ContainerPanel.BackColor = colorDialog.Color;
        }

        private void BackgroundImageButton_MouseClick(object sender, MouseEventArgs e)
        {
            BackgroundColor = false;
            ContainerPanel.BackColor = Color.Black;
        }
        #endregion

        #region Switches properties
        private void TopValueTextBox_TextChanged(object sender, EventArgs e)
        {
            if (TopValueTextBox.Text == "")
            {
                NextPageButton.Top = 0;
                PreviusPageButton.Top = 0;
            }
            try
            {
                NextPageButton.Top = Convert.ToInt32(TopValueTextBox.Text);
                PreviusPageButton.Top = Convert.ToInt32(TopValueTextBox.Text);
            }
            catch { }
        }

        private void SwitchesBackColorButton_MouseClick(object sender, MouseEventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.Color = NextPageButton.BackColor;
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                NextPageButton.BackColor = colorDialog.Color;
                NextPageButton.FlatAppearance.MouseOverBackColor = colorDialog.Color;
                NextPageButton.FlatAppearance.MouseDownBackColor = colorDialog.Color;

                PreviusPageButton.BackColor = colorDialog.Color;
                PreviusPageButton.FlatAppearance.MouseOverBackColor = colorDialog.Color;
                PreviusPageButton.FlatAppearance.MouseDownBackColor = colorDialog.Color;
            }
        }

        private void SwitchesMouseOverBackColorButton_MouseClick(object sender, MouseEventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.Color = NextPageButton.FlatAppearance.MouseOverBackColor;
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                NextPageButton.FlatAppearance.MouseOverBackColor = colorDialog.Color;
                PreviusPageButton.FlatAppearance.MouseOverBackColor = colorDialog.Color;
            }
        }

        private void SwitchesMouseDownBackColorButton_MouseClick(object sender, MouseEventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.Color = NextPageButton.FlatAppearance.MouseDownBackColor;
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                NextPageButton.FlatAppearance.MouseDownBackColor = colorDialog.Color;
                PreviusPageButton.FlatAppearance.MouseDownBackColor = colorDialog.Color;
            }
        }

        private void SwitchesTransparentColorButton_MouseClick(object sender, MouseEventArgs e)
        {
            NextPageButton.BackColor = Color.Transparent;
            NextPageButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
            NextPageButton.FlatAppearance.MouseDownBackColor = Color.Transparent;

            PreviusPageButton.BackColor = Color.Transparent;
            PreviusPageButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
            PreviusPageButton.FlatAppearance.MouseDownBackColor = Color.Transparent;
        }
        #endregion

        #region Elements
        private void TextButton_MouseClick(object sender, MouseEventArgs e)
        {
            AddCustomLabel();
        }

        private void InputButton_MouseClick(object sender, MouseEventArgs e)
        {
            AddCustomTextBox();
        }

        private void PictureButton_MouseClick(object sender, MouseEventArgs e)
        {
            AddCustomPictureBox();
        }

        private void PanelButton_MouseClick(object sender, MouseEventArgs e)
        {
            AddCustomPanel();
        }

        private void ButtonButton_MouseClick(object sender, MouseEventArgs e)
        {
            AddCustomButton();
        }
        #endregion

        #region Common properties
        private void PointXTextBox_TextChanged(object sender, EventArgs e)
        {
            if (PointXTextBox.Text == "")
                FocusedCustomElement.Control.Left = 0;
            try { FocusedCustomElement.Control.Left = Convert.ToInt32(PointXTextBox.Text); }
            catch { }
        }

        private void PointYTextBox_TextChanged(object sender, EventArgs e)
        {
            if (PointYTextBox.Text == "")
                FocusedCustomElement.Control.Top = 0;
            try { FocusedCustomElement.Control.Top = Convert.ToInt32(PointYTextBox.Text); }
            catch { }
        }

        private void WidthTextBox_TextChanged(object sender, EventArgs e)
        {
            if (WidthTextBox.Text == "")
                FocusedCustomElement.Control.Width = 0;
            try { FocusedCustomElement.Control.Width = Convert.ToInt32(WidthTextBox.Text); }
            catch { }
        }

        private void HeightTextBox_TextChanged(object sender, EventArgs e)
        {
            if (HeightTextBox.Text == "")
                FocusedCustomElement.Control.Height = 0;
            try { FocusedCustomElement.Control.Height = Convert.ToInt32(HeightTextBox.Text); }
            catch { }
        }

        private void ColorButton_MouseClick(object sender, MouseEventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.Color = FocusedCustomElement.Control.BackColor;
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                FocusedCustomElement.Control.BackColor = colorDialog.Color;
                if (FocusedCustomElement.ControlType == ControlType.Button)
                {
                    Button CustomButton = FocusedCustomElement.Control as Button;
                    CustomButton.FlatAppearance.MouseOverBackColor = colorDialog.Color;
                    CustomButton.FlatAppearance.MouseDownBackColor = colorDialog.Color;
                }
            }
        }

        private void TransparentBackColorButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (FocusedCustomElement.ControlType == ControlType.Label || FocusedCustomElement.ControlType == ControlType.PictureBox || FocusedCustomElement.ControlType == ControlType.Panel || FocusedCustomElement.ControlType == ControlType.Button)
            {
                FocusedCustomElement.Control.BackColor = Color.Transparent;
                if (FocusedCustomElement.ControlType == ControlType.Button)
                {
                    Button CustomButton = FocusedCustomElement.Control as Button;
                    CustomButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
                    CustomButton.FlatAppearance.MouseDownBackColor = Color.Transparent;
                }
            }
        }
        #endregion

        #region Text properties
        private void FontSizeTextBox_TextChanged(object sender, EventArgs e)
        {
            if (FocusedCustomElement.ControlType == ControlType.Label || FocusedCustomElement.ControlType == ControlType.TextBox || FocusedCustomElement.ControlType == ControlType.Button)
            {
                if (FontSizeTextBox.Text == "")
                    FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily.Name, 1, FocusedCustomElement.Control.Font.Style);
                try { FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily.Name, Convert.ToInt32(FontSizeTextBox.Text), FocusedCustomElement.Control.Font.Style); }
                catch { }
            }
        }

        private void FontSizePlusButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (FocusedCustomElement.ControlType == ControlType.Label || FocusedCustomElement.ControlType == ControlType.TextBox || FocusedCustomElement.ControlType == ControlType.Button)
            {
                try
                {
                    FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily.Name, Convert.ToInt32(FontSizeTextBox.Text) + 1, FocusedCustomElement.Control.Font.Style);
                    FontSizeTextBox.Text = Convert.ToString(Convert.ToInt32(FontSizeTextBox.Text) + 1);
                }
                catch { }
            }
        }

        private void FontSizeMinusButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (FocusedCustomElement.ControlType == ControlType.Label || FocusedCustomElement.ControlType == ControlType.TextBox || FocusedCustomElement.ControlType == ControlType.Button)
            {
                try
                {
                    FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily.Name, Convert.ToInt32(FontSizeTextBox.Text) - 1, FocusedCustomElement.Control.Font.Style);
                    FontSizeTextBox.Text = Convert.ToString(Convert.ToInt32(FontSizeTextBox.Text) - 1);
                }
                catch { }
            }
        }

        private void FontButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (FocusedCustomElement.ControlType == ControlType.Label || FocusedCustomElement.ControlType == ControlType.TextBox || FocusedCustomElement.ControlType == ControlType.Button)
            {
                FontDialog fontDialog = new FontDialog();
                fontDialog.Font = FocusedCustomElement.Control.Font;
                if (fontDialog.ShowDialog() == DialogResult.OK)
                {
                    FocusedCustomElement.Control.Font = fontDialog.Font;
                    FontSizeTextBox.Text = Convert.ToString(Convert.ToInt32(FocusedCustomElement.Control.Font.Size));

                    if (FocusedCustomElement.Control.Font.Bold == true)
                        IsBold = true;
                    else
                        IsBold = false;

                    if (FocusedCustomElement.Control.Font.Italic == true)
                        IsItalic = true;
                    else
                        IsItalic = false;

                    if (FocusedCustomElement.Control.Font.Underline == true)
                        IsUnderline = true;
                    else
                        IsUnderline = false;

                    if (FocusedCustomElement.Control.Font.Strikeout == true)
                        IsStrikeout = true;
                    else
                        IsStrikeout = false;
                }
            }
        }

        private void ForeColorButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (FocusedCustomElement.ControlType == ControlType.Label || FocusedCustomElement.ControlType == ControlType.TextBox || FocusedCustomElement.ControlType == ControlType.Button)
            {
                ColorDialog colorDialog = new ColorDialog();
                colorDialog.Color = FocusedCustomElement.Control.ForeColor;
                if (colorDialog.ShowDialog() == DialogResult.OK)
                    FocusedCustomElement.Control.ForeColor = colorDialog.Color;
            }
        }

        private void BoldTextButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (FocusedCustomElement.ControlType == ControlType.Label || FocusedCustomElement.ControlType == ControlType.TextBox || FocusedCustomElement.ControlType == ControlType.Button)
            {
                IsBold = !IsBold;
                if (IsBold == true)
                {
                    if (IsItalic == false && IsUnderline == false && IsStrikeout == false)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Bold);

                    else if (IsItalic == true && IsUnderline == false && IsStrikeout == false)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Bold | FontStyle.Italic);
                    else if (IsItalic == true && IsUnderline == true && IsStrikeout == false)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Bold | FontStyle.Italic | FontStyle.Underline);
                    else if (IsItalic == true && IsUnderline == false && IsStrikeout == true)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Bold | FontStyle.Italic | FontStyle.Strikeout);

                    else if (IsItalic == false && IsUnderline == true && IsStrikeout == false)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Bold | FontStyle.Underline);
                    else if (IsItalic == true && IsUnderline == true && IsStrikeout == false)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Bold | FontStyle.Italic | FontStyle.Underline);
                    else if (IsItalic == false && IsUnderline == true && IsStrikeout == true)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Bold | FontStyle.Underline | FontStyle.Strikeout);

                    else if (IsItalic == false && IsUnderline == false && IsStrikeout == true)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Bold | FontStyle.Strikeout);
                    else if (IsItalic == true && IsUnderline == false && IsStrikeout == true)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Bold | FontStyle.Italic | FontStyle.Strikeout);
                    else if (IsItalic == false && IsUnderline == true && IsStrikeout == true)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Bold | FontStyle.Underline | FontStyle.Strikeout);

                    else if (IsItalic == true && IsUnderline == true && IsStrikeout == true)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Bold | FontStyle.Italic | FontStyle.Underline | FontStyle.Strikeout);
                }
                else
                {
                    if (IsItalic == false && IsUnderline == false && IsStrikeout == false)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Regular);

                    else if (IsItalic == true && IsUnderline == false && IsStrikeout == false)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Italic);
                    else if (IsItalic == true && IsUnderline == true && IsStrikeout == false)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Italic | FontStyle.Underline);
                    else if (IsItalic == true && IsUnderline == false && IsStrikeout == true)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Italic | FontStyle.Strikeout);

                    else if (IsItalic == false && IsUnderline == true && IsStrikeout == false)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Underline);
                    else if (IsItalic == true && IsUnderline == true && IsStrikeout == false)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Italic | FontStyle.Underline);
                    else if (IsItalic == false && IsUnderline == true && IsStrikeout == true)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Underline | FontStyle.Strikeout);

                    else if (IsItalic == false && IsUnderline == false && IsStrikeout == true)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Strikeout);
                    else if (IsItalic == true && IsUnderline == false && IsStrikeout == true)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Italic | FontStyle.Strikeout);
                    else if (IsItalic == false && IsUnderline == true && IsStrikeout == true)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Underline | FontStyle.Strikeout);

                    else if (IsItalic == true && IsUnderline == true && IsStrikeout == true)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Italic | FontStyle.Underline | FontStyle.Strikeout);
                }
            }
        }

        private void ItalicTextButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (FocusedCustomElement.ControlType == ControlType.Label || FocusedCustomElement.ControlType == ControlType.TextBox || FocusedCustomElement.ControlType == ControlType.Button)
            {
                IsItalic = !IsItalic;
                if (IsItalic == true)
                {
                    if (IsBold == false && IsUnderline == false && IsStrikeout == false)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Italic);

                    else if (IsBold == true && IsUnderline == false && IsStrikeout == false)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Italic | FontStyle.Bold);
                    else if (IsBold == true && IsUnderline == true && IsStrikeout == false)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Italic | FontStyle.Bold | FontStyle.Underline);
                    else if (IsBold == true && IsUnderline == false && IsStrikeout == true)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Italic | FontStyle.Bold | FontStyle.Strikeout);

                    else if (IsBold == false && IsUnderline == true && IsStrikeout == false)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Italic | FontStyle.Underline);
                    else if (IsBold == true && IsUnderline == true && IsStrikeout == false)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Italic | FontStyle.Bold | FontStyle.Underline);
                    else if (IsBold == false && IsUnderline == true && IsStrikeout == true)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Italic | FontStyle.Underline | FontStyle.Strikeout);

                    else if (IsBold == false && IsUnderline == false && IsStrikeout == true)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Italic | FontStyle.Strikeout);
                    else if (IsBold == true && IsUnderline == false && IsStrikeout == true)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Italic | FontStyle.Bold | FontStyle.Strikeout);
                    else if (IsBold == false && IsUnderline == true && IsStrikeout == true)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Italic | FontStyle.Underline | FontStyle.Strikeout);

                    else if (IsBold == true && IsUnderline == true && IsStrikeout == true)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Italic | FontStyle.Bold | FontStyle.Underline | FontStyle.Strikeout);
                }
                else
                {
                    if (IsBold == false && IsUnderline == false && IsStrikeout == false)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Regular);

                    else if (IsBold == true && IsUnderline == false && IsStrikeout == false)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Bold);
                    else if (IsBold == true && IsUnderline == true && IsStrikeout == false)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Bold | FontStyle.Underline);
                    else if (IsBold == true && IsUnderline == false && IsStrikeout == true)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Bold | FontStyle.Strikeout);

                    else if (IsBold == false && IsUnderline == true && IsStrikeout == false)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Underline);
                    else if (IsBold == true && IsUnderline == true && IsStrikeout == false)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Bold | FontStyle.Underline);
                    else if (IsBold == false && IsUnderline == true && IsStrikeout == true)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Underline | FontStyle.Strikeout);

                    else if (IsBold == false && IsUnderline == false && IsStrikeout == true)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Strikeout);
                    else if (IsBold == true && IsUnderline == false && IsStrikeout == true)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Bold | FontStyle.Strikeout);
                    else if (IsBold == false && IsUnderline == true && IsStrikeout == true)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Underline | FontStyle.Strikeout);

                    else if (IsBold == true && IsUnderline == true && IsStrikeout == true)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Bold | FontStyle.Underline | FontStyle.Strikeout);
                }
            }
        }

        private void UnderlineTextButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (FocusedCustomElement.ControlType == ControlType.Label || FocusedCustomElement.ControlType == ControlType.TextBox || FocusedCustomElement.ControlType == ControlType.Button)
            {
                IsUnderline = !IsUnderline;
                if (IsUnderline == true)
                {
                    if (IsBold == false && IsItalic == false && IsStrikeout == false)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Underline);

                    else if (IsBold == true && IsItalic == false && IsStrikeout == false)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Underline | FontStyle.Bold);
                    else if (IsBold == true && IsItalic == true && IsStrikeout == false)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Underline | FontStyle.Bold | FontStyle.Italic);
                    else if (IsBold == true && IsItalic == false && IsStrikeout == true)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Underline | FontStyle.Bold | FontStyle.Strikeout);

                    else if (IsBold == false && IsItalic == true && IsStrikeout == false)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Underline | FontStyle.Italic);
                    else if (IsBold == true && IsItalic == true && IsStrikeout == false)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Underline | FontStyle.Bold | FontStyle.Italic);
                    else if (IsBold == false && IsItalic == true && IsStrikeout == true)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Underline | FontStyle.Italic | FontStyle.Strikeout);

                    else if (IsBold == false && IsItalic == false && IsStrikeout == true)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Underline | FontStyle.Strikeout);
                    else if (IsBold == true && IsItalic == false && IsStrikeout == true)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Underline | FontStyle.Bold | FontStyle.Strikeout);
                    else if (IsBold == false && IsItalic == true && IsStrikeout == true)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Underline | FontStyle.Italic | FontStyle.Strikeout);

                    else if (IsBold == true && IsItalic == true && IsStrikeout == true)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Underline | FontStyle.Bold | FontStyle.Italic | FontStyle.Strikeout);
                }
                else
                {
                    if (IsBold == false && IsItalic == false && IsStrikeout == false)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Regular);

                    else if (IsBold == true && IsItalic == false && IsStrikeout == false)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Bold);
                    else if (IsBold == true && IsItalic == true && IsStrikeout == false)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Bold | FontStyle.Italic);
                    else if (IsBold == true && IsItalic == false && IsStrikeout == true)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Bold | FontStyle.Strikeout);

                    else if (IsBold == false && IsItalic == true && IsStrikeout == false)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Italic);
                    else if (IsBold == true && IsItalic == true && IsStrikeout == false)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Bold | FontStyle.Italic);
                    else if (IsBold == false && IsItalic == true && IsStrikeout == true)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Italic | FontStyle.Strikeout);

                    else if (IsBold == false && IsItalic == false && IsStrikeout == true)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Strikeout);
                    else if (IsBold == true && IsItalic == false && IsStrikeout == true)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Bold | FontStyle.Strikeout);
                    else if (IsBold == false && IsItalic == true && IsStrikeout == true)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Italic | FontStyle.Strikeout);

                    else if (IsBold == true && IsItalic == true && IsStrikeout == true)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Bold | FontStyle.Italic | FontStyle.Strikeout);
                }
            }
        }

        private void StrikeoutTextButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (FocusedCustomElement.ControlType == ControlType.Label || FocusedCustomElement.ControlType == ControlType.TextBox || FocusedCustomElement.ControlType == ControlType.Button)
            {
                IsStrikeout = !IsStrikeout;
                if (IsStrikeout == true)
                {
                    if (IsBold == false && IsItalic == false && IsUnderline == false)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Strikeout);

                    else if (IsBold == true && IsItalic == false && IsUnderline == false)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Strikeout | FontStyle.Bold);
                    else if (IsBold == true && IsItalic == true && IsUnderline == false)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Strikeout | FontStyle.Bold | FontStyle.Italic);
                    else if (IsBold == true && IsItalic == false && IsUnderline == true)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Strikeout | FontStyle.Bold | FontStyle.Underline);

                    else if (IsBold == false && IsItalic == true && IsUnderline == false)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Strikeout | FontStyle.Italic);
                    else if (IsBold == true && IsItalic == true && IsUnderline == false)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Strikeout | FontStyle.Bold | FontStyle.Italic);
                    else if (IsBold == false && IsItalic == true && IsUnderline == true)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Strikeout | FontStyle.Italic | FontStyle.Underline);

                    else if (IsBold == false && IsItalic == false && IsUnderline == true)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Strikeout | FontStyle.Underline);
                    else if (IsBold == true && IsItalic == false && IsUnderline == true)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Strikeout | FontStyle.Bold | FontStyle.Underline);
                    else if (IsBold == false && IsItalic == true && IsUnderline == true)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Strikeout | FontStyle.Italic | FontStyle.Underline);

                    else if (IsBold == true && IsItalic == true && IsUnderline == true)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Strikeout | FontStyle.Bold | FontStyle.Italic | FontStyle.Underline);
                }
                else
                {
                    if (IsBold == false && IsItalic == false && IsUnderline == false)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Regular);

                    else if (IsBold == true && IsItalic == false && IsUnderline == false)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Bold);
                    else if (IsBold == true && IsItalic == true && IsUnderline == false)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Bold | FontStyle.Italic);
                    else if (IsBold == true && IsItalic == false && IsUnderline == true)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Bold | FontStyle.Underline);

                    else if (IsBold == false && IsItalic == true && IsUnderline == false)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Italic);
                    else if (IsBold == true && IsItalic == true && IsUnderline == false)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Bold | FontStyle.Italic);
                    else if (IsBold == false && IsItalic == true && IsUnderline == true)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Italic | FontStyle.Underline);

                    else if (IsBold == false && IsItalic == false && IsUnderline == true)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Underline);
                    else if (IsBold == true && IsItalic == false && IsUnderline == true)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Bold | FontStyle.Underline);
                    else if (IsBold == false && IsItalic == true && IsUnderline == true)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Italic | FontStyle.Underline);

                    else if (IsBold == true && IsItalic == true && IsUnderline == true)
                        FocusedCustomElement.Control.Font = new Font(FocusedCustomElement.Control.Font.FontFamily, FocusedCustomElement.Control.Font.Size, FontStyle.Bold | FontStyle.Italic | FontStyle.Underline);
                }
            }
        }

        private void TextAlignLeftButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (FocusedCustomElement.ControlType == ControlType.Label)
            {
                Label CustomLabel = FocusedCustomElement.Control as Label;
                CustomLabel.TextAlign = ContentAlignment.MiddleLeft;
            }
            else if (FocusedCustomElement.ControlType == ControlType.TextBox)
            {
                TextBox CustomTextBox = FocusedCustomElement.Control as TextBox;
                CustomTextBox.TextAlign = HorizontalAlignment.Left;
            }
            else if (FocusedCustomElement.ControlType == ControlType.Button)
            {
                Button CustomButton = FocusedCustomElement.Control as Button;
                CustomButton.TextAlign = ContentAlignment.MiddleLeft;
            }
        }

        private void TextAlignCenterButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (FocusedCustomElement.ControlType == ControlType.Label)
            {
                Label CustomLabel = FocusedCustomElement.Control as Label;
                CustomLabel.TextAlign = ContentAlignment.MiddleCenter;
            }
            else if (FocusedCustomElement.ControlType == ControlType.TextBox)
            {
                TextBox CustomTextBox = FocusedCustomElement.Control as TextBox;
                CustomTextBox.TextAlign = HorizontalAlignment.Center;
            }
            else if (FocusedCustomElement.ControlType == ControlType.Button)
            {
                Button CustomButton = FocusedCustomElement.Control as Button;
                CustomButton.TextAlign = ContentAlignment.MiddleCenter;
            }
        }

        private void TextAlignRightButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (FocusedCustomElement.ControlType == ControlType.Label)
            {
                Label CustomLabel = FocusedCustomElement.Control as Label;
                CustomLabel.TextAlign = ContentAlignment.MiddleRight;
            }
            else if (FocusedCustomElement.ControlType == ControlType.TextBox)
            {
                TextBox CustomTextBox = FocusedCustomElement.Control as TextBox;
                CustomTextBox.TextAlign = HorizontalAlignment.Right;
            }
            else if (FocusedCustomElement.ControlType == ControlType.Button)
            {
                Button CustomButton = FocusedCustomElement.Control as Button;
                CustomButton.TextAlign = ContentAlignment.MiddleRight;
            }
        }
        #endregion

        #region Image properties
        private void SizeMode1Button_MouseClick(object sender, MouseEventArgs e)
        {
            if (FocusedCustomElement.ControlType == ControlType.PictureBox)
            {
                PictureBox CustomPictureBox = FocusedCustomElement.Control as PictureBox;
                CustomPictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
            }
        }

        private void SizeMode2Button_MouseClick(object sender, MouseEventArgs e)
        {
            if (FocusedCustomElement.ControlType == ControlType.PictureBox)
            {
                PictureBox CustomPictureBox = FocusedCustomElement.Control as PictureBox;
                CustomPictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
            }
        }

        private void SizeMode3Button_MouseClick(object sender, MouseEventArgs e)
        {
            if (FocusedCustomElement.ControlType == ControlType.PictureBox)
            {
                PictureBox CustomPictureBox = FocusedCustomElement.Control as PictureBox;
                CustomPictureBox.SizeMode = PictureBoxSizeMode.Normal;
            }
        }

        private void SizeMode4Button_MouseClick(object sender, MouseEventArgs e)
        {
            if (FocusedCustomElement.ControlType == ControlType.PictureBox)
            {
                PictureBox CustomPictureBox = FocusedCustomElement.Control as PictureBox;
                CustomPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private void SizeMode5Button_MouseClick(object sender, MouseEventArgs e)
        {
            if (FocusedCustomElement.ControlType == ControlType.PictureBox)
            {
                PictureBox CustomPictureBox = FocusedCustomElement.Control as PictureBox;
                CustomPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }
        #endregion

        #region Z-index properties
        private void BringToFrontButton_MouseClick(object sender, MouseEventArgs e)
        {
            int MaxZindex = 0;
            for (int i = 0; i < CustomElements.Count; i++)
            {
                if (CustomElements[i].Zindex > MaxZindex)
                    MaxZindex = CustomElements[i].Zindex;
            }
            FocusedCustomElement.Zindex = MaxZindex + 1;
            FocusedCustomElement.Control.BringToFront();
        }

        private void SendToBackButton_MouseClick(object sender, MouseEventArgs e)
        {
            int MinZindex = 0;
            for (int i = 0; i < CustomElements.Count; i++)
            {
                if (CustomElements[i].Zindex < MinZindex)
                    MinZindex = CustomElements[i].Zindex;
            }
            FocusedCustomElement.Zindex = MinZindex - 1;
            FocusedCustomElement.Control.SendToBack();
        }
        #endregion

        #region Interactive properties
        private void ReadCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (FocusedCustomElement.ControlType == ControlType.Label)
                FocusedCustomElement.Read = ReadCheckBox.Checked;
        }

        private void ListenCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (FocusedCustomElement.ControlType == ControlType.Label)
                FocusedCustomElement.Listen = ListenCheckBox.Checked;
        }
        #endregion

        #region Mouse properties
        private void MouseOverBackColorButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (FocusedCustomElement.ControlType == ControlType.Button)
            {
                Button CustomButton = FocusedCustomElement.Control as Button;
                ColorDialog colorDialog = new ColorDialog();
                colorDialog.Color = CustomButton.FlatAppearance.MouseOverBackColor;
                if (colorDialog.ShowDialog() == DialogResult.OK)
                    CustomButton.FlatAppearance.MouseOverBackColor = colorDialog.Color;
            }
        }

        private void MouseDownBackColorButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (FocusedCustomElement.ControlType == ControlType.Button)
            {
                Button CustomButton = FocusedCustomElement.Control as Button;
                ColorDialog colorDialog = new ColorDialog();
                colorDialog.Color = CustomButton.FlatAppearance.MouseDownBackColor;
                if (colorDialog.ShowDialog() == DialogResult.OK)
                    CustomButton.FlatAppearance.MouseDownBackColor = colorDialog.Color;
            }
        }
        #endregion

        #region Actions
        private void DeleteButton_MouseClick(object sender, MouseEventArgs e)
        {
            CustomElements.Remove(FocusedCustomElement);
            UnfocusCustomElement();
            FocusedCustomElement.Control.Dispose();
        }

        private void CloneButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (FocusedCustomElement.ControlType == ControlType.Label)
            {
                CustomElement ParentCustomElement = FocusedCustomElement;
                AddCustomLabel();
                WidthTextBox.Text = Convert.ToString(ParentCustomElement.Control.Width);
                HeightTextBox.Text = Convert.ToString(ParentCustomElement.Control.Height);
                FocusedCustomElement.Control.BackColor = ParentCustomElement.Control.BackColor;
                FontSizeTextBox.Text = Convert.ToString(ParentCustomElement.Control.Font.Size);
                FocusedCustomElement.Control.Font = ParentCustomElement.Control.Font;
                FocusedCustomElement.Control.ForeColor = ParentCustomElement.Control.ForeColor;
                Label FocusedLabel = FocusedCustomElement.Control as Label;
                Label ParentLabel = ParentCustomElement.Control as Label;
                FocusedLabel.TextAlign = ParentLabel.TextAlign;
                ReadCheckBox.Checked = ParentCustomElement.Read;
                ListenCheckBox.Checked = ParentCustomElement.Listen;
            }
            else if (FocusedCustomElement.ControlType == ControlType.TextBox)
            {
                CustomElement ParentCustomElement = FocusedCustomElement;
                AddCustomTextBox();
                WidthTextBox.Text = Convert.ToString(ParentCustomElement.Control.Width);
                HeightTextBox.Text = Convert.ToString(ParentCustomElement.Control.Height);
                FocusedCustomElement.Control.BackColor = ParentCustomElement.Control.BackColor;
                FontSizeTextBox.Text = Convert.ToString(ParentCustomElement.Control.Font.Size);
                FocusedCustomElement.Control.Font = ParentCustomElement.Control.Font;
                FocusedCustomElement.Control.ForeColor = ParentCustomElement.Control.ForeColor;
                TextBox FocusedTextBox = FocusedCustomElement.Control as TextBox;
                TextBox ParentTextBox = ParentCustomElement.Control as TextBox;
                FocusedTextBox.TextAlign = ParentTextBox.TextAlign;
            }
            else if (FocusedCustomElement.ControlType == ControlType.PictureBox)
            {
                CustomElement ParentCustomElement = FocusedCustomElement;
                AddCustomPictureBox();
                WidthTextBox.Text = Convert.ToString(ParentCustomElement.Control.Width);
                HeightTextBox.Text = Convert.ToString(ParentCustomElement.Control.Height);
                FocusedCustomElement.Control.BackColor = ParentCustomElement.Control.BackColor;
                PictureBox FocusedPictureBox = FocusedCustomElement.Control as PictureBox;
                PictureBox ParentPictureBox = ParentCustomElement.Control as PictureBox;
                FocusedPictureBox.SizeMode = ParentPictureBox.SizeMode;
            }
            else if (FocusedCustomElement.ControlType == ControlType.Panel)
            {
                CustomElement ParentCustomElement = FocusedCustomElement;
                AddCustomPanel();
                WidthTextBox.Text = Convert.ToString(ParentCustomElement.Control.Width);
                HeightTextBox.Text = Convert.ToString(ParentCustomElement.Control.Height);
                FocusedCustomElement.Control.BackColor = ParentCustomElement.Control.BackColor;
            }
            else if (FocusedCustomElement.ControlType == ControlType.Button)
            {
                CustomElement ParentCustomElement = FocusedCustomElement;
                AddCustomButton();
                WidthTextBox.Text = Convert.ToString(ParentCustomElement.Control.Width);
                HeightTextBox.Text = Convert.ToString(ParentCustomElement.Control.Height);
                FocusedCustomElement.Control.BackColor = ParentCustomElement.Control.BackColor;
                FontSizeTextBox.Text = Convert.ToString(ParentCustomElement.Control.Font.Size);
                FocusedCustomElement.Control.Font = ParentCustomElement.Control.Font;
                FocusedCustomElement.Control.ForeColor = ParentCustomElement.Control.ForeColor;
                Button FocusedButton = FocusedCustomElement.Control as Button;
                Button ParentButton = ParentCustomElement.Control as Button;
                FocusedButton.TextAlign = ParentButton.TextAlign;
                FocusedButton.FlatAppearance.MouseOverBackColor = ParentButton.FlatAppearance.MouseOverBackColor;
                FocusedButton.FlatAppearance.MouseDownBackColor = ParentButton.FlatAppearance.MouseDownBackColor;
            }
        }
        #endregion

        #region Text content
        private void TextContentTextBox_TextChanged(object sender, EventArgs e)
        {
            FocusedCustomElement.Control.Text = TextContentTextBox.Text;
        }
        #endregion

        #region Input content
        #endregion

        #region Picture content
        private void SelectPictureButton_MouseClick(object sender, MouseEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Picture (*.jpg)|*.jpg|Picture (*.png)|*.png";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                PictureBox CustomPictureBox = FocusedCustomElement.Control as PictureBox;
                CustomPictureBox.Image = new Bitmap(openFileDialog.FileName);
            }
        }
        #endregion

        #region Panel content
        #endregion

        #region Button content
        private void TargetPageIdTextBox_TextChanged(object sender, EventArgs e)
        {
            FocusedCustomElement.Control.Text = TargetPageIdTextBox.Text;
        }
        #endregion

        private void InitializePages()
        {
            PagesPanel = new Panel();
            PagesPanel.TabIndex = 0;
            PagesPanel.TabStop = false;
            PagesPanel.Location = new Point(1, 126);
            PagesPanel.Size = new Size(100, this.Width - 127);
            PagesPanel.BorderStyle = BorderStyle.None;
            PagesPanel.BackColor = Colors.Curtain;
            PagesPanel.Visible = false;

            AddPageButton = new Button();
            AddPageButton.TabIndex = 0;
            AddPageButton.TabStop = false;
            AddPageButton.Location = new Point(5, 5);
            AddPageButton.Size = new Size(90, 50);
            AddPageButton.FlatStyle = FlatStyle.Flat;
            AddPageButton.BackColor = Colors.Curtain;
            AddPageButton.FlatAppearance.MouseOverBackColor = Colors.CurtainButtonMouseOver;
            AddPageButton.FlatAppearance.MouseDownBackColor = Colors.CurtainButtonMouseDown;
            AddPageButton.FlatAppearance.BorderSize = 0;
            AddPageButton.Font = new Font("Calibri", 16);
            AddPageButton.ForeColor = Colors.Constant;
            AddPageButton.Text = "Add";
            AddPageButton.TextAlign = ContentAlignment.MiddleCenter;
            AddPageButton.MouseClick += AddPageButton_MouseClick;

            PagesPanel.Controls.Add(AddPageButton);

            this.Controls.Add(PagesPanel);
        }

        private void AddPageButton_MouseClick(object sender, MouseEventArgs e)
        {
            AddPage();
        }

        private void AddPage()
        {
            Button PageButton = new Button();
            PageButton.TabIndex = 0;
            PageButton.TabStop = false;
            PageButton.Location = new Point(5,PageButtons.Count * 50);
            PageButton.Size = new Size(90, 50);
            PageButton.FlatStyle = FlatStyle.Flat;
            PageButton.BackColor = Colors.Curtain;
            PageButton.FlatAppearance.MouseOverBackColor = Colors.CurtainButtonMouseOver;
            PageButton.FlatAppearance.MouseDownBackColor = Colors.CurtainButtonMouseDown;
            PageButton.FlatAppearance.BorderSize = 0;
            PageButton.Font = new Font("Calibri", 16);
            PageButton.ForeColor = Colors.Constant;
            PageButton.Text = "Page " + Convert.ToString(PageButtons.Count + 1);
            PageButton.TextAlign = ContentAlignment.MiddleCenter;

            PageButtons.Add(PageButton);
            PagesPanel.Controls.Add(PageButton);

            Thread PageAnimation = new Thread(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    AddPageButton.Invoke((MethodInvoker)(delegate { AddPageButton.Top += 5; }));
                    Thread.Sleep(1);
                }
            });
            PageAnimation.IsBackground = true;
            PageAnimation.Start();

            PageIndex++;
            Pages.Add(new Page(PageIndex, DefaultCustomElements));

            RemoveCustomElements();
            SetPageCustomControls();
            UnfocusCustomElement();
        }

        private void RemoveCustomElements()
        {
            for (int i = 0; i < CustomElements.Count; i++)
                ContainerPanel.Controls.Remove(CustomElements[i].Control);
        }

        private void SetPageCustomControls()
        {
            CustomElements.Clear();
            for (int i = 0; i < Pages[PageIndex].CustomElements.Count; i++)
            {
                CustomElements.Add(Pages[PageIndex].CustomElements[i]);
                ContainerPanel.Controls.Add(Pages[PageIndex].CustomElements[i].Control);
            }
        }

        private void InitializeContainer()
        {
            CustomElements = new List<CustomElement>();

            ContainerPanel = new Panel();
            ContainerPanel.TabIndex = 0;
            ContainerPanel.TabStop = false;
            ContainerPanel.Location = new Point(75, 135);
            ContainerPanel.Size = new Size(706, 478);
            ContainerPanel.BorderStyle = BorderStyle.None;
            ContainerPanel.BackColor = Colors.Background;
            ContainerPanel.MouseDown += ContainerPanel_MouseDown;
            SetUncontrolableBorders(ContainerPanel);

            NextPageButton = new Button();
            NextPageButton.TabIndex = 0;
            NextPageButton.TabStop = false;
            NextPageButton.Location = new Point(ContainerPanel.Width - 73, ContainerPanel.Height / 2 - 50);
            NextPageButton.Size = new Size(70, 100);
            NextPageButton.FlatStyle = FlatStyle.Flat;
            NextPageButton.BackColor = Colors.Origin;
            NextPageButton.FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
            NextPageButton.FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
            NextPageButton.FlatAppearance.BorderSize = 0;
            NextPageButton.Image = new Bitmap(@"..\..\..\downloads\Icons\Next.png");
            NextPageButton.ImageAlign = ContentAlignment.MiddleCenter;
            NextPageButton.MouseDown += PageButton_MouseDown;
            NextPageButton.MouseMove += PageButton_MouseMove;
            NextPageButton.MouseUp += PageButton_MouseUp;

            PreviusPageButton = new Button();
            PreviusPageButton.TabIndex = 0;
            PreviusPageButton.TabStop = false;
            PreviusPageButton.Location = new Point(3, ContainerPanel.Height / 2 - 50);
            PreviusPageButton.Size = new Size(70, 100);
            PreviusPageButton.FlatStyle = FlatStyle.Flat;
            PreviusPageButton.BackColor = Colors.Origin;
            PreviusPageButton.FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
            PreviusPageButton.FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
            PreviusPageButton.FlatAppearance.BorderSize = 0;
            PreviusPageButton.Image = new Bitmap(@"..\..\..\downloads\Icons\Previus.png");
            PreviusPageButton.ImageAlign = ContentAlignment.MiddleCenter;
            PreviusPageButton.MouseDown += PageButton_MouseDown;
            PreviusPageButton.MouseMove += PageButton_MouseMove;
            PreviusPageButton.MouseUp += PageButton_MouseUp;

            ContainerPanel.Controls.Add(NextPageButton);
            ContainerPanel.Controls.Add(PreviusPageButton);

            ContainerPanel.Visible = false;
            this.Controls.Add(ContainerPanel);
        }

        private void ContainerPanel_MouseDown(object sender, MouseEventArgs e)
        {
            UnfocusCustomElement();
        }

        private void UnfocusCustomElement()
        {
            if (ProcessPosition == 1)
            {
                if (FocusedCustomElement != null)
                {
                    BackgroundPropertiesPanel.Visible = true;
                    SwitchesPropertiesPanel.Visible = true;
                    ElementsPanel.Visible = true;

                    CommonPropertiesPanel.Visible = false;
                    CommonPropertiesPanel.Location = new Point(0, 0);

                    TextPropertiesPanel.Visible = false;
                    TextPropertiesPanel.Location = new Point(0, 0);

                    ImagePropertiesPanel.Visible = false;
                    ImagePropertiesPanel.Location = new Point(0, 0);

                    ZindexPropertiesPanel.Visible = false;
                    ZindexPropertiesPanel.Location = new Point(0, 0);

                    InteractivePropertiesPanel.Visible = false;
                    InteractivePropertiesPanel.Location = new Point(0, 0);

                    MousePropertiesPanel.Visible = false;
                    MousePropertiesPanel.Location = new Point(0, 0);

                    ActionsPanel.Visible = false;
                    ActionsPanel.Location = new Point(0, 0);

                    for (int i = 0; i < FocusedCustomElement.Control.Controls.Count; i++)
                        FocusedCustomElement.Control.Controls[i].Visible = false;
                }
            }
            else if (ProcessPosition == 2)
            {
                if (FocusedCustomElement != null)
                {
                    TextContentPanel.Visible = false;
                    InputContentPanel.Visible = false;
                    PictureContentPanel.Visible = false;
                    PanelContentPanel.Visible = false;
                    ButtonContentPanel.Visible = false;

                    for (int i = 0; i < FocusedCustomElement.Control.Controls.Count; i++)
                        FocusedCustomElement.Control.Controls[i].Visible = false;
                }
            }
        }

        private void PageButton_MouseDown(object sender, MouseEventArgs e)
        {
            if (ProcessPosition == 1)
            {
                DragStarted = true;
                DragStartY = e.Y;
                NextPageButton.Cursor = Cursors.SizeAll;
                PreviusPageButton.Cursor = Cursors.SizeAll;
                UnfocusCustomElement();
            }
        }

        private void PageButton_MouseMove(object sender, MouseEventArgs e)
        {
            if (DragStarted == true && ProcessPosition == 1)
            {
                NextPageButton.Top = NextPageButton.Top - DragStartY + e.Y;
                PreviusPageButton.Top = PreviusPageButton.Top - DragStartY + e.Y;
                TopValueTextBox.Text = Convert.ToString(NextPageButton.Top);
            }
        }

        private void PageButton_MouseUp(object sender, MouseEventArgs e)
        {
            if (ProcessPosition == 1)
            {
                DragStarted = false;
                NextPageButton.Cursor = Cursors.Default;
                PreviusPageButton.Cursor = Cursors.Default;
            }
        }

        private void SetUncontrolableBorders(Control ClientControl)
        {
            BorderPanels = new Panel[4];

            BorderPanels[0] = new Panel();
            BorderPanels[0].TabIndex = 0;
            BorderPanels[0].TabStop = false;
            BorderPanels[0].Location = new Point(0, 0);
            BorderPanels[0].Size = new Size(3, ContainerPanel.Height);
            BorderPanels[0].BorderStyle = BorderStyle.None;
            BorderPanels[0].BackColor = Colors.Constant;

            BorderPanels[1] = new Panel();
            BorderPanels[1].TabIndex = 0;
            BorderPanels[1].TabStop = false;
            BorderPanels[1].Location = new Point(0, ContainerPanel.Height - 3);
            BorderPanels[1].Size = new Size(ContainerPanel.Width, 3);
            BorderPanels[1].BorderStyle = BorderStyle.None;
            BorderPanels[1].BackColor = Colors.Constant;

            BorderPanels[2] = new Panel();
            BorderPanels[2].TabIndex = 0;
            BorderPanels[2].TabStop = false;
            BorderPanels[2].Location = new Point(ContainerPanel.Width - 3, 0);
            BorderPanels[2].Size = new Size(3, ContainerPanel.Height);
            BorderPanels[2].BorderStyle = BorderStyle.None;
            BorderPanels[2].BackColor = Colors.Constant;

            BorderPanels[3] = new Panel();
            BorderPanels[3].TabIndex = 0;
            BorderPanels[3].TabStop = false;
            BorderPanels[3].Location = new Point(0, 0);
            BorderPanels[3].Size = new Size(ContainerPanel.Width, 3);
            BorderPanels[3].BorderStyle = BorderStyle.None;
            BorderPanels[3].BackColor = Colors.Constant;

            for (int i = 0; i < 4; i++)
                ClientControl.Controls.Add(BorderPanels[i]);
        }

        private void AddCustomLabel()
        {
            Label CustomLabel = new Label();
            CustomLabel.TabIndex = 0;
            CustomLabel.Location = new Point(0, 0);
            CustomLabel.Size = new Size(200, 100);
            CustomLabel.FlatStyle = FlatStyle.Flat;
            CustomLabel.BorderStyle = BorderStyle.None;
            CustomLabel.BackColor = Colors.Background;
            CustomLabel.Font = new Font("Calibri", 24);
            CustomLabel.ForeColor = Colors.Constant;
            CustomLabel.Text = "Text";
            CustomLabel.TextAlign = ContentAlignment.MiddleCenter;
            CustomLabel.MouseDown += CustomElement_MouseDown;
            CustomLabel.MouseMove += CustomElement_MouseMove;
            CustomLabel.MouseUp += CustomElement_MouseUp;
            CustomLabel.Resize += CustomElement_Resize;

            int MaxZindex = 0;
            for (int i = 0; i < CustomElements.Count; i++)
            {
                if (CustomElements[i].Zindex > MaxZindex)
                    MaxZindex = CustomElements[i].Zindex;
            }

            int Zindex = MaxZindex + 1;
            CustomElement NewCustomElement = new CustomElement(CustomLabel, ControlType.Label, Zindex);
            CustomElements.Add(NewCustomElement);

            SetControlableBorders(NewCustomElement);
            FocusedCustomElement = NewCustomElement;
            LeaveFocusOnCustomElement(NewCustomElement);

            ContainerPanel.Controls.Add(CustomLabel);
            CustomLabel.BringToFront();
            NextPageButton.BringToFront();
            PreviusPageButton.BringToFront();
            for (int i = 0; i < 4; i++)
                BorderPanels[i].BringToFront();
        }

        private void AddCustomTextBox()
        {
            TextBox CustomTextBox = new TextBox();
            CustomTextBox.TabIndex = 0;
            CustomTextBox.TabStop = false;
            CustomTextBox.Location = new Point(0, 0);
            CustomTextBox.Size = new Size(200, 40);
            CustomTextBox.BorderStyle = BorderStyle.None;
            CustomTextBox.BackColor = Colors.Origin;
            CustomTextBox.Font = new Font("Calibri", 24);
            CustomTextBox.ForeColor = Colors.Constant;
            CustomTextBox.TextAlign = HorizontalAlignment.Left;
            CustomTextBox.Multiline = true;
            CustomTextBox.MouseDown += CustomElement_MouseDown;
            CustomTextBox.MouseMove += CustomElement_MouseMove;
            CustomTextBox.MouseUp += CustomElement_MouseUp;
            CustomTextBox.Resize += CustomElement_Resize;

            int MaxZindex = 0;
            for (int i = 0; i < CustomElements.Count; i++)
            {
                if (CustomElements[i].Zindex > MaxZindex)
                    MaxZindex = CustomElements[i].Zindex;
            }
            int Zindex = MaxZindex + 1;
            CustomElement NewCustomElement = new CustomElement(CustomTextBox, ControlType.TextBox, Zindex);
            CustomElements.Add(NewCustomElement);

            SetControlableBorders(NewCustomElement);
            FocusedCustomElement = NewCustomElement;
            LeaveFocusOnCustomElement(NewCustomElement);

            ContainerPanel.Controls.Add(CustomTextBox);
            CustomTextBox.BringToFront();
            NextPageButton.BringToFront();
            PreviusPageButton.BringToFront();
            for (int i = 0; i < 4; i++)
                BorderPanels[i].BringToFront();
        }

        private void AddCustomPictureBox()
        {
            PictureBox CustomPictureBox = new PictureBox();
            CustomPictureBox.Location = new Point(0, 0);
            CustomPictureBox.Size = new Size(150, 150);
            CustomPictureBox.BorderStyle = BorderStyle.None;
            CustomPictureBox.BackColor = Colors.Background;
            CustomPictureBox.Image = new Bitmap(@"..\..\..\downloads\Icons\Image.png");
            CustomPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            CustomPictureBox.MouseDown += CustomElement_MouseDown;
            CustomPictureBox.MouseMove += CustomElement_MouseMove;
            CustomPictureBox.MouseUp += CustomElement_MouseUp;
            CustomPictureBox.Resize += CustomElement_Resize;

            int MaxZindex = 0;
            for (int i = 0; i < CustomElements.Count; i++)
            {
                if (CustomElements[i].Zindex > MaxZindex)
                    MaxZindex = CustomElements[i].Zindex;
            }
            int Zindex = MaxZindex + 1;
            CustomElement NewCustomElement = new CustomElement(CustomPictureBox, ControlType.PictureBox, Zindex);
            CustomElements.Add(NewCustomElement);

            SetControlableBorders(NewCustomElement);
            FocusedCustomElement = NewCustomElement;
            LeaveFocusOnCustomElement(NewCustomElement);

            ContainerPanel.Controls.Add(CustomPictureBox);
            CustomPictureBox.BringToFront();
            NextPageButton.BringToFront();
            PreviusPageButton.BringToFront();
            for (int i = 0; i < 4; i++)
                BorderPanels[i].BringToFront();
        }

        private void AddCustomPanel()
        {
            Panel CustomPanel = new Panel();
            CustomPanel.TabIndex = 0;
            CustomPanel.TabStop = false;
            CustomPanel.Location = new Point(0, 0);
            CustomPanel.Size = new Size(150, 150);
            CustomPanel.BorderStyle = BorderStyle.None;
            CustomPanel.BackColor = Colors.Background;
            CustomPanel.MouseDown += CustomElement_MouseDown;
            CustomPanel.MouseMove += CustomElement_MouseMove;
            CustomPanel.MouseUp += CustomElement_MouseUp;
            CustomPanel.Resize += CustomElement_Resize;

            int MaxZindex = 0;
            for (int i = 0; i < CustomElements.Count; i++)
            {
                if (CustomElements[i].Zindex > MaxZindex)
                    MaxZindex = CustomElements[i].Zindex;
            }
            int Zindex = MaxZindex + 1;
            CustomElement NewCustomElement = new CustomElement(CustomPanel, ControlType.Panel, Zindex);
            CustomElements.Add(NewCustomElement);

            SetControlableBorders(NewCustomElement);
            FocusedCustomElement = NewCustomElement;
            LeaveFocusOnCustomElement(NewCustomElement);

            ContainerPanel.Controls.Add(CustomPanel);
            CustomPanel.BringToFront();
            NextPageButton.BringToFront();
            PreviusPageButton.BringToFront();
            for (int i = 0; i < 4; i++)
                BorderPanels[i].BringToFront();
        }

        private void AddCustomButton()
        {
            Button CustomButton = new Button();
            CustomButton.TabIndex = 0;
            CustomButton.TabStop = false;
            CustomButton.Location = new Point(0, 0);
            CustomButton.Size = new Size(200, 100);
            CustomButton.FlatStyle = FlatStyle.Flat;
            CustomButton.BackColor = Colors.Origin;
            CustomButton.FlatAppearance.MouseOverBackColor = Colors.OriginButtonMouseOver;
            CustomButton.FlatAppearance.MouseDownBackColor = Colors.OriginButtonMouseDown;
            CustomButton.FlatAppearance.BorderSize = 0;
            CustomButton.Font = new Font("Calibri", 24);
            CustomButton.ForeColor = Colors.Constant;
            CustomButton.Text = "Button";
            CustomButton.TextAlign = ContentAlignment.MiddleCenter;
            CustomButton.MouseDown += CustomElement_MouseDown;
            CustomButton.MouseMove += CustomElement_MouseMove;
            CustomButton.MouseUp += CustomElement_MouseUp;
            CustomButton.Resize += CustomElement_Resize;

            int MaxZindex = 0;
            for (int i = 0; i < CustomElements.Count; i++)
            {
                if (CustomElements[i].Zindex > MaxZindex)
                    MaxZindex = CustomElements[i].Zindex;
            }
            int Zindex = MaxZindex + 1;
            CustomElement NewCustomElement = new CustomElement(CustomButton, ControlType.Button, Zindex);
            CustomElements.Add(NewCustomElement);

            SetControlableBorders(NewCustomElement);
            FocusedCustomElement = NewCustomElement;
            LeaveFocusOnCustomElement(NewCustomElement);

            ContainerPanel.Controls.Add(CustomButton);
            CustomButton.BringToFront();
            NextPageButton.BringToFront();
            PreviusPageButton.BringToFront();
            for (int i = 0; i < 4; i++)
                BorderPanels[i].BringToFront();
        }

        private void LeaveFocusOnCustomElement(CustomElement CustomElement)
        {
            if (ProcessPosition == 1)
            {
                FocusedCustomElement = CustomElement;

                for (int i = 0; i < CustomElements.Count; i++)
                {
                    if (CustomElements[i] == CustomElement)
                        continue;
                    for (int j = 0; j < CustomElements[i].Control.Controls.Count; j++)
                        CustomElements[i].Control.Controls[j].Visible = false;
                }

                BackgroundPropertiesPanel.Visible = false;
                SwitchesPropertiesPanel.Visible = false;
                ElementsPanel.Visible = false;

                CommonPropertiesPanel.Visible = false;
                CommonPropertiesPanel.Location = new Point(0, 0);

                TextPropertiesPanel.Visible = false;
                TextPropertiesPanel.Location = new Point(0, 0);

                ImagePropertiesPanel.Visible = false;
                ImagePropertiesPanel.Location = new Point(0, 0);

                ZindexPropertiesPanel.Visible = false;
                ZindexPropertiesPanel.Location = new Point(0, 0);

                InteractivePropertiesPanel.Visible = false;
                InteractivePropertiesPanel.Location = new Point(0, 0);

                MousePropertiesPanel.Visible = false;
                MousePropertiesPanel.Location = new Point(0, 0);

                ActionsPanel.Visible = false;
                ActionsPanel.Location = new Point(0, 0);

                if (FocusedCustomElement.ControlType == ControlType.Label)
                {
                    CommonPropertiesPanel.Visible = true;
                    CommonPropertiesPanel.Location = new Point(43, 0);
                    TextPropertiesPanel.Visible = true;
                    TextPropertiesPanel.Location = new Point(CommonPropertiesPanel.Right + 30, 0);
                    ZindexPropertiesPanel.Visible = true;
                    ZindexPropertiesPanel.Location = new Point(TextPropertiesPanel.Right + 30, 0);
                    InteractivePropertiesPanel.Visible = true;
                    InteractivePropertiesPanel.Location = new Point(ZindexPropertiesPanel.Right + 30, 0);
                    ActionsPanel.Visible = true;
                    ActionsPanel.Location = new Point(InteractivePropertiesPanel.Right + 30, 0);
                }
                else if (FocusedCustomElement.ControlType == ControlType.TextBox)
                {
                    CommonPropertiesPanel.Visible = true;
                    CommonPropertiesPanel.Location = new Point(90, 0);
                    TextPropertiesPanel.Visible = true;
                    TextPropertiesPanel.Location = new Point(CommonPropertiesPanel.Right + 30, 0);
                    ZindexPropertiesPanel.Visible = true;
                    ZindexPropertiesPanel.Location = new Point(TextPropertiesPanel.Right + 30, 0);
                    ActionsPanel.Visible = true;
                    ActionsPanel.Location = new Point(ZindexPropertiesPanel.Right + 30, 0);
                }
                else if (FocusedCustomElement.ControlType == ControlType.PictureBox)
                {
                    CommonPropertiesPanel.Visible = true;
                    CommonPropertiesPanel.Location = new Point(50, 0);
                    ImagePropertiesPanel.Visible = true;
                    ImagePropertiesPanel.Location = new Point(CommonPropertiesPanel.Right + 30, 0);
                    ZindexPropertiesPanel.Visible = true;
                    ZindexPropertiesPanel.Location = new Point(ImagePropertiesPanel.Right + 30, 0);
                    ActionsPanel.Visible = true;
                    ActionsPanel.Location = new Point(ZindexPropertiesPanel.Right + 30, 0);
                }
                else if (FocusedCustomElement.ControlType == ControlType.Panel)
                {
                    CommonPropertiesPanel.Visible = true;
                    CommonPropertiesPanel.Location = new Point(210, 0);
                    ZindexPropertiesPanel.Visible = true;
                    ZindexPropertiesPanel.Location = new Point(CommonPropertiesPanel.Right + 30, 0);
                    ActionsPanel.Visible = true;
                    ActionsPanel.Location = new Point(ZindexPropertiesPanel.Right + 30, 0);
                }
                else if (FocusedCustomElement.ControlType == ControlType.Button)
                {
                    CommonPropertiesPanel.Visible = true;
                    CommonPropertiesPanel.Location = new Point(40, 0);
                    TextPropertiesPanel.Visible = true;
                    TextPropertiesPanel.Location = new Point(CommonPropertiesPanel.Right + 20, 0);
                    MousePropertiesPanel.Visible = true;
                    MousePropertiesPanel.Location = new Point(TextPropertiesPanel.Right + 20, 0);
                    ZindexPropertiesPanel.Visible = true;
                    ZindexPropertiesPanel.Location = new Point(MousePropertiesPanel.Right + 20, 0);
                    ActionsPanel.Visible = true;
                    ActionsPanel.Location = new Point(ZindexPropertiesPanel.Right + 20, 0);
                }

                PointXTextBox.Text = Convert.ToString(CustomElement.Control.Left);
                PointYTextBox.Text = Convert.ToString(CustomElement.Control.Top);
                WidthTextBox.Text = Convert.ToString(CustomElement.Control.Width);
                HeightTextBox.Text = Convert.ToString(CustomElement.Control.Height);
                FontSizeTextBox.Text = Convert.ToString(CustomElement.Control.Font.Size);
                ReadCheckBox.Checked = CustomElement.Read;
                ListenCheckBox.Checked = CustomElement.Listen;
            }

            else if (ProcessPosition == 2)
            {
                FocusedCustomElement = CustomElement;

                for (int i = 0; i < CustomElements.Count; i++)
                {
                    if (CustomElements[i] == CustomElement)
                        continue;
                    for (int j = 0; j < CustomElements[i].Control.Controls.Count; j++)
                        CustomElements[i].Control.Controls[j].Visible = false;
                }

                TextContentPanel.Visible = false;
                InputContentPanel.Visible = false;
                PictureContentPanel.Visible = false;
                PanelContentPanel.Visible = false;
                ButtonContentPanel.Visible = false;

                if (FocusedCustomElement.ControlType == ControlType.Label)
                {
                    TextContentPanel.Visible = true;
                    TextContentTextBox.Text = FocusedCustomElement.Control.Text;
                }
                else if (FocusedCustomElement.ControlType == ControlType.TextBox)
                    InputContentPanel.Visible = true;
                else if (FocusedCustomElement.ControlType == ControlType.PictureBox)
                    PictureContentPanel.Visible = true;
                else if (FocusedCustomElement.ControlType == ControlType.Panel)
                    PanelContentPanel.Visible = true;
                else if (FocusedCustomElement.ControlType == ControlType.Button)
                {
                    ButtonContentPanel.Visible = true;
                    if (FocusedCustomElement.Control.Text == "Button")
                        TargetPageIdTextBox.Text = "0";
                    else
                        TargetPageIdTextBox.Text = FocusedCustomElement.Control.Text;
                }
            }
        }

        private void CustomElement_MouseDown(object sender, MouseEventArgs e)
        {
            if (ProcessPosition == 1)
            {
                DragStarted = true;
                DragStartX = e.X;
                DragStartY = e.Y;

                Control CustomControl = sender as Control;
                for (int i = 0; i < CustomElements.Count; i++)
                {
                    if (CustomElements[i].Control == CustomControl)
                    {
                        FocusedCustomElement = CustomElements[i];
                        break;
                    }
                }

                BackgroundPropertiesPanel.Visible = false;
                SwitchesPropertiesPanel.Visible = false;
                ElementsPanel.Visible = false;

                CommonPropertiesPanel.Visible = false;
                CommonPropertiesPanel.Location = new Point(0, 0);

                TextPropertiesPanel.Visible = false;
                TextPropertiesPanel.Location = new Point(0, 0);

                ImagePropertiesPanel.Visible = false;
                ImagePropertiesPanel.Location = new Point(0, 0);

                ZindexPropertiesPanel.Visible = false;
                ZindexPropertiesPanel.Location = new Point(0, 0);

                InteractivePropertiesPanel.Visible = false;
                InteractivePropertiesPanel.Location = new Point(0, 0);

                MousePropertiesPanel.Visible = false;
                MousePropertiesPanel.Location = new Point(0, 0);

                ActionsPanel.Visible = false;
                ActionsPanel.Location = new Point(0, 0);

                if (FocusedCustomElement.ControlType == ControlType.Label)
                {
                    CommonPropertiesPanel.Visible = true;
                    CommonPropertiesPanel.Location = new Point(43, 0);
                    TextPropertiesPanel.Visible = true;
                    TextPropertiesPanel.Location = new Point(CommonPropertiesPanel.Right + 30, 0);
                    ZindexPropertiesPanel.Visible = true;
                    ZindexPropertiesPanel.Location = new Point(TextPropertiesPanel.Right + 30, 0);
                    InteractivePropertiesPanel.Visible = true;
                    InteractivePropertiesPanel.Location = new Point(ZindexPropertiesPanel.Right + 30, 0);
                    ActionsPanel.Visible = true;
                    ActionsPanel.Location = new Point(InteractivePropertiesPanel.Right + 30, 0);
                }
                else if (FocusedCustomElement.ControlType == ControlType.TextBox)
                {
                    CommonPropertiesPanel.Visible = true;
                    CommonPropertiesPanel.Location = new Point(90, 0);
                    TextPropertiesPanel.Visible = true;
                    TextPropertiesPanel.Location = new Point(CommonPropertiesPanel.Right + 30, 0);
                    ZindexPropertiesPanel.Visible = true;
                    ZindexPropertiesPanel.Location = new Point(TextPropertiesPanel.Right + 30, 0);
                    ActionsPanel.Visible = true;
                    ActionsPanel.Location = new Point(ZindexPropertiesPanel.Right + 30, 0);
                }
                else if (FocusedCustomElement.ControlType == ControlType.PictureBox)
                {
                    CommonPropertiesPanel.Visible = true;
                    CommonPropertiesPanel.Location = new Point(50, 0);
                    ImagePropertiesPanel.Visible = true;
                    ImagePropertiesPanel.Location = new Point(CommonPropertiesPanel.Right + 30, 0);
                    ZindexPropertiesPanel.Visible = true;
                    ZindexPropertiesPanel.Location = new Point(ImagePropertiesPanel.Right + 30, 0);
                    ActionsPanel.Visible = true;
                    ActionsPanel.Location = new Point(ZindexPropertiesPanel.Right + 30, 0);
                }
                else if (FocusedCustomElement.ControlType == ControlType.Panel)
                {
                    CommonPropertiesPanel.Visible = true;
                    CommonPropertiesPanel.Location = new Point(210, 0);
                    ZindexPropertiesPanel.Visible = true;
                    ZindexPropertiesPanel.Location = new Point(CommonPropertiesPanel.Right + 30, 0);
                    ActionsPanel.Visible = true;
                    ActionsPanel.Location = new Point(ZindexPropertiesPanel.Right + 30, 0);
                }
                else if (FocusedCustomElement.ControlType == ControlType.Button)
                {
                    CommonPropertiesPanel.Visible = true;
                    CommonPropertiesPanel.Location = new Point(40, 0);
                    TextPropertiesPanel.Visible = true;
                    TextPropertiesPanel.Location = new Point(CommonPropertiesPanel.Right + 20, 0);
                    MousePropertiesPanel.Visible = true;
                    MousePropertiesPanel.Location = new Point(TextPropertiesPanel.Right + 20, 0);
                    ZindexPropertiesPanel.Visible = true;
                    ZindexPropertiesPanel.Location = new Point(MousePropertiesPanel.Right + 20, 0);
                    ActionsPanel.Visible = true;
                    ActionsPanel.Location = new Point(ZindexPropertiesPanel.Right + 20, 0);
                }

                CustomControl.Cursor = Cursors.SizeAll;

                PointXTextBox.Text = Convert.ToString(FocusedCustomElement.Control.Left);
                PointYTextBox.Text = Convert.ToString(FocusedCustomElement.Control.Top);
                WidthTextBox.Text = Convert.ToString(FocusedCustomElement.Control.Width);
                HeightTextBox.Text = Convert.ToString(FocusedCustomElement.Control.Height);
                FontSizeTextBox.Text = Convert.ToString(FocusedCustomElement.Control.Font.Size);
                ReadCheckBox.Checked = FocusedCustomElement.Read;
                ListenCheckBox.Checked = FocusedCustomElement.Listen;

                for (int i = 0; i < CustomControl.Controls.Count; i++)
                    CustomControl.Controls[i].Visible = true;
                LeaveFocusOnCustomElement(FocusedCustomElement);
            }

            else if (ProcessPosition == 2)
            {
                Control CustomControl = sender as Control;
                for (int i = 0; i < CustomElements.Count; i++)
                {
                    if (CustomElements[i].Control == CustomControl)
                    {
                        FocusedCustomElement = CustomElements[i];
                        break;
                    }
                }

                TextContentPanel.Visible = false;
                InputContentPanel.Visible = false;
                PictureContentPanel.Visible = false;
                PanelContentPanel.Visible = false;
                ButtonContentPanel.Visible = false;

                if (FocusedCustomElement.ControlType == ControlType.Label)
                {
                    TextContentPanel.Visible = true;
                    TextContentTextBox.Text = FocusedCustomElement.Control.Text;
                }
                else if (FocusedCustomElement.ControlType == ControlType.TextBox)
                    InputContentPanel.Visible = true;
                else if (FocusedCustomElement.ControlType == ControlType.PictureBox)
                    PictureContentPanel.Visible = true;
                else if (FocusedCustomElement.ControlType == ControlType.Panel)
                    PanelContentPanel.Visible = true;
                else if (FocusedCustomElement.ControlType == ControlType.Button)
                {
                    ButtonContentPanel.Visible = true;
                    if (FocusedCustomElement.Control.Text == "Button")
                        TargetPageIdTextBox.Text = "0";
                    else
                        TargetPageIdTextBox.Text = FocusedCustomElement.Control.Text;
                }

                for (int i = 0; i < CustomControl.Controls.Count; i++)
                    CustomControl.Controls[i].Visible = true;
                LeaveFocusOnCustomElement(FocusedCustomElement);
            }
        }

        private void CustomElement_MouseMove(object sender, MouseEventArgs e)
        {
            NextPageButton.BringToFront();
            PreviusPageButton.BringToFront();

            Control CustomControl = sender as Control;
            if (DragStarted == true)
            {
                CustomControl.Left = CustomControl.Left - DragStartX + e.X;
                CustomControl.Top = CustomControl.Top - DragStartY + e.Y;

                PointXTextBox.Text = Convert.ToString(CustomControl.Left);
                PointYTextBox.Text = Convert.ToString(CustomControl.Top);
            }
        }

        private void CustomElement_MouseUp(object sender, MouseEventArgs e)
        {
            DragStarted = false;

            Control CustomControl = sender as Control;
            CustomControl.Cursor = Cursors.Default;
        }

        private void CustomElement_Resize(object sender, EventArgs e)
        {
            Control CustomControl = sender as Control;

            CustomControl.Controls[0].Location = new Point(0, 0);
            CustomControl.Controls[0].Size = new Size(7, 7);

            CustomControl.Controls[1].Location = new Point(0, 7);
            CustomControl.Controls[1].Size = new Size(3, CustomControl.Height - 14);

            CustomControl.Controls[2].Location = new Point(0, CustomControl.Height - 7);
            CustomControl.Controls[2].Size = new Size(7, 7);

            CustomControl.Controls[3].Location = new Point(7, CustomControl.Height - 3);
            CustomControl.Controls[3].Size = new Size(CustomControl.Width - 14, 3);

            CustomControl.Controls[4].Location = new Point(CustomControl.Width - 7, CustomControl.Height - 7);
            CustomControl.Controls[4].Size = new Size(7, 7);

            CustomControl.Controls[5].Location = new Point(CustomControl.Width - 3, 7);
            CustomControl.Controls[5].Size = new Size(3, CustomControl.Height - 14);

            CustomControl.Controls[6].Location = new Point(CustomControl.Width - 7, 0);
            CustomControl.Controls[6].Size = new Size(7, 7);

            CustomControl.Controls[7].Location = new Point(7, 0);
            CustomControl.Controls[7].Size = new Size(CustomControl.Width - 14, 3);

            PointXTextBox.Text = Convert.ToString(CustomControl.Left);
            PointYTextBox.Text = Convert.ToString(CustomControl.Top);
            WidthTextBox.Text = Convert.ToString(CustomControl.Width);
            HeightTextBox.Text = Convert.ToString(CustomControl.Height);
        }

        private void SetControlableBorders(CustomElement CustomElement)
        {
            Panel LeftTopBorderPanel = new Panel();
            LeftTopBorderPanel.TabIndex = 0;
            LeftTopBorderPanel.TabStop = false;
            LeftTopBorderPanel.Location = new Point(0, 0);
            LeftTopBorderPanel.Size = new Size(7, 7);
            LeftTopBorderPanel.BorderStyle = BorderStyle.None;
            LeftTopBorderPanel.BackColor = Colors.Constant;
            LeftTopBorderPanel.Cursor = Cursors.SizeNWSE;
            LeftTopBorderPanel.MouseEnter += BorderPanel_MouseEnter;
            LeftTopBorderPanel.MouseDown += BorderPanel_MouseDown;
            LeftTopBorderPanel.MouseMove += BorderPanel_MouseMove;
            LeftTopBorderPanel.MouseUp += BorderPanel_MouseUp;

            Panel LeftBorderPanel = new Panel();
            LeftBorderPanel.TabIndex = 0;
            LeftBorderPanel.TabStop = false;
            LeftBorderPanel.Location = new Point(0, 7);
            LeftBorderPanel.Size = new Size(3, CustomElement.Control.Height - 14);
            LeftBorderPanel.BorderStyle = BorderStyle.None;
            LeftBorderPanel.BackColor = Colors.Constant;
            LeftBorderPanel.Cursor = Cursors.SizeWE;
            LeftBorderPanel.MouseEnter += BorderPanel_MouseEnter;
            LeftBorderPanel.MouseDown += BorderPanel_MouseDown;
            LeftBorderPanel.MouseMove += BorderPanel_MouseMove;
            LeftBorderPanel.MouseUp += BorderPanel_MouseUp;

            Panel LeftBottomBorderPanel = new Panel();
            LeftBottomBorderPanel.TabIndex = 0;
            LeftBottomBorderPanel.TabStop = false;
            LeftBottomBorderPanel.Location = new Point(0, CustomElement.Control.Height - 7);
            LeftBottomBorderPanel.Size = new Size(7, 7);
            LeftBottomBorderPanel.BorderStyle = BorderStyle.None;
            LeftBottomBorderPanel.BackColor = Colors.Constant;
            LeftBottomBorderPanel.Cursor = Cursors.SizeNESW;
            LeftBottomBorderPanel.MouseEnter += BorderPanel_MouseEnter;
            LeftBottomBorderPanel.MouseDown += BorderPanel_MouseDown;
            LeftBottomBorderPanel.MouseMove += BorderPanel_MouseMove;
            LeftBottomBorderPanel.MouseUp += BorderPanel_MouseUp;

            Panel BottomBorderPanel = new Panel();
            BottomBorderPanel.TabIndex = 0;
            BottomBorderPanel.TabStop = false;
            BottomBorderPanel.Location = new Point(7, CustomElement.Control.Height - 3);
            BottomBorderPanel.Size = new Size(CustomElement.Control.Width - 14, 3);
            BottomBorderPanel.BorderStyle = BorderStyle.None;
            BottomBorderPanel.BackColor = Colors.Constant;
            BottomBorderPanel.Cursor = Cursors.SizeNS;
            BottomBorderPanel.MouseEnter += BorderPanel_MouseEnter;
            BottomBorderPanel.MouseDown += BorderPanel_MouseDown;
            BottomBorderPanel.MouseMove += BorderPanel_MouseMove;
            BottomBorderPanel.MouseUp += BorderPanel_MouseUp;

            Panel RightBottomBorderPanel = new Panel();
            RightBottomBorderPanel.TabIndex = 0;
            RightBottomBorderPanel.TabStop = false;
            RightBottomBorderPanel.Location = new Point(CustomElement.Control.Width - 7, CustomElement.Control.Height - 7);
            RightBottomBorderPanel.Size = new Size(7, 7);
            RightBottomBorderPanel.BorderStyle = BorderStyle.None;
            RightBottomBorderPanel.BackColor = Colors.Constant;
            RightBottomBorderPanel.Cursor = Cursors.SizeNWSE;
            RightBottomBorderPanel.MouseEnter += BorderPanel_MouseEnter;
            RightBottomBorderPanel.MouseDown += BorderPanel_MouseDown;
            RightBottomBorderPanel.MouseMove += BorderPanel_MouseMove;
            RightBottomBorderPanel.MouseUp += BorderPanel_MouseUp;

            Panel RightBorderPanel = new Panel();
            RightBorderPanel.TabIndex = 0;
            RightBorderPanel.TabStop = false;
            RightBorderPanel.Location = new Point(CustomElement.Control.Width - 3, 7);
            RightBorderPanel.Size = new Size(3, CustomElement.Control.Height - 14);
            RightBorderPanel.BorderStyle = BorderStyle.None;
            RightBorderPanel.BackColor = Colors.Constant;
            RightBorderPanel.Cursor = Cursors.SizeWE;
            RightBorderPanel.MouseEnter += BorderPanel_MouseEnter;
            RightBorderPanel.MouseDown += BorderPanel_MouseDown;
            RightBorderPanel.MouseMove += BorderPanel_MouseMove;
            RightBorderPanel.MouseUp += BorderPanel_MouseUp;

            Panel RightTopBorderPanel = new Panel();
            RightTopBorderPanel.TabIndex = 0;
            RightTopBorderPanel.TabStop = false;
            RightTopBorderPanel.Location = new Point(CustomElement.Control.Width - 7, 0);
            RightTopBorderPanel.Size = new Size(7, 7);
            RightTopBorderPanel.BorderStyle = BorderStyle.None;
            RightTopBorderPanel.BackColor = Colors.Constant;
            RightTopBorderPanel.Cursor = Cursors.SizeNESW;
            RightTopBorderPanel.MouseEnter += BorderPanel_MouseEnter;
            RightTopBorderPanel.MouseDown += BorderPanel_MouseDown;
            RightTopBorderPanel.MouseMove += BorderPanel_MouseMove;
            RightTopBorderPanel.MouseUp += BorderPanel_MouseUp;

            Panel TopBorderPanel = new Panel();
            TopBorderPanel.TabIndex = 0;
            TopBorderPanel.TabStop = false;
            TopBorderPanel.Location = new Point(7, 0);
            TopBorderPanel.Size = new Size(CustomElement.Control.Width - 14, 3);
            TopBorderPanel.BorderStyle = BorderStyle.None;
            TopBorderPanel.BackColor = Colors.Constant;
            TopBorderPanel.Cursor = Cursors.SizeNS;
            TopBorderPanel.MouseEnter += BorderPanel_MouseEnter;
            TopBorderPanel.MouseDown += BorderPanel_MouseDown;
            TopBorderPanel.MouseMove += BorderPanel_MouseMove;
            TopBorderPanel.MouseUp += BorderPanel_MouseUp;

            CustomElement.Control.Controls.Add(LeftTopBorderPanel);
            CustomElement.Control.Controls.Add(LeftBorderPanel);
            CustomElement.Control.Controls.Add(LeftBottomBorderPanel);
            CustomElement.Control.Controls.Add(BottomBorderPanel);
            CustomElement.Control.Controls.Add(RightBottomBorderPanel);
            CustomElement.Control.Controls.Add(RightBorderPanel);
            CustomElement.Control.Controls.Add(RightTopBorderPanel);
            CustomElement.Control.Controls.Add(TopBorderPanel);
        }

        private void BorderPanel_MouseEnter(object sender, EventArgs e)
        {
            if (ProcessPosition == 2)
            {
                Panel BorderPanel = sender as Panel;
                BorderPanel.Cursor = Cursors.Default;
            }
        }

        private void BorderPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (ProcessPosition == 1)
            {
                ResizeStarted = true;
                ResizeStartX = e.X;
                ResizeStartY = e.Y;
            }
        }

        private void BorderPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (ResizeStarted == true && ProcessPosition == 1)
            {
                for (int i = 0; i < CustomElements.Count; i++)
                {
                    if (sender == CustomElements[i].Control.Controls[0])
                    {
                        CustomElements[i].Control.Width += ResizeStartX - e.X;
                        CustomElements[i].Control.Left -= ResizeStartX - e.X;
                        CustomElements[i].Control.Height += ResizeStartY - e.Y;
                        CustomElements[i].Control.Top -= ResizeStartY - e.Y;
                        return;
                    }

                    if (sender == CustomElements[i].Control.Controls[1])
                    {
                        CustomElements[i].Control.Width += ResizeStartX - e.X;
                        CustomElements[i].Control.Left -= ResizeStartX - e.X;
                        return;
                    }

                    if (sender == CustomElements[i].Control.Controls[2])
                    {
                        CustomElements[i].Control.Width += ResizeStartX - e.X;
                        CustomElements[i].Control.Left -= ResizeStartX - e.X;
                        CustomElements[i].Control.Height += e.Y - ResizeStartY;
                        return;
                    }

                    if (sender == CustomElements[i].Control.Controls[3])
                    {
                        CustomElements[i].Control.Height += e.Y - ResizeStartY;
                        return;
                    }

                    if (sender == CustomElements[i].Control.Controls[4])
                    {
                        CustomElements[i].Control.Width += e.X - ResizeStartX;
                        CustomElements[i].Control.Height += e.Y - ResizeStartY;
                        return;
                    }

                    if (sender == CustomElements[i].Control.Controls[5])
                    {
                        CustomElements[i].Control.Width += e.X - ResizeStartX;
                        return;
                    }

                    if (sender == CustomElements[i].Control.Controls[6])
                    {
                        CustomElements[i].Control.Width += e.X - ResizeStartX;
                        CustomElements[i].Control.Height += ResizeStartY - e.Y;
                        CustomElements[i].Control.Top -= ResizeStartY - e.Y;
                        return;
                    }

                    if (sender == CustomElements[i].Control.Controls[7])
                    {
                        CustomElements[i].Control.Height += ResizeStartY - e.Y;
                        CustomElements[i].Control.Top -= ResizeStartY - e.Y;
                        return;
                    }
                }
            }
        }

        private void BorderPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (ProcessPosition == 1)
                ResizeStarted = false;
        }
    }

    public class CustomElement
    {
        private Control control;
        private ControlType controlType;
        private int zindex;
        private bool read = false;
        private bool listen = false;

        public Control Control
        {
            get { return control; }
        }

        public ControlType ControlType
        {
            get { return controlType; }
        }

        public int Zindex
        {
            get { return zindex; }
            set { zindex = value; }
        }

        public bool Read
        {
            get { return read; }
            set { read = value; }
        }

        public bool Listen
        {
            get { return listen; }
            set { listen = value; }
        }

        public CustomElement(Control ElementControl, ControlType ElementControlType, int ElementZindex)
        {
            control = ElementControl;
            controlType = ElementControlType;
            zindex = ElementZindex;
        }

        public CustomElement()
        { }
    }

    public enum ControlType { Label, TextBox, PictureBox, Panel, Button }

    public class Page
    {
        private List<CustomElement> customElements;
        private int id;

        public int Id
        {
            get { return id; }
        }

        public List<CustomElement> CustomElements
        {
            get { return customElements; }
        }

        public Page(int Id, List<CustomElement> CustomElements)
        {
            this.id = Id;
            customElements = new List<CustomElement>();
            for (int i = 0; i < CustomElements.Count; i++)
                customElements.Add(CustomElements[i]);
        }
    }
}
