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
    public class TileButton : Window
    {
        public event EventHandler Clicked;

        private Map.Tile _tile;
        public Map.Tile Tile 
        {
            get { return _tile; }
            set
            {
                _tile = value;
            }
        }

        public TileButton(Rectangle windowRectangle)
            : base(windowRectangle)
        {
            DrawWindow = false;
        }

        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);

            if (Parent == null)
                Tile.Position = new Vector2(WindowRectangle.X + Tile.Texture.Width / 2, WindowRectangle.Y + Tile.Texture.Height / 2);
            else
                Tile.Position = new Vector2(Parent.WindowRectangle.X + WindowRectangle.X, Parent.WindowRectangle.Y + WindowRectangle.Y);

            Clicked += TileButton_Clicked;
        }

        void TileButton_Clicked(object sender, EventArgs e)
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
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (!Visible)
                return;

            Tile.Draw(spriteBatch);
        }
    }
}
