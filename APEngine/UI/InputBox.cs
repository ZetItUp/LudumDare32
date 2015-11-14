using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AtriLib2;

namespace APEngine.UI
{
    public class InputBox : Window
    {
        private UI.Panel wndPanel;

        public String Message
        {
            set
            {
                var wndText = wndPanel.GetWindow<UI.Label>("lblText");
                wndText.Text = value;
            }
        }

        public String Text
        {
            get
            {
                var wndText = wndPanel.GetWindow<UI.TextBox>("txtText");
                return wndText.Text;
            }
            set
            {
                var wndText = wndPanel.GetWindow<TextBox>("txtText");
                wndText.Text = value;
            }
        }

        public InputBox(string title, Rectangle windowRectangle)
            : base(windowRectangle)
        {
            wndPanel = new Panel(new Rectangle(windowRectangle.X + 4, windowRectangle.Y + 4, windowRectangle.Width - 8, windowRectangle.Height - 8));
            Title = title;
            DrawWindow = true;
        }

        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);

            wndPanel.LoadContent(Content);
            wndPanel.DrawWindow = false;
            wndPanel.Parent = this;

            Label lblText;
            TextBox txtText;
            Button btnOK;
            Button btnCancel;

            lblText = new Label(new Rectangle(4, 20, WindowRectangle.Width - 106, 50));
            lblText.Parent = wndPanel;
            lblText.LoadContent(Content);
            wndPanel.AddWindow("lblText", lblText);

            txtText = new TextBox(new Rectangle(4, 60, WindowRectangle.Width - 118, 32));
            txtText.Parent = wndPanel;
            txtText.LoadContent(Content);
            txtText.Clicked += txtText_Clicked;
            wndPanel.AddWindow("txtText", txtText);

            btnOK = new Button(new Rectangle(WindowRectangle.Width - 110, 20, 96, 32));
            btnOK.Parent = wndPanel;
            btnOK.LoadContent(Content);
            btnOK.Text = "OK";
            btnOK.Clicked += btnOK_Clicked;
            wndPanel.AddWindow("btnOK", btnOK);

            btnCancel = new Button(new Rectangle(WindowRectangle.Width - 110, 60, 96, 32));
            btnCancel.Parent = wndPanel;
            btnCancel.LoadContent(Content);
            btnCancel.Text = "Cancel";
            btnCancel.Clicked += btnCancel_Clicked;
            wndPanel.AddWindow("btnCancel", btnCancel);
        }

        void btnOK_Clicked(object sender, EventArgs e)
        {
            if(wndPanel.Parent != null)
            {
                var wndText = wndPanel.GetWindow<TextBox>("txtText");
                wndPanel.Parent.ReturnString = wndText.Text;
            }

            DestroyMe = true;

            ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;
            gs.EditUI.MousePointer.AllowWorldModification = true;
        }

        void btnCancel_Clicked(object sender, EventArgs e)
        {
            DestroyMe = true;

            ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;
            gs.EditUI.MousePointer.AllowWorldModification = true;
        }

        void txtText_Clicked(TextBox sender)
        {
            Engine.KeyboardDispatcher.Subscriber = sender;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!Visible)
                return;

            wndPanel.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (!Visible)
                return;

            wndPanel.Draw(spriteBatch);
        }
    }
}
