using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using APEngine.Entities;
using AtriLib2;

namespace APEngine.Map
{
    public enum Theme
    {
        City = 0,
        SandyBeach,
        JungleForest,
        Prison
    };

    public class Level
    {
        [DllImport("Kernel32.dll", EntryPoint="RtlZeroMemory")]
        public static extern bool ZeroMemory(ref string Destination, int Length);

        public static int MAX_WATER_LEVEL = 5;
        public int Width { get; set; }
        public int Height { get; set; }
        public Tile[,] tile { get; set; }
        public Vector2 PlayerSpawn { get; set; }
        public static Level CurrentLevel { get; private set; }
        public Theme Theme { get; set; }
        public bool CheckpointReached { get; set; }
        private string _name;
        public string Name 
        { 
            get
            {
                return _name;
            }
            set
            {
                _name = value;

                ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;
                if(gs.EditUI != null)
                {
                    gs.EditUI.SetLevelName(_name);
                }
            }
        }

        List<Fence> fences;

        private GCHandle gch;

        public Level()
        {
            this.Theme = Theme.JungleForest;
            CheckpointReached = false;
            Name = "";
        }

        public int GetWidthInPixels
        {
            get { return Width * Tile.WIDTH; }
        }

        public int GetHeightInPixels
        {
            get { return Height * Tile.HEIGHT; }
        }

        private string DecryptFile(string mapName)
        {
            gch = GCHandle.Alloc(GameSettings.GAME_KEY, GCHandleType.Pinned);

            string lvlName = mapName.Replace(" ", "");
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = ASCIIEncoding.ASCII.GetBytes(GameSettings.GAME_KEY);
            DES.IV = ASCIIEncoding.ASCII.GetBytes(GameSettings.GAME_KEY);

            string path = Path.Combine(ATools.AssemblyDirectory + "\\Content\\Data\\Maps\\" + lvlName + ".map");

            FileStream fs = new FileStream(path, Helper.FileMode(Helper.FileModeType.Open), Helper.FileAccess(Helper.FileAccesser.Read));

            ICryptoTransform desdecrypt = DES.CreateDecryptor();
            CryptoStream cryptoStream = new CryptoStream(fs, desdecrypt, CryptoStreamMode.Read);

            string strRetVal = "";

            using(StreamReader sr = new StreamReader(cryptoStream))
            {
                while(sr.Peek() > 0)
                {
                    string read = sr.ReadLine();
                    if (read == null)
                        continue;

                    read = read.Replace("\r", String.Empty);
                    string[] r2 = read.Split('\n');

                    foreach(String s in r2)
                    {
                        strRetVal += s + "\n";
                    }
                }
            }

            fs.Close();
            gch.Free();

            return strRetVal;
        }

        public void GenerateNewBackground()
        {
            if(this.Theme == Theme.City)
            {
                fences = new List<Fence>();
                GenerateFences();
            }
            else if(this.Theme == Theme.JungleForest)
            {
                GenerateMushrooms();
                GenerateFlowers();
                GenerateBigFlowers();
                GenerateGrass();
                GenerateWoodenFences();
            }
        }

        private void GenerateWoodenFences()
        {
            ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;

            for(int y = 1; y < tile.GetLength(1) - 1; y++)
            {
                for(int x = 1; x < tile.GetLength(0); x++)
                {
                    if(y < tile.GetLength(1) - 1)
                    {
                        // If just a single platform we do not set a fence!

                        // The LEFT Wooden platform with a continuous platform to the right.
                        if(tile[x - 1, y].ID != TileID.WoodPlatform && 
                            tile[x, y].ID == TileID.WoodPlatform && 
                            tile[x + 1, y].ID == TileID.WoodPlatform)
                        {
                            GameObjects.WoodenFence fence = new GameObjects.WoodenFence(new Vector2((x * Tile.WIDTH), y * Tile.HEIGHT - 27));
                            fence.Piece = GameObjects.WoodFencePiece.Left;
                            gs.AddGameObject(fence);
                        }
                        // The CENTER Wooden platform with continous platforms
                        else if(tile[x - 1, y].ID == TileID.WoodPlatform && 
                            tile[x, y].ID == TileID.WoodPlatform && 
                            tile[x + 1, y].ID == TileID.WoodPlatform)
                        {
                            GameObjects.WoodenFence fence = new GameObjects.WoodenFence(new Vector2((x * Tile.WIDTH), y * Tile.HEIGHT - 27));
                            fence.Piece = GameObjects.WoodFencePiece.Center;
                            gs.AddGameObject(fence);
                        }
                        // The RIGHT Wooden platform with a continuous platform to the right.
                        if(tile[x - 1, y].ID == TileID.WoodPlatform && 
                            tile[x, y].ID == TileID.WoodPlatform && 
                            tile[x + 1, y].ID != TileID.WoodPlatform)
                        {
                            GameObjects.WoodenFence fence = new GameObjects.WoodenFence(new Vector2((x * Tile.WIDTH), y * Tile.HEIGHT - 27));
                            fence.Piece = GameObjects.WoodFencePiece.Right;
                            gs.AddGameObject(fence);
                        }
                    }
                }
            }
        }

