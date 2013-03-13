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
    class Player
    {
        KeyboardState current, previous;
        int _lives;
        public int _health;
        float _playerRot;
        Mouse _mouse;
        public Vector2 _playerPos;
        public Texture2D _playerTexture;
        Texture2D _bulletTexture;
        public int MAX_SHOTS;
        public List<Bullet> bulletList;
        AnimatedSprite playerSprite;


        public Player(Texture2D playerTexture, Texture2D mouseTexture, Vector2 playerPos, Texture2D bulletText, AnimatedSprite animatedSprite)
        {
            _mouse = new Mouse(mouseTexture);
            _playerTexture = playerTexture;
            _playerPos = playerPos;
            _lives = 3;
            _health = 100;
            _playerRot = 0;
            MAX_SHOTS = 5;
            _bulletTexture = bulletText;
            bulletList = new List<Bullet>();
            playerSprite = animatedSprite;
        }

        public void Update(GameTime gameTime)
        {
            
            //Calculate Sprite Rotation
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
                if(bulletList.Count < MAX_SHOTS) bulletList.Add(new Bullet(_bulletTexture, _mouse, this));
            }

            if (bulletList != null) {

                foreach (Bullet b in bulletList)
                {
                    b.Update(this, _mouse);
                }

                removeBullets(bulletList);
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
