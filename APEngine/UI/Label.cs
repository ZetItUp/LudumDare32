using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace APEngine.UI
{
    public class Label : Window
    {
        GameFont font;
        public bool TextCentered { get; set; }
        private Vector2 textPos { get; set; }
        public float UIScale { get; set; }
        public int TextXOffset { get; set; }
        public int TextYOffset { get; set; }

        public string Text
        {
            get { return font.Text; }
            set { font.Text = value; }
        }

        public Label(Rectangle windowRect)
            : base(windowRect)
        {
            BackgroundColor = Color.Transparent;
            UIScale = 1.0f;
            DrawWindow = false;
            TextXOffset = 5;
            TextYOffset = 6;
        }

        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);
            textPos = Position;
            font = new GameFont(new Rectangle(WindowRectangle.X + TextXOffset, WindowRectangle.Y + TextYOffset, WindowRectangle.Width - (TextXOffset * 2), WindowRectangle.Height - (TextYOffset * 2)), WindowRectangle.Width - (TextXOffset * 2));
            font.LoadContent(Content, FontColor.Blue);
            font.Text = "";
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if(TextCentered)
            {
                textPos = new Vector2(WindowRectangle.X + WindowRectangle.Width / 2 - font.MaxWidth / 2, Position.Y);
            }

            font.WindowRectangle = new Rectangle(WindowRectangle.X + TextXOffset, WindowRectangle.Y + TextYOffset, WindowRectangle.Width - (TextXOffset * 2), WindowRectangle.Height - (TextYOffset * 2));
            font.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            font.Draw(spriteBatch, UIScale);
        }
    }
}
