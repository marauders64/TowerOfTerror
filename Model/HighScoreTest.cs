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
            HighScores scores = new HighScores();
            scores.AddHighScore("Jon", 1330);
            scores.AddHighScore("Sue", 42);
            Assert.IsTrue(scores.Scores[0][0] == "Jon");
            Assert.IsTrue(scores.Scores[0][1] == "1330");
            Assert.IsTrue(scores.Scores[1][0] == "Sue");
            Assert.IsTrue(scores.Scores[1][1] == "42");
        }

        [TestMethod]
        public void AddHighScoresList_Works()
        {
            HighScores highscore = new HighScores();
            highscore.AddHighScore("Answer", 42);
            highscore.AddHighScore("Mel", 166);
            highscore.AddHighScore("Kirk", 133000);
            Assert.IsTrue(highscore.Scores[0][1] == "133000");
            Assert.IsTrue(highscore.Scores[1][1] == "166");
            Assert.IsTrue(highscore.Scores[2][1] == "42");
        }



    }
}
