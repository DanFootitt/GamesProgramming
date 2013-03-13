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
        

        //control how often can damage player

        float damageTime;
        float elapsedFrameTime;
        public bool canDamage;
        public bool canAttack;

        public Enemy(Texture2D enemyTexture, Vector2 enemyPos) {
            this.enemyTexture = enemyTexture;
            this._enemyPos = enemyPos;
            _enemyRot = 180.0f;
            enemyRec = new Rectangle((int)_enemyPos.X, (int)_enemyPos.Y, enemyTexture.Width * 3, enemyTexture.Height * 3);
            damageTime = 0.5f;
            elapsedFrameTime = 0;
            canDamage = true;
            canAttack = false;
        }


        public void Update(Rectangle targetRec, GameTime gameTime)
        {
            Vector2 targetPos = new Vector2(targetRec.X, targetRec.Y);
            enemyRec = new Rectangle((int)_enemyPos.X, (int)_enemyPos.Y, enemyTexture.Width * 3, enemyTexture.Height * 3);

            Vector2 diff = _enemyPos - targetPos;

            if (diff.X < 300 && diff.Y < 300)
            {
                canAttack = true;
            }
            else canAttack = false;

            if (!targetRec.Intersects(enemyRec) && canAttack)
            {
                Vector2 direction = _enemyPos - targetPos;
                direction.Normalize();
                _enemyRot = (float)Math.Atan2(direction.Y, direction.X) - (float)(Math.PI * 1.0f);
                _enemyPos -= direction * 1;

            }
            else
            {
                canDamage = false;
            }

            elapsedFrameTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (elapsedFrameTime > damageTime)
            {
                elapsedFrameTime = 0.0f;
                canDamage = true;
            }



        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(enemyTexture, _enemyPos, null, Microsoft.Xna.Framework.Color.White, _enemyRot, new Vector2(enemyTexture.Width / 2, enemyTexture.Height / 2), 3.0f, SpriteEffects.None, 0);
            spriteBatch.End();
        }

        public bool returnIntersect(Rectangle targetRec) {
            if (targetRec.Intersects(enemyRec)) return true;
            else return false;
        }

    }
}
