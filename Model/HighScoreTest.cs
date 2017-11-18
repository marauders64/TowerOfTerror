using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TowerOfTerror.Model
{
    [TestClass]
    class HighScoreTest
    {
        [TestMethod]
        public void AddHighScores_Works()
        {
            HighScores scores = HighScores.Leaderboard;
            scores.AddHighScore(new HighScore("Jon", 1330));
            scores.AddHighScore(new HighScore("Sue", 42));
            Assert.IsTrue(scores.Scores[0].Name == "Jon");
            Assert.IsTrue(scores.Scores[0].Score == 1330);
            Assert.IsTrue(scores.Scores[1].Name == "Sue");
            Assert.IsTrue(scores.Scores[1].Score == 42);
        }
    }
}
