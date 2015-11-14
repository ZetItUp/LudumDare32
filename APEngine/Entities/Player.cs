using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using AtriLib2;

namespace APEngine.Entities
{
    public class Player : Entity
    {
        public Graphics.Animation animation;
        public bool AllowMovement { get; set; }

        //float tileDmgTick = 0f;
        //float tileDmgTickRate = 1000f;
        public float FireRate { get; set; }
        private int clip = 10;
        public int Lives { get; set; }

        private ATimer idleLoopTimer;

        public override Vector2 Position
        {
            get
            {
                return base.Position;
            }
            set
            {
                base.Position = value;

                if(animation != null)
                    animation.Position = value;

                CollisionRectangle = new Rectangle((int)Position.X, (int)Position.Y, 16, CollisionRectangle.Height);
            }
        }

        private int starPower;

        public int StarPower
        {
            get { return starPower; }
            private set
            {
                starPower = value;

                if (starPower >= 100)
                {
                    starPower = 0;
                    Lives++;

                    ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;
                    gs.lifeCountTimer.Show();
                }
            }
        }

        public void AddStarPower()
        {
            StarPower++;
        }

        public int Clip
        {
            get { return clip; }
        }

        public Player(Vector2 pos, int maxHealth)
            : base(pos, maxHealth)
        {
            FireRate = 500f;
            Lives = 3;
            Speed = 0.50f;
            StarPower = 0;
            Ability = new Abilities.Abilities();
            Owner = ProjectileOwner.Player;
            AllowMovement = true;
        }

        public override void LoadContent(ContentManager Content)
        {
            Texture2D player = Content.Load<Texture2D>("Data\\GFX\\player");
            animation = new Graphics.Animation(player, Position, 32, 48, 7, 75);
            animation.Depth = 0.9f;
            CollisionRectangle = new Rectangle((int)Position.X, (int)Position.Y, 32, 48);

            idleLoopTimer = new ATimer();
            idleLoopTimer.Interval = 3000;
            idleLoopTimer.Start();
        }

        public bool TouchingGround(Map.Tile[,] tiles)
        {
            foreach(Map.Tile t in tiles)
            {
                if (t.Solid && 
                    Position.Y >= t.Position.Y - HEIGHT &&
                    Position.X + WIDTH > t.Position.X && Position.X < t.Position.X + t.Rect.Width)
                    return true;
            }

            return false;
        }

        public void Shoot()
        {
            if (clip <= 0)
                return;

            bool left = false;

            if(!animation.FacingRight)
                left = true;

            Grenade g = new Grenade(50, 60, left, 12, 12);

            if (animation.FacingRight)
            {
                g.Position = new Vector2(Position.X + 30, Position.Y + 13);
            }
            else
            {
                g.Position = new Vector2(Position.X - 12, Position.Y + 13);
            }
            
            g.Texture = ScreenManagers.ScreenManager.Content.Load<Texture2D>("Data\\GFX\\Grenade");

            ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;
            gs.AddProjectile(g);

            clip--;
        }

        public override void Update(GameTime gameTime)
        {
            ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;

            if(gs.EditorMode)
            {
                UpdateEditor(gameTime);
                HasWorldCollision = false;
            }
            else
            {
                UpdateNormal(gameTime);
                HasWorldCollision = true;
            }
        }

        #region EDITOR UPDATE
        public void UpdateEditor(GameTime gameTime)
        {
            float currTick = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(AllowMovement)
            {
                if(AInput.KeyDown(Keys.Left) || AInput.KeyDown(Keys.A) || AInput.ButtonDown(Buttons.DPadLeft, PlayerIndex.One))
                {
                    animation.FacingRight = false;
                    Movement += new Vector2(-Speed, 0);
                }
                else if(AInput.KeyDown(Keys.Right) || AInput.KeyDown(Keys.D) || AInput.ButtonDown(Buttons.DPadRight, PlayerIndex.One))
                {
                    animation.FacingRight = true;
                    Movement += new Vector2(Speed, 0);
                }

                if(AInput.KeyDown(Keys.Space) || AInput.KeyDown(Keys.W) || AInput.ButtonPressed(Buttons.A, PlayerIndex.One))
                {
                    Movement += new Vector2(0, -Speed);
                }
                else if(AInput.KeyDown(Keys.S))
                {
                    Movement += new Vector2(0, Speed);
                }
            }

            State = EntityState.Idle;

            CollisionRectangle = new Rectangle((int)Position.X, (int)Position.Y, 16, CollisionRectangle.Height);

            base.Update(gameTime);
            animation.Position = new Vector2(Position.X - 8, Position.Y);

            if(State == EntityState.Idle)
            {
                animation.MaxFrames = 5;
                animation.TimeBetweenLoops = 3000f;
                animation.Repeat = false;
                animation.Column = 2;
                animation.AnimationSpeed = 150 + Math.Abs(1 * (float)gameTime.ElapsedGameTime.TotalSeconds) * 150;
            }

            animation.Update(gameTime);
        }
        #endregion


