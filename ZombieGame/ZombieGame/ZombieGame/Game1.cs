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


        //TILE STUFF
        Texture2D tileDirt;
        Texture2D tileGrass;
        Texture2D tileMud;
        Texture2D tileGround;

        List<Texture2D> tileList = new List<Texture2D>();

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
            playerTexture = Content.Load<Texture2D>("player");
            mouseTexture = Content.Load<Texture2D>("crosshair");
            bulletTexture = Content.Load<Texture2D>("bullet");
            enemyTexture = Content.Load<Texture2D>("spider");

            enemyList = new List<Enemy>();
            enemyList.Add(new Enemy(enemyTexture, new Vector2(500, 500)));
            enemyList.Add(new Enemy(enemyTexture, new Vector2(500, 10)));
            enemyList.Add(new Enemy(enemyTexture, new Vector2(200, 200)));
            enemyList.Add(new Enemy(enemyTexture, new Vector2(100, 500)));
            enemyList.Add(new Enemy(enemyTexture, new Vector2(200, 10)));
            enemyList.Add(new Enemy(enemyTexture, new Vector2(300, 200)));
            enemyList.Add(new Enemy(enemyTexture, new Vector2(400, 500)));
            enemyList.Add(new Enemy(enemyTexture, new Vector2(500, 10)));
            enemyList.Add(new Enemy(enemyTexture, new Vector2(600, 200)));

            player = new Player(playerTexture, mouseTexture, new Vector2(200, 200), bulletTexture);

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

            // TODO: Add your update logic here

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


            foreach (Enemy e in enemyList)
            {
                e.Update(new Rectangle((int)player._playerPos.X, (int)player._playerPos.Y, player._playerTexture.Width/2, player._playerTexture.Height/2));
                if (e.returnIntersect(new Rectangle((int)player._playerPos.X, (int)player._playerPos.Y, player._playerTexture.Width/2, player._playerTexture.Height/2))){
                    player._health -=1;
                }

                foreach (Bullet b in player.bulletList)
                {
                    if (b.intersectsEnemy(e))
                    {
                        enemyDeleteList.Add(e);
                        bulletDeleteList.Add(b);
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

            player.Update();
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

            spriteBatch.DrawString(spriteFont, "Player Health : " + player._health.ToString(), new Vector2(10, 10), Color.White);

            spriteBatch.End();

            
            foreach (Enemy e in enemyList)
            {
                e.Draw(spriteBatch);
            }
            player.Draw(spriteBatch);

            base.Draw(gameTime);
        }
    }
}
