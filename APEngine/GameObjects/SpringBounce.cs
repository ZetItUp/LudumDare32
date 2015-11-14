using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace APEngine.GameObjects
{
    public class SpringBounce : GameObject
    {
        public Graphics.Animation animation { get; private set; }

        public SpringBounce(Vector2 position)
            : base(position)
        {
            Rect = new Rectangle((int)Position.X, (int)Position.Y, DEFAULT_WIDTH, DEFAULT_HEIGHT);
        }

        public override void LoadContent(ContentManager Content)
        {
            animation = new Graphics.Animation(Content.Load<Texture2D>("Data\\GFX\\SpringBounce"), Position, DEFAULT_WIDTH, DEFAULT_HEIGHT, 2, 120);
            animation.Repeat = false;
            animation.PlayOnce = false;
            animation.Pause = true;
            animation.Depth = 0.63f;
        }

        public override void Update(GameTime gameTime)
        {
            Rect = new Rectangle((int)Position.X, (int)Position.Y, DEFAULT_WIDTH, DEFAULT_HEIGHT);
            animation.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
    }
}
