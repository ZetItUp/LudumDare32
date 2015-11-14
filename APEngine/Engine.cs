using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using AtriLib2;
using System.Globalization;

namespace APEngine
{
    public class Camera
    {
        Matrix transform;
        Matrix inverseTransform;
        Vector2 position;
        Viewport viewport;
        public float LevelWidth { get; set; }
        public float LevelHeight { get; set; }
        public float Rotation { get; set; }
        public float Zoom { get; set; }

        public Rectangle Rect
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, Engine.Monitor.Viewport.Width, Engine.Monitor.Viewport.Height);
            }
        }

        public Rectangle ViewRectangle
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, Engine.Monitor.VirtualWidth, Engine.Monitor.VirtualHeight);
            }
        }

        public Matrix Transform
        {
            get { return transform; }
        }

        public Matrix InverseTransform
        {
            get { return inverseTransform; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Camera(Viewport viewPort)
        {
            viewport = viewPort;
            Position = new Vector2(0, 0);
            Rotation = 0f;
            Zoom = 2f;
        }

        public void Update(GameTime gameTime, Entities.Player player)
        {
            Position = new Vector2(player.Position.X - (Engine.Monitor.VirtualWidth / 2) / Zoom, player.Position.Y - (Engine.Monitor.VirtualHeight / 2) / Zoom);

            if (position.X < 0)
                position.X = 0;
            if (position.Y < 0)
                position.Y = 0;

            if (position.X > LevelWidth - Engine.Monitor.VirtualWidth / Zoom)
                position.X = LevelWidth - Engine.Monitor.VirtualWidth / Zoom;
            if (position.Y > LevelHeight - Engine.Monitor.VirtualHeight / Zoom)
                position.Y = LevelHeight - Engine.Monitor.VirtualHeight / Zoom;

            transform = Matrix.CreateTranslation(-position.X, -position.Y, 0) *
                Matrix.CreateRotationZ(Rotation) *
                Engine.Monitor.GetTransformationMatrix() *
                Matrix.CreateScale(new Vector3(Zoom, Zoom, 1f));
            inverseTransform = Matrix.Invert(transform);
        }

        public Vector2 GetMousePos
        {
            get
            {
                Vector2 mousepos = AMouse.MousePosition();
                mousepos = Vector2.Transform(mousepos, this.InverseTransform);

                return mousepos;
            }
        }
    }

    public class Engine
    {
        public static ContentManager ContentManager;
        public static AMonitor Monitor;
        public static AGraphics AtriceGraphics;
        public static Random Randomizer = new Random();
        public static bool Paused = false;
        public static bool DoExit = false;
        public static KeyboardDispatcher KeyboardDispatcher;
        public static Game CurrentGame;
        public static ScreenManagers.ScreenManager ScreenManager;

        private static Dictionary<string, Cursor> cursors;

        public static void SetMouseCursor(string cursorID)
        {
            var nativeWindow = Control.FromHandle(CurrentGame.Window.Handle);

            nativeWindow.Cursor = cursors[cursorID];
        }

        public static void LoadCursors()
        {
            if (cursors != null && cursors.Count > 0)
                return;

            cursors = new Dictionary<string, Cursor>();

            cursors.Add(GameSettings.MOUSE_POINTER_NORMAL, AGraphics.LoadCursor(Path.Combine(ATools.AssemblyDirectory + "\\Content\\Data\\GFX\\Editor\\MousePointer.cur")));
            cursors.Add(GameSettings.MOUSE_POINTER_ADD, AGraphics.LoadCursor(Path.Combine(ATools.AssemblyDirectory + "\\Content\\Data\\GFX\\Editor\\MousePointerAdd.cur")));
            cursors.Add(GameSettings.MOUSE_POINTER_MOVE, AGraphics.LoadCursor(Path.Combine(ATools.AssemblyDirectory + "\\Content\\Data\\GFX\\Editor\\MousePointerMove.cur")));
            cursors.Add(GameSettings.MOUSE_POINTER_REMOVE, AGraphics.LoadCursor(Path.Combine(ATools.AssemblyDirectory + "\\Content\\Data\\GFX\\Editor\\MousePointerRemove.cur")));
            cursors.Add(GameSettings.MOUSE_POINTER_SETTINGS, AGraphics.LoadCursor(Path.Combine(ATools.AssemblyDirectory + "\\Content\\Data\\GFX\\Editor\\MousePointerSettings.cur")));
        }

        public Engine()
        {

        }
    }

    public static class GameSettings
    {
        public const string UI_EDITOR_SELECTED_TILE_BUTTON = "wndSelectedTileButton";
        public const string UI_EDITOR_SELECTED_GAME_OBJECT = "wndSelectedGameObject";

        public const string MOUSE_POINTER_NORMAL = "MousePointerNormal";
        public const string MOUSE_POINTER_ADD = "MousePointerAdd";
        public const string MOUSE_POINTER_REMOVE = "MousePointerRemove";
        public const string MOUSE_POINTER_MOVE = "MousePointerMove";
        public const string MOUSE_POINTER_SETTINGS = "MousePointerSettings";

        public static readonly Microsoft.Xna.Framework.Input.Keys BUILD_KEY = Microsoft.Xna.Framework.Input.Keys.B;
        public static readonly Microsoft.Xna.Framework.Input.Keys MOVE_KEY = Microsoft.Xna.Framework.Input.Keys.M;
        public static readonly Microsoft.Xna.Framework.Input.Keys REMOVE_KEY = Microsoft.Xna.Framework.Input.Keys.K;
        public static readonly Microsoft.Xna.Framework.Input.Keys TEXTURE_SELECT_KEY = Microsoft.Xna.Framework.Input.Keys.N;

        public static string GAME_KEY = "ABCDEFGH";     // TODO: SET A PROPER KEY!

        public static readonly Color DEFAULT_DEBUG_COLOR = new Color(0, 200, 0);
        public static readonly Color DEFAULT_DEBUG_BUSY_COLOR = new Color(200, 0, 0);

        public static int ResolutionWidth = 0;
        public static int ResolutionHeight = 0;
        public static int Fullscreen = 0;
        public static float SFXVol = 0f;
        public static float MusicVol = 0f;

        public static void SaveSettings(float sfxVol, float muscVol)
        {
            string path = Directory.GetCurrentDirectory();
            path += "\\Settings\\GameSettings.dat";

            if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\Settings"))
            {
                DirectoryInfo dInfo = Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Settings");
            }

            ResolutionWidth = Engine.Monitor.Width;
            ResolutionHeight = Engine.Monitor.Height;
            SFXVol = sfxVol;
            MusicVol = muscVol;

            int fs = 0;
            if (Engine.Monitor.Fullscreen)
                fs = 1;

            Fullscreen = fs;

            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine("SFX=" + SFXVol.ToString());
                sw.WriteLine("Music=" + MusicVol.ToString());
                sw.WriteLine("Width=" + ResolutionWidth.ToString());
                sw.WriteLine("Height=" + ResolutionHeight.ToString());
                sw.WriteLine("Fullscreen=" + Fullscreen.ToString());
            }
        }

        public static void SaveSettings(float sfxVol, float muscVol, int width, int height, int fullscreen)
        {
            string path = Directory.GetCurrentDirectory();
            path += "\\Settings\\GameSettings.dat";

            if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\Settings"))
            {
                DirectoryInfo dInfo = Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Settings");
            }

            ResolutionWidth = width;
            ResolutionHeight = height;
            Fullscreen = fullscreen;
            SFXVol = sfxVol;
            MusicVol = muscVol;

            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine("SFX=" + SFXVol.ToString());
                sw.WriteLine("Music=" + MusicVol.ToString());
                sw.WriteLine("Width=" + ResolutionWidth.ToString());
                sw.WriteLine("Height=" + ResolutionHeight.ToString());
                sw.WriteLine("Fullscreen=" + Fullscreen.ToString());
            }
        }

        public static bool LoadSettings()
        {
            // Make sure the directory exists!
            if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\Settings"))
            {
                return false;
            }

            // Make sure the file exists!
            if (!File.Exists(Directory.GetCurrentDirectory() + "\\Settings\\GameSettings.dat"))
            {
                return false;
            }

            // Everything exists, we should be fine to load
            string path = Directory.GetCurrentDirectory();
            path += "\\Settings\\GameSettings.dat";

            using (StreamReader sr = new StreamReader(path))
            {
                while (sr.Peek() > 0)
                {
                    string line = sr.ReadLine();

                    if (line.StartsWith("Width="))
                    {
                        string strWidth = "";

                        for (int i = 6; i < line.Length; i++)
                        {
                            strWidth += line[i];
                        }

                        GameSettings.ResolutionWidth = int.Parse(strWidth);
                    }
                    else if (line.StartsWith("Height="))
                    {
                        string strHeight = "";

                        for (int i = 7; i < line.Length; i++)
                        {
                            strHeight += line[i];
                        }

                        GameSettings.ResolutionHeight = int.Parse(strHeight);
                    }
                    else if (line.StartsWith("Fullscreen="))
                    {
                        string fs = line[11].ToString();

                        if (fs != "0" && fs != "1")
                            fs = "0";

                        GameSettings.Fullscreen = int.Parse(fs);
                    }
                    else if (line.StartsWith("SFX="))
                    {
                        string sf = "";

                        for (int i = 4; i < line.Length; i++)
                        {
                            sf += line[i];
                        }

                        CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                        ci.NumberFormat.CurrencyDecimalSeparator = ".";
                        SFXVol = float.Parse(sf, NumberStyles.Any, ci);
                    }
                    else if (line.StartsWith("Music="))
                    {
                        string mv = "";

                        for (int i = 6; i < line.Length; i++)
                        {
                            mv += line[i];
                        }

                        CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                        ci.NumberFormat.CurrencyDecimalSeparator = ".";
                        MusicVol = float.Parse(mv, NumberStyles.Any, ci);
                        SoundManager.MusicManager.Volume = MusicVol;
                    }
                }
            }

            return true;
        }

        // Worst name ever :P
        public static int GetSelectedResolutionFromResolution(int width, int height)
        {
            if (width == 800 && height == 600)
                return 0;
            else if (width == 1024 && height == 768)
                return 1;
            else if (width == 1280 && height == 1024)
                return 2;
            else if (width == 1280 && height == 720)
                return 3;
            else if (width == 1920 && height == 1080)
                return 4;
            else
                return 0;
        }

        public static void ApplySettings()
        {
            bool fs = false;
            if (Fullscreen == 1)
                fs = true;

            Engine.Monitor.Fullscreen = fs;

            Engine.Monitor.SetVirtualResolution(800, 600);

            switch (GetSelectedResolutionFromResolution(ResolutionWidth, ResolutionHeight))
            {
                case 0:

                    Engine.Monitor.SetResolution(Resolutions.R800x600);
                    break;
                case 1:

                    Engine.Monitor.SetResolution(Resolutions.R1024x768);
                    break;
                case 2:

                    Engine.Monitor.SetResolution(Resolutions.R1280x1024);
                    break;
                case 3:

                    Engine.Monitor.SetResolution(Resolutions.R1280x720);
                    break;
                case 4:

                    Engine.Monitor.SetResolution(Resolutions.R1920x1080);
                    break;
                default:

                    Engine.Monitor.SetResolution(Resolutions.R800x600);
                    break;
            }

            Engine.Monitor.CenterToScreen(ref Engine.CurrentGame);
        }
    }
}
