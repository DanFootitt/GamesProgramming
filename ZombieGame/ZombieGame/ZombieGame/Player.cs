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
    public enum PlayerState { 
        ALIVE,
        DYING
    }

    public enum PlayerMovementState { 
        WALKING,
        IDLE,
        DYING
    }

    public class Player
    {
        public int MAX_SHOTS = 5;

        KeyboardState current, previous;

        public String name;
        public int level;
        public int score;
        public int _lives;
        public int _health;
        public Vector2 _playerPos;
        public Vector2 _playerPrevPos;
        public Texture2D _playerTexture;
        public List<Bullet> bulletList;
        public Rectangle _playerRec;
        public RotatedRectangle rRec;
        public Vector2 velocity;
        public bool isAlive;
        SoundEffect shot;
        
        
        float _playerRot;
        public Mouse _mouse;
        Texture2D _bulletTexture;
        public AnimatedSprite pSprite;
        AnimatedSprite moveSprite;
        AnimatedSprite idleSprite;
        AnimatedSprite dieSprite;
        Texture2D _healthIn;
        Texture2D _healthOut;
        Texture2D die;

        PlayerState pState;
        PlayerMovementState pmState;


        public Player(Texture2D playerTexture, Texture2D mouseTexture, Vector2 playerPos, ContentManager content)
        {
            _mouse = new Mouse(mouseTexture);
            _playerTexture = playerTexture;

            _playerPos = playerPos;
            _lives = 3;
            this.score = 0;
            _health = 100;
            _playerRot = 0;
            
            bulletList = new List<Bullet>();
            
            _playerRec = new Rectangle();

            //Load Textures

            shot = content.Load<SoundEffect>("Sounds\\shot");
            _bulletTexture = content.Load<Texture2D>("bullet2");
            _healthIn = content.Load<Texture2D>("healthbarInside");
            _healthOut = content.Load<Texture2D>("healthbarOutside");
            Texture2D idle = content.Load<Texture2D>("manIdle");
            die = content.Load<Texture2D>("manDie");

            _playerRec = new Rectangle((int)_playerPos.X, (int)_playerPos.Y, playerTexture.Width / 2, playerTexture.Height / 2);
            rRec = new RotatedRectangle(_playerRec, 0.0f);
            velocity = Vector2.Zero;


            dieSprite = new AnimatedSprite(die, AnimatedSprite.AnimType.FLASH, 1, 2, 0.5f, new Vector2(200, 200), 2.5f);
            moveSprite = new AnimatedSprite(_playerTexture, AnimatedSprite.AnimType.LOOP, 1, 2, 0.5f, new Vector2(200, 200), 2.5f);
            idleSprite = new AnimatedSprite(idle, AnimatedSprite.AnimType.LOOP, 1, 1, 0.5f, new Vector2(200, 200), 2.5f);

            pSprite = idleSprite;
            pmState = PlayerMovementState.IDLE;
            isAlive = true;
            level = 1;

        }

        public Player(Texture2D playerTexture, Mouse mouse, Vector2 playerPos, AnimatedSprite animatedSprite, ContentManager content)
        {
            _mouse = mouse;
            _playerTexture = playerTexture;

            _playerPos = playerPos;
            _playerRot = 0;

            bulletList = new List<Bullet>();
            pSprite = animatedSprite;
            _playerRec = new Rectangle();

            //Load Textures
            shot = content.Load<SoundEffect>("Sounds\\shot");
            _bulletTexture = content.Load<Texture2D>("bullet2");

            _playerRec = new Rectangle((int)_playerPos.X, (int)_playerPos.Y, playerTexture.Width / 2, playerTexture.Height / 2);
            rRec = new RotatedRectangle(_playerRec, 0.0f);
            velocity = Vector2.Zero;

            pState = PlayerState.ALIVE;
            pmState = PlayerMovementState.IDLE;

        }

        public void Update(GameTime gameTime, List<TileObject> t, Camera cam)
        {
            
            //Calculate Sprite Rotation
            _playerPrevPos = _playerPos;
            Vector2 direction = _playerPos - _mouse.position;
            direction.Normalize();
            _playerRot = (float)Math.Atan2(direction.Y, direction.X) + (float)(Math.PI * 0.5f);

            previous = current;
            current = Keyboard.GetState();


            if (isAlive)
            {
                if (current.IsKeyDown(Keys.W) && !collision(new Vector2(0, -3), t) && !cam.isMovingUp)
                {
                    if (_playerPos.Y >= 0 + _playerRec.Height / 2)
                        _playerPos.Y -= 3;
                }


                if (current.IsKeyDown(Keys.S) && !collision(new Vector2(0, 3), t) && !cam.isMovingDown)
                {
                    if (_playerPos.Y <= 600 - _playerRec.Height / 2)
                        _playerPos.Y += 3;
                }

                if (current.IsKeyDown(Keys.A) && !collision(new Vector2(-3, 0), t) && !cam.isMovingLeft)
                {
                    if (_playerPos.X >= 0 + _playerRec.Width / 2)
                        _playerPos.X -= 3;
                }

                if (current.IsKeyDown(Keys.D) && !collision(new Vector2(3, 0), t) && !cam.isMovingRight)
                {
                    if (_playerPos.X <= 800 - _playerRec.Width / 2)
                        _playerPos.X += 3;
                }
            }
            
            if (_mouse.newLeftClick)
            {
                if (bulletList.Count < MAX_SHOTS)
                {
                    bulletList.Add(new Bullet(_bulletTexture, _mouse.position, this._playerPos));
                    shot.Play();
                }
            }

            if (bulletList != null ) {

               
            foreach (Bullet b in bulletList)
            {
                b.Update();
            }

            removeBullets(bulletList);
            }


            if (_health <= 0)
            {
                isAlive = false;
                pSprite = dieSprite;
            }

            if (_playerPos == _playerPrevPos && !cam.isMovingUp && !cam.isMovingDown && !cam.isMovingLeft && !cam.isMovingRight && isAlive) pmState = PlayerMovementState.IDLE;
            else if  (_playerPos != _playerPrevPos && isAlive) pmState = PlayerMovementState.WALKING;

            if (pmState == PlayerMovementState.IDLE && isAlive)
            {
                pSprite = idleSprite;
            }
            if (pmState == PlayerMovementState.WALKING && isAlive)
            {
                pSprite = moveSprite;
            }



            if (!isAlive & pSprite.isFinished)
            {
                dieSprite = new AnimatedSprite(die, AnimatedSprite.AnimType.FLASH, 1, 2, 0.5f, new Vector2(200, 200), 2.5f);
                _playerPos = new Vector2(150, 150);
                _lives--;
                isAlive = true;
                _health = 100;
                pState = PlayerState.ALIVE;
                pmState = PlayerMovementState.IDLE;
            }
            

            _playerRec.X = (int)_playerPos.X;
            _playerRec.Y = (int)_playerPos.Y;
            pSprite.Update(gameTime);
            _mouse.Update();
        }

        public void UpdateSimple(GameTime gameTime)
        {

            //Calculate Sprite Rotation
            _playerPrevPos = _playerPos;
            Vector2 direction = _playerPos - _mouse.position;
            direction.Normalize();
            _playerRot = (float)Math.Atan2(direction.Y, direction.X) + (float)(Math.PI * 0.5f);

            previous = current;
            current = Keyboard.GetState();

                if (current.IsKeyDown(Keys.W))
                {
                    if (_playerPos.Y >= 50 + _playerRec.Height)
                        _playerPos.Y -= 3;
                }


                if (current.IsKeyDown(Keys.S))
                {
                    if (_playerPos.Y <= 550 - _playerRec.Height)
                        _playerPos.Y += 3;
                }

                if (current.IsKeyDown(Keys.A))
                {
                    if (_playerPos.X >= 50 + _playerRec.Width)
                        _playerPos.X -= 3;
                }

                if (current.IsKeyDown(Keys.D))
                {
                    if (_playerPos.X <= 750 - _playerRec.Width)
                        _playerPos.X += 3;
                }
  
            if (_mouse.newLeftClick)
            {
                if (bulletList.Count < MAX_SHOTS)
                {
                    bulletList.Add(new Bullet(_bulletTexture, _mouse.position, this._playerPos));
                    shot.Play();
                }
            }

            if (bulletList != null)
            {

                foreach (Bullet b in bulletList)
                {
                    b.Update();
                }

                removeBullets(bulletList);
            }


            if (_health <= 0)
            {
                _lives--;
                _playerPos = new Vector2(100, 100);
                _health = 100;
            }

            _playerRec.X = (int)_playerPos.X;
            _playerRec.Y = (int)_playerPos.Y;
            pSprite.Update(gameTime);
            _mouse.Update();
        }


        public bool collision(Vector2 pos, List<TileObject> tObj)
        {
            Rectangle r = _playerRec;
            r.X += (int)pos.X;
            r.Y += (int)pos.Y;

            foreach (TileObject t in tObj)
            { 
                if(r.Intersects(t.objRec) && t.objType != ObjectType.PICKUP)
                {
                    return true;
                }
            }

            return false;
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


            pSprite.Draw(spriteBatch, _playerPos, _playerRot);

                spriteBatch.Begin();
                spriteBatch.Draw(_healthIn, new Rectangle(10, 10, _healthIn.Width / 2, _healthIn.Height / 2), Color.Gray);
                spriteBatch.Draw(_healthIn, new Rectangle(10, 10, (int)(_healthIn.Width / 2 * ((double)_health / 100)), _healthIn.Height / 2), Color.Red);
                spriteBatch.Draw(_healthOut, new Rectangle(10, 10, _healthOut.Width / 2, _healthOut.Height / 2), Color.White);
                spriteBatch.End();
        }

        public void DrawSimple(SpriteBatch spriteBatch)
        {
            _mouse.Draw(spriteBatch);

            if (bulletList != null)
            {
                foreach (Bullet b in bulletList)
                {
                    b.Draw(spriteBatch);
                }
            }


            pSprite.Draw(spriteBatch, _playerPos, _playerRot);
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