        #region UPDATE NORMAL
        public void UpdateNormal(GameTime gameTime)
        {
            float currTick = (float)gameTime.ElapsedGameTime.TotalSeconds;
            //tileDmgTick += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if(AInput.KeyDown(Keys.Left) || AInput.KeyDown(Keys.A) || AInput.ButtonDown(Buttons.DPadLeft, PlayerIndex.One))
            {
                if(IsOnGround())
                    State = EntityState.Walking;
                else
                    State = EntityState.Falling;

                animation.FacingRight = false;
                Movement += new Vector2(-Speed, 0);
            }
            else if(AInput.KeyDown(Keys.Right) || AInput.KeyDown(Keys.D) || AInput.ButtonDown(Buttons.DPadRight, PlayerIndex.One))
            {
                if(IsOnGround())
                    State = EntityState.Walking;
                else
                    State = EntityState.Falling;

                animation.FacingRight = true;
                Movement += new Vector2(Speed, 0);
            }

            if(AInput.KeyPressed(Keys.V) || AInput.ButtonPressed(Buttons.X, PlayerIndex.One))
            {
                ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;
                gs.ammoTimer.Show();
                Shoot();
            }

            if(AInput.KeyPressed(Keys.C) || AInput.ButtonPressed(Buttons.B, PlayerIndex.One))
            {
                ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;
                gs.ammoTimer.Show();

                clip = 10;
            }

            if(AInput.KeyPressed(Keys.Space) || AInput.ButtonPressed(Buttons.A, PlayerIndex.One))
            {
                Jump();
            }

            if(AInput.PressedKeys().Count() <= 0)
            {
                if(IsOnGround() && (Movement.X >= -0.02f && Movement.X <= 0.02f))
                    State = EntityState.Idle;
                else if(IsOnGround() && Movement.X > 0.1f)
                {
                    State = EntityState.Walking;
                    animation.FacingRight = true;
                }
                else if(IsOnGround() && Movement.X < -0.1f)
                {
                    State = EntityState.Walking;
                    animation.FacingRight = false;
                }
            }

            //bool isBadTile = false;
            //if(tileDmgTick > tileDmgTickRate)
            //{
            //    ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManagers.CurrentScreen;
            //    foreach(Map.Tile t in gs.level.tile)
            //    {
            //        Rectangle playerRect = new Rectangle(CollisionRectangle.X + 8, CollisionRectangle.Y, CollisionRectangle.Width - 16, CollisionRectangle.Height);

            //        if(playerRect.Intersects(t.Rect) && t.Damage)
            //        {
            //            isBadTile = true;
            //        }
            //    }

            //    tileDmgTick -= tileDmgTickRate;
            //}

            //if(isBadTile)
            //{
            //    Health.Subtract(10);
            //}

            CollisionRectangle = new Rectangle((int)Position.X, (int)Position.Y, 16, CollisionRectangle.Height);

            base.Update(gameTime);
            animation.Position = new Vector2(Position.X - 8, Position.Y);
            
            if(State == EntityState.Idle)
            {
                animation.MaxFrames = 5;
                animation.TimeBetweenLoops = 3000f;
                animation.Repeat = false;
                animation.Column = 2;
                animation.AnimationSpeed = 150 + Math.Abs(1 * (float)gameTime.ElapsedGameTime.TotalSeconds) * 150;
            }
            else if(State == EntityState.Jumping)
            {
                animation.MaxFrames = 9;
                animation.Repeat = true;
                animation.AnimationSpeed = 150 + Math.Abs(1 * (float)gameTime.ElapsedGameTime.TotalSeconds) * 150;
                animation.Column = 3;
            }
            else if(State == EntityState.Shooting)
            {
                animation.Repeat = true;
                animation.Column = 1;
                animation.MaxFrames = 7;
                animation.AnimationSpeed = 150;
                animation.TimeBetweenLoops = 0f;
            }
            else if(State == EntityState.Walking)
            {
                animation.MaxFrames = 7;
                animation.Repeat = true;
                animation.Column = 0;
                animation.TimeBetweenLoops = 0f;
                animation.AnimationSpeed = 100 + Math.Abs(Movement.X * (float)gameTime.ElapsedGameTime.TotalSeconds) * 100;
            }

            animation.Update(gameTime);
        }
        #endregion

        public override void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
    }
}
