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
        List<string> difficulties = new List<string>(3);
        Dictionary<Image, Entity> entities = new Dictionary<Image, Entity>();

        public MainWindow()
        {
            InitializeComponent();
            difficulties.Add("Easy");
            difficulties.Add("Medium");
            difficulties.Add("Hard");
            cmbDifficultyPicker.ItemsSource = difficulties;
        }

        // Take info from Name and Difficulty fields and send them to GameController
        // Create new game from there
        private void btnStartGame_Click(object sender, RoutedEventArgs e)
        {
            btnCheatMode.IsEnabled = false;
            string diff;
            Difficulty sett;
            if (cmbDifficultyPicker.SelectedValue != null)
            {
                diff = cmbDifficultyPicker.SelectedValue.ToString();
            }
            else
            {
                diff = null;
            }
            switch (diff)
            {
                case "Easy":
                    sett = Difficulty.Easy;
                    break;
                case "Medium":
                    sett = Difficulty.Medium;
                    break;
                case "Hard":
                    sett = Difficulty.Hard;
                    break;
                default:
                    sett = Difficulty.Easy;
                    break;
            }
            ctrl = new GameController(sett);
            ctrl.adventurer.Name = txtPlayerName.Text;
            // Create images
            ctrl.BuildTower();
            ctrl.Setup();
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
            }
            else
            {
                ctrl.Cheating = false;
            }
        }
        
        /// Moving/Attacking stuff

       
        private void Arena_cvs_KeyUp(object sender, KeyEventArgs e)
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
                    //needs work
                    if((Math.Abs(enemy.Position.X - player.Position.X) <= 20) && (Math.Abs(enemy.Position.Y - player.Position.Y)) <= 20)
                    {
                        ctrl.PlayerAttack(player, enemy);
                    }
                }
            }
            //Update canvas positions
             foreach (Image img in Arena.Children)
             {
                //get entity ascociated with image and move it
                Entity entity = entities[img];
                Canvas.SetLeft(img, entity.Position.X);
                Canvas.SetTop(img, entity.Position.Y);
                                
             }

             
        }

    }
}
