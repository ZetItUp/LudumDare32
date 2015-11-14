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
    public class Squidien : Entity
    {
        EntityState oldState;
        Graphics.Animation animation;
        SoundEffectInstance sfxInstance;
        float viewDistance = 125f;

        Vector2 oldPos;
        float idleMoveTimer = 0f;
        float idleMoveInterval = 3000f;

        public Squidien(Vector2 position, int maxHealth)
            : base(position, maxHealth)
        {
            Speed = 0.10f;
            State = EntityState.Idle;
            CollisionRectangle = new Rectangle((int)Position.X, (int)Position.Y, 40, 40);
            HasGravity = false;
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
            Texture2D etex = Content.Load<Texture2D>("Data\\GFX\\Squidien");
            animation = new Graphics.Animation(etex, Position, 40, 40, 3, 150);
            animation.Depth = 0.81f;
        }

        public override void Update(GameTime gameTime)
        {
            CollisionRectangle = new Rectangle((int)Position.X, (int)Position.Y, CollisionRectangle.Width, CollisionRectangle.Height);
            base.Update(gameTime);

            ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;
            Player p = gs.Player;

            if(oldPos == Position && Movement == Vector2.Zero)
            {
                idleMoveTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if(idleMoveTimer > idleMoveInterval)
                {
                    float xm = Engine.Randomizer.Next(-10, 10);
                    float ym = Engine.Randomizer.Next(-3, 3);
                    Movement += new Vector2(xm, ym);

                    idleMoveTimer -= idleMoveInterval;
                }
            }

            float currTick = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Vector2.Distance(p.Position, Position) < viewDistance)
            {
                State = EntityState.Walking;
                if ((Position.X + CollisionRectangle.Width / 2 - Movement.X) < (p.Position.X + p.CollisionRectangle.Width / 2 + Movement.X) - 3f)
                {
                    animation.FacingRight = false;
                    Movement += new Vector2(Speed, 0f);
                }
                else if (Position.X + CollisionRectangle.Width / 2 - Movement.X > p.Position.X + p.CollisionRectangle.Width / 2 - Movement.X)
                {
                    animation.FacingRight = true;
                    Movement += new Vector2(-Speed, 0f);
                }
                else
                {
                    animation.FacingRight = p.animation.FacingRight;
                    Movement = new Vector2(0f, Movement.Y);
                }

                if ((Position.Y - CollisionRectangle.Height - Movement.Y) < (p.Position.Y - p.CollisionRectangle.Height + Movement.Y) - 3f)
                {
                    Movement += new Vector2(0f, Speed);
                }
                else if(Position.Y > p.Position.Y)
                {
                    Movement += new Vector2(0f, -Speed);
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

            oldPos = Position;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
    }
}
