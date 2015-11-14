using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace APEngine.ScreenManagers.SceneObjects
{
    public class Zzz : SceneObject
    {
        public Zzz()
            : base()
        {

        }

        public override void LoadContent(ContentManager Content)
        {
            Animation = new Graphics.Animation(Content.Load<Texture2D>("Data\\GFX\\Scenes\\Zzz"), Position, 14, 17, 2, 150f);
            Animation.Depth = 0.3f;
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
