using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ZombieGame
{
    public class Camera
    {

        float speed;
        public Vector2 position;
        public Vector2 prevPosition;

        static int screenHeight = 600;
        static int screenWidth = 800;


        public bool isMovingLeft;
        public bool isMovingRight;
        public bool isMovingUp;
        public bool isMovingDown;

        public bool hasMoved;

        public float Speed
        { 
            get {return speed;}
            set { speed = value; }
        }

        public Camera(Vector2 pos) {
            this.position = pos;
        }

        public void Update(Player player, List<TileObject> objList ,TileLayer tLayer) {

            KeyboardState kbs = Keyboard.GetState();

            isMovingLeft = false;
            isMovingRight = false;
            isMovingDown = false;
            isMovingUp = false;

            prevPosition = position;


            //BORDER TOP
            if (player._playerRec.Y <= (200 + player._playerRec.Height) && kbs.IsKeyDown(Keys.W) && !player.collision(new Vector2(0, -3), objList))
            {
                position.Y -= 3;

                if (position.Y < 0)
                {
                    position.Y = 0;
                }
                else
                {
                    isMovingUp = true;
                }
            }
            else
            {
                isMovingUp = false;
            }
            //BOREDER BOTTOM
            if (player._playerRec.Y >= (screenHeight - 200 - player._playerRec.Height) && kbs.IsKeyDown(Keys.S) && !player.collision(new Vector2(0, 3), objList))
            {
                position.Y += 3;

                if (position.Y > tLayer.MapHeight - screenHeight)
                {
                    position.Y = tLayer.MapHeight - screenHeight;
                }
                else
                {
                    isMovingDown = true;
                }
            }
            else
            {
                isMovingDown = false;
            }
            //BORDER RIGHT
            if (player._playerRec.X <= (200 + player._playerRec.Width) && kbs.IsKeyDown(Keys.A) && !player.collision(new Vector2(-3, 0), objList))
            {
                position.X -= 3;

                if (position.X < 0)
                {
                    position.X = 0;

                }

                else
                {
                    isMovingLeft = true;
                }
            }
            else
            {
                isMovingLeft = false;
            }
            //BORDER LEFT
            if (player._playerRec.X >= (screenWidth - 200 - player._playerRec.Width) && kbs.IsKeyDown(Keys.D) && !player.collision(new Vector2(3, 0), objList))
            {
                position.X += 3;

                if (position.X > tLayer.MapWidth - screenWidth)
                {
                    position.X = tLayer.MapWidth - screenWidth;
                }

                else
                {
                    isMovingRight = true;
                }
            }
            else
            {
                isMovingRight = false;
            }

            if (position == prevPosition)
            {
                isMovingLeft = false;
                isMovingDown = false;
                isMovingRight = false;
                isMovingUp = false;
                hasMoved = false;
            }
            else {
                hasMoved = true;
            }
        
        
        }
    }
}
