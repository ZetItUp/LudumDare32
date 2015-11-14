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
    public class Alien : Entity
    {
        EntityState oldState;
        Graphics.Animation animation;
        SoundEffectInstance sfxInstance;
        Texture2D bullet;
        ATimer timerSwitch;

        float fireRate = 1200f;
        float currentTick = 0f;
        float viewDistance = 100;
        float shootDistance = 130;

        public Alien(Vector2 position, int maxHealth)
            :base(position, maxHealth)
        {
            timerSwitch = new ATimer();
            timerSwitch.Interval = Engine.Randomizer.Next(4000, 8000);
            timerSwitch.TimerType = ATimerType.MilliSeconds;

            Speed = 0.15f;
            State = EntityState.Idle;
            CollisionRectangle = new Rectangle((int)Position.X, (int)Position.Y, Entity.WIDTH, Entity.HEIGHT);
            Owner = ProjectileOwner.Alien;
        }

        public void Shoot(bool right)
        {
            EyeBullet b = new EyeBullet(10, 15, right, 12, 14);
            b.Texture = bullet;

            if(!right)
            {
                b.Position = new Vector2(Position.X + 32, Position.Y + (CollisionRectangle.Height / 2) - (b.Texture.Height / 2));
            }
            else
            {
                b.Position = new Vector2(Position.X - 5, Position.Y + (CollisionRectangle.Height / 2) - (b.Texture.Height / 2));
            }

            ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;
            gs.AddProjectile(b);
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
            Texture2D etex = Content.Load<Texture2D>("Data\\GFX\\alien");
            bullet = Content.Load<Texture2D>("Data\\GFX\\EyeBullet");
            animation = new Graphics.Animation(etex, Position, Entity.WIDTH, Entity.HEIGHT, 3, 150);
            animation.Depth = 0.80f;
        }

        public override void Update(GameTime gameTime)
        {
            CollisionRectangle = new Rectangle((int)Position.X, (int)Position.Y, CollisionRectangle.Width, CollisionRectangle.Height);
            base.Update(gameTime);

            timerSwitch.Update(gameTime);

            ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;
            List<Projectile> proj = gs.projectiles;
            Player p = gs.Player;

            float currTick = (float)gameTime.ElapsedGameTime.TotalSeconds;
            currentTick += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if(Vector2.Distance(p.Position, Position) < viewDistance)
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

            // We just changed state
            if(oldState != State)
            {
                if (State == EntityState.Idle)
                {
                    timerSwitch.Start();
                }
                else if(State != EntityState.Idle)
                {
                    timerSwitch.Stop();
                }
            }

            if(currentTick > fireRate)
            {
                if(Vector2.Distance(p.Position, Position) < shootDistance)
                {
                    Shoot(animation.FacingRight);
                }
                currentTick -= fireRate;
            }

            if (State == EntityState.Idle)
            {
                animation.Column = 2;
                animation.AnimationSpeed = 400;

                if(timerSwitch.DidTick)
                {
                    animation.FacingRight = !animation.FacingRight;
                }
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
