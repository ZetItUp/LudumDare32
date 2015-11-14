using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace APEngine.ScreenManagers
{
    public class ScreenManager
    {
        public static ContentManager Content { get; private set; }
        private static Screen currentScreen;

        public static Screen CurrentScreen { get { return currentScreen; } }

        public static GameScreen GameScreen
        {
            get
            {
                return (GameScreen)currentScreen;
            }
        }

        public void SetScreen(Screen newScreen)
        {
            if(currentScreen != null)
            {
                currentScreen.Unload();
                currentScreen = null;
            }

            currentScreen = newScreen;
            currentScreen.LoadContent(Content);
        }

        public void SetScreen(Screen newScreen, string args)
        {
            if (currentScreen != null)
            {
                currentScreen.Unload();
                currentScreen = null;
            }

            currentScreen = newScreen;
            currentScreen.LoadContent(Content, args);
        }

        public ScreenManager()
        {

        }

        public void LoadContent(ContentManager Content)
        {
            ScreenManager.Content = Content;
        }

        public void Update(GameTime gameTime)
        {
            if (currentScreen == null)
                return;

            currentScreen.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (currentScreen == null)
                return;

            currentScreen.Draw(spriteBatch);
        }
    }
}
