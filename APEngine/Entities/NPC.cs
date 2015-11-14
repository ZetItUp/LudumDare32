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
    public class NPC : Entity
    {
        public Graphics.Animation animation;
        public bool HasGossip { get; set; }

        private ATimer idleLoopTimer;
        Texture2D gossipTexture;
        Color gossipColor = Color.White;

        public NPC(Vector2 pos, int maxHealth)
            : base(pos, maxHealth)
        {
            Speed = 0.50f;
            Ability = new Abilities.Abilities();
            Owner = ProjectileOwner.Player;
            HasGossip = false;
        }

        public override void LoadContent(ContentManager Content)
        {
            Texture2D player = Content.Load<Texture2D>("Data\\GFX\\NPCTemp");
            gossipTexture = Content.Load<Texture2D>("Data\\GFX\\GossipBubble");
            animation = new Graphics.Animation(player, Position, 32, 48, 7, 75);
            animation.Depth = 0.89f;
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

        public override void Update(GameTime gameTime)
        {
            float currTick = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (IsOnGround() && (Movement.X >= -0.02f && Movement.X <= 0.02f))
                State = EntityState.Idle;
            else if (IsOnGround() && Movement.X > 0.1f)
            {
                State = EntityState.Walking;
                animation.FacingRight = true;
            }
            else if(IsOnGround() && Movement.X < -0.1f)
            {
                State = EntityState.Walking;
                animation.FacingRight = false;
            }

            CollisionRectangle = new Rectangle((int)Position.X, (int)Position.Y, 32, CollisionRectangle.Height);

            ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;
            Player p = gs.Player;

            if(p.CollisionRectangle.Intersects(CollisionRectangle))
            {
                gossipColor = new Color(157, 224, 0);
            }
            else
            {
                gossipColor = Color.White;
            }

            base.Update(gameTime);
            animation.Position = Position;

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
                animation.MaxFrames = 7;
                animation.Repeat = true;
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

        public override void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch);

            if(HasGossip)
                spriteBatch.Draw(gossipTexture, new Rectangle((int)(animation.Position.X + animation.FrameWidth / 2 - gossipTexture.Width / 2), (int)(animation.Position.Y - gossipTexture.Height - 3), gossipTexture.Width, gossipTexture.Height), null, gossipColor, 0f, Vector2.Zero, SpriteEffects.None, animation.Depth);

            base.Draw(spriteBatch);
        }
    }
}
