using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace ZombieGame
{
    public class AnimatedSprite
    {

        public enum AnimType
        {
            LOOP,
            PLAY_ONCE,
            FLASH
        }


        public Texture2D Texture { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        private int currentFrame;
        private int totalFrames;
        public float elapsedFrameTime;
        public float frameTime;
        public Vector2 pos;
        public AnimType animType;
        public bool isFinished;
        public Rectangle spriteRec;
        public bool playerDamaged;
        public List<Enemy> damagedEnemies;
        public float scale;
        public int count = 0;

        public AnimatedSprite(Texture2D texture, AnimType at, int rows, int columns, float frametime, Vector2 pos, float s)
        {
            Texture = texture;
            Rows = rows;
            frameTime = frametime;
            Columns = columns;
            currentFrame = 0;
            totalFrames = Rows * Columns;
            this.pos = pos;
            this.animType = at;
            isFinished = false;

            playerDamaged = false;
            damagedEnemies = new List<Enemy>();
            this.scale = s;


            this.spriteRec = new Rectangle((int)pos.X, (int)pos.Y, (texture.Width / columns) * (int)scale, (texture.Height / rows) * (int)scale);
        }

        public void Update(GameTime gameTime)
        {
            

            elapsedFrameTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (elapsedFrameTime > frameTime)
            {
                // Advance the frame index; looping or clamping as appropriate.
                currentFrame++;
                if (currentFrame == totalFrames)
                {
                    if (animType == AnimType.LOOP || animType == AnimType.FLASH)
                    {
                        currentFrame = 0;
                        count++;
                    }
                    else isFinished = true;

                    if (animType == AnimType.FLASH && count >= 3) isFinished = true;
                    
                    
                }

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
            spriteBatch.Draw(Texture, new Vector2(destinationRectangle.X, destinationRectangle.Y), sourceRectangle, Microsoft.Xna.Framework.Color.White, rot, new Vector2(width / 2, height / 2), scale, SpriteEffects.None, 0);
            spriteBatch.End();
        }

    }
}
