using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace APEngine.UI
{
    public class InfoBox : Window
    {
        GameFont font;
        Texture2D infoWnd;

        public string Text
        {
            get
            {
                return font.Text;
            }
            set
            {
                font.Text = value;
            }
        }

        public InfoBox(Rectangle windowRect)
            : base(windowRect)
        {
            Visible = true;
            WindowRectangle = windowRect;
            BackgroundColor = Color.Transparent;
        }

        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);
            infoWnd = Content.Load<Texture2D>("Data\\GFX\\InfoBox");

            font = new GameFont(new Rectangle(WindowRectangle.X + 10, WindowRectangle.Y + 10, WindowRectangle.Width - 20, WindowRectangle.Height - 20), WindowRectangle.Width - 30);
            font.LoadContent(Content, FontColor.Yellow);
            font.MultiLine = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            font.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible)
                return;

            base.Draw(spriteBatch);

            spriteBatch.Draw(infoWnd, WindowRectangle, Color.White);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Engine.Monitor.GetTransformationMatrix());
            font.Draw(spriteBatch);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Engine.Monitor.GetTransformationMatrix());
        }
    }
}
