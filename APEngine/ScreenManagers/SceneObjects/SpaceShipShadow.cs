using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace APEngine.ScreenManagers.SceneObjects
{
    public class SpaceShipShadow : SceneObject
    {
        public SpaceShipShadow()
            : base()
        {

        }

        public override void LoadContent(ContentManager Content)
        {
            Animation = new Graphics.Animation(Content.Load<Texture2D>("Data\\GFX\\Scenes\\SpaceShipShadow"), Position, 129, 91, 1, 0);
            Animation.Depth = 0.35f;
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
