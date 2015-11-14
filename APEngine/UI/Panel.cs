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
    public class Panel : Window
    {
        Dictionary<string, Window> _windows;

        public Panel(Rectangle windowRect)
            : base(windowRect)
        {
            RasterizerState.ScissorTestEnable = true;
        }

        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);
            _windows = new Dictionary<string, Window>();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach(KeyValuePair<string, Window> w in _windows)
            {
                w.Value.Update(gameTime);
            }
        }

        public void SetActive(string windowID)
        {
            if(!AllowActiveChange)
                return;

            if(!_windows.ContainsKey(windowID))
                return;

            foreach(KeyValuePair<string, Window> w in _windows)
            {
                if(w.Key == windowID)
                    w.Value.Active = true;
                else
                    w.Value.Active = false;
            }
        }

        public void AddWindow(string windowID, UI.Window newWindow)
        {
            if(newWindow.Parent != null)
            {
                newWindow.WindowRectangle = new Rectangle(newWindow.Parent.WindowRectangle.X + newWindow.WindowRectangle.X, newWindow.Parent.WindowRectangle.Y + newWindow.WindowRectangle.Y, newWindow.WindowRectangle.Width, newWindow.WindowRectangle.Height);
            }

            _windows.Add(windowID, newWindow);
        }

        public void ShowWindow(string windowID)
        {
            if (_windows.ContainsKey(windowID))
                _windows[windowID].Visible = true;
        }

        public void HideWindow(string windowID)
        {
            if (_windows.ContainsKey(windowID))
                _windows[windowID].Visible = false;
        }

        public T GetWindow<T>(string windowID) where T : UI.Window
        {
            UI.Window window;

            if (_windows.TryGetValue(windowID, out window))
            {
                return (T)window;
            }
            else
            {
                throw new Exception(string.Format("Window {0} was not found!", windowID));
            }
        }

        public bool RemoveWindow(string windowID)
        {
            if(_windows.ContainsKey(windowID))
            {
                _windows.Remove(windowID);
                return true;
            }

            return false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(!Visible)
                return;

            base.Draw(spriteBatch);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
            Rectangle currentRect = spriteBatch.GraphicsDevice.ScissorRectangle;
            spriteBatch.GraphicsDevice.ScissorRectangle = WindowRectangle;

            // Draw clipped items here
            foreach(KeyValuePair<string, Window> w in _windows.Reverse())
            {
                w.Value.Draw(spriteBatch);
            }

            spriteBatch.GraphicsDevice.ScissorRectangle = currentRect;
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Engine.Monitor.GetTransformationMatrix());
        }
    }
}
