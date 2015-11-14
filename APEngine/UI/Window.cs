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
    public static class WindowGraphics
    {
        public static Texture2D WindowBasics { get; private set; }
        public static Texture2D CheckedTexture { get; private set; }
        public static Texture2D UncheckedTexture { get; private set; }
        public static Texture2D ButtonTexture { get; private set; }

        public static void LoadContent(ContentManager Content)
        {
            WindowBasics = Content.Load<Texture2D>("Data\\GFX\\Editor\\WindowBasics");
            CheckedTexture = Content.Load<Texture2D>("Data\\GFX\\Editor\\CheckerChecked");
            UncheckedTexture = Content.Load<Texture2D>("Data\\GFX\\Editor\\CheckerUnchecked");
            ButtonTexture = Content.Load<Texture2D>("Data\\GFX\\Editor\\GameButton");
        }
    }

    public class Window
    {
        public static bool AllowActiveChange = true;

        protected RasterizerState RasterizerState { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle WindowRectangle { get; set; }
        public Color BackgroundColor { get; set; }
        public Window Parent { get; set; }
        Texture2D windowTexture;

        public event EventHandler WindowClicked;
        public event EventHandler ReturnStringChanged;

        public bool AllowClose { get; set; }
        public bool Active { get; set; }
        public string ID { get; set; }
        public bool Visible { get; set; }
        public bool DrawWindow { get; set; }
        public bool Initialized
        {
            get;
            private set;
        }

        public bool DestroyMe { get; set; }
        private string _retString;
        public string ReturnString 
        {
            set
            {
                _retString = value;

                if(ReturnStringChanged != null)
                {
                    ReturnStringChanged(this, null);
                }
            }
            get
            {
                return _retString;
            }
        }

        private GameFont _title { get; set; }
        public string Title
        {
            get { return _title.Text; }
            set { _title.Text = value; }
        }

        public static void SetTitle(Dictionary<string, Window> windowList, string windowID, string title)
        {
            if(windowList.ContainsKey(windowID))
            {
                windowList[windowID].Title = title;
            }
        }

        public Window(Rectangle windowRectangle)
        {
            Active = false;
            WindowRectangle = windowRectangle;
            Position = new Vector2(WindowRectangle.X, WindowRectangle.Y);
            _title = new GameFont(new Rectangle(WindowRectangle.X, WindowRectangle.Y + 5, WindowRectangle.Width, (int)FontManager.GetCharHeight), WindowRectangle.Width);
            BackgroundColor = new Color(255, 161, 0);
            RasterizerState = new RasterizerState();
            Visible = true;
            DrawWindow = true;
            Initialized = false;
            AllowClose = true;
        }

        public virtual void LoadContent(ContentManager Content)
        {
            _title.LoadContent(Content);
            Title = "";
            _title.CenterText = true;
            windowTexture = Content.Load<Texture2D>("Data\\GFX\\Editor\\Window");
            Initialized = true;
            
            WindowClicked += Window_WindowClicked;
            ReturnStringChanged += Window_ReturnStringChanged;
        }

        void Window_ReturnStringChanged(object sender, EventArgs e)
        {
            
        }

        public virtual void Update(GameTime gameTime)
        {
            _title.Update(gameTime);

            if(AMouse.MouseRectangle().Intersects(this.WindowRectangle))
            {
                if(AMouse.MousePressed(AMouse.MouseButton.Left))
                {
                    if(WindowClicked != null)
                    {
                        WindowClicked(this, EventArgs.Empty);
                    }
                }
            }
        }

        void Window_WindowClicked(object sender, EventArgs e)
        {
            
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if(!Visible)
                return;

            if(DrawWindow)
            {
                // TOP CORNERS
                spriteBatch.Draw(windowTexture, new Rectangle(WindowRectangle.X, WindowRectangle.Y, 16, 16), new Rectangle(0, 0, 16, 16), BackgroundColor);
                spriteBatch.Draw(windowTexture, new Rectangle(WindowRectangle.X + WindowRectangle.Width - 16, WindowRectangle.Y, 16, 16), new Rectangle(16, 0, 16, 16), BackgroundColor);

                // HORIZONTAL LINES
                for(int x = WindowRectangle.X + 16; x < WindowRectangle.X + WindowRectangle.Width - 16; x++)
                {
                    spriteBatch.Draw(windowTexture, new Rectangle(x, WindowRectangle.Y, 1, 16), new Rectangle(16, 0, 1, 16), BackgroundColor);
                    spriteBatch.Draw(windowTexture, new Rectangle(x, WindowRectangle.Y + WindowRectangle.Height - 16, 1, 16), new Rectangle(16, 16, 1, 16), BackgroundColor);
                }

                // VERTICAL LINES
                for(int y = WindowRectangle.Y + 16; y < WindowRectangle.Y + WindowRectangle.Height - 16; y++)
                {
                    spriteBatch.Draw(windowTexture, new Rectangle(WindowRectangle.X, y, 16, 1), new Rectangle(0, 16, 16, 1), BackgroundColor);
                    spriteBatch.Draw(windowTexture, new Rectangle(WindowRectangle.X + WindowRectangle.Width - 16, y, 16, 1), new Rectangle(16, 16, 16, 1), BackgroundColor);
                }

                // BOTTOM CORNERS
                spriteBatch.Draw(windowTexture, new Rectangle(WindowRectangle.X, WindowRectangle.Y + WindowRectangle.Height - 16, 16, 16), new Rectangle(0, 16, 16, 16), BackgroundColor);
                spriteBatch.Draw(windowTexture, new Rectangle(WindowRectangle.X + WindowRectangle.Width - 16, WindowRectangle.Y + WindowRectangle.Height - 16, 16, 16), new Rectangle(16, 16, 16, 16), BackgroundColor);

                // SURFACE
                spriteBatch.Draw(windowTexture, new Rectangle(WindowRectangle.X + 16, WindowRectangle.Y + 16, WindowRectangle.Width - 32, WindowRectangle.Height - 32), new Rectangle(16, 16, 1, 1), BackgroundColor);
            }

            _title.Draw(spriteBatch, 1f);
        }
    }
}
