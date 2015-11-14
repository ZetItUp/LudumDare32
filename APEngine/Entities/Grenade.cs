using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace APEngine.Entities
{
    public class Grenade : Projectile
    {
        private float downForce = 0.1f;
        float lifeLength = 1000f;
        float currtick = 0f;

        public Grenade(int minDmg, int maxDmg, bool left, int width, int height)
            : base(minDmg, maxDmg, left, width, height)
        {
            Speed = 0.5f;
            Owner = ProjectileOwner.Player;
        }

        public override void Update(GameTime gameTime, Map.Level level)
        {
            base.Update(gameTime, level);

            currtick += (float)gameTime.ElapsedGameTime.Milliseconds;

            float elapsed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if(currtick > lifeLength)
            {
                DestroyNextFrame = true;
                // Not needed but anyway :P
                currtick -= lifeLength;
            }

            downForce += 0.1f * elapsed;

            if (downForce > 5f)
                downForce = 5f;

            if (Left)
            {
                Position += new Vector2(-Speed * elapsed, 0);

            }
            else
            {
                Position += new Vector2(Speed * elapsed, 0);
            }

            Position += new Vector2(0, downForce);

            foreach (Map.Tile t in level.tile)
            {
                if (Rect.Intersects(t.Rect) && t.Solid)
                {
                    // Falling
                    if (Rect.Y + Rect.Height >= t.Rect.Y &&
                        Rect.Y + Rect.Height <= t.Rect.Y + 5 + downForce)
                    {
                        Position = new Vector2(Position.X, t.Rect.Y - Rect.Height);
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
