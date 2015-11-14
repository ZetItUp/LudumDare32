using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AtriLib2;

namespace APEngine.UI
{
    public class ComboBox : Window
    {
        List<Label> items;
        Label selectedItem;

        private bool ShowList { get; set; }
        private Button dropDownButton { get; set; }
        public string SelectedItem
        {
            get { return selectedItem.Text; }
            set { selectedItem.Text = value; }
        }

        public void AddItem(string text)
        {
            UI.Label lblItem = new UI.Label(new Rectangle(WindowRectangle.X, WindowRectangle.Y, WindowRectangle.Width, 28));
            lblItem.LoadContent(Engine.ContentManager);
            lblItem.Text = text;
            lblItem.Parent = this;
            lblItem.DrawWindow = true;
            lblItem.BackgroundColor = new Color(0, 148, 255);
            items.Add(lblItem);
        }

        public void RemoveItem(int index)
        {
            items.RemoveAt(index);
        }

        public ComboBox(Rectangle windowRectangle)
            : base(windowRectangle)
        {
            items = new List<Label>();
            BackgroundColor = new Color(0, 148, 255);
        }

        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);

            dropDownButton = new UI.Button(new Rectangle(WindowRectangle.X + WindowRectangle.Width - 36, WindowRectangle.Y + 4, 32, 23));
            dropDownButton.Parent = this;
            dropDownButton.LoadContent(Content);
            dropDownButton.ButtonImage = AGraphics.Crop(WindowGraphics.WindowBasics, new Rectangle(3, 0, 9, 6));
            dropDownButton.ButtonColor = new Color(255, 161, 0);
            dropDownButton.Clicked += dropDownButton_Clicked;

            selectedItem = new UI.Label(new Rectangle(WindowRectangle.X, WindowRectangle.Y, WindowRectangle.Width, 28));
            selectedItem.LoadContent(ScreenManagers.ScreenManager.Content);
            selectedItem.Text = "";
            selectedItem.Parent = this;
            selectedItem.TextYOffset = 8;
        }

        void dropDownButton_Clicked(object sender, EventArgs e)
        {
            SortList();

            ShowList = !ShowList;

            if(ShowList)
            {
                dropDownButton.FlipVerticalImage = true;
                AllowActiveChange = false;
            }
            else
            {
                dropDownButton.FlipVerticalImage = false;
                AllowActiveChange = true;
            }
        }

        private void SortList()
        {
            for(int i = 0; i < items.Count; i++)
            {
                items[i].WindowRectangle = new Rectangle(items[i].WindowRectangle.X, WindowRectangle.Y + WindowRectangle.Height + (i * items[i].WindowRectangle.Height), items[i].WindowRectangle.Width, items[i].WindowRectangle.Height);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            dropDownButton.Update(gameTime);

            if(ShowList)
            {
                foreach(var l in items)
                {
                    l.Update(gameTime);

                    if(AMouse.MouseRectangle().Intersects(l.WindowRectangle))
                    {
                        l.BackgroundColor = new Color(0, 50, 90);

                        if(AMouse.MousePressed(AMouse.MouseButton.Left))
                        {
                            selectedItem.Text = l.Text;
                            ShowList = false;
                            dropDownButton.FlipVerticalImage = false;
                            AllowActiveChange = true;
                        }
                    }
                    else
                    {
                        l.BackgroundColor = new Color(0, 148, 255);
                    }
                }
            }

            selectedItem.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if(ShowList)
            {
                foreach(var l in items)
                {
                    l.Draw(spriteBatch);
                }
            }

            selectedItem.Draw(spriteBatch);
            dropDownButton.Draw(spriteBatch);
        }
    }
}
