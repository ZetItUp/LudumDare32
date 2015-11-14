using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using AtriLib2;

namespace APEngine.ScreenManagers
{
    public class MenuOptions : Screen
    {
        static string INFORMATION_TEXT = "CONTROLS\n \n \n" +
            "Move Left\n" +
            "Move Right\n" + 
            "Jump\n" + 
            "Reload\n" +
            "Fire/Action\n" +
            "Show Status\n \n \nCollect stars to drain their energy.\nDraining 100 stars energy will give you\n \n1 extra life!";
        static string INFORMATION_TEXT_KEYS =
            "Left Arrow\nRight Arrow\nSpace\nC\nV\nN";

        List<string> resolutions;

        Texture2D menuBackground;
        Texture2D menuBack;
        Texture2D menuBackSelect;
        Texture2D menuSFX;
        Texture2D menuSFXSelect;
        Texture2D menuMusic;
        Texture2D menuMusicSelect;
        Texture2D menuResolution;
        Texture2D menuResolutionSelect;
        Texture2D menuFullscreen;
        Texture2D menuFullscreenSelect;
        Texture2D menuApply;
        Texture2D menuApplySelect;

        Texture2D aButton;
        Texture2D bButton;
        Texture2D xButton;
        Texture2D yButton;

        bool fullScreen = false;
        int selRes = 0;
        int selected = 0;
        int menuSelectHeight = 0;
        Vector2 MenuPos = Vector2.Zero;

        SoundEffectInstance sfxInstance;
        SoundManager.SoundManager soundManager;

        UI.GameFont fontSFX;
        UI.GameFont fontMusic;
        UI.GameFont fontRes;
        UI.GameFont fontFullscreen;
        UI.GameFont fontInfo;
        UI.GameFont fontInfo2;

        public MenuOptions()
        {
            resolutions = new List<string>();
        }

        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);

            menuBackground = Content.Load<Texture2D>("Data\\GFX\\MainMenuBackground");
            menuBack = Content.Load<Texture2D>("Data\\GFX\\MenuBack");
            menuBackSelect = Content.Load<Texture2D>("Data\\GFX\\MenuBackSelected");
            menuApply = Content.Load<Texture2D>("Data\\GFX\\MenuApply");
            menuApplySelect = Content.Load<Texture2D>("Data\\GFX\\MenuApplySelected");
            menuSFX = Content.Load<Texture2D>("Data\\GFX\\MenuSFXVolume");
            menuSFXSelect = Content.Load<Texture2D>("Data\\GFX\\MenuSFXVolumeSelected");
            menuMusic = Content.Load<Texture2D>("Data\\GFX\\MenuMusicVolume");
            menuMusicSelect = Content.Load<Texture2D>("Data\\GFX\\MenuMusicVolumeSelected");
            menuResolution = Content.Load<Texture2D>("Data\\GFX\\MenuResolution");
            menuResolutionSelect = Content.Load<Texture2D>("Data\\GFX\\MenuResolutionSelected");
            menuFullscreen = Content.Load<Texture2D>("Data\\GFX\\MenuFullscreen");
            menuFullscreenSelect = Content.Load<Texture2D>("Data\\GFX\\MenuFullscreenSelected");

            aButton = Content.Load<Texture2D>("Data\\GFX\\AButtonIcon");
            bButton = Content.Load<Texture2D>("Data\\GFX\\BButtonIcon");
            xButton = Content.Load<Texture2D>("Data\\GFX\\XButtonIcon");
            yButton = Content.Load<Texture2D>("Data\\GFX\\YButtonIcon");

            menuSelectHeight = menuBack.Height;

            SoundManager.SoundManager.LoadContent(Content);
            soundManager = new SoundManager.SoundManager();

            MenuPos = new Vector2(Engine.Monitor.VirtualWidth - (int)(menuBack.Width * 1.5f), 120);

            fontSFX = new UI.GameFont(new Rectangle((int)MenuPos.X + 9, (int)MenuPos.Y + menuSelectHeight + 10, 256, 20), 256);
            fontSFX.LoadContent(Content);
            soundManager.Volume = GameSettings.SFXVol;
            fontSFX.Text = Math.Round((soundManager.Volume * 100f), 0).ToString() + "%";

            fontMusic = new UI.GameFont(new Rectangle((int)MenuPos.X + 9, (int)MenuPos.Y + (menuSelectHeight * 3) + 10, 256, 20), 256);
            fontMusic.LoadContent(Content);
            SoundManager.MusicManager.Volume = GameSettings.MusicVol;
            fontMusic.Text = Math.Round((SoundManager.MusicManager.Volume * 100f), 0).ToString() + "%";

