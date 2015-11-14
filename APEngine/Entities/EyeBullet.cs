using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace APEngine.Entities
{
    public class EyeBullet : Projectile
    {
        float lifeLength = 1000f;
        float currtick = 0f;

        public EyeBullet(int minDmg, int maxDmg, bool left, int width, int height)
            : base(minDmg, maxDmg, left, width, height)
        {
            Speed = 100f;
            Owner = ProjectileOwner.Alien;
        }

        public override void Update(GameTime gameTime, Map.Level level)
        {
            base.Update(gameTime, level);

            currtick += (float)gameTime.ElapsedGameTime.Milliseconds;
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (currtick > lifeLength)
            {
                DestroyNextFrame = true;
                // Not needed but anyway :P
                currtick -= lifeLength;
            }

            if (Left)
            {
                Position += new Vector2(-Speed * elapsed, 0);
            }
            else
            {
                Position += new Vector2(Speed * elapsed, 0);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
