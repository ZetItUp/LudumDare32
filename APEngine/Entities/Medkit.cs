using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace APEngine.Entities
{
    public class Medkit
    {
        public Rectangle Rect { get; set; }
        public int Health { get; set; }
        public Texture2D Texture { get; set; }

        public Medkit(Rectangle rect, Texture2D texture)
        {
            Rect = rect;
            Texture = texture;
            Health = 15;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, new Vector2(Rect.X, Rect.Y), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);
        }
    }
}
