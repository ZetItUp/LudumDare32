using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace APEngine.ScreenManagers
{
    public class Screen
    {
        public Screen()
        {

        }

        public virtual void LoadContent(ContentManager Content)
        {

        }

        public virtual void LoadContent(ContentManager Content, string args)
        {

        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Engine.Monitor.AGraphicsDevice.Clear(Color.Black);
        }

        public virtual void Unload()
        {

        }
    }
}
