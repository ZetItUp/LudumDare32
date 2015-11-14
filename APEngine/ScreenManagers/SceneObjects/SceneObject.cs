using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace APEngine.ScreenManagers.SceneObjects
{
    public class SceneObject
    {
        public int ID { get; set; }
        protected Graphics.Animation Animation;
        public Vector2 Position { get; set; }
        public bool Visible { get; set; }

        public Vector2 Destination { get; set; }
        public float Speed { get; set; }
        public int ObjectID { get; set; }

        Vector2 dir = Vector2.Zero;

        public bool FacingRight 
        { 
            get
            {
                return Animation.FacingRight;
            }
            set
            {
                Animation.FacingRight = value;
            }
        }

        public void SetFrame(int newFrame)
        {
            Animation.SetFrame(newFrame);
        }

        public bool DoMove { get; set; }

        public SceneObject()
        {
            Visible = true;
            DoMove = false;
            ID = -1;
        }

        public virtual void LoadContent(ContentManager Content)
        {

        }

        public virtual void Update(GameTime gameTime)
        {
            if(DoMove)
            {
                if (Position.X < Destination.X)
                    FacingRight = false;
                else if (Position.X > Destination.X)
                    FacingRight = true;

                if (Position.Y < Destination.Y + Speed + 1f &&
                Position.Y > Destination.Y - Speed - 1f &&
                Position.X < Destination.X + Speed + 1f &&
                Position.X > Destination.X - Speed - 1f)
                {
                    DoMove = false;
                }

                dir.X = Destination.X - Position.X;
                dir.Y = Destination.Y - Position.Y;
                dir.Normalize();

                float direction = (float)Math.Atan2(dir.Y, dir.X);

                dir *= Speed;
                Vector2 pos = Position;
                pos.X += dir.X * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                pos.Y += dir.Y * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                Position = pos;
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible)
                return;
        }
    }
}
