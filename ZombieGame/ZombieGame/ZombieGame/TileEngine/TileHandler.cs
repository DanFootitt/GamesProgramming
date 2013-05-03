using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ZombieGame
{
    public static class TileHandler
    {
        public static List<Enemy> getEnemyLayout(ContentManager content, string filename)
        {
            TileLayer tLayer = TileLayer.FromFile(content, filename);
            List<Enemy> enemyList = new List<Enemy>();

            int spawnMapWidth = tLayer.map.GetLength(1);
            int spawnMapHeight = tLayer.map.GetLength(0);

            for (int x = 0; x < spawnMapWidth; x++)
            {
                for (int y = 0; y < spawnMapHeight; y++)
                {
                    int textIndex = tLayer.map[y, x];

                    if (textIndex == 0)
                    {
                        continue;
                    }

                    if (textIndex == 1)
                    {
                        Enemy e = new Enemy(tLayer.tileTextures[textIndex - 1], new Vector2(x * tLayer.TileWidth, y * tLayer.TileHeight), EnemyType.normal, content);
                        enemyList.Add(e);
                    }

                    if (textIndex == 2)
                    {
                        Enemy e = new Enemy(tLayer.tileTextures[textIndex - 1], new Vector2(x * tLayer.TileWidth, y * tLayer.TileHeight), EnemyType.fast, content);
                        enemyList.Add(e);
                    }

                    if (textIndex == 3)
                    {
                        Enemy e = new Enemy(tLayer.tileTextures[textIndex - 1], new Vector2(x * tLayer.TileWidth, y * tLayer.TileHeight), EnemyType.projectile, content);
                        enemyList.Add(e);
                    }

                    if (textIndex == 4)
                    {
                        Enemy e = new Enemy(tLayer.tileTextures[textIndex - 1], new Vector2(x * tLayer.TileWidth, y * tLayer.TileHeight), EnemyType.boss, content);
                        enemyList.Add(e);
                    }


                }
            }

            return enemyList;
        }

        public static List<TileObject> getTileObjectLayout(ContentManager content, string filename)
        {
            TileLayer tLayer = TileLayer.FromFile(content, filename);
            List<TileObject> objList = new List<TileObject>();

            int spawnMapWidth = tLayer.map.GetLength(1);
            int spawnMapHeight = tLayer.map.GetLength(0);

            for (int x = 0; x < spawnMapWidth; x++)
            {
                for (int y = 0; y < spawnMapHeight; y++)
                {
                    int textIndex = tLayer.map[y, x];

                    ObjectType objtype = ObjectType.NORMAL;

                    if (textIndex == 0)
                    {
                        continue;
                    }

                    if (textIndex == 1)
                    {
                        objtype = ObjectType.CRATE;
                    }

                    if (textIndex == 2)
                    {
                        objtype = ObjectType.PICKUP;
                    }

                    if (textIndex == 3)
                    {
                        objtype = ObjectType.COLLISION;
                    }

                    if (textIndex == 4)
                    {
                        objtype = ObjectType.COLLISION;
                    }

                    if (textIndex == 5)
                    {
                        objtype = ObjectType.COLLISION;
                    }

                    if (textIndex == 6)
                    {
                        objtype = ObjectType.DESTRUCTIBLE;
                    }

                    TileObject tobj = new TileObject(tLayer.tileTextures[textIndex - 1], objtype, new Vector2(x * tLayer.TileWidth, y * tLayer.TileHeight));
                    objList.Add(tobj);

                }
            }

            return objList;
        }

    }

       
}
