using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
        GameController ctrl = new GameController();
        List<string> difficulties = new List<string>(3);
        Dictionary<Image, Entity> entities = new Dictionary<Image, Entity>();

        public MainWindow()
        {
            InitializeComponent();
            difficulties.Add("Easy");
            difficulties.Add("Medium");
            difficulties.Add("Hard");
            cmbDifficultyPicker.ItemsSource = difficulties;
            HighScores.Leaderboard.GetHighScores();
        }

        // Take info from Name and Difficulty fields and send them to GameController
        // Create new game from there
        private void btnStartGame_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Howdi");
            // Determine difficulty setting based on Combo Box
            // Enabling Cheat mode defaults the difficulty to easy
            string diff;
            Difficulty sett;
            btnSaveGame.IsEnabled = true;
            btnCheatMode.IsEnabled = false;

            if (ctrl.Cheating)
            {
                btnSaveGame.IsEnabled = false;
                btnLoadGame.IsEnabled = false;
                sett = Difficulty.Easy;
            }
            else
            {
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
            }
            ctrl.Setting = sett;
            ctrl.adventurer.Name = txtPlayerName.Text;
            ctrl.BuildTower();
            ctrl.Setup();
            Health_txt.Text = Convert.ToString(ctrl.adventurer.Health);

            img_Protagonist.Visibility = Visibility.Visible;

            // Setup images
            foreach (Entity en in ctrl.currentFloor.Enemies)
            {
                Image img_enemy = new Image
                {
                    Source = new BitmapImage(new Uri("Graphics/chitiniac_idle-1.png", UriKind.Relative)),
                    Visibility = Visibility.Visible,
                    Height = 40
                };
                Canvas.SetLeft(img_enemy, en.Position.X);
                Canvas.SetTop(img_enemy, en.Position.Y);
                Arena.Children.Add(img_enemy);
                entities.Add(img_enemy, en);
            }
            Arena.Focus();
            entities.Add(img_Protagonist, ctrl.adventurer);
        }

        /// <summary>
        /// Allows user to choose a file to load via Windows dialog box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoadGame_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Data files|*.dat";
            dialog.Title = "Please Choose a Game to Load:";
            dialog.ShowDialog();

            if (dialog.FileName != "")
            {
                ctrl.Load(dialog.FileName);
            }
            Arena.Focus();

            /*try
            {
                ctrl.Load(@"C:\Users\Heather\Desktop\Fall2017\CpS209\programs\TowerOfTerror\SavedGames\ToT.dat");
            }
            // needs tested!!
            catch (FileNotFoundException)
            {
                string errorMsg = "The system cannot find the file you have requested. Please try again.";
                MessageBoxButton ok = MessageBoxButton.OK;
                MessageBoxImage img = MessageBoxImage.Error;
                MessageBox.Show(errorMsg, "Error", ok, img);
            }*/
        }

        // Show credits
        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Opening Credits");
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
* Game will autosave at end of each level.
Difficulty: Set difficulty using the dropdown box provided.
* Easy difficulty gives you full health. Subsequent levels reduce health by 20% each.";
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
        
        /// Moving/Attacking stuff
        private void Arena_cvs_KeyUp(object sender, KeyEventArgs e)
        {
            Character player = ctrl.adventurer;
            
            //Need to get images connected to Entities.

            if (e.Key == Key.W)
            {
                ctrl.UpdatePositions(player, Direction.Up);
                img_Protagonist.RenderTransform = new RotateTransform(0.0);
            }
            else if (e.Key == Key.S)
            {
                ctrl.UpdatePositions(player, Direction.Down);
                img_Protagonist.RenderTransform = new RotateTransform(180.0);
            }
            else if (e.Key == Key.A)
            {
                ctrl.UpdatePositions(player, Direction.Left);
                img_Protagonist.RenderTransform = new RotateTransform(-90.0);
            }
            else if (e.Key == Key.D)
            {
                ctrl.UpdatePositions(player, Direction.Right);
                img_Protagonist.RenderTransform = new RotateTransform(90.0);
            }
            else if(e.Key == Key.Space)
            {
                Console.WriteLine("Attacking");
                foreach (Enemy enemy in ctrl.currentFloor.Enemies)
                {   
                    //needs work
                    if((Math.Abs(enemy.Position.X - player.Position.X) <= 45) && (Math.Abs(enemy.Position.Y - player.Position.Y)) <= 45)
                    {
                        Console.WriteLine("hitting");
                        ctrl.PlayerAttack(player, enemy);
                    }
                    
                }
            }

            foreach(Enemy enemy in ctrl.currentFloor.Enemies)
            {
                if ((Math.Abs(enemy.Position.X - player.Position.X) <= 45) && (Math.Abs(enemy.Position.Y - player.Position.Y)) <= 45)
                {
                    ctrl.EnemyAttack(enemy);
                }
            }

            //Update canvas positions
            List<Image> deadentity = new List<Image>();
            foreach (Image img in Arena.Children)
            {
                //get entity ascociated with image and move it
                Entity entity = entities[img];
                Canvas.SetLeft(img, entity.Position.X);
                Canvas.SetTop(img, entity.Position.Y);
                Console.WriteLine(entity.Position.X + "  " + entity.Position.Y);
                if (entity.IsDead())
                {
                    deadentity.Add(img);
                    ctrl.currentFloor.Enemies.Remove(entity as Enemy);
                }
            }
            
            foreach(Image img in deadentity)
            {
                Arena.Children.Remove(img);
                if (ctrl.currentFloor.LevelComplete())
                {
                    string goodText = @"Level Complete! Proceed to next level";
                    MessageBoxButton exit = MessageBoxButton.OK;
                    MessageBoxImage icon = MessageBoxImage.Information;
                    MessageBox.Show(goodText, "Yay", exit, icon);
                }
                if (ctrl.IsGameOver())
                {
                    string deadText = @"Game over!";
                    MessageBoxButton exit = MessageBoxButton.OK;
                    MessageBoxImage icon = MessageBoxImage.Information;
                    MessageBox.Show(deadText, "Game Over", exit, icon);
                }
            }
            Health_txt.Text = Convert.ToString(ctrl.adventurer.Health);

        }

        // NOTE TO HEATHER EAST:
        // This button will be disabled when the window starts up, but it will re-enable when "Start"
        // You can dump your save logic here
        private void btnSaveGame_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Data files|*.dat";
            dialog.Title = "Saving Game File:";
            dialog.ShowDialog();

            if (dialog.FileName != "")
            {
                ctrl.Save(dialog.FileName);
            }
            Arena.Focus();
        }
    }
}
