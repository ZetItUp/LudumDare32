using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace APEngine.GameObjects
{
    public class RoofLamp : GameObject
    {
        public Graphics.Animation animation;
        public Graphics.Animation lightGraphics;

        public RoofLamp(Vector2 position)
            : base(position)
        {
            Rect = new Rectangle((int)Position.X, (int)Position.Y, DEFAULT_WIDTH, DEFAULT_HEIGHT);
        }

        public override void LoadContent(ContentManager Content)
        {
            animation = new Graphics.Animation(Content.Load<Texture2D>("Data\\GFX\\Lamp01"), Position, DEFAULT_WIDTH, DEFAULT_HEIGHT, 2, 650);
            lightGraphics = new Graphics.Animation(Content.Load<Texture2D>("Data\\GFX\\LongLight01"), new Vector2(Position.X - 16, Position.Y + 13), 64, 56, 1, 100);
        }

        public override void Update(GameTime gameTime)
        {
            Rect = new Rectangle((int)Position.X, (int)Position.Y, DEFAULT_WIDTH, DEFAULT_HEIGHT);
            animation.Update(gameTime);
            lightGraphics.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch);
            lightGraphics.Draw(spriteBatch);
        }
    }
}
