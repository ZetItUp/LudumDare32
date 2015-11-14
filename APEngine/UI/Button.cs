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
    public class Button : Window
    {
        public Color ButtonColor { get; set; }
        public Color HoverColor { get; set; }
        public Texture2D ButtonImage { get; set; }
        private GameFont font;

        bool isHover { get; set; }

        public event EventHandler Clicked;
        public string Text
        {
            get { return font.Text; }
            set { font.Text = value; }
        }

        private bool flipHoriz;
        private bool flipVert;

        public bool FlipHorizontalImage
        {
            get { return flipHoriz; }
            set
            {
                flipHoriz = value;

                if(flipVert)
                    flipVert = false;
            }
        }

        public bool FlipVerticalImage
        {
            get { return flipVert; }
            set
            {
                flipVert = value;

                if(flipHoriz)
                    flipHoriz = false;
            }
        }

        public Button(Rectangle buttonRect)
            : base(buttonRect)
        {
            ButtonColor = new Color(0, 148, 255);
            HoverColor = new Color(0, 50, 90);
            isHover = false;
            DrawWindow = false;
        }

        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);
            
            if(Parent == null)
                font = new GameFont(WindowRectangle, WindowRectangle.Width);
            else
                font = new GameFont(new Rectangle(Parent.WindowRectangle.X + WindowRectangle.X, Parent.WindowRectangle.Y + WindowRectangle.Y + (WindowRectangle.Height / 2 - 8), WindowRectangle.Width, WindowRectangle.Height), WindowRectangle.Width);
            font.LoadContent(Content);
            font.CenterText = true;

            Clicked += Button_Clicked;
        }

        void Button_Clicked(object sender, EventArgs e)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;
            Rectangle mRect = new Rectangle((int)(AMouse.MousePosition().X / gs.camera.Zoom), (int)(AMouse.MousePosition().Y / gs.camera.Zoom), 1, 1);
            Rectangle bRect = new Rectangle((int)(WindowRectangle.X / gs.camera.Zoom), (int)(WindowRectangle.Y / gs.camera.Zoom), (int)(WindowRectangle.Width / gs.camera.Zoom), (int)(WindowRectangle.Height / gs.camera.Zoom));

            if(mRect.Intersects(bRect))
            {
                isHover = true;

                if(AMouse.MousePressed(AMouse.MouseButton.Left))
                {
                    if(Clicked != null)
                    {
                        Clicked(this, EventArgs.Empty);
                    }
                }
            }
            else
            {
                isHover = false;
            }

            font.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            Color currColor;

            if(!isHover)
            {
                currColor = ButtonColor;
            }
            else
            {
                currColor = HoverColor;
            }

            spriteBatch.Draw(WindowGraphics.ButtonTexture, new Rectangle(WindowRectangle.X, WindowRectangle.Y, 4, 4), new Rectangle(0, 0, 4, 4), currColor);
            spriteBatch.Draw(WindowGraphics.ButtonTexture, new Rectangle(WindowRectangle.X + WindowRectangle.Width - 4, WindowRectangle.Y, 4, 4), new Rectangle(28, 0, 4, 4), currColor);

            for(int x = 4; x < WindowRectangle.Width - 4; x++)
            {
                spriteBatch.Draw(WindowGraphics.ButtonTexture, new Rectangle(WindowRectangle.X + x, WindowRectangle.Y, 1, 4), new Rectangle(4, 0, 1, 4), currColor);
                spriteBatch.Draw(WindowGraphics.ButtonTexture, new Rectangle(WindowRectangle.X + x, WindowRectangle.Y + WindowRectangle.Height - 4, 1, 4), new Rectangle(4, 28, 1, 4), currColor);
            }

            for(int y = 4; y < WindowRectangle.Height - 4; y++)
            {
                spriteBatch.Draw(WindowGraphics.ButtonTexture, new Rectangle(WindowRectangle.X, WindowRectangle.Y + y, 4, 1), new Rectangle(0, 4, 4, 1), currColor);
                spriteBatch.Draw(WindowGraphics.ButtonTexture, new Rectangle(WindowRectangle.X + WindowRectangle.Width - 4, WindowRectangle.Y + y, 4, 1), new Rectangle(28, 4, 4, 1), currColor);
            }

            for(int xx = 4; xx < WindowRectangle.Width - 4; xx++)
            {
                for(int yy = 4; yy < WindowRectangle.Height - 4; yy++)
                {
                    spriteBatch.Draw(WindowGraphics.ButtonTexture, new Rectangle(WindowRectangle.X + xx, WindowRectangle.Y + yy, 1, 1), currColor);
                }
            }

            spriteBatch.Draw(WindowGraphics.ButtonTexture, new Rectangle(WindowRectangle.X, WindowRectangle.Y + WindowRectangle.Height - 4, 4, 4), new Rectangle(0, 28, 4, 4), currColor);
            spriteBatch.Draw(WindowGraphics.ButtonTexture, new Rectangle(WindowRectangle.X + WindowRectangle.Width - 4, WindowRectangle.Height + WindowRectangle.Y - 4, 4, 4), new Rectangle(28, 28, 4, 4), currColor);

            if(ButtonImage != null)
            {
                if(FlipHorizontalImage)
                {
                    spriteBatch.Draw(ButtonImage, new Rectangle(WindowRectangle.X + 2, WindowRectangle.Y + 2, WindowRectangle.Width - 4, WindowRectangle.Height - 4), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 1f);
                }
                else if(FlipVerticalImage)
                {
                    spriteBatch.Draw(ButtonImage, new Rectangle(WindowRectangle.X + 2, WindowRectangle.Y + 2, WindowRectangle.Width - 4, WindowRectangle.Height - 4), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipVertically, 1f);
                }
                else
                {
                    spriteBatch.Draw(ButtonImage, new Rectangle(WindowRectangle.X + 2, WindowRectangle.Y + 2, WindowRectangle.Width - 4, WindowRectangle.Height - 4), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
                }
            }

            font.Draw(spriteBatch);
        }
    }
}
