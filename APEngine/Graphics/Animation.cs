using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace APEngine.Graphics
{
    public class Animation : IResizable
    {
        public Texture2D Texture { get; set; }
        public int FrameWidth { get; private set; }
        public int FrameHeight { get; private set; }
        public int CurrentFrame { get; private set; }
        public int MaxFrames { get; set; }
        public float Depth { get; set; }
        private int col;
        private bool repeat;
        private bool didPlayOnce = false;
        private bool playOnce;
        public bool Pause { get; set; }
        public bool Visible { get; set; }

        public void SetFrame(int newFrame)
        {
            CurrentFrame = newFrame;
        }

        public bool PlayOnce 
        {
            get { return playOnce; }
            set
            {
                if(value == true)
                {
                    didPlayOnce = false;
                    Pause = false;
                }

                playOnce = value;
            }
        }

        public bool Repeat 
        {
            get { return repeat; }
            set
            {
                if (repeat != value)
                    loopTick = 0f;

                repeat = value;
            }
        }
        private bool countBack { get; set; }

        public bool InProportion { get; set; }
        public bool Resizable { get; set; }
        public float ScaleXY { get; set; }
        public float ScaleX { get; set; }
        public float ScaleY { get; set; }

        public int Column 
        {
            get { return col; }
            set
            {
                if (col != value)
                {
                    col = value;
                    CurrentFrame = 0;
                }
            }
        }
        public float AnimationSpeed { get; set; }
        public Color DrawColor { get; set; }
        public Vector2 Position { get; set; }
        public bool FacingRight { get; set; }

        private float currentTick = 0f;
        private float loopTick = 0f;
        public float TimeBetweenLoops = 0f;

        public Animation(Texture2D animationTexture, Vector2 position, int frameWidth, int frameHeight, int maxFrames, float animationSpeed)
        {
            FacingRight = true;
            Texture = animationTexture;
            FrameWidth = frameWidth;
            FrameHeight = frameHeight;
            ScaleTo(frameWidth, frameHeight);
            CurrentFrame = 0;
            Column = 0;
            MaxFrames = maxFrames;
            AnimationSpeed = animationSpeed;
            DrawColor = Color.White;
            Position = position;
            Depth = 1f;
            ScaleX = 1f;
            ScaleY = 1f;
            ScaleXY = 1f;
            Repeat = true;
            PlayOnce = false;
            Visible = true;
        }

        public void ScaleTo(float width, float height)
        {
            if (width % FrameWidth == 0 && height % FrameHeight == 0)
            {
                // They are in proportion to eachother
                ScaleXY = width / FrameWidth;
                InProportion = true;
            }
            else
            {
                // They are NOT in proportion to eachother
                ScaleX = width / FrameWidth;
                ScaleY = height / FrameHeight;
                InProportion = false;
            }
        }

        public void SetScale(float scaleX, float scaleY)
        {
            float newWidth = FrameWidth * scaleX;
            float newHeight = FrameHeight * scaleY;

            if(newWidth % FrameWidth == 0 && newHeight % FrameHeight == 0)
            {
                InProportion = true;
                ScaleXY = newWidth / FrameWidth;
            }
            else
            {
                InProportion = false;
                ScaleX = newWidth / FrameWidth;
                ScaleY = newHeight / FrameHeight;
            }
        }

        public void ScaleTo(float scaleXY)
        {
            SetScale(scaleXY, scaleXY);
        }

        public void Update(GameTime gameTime)
        {
            if (Pause)
                return;

            currentTick += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if(!Repeat)
                loopTick += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if(currentTick >= AnimationSpeed)
            {
                if (Repeat)
                {
                    CurrentFrame++;

                    if (CurrentFrame > MaxFrames)
                        CurrentFrame = 0;
                }
                else if(PlayOnce && !didPlayOnce)
                {
                    CurrentFrame++;

                    if(CurrentFrame > MaxFrames)
                    {
                        CurrentFrame = 0;
                        didPlayOnce = true;
                        Pause = true;
                        PlayOnce = false;
                    }
                }
                else
                {
                    if (loopTick >= TimeBetweenLoops)
                    {
                        if (countBack)
                        {
                            CurrentFrame--;

                            if (CurrentFrame < 0)
                            {
                                CurrentFrame = 0;
                                countBack = false;

                                loopTick = 0;
                            }
                        }
                        else
                        {
                            CurrentFrame++;

                            if (CurrentFrame > MaxFrames)
                            {
                                CurrentFrame = MaxFrames;
                                countBack = true;
                            }
                        }
                    }
                }

                currentTick -= AnimationSpeed;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible)
                return;

            if (FacingRight)
            {
                if (InProportion)
                {
                    var sourceRect = new Rectangle(CurrentFrame * FrameWidth, Column * FrameHeight, FrameWidth, FrameHeight);
                    Rectangle destRect = new Rectangle();
                    
                    if(ScaleXY != 1.0f)
                    {
                        destRect = new Rectangle((int)Position.X - ((int)(FrameWidth * ScaleXY) / 2), (int)Position.Y - (int)(FrameHeight * ScaleXY) + FrameHeight, (int)(sourceRect.Width * ScaleXY), (int)(sourceRect.Height * ScaleXY));
                    }
                    else
                    {
                        destRect = new Rectangle((int)Position.X, (int)Position.Y, (int)(sourceRect.Width * ScaleXY), (int)(sourceRect.Height * ScaleXY));
                    }

                    spriteBatch.Draw(Texture, destRect, sourceRect, DrawColor, 0f, Vector2.Zero, SpriteEffects.None, Depth);
                }
                else
                {
                    var sourceRect = new Rectangle(CurrentFrame * FrameWidth, Column * FrameHeight, FrameWidth, FrameHeight);
                    Rectangle destRect = new Rectangle();

                    if (ScaleX != 1.0f || ScaleY != 1.0f)
                    {
                        destRect = new Rectangle((int)Position.X - ((int)(FrameWidth * ScaleX) / 2), (int)Position.Y - (int)(FrameHeight * ScaleY) + FrameHeight, (int)(sourceRect.Width * ScaleX), (int)(sourceRect.Height * ScaleY));
                    }
                    else
                    {
                        destRect = new Rectangle((int)Position.X, (int)Position.Y, (int)(sourceRect.Width * ScaleX), (int)(sourceRect.Height * ScaleY));
                    }

                    spriteBatch.Draw(Texture, destRect, sourceRect, DrawColor, 0f, Vector2.Zero, SpriteEffects.None, Depth);
                }
            }
            else
            {
                if (InProportion)
                {
                    var sourceRect = new Rectangle(CurrentFrame * FrameWidth, Column * FrameHeight, FrameWidth, FrameHeight);
                    Rectangle destRect = new Rectangle();

                    if (ScaleXY != 1.0f)
                    {
                        destRect = new Rectangle((int)Position.X - ((int)(FrameWidth * ScaleXY) / 2), (int)Position.Y - (int)(FrameHeight * ScaleXY) + FrameHeight, (int)(sourceRect.Width * ScaleXY), (int)(sourceRect.Height * ScaleXY));
                    }
                    else
                    {
                        destRect = new Rectangle((int)Position.X, (int)Position.Y, (int)(sourceRect.Width * ScaleXY), (int)(sourceRect.Height * ScaleXY));
                    }

                    spriteBatch.Draw(Texture, destRect, sourceRect, DrawColor, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, Depth);
                }
                else
                {
                    var sourceRect = new Rectangle(CurrentFrame * FrameWidth, Column * FrameHeight, FrameWidth, FrameHeight);
                    Rectangle destRect = new Rectangle();

                    if (ScaleX != 1.0f || ScaleY != 1.0f)
                    {
                        destRect = new Rectangle((int)Position.X - (int)((sourceRect.Width * ScaleX) / 2), (int)(Position.Y - (int)(sourceRect.Height * ScaleY)), (int)(sourceRect.Width * ScaleX), (int)(sourceRect.Height * ScaleY));
                    }
                    else
                    {
                        destRect = new Rectangle((int)Position.X, (int)Position.Y, (int)(sourceRect.Width * ScaleX), (int)(sourceRect.Height * ScaleY));
                    }

                    spriteBatch.Draw(Texture, destRect, sourceRect, DrawColor, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, Depth);
                }
            }
        }
    }
}
