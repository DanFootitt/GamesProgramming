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
    public class TileLayer
    {
        static int tileWidth = 64;
        static int tileHeight = 64;
 
        public List<Texture2D> tileTextures = new List<Texture2D>();
        public int[,] map;


        //GETS AND SETS

        public int TileWidth 
        {
            get { return tileWidth;}
            set { tileWidth = value; }
        }

        public int TileHeight
        {
            get { return tileHeight; }
            set { tileHeight = value; }
        }

        public int MapWidth
        {
            get { return map.GetLength(1) * tileWidth; }
        }

        public int MapHeight
        {
            get { return map.GetLength(0) * tileHeight; }
        }


        //CONSTRUCTORS

        public TileLayer(int w, int h)
        { 
            map = new int[h,w];
        }

        public TileLayer(int[,] m)
        {
            map = (int[,])m.Clone();
        }

        public static TileLayer FromFile(ContentManager content, string file) {
            
            
            bool readingLayout = false;
            bool readingTextures = false;
            List<String> textureNames = new List<string>();
            List<List<int>> tempLayout = new List<List<int>>();

            using (StreamReader reader = new StreamReader(file))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine().Trim();

                    if (line.Contains("[Textures]"))
                    {
                        readingTextures = true;
                        readingLayout = false;
                    }
                    else if (line.Contains("[Layout]"))
                    {
                        readingTextures = false;
                        readingLayout = true;
                    }
                    else if (readingLayout)
                    {
                        List<int> row = new List<int>();
                        string[] cells = line.Split(' ');

                        foreach (string c in cells)
                        {
                            if (!string.IsNullOrEmpty(c)) row.Add(int.Parse(c));
                        }

                        tempLayout.Add(row);
                    }
                    else if (readingTextures)
                    {
                        textureNames.Add(line);
                    }
                }
            }

            int width = tempLayout[0].Count;
            int height = tempLayout.Count;

            TileLayer tileLayer = new TileLayer(width,height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                { 
                    tileLayer.setCellIndex(x,y, tempLayout[y][x]);
                }
            }

            tileLayer.loadTextures(content, textureNames.ToArray());

            return tileLayer;
        }


        public void setCellIndex(int x, int y, int cellIndex)
        {
            map[y,x] = cellIndex;
        }


        //LOAD

        public void loadTextures(ContentManager content, params string[] textureNames)
        {
            Texture2D text;

            foreach (string textureName in textureNames)
            {
                text = content.Load<Texture2D>(textureName);
                tileTextures.Add(text);
            }
        }

        //DRAW

        public void Draw(SpriteBatch sb, Vector2 c)
        {
            int tileMapWidth = map.GetLength(1);
            int tileMapHeight = map.GetLength(0);

            sb.Begin();

            for (int x = 0; x < tileMapWidth; x++)
            {
                for (int y = 0; y < tileMapHeight; y++)
                {
                    int textIndex = map[y, x];
                    Texture2D text = tileTextures[textIndex];

                    sb.Draw(text, new Rectangle(x * tileWidth - (int)c.X, y * tileHeight - (int)c.Y, tileWidth, tileHeight), Color.White);
                }
            }

            sb.End();

        }
    }
}
