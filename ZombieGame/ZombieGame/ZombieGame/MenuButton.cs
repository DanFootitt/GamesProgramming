using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieGame
{
    class MenuButton
    {
        Texture2D activeText;
        Texture2D normText;
        Texture2D glowText;
        Rectangle rec;


        public MenuButton(Texture2D glowTexture, Texture2D normTexture, Vector2 position)
        {
            this.glowText = glowTexture;
            this.normText = normTexture;

            this.rec = new Rectangle((int)position.X, (int)position.Y, normTexture.Width, normTexture.Height);
            this.activeText = normText;
        }

        public void Update(Rectangle target)
        {
            if (Intersects(target)) activeText = glowText;
            else activeText = normText;
        }

        public bool Intersects(Rectangle target)
        {
            if (this.rec.Intersects(target)) return true;
            else return false;
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Begin();
            spriteBatch.Draw(activeText, this.rec, Color.White);
            spriteBatch.End();
        }
    }
}
