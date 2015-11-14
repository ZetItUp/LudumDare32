using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace APEngine.ScreenManagers.SceneObjects
{
    public class Moon : SceneObject
    {
        public Moon()
            : base()
        {

        }

        public override void LoadContent(ContentManager Content)
        {
            Texture2D text = Content.Load<Texture2D>("Data\\GFX\\Scenes\\Moon");
            Animation = new Graphics.Animation(text, Position, text.Width, text.Height, 1, 0);
            Animation.Pause = true;
            Animation.Depth = 0.21f;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Animation.Update(gameTime);
            Animation.Position = Position;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible)
                return;

            Animation.Draw(spriteBatch);
        }
    }
}
