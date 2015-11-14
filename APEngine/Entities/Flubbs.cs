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
    public class Flubbs : Entity
    {
        EntityState oldState;
        Graphics.Animation animation;
        SoundEffectInstance sfxInstance;
        float viewDistance = 100f;

        public Flubbs(Vector2 position, int maxHealth)
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
            int t = Engine.Randomizer.Next(0, 3);
            Texture2D etex;
            if (t == 0)
                etex = Content.Load<Texture2D>("Data\\GFX\\FlubbsPurple");
            else if(t == 1)
                etex = Content.Load<Texture2D>("Data\\GFX\\FlubbsRed");
            else
                etex = Content.Load<Texture2D>("Data\\GFX\\FlubbsYellow");

            animation = new Graphics.Animation(etex, Position, Entity.WIDTH, Entity.HEIGHT, 0, 0);
            animation.Depth = 0.791f;
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
