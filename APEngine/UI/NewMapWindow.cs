using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using AtriLib2;

namespace APEngine.UI
{
    public class NewMapWindow : Window
    {
        private UI.Panel wndPanel;

        static string DEFAULT_WATER_LEVEL = "3";

        public NewMapWindow(Rectangle windowRect)
            : base(windowRect)
        {
            wndPanel = new Panel(new Rectangle(windowRect.X + 4, windowRect.Y + 4, windowRect.Width - 8, windowRect.Height - 8));
        }

        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);

            wndPanel.LoadContent(Content);
            wndPanel.DrawWindow = false;

            UI.Button bClose = new UI.Button(new Rectangle(WindowRectangle.Width - 112, WindowRectangle.Height - 48, 96, 32));
            bClose.Parent = wndPanel;
            bClose.LoadContent(Content);
            bClose.Text = "Cancel";
            bClose.Clicked += bClose_Clicked;
            wndPanel.AddWindow("btnClose", bClose);

            UI.Button bCreate = new UI.Button(new Rectangle(WindowRectangle.Width - bClose.WindowRectangle.Width - 16 - 112, WindowRectangle.Height - 48, 96, 32));
            bCreate.Parent = wndPanel;
            bCreate.LoadContent(Content);
            bCreate.Text = "Create";
            bCreate.Clicked += bCreate_Clicked;
            wndPanel.AddWindow("btnCreate", bCreate);

            UI.ComboBox cboWorldType = new ComboBox(new Rectangle(WindowRectangle.X + 9, WindowRectangle.Y + 104, 256, 32));
            cboWorldType.LoadContent(Content);
            cboWorldType.AddItem("Forest Jungle");
            cboWorldType.AddItem("Prison Cells");
            cboWorldType.AddItem("Sandy Beach");
            cboWorldType.SelectedItem = "Forest Jungle";
            wndPanel.AddWindow("cboWorldType", cboWorldType);

            UI.Label lblWorldType = new UI.Label(new Rectangle(0, 74, wndPanel.WindowRectangle.Width, 20));
            lblWorldType.Parent = wndPanel;
            lblWorldType.LoadContent(Content);
            lblWorldType.Text = "World Type";
            wndPanel.AddWindow("lblWorldType", lblWorldType);
            
            UI.Label lblName = new UI.Label(new Rectangle(0, 12, wndPanel.WindowRectangle.Width, 20));
            lblName.Parent = wndPanel;
            lblName.LoadContent(Content);
            lblName.Text = "Level Name:";
            wndPanel.AddWindow("lblLvlName", lblName);

            UI.TextBox txtLevelName = new TextBox(new Rectangle(4, lblName.WindowRectangle.Height * 2, wndPanel.WindowRectangle.Width - 10, 32));
            txtLevelName.Parent = wndPanel;
            txtLevelName.LoadContent(Content);
            txtLevelName.Clicked += txtLevelName_Clicked;
            txtLevelName.OnTabPressed += txtLevelName_OnTabPressed;
            txtLevelName.Text = "bla";
            wndPanel.AddWindow("txtLevelName", txtLevelName);

            UI.Label lblSize = new UI.Label(new Rectangle(0, 140, wndPanel.WindowRectangle.Width, 20));
            lblSize.Parent = wndPanel;
            lblSize.LoadContent(Content);
            lblSize.Text = "World Size (In Tiles)";
            wndPanel.AddWindow("lblSize", lblSize);

            UI.Label lblWorldWidth = new UI.Label(new Rectangle(0, 180, 150, 20));
            lblWorldWidth.Parent = wndPanel;
            lblWorldWidth.LoadContent(Content);
            lblWorldWidth.Text = "World Width:";
            wndPanel.AddWindow("lblWorldWidth", lblWorldWidth);

            UI.Label lblWorldHeight = new UI.Label(new Rectangle(220, 180, 50, 20));
            lblWorldHeight.Parent = wndPanel;
            lblWorldHeight.LoadContent(Content);
            lblWorldHeight.Text = "World Height:";
            wndPanel.AddWindow("lblWorldHeight", lblWorldHeight);

            UI.TextBox txtWorldWidth = new TextBox(new Rectangle(140, 175, 80, 32));
            txtWorldWidth.Parent = wndPanel;
            txtWorldWidth.LoadContent(Content);
            txtWorldWidth.MaxLength = 3;
            txtWorldWidth.Clicked += txtLevelWidth_Clicked;
            txtWorldWidth.OnTabPressed += txtWorldWidth_OnTabPressed;
            txtWorldWidth.NumericBox = true;
            txtWorldWidth.Text = "22";
            wndPanel.AddWindow("txtLevelWidth", txtWorldWidth);

            UI.TextBox txtLevelHeight = new TextBox(new Rectangle(360, 175, 80, 32));
            txtLevelHeight.Parent = wndPanel;
            txtLevelHeight.LoadContent(Content);
            txtLevelHeight.NumericBox = true;
            txtLevelHeight.MaxLength = 3;
            txtLevelHeight.Text = "22";
            txtLevelHeight.Clicked += txtLevelHeight_Clicked;
            txtLevelHeight.OnTabPressed += txtLevelHeight_OnTabPressed;
            wndPanel.AddWindow("txtLevelHeight", txtLevelHeight);

