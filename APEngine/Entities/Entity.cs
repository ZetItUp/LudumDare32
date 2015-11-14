using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using AtriLib2;

namespace APEngine.Entities
{
    public enum EntityState
    {
        Idle = 0,
        Walking,
        Shooting,
        Jumping,
        Falling,
        Dead
    };

    public class Entity
    {
        public static int WIDTH = 32;
        public static int HEIGHT = 32;
        public static float GRAVITY = 0.005f;
        public static float BOUNCE_SPEED = -1.8f;

        public static float BaseSpeed = 0.15f;
        public static float BaseJumpSpeed = -1.2f;
        public float Friction { get; set; }

        public bool HasWorldCollision { get; set; }

        public Vector2 Movement { get; set; }
        public float Speed { get; set; }
        public float JumpSpeed { get; set; }
        public bool Jumping { get; set; }
        public bool HasGravity { get; set; }
        SoundEffectInstance sfxInstance;

        private Vector2 startPosition { get; set; }
        private Vector2 oldPosition;
        private Vector2 pos;
        public GameAttribute Health { get; set; }
        public EntityState State { get; set; }
        public Abilities.Abilities Ability { get; set; }
        public ProjectileOwner Owner { get; set; }

        protected bool CanDoubleJump;
        private bool currOnGround;
        private bool prevOnGround;
        public ATimer airTime;
        private float airFactor = 0.01f;
        public float timeInAir = 0.0f;
        public bool HasReachedTop = false;

        public virtual Vector2 Position 
        {
            get { return pos; }
            set
            {
                pos = value;

                // Start position has NOT been set!
                if(startPosition == Vector2.Zero)
                {
                    startPosition = pos;
                }
            }
        }
        public virtual Rectangle CollisionRectangle { get; set; }
        public Color DrawColor { get; set; }

        public Entity(int maxHealth)
        {
            airTime = new ATimer();
            airTime.Interval = 100f;
            airTime.TimerType = ATimerType.MilliSeconds;
            airTime.Stop();
            Owner = ProjectileOwner.None;

            pos = Vector2.Zero;
            startPosition = Vector2.Zero;

            DrawColor = Color.White;
            CollisionRectangle = new Rectangle(0, 0, WIDTH, HEIGHT);
            Health = new GameAttribute(0, maxHealth);
            Position = Vector2.Zero;
            Friction = 0.2f;
            HasGravity = true;
            HasWorldCollision = true;
        }

        public Entity(Vector2 position, int maxHealth)
        {
            airTime = new ATimer();
            airTime.Interval = 100f;
            airTime.TimerType = ATimerType.MilliSeconds;
            airTime.Stop();

            startPosition = Vector2.Zero;
            DrawColor = Color.White;
            CollisionRectangle = new Rectangle(0, 0, WIDTH, HEIGHT);
            Health = new GameAttribute(0, maxHealth);
            Position = position;
            Friction = 0.2f;
            HasGravity = true;
            HasWorldCollision = true;
        }

        public virtual void TakeDamage(int amount)
        {
            Health.Subtract(amount);
        }

        public virtual void LoadContent(ContentManager Content)
        {
            Speed = BaseSpeed;
            JumpSpeed = BaseJumpSpeed;
            Jumping = false;
            CanDoubleJump = false;
        }

        public void StartAirTime()
        {
            airTime.Start();
        }

        public void StopAirTime()
        {
            airTime.Stop();
        }

        public void MoveToStart()
        {
            Position = startPosition;
        }

