using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AtriLib2;

namespace APEngine.GameObjects
{
    public class FlameBowl : GameObject
    {
        ATimer burstTimer;
        Graphics.Animation burstGraphics;
        Graphics.Animation idleGraphics;
        Graphics.Animation bowlTop;
        Graphics.Animation bowlBottom;

        public FlameBowl(Vector2 position)
            : base(position)
        {
            burstTimer = new ATimer();
            burstTimer.TimerType = ATimerType.MilliSeconds;
            burstTimer.Interval = 3000;
            burstTimer.Start();

            Rect = new Rectangle((int)Position.X, (int)Position.Y, DEFAULT_WIDTH, DEFAULT_HEIGHT);
        }

        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);

            Texture2D flameBurst = Content.Load<Texture2D>("Data\\GFX\\Forest\\FlameBowlFireBurst");
            Texture2D flameTop = Content.Load<Texture2D>("Data\\GFX\\Forest\\FlameBowlTop");
            Texture2D flameBottom = Content.Load<Texture2D>("Data\\GFX\\Forest\\FlameBowlBottom");
            Texture2D flameIdle = Content.Load<Texture2D>("Data\\GFX\\Forest\\FlameBowlFireIdle");
            burstGraphics = new Graphics.Animation(flameBurst, new Vector2(Position.X + 16 - 10, Position.Y - flameBurst.Height + 1), 20, flameBurst.Height, 2, 150);
            idleGraphics = new Graphics.Animation(flameIdle, new Vector2(Position.X, Position.Y - 7), 32, flameIdle.Height, 2, 150);
            bowlTop = new Graphics.Animation(flameTop, new Vector2(Position.X, Position.Y - flameTop.Height), 32, flameTop.Height, 0, 0);
            bowlBottom = new Graphics.Animation(flameBottom, Position, flameBottom.Width, flameBottom.Height, 0, 0);

            bowlTop.Visible = true;
            bowlBottom.Visible = true;
            idleGraphics.Visible = true;
            burstGraphics.Visible = false;

            bowlBottom.Depth = 0.75f;
            bowlTop.Depth = 0.75f;
            idleGraphics.Depth = 0.751f;
            burstGraphics.Depth = 0.751f;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            burstTimer.Update(gameTime);

            bowlBottom.Update(gameTime);
            bowlTop.Update(gameTime);
            idleGraphics.Update(gameTime);
            burstGraphics.Update(gameTime);

            if(burstTimer.DidTick)
            {
                if(idleGraphics.Visible)
                {
                    burstGraphics.SetFrame(0);
                    burstGraphics.Visible = true;
                    idleGraphics.Visible = false;
                }
                else
                {
                    idleGraphics.SetFrame(0);
                    idleGraphics.Visible = true;
                    burstGraphics.Visible = false;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            bowlBottom.Draw(spriteBatch);
            bowlTop.Draw(spriteBatch);
            idleGraphics.Draw(spriteBatch);
            burstGraphics.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
    }
}

