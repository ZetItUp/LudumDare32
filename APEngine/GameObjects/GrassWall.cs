using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace APEngine.GameObjects
{
    public enum GrassWallStyle
    {
        Long = 0,
        Short
    };

    public class GrassWall : GameObject
    {
        Graphics.Animation animation;
        public GrassWallStyle Style { get; set; }

        public GrassWall(Vector2 position)
            : base(position)
        {
            Rect = new Rectangle((int)Position.X, (int)Position.Y, DEFAULT_WIDTH, DEFAULT_HEIGHT);
        }

        public override void LoadContent(ContentManager Content)
        {
            if (Style == GrassWallStyle.Long)
            {
                animation = new Graphics.Animation(Content.Load<Texture2D>("Data\\GFX\\GrassWall"), Position, DEFAULT_WIDTH, 64, 0, 0);
            }
            else if(Style == GrassWallStyle.Short)
            {
                animation = new Graphics.Animation(Content.Load<Texture2D>("Data\\GFX\\GrassWallShort"), Position, DEFAULT_WIDTH, 64, 0, 0);
            }
            animation.Depth = 0.18f;
        }

        public override void Update(GameTime gameTime)
        {
            Rect = new Rectangle((int)Position.X, (int)Position.Y, DEFAULT_WIDTH, 64);
            animation.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch);
        }
    }
}
