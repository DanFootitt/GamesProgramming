using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZombieGame
{
    public class HighScoreTable
    {
        public String[] names = new String[10];
        public int[] scores = new int[10];

        public HighScoreTable(String[] n, int[] s)
        {
            this.names = n;
            this.scores = s;
        }

    }
}