            FillRes();

            fontRes = new UI.GameFont(new Rectangle((int)MenuPos.X + 9, (int)MenuPos.Y + (menuSelectHeight * 5) + 10, 256, 20), 300);
            fontRes.LoadContent(Content);
            fontRes.Text = GetResolution(selRes);

            fontFullscreen = new UI.GameFont(new Rectangle((int)MenuPos.X + 9, (int)MenuPos.Y + (menuSelectHeight * 7) + 10, 256, 20), 300);
            fontFullscreen.LoadContent(Content);
            fontFullscreen.Text = fullScreen.ToString().ToUpper();

            fontInfo = new UI.GameFont(new Rectangle(10, 100, 300, 20), 300);
            fontInfo.LoadContent(Content);
            fontInfo.Text = INFORMATION_TEXT;

            fontInfo2 = new UI.GameFont(new Rectangle(160, 151, 300, 20), 300);
            fontInfo2.LoadContent(Content, UI.FontColor.Yellow);
            fontInfo2.Text = INFORMATION_TEXT_KEYS;

            selRes = GameSettings.GetSelectedResolutionFromResolution(Engine.Monitor.Width, Engine.Monitor.Height);
            fullScreen = false;

            if (GameSettings.Fullscreen == 1)
                fullScreen = true;
        }

        private string GetResolution(int index)
        {
            if (resolutions == null)
                return "";
            else
                return resolutions[index];
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            fontSFX.WindowRectangle = new Rectangle((int)MenuPos.X + 9, (int)MenuPos.Y + menuSelectHeight + 10, 256, 20);
            fontMusic.WindowRectangle = new Rectangle((int)MenuPos.X + 9, (int)MenuPos.Y + (menuSelectHeight * 3) + 10, 256, 20);
            fontRes.WindowRectangle = new Rectangle((int)MenuPos.X + 9, (int)MenuPos.Y + (menuSelectHeight * 5) + 10, 256, 20);
            fontFullscreen.WindowRectangle = new Rectangle((int)MenuPos.X + 9, (int)MenuPos.Y + (menuSelectHeight * 7) + 10, 256, 20);

            fontSFX.Text = Math.Round((soundManager.Volume * 100f), 0).ToString() + "%";
            fontSFX.Update(gameTime);

            fontMusic.Text = Math.Round((SoundManager.MusicManager.Volume * 100f), 0).ToString() + "%";
            fontMusic.Update(gameTime);

            fontRes.Text = GetResolution(selRes);
            fontRes.Update(gameTime);

            fontInfo.Update(gameTime);
            fontInfo2.Update(gameTime);

            if(fullScreen)
            {
                fontFullscreen.Text = "ON";
            }
            else
            {
                fontFullscreen.Text = "OFF";
            }

            fontFullscreen.Update(gameTime);

            if (AInput.KeyPressed(Keys.Down) || AInput.ButtonPressed(Buttons.DPadDown, PlayerIndex.One))
            {
                sfxInstance = SoundManager.SoundManager.MenuSelect01.CreateInstance();
                soundManager.PlaySoundEffect(sfxInstance);
                selected++;

                if (selected > 5)
                    selected = 0;
            }
            else if (AInput.KeyPressed(Keys.Up) || AInput.ButtonPressed(Buttons.DPadUp, PlayerIndex.One))
            {
                sfxInstance = SoundManager.SoundManager.MenuSelect01.CreateInstance();
                soundManager.PlaySoundEffect(sfxInstance);
                selected--;

                if (selected < 0)
                    selected = 5;
            }

            if (AInput.KeyPressed(Keys.Left) || AInput.ButtonPressed(Buttons.DPadLeft, PlayerIndex.One))
            {
                if(selected == 0)
                {
                    sfxInstance = SoundManager.SoundManager.MenuSelect01.CreateInstance();
                    soundManager.PlaySoundEffect(sfxInstance);

                    soundManager.Volume -= 0.05f;

                    if (soundManager.Volume < 0f)
                        soundManager.Volume = 0f;
                }
                else if (selected == 1)
                {
                    sfxInstance = SoundManager.SoundManager.MenuSelect01.CreateInstance();
                    soundManager.PlaySoundEffect(sfxInstance);

                    SoundManager.MusicManager.Volume -= 0.05f;
                }
                else if(selected == 2)
                {
                    selRes--;

                    if (selRes < 0)
                        selRes = 0;
                }
                else if(selected == 3)
                {
                    fullScreen = !fullScreen;
                }
                else if (selected == 4)
                {
                    selected++;
                }
                else if (selected == 5)
                {
                    selected--;
                }
            }
            else if (AInput.KeyPressed(Keys.Right) || AInput.ButtonPressed(Buttons.DPadRight, PlayerIndex.One))
            {
                if (selected == 0)
                {
                    sfxInstance = SoundManager.SoundManager.MenuSelect01.CreateInstance();
                    soundManager.PlaySoundEffect(sfxInstance);

                    soundManager.Volume += 0.05f;

                    if (soundManager.Volume > 1f)
                        soundManager.Volume = 1f;
                }
                else if (selected == 1)
                {
                    sfxInstance = SoundManager.SoundManager.MenuSelect01.CreateInstance();
                    soundManager.PlaySoundEffect(sfxInstance);

                    SoundManager.MusicManager.Volume += 0.05f;
                }
                else if(selected == 2)
                {
                    selRes++;

                    if (selRes > resolutions.Count - 1)
                        selRes = resolutions.Count - 1;
                }
                else if (selected == 3)
                {
                    fullScreen = !fullScreen;
                }
                else if(selected == 4)
                {
                    selected++;
                }
                else if(selected == 5)
                {
                    selected--;
                }
            }

            if (AInput.KeyPressed(Keys.Enter) || AInput.ButtonPressed(Buttons.A, PlayerIndex.One))
            {
                if (selected == 0)
                {
                    // SFX
                }
                else if (selected == 1)
                {
                    // Music
                }
                else if(selected == 2)
                {
                    // Resolution
                }
                else if(selected == 3)
                {
                    fullScreen = !fullScreen;
                }
                else if(selected == 4)
                {
                    sfxInstance = SoundManager.SoundManager.MenuSelect02.CreateInstance();
                    soundManager.PlaySoundEffect(sfxInstance);

                    Engine.ScreenManager.SetScreen(new MainMenu());
                }
                else
                {
                    // Apply Settings
                    int width = 0, height = 0;

                    switch(selRes)
                    {
                        case 0:
                            width = 800;
                            height = 600;
                            break;
                        case 1:
                            width = 1024;
                            height = 768;
                            break;
                        case 2:
                            width = 1280;
                            height = 1024;
                            break;
                        case 3:
                            width = 1280;
                            height = 720;
                            break;
                        case 4:
                            width = 1920;
                            height = 1080;
                            break;
                        default:
                            width = 800;
                            height = 600;
                            break;
                    }

                    int fs = 0;
                    if (fullScreen == true)
                        fs = 1;

                    GameSettings.SaveSettings(soundManager.Volume, SoundManager.MusicManager.Volume, width, height, fs);
                    GameSettings.ApplySettings();

                    MenuPos = new Vector2((Engine.Monitor.Width - (int)(menuBack.Width * 1.5f)), 120);

                    sfxInstance = SoundManager.SoundManager.MenuSelect02.CreateInstance();
                    soundManager.PlaySoundEffect(sfxInstance);

                    Engine.ScreenManager.SetScreen(new MainMenu());
                }
            }
        }

        private void FillRes()
        {
            resolutions.Add("800x600 (Ratio 4:3)");
            resolutions.Add("1024x768 (Ratio 4:3)");
            resolutions.Add("1280x1024 (Ratio 4:3)");
            resolutions.Add("1280x720 (Ratio 16:9)");
            resolutions.Add("1920x1080 (Ratio 16:9)");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null, Engine.Monitor.GetTransformationMatrix());

            spriteBatch.Draw(menuBackground, new Rectangle(0, 0, 800, 600), Color.White);

            if (selected == 0)
            {
                spriteBatch.Draw(menuSFXSelect, MenuPos, Color.White);
                spriteBatch.Draw(menuMusic, new Vector2(MenuPos.X, MenuPos.Y + menuSelectHeight * 2), Color.White);
                spriteBatch.Draw(menuResolution, new Vector2(MenuPos.X, MenuPos.Y + menuSelectHeight * 4), Color.White);
                spriteBatch.Draw(menuFullscreen, new Vector2(MenuPos.X, MenuPos.Y + menuSelectHeight * 6), Color.White);
                spriteBatch.Draw(menuBack, new Vector2(MenuPos.X, MenuPos.Y + menuSelectHeight * 8), Color.White);
                spriteBatch.Draw(menuApply, new Vector2(MenuPos.X + 160, MenuPos.Y + menuSelectHeight * 8), Color.White);
            }
            else if (selected == 1)
            {
                spriteBatch.Draw(menuSFX, MenuPos, Color.White);
                spriteBatch.Draw(menuMusicSelect, new Vector2(MenuPos.X, MenuPos.Y + menuSelectHeight * 2), Color.White);
                spriteBatch.Draw(menuResolution, new Vector2(MenuPos.X, MenuPos.Y + menuSelectHeight * 4), Color.White);
                spriteBatch.Draw(menuFullscreen, new Vector2(MenuPos.X, MenuPos.Y + menuSelectHeight * 6), Color.White);
                spriteBatch.Draw(menuBack, new Vector2(MenuPos.X, MenuPos.Y + menuSelectHeight * 8), Color.White);
                spriteBatch.Draw(menuApply, new Vector2(MenuPos.X + 160, MenuPos.Y + menuSelectHeight * 8), Color.White);
            }
            else if (selected == 2)
            {
                spriteBatch.Draw(menuSFX, MenuPos, Color.White);
                spriteBatch.Draw(menuMusic, new Vector2(MenuPos.X, MenuPos.Y + menuSelectHeight * 2), Color.White);
                spriteBatch.Draw(menuResolutionSelect, new Vector2(MenuPos.X, MenuPos.Y + menuSelectHeight * 4), Color.White);
                spriteBatch.Draw(menuFullscreen, new Vector2(MenuPos.X, MenuPos.Y + menuSelectHeight * 6), Color.White);
                spriteBatch.Draw(menuBack, new Vector2(MenuPos.X, MenuPos.Y + menuSelectHeight * 8), Color.White);
                spriteBatch.Draw(menuApply, new Vector2(MenuPos.X + 160, MenuPos.Y + menuSelectHeight * 8), Color.White);
            }
            else if (selected == 3)
            {
                spriteBatch.Draw(menuSFX, MenuPos, Color.White);
                spriteBatch.Draw(menuMusic, new Vector2(MenuPos.X, MenuPos.Y + menuSelectHeight * 2), Color.White);
                spriteBatch.Draw(menuResolution, new Vector2(MenuPos.X, MenuPos.Y + menuSelectHeight * 4), Color.White);
                spriteBatch.Draw(menuFullscreenSelect, new Vector2(MenuPos.X, MenuPos.Y + menuSelectHeight * 6), Color.White);
                spriteBatch.Draw(menuBack, new Vector2(MenuPos.X, MenuPos.Y + menuSelectHeight * 8), Color.White);
                spriteBatch.Draw(menuApply, new Vector2(MenuPos.X + 160, MenuPos.Y + menuSelectHeight * 8), Color.White);
            }
            else if (selected == 4)
            {
                spriteBatch.Draw(menuSFX, MenuPos, Color.White);
                spriteBatch.Draw(menuMusic, new Vector2(MenuPos.X, MenuPos.Y + menuSelectHeight * 2), Color.White);
                spriteBatch.Draw(menuResolution, new Vector2(MenuPos.X, MenuPos.Y + menuSelectHeight * 4), Color.White);
                spriteBatch.Draw(menuFullscreen, new Vector2(MenuPos.X, MenuPos.Y + menuSelectHeight * 6), Color.White);
                spriteBatch.Draw(menuBackSelect, new Vector2(MenuPos.X, MenuPos.Y + menuSelectHeight * 8), Color.White);
                spriteBatch.Draw(menuApply, new Vector2(MenuPos.X + 160, MenuPos.Y + menuSelectHeight * 8), Color.White);
            }
            else if (selected == 5)
            {
                spriteBatch.Draw(menuSFX, MenuPos, Color.White);
                spriteBatch.Draw(menuMusic, new Vector2(MenuPos.X, MenuPos.Y + menuSelectHeight * 2), Color.White);
                spriteBatch.Draw(menuResolution, new Vector2(MenuPos.X, MenuPos.Y + menuSelectHeight * 4), Color.White);
                spriteBatch.Draw(menuFullscreen, new Vector2(MenuPos.X, MenuPos.Y + menuSelectHeight * 6), Color.White);
                spriteBatch.Draw(menuBack, new Vector2(MenuPos.X, MenuPos.Y + menuSelectHeight * 8), Color.White);
                spriteBatch.Draw(menuApplySelect, new Vector2(MenuPos.X + 160, MenuPos.Y + menuSelectHeight * 8), Color.White);
            }

            fontInfo.Draw(spriteBatch);
            fontInfo2.Draw(spriteBatch);
            fontSFX.Draw(spriteBatch);
            fontMusic.Draw(spriteBatch);
            fontRes.Draw(spriteBatch);
            fontFullscreen.Draw(spriteBatch);

            spriteBatch.Draw(aButton, new Vector2(320, 185), Color.White);
            spriteBatch.Draw(bButton, new Vector2(320, 202), Color.White);
            spriteBatch.Draw(xButton, new Vector2(320, 218), Color.White);
            spriteBatch.Draw(yButton, new Vector2(320, 235), Color.White);

            spriteBatch.End();
        }
    }
}