        private void GenerateMushrooms()
        {
            ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;

            for(int y = 1; y < tile.GetLength(1) - 1; y++)
            {
                for(int x = 0; x < tile.GetLength(0); x++)
                {
                    if (y < tile.GetLength(1) - 1)
                    {
                        if (tile[x, y + 1].Solid == true && tile[x, y].Solid == false &&
                        (tile[x, y + 1].ID == TileID.LeftGrassPlatform || 
                        tile[x, y + 1].ID == TileID.RightGrassPlatform || 
                        tile[x, y + 1].ID == TileID.GrassPlatform))
                        {
                            // We can have a mushroom here
                            int shroom = Engine.Randomizer.Next(0, 100);

                            if (shroom > 0 && shroom < 20)
                            {
                                // We will have a mushroom here
                                GameObjects.Mushroom nShroom = new GameObjects.Mushroom(new Vector2((x * Tile.WIDTH) + Engine.Randomizer.Next(-7, 32 - 7), y * Tile.HEIGHT));
                                gs.AddGameObject(nShroom);
                            }
                        }
                    }
                    else
                    {
                        if (tile[x, y].Solid == false)
                        {
                            // We can have a mushroom here
                            int shroom = Engine.Randomizer.Next(0, 100);

                            if (shroom > 0 && shroom < 20)
                            {
                                // We will have a mushroom here
                                GameObjects.Mushroom nShroom = new GameObjects.Mushroom(new Vector2((x * Tile.WIDTH) + Engine.Randomizer.Next(-7, 32 - 7), y * Tile.HEIGHT));
                                gs.AddGameObject(nShroom);
                            }
                        }
                    }
                }
            }
        }

        private void GenerateFlowers()
        {
            ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;

            for (int y = 1; y < tile.GetLength(1) - 1; y++)
            {
                for (int x = 0; x < tile.GetLength(0); x++)
                {
                    if (tile[x, y].Solid == false && 
                        (tile[x, y + 1].ID == TileID.LeftGrassPlatform || 
                        tile[x, y + 1].ID == TileID.RightGrassPlatform || 
                        tile[x, y + 1].ID == TileID.GrassPlatform))
                    {
                        // We can have a flower here
                        int flower = Engine.Randomizer.Next(0, 100);

                        if (flower > 0 && flower < 70)
                        {
                            GameObjects.Flower nflower = new GameObjects.Flower(new Vector2((x * Tile.WIDTH) + Engine.Randomizer.Next(-2, 30-8), y * Tile.HEIGHT + 16));
                            nflower.FlowerColor = (GameObjects.FlowerColor)Engine.Randomizer.Next(0, Enum.GetNames(typeof(GameObjects.FlowerColor)).Length);
                            gs.AddGameObject(nflower);
                        }
                    }
                }
            }
        }

        private void GenerateBigFlowers()
        {
            ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;

            float d = 0.31f;

            for (int y = 1; y < tile.GetLength(1) - 2; y++)
            {
                for (int x = 0; x < tile.GetLength(0) - 1; x++)
                {
                    if (tile[x, y].Solid == false &&
                        (tile[x, y + 1].ID == TileID.LeftGrassPlatform ||
                        tile[x, y + 1].ID == TileID.RightGrassPlatform ||
                        tile[x, y + 1].ID == TileID.GrassPlatform))
                    {
                        // We can have a flower here
                        int flower = Engine.Randomizer.Next(0, 100);

                        if (flower > 0 && flower < 10)
                        {
                            GameObjects.BigFlower nflower = new GameObjects.BigFlower(new Vector2((x * Tile.WIDTH), y * Tile.HEIGHT));
                            nflower.FlowerColor = (GameObjects.BigFlowerColor)Engine.Randomizer.Next(0, Enum.GetNames(typeof(GameObjects.BigFlowerColor)).Length);
                            nflower.Depth = d;
                            gs.AddGameObject(nflower);

                            d += 0.0001f;
                        }
                    }
                }
            }
        }

