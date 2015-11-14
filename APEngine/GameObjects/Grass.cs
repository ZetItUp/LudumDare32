using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace APEngine.GameObjects
{
    public enum GrassStyle
    {
        One = 0,
        Two,
        Three,
        Four,
        Five
    };

    public class Grass : GameObject
    {
        Graphics.Animation animation;
        public GrassStyle Style { get; set; }

        public Grass(Vector2 position)
            : base(position)
        {
            Style = GrassStyle.One;
            Rect = new Rectangle((int)Position.X, (int)Position.Y, DEFAULT_WIDTH, DEFAULT_HEIGHT);
        }

        public override void LoadContent(ContentManager Content)
        {
            int val = 1;

            if (Style == GrassStyle.One)
                val = 1;
            else if (Style == GrassStyle.Two)
                val = 2;
            else if (Style == GrassStyle.Three)
                val = 3;
            else if (Style == GrassStyle.Four)
                val = 4;
            else
                val = 5;

            animation = new Graphics.Animation(Content.Load<Texture2D>("Data\\GFX\\Grass0" + val), Position, DEFAULT_WIDTH, DEFAULT_HEIGHT, 0, 0);
            animation.Depth = 0.42f;
        }

        public override void Update(GameTime gameTime)
        {
            Rect = new Rectangle((int)Position.X, (int)Position.Y, DEFAULT_WIDTH, DEFAULT_HEIGHT);
            animation.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch);
        }
    }
}
