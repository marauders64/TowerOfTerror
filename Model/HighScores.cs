﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerOfTerror.Model
{
    class HighScores
    {
        public Dictionary<string, int> HighScoreDict { get; }
        public List<int> ScoreList { get; set; }
        public List<string[]> Scores { get; set; }

        //Adds a score to the Scores list and sorts the list.
        //Adds the score to the score file.
        public void AddHighScore(string name, int score)
        {

        }

        //Gets the high scores from a file and sorts them based on score.
        public void GetHighScores()
        {

        }

    }
}
