using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using AtriLib2;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace APEngine.UI
{
    public enum FontColor
    {
        Blue = 0,
        Yellow = 1
    };

    public class GameFont
    {
        public float MaxWidth { get; set; }
        public bool Visisble { get; set; }
        public float Alpha { get; set; }
        public bool MultiLine { get; set; }
        public bool CenterText { get; set; }
        private Vector2 textPos { get; set; }
        private Texture2D texture;

        private string text;
        public string Text
        {
            get { return text; }
            set
            {
                if(value.Length > MaxWidth && MultiLine)
                {
                    string newVal = "";
                    int s = 0;
                    bool lineAtNext = false;

                    for(int i = 0; i < value.Length; i++)
                    {
                        if(value.Contains(' '))
                        {
                            if(lineAtNext && value[i] == ' ')
                            {
                                lineAtNext = false;
                                s = 0;
                                newVal += '\n';
                                continue;
                            }
                        }
                        else
                        {
                            if(lineAtNext)
                            {
                                lineAtNext = false;
                                s = 0;
                                newVal += '\n';
                            }
                        }

                        s++;

                        if(s % 10 == 0)
                        {
                            lineAtNext = true;
                        }

                        newVal += value[i];
                    }

                    value = newVal;
                }

                if(MultiLine)
                    text = WrapText(value, MaxWidth);
                else
                    text = value;
            }
        }

        public Rectangle WindowRectangle { get; set; }

        public GameFont(Rectangle wndRect, float maxWidth)
        {
            WindowRectangle = wndRect;
            Visisble = true;
            textPos = Vector2.Zero;
            MaxWidth = maxWidth;
            Alpha = 1.0f;
            MultiLine = false;
        }

        public void LoadContent(ContentManager Content)
        {
            Text = "";

            texture = Content.Load<Texture2D>("Data\\GFX\\GameFont");
            if (!FontManager.IsInitialized)
                FontManager.LoadContent();
        }

        public void LoadContent(ContentManager Content, FontColor color)
        {
            Text = "";

            string fontColor = "GameFont";

            if (color == FontColor.Yellow)
                fontColor += "Yellow";

            texture = Content.Load<Texture2D>("Data\\GFX\\" + fontColor);
            if (!FontManager.IsInitialized)
                FontManager.LoadContent();
        }

        public void Update(GameTime gameTime)
        {
            if(CenterText)
            {
                textPos = new Vector2(WindowRectangle.X + WindowRectangle.Width / 2 - (((int)FontManager.GetStringSize(Text).X / 2)), WindowRectangle.Y);
            }
            else
            {
                textPos = new Vector2(WindowRectangle.X, WindowRectangle.Y);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!Visisble)
                return;

            List<Rectangle> textRects = FontManager.GetRectangles(Text);

            int x = (int)textPos.X;
            int y = (int)textPos.Y;

            for(int i = 0; i < Text.Length; i++)
            {
                if(Text[i] == '\n')
                {
                    y += (int)FontManager.GetCharHeight;
                    x = WindowRectangle.X;
                    continue;
                }

                spriteBatch.Draw(texture, new Vector2(x, y), textRects[i], Color.White * Alpha);
                x += textRects[i].Width;
            }
        }

        public void Draw(SpriteBatch spriteBatch, float scale)
        {
            if (!Visisble)
                return;

            List<Rectangle> textRects = FontManager.GetRectangles(Text);

            float x = (int)textPos.X;
            float y = (int)textPos.Y;

            for (int i = 0; i < Text.Length; i++)
            {
                if (Text[i] == '\n' && MultiLine)
                {
                    y += (int)FontManager.GetCharHeight * scale;
                    x = WindowRectangle.X;
                    continue;
                }

                spriteBatch.Draw(texture, new Vector2(x, y), textRects[i], Color.White * Alpha, 0, Vector2.Zero, scale, SpriteEffects.None, 1f);
                x += textRects[i].Width * scale;
            }
        }

        private string[] SplitStringAtLine(string str)
        {
            string[] result = Regex.Split(str, "[\r\n]+");

            return result;
        }

        public string WrapText(string text, float maxLineWidth)
        {
            if (text == "")
                return "";

            StringBuilder sb = new StringBuilder();
            float lineWidth = 0f;
            float spaceWidth = FontManager.GetCharWidth(' ');

            string[] lines = SplitStringAtLine(text);

            foreach(string line in lines)
            {
                Vector2 size = FontManager.GetStringSize(line);

                if(size.X > maxLineWidth)
                {
                    string[] word = line.Split(' ');

                    foreach(string w in word)
                    {
                        Vector2 sw = FontManager.GetStringSize(w);

                        if(sw.X + lineWidth < maxLineWidth)
                        {
                            sb.Append(w + " ");
                            lineWidth += sw.X + spaceWidth;
                        }
                        else
                        {
                            sb.Append("\n" + w + " ");
                            lineWidth = sw.X + spaceWidth;
                        }
                    }
                }
                else
                {
                    sb.Append(line + "\n");
                }
            }

            return sb.ToString();
        }
    }
}
