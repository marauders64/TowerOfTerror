//This file contains the HighScores and HighScore classes.
//These classes are used to manage high scores.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TowerOfTerror.Model
{
    /// <summary>
    /// A class for tracking and adding high scores.
    /// </summary>
    class HighScores
    {
        public List<HighScore> Scores { get; set; }

        private static HighScores leaderboard = new HighScores();
        public static HighScores Leaderboard
        {
            get
            {
                return leaderboard;
            }
        }

        //Constructs the HighScore class.
        private HighScores()
        {
            Scores = new List<HighScore>();
        }

        //Adds a score to the Scores list and sorts the list.
        //Adds the score to the score file.
        public void AddHighScore(HighScore score)
        {
            Scores.Add(score);
            Scores.Sort();
            Scores.Reverse();
            SaveHighScores();
            
        }

        /// <summary>
        /// Saves all of the high scores to a txt file
        /// for safekeeping.
        /// </summary>
        public void SaveHighScores()
        {
            using (StreamWriter writer = new StreamWriter("HighScores.txt"))
            {
                foreach (HighScore entry in Scores)
                {
                    writer.WriteLine(entry.ToString());
                }
            }
        }


        //Gets the high scores from a file and sorts them based on score.
        public void GetHighScores()
        {
            StreamReader reader;
            string filename = "HighScores.txt";
            if(File.Exists(filename))
            {
                using(reader = new StreamReader(filename))
                {
                    string entry = reader.ReadLine();
                    while (entry != null)
                    {
                        string[] score = entry.Split(':');
                        score[1].Trim(' ');
                        HighScore highscore = new HighScore(score[0], Convert.ToInt32(score[1]));
                        Scores.Add(highscore);
                        entry = reader.ReadLine();
                    }
                }
                Scores.Sort();
                Scores.Reverse();
            }
            else
            {
                File.Create(filename);
            }
        }

    }

    /// <summary>
    /// A class for keeping a high scorer's name and score.
    /// </summary>
    class HighScore : IComparable<HighScore>
    {
        public string Name { get; set; }
        public int Score { get; set; }

        public HighScore(string name, int score )
        {
            Name = name;
            Score = score;
        }

        /// <summary>
        /// Returns a string of the Name and score of a high score.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string entry = Name + ": " + Score;
            return entry;
        }
        
        /// <summary>
        /// Compares scores to each other.
        /// </summary>
        /// <param name="score"></param>
        /// <returns></returns>
        public int CompareTo(HighScore score)
        {
            if (score == null)
                return 1;

            else
                return this.Score.CompareTo(score.Score);
        }
    }

}
