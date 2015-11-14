using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace APEngine.GameObjects
{
    public class ExitPortal : GameObject
    {
        public Graphics.Animation animation { get; private set; }

        public ExitPortal(Vector2 position)
            : base(position)
        {

        }

        public override void LoadContent(ContentManager Content)
        {
            Texture2D texture = Content.Load<Texture2D>("Data\\GFX\\ExitPortal");

            Position = new Vector2(Position.X, Position.Y - (texture.Height - 32));
            animation = new Graphics.Animation(texture, Position, texture.Width, texture.Height, 0, 0);
            Rect = new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height);
            animation.Depth = 0.59f;
        }

        public override void Update(GameTime gameTime)
        {
            animation.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch);
        }
    }
}
