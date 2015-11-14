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
    public class CheckBox : Window
    {
        public bool Checked { get; set; }

        private GameFont font;
        public string Text
        {
            get { return font.Text; }
            set { font.Text = value; }
        }

        public CheckBox(Rectangle windowRectangle)
            : base(windowRectangle)
        {
            DrawWindow = false;
        }

        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);

            if (Parent == null)
                font = new GameFont(WindowRectangle, WindowRectangle.Width);
            else
                font = new GameFont(new Rectangle(Parent.WindowRectangle.X + 8, Parent.WindowRectangle.Y + WindowRectangle.Y + (WindowRectangle.Height / 2 - 8), WindowRectangle.Width, WindowRectangle.Height), WindowRectangle.Width);
            font.LoadContent(Content);
            font.CenterText = true;

            WindowClicked += CheckBox_WindowClicked;
        }

        void CheckBox_WindowClicked(object sender, EventArgs e)
        {
            Checked = !Checked;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            font.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if(Checked)
            {
                spriteBatch.Draw(WindowGraphics.CheckedTexture, new Rectangle(WindowRectangle.X, WindowRectangle.Y + 3, 16, 16), Color.White);
            }
            else
            {
                spriteBatch.Draw(WindowGraphics.UncheckedTexture, new Rectangle(WindowRectangle.X, WindowRectangle.Y + 3, 16, 16), Color.White);
            }

            font.Draw(spriteBatch);
        }
    }
}