            UI.CheckBox chkWater = new CheckBox(new Rectangle(4, WindowRectangle.Height - 48, 150, 24));
            chkWater.Parent = wndPanel;
            chkWater.LoadContent(Content);
            chkWater.Text = "Water Level";
            chkWater.WindowClicked += chkWater_WindowClicked;
            wndPanel.AddWindow("chkWater", chkWater);

            UI.TextBox txtWaterLevel = new TextBox(new Rectangle(160, WindowRectangle.Height - 48, 40, 32));
            txtWaterLevel.Parent = wndPanel;
            txtWaterLevel.LoadContent(Content);
            txtWaterLevel.NumericBox = true;
            txtWaterLevel.MaxLength = 1;
            txtWaterLevel.Text = DEFAULT_WATER_LEVEL;
            txtWaterLevel.Clicked += txtWaterLevel_Clicked;
            txtWaterLevel.OnTabPressed += txtWaterLevel_OnTabPressed;
            txtWaterLevel.OnEnterPressed += txtWaterLevel_OnEnterPressed;
            txtWaterLevel.Visible = false;
            wndPanel.AddWindow("txtWaterLevel", txtWaterLevel);
        }

        void txtWaterLevel_OnEnterPressed(TextBox sender)
        {
            if (sender.Text == "")
                sender.Text = DEFAULT_WATER_LEVEL;
        }

        void chkWater_WindowClicked(object sender, EventArgs e)
        {
            var wnd = sender as CheckBox;
            var wndWaterLvl = wndPanel.GetWindow<TextBox>("txtWaterLevel");

            wndWaterLvl.Visible = wnd.Checked;
        }

        void txtWaterLevel_OnTabPressed(TextBox sender)
        {
            if (sender.Text == "")
                sender.Text = DEFAULT_WATER_LEVEL;

            txtLevelName_Clicked(sender);
        }

        void txtWaterLevel_Clicked(TextBox sender)
        {
            Engine.KeyboardDispatcher.Subscriber = wndPanel.GetWindow<TextBox>("txtWaterLevel");
        }

        void bCreate_Clicked(object sender, EventArgs e)
        {
            var widthWindow = wndPanel.GetWindow<TextBox>("txtLevelWidth");
            var heightWindow = wndPanel.GetWindow<TextBox>("txtLevelHeight");
            var nameWindow = wndPanel.GetWindow<TextBox>("txtLevelName");
            var wndWaterLvl = wndPanel.GetWindow<TextBox>("txtWaterLevel");
            var cboWorldType = wndPanel.GetWindow<ComboBox>("cboWorldType");
            
            if((!ATools.IsNumeric(widthWindow.Text) || !ATools.IsNumeric(heightWindow.Text)) && nameWindow.Text != "")
            {
                return;
            }

            DestroyMe = true;

            ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;
            Map.Theme lvlType = Map.Theme.JungleForest;

            if(cboWorldType.SelectedItem == "Jungle Forest")
            {
                lvlType = Map.Theme.JungleForest;
            }
            else if(cboWorldType.SelectedItem == "Sandy Beach")
            {
                lvlType = Map.Theme.SandyBeach;
            }
            else if(cboWorldType.SelectedItem == "Prison Cells")
            {
                lvlType = Map.Theme.Prison;
            }

            if (wndWaterLvl.Visible)
            {
                gs.level.CreateNewLevel(lvlType, int.Parse(widthWindow.Text), int.Parse(heightWindow.Text), nameWindow.Text, int.Parse(wndWaterLvl.Text));
            }
            else
            {
                gs.level.CreateNewLevel(lvlType, int.Parse(widthWindow.Text), int.Parse(heightWindow.Text), nameWindow.Text);
            }
        }

        void txtLevelName_OnTabPressed(TextBox sender)
        {
            txtLevelWidth_Clicked(sender);
        }

        void txtWorldWidth_OnTabPressed(TextBox sender)
        {
            txtLevelHeight_Clicked(sender);
        }

        void txtLevelHeight_OnTabPressed(TextBox sender)
        {
            var wnd = wndPanel.GetWindow<TextBox>("txtWaterLevel");

            if (wnd.Visible)
            {
                txtWaterLevel_Clicked(sender);
            }
            else
            {
                txtLevelName_Clicked(sender);
            }
        }

        void txtLevelHeight_Clicked(TextBox sender)
        {
            Engine.KeyboardDispatcher.Subscriber = wndPanel.GetWindow<TextBox>("txtLevelHeight");
        }

        void txtLevelWidth_Clicked(TextBox sender)
        {
            Engine.KeyboardDispatcher.Subscriber = wndPanel.GetWindow<TextBox>("txtLevelWidth");
        }

        void txtLevelName_Clicked(TextBox sender)
        {
            Engine.KeyboardDispatcher.Subscriber = wndPanel.GetWindow<TextBox>("txtLevelName");
        }

        void bClose_Clicked(object sender, EventArgs e)
        {
            DestroyMe = true;
            ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;
            gs.Player.AllowMovement = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            wndPanel.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if(!Visible)
                return;

            wndPanel.Draw(spriteBatch);
        }
    }
}
