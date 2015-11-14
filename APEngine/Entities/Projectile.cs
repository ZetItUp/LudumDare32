using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace APEngine.Entities
{
    public enum ProjectileOwner
    {
        None = 0,
        Player,
        Alien
    }

    public class Projectile
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public GameAttribute Damage { get; set; }
        public bool Left { get; set; }
        public float Speed { get; set; }
        public float Velocity { get; set; }
        public Texture2D Texture { get; set; }
        private bool destroyNextFrame;
        public bool DestroyNextFrame 
        {
            get { return destroyNextFrame; }
            set
            {
                if(value == true)
                {
                    // Make it so that the bullet wont deal damage once it's collided.
                    Damage.Subtract(Damage.Value);
                }

                destroyNextFrame = true;
            }
        }
        public ProjectileOwner Owner { get; protected set; }

        public Vector2 Position 
        { 
            get;
            set; 
        }

        public Rectangle Rect 
        {
            get 
            {
                return new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            } 
        }

        public Projectile(int minDmg, int maxDmg, bool left, int width, int height)
        {
            Damage = new GameAttribute(minDmg, maxDmg);
            Left = left;
            Width = width;
            Height = height;
        }

        public virtual void Update(GameTime gameTime, Map.Level level)
        {
            foreach (Map.Tile t in level.tile)
            {
                if (Rect.Intersects(t.Rect) && t.Solid)
                {
                    DestroyNextFrame = true;
                }
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Texture == null)
                return;

            if(Left)
                spriteBatch.Draw(Texture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);
            else
                spriteBatch.Draw(Texture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0.9f);
        }
    }
}
