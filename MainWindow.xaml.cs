﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TowerOfTerror.Model;

namespace TowerOfTerror
{
    /// <summary>
    /// Contains front end logic for the Tower of Terror game
    /// </summary>
    public partial class MainWindow : Window
    {
        GameController ctrl;

        public MainWindow()
        {
            InitializeComponent();
        }

        // Show a window with game setup info (ask for name and difficulty setting)
        private void btnStartGame_Click(object sender, RoutedEventArgs e)
        {
            Difficulty sett;
            Window prepGame = new Window();
            prepGame.Activate();
            //ctrl = new GameController(sett);
        } 

        // Show a window to load a file
        private void btnLoadGame_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        // Show credits
        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            string aboutText = @"Tower Of Terror
Produced by:
    Heather East
    Jonathan Mauk
    Carlos Santana

Inspired by: The Legend of Zelda: A Link Between Worlds";
            MessageBoxButton exit = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Information;
            MessageBox.Show(aboutText, "About", exit, icon);
        }

        // Display game controls
        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            string helpText = @"Controls:
Move: WASD or Arrow Keys
Attack: Space Bar
Use Item: Click on it in the sidebar (100 level)
Save Game: Click on the button in the top right (100 level)
* Game will autosave at end of each level.";
            MessageBoxButton exit = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Information;
            MessageBox.Show(helpText, "How to Play", exit, icon);
        }

        // Show a screen with the high scores
        private void btnHighScores_Click(object sender, RoutedEventArgs e)
        {
            string scores = "High Scores thus far:\n";
            if (HighScores.Leaderboard.Scores == null)
            {
                scores = "No high scores to show.";
            }
            else
            {
                foreach (HighScore score in HighScores.Leaderboard.Scores)
                {
                    scores += score + "\n";
                }
            }
            MessageBoxButton exit = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Information;
            MessageBox.Show(scores, "How to Play", exit, icon);
        }

        // Invisible button that activates cheat mode
        // Also changes the Start Game button to Cheating (Yellow) or Not (Alice Blue)
        private void btnCheatMode_Click(object sender, RoutedEventArgs e)
        {
            if (!ctrl.Cheating)
            {
                ctrl.Cheating = true;
                btnStartGame.Background = Brushes.Yellow;
            }
            else
            {
                ctrl.Cheating = false;
                btnStartGame.Background = Brushes.AliceBlue;
            }
        }

        /// <summary>
        /// Moving/Attacking stuff putting it here for now
        /// </summary>
        private void SomeGameArea_cvs_KeyUp(object sender, KeyEventArgs e)
        {
            Character player = ctrl.currentFloor.dude;
            
            //Need to get images connected to Entities.

            if (e.Key == Key.W)
            {
                ctrl.UpdatePositions(player, Direction.Up);
            }
            else if (e.Key == Key.S)
            {
                ctrl.UpdatePositions(player, Direction.Down);
            }
            else if (e.Key == Key.A)
            {
                ctrl.UpdatePositions(player, Direction.Left);
            }
            else if (e.Key == Key.D)
            {
                ctrl.UpdatePositions(player, Direction.Right);
            }
            else if(e.Key == Key.Space)
            {
                foreach(Enemy enemy in ctrl.Enemies)
                {
                    if((Math.Abs(enemy.Position.X - player.Position.X) <= 20) && (Math.Abs(enemy.Position.Y - player.Position.Y)) <= 20)
                    {
                        ctrl.PlayerAttack(player, enemy);
                    }
                }
            }
            //Update canvas positions
        }

    }
}
