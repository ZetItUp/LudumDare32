using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using AtriLib2;

namespace APEngine.Entities
{
    public class Mangorra : Entity
    {
        Graphics.Animation animation;
        bool doWalk = false;
        bool walkLeft = false;
        bool ShouldJump = false;
        bool CanChangeDirection = false;

        SoundEffectInstance sfxInstance;

        bool WalkLeft
        {
            get
            {
                return walkLeft;
            }
            set
            {
                if(walkLeft != value)
                {
                    StopHorizontalMovement();
                }

                walkLeft = value;
            }
        }

        public Mangorra(Vector2 position, int maxHealth)
            : base(position, maxHealth)
        {
            State = EntityState.Idle;
            Speed = 0.10f;
            JumpSpeed = 10f;
            CollisionRectangle = new Rectangle((int)Position.X, (int)Position.Y, Entity.WIDTH, Entity.HEIGHT);
            Ability = new Abilities.Abilities();
        }

        public override void Jump()
        {
            base.Jump();
            CanChangeDirection = false;
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
            Texture2D mangor = Content.Load<Texture2D>("Data\\GFX\\Mangorra");
            animation = new Graphics.Animation(mangor, Position, Entity.WIDTH, Entity.HEIGHT, 3, 300);
        }

        public override void Update(GameTime gameTime)
        {
            CollisionRectangle = new Rectangle((int)Position.X, (int)Position.Y, CollisionRectangle.Width, CollisionRectangle.Height);
            base.Update(gameTime);

            if(!doWalk)
            {
                int walk = Engine.Randomizer.Next(0, 200);

                if(walk >= 20 && walk <= 22)
                {
                    doWalk = true;

                    if(CanChangeDirection)
                    {
                        // Block to the left
                        Map.Tile t1 = Map.Level.CurrentLevel.GetTileAt(new Vector2((Position.X / WIDTH) - 1, Position.Y / HEIGHT + 1));

                        // Block to the Right
                        Map.Tile t2 = Map.Level.CurrentLevel.GetTileAt(new Vector2((Position.X / WIDTH) + 1, Position.Y / HEIGHT + 1));

                        if(t1.Solid == true && t2.Solid == true)
                        {
                            // Free to choose any direction..
                            WalkLeft = ATools.IntToBool(Engine.Randomizer.Next(0, 1 + 1));
                        }
                        else if(t1.Solid == false && t2.Solid == true)
                        {
                            WalkLeft = false;
                        }
                        else if(t1.Solid == true && t2.Solid == false)
                        {
                            WalkLeft = true;
                        }
                    }
                }
            }
            else
            {
                State = EntityState.Walking;

                if (IsOnGround())
                {
                    if(!walkLeft)
                    {
                        Movement += new Vector2(Speed, 0);

                        // Block to the right and 1 down
                        Map.Tile t2 = Map.Level.CurrentLevel.GetTileAt(new Vector2((Position.X) + 32, (Position.Y) + 32));

                        if(t2.Solid == false)
                            WalkLeft = true;
                    }
                    else
                    {
                        Movement += new Vector2(-Speed, 0);

                        // Block to the left and 1 down
                        Map.Tile t2 = Map.Level.CurrentLevel.GetTileAt(new Vector2((Position.X), (Position.Y) + 32));

                        if(t2.Solid == false)
                            WalkLeft = false;
                    }

                    int walk = Engine.Randomizer.Next(0, 200);
                    int j = Engine.Randomizer.Next(0, 100);
                    CanChangeDirection = true;

                    // Block to the left
                    Map.Tile tA = Map.Level.CurrentLevel.GetTileAt(new Vector2((Position.X / WIDTH) - 1, Position.Y / HEIGHT + 1));
                    // Block to the Right
                    Map.Tile tB = Map.Level.CurrentLevel.GetTileAt(new Vector2((Position.X / WIDTH) + 1, Position.Y / HEIGHT + 1));

                    if(tA.Solid && tB.Solid)
                    {
                        ShouldJump = true;
                    }
                    else
                    {
                        ShouldJump = false;
                    }

                    if(j > 10 && j < 20)
                    {
                        if(ShouldJump)
                            Jump();
                    }

                    if (walk >= 20 && walk <= 22)
                    {
                        doWalk = false;
                        StopHorizontalMovement();
                        State = EntityState.Idle;
                    }
                }
            }

            animation.FacingRight = walkLeft;

            if (State == EntityState.Idle)
            {
                animation.Column = 1;
                animation.AnimationSpeed = 200;
            }
            else if (State == EntityState.Jumping)
            {

            }
            else if (State == EntityState.Dead)
            {
                // Since this is not shooting, idle..
                animation.Column = 3;
                animation.AnimationSpeed = 150;
            }
            else if (State == EntityState.Walking)
            {
                animation.Column = 0;
                animation.AnimationSpeed = 150;
            }

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
