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
    public class Bullet
    {
        Texture2D _bulletText;
        public float _bulletRot;
        public Vector2 _bulletPos;
        Vector2 _bulletDir;
        public bool _isOutOfBounds;
        public float scale;

        public Bullet(Texture2D bulletText, Vector2 target, Vector2 origin)
        {
            this._bulletText = bulletText;
            _isOutOfBounds = false;
            _bulletPos = origin;
            _bulletDir = _bulletPos - target;
            _bulletDir.Normalize();
            _bulletRot = (float)Math.Atan2(_bulletDir.Y, _bulletDir.X) + (float)(Math.PI * 0.5f);
            this.scale = 3.0f;
        }
        

        public void Update()
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
            spriteBatch.Draw(_bulletText, _bulletPos, null, Microsoft.Xna.Framework.Color.White, _bulletRot, new Vector2(_bulletText.Width, _bulletText.Height), scale, SpriteEffects.None, 0);
            spriteBatch.End();
            
        }

        public bool intersectsTarget(Rectangle targetRec)
        {

            Rectangle bulletRec = new Rectangle((int)_bulletPos.X, (int)_bulletPos.Y, _bulletText.Width, _bulletText.Height);

            if (targetRec.Intersects(bulletRec))return true;
            else return false;
            
        }

    }
}
