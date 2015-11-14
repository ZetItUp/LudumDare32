using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using AtriLib2;

namespace APEngine.UI
{
    public class GameObjectButton : Window
    {
        public event EventHandler Clicked;

        private GameObjects.GameObject gameObject;
        public GameObjects.GameObject GameObject 
        { 
            get
            {
                return gameObject;
            }
            set
            {
                gameObject = value;

                
            }
        }

        public GameObjectButton(Rectangle windowRectangle)
            : base(windowRectangle)
        {
            DrawWindow = false;
        }

        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);

            if (GameObject != null)
            {
                if (Parent == null)
                    GameObject.Position = new Vector2(WindowRectangle.X + WindowRectangle.Width / 2, WindowRectangle.Y + WindowRectangle.Height / 2);
                else
                    GameObject.Position = new Vector2(Parent.WindowRectangle.X + WindowRectangle.X, Parent.WindowRectangle.Y + WindowRectangle.Y);
            }

            Clicked += GameObjectButton_Clicked;
        }

        void GameObjectButton_Clicked(object sender, EventArgs e)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;
            Rectangle mmouseRect = new Rectangle((int)(AMouse.MousePosition().X / gs.camera.Zoom), (int)(AMouse.MousePosition().Y / gs.camera.Zoom), 1, 1);
            Rectangle buttonRect = new Rectangle((int)(WindowRectangle.X / gs.camera.Zoom), (int)(WindowRectangle.Y / gs.camera.Zoom), (int)(WindowRectangle.Width / gs.camera.Zoom), (int)(WindowRectangle.Height / gs.camera.Zoom));

            if (mmouseRect.Intersects(buttonRect))
            {
                if (AMouse.MousePressed(AMouse.MouseButton.Left))
                {
                    if (Clicked != null)
                    {
                        Clicked(this, EventArgs.Empty);
                    }
                }
            }

            if (GameObject != null)
            {
                GameObject.Update(gameTime);
                GameObject.AllowedToDrawDebug = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (!Visible)
                return;

            if(gameObject != null)
                gameObject.Draw(spriteBatch);
        }
    }
}
