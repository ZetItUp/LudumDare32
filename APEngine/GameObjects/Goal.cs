using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace APEngine.GameObjects
{
    public class Goal : GameObject
    {
        Graphics.Animation animation;
        public Texture2D Texture { get; set; }

        public Goal(Vector2 position)
            : base(position)
        {
            Rect = new Rectangle((int)Position.X, (int)Position.Y, DEFAULT_WIDTH, DEFAULT_HEIGHT);
        }

        public override void LoadContent(ContentManager Content)
        {
            animation = new Graphics.Animation(Content.Load<Texture2D>("Data\\GFX\\Goal"), Position, DEFAULT_WIDTH, DEFAULT_HEIGHT, 2, 100);
            animation.Depth = 0.8f;
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
