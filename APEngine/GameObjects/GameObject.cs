using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace APEngine.GameObjects
{
    public class GameObject : IResizable, ICloneable
    {
        public static GameObject GetEmptyGameObject(Vector2 position)
        {
            var go = new GameObject(position);
            
            go.LoadContent(Engine.ContentManager);

            return go;
        }

        public static T GetNewGameObject<T>(params object[] args) where T: GameObject
        {
            var obj = (T)Activator.CreateInstance(typeof(T), args);
            obj.LoadContent(Engine.ContentManager);

            return obj;
        }

        public static int DEFAULT_WIDTH = 32;
        public static int DEFAULT_HEIGHT = 32;

        protected Vector2 startPosition { get; set; }

        public bool InProportion { get; set; }
        public bool Resizable { get; set; }
        public float ScaleXY { get; set; }
        public float ScaleX 
        { 
            get; 
            set; 
        }
        public float ScaleY { get; set; }

        private Vector2 pos;
        public Vector2 Position
        {
            get { return pos; }
            set
            {
                pos = value;
                rect = new Rectangle((int)value.X, (int)value.Y, rect.Width, rect.Height);

                // Start position has NOT been set!
                if (startPosition == Vector2.Zero)
                {
                    startPosition = pos;
                }
            }
        }

        public string FileName { get; set; }
        private float depth;

        public float Depth
        {
            get { return depth; }
            set
            {
                depth = value;
            }
        }

        private Rectangle rect;
        public Rectangle Rect 
        {
            get 
            {
                if (Resizable)
                {
                    if(InProportion)
                        return new Rectangle(rect.X, rect.Y, (int)(rect.Width * ScaleXY), (int)(rect.Height * ScaleXY));
                    else
                        return new Rectangle(rect.X, rect.Y, (int)(rect.Width * ScaleX), (int)(rect.Height * ScaleY));
                }
                else
                    return rect;
            }
            set
            {
                rect = value;
            }
        }

        public virtual object Clone()
        {
            startPosition = Position;

            return this.MemberwiseClone();
        }

        public Color DebugColor { get; set; }
        public Color DrawColor { get; set; }
        protected SoundEffectInstance sfxInstance;
        public bool DrawAtAllTimes { get; set; }
        public bool AllowedToDrawDebug { get; set; }
        public bool Animate { get; set; }

        public GameObject(Vector2 pos)
        {
            FileName = "";
            DrawColor = Color.White;
            startPosition = Vector2.Zero;
            Position = pos;
            ScaleXY = 1f;
            ScaleX = 1f;
            ScaleY = 1f;
            Resizable = true;
            Rect = new Rectangle((int)Position.X, (int)Position.Y, DEFAULT_WIDTH, DEFAULT_HEIGHT);
            DrawAtAllTimes = false;
            AllowedToDrawDebug = true;
            Animate = false;
            DebugColor = GameSettings.DEFAULT_DEBUG_COLOR;
        }

        public virtual void LoadContent(ContentManager Content)
        {
            
        }

        public string GetGameObjectData()
        {
            return "[T=\"GAMEOBJECT\", XPos=\"" + (int)Position.X + "\", YPos=\"" + (int)Position.Y + "\", FileName=\"" +  FileName + "\"]";
        }

        public void MoveToStart()
        {
            Position = startPosition;
        }

        public void ScaleTo(float width, float height)
        {
            if(width % Rect.Width == 0 && height % Rect.Height == 0)
            {
                // They are in proportion to eachother
                ScaleXY = width / Rect.Width;
                InProportion = true;
            }
            else
            {
                // They are NOT in proportion to eachother
                ScaleX = width / Rect.Width;
                ScaleY = height / Rect.Height;
                InProportion = false;
            }
        }

        public void SetScale(float scaleX, float scaleY)
        {
            float newWidth = Rect.Width * scaleX;
            float newHeight = Rect.Height * scaleY;

            if (newWidth % Rect.Width == 0 && newHeight % Rect.Height == 0)
            {
                InProportion = true;
                ScaleXY = newWidth / Rect.Width;
            }
            else
            {
                InProportion = false;
                ScaleX = newWidth / Rect.Width;
                ScaleY = newHeight / Rect.Height;
            }
        }

        public void ScaleTo(float scaleXY)
        {
            SetScale(scaleXY, scaleXY);
        }

        public virtual void Update(GameTime gameTime)
        {
            Rect = new Rectangle((int)Position.X, (int)Position.Y, Rect.Width, Rect.Height);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;

            if(gs.EditorMode && AllowedToDrawDebug)
                Engine.AtriceGraphics.DrawBorder(spriteBatch, 1, Rect, DebugColor);
        }
    }
}