        public virtual void Jump()
        {
            if (IsOnGround())
            {
                JumpSpeed = BaseJumpSpeed;
                ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;
                sfxInstance = SoundManager.SoundManager.Jump.CreateInstance();
                gs.SoundManager.PlaySoundEffect(sfxInstance);
                Jumping = true;
                CanDoubleJump = true;

                State = EntityState.Jumping;
            }

            if (Ability.HasDoubleJump)
            {
                // We can double jump

                if (!IsOnGround() && Jumping)
                {
                    if (CanDoubleJump)
                    {
                        ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;
                        sfxInstance = SoundManager.SoundManager.Jump.CreateInstance();
                        gs.SoundManager.PlaySoundEffect(sfxInstance);
                        Jumping = true;
                        CanDoubleJump = false;
                        JumpSpeed = BaseJumpSpeed;

                        State = EntityState.Jumping;
                    }
                }
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            prevOnGround = currOnGround;
            currOnGround = IsOnGround();

            if(WasOnGround())
            {
                StartAirTime();
            }

            if (!IsOnGround())
            {
                if (HasGravity && HasWorldCollision)
                {
                    if (JumpSpeed < 1.0f)
                        JumpSpeed += GRAVITY * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                    Movement += new Vector2(0, JumpSpeed);
                }
            }
            
            if(IsOnGround() && Jumping && JumpSpeed > 0)
            {
                JumpSpeed = 0f;
                Jumping = false;
            }

            if(HitTileWithTop() && JumpSpeed < 0)
            {
                JumpSpeed = 0;
            }

            if(Landed())
            {
                StopAirTime();
                airFactor = 0f;
                timeInAir = 0.0f;
                Jumping = false;

                if (this is Entities.Player)
                {
                    ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;
                    // Check Tile Type
                    Vector2 realPos = new Vector2(Position.X, Position.Y + CollisionRectangle.Height);
                    
                    if(realPos.X + CollisionRectangle.Width > gs.level.GetWidthInPixels)
                    {
                        realPos = new Vector2(gs.level.GetWidthInPixels - CollisionRectangle.Width, realPos.Y);
                    }

                    if(realPos.Y + CollisionRectangle.Height > gs.level.GetHeightInPixels)
                    {
                        realPos = new Vector2(realPos.X, gs.level.GetHeightInPixels - CollisionRectangle.Height);
                    }

                    //if (Map.Level.CurrentLevel.GetTileAt(realPos).ID == Map.TileID.ItemCrateObject && Map.Level.CurrentLevel.GetTileAt(realPos).Solid)
                    //{
                    //    if(!gs.EditorMode)
                    //    {
                    //        Vector2 pos = Map.Level.CurrentLevel.GetWorldPosition(realPos);
                    //        Map.Tile t = Map.Level.CurrentLevel.GetTileAt(realPos);
                    //        t.Solid = false;

                    //        Map.Level.CurrentLevel.SetTile((int)pos.X, (int)pos.Y, t);

                    //        List<Map.GameObject.ItemCrate> crates = gs.GetItemBoxesWithinRange(CollisionRectangle);

                    //        foreach(Map.GameObject.ItemCrate ic in crates)
                    //        {
                    //            if(ic.Rect == t.Rect)
                    //            {
                    //                this.Position = new Vector2(this.Position.X, ic.Position.Y - ic.Rect.Height - this.CollisionRectangle.Height / 2);
                    //                ic.BreakBox();
                    //            }
                    //        }
                    //        JumpSpeed = -1.1f;
                    //        Jumping = true;
                    //    }
                    //}

                    //if(Map.Level.CurrentLevel.GetTileAt(realPos).ID == Map.TileID.BounceTile ||
                    //    Map.Level.CurrentLevel.GetTileAt(new Vector2(realPos.X + CollisionRectangle.Width - 1, realPos.Y)).ID == Map.TileID.BounceTile)
                    //{
                    //    Rectangle prect = new Rectangle((int)realPos.X - CollisionRectangle.Width - 1, (int)realPos.Y + 2, CollisionRectangle.Width, CollisionRectangle.Height);
                    //    Map.Tile _t = Map.Level.CurrentLevel.GetTileAt(realPos);
                    //    Rectangle trect = new Rectangle(_t.Rect.X, _t.Rect.Y, _t.Rect.Width + prect.Width, _t.Rect.Height);
                    //    List<Map.GameObject.SpringBounce> _crates = gs.GetSpringBouncers(CollisionRectangle);

                    //    if(prect.Intersects(_t.Rect))
                    //    {
                    //        this.Position = new Vector2(this.Position.X, _t.Position.Y - _t.Rect.Height - this.CollisionRectangle.Height / 2);

                    //        foreach(var sb in _crates)
                    //        {
                    //            if(sb.Position == _t.Position)
                    //                sb.animation.PlayOnce = true;
                    //        }

                    //        JumpSpeed = BOUNCE_SPEED;
                    //        Jumping = true;

                    //        if(Ability.HasDoubleJump)
                    //        {
                    //            CanDoubleJump = true;
                    //        }
                    //    }
                    //}
                }
                else
                {
                    JumpSpeed = 0f;
                }
            }

            if(airTime.DidTick)
            {
                airFactor += 0.05f;
                timeInAir += airTime.Interval;
            }

            airTime.Update(gameTime);

            SimulateFriciton();
            AffectGravity();
            MoveAsFarPossible(gameTime);
            StopMoveIfBlock();
        }

        public bool WasOnGround()
        {
            if (prevOnGround == true && currOnGround == false)
            {
                // We did just jump
                return true;
            }
            else
                return false;
        }

        public bool Landed()
        {
            if (prevOnGround == false && currOnGround == true)
            {
                // We just landed
                return true;
            }
            else
                return false;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;
            if(gs.EditorMode && HasWorldCollision)
                Engine.AtriceGraphics.DrawBorder(spriteBatch, 1, CollisionRectangle, Color.Red);
        }

        public void StopHorizontalMovement()
        {
            Movement = new Vector2(0, Movement.Y);
        }

        private void StopMoveIfBlock()
        {
            Vector2 lastMovement = Position - oldPosition;
            if(lastMovement.X == 0)
            {
                Movement *= Vector2.UnitY;
            }
            if(lastMovement.Y == 0)
            {
                Movement *= Vector2.UnitX;
            }
        }

        public bool IsOnGround()
        {
            Rectangle onePixelLower = CollisionRectangle;
            onePixelLower.Offset(0, 1);

            return !Map.Level.CurrentLevel.HasRoomForRectangle(onePixelLower);
        }

        public bool HitTileWithTop()
        {
            Rectangle onePixelLower = CollisionRectangle;
            onePixelLower.Offset(0, -1);

            return !Map.Level.CurrentLevel.HasRoomForRectangle(onePixelLower);
        }

        public Map.Tile GetTile(Vector2 position)
        {
            return Map.Level.CurrentLevel.GetTileAt(position);
        }

        private void AffectGravity()
        {
            if(HasGravity && HasWorldCollision)
                Movement += new Vector2(0, JumpSpeed);
        }

        private void SimulateFriciton()
        {
            if (IsOnGround())
            {
                Movement -= Movement * Vector2.One * Friction;
            }
            else
            {
                Movement -= Movement * Vector2.One * Friction;
            }
        }

        private void MoveAsFarPossible(GameTime gameTime)
        {
            oldPosition = Position;
            UpdateMovement(gameTime);
            if(HasWorldCollision)
                Position = Map.Level.CurrentLevel.WhereCanIGetTo(oldPosition, Position, CollisionRectangle);
        }

        private void UpdateMovement(GameTime gameTime)
        {
            Position += Movement * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 15;

            ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;

            if (Position.X < 0)
                Position = new Vector2(0, Position.Y);
            if (Position.X > gs.level.Width * Map.Tile.WIDTH - CollisionRectangle.Width)
                Position = new Vector2(gs.level.Width * Map.Tile.WIDTH - CollisionRectangle.Width, Position.Y);
            if(Position.Y < 0)
                Position = new Vector2(Position.X, 0);
            else if(Position.Y > gs.level.Height * Map.Tile.HEIGHT - 2)
                Position = new Vector2(Position.X, gs.level.Height * Map.Tile.HEIGHT - 2);
        }
    }
}
