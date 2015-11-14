using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AtriLib2;

namespace APEngine.ScreenManagers.SceneEffects
{
    public class PrintEffect : SceneEffect
    {
        UI.GameFont font;
        public string Text { get; set; }
        public Rectangle Rect { get; set; }

        public PrintEffect()
            : base()
        {

        }

        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);

            font = new UI.GameFont(Rect, Rect.Width);
            font.LoadContent(Content, UI.FontColor.Yellow);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (HasPlayed || !PlayThis)
                return;

            font.Text = Text;
            font.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (HasPlayed || !PlayThis || !Visible)
                return;

            font.Draw(spriteBatch);
        }
    }
}
