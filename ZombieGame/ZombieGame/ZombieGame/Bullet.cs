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
    class Bullet
    {
        Texture2D _bulletText;
        float _bulletRot;
        Vector2 _bulletPos;
        Vector2 _bulletDir;
        public bool _isOutOfBounds;

        public Bullet(Texture2D bulletText, Mouse mouse, Player player)
        {
            this._bulletText = bulletText;
            _isOutOfBounds = false;
            _bulletPos = player._playerPos;
            _bulletDir = _bulletPos - mouse.position;
            _bulletDir.Normalize();
            _bulletRot = (float)Math.Atan2(_bulletDir.Y, _bulletDir.X) + (float)(Math.PI * 0.5f);
        }
        

        public void Update(Player player, Mouse mouse)
        {

            if (_bulletPos.X > 800 || _bulletPos.X < 0)
            {
                _isOutOfBounds = true;
            }
            if (_bulletPos.Y > 800 || _bulletPos.Y < 0)
            {
                _isOutOfBounds = true;
            }

            _bulletPos -= _bulletDir * 7;
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(_bulletText, _bulletPos, null, Microsoft.Xna.Framework.Color.White, _bulletRot, new Vector2(_bulletText.Width, _bulletText.Height), 3.0f, SpriteEffects.None, 0);
            spriteBatch.End();
            
        }

        public bool intersectsEnemy(Enemy enemy)
        {

            Rectangle bulletRec = new Rectangle((int)_bulletPos.X, (int)_bulletPos.Y, _bulletText.Width, _bulletText.Height);
            Rectangle enemyRec = new Rectangle((int)enemy._enemyPos.X, (int)enemy._enemyPos.Y, enemy.enemyTexture.Width, enemy.enemyTexture.Height);

            if (enemyRec.Intersects(bulletRec))return true;
            else return false;
            
        }
    }
}
