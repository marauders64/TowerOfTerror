//This file contains the unit tests for the HighScore classes.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TowerOfTerror.Model
{
    [TestClass]
    public class HighScoreTest
    {
        [TestMethod]
        public void Leaderboard_Exists()
        {
            Assert.IsTrue(HighScores.Leaderboard != null);
        }

        [TestMethod]
        public void AddHighScores_Works()
        {
            HighScore entry1 = new HighScore("Jon", 1337);
            HighScore entry2 = new HighScore("Ash", 180);
            HighScores.Leaderboard.AddHighScore(entry1);
            HighScores.Leaderboard.AddHighScore(entry2);
            Assert.IsTrue(HighScores.Leaderboard.Scores[0].Name == "Jon");
            Assert.IsTrue(HighScores.Leaderboard.Scores[0].Score == 1337);
            Assert.IsTrue(HighScores.Leaderboard.Scores[1].Name == "Ash");
            Assert.IsTrue(HighScores.Leaderboard.Scores[1].Score == 180);
            HighScores.Leaderboard.Scores.Clear();
        }

        [TestMethod]
        public void SaveHighScoresWork()
        {
            HighScore entry1 = new HighScore("Jon", 1337);
            HighScore entry2 = new HighScore("Ash", 180);
            HighScores.Leaderboard.AddHighScore(entry1);
            HighScores.Leaderboard.AddHighScore(entry2);
            HighScores.Leaderboard.SaveHighScores();
            HighScores.Leaderboard.Scores.Clear();
            HighScores.Leaderboard.GetHighScores();
            Assert.IsTrue(HighScores.Leaderboard.Scores[0].Name == "Jon");
            Assert.IsTrue(HighScores.Leaderboard.Scores[0].Score == 1337);
            Assert.IsTrue(HighScores.Leaderboard.Scores[1].Name == "Ash");
            Assert.IsTrue(HighScores.Leaderboard.Scores[1].Score == 180);
            HighScores.Leaderboard.Scores.Clear();
        }

    }

    [TestClass]
    public class ScoreTest
    {
        [TestMethod]
        public void HighScoresTest()
        {
            HighScore entry = new HighScore("Jon", 1337);
            Assert.IsTrue(entry.Name == "Jon");
            Assert.IsTrue(entry.Score == 1337);
            Assert.IsTrue(entry.ToString() == "Jon: 1337");
        }
    }

}