        private void GenerateGrass()
        {
            ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;

            int wallPercent = 60;

            for (int y = 1; y < tile.GetLength(1); y++)
            {
                for (int x = 0; x < tile.GetLength(0); x++)
                {
                    if (tile[x, y].ID == TileID.GrassPlatform)
                    {
                        GameObjects.Grass grass = new GameObjects.Grass(new Vector2((x * Tile.WIDTH), y * Tile.HEIGHT - Tile.HEIGHT));
                        grass.Style = (GameObjects.GrassStyle)Engine.Randomizer.Next(0, 3);
                        gs.AddGameObject(grass);

                        int wall = Engine.Randomizer.Next(0, 100);
                        int wallStyle = Engine.Randomizer.Next(0, Enum.GetNames(typeof(GameObjects.GrassWallStyle)).Length);

                        if(wall > 0 && wall < wallPercent)
                        {
                            GameObjects.GrassWall wl = new GameObjects.GrassWall(new Vector2((x * Tile.WIDTH), y * Tile.HEIGHT - (Tile.HEIGHT * 2)));

                            if (wallStyle != 0)
                                wl.Style = GameObjects.GrassWallStyle.Short;

                            gs.AddGameObject(wl);
                        }
                    }
                    else if (tile[x, y].ID == TileID.LeftGrassPlatform)
                    {
                        GameObjects.Grass grass = new GameObjects.Grass(new Vector2((x * Tile.WIDTH), y * Tile.HEIGHT - Tile.HEIGHT));
                        grass.Style = (GameObjects.GrassStyle)Engine.Randomizer.Next(0, 3);
                        gs.AddGameObject(grass);

                        GameObjects.Grass side = new GameObjects.Grass(new Vector2((x * Tile.WIDTH - Tile.WIDTH), y * Tile.HEIGHT));
                        side.Style = GameObjects.GrassStyle.Four;
                        gs.AddGameObject(side);

                        int wall = Engine.Randomizer.Next(0, 100);
                        int wallStyle = Engine.Randomizer.Next(0, Enum.GetNames(typeof(GameObjects.GrassWallStyle)).Length);

                        if (wall > 0 && wall < wallPercent)
                        {
                            GameObjects.GrassWall wl = new GameObjects.GrassWall(new Vector2((x * Tile.WIDTH), y * Tile.HEIGHT - (Tile.HEIGHT * 2)));

                            if (wallStyle != 0)
                                wl.Style = GameObjects.GrassWallStyle.Short;

                            gs.AddGameObject(wl);
                        }
                    }
                    else if (tile[x, y].ID == TileID.RightGrassPlatform)
                    {
                        GameObjects.Grass grass = new GameObjects.Grass(new Vector2((x * Tile.WIDTH), y * Tile.HEIGHT - Tile.HEIGHT));
                        grass.Style = (GameObjects.GrassStyle)Engine.Randomizer.Next(0, 3);
                        gs.AddGameObject(grass);

                        GameObjects.Grass side = new GameObjects.Grass(new Vector2((x * Tile.WIDTH) + Tile.WIDTH, y * Tile.HEIGHT));
                        side.Style = GameObjects.GrassStyle.Five;
                        gs.AddGameObject(side);

                        int wall = Engine.Randomizer.Next(0, 100);
                        int wallStyle = Engine.Randomizer.Next(0, Enum.GetNames(typeof(GameObjects.GrassWallStyle)).Length);

                        if (wall > 0 && wall < wallPercent)
                        {
                            GameObjects.GrassWall wl = new GameObjects.GrassWall(new Vector2((x * Tile.WIDTH), y * Tile.HEIGHT - (Tile.HEIGHT * 2)));

                            if (wallStyle != 0)
                                wl.Style = GameObjects.GrassWallStyle.Short;

                            gs.AddGameObject(wl);
                        }
                    }
                }
            }
        }

