using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using AtriLib2;

namespace APEngine.Entities
{
    public class Snailien : Entity
    {
        EntityState oldState;
        Graphics.Animation animation;
        SoundEffectInstance sfxInstance;
        float viewDistance = 100f;

        public Snailien(Vector2 position, int maxHealth)
            :base(position, maxHealth)
        {
            Speed = 0.10f;
            State = EntityState.Idle;
            CollisionRectangle = new Rectangle((int)Position.X, (int)Position.Y, Entity.WIDTH, Entity.HEIGHT);
        }

        public override void TakeDamage(int amount)
        {
            base.TakeDamage(amount);

            ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;
            sfxInstance = SoundManager.SoundManager.AlienHurt01.CreateInstance();
            gs.SoundManager.PlaySoundEffect(sfxInstance);
        }

        public override void LoadContent(ContentManager Content)
        {
            Texture2D etex = Content.Load<Texture2D>("Data\\GFX\\Snailien");
            animation = new Graphics.Animation(etex, Position, Entity.WIDTH, Entity.HEIGHT, 3, 150);
            animation.Depth = 0.790f;
        }

        public override void Update(GameTime gameTime)
        {
            CollisionRectangle = new Rectangle((int)Position.X, (int)Position.Y, CollisionRectangle.Width, CollisionRectangle.Height);
            base.Update(gameTime);

            ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;
            List<Projectile> proj = gs.projectiles;
            Player p = gs.Player;

            float currTick = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Vector2.Distance(p.Position, Position) < viewDistance)
            {
                State = EntityState.Walking;
                if ((Position.X - CollisionRectangle.Width / 2 - Movement.X) < (p.Position.X - p.CollisionRectangle.Width / 2 + Movement.X) - 3f)
                {
                    animation.FacingRight = false;
                    Movement += new Vector2(Speed, 0f);
                }
                else if (Position.X - CollisionRectangle.Width / 2 - Movement.X > p.Position.X - p.CollisionRectangle.Width / 2 - Movement.X)
                {
                    animation.FacingRight = true;
                    Movement += new Vector2(-Speed, 0f);
                }
            }
            else
            {
                State = EntityState.Idle;
            }

            if (State == EntityState.Idle)
            {
                animation.Column = 0;
                animation.AnimationSpeed = 250;
            }
            else if (State == EntityState.Jumping)
            {

            }
            else if (State == EntityState.Shooting)
            {
                animation.Column = 1;
                animation.AnimationSpeed = 150;
            }
            else if (State == EntityState.Walking)
            {
                animation.Column = 0;
                animation.AnimationSpeed = 150 + Math.Abs(Movement.X * (float)gameTime.ElapsedGameTime.TotalSeconds) * 150;
            }

            oldState = State;

            animation.Position = Position;
            animation.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
    }
}
