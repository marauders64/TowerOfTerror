using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TowerOfTerror.Model
{
    class HighScores
    {
        //public Dictionary<string, int> HighScoreDict { get; }
        //public List<int> ScoreList { get; set; }
        public List<HighScore> Scores { get; set; }

        public static HighScores Leaderboard { get; }

        //Constructs the HighScore class.
        private HighScores()
        {

        }

        //Adds a score to the Scores list and sorts the list.
        //Adds the score to the score file.
        public void AddHighScore(HighScore score)
        {
            StreamWriter writer;
        }

        //Gets the high scores from a file and sorts them based on score.
        public void GetHighScores()
        {
            StreamReader reader;
        }

    }

    class HighScore
    {
        public string Name { get; set; }
        public int Score { get; set; }

        public HighScore(string name, int score )
        {
            Name = name;
            Score = score;
        }

    }

}
