using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.IO.IsolatedStorage;

namespace ZombieGame
{
    class Enemy
    {
        public Texture2D enemyTexture;
        public Vector2 _enemyPos;
        float _enemyRot;
        bool intersectsTarget;
        Rectangle enemyRec;

        public Enemy(Texture2D enemyTexture, Vector2 enemyPos) {
            this.enemyTexture = enemyTexture;
            this._enemyPos = enemyPos;
            _enemyRot = 0.0f;
            enemyRec = new Rectangle((int)_enemyPos.X, (int)_enemyPos.Y, enemyTexture.Width / 50, enemyTexture.Height / 50);

        }


        public void Update(Rectangle targetRec)
        {
            Vector2 targetPos = new Vector2(targetRec.X, targetRec.Y);
            enemyRec = new Rectangle((int)_enemyPos.X, (int)_enemyPos.Y, enemyTexture.Width / 50, enemyTexture.Height / 50);
            if (!targetRec.Intersects(enemyRec))
            {
                Vector2 direction = _enemyPos - targetPos;
                direction.Normalize();
                _enemyRot = (float)Math.Atan2(direction.Y, direction.X) - (float)(Math.PI * 0.0f);
                _enemyPos -= direction * 1;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(enemyTexture, _enemyPos, null, Microsoft.Xna.Framework.Color.White, _enemyRot, new Vector2(enemyTexture.Width / 50, enemyTexture.Height / 50), 0.5f, SpriteEffects.None, 0);
            spriteBatch.End();
        }

        public bool returnIntersect(Rectangle targetRec) {
            if (targetRec.Intersects(enemyRec)) return true;
            else return false;
        }

    }
}
