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
    public delegate void TextBoxEvent(TextBox sender);

    public class TextBox : Window, IKeyboardSubscriber
    {
        public event TextBoxEvent Clicked;
        public event TextBoxEvent EscapePressed;
        public event TextBoxEvent OnEnterPressed;
        public event TextBoxEvent OnTabPressed;

        public bool NumericBox { get; set; }
        public bool PasswordBox { get; set; }
        public bool Selected { get; set; }
        public bool MultiLine { get; set; }
        public bool Locked { get; set; }
        public int MaxLength { get; set; }
        private bool CaretVisible { get; set; }

        public Color CaretColor { get; set; }
        private GameFont txtText;
        private string txtPassword;
        public char PasswordChar { get; set; }
        private int xTextOffset { get; set; }

        public String Text
        {
            get
            {
                if(PasswordBox)
                {
                    return txtPassword;
                }
                else
                {
                    return txtText.Text;
                }
            }
            set
            {
                if(value.Length > MaxLength)
                {
                    value = value.Substring(0, MaxLength);
                }

                if(PasswordBox)
                {
                    txtPassword = value;
                    txtText.Text = "";
                    foreach(Char c in value)
                    {
                        txtText.Text += PasswordChar;
                    }
                }
                else
                {
                    txtText.Text = value;
                }
            }
        }

        public TextBox(Rectangle windowRect)
            : base(windowRect)
        {
            txtPassword = "";
            Visible = true;
            MultiLine = false;
            NumericBox = false;
            Locked = false;
            Selected = false;
            CaretColor = Color.White;
            PasswordChar = '*';
            //DrawWindow = false;  // Maybe use?
            BackgroundColor = new Color(0, 148, 255);
            xTextOffset = 10;
            MaxLength = 25;
        }

        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);

            if(Parent == null)
                this.txtText = new GameFont(WindowRectangle, WindowRectangle.Width);
            else
                this.txtText = new GameFont(
                    new Rectangle(
                        Parent.WindowRectangle.X + WindowRectangle.X + xTextOffset, 
                        Parent.WindowRectangle.Y + WindowRectangle.Y + (WindowRectangle.Height / 2 - 8), 
                        WindowRectangle.Width - (xTextOffset * 2), 
                        WindowRectangle.Height), 
                        WindowRectangle.Width);

            this.txtText.LoadContent(Content);
        }

        float interval = 400f;
        float currTime = 0f;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if(!Visible)
                return;

            this.txtText.Update(gameTime);

            if(Selected && AInput.KeyPressed(Keys.Escape))
            {
                Selected = false;

                if(EscapePressed != null)
                {
                    EscapePressed(this);
                }
            }
            else if (Selected && AInput.KeyPressed(Keys.Enter))
            {
                if (OnEnterPressed != null)
                {
                    OnEnterPressed(this);
                }
            }

            if(AMouse.MouseRectangle().Intersects(WindowRectangle))
            {
                if(AMouse.MousePressed(AMouse.MouseButton.Left))
                {
                    if(Clicked != null)
                    {
                        Clicked(this);
                    }
                }
            }

            currTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if(currTime > interval)
            {
                CaretVisible = !CaretVisible;

                currTime -= interval;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if(!Visible)
                return;

            if(CaretVisible && Selected && !Locked)
            {
                int xpos = (int)FontManager.GetStringSize(txtText.Text).X;

                spriteBatch.Draw(WindowGraphics.WindowBasics, new Rectangle(WindowRectangle.X + xTextOffset + xpos, WindowRectangle.Y + WindowRectangle.Height / 2 - 8, 3, 16), new Rectangle(0, 0, 3, 16), CaretColor);
            }

            this.txtText.Draw(spriteBatch);
        }

        public void RecieveCommandInput(char command)
        {
            if(Locked)
                return;

            switch(command)
            {
                case '\b': //backspace
                    if(Text.Length > 0)
                    {
                        Text = Text.Substring(0, Text.Length - 1);
                    }
                    break;
                case '\r': //return
                    if(OnEnterPressed != null)
                        OnEnterPressed(this);
                    break;
                case '\t': //tab
                    if(OnTabPressed != null)
                        OnTabPressed(this);
                    break;
                default:
                    break;
            }
        }

        public void RecieveTextInput(char inputChar)
        {
            if(Locked)
                return;

            if(NumericBox)
            {
                if(inputChar == '0' || inputChar == '1' || inputChar == '2' ||
                    inputChar == '3' || inputChar == '4' || inputChar == '5' ||
                    inputChar == '6' || inputChar == '7' || inputChar == '8' || inputChar == '9')
                {
                    Text = Text + inputChar;
                }
            }
            else
            {
                Text = Text + inputChar;
            }
        }

        public void RecieveTextInput(char inputChar, CharacterEventArgs e)
        {
            if(Locked)
                return;
        }

        public void RecieveTextInput(string text)
        {
            if(Locked)
                return;

            Text = Text + text;
        }

        public void RecieveSpecialInput(Keys key)
        {
            if(Locked)
                return;
        }

        public void RecieveSpecialInput(Keys key, KeyEventArgs e)
        {
            if(Locked)
                return;
        }

        public void RecieveInput(KeyEventArgs e)
        {
            if(Locked)
                return;
        }
    }
}
