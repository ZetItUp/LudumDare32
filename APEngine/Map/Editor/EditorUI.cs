using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using APEngine.Entities;
using AtriLib2;

namespace APEngine.Map.Editor
{
    public enum MousePointerState
    {
        Normal,
        Add,
        Remove,
        Settings,
        Move
    };

    public enum MouseObject
    {
        Nothing = 0,
        Tile,
        GameObject,
        Entity
    };

    public class MousePointer
    {
        public Color MouseColor { get; set; }
        public Vector2 Position { get; private set; }
        public Rectangle MouseRect { get; private set; }
        public bool AllowWorldModification { get; set; }

        private MousePointerState _mstate;
        public MousePointerState State 
        {
            get { return _mstate; }
            set
            {
                _mstate = value;

                switch(_mstate)
                {
                    case MousePointerState.Normal:
                        Engine.SetMouseCursor(GameSettings.MOUSE_POINTER_NORMAL);
                        break;
                    case MousePointerState.Add:
                        Engine.SetMouseCursor(GameSettings.MOUSE_POINTER_ADD);
                        break;
                    case MousePointerState.Move:
                        Engine.SetMouseCursor(GameSettings.MOUSE_POINTER_MOVE);
                        break;
                    case MousePointerState.Remove:
                        Engine.SetMouseCursor(GameSettings.MOUSE_POINTER_REMOVE);
                        break;
                    case MousePointerState.Settings:
                        Engine.SetMouseCursor(GameSettings.MOUSE_POINTER_SETTINGS);
                        break;
                    default:
                        Engine.SetMouseCursor(GameSettings.MOUSE_POINTER_NORMAL);
                        break;
                }
            }
        }

        private Editor.MouseObject mouseObject;
        public Editor.MouseObject MouseObject
        {
            get { return mouseObject; }
            set
            {
                mouseObject = value;

                var ui = ScreenManagers.ScreenManager.GameScreen.EditUI;

                switch (mouseObject)
                {
                    case Editor.MouseObject.Nothing:
                        {
                            CurrentGameObject = null;
                            CurrentTile = null;

                            ui.SetSelectedObject(SelectedObject.None);
                        }
                        break;
                    case Editor.MouseObject.Tile:
                        {
                            CurrentGameObject = null;

                            var selectTile = ScreenManagers.ScreenManager.GameScreen.EditUI.SelectedTile;

                            Map.Tile t = new Map.Tile(false);
                            t.ID = selectTile.Tile.ID;
                            t.Texture = selectTile.Tile.Texture;
                            t.TileType = selectTile.Tile.TileType;

                            ui.SetSelectedObject(SelectedObject.Tile);

                            CurrentTile = t;
                        }
                        break;
                    case Editor.MouseObject.GameObject:
                        {
                            CurrentTile = null;

                            var selectObject = ui.SelectedGameObject.GameObject;
                            selectObject.Position = ui.ToolBarPanel.GetWindow<UI.GameObjectButton>(GameSettings.UI_EDITOR_SELECTED_GAME_OBJECT).Position;

                            ui.SetSelectedObject(SelectedObject.GameObject);
                            CurrentGameObject = selectObject.Clone() as GameObjects.GameObject;
                        }
                        break;
                    case Editor.MouseObject.Entity:
                        {
                            // TODO
                        }
                        break;
                    default:
                        {
                            CurrentGameObject = null;
                            CurrentTile = null;

                            ui.SetSelectedObject(SelectedObject.None);
                            break;
                        }
                }
            }
        }

        private float Depth { get; set; }

        Vector2 originalPosition;    // Current tile originalPosition
        Tile currentTile;            // Current tile
        GameObjects.GameObject currentGameObject;

        public void SetTile(Tile t)
        {
            currentTile = t;
            originalPosition = t.Position;
        }

        public void SetGameObject(GameObjects.GameObject gameObject)
        {
            currentGameObject = gameObject;
        }

        public GameObjects.GameObject CurrentGameObject
        {
            get
            {
                if(State == MousePointerState.Move)
                {
                    GameObjects.GameObject tmpObj = currentGameObject;
                    currentGameObject = null;
                    originalPosition = new Vector2(-tmpObj.Position.X, -tmpObj.Position.Y);
                    return tmpObj;
                }
                else
                {
                    return currentGameObject;
                }
            }
            set
            {
                currentGameObject = value;
            }
        }

