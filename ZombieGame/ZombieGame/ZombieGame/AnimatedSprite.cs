using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ZombieGame
{
    public class AnimatedSprite
    {
        public Texture2D Texture { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        private int currentFrame;
        private int totalFrames;
        private float elapsedFrameTime;
        private float frameTime;

        public AnimatedSprite(Texture2D texture, int rows, int columns, float frametime)
        {
            Texture = texture;
            Rows = rows;
            frameTime = frametime;
            Columns = columns;
            currentFrame = 0;
            totalFrames = Rows * Columns;
        }

        public void Update(GameTime gameTime)
        {
            

            elapsedFrameTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (elapsedFrameTime > frameTime)
            {
                // Advance the frame index; looping or clamping as appropriate.
                currentFrame++;
                if (currentFrame == totalFrames)
                    currentFrame = 0;
                elapsedFrameTime = 0.0f;
            }
            
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location, float rot)
        {
            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = (int)((float)currentFrame / (float)Columns);
            int column = currentFrame % Columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

            spriteBatch.Begin();
            spriteBatch.Draw(Texture, new Vector2(destinationRectangle.X, destinationRectangle.Y), sourceRectangle, Microsoft.Xna.Framework.Color.White, rot, new Vector2(width / 2, height / 2), 2.5f, SpriteEffects.None, 0);
            spriteBatch.End();
        }

    }
}
