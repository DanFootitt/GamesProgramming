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
    public class Player
    {
        const int MAX_SHOTS = 5;

        KeyboardState current, previous;

        public int score;
        public int _lives;
        public int _health;
        public Vector2 _playerPos;
        public Texture2D _playerTexture;
        public List<Bullet> bulletList;
        public Rectangle _playerRec;
        
        
        float _playerRot;
        Mouse _mouse;
        Texture2D _bulletTexture;
        AnimatedSprite playerSprite;
        Texture2D _healthIn;
        Texture2D _healthOut;


        public Player(Texture2D playerTexture, Texture2D mouseTexture, Vector2 playerPos, AnimatedSprite animatedSprite, ContentManager content)
        {
            _mouse = new Mouse(mouseTexture);
            _playerTexture = playerTexture;

            _playerPos = playerPos;
            _lives = 3;
            this.score = 0;
            _health = 100;
            _playerRot = 0;
            
            bulletList = new List<Bullet>();
            playerSprite = animatedSprite;
            _playerRec = new Rectangle();

            //Load Textures

            _bulletTexture = content.Load<Texture2D>("bullet2");
            _healthIn = content.Load<Texture2D>("healthbarInside");
            _healthOut = content.Load<Texture2D>("healthbarOutside");

        }

        public void Update(GameTime gameTime)
        {
            
            //Calculate Sprite Rotation
            _playerRec = new Rectangle((int)_playerPos.X, (int)_playerPos.Y, 57, 110);
            Vector2 direction = _playerPos - _mouse.position;
            direction.Normalize();
            _playerRot = (float)Math.Atan2(direction.Y, direction.X) + (float)(Math.PI * 0.5f);

            //implement movement - INCLUDE INPUT CLASS
            previous = current;
            current = Keyboard.GetState();

            if (current.IsKeyDown(Keys.W))
            {
                _playerPos.Y -= 3;
            }

            if (current.IsKeyDown(Keys.S))
            {
                _playerPos.Y += 3;
            }

            if (current.IsKeyDown(Keys.A))
            {
                _playerPos.X -= 3;
            }
            if (current.IsKeyDown(Keys.D))
            {
                _playerPos.X += 3;
            }

            if (_mouse.newLeftClick)
            {
                if(bulletList.Count < MAX_SHOTS) bulletList.Add(new Bullet(_bulletTexture, _mouse.position, this._playerPos));
            }

            if (bulletList != null) {

                foreach (Bullet b in bulletList)
                {
                    b.Update();
                }

                removeBullets(bulletList);
            }


            if (_health <= 0) {
                _lives--;
                _playerPos = new Vector2(100, 100);
                _health = 100;
            }

            playerSprite.Update(gameTime);

            _mouse.Update();


        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _mouse.Draw(spriteBatch);

            if (bulletList != null)
            {
                foreach (Bullet b in bulletList)
                {
                    b.Draw(spriteBatch);
                }
            }


            playerSprite.Draw(spriteBatch, _playerPos, _playerRot);

            spriteBatch.Begin();
            spriteBatch.Draw(_healthIn, new Rectangle(10, 10, _healthIn.Width / 2, _healthIn.Height / 2), Color.Gray);
            spriteBatch.Draw(_healthIn, new Rectangle(10, 10, (int)(_healthIn.Width / 2 * ((double)_health / 100)), _healthIn.Height / 2), Color.Red);
            spriteBatch.Draw(_healthOut, new Rectangle(10, 10, _healthOut.Width / 2, _healthOut.Height / 2), Color.White);
            spriteBatch.End();
        }

        public void removeBullets(List<Bullet> list)
        { 
            
            List<Bullet> deleteList = new List<Bullet>();

            foreach (Bullet b in bulletList)
            {
                if (b._isOutOfBounds) deleteList.Add(b);
            }

            foreach (Bullet b in deleteList)
            {
                bulletList.Remove(b); 
            }

        }



    }
}
