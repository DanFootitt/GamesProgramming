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

namespace ZombieGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        Texture2D playerTexture;
        Texture2D mouseTexture;
        Texture2D bulletTexture;
        Texture2D enemyTexture;
        Player player;
        Enemy test;
        List<Enemy> enemyList;
        List<Enemy> enemyDeleteList;
        List<Bullet> bulletDeleteList;
        List<Bullet> eBulletDeleteList;
        List<Rectangle> collisionList;
        List<Rectangle> healthList;
        List<CollisionObject> objList;


        //TILE STUFF
        Texture2D tileDirt;
        Texture2D tileGrass;
        Texture2D tileMud;
        Texture2D tileGround;
        Texture2D tileHand;
        Texture2D heart;

        List<Texture2D> tileList = new List<Texture2D>();
        List<Texture2D> tileList2 = new List<Texture2D>();

        Vector2 previousPlayerPos = Vector2.Zero;


        int[,] tileMap = new int[,]
            {
                {3,3,3,3,3,3,2,2,2,2,3,3,3,3,3,3,2,2,2,3,3,3,1,1,3,2},
                {3,3,3,3,3,3,2,2,2,2,3,3,3,3,3,3,2,2,2,3,3,3,1,1,3,2},
                {3,3,3,3,3,3,2,2,2,2,3,3,3,3,3,3,2,2,2,3,3,3,1,1,3,2},
                {3,3,3,3,3,3,2,2,2,2,3,3,3,3,3,3,2,2,2,3,3,3,1,1,3,2},
                {3,3,3,3,3,3,2,2,2,2,3,3,3,3,3,3,2,2,2,3,3,3,1,1,3,2},
                {3,3,3,3,3,3,2,2,2,2,3,3,3,3,3,3,2,2,2,3,3,3,1,1,3,2},
                {3,3,3,3,3,3,2,2,2,2,3,3,3,3,3,3,2,2,2,3,3,3,1,1,3,2},
                {3,3,3,3,3,3,2,2,2,2,3,3,3,3,3,3,2,2,2,3,3,3,1,1,3,2},
                {3,3,3,3,3,3,2,2,2,2,3,3,3,3,3,3,2,2,2,3,3,3,1,1,3,2},
                {3,3,3,3,3,3,2,2,2,2,3,3,3,3,3,3,2,2,2,3,3,3,1,1,3,2},
                {3,3,3,3,3,3,2,2,2,2,3,3,3,3,3,3,2,2,2,3,3,3,1,1,3,2},
            };

        int[,] objMap = new int[,]
            {
                {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                {-1,-1,2,2,2,-1,-1,-1,-1,-1,-1,2,2,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                {-1,-1,2,2,2,-1,-1,-1,-1,-1,-1,2,2,2,2,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                {-1,-1,2,2,2,-1,-1,-1,0,-1,-1,2,2,2,2,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                {-1,-1,2,2,2,-1,-1,-1,-1,-1,-1,2,2,2,2,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                {-1,-1,2,2,2,-1,-1,-1,-1,-1,-1,2,2,2,2,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                {-1,-1,2,2,2,-1,-1,-1,-1,-1,-1,2,2,2,2,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                {-1,-1,2,2,2,-1,4,-1,-1,-1,-1,2,2,2,2,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,0},
                {-1,-1,2,2,2,-1,-1,-1,-1,-1,-1,2,2,2,2,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,0},
                {-1,-1,2,2,2,-1,-1,-1,-1,-1,-1,3,3,2,2,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,0},
            };

        int[,] spawnMap = new int[,]
            {
                {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                {-1,-1,-1,-1,-1,-1,-1,-1,-1,1,1,1,1,1,-1,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
            };

        int tileWidth = 64;
        int tileHeight = 64;

        Vector2 cameraPosition = Vector2.Zero;
        float cameraSpeed = 5.0f;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            objList = new List<CollisionObject>();

            int tileMapWidth2 = objMap.GetLength(1);
            int tileMapHeight2 = objMap.GetLength(0);

            for (int x = 0; x < tileMapWidth2; x++)
            {
                for (int y = 0; y < tileMapHeight2; y++)
                {
                    int textIndex = objMap[y, x];

                    Texture2D objtext = null;
                    int objtype = 99;

                    if (textIndex == -1)
                    {
                        continue;
                    }

                    if (textIndex == 0)
                    {
                        objtype = (int)objectType.collision;
                        objtext = Content.Load<Texture2D>("crate");
                    }

                    if (textIndex == 1)
                    {   
                        objtype = (int)objectType.destroy;
                        objtext = Content.Load<Texture2D>("1up");
                    }

                    if (textIndex == 2)
                    {
                        objtype = (int)objectType.collision;
                        objtext = Content.Load<Texture2D>("poketree");
                    }

                    if (textIndex == 3)
                    {
                        objtype = (int)objectType.collision;
                        objtext = Content.Load<Texture2D>("poketree2");
                    }

                    if (textIndex == 4)
                    {
                        objtype = (int)objectType.collision;
                        objtext = Content.Load<Texture2D>("truck");
                    }

                    CollisionObject cl = new CollisionObject(objtext, objtype, new Vector2(x * tileWidth,y * tileHeight));
                    objList.Add(cl);

                }
            }

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.

            spriteFont = Content.Load<SpriteFont>("SpriteFont1");

            enemyDeleteList = new List<Enemy>();
            bulletDeleteList = new List<Bullet>();
            eBulletDeleteList = new List<Bullet>();
            playerTexture = Content.Load<Texture2D>("manwalk");
            mouseTexture = Content.Load<Texture2D>("cursor");
            bulletTexture = Content.Load<Texture2D>("bullet2");
            
            enemyTexture = Content.Load<Texture2D>("ZombieGreen");
            //Texture2D enemyTexture2 = Content.Load<Texture2D>("ZombiePink");
            Texture2D enemyTexture3 = Content.Load<Texture2D>("ZombieRed");
            Texture2D enemyTexture4 = Content.Load<Texture2D>("ZombieOrange");
            enemyList = new List<Enemy>();

            int spawnMapWidth = spawnMap.GetLength(1);
            int spawnMapHeight = spawnMap.GetLength(0);

            for (int x = 0; x < spawnMapWidth; x++)
            {
                for (int y = 0; y < spawnMapHeight; y++)
                {
                    int textIndex = spawnMap[y, x];

                    if (textIndex == -1)
                    {
                        continue;
                    }

                    Enemy e = new Enemy(enemyTexture, new Vector2(x * tileWidth, y * tileHeight), Enemy.enemyType.normal);
                    enemyList.Add(e);

                }
            }

            //enemyList.Add(new Enemy(enemyTexture2, new Vector2(400, 400)));
            //enemyList.Add(new Enemy(enemyTexture3, new Vector2(500, 400)));
            enemyList.Add(new Enemy(enemyTexture4, new Vector2(600, 400),Enemy.enemyType.projectile, Content));
            enemyList.Add(new Enemy(enemyTexture3, new Vector2(500, 400), Enemy.enemyType.boss, Content));
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TILE STUFF

            tileDirt = Content.Load<Texture2D>("Tiles/se_free_dirt_texture");
            tileGrass = Content.Load<Texture2D>("Tiles/se_free_grass_texture");
            tileGround = Content.Load<Texture2D>("Tiles/se_free_ground_texture");
            tileMud = Content.Load<Texture2D>("Tiles/se_free_mud_texture");
            

            tileList.Add(tileDirt);
            tileList.Add(tileMud);
            tileList.Add(tileGround);
            tileList.Add(tileGrass);

            Texture2D tileHeart = Content.Load <Texture2D>("1up");
            tileHand = Content.Load<Texture2D>("crate");
            tileList2.Add(tileHand);
            tileList2.Add(tileHeart);
            
            player = new Player(playerTexture, mouseTexture, new Vector2(200, 200), new AnimatedSprite(playerTexture, 1, 2, 0.5f), Content);
            heart = Content.Load<Texture2D>("1up");

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
                
            Vector2 motion = Vector2.Zero;

            KeyboardState kbs = Keyboard.GetState();

            if (kbs.IsKeyDown(Keys.W))
            {
                motion.Y --;
            }

            if (kbs.IsKeyDown(Keys.S))
            {
                motion.Y ++;
            }

            if (kbs.IsKeyDown(Keys.D))
            {
                motion.X ++;
            }

            if (kbs.IsKeyDown(Keys.A))
            {
                motion.X --;
            }


            Vector2 prevCamPos = cameraPosition;

            if (motion != Vector2.Zero) motion.Normalize();
            cameraPosition += motion * cameraSpeed;


            if (cameraPosition.Y < 0) cameraPosition.Y = 0;
            if (cameraPosition.X < 0) cameraPosition.X = 0;

            int screenW = GraphicsDevice.Viewport.Width;
            int screenH = GraphicsDevice.Viewport.Height;

            int tilemapWidth = tileMap.GetLength(1) * tileWidth;
            int tilemapHeight = tileMap.GetLength(0) * tileHeight;

            if (cameraPosition.Y > tilemapHeight - screenH) cameraPosition.Y = tilemapHeight - screenH;
            if (cameraPosition.X > tilemapWidth - screenW) cameraPosition.X = tilemapWidth - screenW;


            // COLLISION
            previousPlayerPos = player._playerPos;
            player.Update(gameTime);
            
            List<CollisionObject> objRemoveList = new List<CollisionObject>();

            foreach (CollisionObject c in objList)
            {
                if (prevCamPos != cameraPosition) c.objPos = c.objPos - (cameraPosition - prevCamPos);
                c.Update();

                if (player._playerRec.Intersects(c.objRec))
                {
                    if (c.objType == (int)objectType.destroy)
                    {
                        if (player._health >= 50) player._health = 100;
                        else player._health += 50;
                        objRemoveList.Add(c);
                    }

                    /*if (c.objType == (int)objectType.collision)
                    {
                        player._playerPos = previousPlayerPos;
                    }*/
                }
            }

            

            foreach (CollisionObject c in objRemoveList)
            {
                objList.Remove(c);
            }

                        
            // COLLISION


            foreach (Enemy e in enemyList)
            {
                if (prevCamPos != cameraPosition) e._enemyPos = e._enemyPos - (cameraPosition - prevCamPos);
                e.Update(player._playerRec, gameTime);

                if (e.returnIntersect(new Rectangle((int)player._playerPos.X, (int)player._playerPos.Y, player._playerTexture.Width/2, player._playerTexture.Height/2))){
                    if (e.canDamage) player._health -=5;
                }

                foreach (Bullet b in e.bulletList) { 
                    if (b.intersectsTarget(player._playerRec))
                    {
                        player._health -= 10;
                        eBulletDeleteList.Add(b);
                    }
                }

                foreach (Bullet b in eBulletDeleteList)
                {
                    e.bulletList.Remove(b);
                }

                foreach (Bullet b in player.bulletList)
                {
                    if (b.intersectsTarget(e.enemyRec))
                    {
                        e.health -= 10;
                        bulletDeleteList.Add(b);

                        if (e.health <= 0)
                        {
                            enemyDeleteList.Add(e);
                            // TO DO : increase score based on enemy type
                            player.score += 10;
                        }
                    }
                }
            }

           


            foreach (Bullet b in bulletDeleteList)
            {
                player.bulletList.Remove(b);
            }

            foreach (Enemy e in enemyDeleteList)
            {
                enemyList.Remove(e);
            }

            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            spriteBatch.Begin();
            

            int tileMapWidth = tileMap.GetLength(1);
            int tileMapHeight = tileMap.GetLength(0);

            for (int x = 0; x < tileMapWidth; x++) 
            {
                for (int y = 0; y < tileMapHeight; y++)
                {
                    int textIndex = tileMap[y, x];
                    Texture2D text = tileList[textIndex];

                    spriteBatch.Draw(text, new Rectangle(x * tileWidth - (int)cameraPosition.X, y * tileHeight - (int)cameraPosition.Y, tileWidth, tileHeight), Color.White);
                }
            }

            spriteBatch.End();

            /*int tileMapWidth2 = objMap.GetLength(1);
            int tileMapHeight2 = objMap.GetLength(0);

            for (int x = 0; x < tileMapWidth2; x++)
            {
                for (int y = 0; y < tileMapHeight2; y++)
                {
                    int textIndex = objMap[y, x];

                    if (textIndex == -1)
                    {
                        continue;
                    }

                    Texture2D text = tileList2[textIndex];

                    spriteBatch.Draw(text, new Rectangle(x * tileWidth - (int)cameraPosition.X, y * tileHeight - (int)cameraPosition.Y, tileWidth, tileHeight), Color.White);
                }
            } */

            foreach (CollisionObject c in objList)
            {
                c.Draw(spriteBatch);
            }

            foreach (Enemy e in enemyList)
            {
                e.Draw(spriteBatch);
            }

            player.Draw(spriteBatch);

            int hx = 10;
            int hy = 50;


            spriteBatch.Begin();

            for (int i = 0; i < player._lives; i++)
            {
                spriteBatch.Draw(heart, new Rectangle(hx, hy, heart.Width / 2, heart.Height / 2), Color.White);
                hx += heart.Width / 2 + 10;
        
            }

            spriteBatch.DrawString(spriteFont, "Score : " + player.score.ToString(), new Vector2(10, 90), Color.White);
            spriteBatch.End();




            base.Draw(gameTime);
        }
    }
}