        public Tile CurrentTile
        {
            get 
            {
                if (State == MousePointerState.Move)
                {
                    // Clear current tile and return the current tile
                    Tile tmpTile = currentTile;
                    currentTile = null;
                    originalPosition = new Vector2(-32, -32);

                    return tmpTile;
                }
                else
                {
                    return currentTile;
                }
            }
            set
            {
                currentTile = value;
            }
        }

        public MousePointer()
        {
            MouseColor = Color.White;
            State = MousePointerState.Normal;
            Depth = 1f;
            AllowWorldModification = true;
        }

        public void LoadContent(ContentManager Content)
        {
            
        }

        public void Update(GameTime gameTime, UI.Window owner)
        {
            ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;

            Position = gs.camera.GetMousePos;
            MouseRect = AMouse.MouseRectangle();

            if(Position.X >= gs.level.GetWidthInPixels - 1)
                Position = new Vector2(gs.level.GetWidthInPixels - 1, Position.Y);

            if(Position.Y >= gs.level.GetHeightInPixels - 1)
                Position = new Vector2(Position.X, gs.level.GetHeightInPixels - 1);

            if(!MouseRect.Intersects(owner.WindowRectangle) && AllowWorldModification && MouseRect.Intersects(Engine.Monitor.ScreenRectangle))
            {
                if(State == MousePointerState.Move)
                {
                    if (MouseObject == Editor.MouseObject.Tile)
                    {
                        if (AMouse.MouseDown(AMouse.MouseButton.Left) && currentTile != null)
                        {
                            gs.EditUI.Saved = false;
                            currentTile.Position = new Vector2((int)(gs.camera.GetMousePos.X / Map.Tile.WIDTH) * Map.Tile.WIDTH, (int)(gs.camera.GetMousePos.Y / Map.Tile.HEIGHT) * Map.Tile.HEIGHT);
                            currentTile.Depth = 0.7f;
                        }
                        else if (AMouse.MousePressed(AMouse.MouseButton.Left) && currentTile == null)
                        {
                            gs.EditUI.Saved = false;
                            SetTile(gs.level.GetTileAt(gs.camera.GetMousePos));
                            gs.level.SetEmptyTile((int)(gs.camera.GetMousePos.X / Map.Tile.WIDTH), (int)(gs.camera.GetMousePos.Y / Map.Tile.HEIGHT));
                        }
                        else if (AMouse.MouseReleased(AMouse.MouseButton.Left) && currentTile != null)
                        {
                            gs.EditUI.Saved = false;
                            if (originalPosition != currentTile.Position && gs.level.GetTileAt(gs.camera.GetMousePos).ID == TileID.None)
                            {
                                int x = (int)(gs.camera.GetMousePos.X / Map.Tile.WIDTH) * Map.Tile.WIDTH;
                                int y = (int)(gs.camera.GetMousePos.Y / Map.Tile.HEIGHT) * Map.Tile.HEIGHT;
                                currentTile.Depth = 0.6f;
                                Tile t = CurrentTile;
                                gs.level.SetTile(x, y, t);
                            }
                            else
                            {
                                int x = (int)originalPosition.X;
                                int y = (int)originalPosition.Y;
                                currentTile.Depth = 0.6f;
                                Tile t = CurrentTile;
                                gs.level.SetTile(x, y, t);
                            }
                        }
                    }
                    else if(MouseObject == Editor.MouseObject.GameObject)
                    {

                    }
                }
                else if(State == MousePointerState.Remove)
                {
                    gs.EditUI.Saved = false;

                    if(AMouse.MouseDown(AMouse.MouseButton.Left) || AMouse.MouseDown(AMouse.MouseButton.Right))
                    {
                        int yp = (int)gs.camera.GetMousePos.Y;
                        int xp = (int)gs.camera.GetMousePos.X;
                        yp = MathHelper.Clamp(yp, 0, gs.level.GetHeightInPixels - 1) / Map.Tile.HEIGHT;
                        xp = MathHelper.Clamp(xp, 0, gs.level.GetWidthInPixels - 1) / Map.Tile.WIDTH;

                        gs.level.SetEmptyTile(xp, yp);
                    }
                }
                else if(State == MousePointerState.Add)
                {
                    if (MouseObject == Editor.MouseObject.Tile)
                    {
                        if (currentTile != null)
                        {
                            gs.EditUI.Saved = false;
                            currentTile.Position = new Vector2((int)(gs.camera.GetMousePos.X / Map.Tile.WIDTH) * Map.Tile.WIDTH, (int)(gs.camera.GetMousePos.Y / Map.Tile.HEIGHT) * Map.Tile.HEIGHT);

                            if (AMouse.MouseDown(AMouse.MouseButton.Left) && gs.level.GetTileAt(currentTile.Position).ID == TileID.None)
                            {
                                currentTile.Depth = 0.6f;
                                gs.level.SetTile((int)CurrentTile.Position.X, (int)CurrentTile.Position.Y, CurrentTile);
                            }
                        }

                        if (AMouse.MouseDown(AMouse.MouseButton.Right))
                        {
                            int yp = (int)gs.camera.GetMousePos.Y;
                            int xp = (int)gs.camera.GetMousePos.X;
                            yp = MathHelper.Clamp(yp, 0, gs.level.GetHeightInPixels - 1) / Map.Tile.HEIGHT;
                            xp = MathHelper.Clamp(xp, 0, gs.level.GetWidthInPixels - 1) / Map.Tile.WIDTH;

                            gs.level.SetEmptyTile(xp, yp);
                        }
                    }
                    else if(MouseObject == Editor.MouseObject.GameObject)
                    {
                        CurrentGameObject.Position = new Vector2((int)(gs.camera.GetMousePos.X / CurrentGameObject.Rect.Width) * CurrentGameObject.Rect.Width, (int)(gs.camera.GetMousePos.Y / CurrentGameObject.Rect.Width) * CurrentGameObject.Rect.Width);
                        
                        if(gs.CanPlaceGameObject(CurrentGameObject.Rect))
                        {
                            CurrentGameObject.DebugColor = GameSettings.DEFAULT_DEBUG_COLOR;
                        }
                        else
                        {
                            CurrentGameObject.DebugColor = GameSettings.DEFAULT_DEBUG_BUSY_COLOR;
                        }

                        if (AMouse.MouseDown(AMouse.MouseButton.Left))
                        {
                            var obj = CurrentGameObject.Clone() as GameObjects.GameObject;
                            obj.Animate = true;

                            gs.AddGameObject(obj);
                        }
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, float UIScale)
        {
            ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, gs.camera.Transform);

            if (MouseObject == Editor.MouseObject.Tile)
            {
                if (currentTile != null && (State != MousePointerState.Normal && State != MousePointerState.Settings))
                {
                    currentTile.Draw(spriteBatch);
                }
            }
            else if(MouseObject == Editor.MouseObject.GameObject)
            {
                if (CurrentGameObject != null && State != MousePointerState.Normal)
                {
                    CurrentGameObject.Draw(spriteBatch);
                }
            }

            spriteBatch.End();
            spriteBatch.Begin();
        }
    }

    public enum SelectedObject
    {
        None = -1,
        Tile = 0,
        GameObject,
        Entity
    };

    public class EditorUI
    {
        private bool visible;
        public bool Visible 
        {
            get { return visible; }
            set
            {
                visible = value;
                MousePointer.State = MousePointerState.Normal;
            }
        }
        public MousePointer MousePointer { get; set; }

        public UI.Panel ToolBarPanel { get; set; }

        private Dictionary<string, UI.Window> _windows;
        public bool Saved { get; set; }

        public void AddWindow(string windowID, UI.Window newWindow)
        {
            if(newWindow.Parent != null)
            {
                newWindow.WindowRectangle = new Rectangle(newWindow.Parent.WindowRectangle.X + newWindow.WindowRectangle.X, newWindow.Parent.WindowRectangle.Y + newWindow.WindowRectangle.Y, newWindow.WindowRectangle.Width, newWindow.WindowRectangle.Height);
            }

            if(!newWindow.Initialized)
                newWindow.LoadContent(ScreenManagers.ScreenManager.Content);

            _windows.Add(windowID, newWindow);
        }

        public void AddWindowNOLC(string windowID, UI.Window newWindow)
        {
            _windows.Add(windowID, newWindow);
        }

        public T GetWindow<T>(string windowID) where T: UI.Window
        {
            UI.Window window;

            if(_windows.TryGetValue(windowID, out window))
            {
                return (T)window;
            }
            else
            {
                throw new Exception(string.Format("Window {0} was not found!", windowID));
            }
        }

        public bool IsWindowsVisible()
        {
            if (_windows.Count > 0)
                return true;
            else
                return false;
        }

        public void ShowWindow(string windowID)
        {
            if (_windows.ContainsKey(windowID))
                _windows[windowID].Visible = true;
        }

        public void HideWindow(string windowID)
        {
            if(_windows.ContainsKey(windowID))
                _windows[windowID].Visible = false;
        }

        public void ShowChildWindow(string windowID)
        {
            ToolBarPanel.ShowWindow(windowID);
        }

        public void HideChildWindow(string windowID)
        {
            ToolBarPanel.HideWindow(windowID);
        }

        private SelectedObject selObj;
        public void SetSelectedObject(SelectedObject obj)
        {
            var wndObjSel = ToolBarPanel.GetWindow<UI.GameObjectButton>(GameSettings.UI_EDITOR_SELECTED_GAME_OBJECT);
            var wndTileSel = ToolBarPanel.GetWindow<UI.TileButton>(GameSettings.UI_EDITOR_SELECTED_TILE_BUTTON);
            selObj = obj;

            if(obj == SelectedObject.Entity)
            {

            }
            else if(obj == SelectedObject.GameObject)
            {
                wndObjSel.Visible = true;
                wndTileSel.Visible = false;
            }
            else if(obj == SelectedObject.Tile)
            {
                wndObjSel.Visible = false;
                wndTileSel.Visible = true;
            }
            else
            {
                wndObjSel.Visible = false;
                wndTileSel.Visible = true;
            }
        }

        public bool IsWindowsVisible(UI.Window exceptionWindow)
        {
            foreach(KeyValuePair<string, UI.Window> wnd in _windows)
            {
                

            }

            return false;
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

        public bool RemoveWindow(UI.Window window)
        {
            var _wnd = _windows.First(wnd => wnd.Value == window);
            _windows.Remove(_wnd.Key);

            return false;
        }

        public EditorUI(float UIScale)
        {
            MousePointer = new MousePointer();
            Visible = false;
            Saved = true;
            ToolBarPanel = new UI.Panel(new Rectangle(0, Engine.Monitor.Height - 50, Engine.Monitor.Width, 50));
        }

        public void LoadContent(ContentManager Content)
        {
            MousePointer.LoadContent(Content);
            ToolBarPanel.LoadContent(Content);

            _windows = new Dictionary<string, UI.Window>();

            ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;

            UI.Button bNew = new UI.Button(new Rectangle(64, 8, 32, 32));
            bNew.LoadContent(Content);
            bNew.Parent = ToolBarPanel;
            bNew.ButtonImage = Content.Load<Texture2D>("Data\\GFX\\Editor\\New");
            bNew.Clicked += bNew_Clicked;
            ToolBarPanel.AddWindow("btnNew", bNew);

            UI.Label title = new UI.Label(new Rectangle(4, 10, 40, 20));
            title.LoadContent(Content);
            title.Text = "Menu";
            title.Parent = ToolBarPanel;
            ToolBarPanel.AddWindow("lblTitle", title);

            UI.Label lblName = new UI.Label(new Rectangle(ToolBarPanel.WindowRectangle.Width / 2 - 40, 10, 80, 20));
            lblName.LoadContent(Content);
            lblName.Text = gs.level.Name;
            lblName.Parent = ToolBarPanel;
            lblName.WindowClicked += lblName_WindowClicked;
            ToolBarPanel.AddWindow("lblName", lblName);

            UI.Button bOpen = new UI.Button(new Rectangle(98, 8, 32, 32));
            bOpen.LoadContent(Content);
            bOpen.Parent = ToolBarPanel;
            bOpen.ButtonImage = Content.Load<Texture2D>("Data\\GFX\\Editor\\Open");
            bOpen.Clicked += bOpen_Clicked;
            ToolBarPanel.AddWindow("btnOpen", bOpen);

            UI.Button bSave = new UI.Button(new Rectangle(132, 8, 32, 32));
            bSave.LoadContent(Content);
            bSave.Parent = ToolBarPanel;
            bSave.ButtonImage = Content.Load<Texture2D>("Data\\GFX\\Editor\\Save");
            bSave.Clicked += bSave_Clicked;
            ToolBarPanel.AddWindow("btnSave", bSave);

            UI.Button bBuild = new UI.Button(new Rectangle(172, 8, 32, 32));
            bBuild.LoadContent(Content);
            bBuild.Parent = ToolBarPanel;
            bBuild.ButtonImage = Content.Load<Texture2D>("Data\\GFX\\Editor\\Build");
            bBuild.Clicked += bBuild_Clicked;
            ToolBarPanel.AddWindow("btnBuild", bBuild);

            UI.Button bDelete = new UI.Button(new Rectangle(206, 8, 32, 32));
            bDelete.LoadContent(Content);
            bDelete.Parent = ToolBarPanel;
            bDelete.ButtonImage = Content.Load<Texture2D>("Data\\GFX\\Editor\\Delete");
            bDelete.Clicked += bDelete_Clicked;
            ToolBarPanel.AddWindow("btnDelete", bDelete);

            UI.Button bMove = new UI.Button(new Rectangle(240, 8, 32, 32));
            bMove.LoadContent(Content);
            bMove.Parent = ToolBarPanel;
            bMove.ButtonImage = Content.Load<Texture2D>("Data\\GFX\\Editor\\Move");
            bMove.Clicked += bMove_Clicked;
            ToolBarPanel.AddWindow("btnMove", bMove);

            UI.Button bSettings = new UI.Button(new Rectangle(Engine.Monitor.Width - 40, 8, 32, 32));
            bSettings.LoadContent(Content);
            bSettings.Parent = ToolBarPanel;
            bSettings.ButtonImage = Content.Load<Texture2D>("Data\\GFX\\Editor\\CogWheel");
            bSettings.Clicked += bSettings_Clicked;
            ToolBarPanel.AddWindow("bSettings", bSettings);

            Map.Tile t = new Tile();
            t.ID = Map.TileID.Empty;
            t.Texture = Content.Load<Texture2D>("Data\\GFX\\Editor\\EmptyTile");
            t.AllowedToDrawDebug = false;

            UI.TileButton tbSelectedTile = new UI.TileButton(new Rectangle(Engine.Monitor.Width - 90, 8, 32, 32));
            tbSelectedTile.Tile = t;
            tbSelectedTile.Parent = ToolBarPanel;
            tbSelectedTile.LoadContent(Content);
            tbSelectedTile.Clicked += tbSelectedTile_Clicked;
            ToolBarPanel.AddWindow(GameSettings.UI_EDITOR_SELECTED_TILE_BUTTON, tbSelectedTile);

            var gob = GameObjects.GameObject.GetEmptyGameObject(new Vector2(Engine.Monitor.Width - 90, ToolBarPanel.WindowRectangle.Y + 8));
            gob.AllowedToDrawDebug = false;

            UI.GameObjectButton gobSelectedObject = new UI.GameObjectButton(new Rectangle(Engine.Monitor.Width - 90, ToolBarPanel.WindowRectangle.Y + 8, 32, 32));
            gobSelectedObject.GameObject = gob;
            gobSelectedObject.Parent = ToolBarPanel;
            gobSelectedObject.LoadContent(Content);
            gobSelectedObject.Visible = false;
            gobSelectedObject.Clicked += gobSelectedObject_Clicked;
            ToolBarPanel.AddWindow(GameSettings.UI_EDITOR_SELECTED_GAME_OBJECT, gobSelectedObject);
        }

        void gobSelectedObject_Clicked(object sender, EventArgs e)
        {
            var button = sender as UI.GameObjectButton;

            if(button.Visible)
            {
                AddObjectSelectWindow();
                MousePointer.AllowWorldModification = false;
            }
        }

        public UI.TileButton SelectedTile
        {
            get
            {
                return ToolBarPanel.GetWindow<UI.TileButton>(GameSettings.UI_EDITOR_SELECTED_TILE_BUTTON);
            }
        }

        public UI.GameObjectButton SelectedGameObject
        {
            get
            {
                return ToolBarPanel.GetWindow<UI.GameObjectButton>(GameSettings.UI_EDITOR_SELECTED_GAME_OBJECT);
            }
        }

        void tbSelectedTile_Clicked(object sender, EventArgs e)
        {
            var button = sender as UI.TileButton;

            if (button.Visible)
            {
                AddObjectSelectWindow();
                MousePointer.AllowWorldModification = false;
            }
        }

        void bSettings_Clicked(object sender, EventArgs e)
        {
            // Settings window

        }

        void bSave_Clicked(object sender, EventArgs e)
        {
            ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;
            Saved = gs.SaveMap();
        }

        void bOpen_Clicked(object sender, EventArgs e)
        {
            ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;

            if(Saved)
            {
                // Fix this to show proper window
                AddNewMapWindow();
                MousePointer.AllowWorldModification = false;
            }
            else
            {
                DialogResult DR = System.Windows.Forms.MessageBox.Show("Do you want to save changes to the map?", "New Map", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);

                if(DR == DialogResult.Cancel)
                {
                    return;
                }
                else if(DR == DialogResult.No)
                {
                    AddNewMapWindow();
                }
                else if(DR == DialogResult.Yes)
                {
                    gs.SaveMap();
                    AddNewMapWindow();
                }
            }
        }

        private void AddNewMapWindow()
        {
            ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;
            gs.EditUI.MousePointer.CurrentTile = null;
            gs.EditUI.MousePointer.State = MousePointerState.Normal;

            RemoveWindow("wndNewMap");
            RemoveWindow("wndObjectSelect");

            var wnd = new UI.NewMapWindow(new Rectangle(Engine.Monitor.Width / 2 - 500 / 2, Engine.Monitor.Height / 2 - 300 / 2, 500, 300));
            AddWindow("wndNewMap", wnd);
            UI.Window.SetTitle(_windows, "wndNewMap", "New Map");
        }

        private void AddObjectSelectWindow()
        {
            ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;
            gs.EditUI.MousePointer.State = MousePointerState.Normal;

            if (_windows.ContainsKey("wndObjectSelect"))
            {
                RemoveWindow("wndObjectSelect");
            }
            else
            {
                RemoveWindow("wndNewMap");

                var wnd = new UI.ObjectSelectWindow(new Rectangle(Engine.Monitor.Width / 2 - 500 / 2, Engine.Monitor.Height / 2 - 300 / 2, 500, 300));
                wnd.AllowClose = false;
                AddWindow("wndObjectSelect", wnd);

                var w = GetWindow<UI.ObjectSelectWindow>("wndObjectSelect");
                w.FillWithObjects(ScreenManagers.ScreenManager.GameScreen.level.Theme);

                UI.Window.SetTitle(_windows, "wndObjectSelect", "Select Object");
            }
        }

        void lblName_WindowClicked(object sender, EventArgs e)
        {
            RemoveWindow("wndNameInput");

            ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;
            MousePointer.AllowWorldModification = false;

            UI.InputBox wndInput = new UI.InputBox("New Level Name", new Rectangle(Engine.Monitor.Width / 2 - 500 / 2, Engine.Monitor.Height / 2 - 120 / 2, 500, 120));
            AddWindowNOLC("wndNameInput", wndInput);

            var wnd = GetWindow<UI.InputBox>("wndNameInput");
            wnd.Parent = sender as UI.Label;
            wnd.LoadContent(ScreenManagers.ScreenManager.Content);
            wnd.Message = "Enter new level name:";
            wnd.Text = "Empty Level";
            wnd.ReturnStringChanged += wnd_ReturnStringChanged;
        }

        void wnd_ReturnStringChanged(object sender, EventArgs e)
        {
            var wndIB = sender as UI.InputBox;
            var wndLblName = GetWindow<UI.Label>("lblName");

            if (wndIB.ReturnString != "" && wndLblName.Text != wndIB.Text)
            {
                wndLblName.Text = wndIB.ReturnString;
                ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;
                gs.level.Name = wndLblName.Text;
                wndLblName.WindowRectangle = new Rectangle(ToolBarPanel.WindowRectangle.Width / 2 - wndLblName.WindowRectangle.Width / 2, wndLblName.WindowRectangle.Y, wndLblName.WindowRectangle.Width, wndLblName.WindowRectangle.Height);
            }
        }

        public void SetLevelName(string name)
        {
            var wnd = ToolBarPanel.GetWindow<UI.Label>("lblName");

            wnd.Text = name;
            wnd.WindowRectangle = new Rectangle(ToolBarPanel.WindowRectangle.Width / 2 - wnd.WindowRectangle.Width / 2, wnd.WindowRectangle.Y, wnd.WindowRectangle.Width, wnd.WindowRectangle.Height);
        }

        void bNew_Clicked(object sender, EventArgs e)
        {
            ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;
            MousePointer.AllowWorldModification = false;

            if(Saved)
            {
                AddNewMapWindow();
            }
            else
            {
                DialogResult DR = System.Windows.Forms.MessageBox.Show("Do you want to save changes to the map?", "New Map", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);

                if(DR == DialogResult.Cancel)
                {
                    return;
                }
                else if(DR == DialogResult.No)
                {
                    AddNewMapWindow();
                }
                else if(DR == DialogResult.Yes)
                {
                    gs.SaveMap();
                    AddNewMapWindow();
                }
            }
        }

        void bMove_Clicked(object sender, EventArgs e)
        {
            if(MousePointer.State == MousePointerState.Move)
            {
                MousePointer.State = MousePointerState.Normal;
            }
            else
            {
                MousePointer.State = MousePointerState.Move;
            }

            ScreenManagers.ScreenManager.GameScreen.EditUI.MousePointer.CurrentTile = null;
        }

        void bDelete_Clicked(object sender, EventArgs e)
        {
            if(MousePointer.State == MousePointerState.Remove)
            {
                MousePointer.State = MousePointerState.Normal;
            }
            else
            {
                MousePointer.State = MousePointerState.Remove;
            }

            ScreenManagers.ScreenManager.GameScreen.EditUI.MousePointer.CurrentTile = null;
        }

        private void ShowObjectSelectWindow()
        {
            AddObjectSelectWindow();
            MousePointer.AllowWorldModification = false;
        }

        void bBuild_Clicked(object sender, EventArgs e)
        {
            if (MousePointer.MouseObject == MouseObject.Tile)
            {
                var currTile = ScreenManagers.ScreenManager.GameScreen.EditUI.SelectedTile.Tile;
                if (currTile.ID == TileID.None || currTile.ID == TileID.Empty)
                {
                    ShowObjectSelectWindow();
                    return;
                }
            }
            else if(MousePointer.MouseObject == MouseObject.GameObject)
            {
                var currObj = ScreenManagers.ScreenManager.GameScreen.EditUI.SelectedGameObject.GameObject;

                if(currObj == null)
                {
                    ShowObjectSelectWindow();
                    return;
                }
            }

            if(MousePointer.State == MousePointerState.Add)
            {
                MousePointer.State = MousePointerState.Normal;
            }
            else
            {
                MousePointer.State = MousePointerState.Add;
                MousePointer.AllowWorldModification = true;
            }
        }

        public void Update(GameTime gameTime)
        {
            ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;
            
            MousePointer.Update(gameTime, ToolBarPanel);
            ToolBarPanel.Update(gameTime);
            
            if (!IsWindowsVisible())
            {
                if (AInput.KeyPressed(GameSettings.BUILD_KEY))
                {
                    bBuild_Clicked(null, null);
                }
                else if (AInput.KeyPressed(GameSettings.MOVE_KEY))
                {
                    bMove_Clicked(null, null);
                }
                else if (AInput.KeyPressed(GameSettings.REMOVE_KEY))
                {
                    bDelete_Clicked(null, null);
                }
                else if (AInput.KeyPressed(GameSettings.TEXTURE_SELECT_KEY))
                {
                    ShowObjectSelectWindow();
                }

                gs.Player.AllowMovement = true;
            }
            else
            {
                gs.Player.AllowMovement = false;
            }

            foreach(var item in _windows.ToList())
            {
                item.Value.Update(gameTime);

                if(item.Value.DestroyMe)
                {
                    _windows.Remove(item.Key);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, float UIScale)
        {
            if(!Visible)
                return;

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);
            ToolBarPanel.Draw(spriteBatch);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);

            Dictionary<string, UI.Window> tmpList = _windows;
            foreach(KeyValuePair<string, UI.Window> w in tmpList)
            {
                w.Value.Draw(spriteBatch);
            }

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Engine.Monitor.GetTransformationMatrix());
            // DRAW LAST
            MousePointer.Draw(spriteBatch, UIScale);
        }
    }
}