        private void GenerateFences()
        {
            List<Vector3> lengths = new List<Vector3>();

            // we aint gonna generate fences when we are higher than 4 blocks form the top
            for (int y = 4; y < tile.GetLength(1); y++)
            {
                // no need to check the first tile since that will be a level block
                int length = 0;
                int start = 0;
                Vector3 currLength = Vector3.Zero;

                for (int x = 0; x < tile.GetLength(0); x++)
                {
                    if(length == 0)
                    {
                        start = x;
                    }
                    // what we need to do is calculate how far we can generate a fence, if only 1 block, we are not generating one

                    // Make sure the 3 tiles above it is not Solid, we dont care what type of tile it is thou
                    if(tile[x, y - 1].Solid == false && tile[x, y - 2].Solid == false && tile[x, y - 3].Solid == false)
                    {
                        if (tile[x, y].ID == TileID.Platform)
                        {
                            length++;

                            if(tile[x + 1, y].Solid == false)
                            {
                                currLength = new Vector3(start, length, y);
                                lengths.Add(currLength);
                                length = 0;
                                start = 0;
                            }
                        }
                    }
                }
            }

            // Add the fences on current Y position..
            foreach (Vector3 v in lengths)
            {
                // Make sure the length is greater than 1, else ignore it
                if (v.Y < 2)
                    continue;
                else
                {
                    if (v.Y >= 3)
                    {
                        Fence f1 = new Fence(FenceStyle.LeftEdge);
                        f1.Position = new Vector2(v.X * Tile.WIDTH, (v.Z - 3) * Tile.HEIGHT);
                        fences.Add(f1);

                        if (v.Y > 4)
                        {
                            int width = (int)v.X + (int)v.Y;
                            bool doorLast = ATools.IntToBool(Engine.Randomizer.Next(0, 1 + 1));

                            if (doorLast)
                            {
                                for (int i = (int)v.X + 1; i < width - 4; i++)
                                {
                                    Fence currFence = new Fence(FenceStyle.Short);
                                    currFence.Position = new Vector2(i * Tile.WIDTH, (v.Z - 3) * Tile.HEIGHT);
                                    fences.Add(currFence);
                                }

                                Fence doorFence = new Fence(FenceStyle.Door);
                                doorFence.Position = new Vector2((width - 4) * Tile.WIDTH, (v.Z - 3) * Tile.HEIGHT);
                                fences.Add(doorFence);
                            }
                            else
                            {
                                Fence doorFence = new Fence(FenceStyle.Door);
                                doorFence.Position = new Vector2((v.X + 1) * Tile.WIDTH, (v.Z - 3) * Tile.HEIGHT);
                                fences.Add(doorFence);

                                for (int i = (int)v.X + 4; i < width - 1; i++)
                                {
                                    Fence currFence = new Fence(FenceStyle.Short);
                                    currFence.Position = new Vector2(i * Tile.WIDTH, (v.Z - 3) * Tile.HEIGHT);
                                    fences.Add(currFence);
                                }
                            }
                        }
                        else
                        {
                            int width = (int)v.X + (int)v.Y;

                            for (int i = (int)v.X + 1; i < width - 1; i++)
                            {
                                Fence currFence = new Fence(FenceStyle.Short);
                                currFence.Position = new Vector2(i * Tile.WIDTH, (v.Z - 3) * Tile.HEIGHT);
                                fences.Add(currFence);
                            }
                        }

                        Fence fLast = new Fence(FenceStyle.RightEdge);
                        fLast.Position = new Vector2((v.X + v.Y - 1) * Tile.WIDTH, (v.Z - 3) * Tile.HEIGHT);
                        fences.Add(fLast);
                    }
                    else
                    {
                        Fence f1 = new Fence(FenceStyle.LeftEdge);
                        f1.Position = new Vector2(v.X * Tile.WIDTH, (v.Z - 3) * Tile.HEIGHT);
                        fences.Add(f1);

                        Fence fLast = new Fence(FenceStyle.RightEdge);
                        fLast.Position = new Vector2((v.X + 1) * Tile.WIDTH, (v.Z - 3) * Tile.HEIGHT);
                        fences.Add(fLast);
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            int xlength = (int)((camera.ViewRectangle.Width / Tile.WIDTH) / camera.Zoom) + 1;
            int ylength = (int)((camera.ViewRectangle.Height / Tile.HEIGHT) / camera.Zoom) + 1;
            int xpos = (int)((camera.ViewRectangle.X / Tile.WIDTH));
            int ypos = (int)((camera.ViewRectangle.Y / Tile.HEIGHT));

            if (xpos < 0)
                xpos = 0;
            if (ypos < 0)
                ypos = 0;

            for(int x = 0; x <= xlength; x++)
            {
                for(int y = 0; y <= ylength; y++)
                {
                    int _x = xpos + x;
                    int _y = ypos + y;

                    if (_x >= tile.GetLength(0))
                        _x = tile.GetLength(0) - 1;

                    if (_y >= tile.GetLength(1))
                        _y = tile.GetLength(1) - 1;

                    tile[_x, _y].Draw(spriteBatch);
                }
            }

            if (this.Theme == Theme.City)
            {
                foreach (Fence f in fences)
                {
                    f.Draw(spriteBatch);
                }
            }
        }

        public void SaveMap()
        {
            gch = GCHandle.Alloc(GameSettings.GAME_KEY, GCHandleType.Pinned);

            string lvlName = CurrentLevel.Name.Replace(" ", "");
            string fname = ATools.AssemblyDirectory + "\\Content\\Data\\Maps\\" + lvlName + ".map";

            FileStream fs = new FileStream(fname, Helper.FileMode(Helper.FileModeType.Create), Helper.FileAccess(Helper.FileAccesser.Write));
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = Encoding.ASCII.GetBytes(GameSettings.GAME_KEY);
            DES.IV = Encoding.ASCII.GetBytes(GameSettings.GAME_KEY);

            ICryptoTransform desencrypt = DES.CreateEncryptor();
            CryptoStream cryptoStream = new CryptoStream(fs, desencrypt, CryptoStreamMode.Write);

            string fileData = "";

            // Fill data here!

            // Theme
            fileData += ((int)Theme) + Environment.NewLine;

            // Name
            fileData += CurrentLevel.Name + Environment.NewLine;

            // Width
            fileData += ((int)Width) + Environment.NewLine;

            // Height
            fileData += ((int)Height) + Environment.NewLine;

            // Write all Tiles first
            for(int x = 0; x < Width; x++)
            {
                for(int y = 0; y < Height; y++)
                {
                    string TileData = tile[x, y].GetTileData;

                    if (x == Width - 1)
                        TileData += Environment.NewLine;

                    fileData += TileData;
                }
            }

            // Write all Game objects
            ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;
            var gobjList = gs.GetAllNoneBackgroundObjects();
            int objCounter = 0;

            for(int i = 0; i < gobjList.Count; i++)
            {
                string objData = gobjList[i].GetGameObjectData();
                objCounter++;

                if(objCounter > 15)
                {
                    objData += Environment.NewLine;
                    objCounter = 0;
                }

                fileData += objData;
            }

            // End fill data

            int fileSize = fileData.Length;
            byte[] byteArray = new byte[fileSize];
            for(int i = 0; i < fileSize; i++)
            {
                byteArray[i] = (byte)fileData[i];
            }

            cryptoStream.Write(byteArray, 0, byteArray.Length);
            cryptoStream.Close();
            fs.Close();

            gch.Free();

            MessageBox.Show(CurrentLevel.Name + " was saved!", "Save Complete!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private struct TileData
        {
            public TileType TileType;
            public bool Solid;
            public int XPos;
            public int YPos;
        };

        //private struct GameObjectData
        //{
        //    public GameObjects.GameObject Object;
        //    public float XPos;
        //    public float YPos;
        //    public string FileName;
        //};

        //private struct EntityData
        //{
        //    public float XPos;
        //    public float YPos;
        //    public string EntityFilename;
        //    public int Health;
        //};

        private enum DataType
        {
            NotSet = -1,
            Tile = 0,
            GameObject,
            Entity
        };

        public void LoadMap(string mapName)
        {
            string mapData = DecryptFile(mapName);
            int currentLine = 0;

            string currData = "";
            bool inDataField = false;
            bool spawnPointFound = false;

            DataType dataType = DataType.NotSet;
            TileData tileData = new TileData();
            //GameObjectData gameObjectData = new GameObjectData();
            //EntityData entityData = new EntityData();

            for(int i = 0; i < mapData.Length; i++)
            {
                if(currentLine != (int)LineType.GameType)
                {
                    if (mapData[i] == '\n')
                    {
                        if (currentLine == (int)LineType.Theme)
                        {
                            SetTheme(currData);
                            currData = "";
                        }
                        else if (currentLine == (int)LineType.Name)
                        {
                            SetLevelName(currData);
                            currData = "";
                        }
                        else if (currentLine == (int)LineType.Width)
                        {
                            SetLevelWidth(currData);
                            currData = "";
                        }
                        else if (currentLine == (int)LineType.Height)
                        {
                            SetLevelHeight(currData);
                            currData = "";
                            InitializeMap();
                        }

                        currentLine++;
                        continue;
                    }

                    currData += mapData[i];
                }
                else 
                {
                    // Ignore newlines
                    if (mapData[i] == '\n')
                        continue;

                    if(inDataField)
                    {
                        if(mapData[i] == ']')
                        {
                            // End of data field found
                            // Set data field info and search for next data field
                            if(dataType == DataType.Tile)
                            {
                                int x = tileData.XPos;
                                int y = tileData.YPos;

                                Tile t = Tile.GetTile(tileData.TileType.ToString());
                                t.Solid = tileData.Solid;
                                t.Position = new Vector2(tileData.XPos, tileData.YPos);
                                t.AllowedToDrawDebug = true;

                                SetTile(x, y, t);
                            }

                            inDataField = false;
                            dataType = DataType.NotSet;
                            continue;
                        }
                        else if(dataType == DataType.NotSet)
                        {
                            if(ATools.ContainsAtRange(mapData, "T=\"", i, i + 3))
                            {
                                int len = ATools.GetNextChar(mapData, '\"', i + 3);
                                string type = "";

                                for(int n = i + 3; n < len; n++)
                                {
                                    type += mapData[n];
                                }

                                if(type == "TILE")
                                {
                                    dataType = DataType.Tile;
                                    tileData = new TileData();
                                    i = len;
                                }
                            }
                        }
                        else if(dataType == DataType.Tile)
                        {
                            // Check for TileType
                            if(ATools.ContainsAtRange(mapData, "TileType=\"", i, i + 10))
                            {
                                int len = ATools.GetNextChar(mapData, '\"', i + 10);
                                string tileType = "";

                                for (int n = i + 10; n < len; n++)
                                {
                                    tileType += mapData[n];
                                }

                                tileData.TileType = (Map.TileType)StringToInt(tileType, string.Format("TileData at index {0}", i));
                                i = len;
                            }
                            // Check for Solid
                            else if (ATools.ContainsAtRange(mapData, "Solid=\"", i, i + 7))
                            {
                                int len = ATools.GetNextChar(mapData, '\"', i + 7);
                                string solid = "";

                                for (int n = i + 7; n < len; n++)
                                {
                                    solid += mapData[n];
                                }

                                int bol = StringToInt(solid, string.Format("TileData at index {0}", i));

                                if(bol != 0 && bol != 1)
                                {
                                    throw new Exception("Corrupt File Data!\nTileData at index {0}, tried to convert int to bool.");
                                }

                                tileData.Solid = Convert.ToBoolean(bol);
                                i = len;
                            }
                            // Check for XPos
                            else if (ATools.ContainsAtRange(mapData, "XPos=\"", i, i + 6))
                            {
                                int len = ATools.GetNextChar(mapData, '\"', i + 6);
                                string xpos = "";

                                for (int n = i + 6; n < len; n++)
                                {
                                    xpos += mapData[n];
                                }

                                tileData.XPos = StringToInt(xpos, string.Format("TileData at index {0}", i));
                                i = len;
                            }
                            // Check for YPos
                            else if (ATools.ContainsAtRange(mapData, "YPos=\"", i, i + 6))
                            {
                                int len = ATools.GetNextChar(mapData, '\"', i + 6);
                                string ypos = "";

                                for (int n = i + 6; n < len; n++)
                                {
                                    ypos += mapData[n];
                                }

                                tileData.YPos = StringToInt(ypos, string.Format("TileData at index {0}", i));
                                i = len;
                            }
                        }
                    }
                    else
                    {
                        // Ignore all data until beginning of data field is found '['
                        if(mapData[i] == '[')
                        {
                            inDataField = true;
                            continue;
                        }
                    }
                }
            }

            ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;
            gs.Player.Position = PlayerSpawn;
            gs.Player.AllowMovement = true;
            gs.camera.LevelWidth = this.Width * Tile.WIDTH;
            gs.camera.LevelHeight = this.Height * Tile.HEIGHT;
            GenerateNewBackground();

            CurrentLevel = this;

            if (spawnPointFound == false)
            {
                PlayerSpawn = Vector2.Zero;
            }
        }

        private void InitializeMap()
        {
            tile = new Tile[Width, Height];

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    SetEmptyTile(x, y);
                }
            }
        }

        private void SetTheme(string theme)
        {
            int val;
            bool success = int.TryParse(theme, out val);

            if (success)
                Theme = (Theme)val;
            else
                throw new Exception(string.Format("Corrupt map data!\nTried to set Theme '{0}'!", theme));
        }

        private void SetLevelWidth(string width)
        {
            int val;
            bool success = int.TryParse(width, out val);

            if (success)
                Width = val;
            else
                throw new Exception(string.Format("Corrupt map data!\nTried to set Width '{0}'!", width));
        }

        private void SetLevelHeight(string height)
        {
            int val;
            bool success = int.TryParse(height, out val);

            if (success)
                Height = val;
            else
                throw new Exception(string.Format("Corrupt map data!\nTried to set Height '{0}'!", height));
        }

        private int StringToInt(string str, string senderData)
        {
            int val;
            bool success = int.TryParse(str, out val);

            if (success)
                return val;
            else
                throw new Exception(string.Format("Corrupt map data!\n{0}", senderData));
        }

        private void SetLevelName(string levelName)
        {
            Name = levelName;
        }

        private enum LineType
        {
            Theme = 0,
            Name,
            Width,
            Height,
            GameType
        };

        public void CreateNewLevel(Theme theme, int width, int height, string name)
        {
            tile = new Tile[width, height];
            this.Width = width;
            this.Height = height;
            this.Theme = theme;
            this.Name = name;
            this.PlayerSpawn = Vector2.Zero;
            CurrentLevel = this;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    SetEmptyTile(x, y);
                }
            }

            ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;
            gs.RemoveAllEntities();
            gs.RemoveAllGameObjects();
            gs.Player.Position = PlayerSpawn;
            gs.Player.AllowMovement = true;
            gs.camera.LevelWidth = Width * Tile.WIDTH;
            gs.camera.LevelHeight = Height * Tile.HEIGHT;
        }

        public void CreateNewLevel(Theme theme, int width, int height, string name, int waterLevel)
        {
            if (waterLevel < 0)
                waterLevel = 0;
            if (waterLevel > MAX_WATER_LEVEL)
                waterLevel = MAX_WATER_LEVEL;

            ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;
            CreateNewLevel(theme, width, height, name);

            for(int x = 0; x < gs.level.Width; x++)
            {
                for(int y = waterLevel; y >= 0; y--)
                {
                    GameObjects.Water waterObj = new GameObjects.Water(new Vector2(x * Tile.WIDTH, (height - y) * Tile.HEIGHT));
                    if(y == waterLevel)
                    {
                        waterObj.WaterType = GameObjects.WaterType.Top;
                    }

                    gs.AddWater(waterObj);
                }
            }
        }

        public Tile GetTileAt(Vector2 pos)
        {
            if(pos.X < 0 || pos.Y < 0 || pos.X >= Width * Tile.WIDTH || pos.Y >= Height * Tile.HEIGHT)
                return null;

            return tile[(int)pos.X / Tile.WIDTH, (int)pos.Y / Tile.HEIGHT];
        }

        public Vector2 GetWorldPosition(Vector2 position)
        {
            return new Vector2(position.X / Tile.WIDTH, position.Y / Tile.HEIGHT);
        }

        public void SetTile(int x, int y, Tile t)
        {
            Tile _t = new Tile();
            _t.ID = t.ID;
            _t.Position = new Vector2(x, y);
            _t.Texture = t.Texture;
            _t.Solid = t.Solid;
            _t.TileType = t.TileType;

            tile[x / Tile.WIDTH, y / Tile.HEIGHT] = _t;
        }

        public void SetTile(Tile t)
        {
            if(t == null)
                return;

            Tile _t = new Tile();
            _t.ID = t.ID;
            _t.Position = new Vector2((int)(t.Position.X / Tile.WIDTH), (int)(t.Position.Y / Tile.HEIGHT));
            _t.Texture = t.Texture;
            _t.Solid = t.Solid;
            _t.TileType = t.TileType;

            tile[(int)(_t.Position.X), (int)(_t.Position.Y)] = _t;
        }

        public void SetEmptyTile(int x, int y)
        {
            Tile t = Tile.GetTile("Empty");
            t.Position = new Vector2(x * Tile.WIDTH, y * Tile.HEIGHT);
            t.AllowedToDrawDebug = true;

            tile[x, y] = t;
        }

        public bool HasRoomForRectangle(Rectangle rectangleToCheck)
        {
            if(rectangleToCheck.X < 0 || rectangleToCheck.Y < 0 || rectangleToCheck.X + rectangleToCheck.Width > Width * Tile.WIDTH || rectangleToCheck.Y + rectangleToCheck.Height > Height * Tile.HEIGHT)
                return false;

            for (int x = rectangleToCheck.X; x < rectangleToCheck.X + rectangleToCheck.Width; x++)
            {
                for(int y = rectangleToCheck.Y; y < rectangleToCheck.Y + rectangleToCheck.Height; y++)
                {
                    if (tile[x / Tile.WIDTH, y / Tile.HEIGHT].Solid && tile[x / Tile.WIDTH, y / Tile.HEIGHT].Rect.Intersects(rectangleToCheck))
                        return false;
                }
            }

            return true;
        }

        public Vector2 WhereCanIGetTo(Vector2 originalPosition, Vector2 destination, Rectangle boundingRectangle)
        {
            MovementWrapper move = new MovementWrapper(originalPosition, destination, boundingRectangle);

            for (int i = 1; i <= move.NumberOfStepsToBreakMovementInto; i++)
            {
                Vector2 positionToTry = originalPosition + move.OneStep * i;
                Rectangle newBoundary = CreateRectangleAtPosition(positionToTry, boundingRectangle.Width, boundingRectangle.Height);
                if (HasRoomForRectangle(newBoundary)) 
                {
                    move.FurthestAvailableLocationSoFar = positionToTry; 
                }
                else
                {
                    if (move.IsDiagonalMove)
                    {
                        move.FurthestAvailableLocationSoFar = CheckPossibleNonDiagonalMovement(move, i);
                    }
                    break;
                }
            }
            return move.FurthestAvailableLocationSoFar;
        }

        private Rectangle CreateRectangleAtPosition(Vector2 positionToTry, int width, int height)
        {
            return new Rectangle((int)positionToTry.X, (int)positionToTry.Y, width, height);
        }

        private Vector2 CheckPossibleNonDiagonalMovement(MovementWrapper wrapper, int i)
        {
            if (wrapper.IsDiagonalMove)
            {
                int stepsLeft = wrapper.NumberOfStepsToBreakMovementInto - (i - 1);

                Vector2 remainingHorizontalMovement = wrapper.OneStep.X * Vector2.UnitX * stepsLeft;
                wrapper.FurthestAvailableLocationSoFar =
                    WhereCanIGetTo(wrapper.FurthestAvailableLocationSoFar, wrapper.FurthestAvailableLocationSoFar + remainingHorizontalMovement, wrapper.BoundingRectangle);

                Vector2 remainingVerticalMovement = wrapper.OneStep.Y * Vector2.UnitY * stepsLeft;
                wrapper.FurthestAvailableLocationSoFar =
                    WhereCanIGetTo(wrapper.FurthestAvailableLocationSoFar, wrapper.FurthestAvailableLocationSoFar + remainingVerticalMovement, wrapper.BoundingRectangle);
            }

            return wrapper.FurthestAvailableLocationSoFar;
        }

        private Color[,] TextureToColor(Texture2D texture)
        {
            Color[] col = new Color[texture.Width * texture.Height];
            texture.GetData(col);

            Color[,] data = new Color[texture.Width, texture.Height];

            for(int i = 0; i < texture.Width; i++)
            {
                for(int j = 0; j < texture.Height; j++)
                {
                    data[i, j] = col[i + j * texture.Width];
                }
            }

            return data;
        }
    }
}
