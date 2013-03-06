using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ZombieGame
{
    public class Mouse
    {
        MouseState previousState, currentState;
        
        public Texture2D texture
        {
            get;
            set;
        }

        public Rectangle rectangle
        {
            get;
            protected set;
        }

        public Vector2 position
        {
            get;
            protected set;
        }

        public bool leftClick
        {
            get { return currentState.LeftButton == ButtonState.Pressed; }
        }

        public bool newLeftClick 
        {
            get { return currentState.LeftButton == ButtonState.Pressed && previousState.LeftButton == ButtonState.Released;  }
        }

        public bool leftRelease
        {
            get { return !leftClick && previousState.LeftButton == ButtonState.Pressed; }
        }


        // Constructors

        public Mouse() { }

        public Mouse(Texture2D texture) 
        {
            this.texture = texture;
        }

        public Mouse(ContentManager content, string assetName)
        {
            texture = content.Load<Texture2D>(assetName);
        }

        // Update

        public void Update() 
        {
            previousState = currentState;
            currentState = Microsoft.Xna.Framework.Input.Mouse.GetState();

            position = new Vector2(currentState.X, currentState.Y);
            rectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }


        // Draw

        public void Draw(SpriteBatch spriteBatch) 
        {
            spriteBatch.Begin();
            spriteBatch.Draw(texture, position, Color.White);
            spriteBatch.End();
        }

    }
}
