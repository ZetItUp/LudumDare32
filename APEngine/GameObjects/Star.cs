using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace APEngine.GameObjects
{
    public class Star : GameObject
    {
        Graphics.Animation animation;

        public Star(Vector2 position)
            : base(position)
        {
            Rect = new Rectangle((int)Position.X, (int)Position.Y, 16, 16);
        }

        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);
            animation = new Graphics.Animation(Content.Load<Texture2D>("Data\\GFX\\Star"), Position, 15, 15, 3, 250);
            //Depth = 0.51f;
            Depth = 1f;
        }

        public override object Clone()
        {
            return base.Clone();
        }

        public override void Update(GameTime gameTime)
        {
            if (InProportion)
            {
                Rect = new Rectangle((int)Position.X, (int)Position.Y, (int)(Rect.Width * ScaleXY), (int)(Rect.Height * ScaleXY));
                animation.ScaleTo(ScaleXY);
            }
            else
            {
                Rect = new Rectangle((int)Position.X, (int)Position.Y, (int)(Rect.Width * ScaleX), (int)(Rect.Height * ScaleY));
                animation.SetScale(ScaleX, ScaleY);
            }

            animation.Position = Position;
            animation.Depth = Depth;
            animation.Update(gameTime);
            
            ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;

            if (gs.EditorMode == false)
            {
                if (gs.Player.CollisionRectangle.Intersects(Rect))
                {
                    gs.RemoveGameObject(this);
                    gs.starPowerTimer.Show();
                    gs.Player.AddStarPower();

                    sfxInstance = SoundManager.SoundManager.StarPickUp.CreateInstance();
                    gs.SoundManager.PlaySoundEffect(sfxInstance);
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }
    }
}
