using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
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
using System.Windows.Threading;
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
        private int attackCount = 0;
        private int defenseCount = 0;
        private int healCount = 0;
        DispatcherTimer Timer = new DispatcherTimer();
        DispatcherTimer PlayerTimer = new DispatcherTimer();

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
        // Create new game
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
                btnAtk.IsEnabled = false;
                btnDef.IsEnabled = false;
                btnHeal.IsEnabled = false;
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
            // Set up
            ctrl.Setting = sett;
            ctrl.adventurer.Name = txtPlayerName.Text;
            ctrl.BuildTower();
            ctrl.Setup();
            Health_txt.Text = Convert.ToString(ctrl.adventurer.Health);

            img_Protagonist.Visibility = Visibility.Visible;


            SetupImages(); // works!
            entities.Add(img_Protagonist, ctrl.adventurer);
            // Setup images
            /*foreach (Entity en in ctrl.currentFloor.Enemies)
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

            Timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            PlayerTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            Timer.Tick += Timer_Tick;
            PlayerTimer.Tick += PlayerTimer_Tick;
            Timer.Start();*/
        }

        /// <summary>
        /// Logic for populating game screen. Extracted for reuse by heast
        /// </summary>
        private void SetupImages()
        {
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


            Timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            PlayerTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            Timer.Tick += Timer_Tick;
            PlayerTimer.Tick += PlayerTimer_Tick;
            Timer.Start();
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

            ctrl.BuildTower();
            ctrl.Setup();

            if (dialog.FileName != "")
            {
                ctrl.Load(dialog.FileName);
            }

            // load images
            img_Protagonist.Visibility = Visibility.Visible;
            txtPlayerName.Text = Convert.ToString(ctrl.adventurer.Name);
            Health_txt.Text = Convert.ToString(ctrl.adventurer.Health);

            foreach (Enemy en in ctrl.currentFloor.Enemies)
            {
                Image img_enemy = new Image
                {
                    Source = new BitmapImage(new Uri("Graphics/chitiniac_idle-1.png", UriKind.Relative)),
                    //Visibility = Visibility.Visible,
                    Height = 40
                };
                if (en.Status == Life.Alive)
                {
                    img_enemy.Visibility = Visibility.Visible;
                } 
                else if (en.Status == Life.Dead)
                {
                    img_enemy.Visibility = Visibility.Hidden;
                }
                Canvas.SetLeft(img_enemy, en.Position.X);
                Canvas.SetTop(img_enemy, en.Position.Y);
                Arena.Children.Add(img_enemy);
                entities.Add(img_enemy, en);
            }
            Arena.Focus();
            entities.Add(img_Protagonist, ctrl.adventurer);

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

        // Invisible button that toggles cheat mode
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
        

        //Independent enemy movement
        public void Timer_Tick(object sender, EventArgs e)
        {
            Character player = ctrl.adventurer;

            foreach (Enemy enemy in ctrl.currentFloor.Enemies)
            {
                if ((Math.Abs(enemy.Position.X - player.Position.X) <= 45) && (Math.Abs(enemy.Position.Y - player.Position.Y)) <= 45)
                {
                    ctrl.EnemyAttack(enemy);
                }

                else if ((Math.Abs(enemy.Position.X - player.Position.X) <= 150) && (Math.Abs(enemy.Position.Y - player.Position.Y)) <= 150)
                {
                    ctrl.EnemyTrack(enemy, player);
                }

                else
                {
                    ctrl.UpdateEnemyPostition(enemy);
                }
            }

            ImageUpdate();
        }

        public void PlayerTimer_Tick(object sender, EventArgs e)
        {
            ctrl.adventurer.Move(ctrl.adventurer.Facing);
            ImageUpdate();
        }

        /// Moving/Attacking stuff
        private void Arena_cvs_KeyUp(object sender, KeyEventArgs e)
        {
            PlayerTimer.Stop();
            Character player = ctrl.adventurer;
            /*
            if (e.Key == Key.W)
            {
                ctrl.UpdatePlayerPosition(player, Direction.Up);
                img_Protagonist.RenderTransform = new RotateTransform(0.0);
            }
            else if (e.Key == Key.S)
            {
                ctrl.UpdatePlayerPosition(player, Direction.Down);
                img_Protagonist.RenderTransform = new RotateTransform(180.0);
            }
            else if (e.Key == Key.A)
            {
                ctrl.UpdatePlayerPosition(player, Direction.Left);
                img_Protagonist.RenderTransform = new RotateTransform(-90.0);
            }
            else if (e.Key == Key.D)
            {
                ctrl.UpdatePlayerPosition(player, Direction.Right);
                img_Protagonist.RenderTransform = new RotateTransform(90.0);
            }*/
            if(e.Key == Key.Space)
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

            ImageUpdate();

        }

        //Player continuous Movement
        public void Arena_cvs_KeyDown(object sender, KeyEventArgs e)
        {
            Character player = ctrl.adventurer;

            if (e.Key == Key.W)
            {
                player.Facing = Direction.Up;
                img_Protagonist.RenderTransform = new RotateTransform(0.0);
                PlayerTimer.Start();
            }
            else if (e.Key == Key.S)
            {
                player.Facing = Direction.Down;
                img_Protagonist.RenderTransform = new RotateTransform(180.0);
                PlayerTimer.Start();
            }
            else if (e.Key == Key.A)
            {
                player.Facing = Direction.Left;
                img_Protagonist.RenderTransform = new RotateTransform(-90.0);
                PlayerTimer.Start();
            }
            else if (e.Key == Key.D)
            {
                player.Facing = Direction.Right;
                img_Protagonist.RenderTransform = new RotateTransform(90.0);
                PlayerTimer.Start();
            }


            
        }

        //After movement Stuff
        public void ImageUpdate()
        {
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
                    //ctrl.currentFloor.Enemies.Remove(entity as Enemy); heast
                    img.Visibility = Visibility.Hidden; //heast
                    UpdateInventory();
                }
            }
            
            foreach(Image img in deadentity)
            {
                Arena.Children.Remove(img);

                // Beat the game logic
                if (ctrl.IsGameWon())
                {
                    string victoryText = @"Congratulations! You beat the game!";
                    MessageBoxButton exit = MessageBoxButton.OK;
                    MessageBoxImage icon = MessageBoxImage.Information;
                    MessageBox.Show(victoryText, "Congrats Dude", exit, icon);
                }

                // Level transition logic
                 if (ctrl.currentFloor.LevelComplete())
                {
                    Timer.Stop(); //heast: necessary to advance to next level (or level will never stop running)
                    PlayerTimer.Stop();
                    string goodText = @"Level Complete! Proceed to next level";
                    MessageBoxButton exit = MessageBoxButton.OK;
                    MessageBoxImage icon = MessageBoxImage.Information;
                    MessageBoxResult OK = MessageBox.Show(goodText, "Yay", exit, icon);

                    //heast
                    if (MessageBoxResult.OK == OK && !ctrl.IsGameWon())
                    {
                        ctrl.MoveForward();
                        SetupImages();                        
                        //ctrl.adventurer.Position = new Point(245, 240);
                        img_Protagonist.Visibility = Visibility.Visible;
                        Timer.Start();
                        PlayerTimer.Start();
                    }
                }

                // Death logic: exits the app
                if (ctrl.IsGameOver())
                {
                    string deadText = @"You Died!";
                    MessageBoxButton exit = MessageBoxButton.OK;
                    MessageBoxImage icon = MessageBoxImage.Stop;
                    MessageBox.Show(deadText, "Game Over", exit, icon);
                    Application.Current.Shutdown();
                }


            }
            Health_txt.Text = Convert.ToString(ctrl.adventurer.Health);
        }

        private void UpdateInventory()
        {
            foreach (Item i in ctrl.adventurer.inventory)
            {
                switch (i.Type)
                {
                    case PowerUp.AtkBuff:
                        attackCount++;
                        lblAtkCount.Text = attackCount.ToString();
                        break;
                    case PowerUp.DefBuff:
                        defenseCount++;
                        lblDefCount.Text = defenseCount.ToString();
                        break;
                    case PowerUp.Heal:
                        healCount++;
                        lblHealCount.Text = healCount.ToString();
                        break;
                    default:
                        break;
                }
            }
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

        private void btnAtk_Click(object sender, RoutedEventArgs e)
        {
            if (attackCount > 0)
            {
                ctrl.adventurer.Power *= 2;
                attackCount--;
                lblAtkCount.Text = attackCount.ToString();
                foreach (Item i in ctrl.adventurer.inventory)
                {
                    if (i.Type == PowerUp.AtkBuff)
                    {
                        ctrl.adventurer.inventory.Remove(i);
                        break;
                    }
                }
            }
            else
            {
                string badBuff = @"You don't have any attack powerups!";
                MessageBoxButton exit = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Exclamation;
                MessageBox.Show(badBuff, "Oops", exit, icon);
            }
            Arena.Focus();
        }

        private void btnDef_Click(object sender, RoutedEventArgs e)
        {
            if (defenseCount > 0)
            {
                ctrl.adventurer.Defense *= 2;
                defenseCount--;
                lblDefCount.Text = defenseCount.ToString();
                foreach (Item i in ctrl.adventurer.inventory)
                {
                    if (i.Type == PowerUp.DefBuff)
                    {
                        ctrl.adventurer.inventory.Remove(i);
                        break;
                    }
                }
            }
            else
            {
                string badBuff = @"You don't have any defense powerups!";
                MessageBoxButton exit = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Exclamation;
                MessageBox.Show(badBuff, "Oops", exit, icon);
            }
            Arena.Focus();
        }

        private void btnHeal_Click(object sender, RoutedEventArgs e)
        {
            if (healCount > 0)
            {
                ctrl.adventurer.Health += 10;
                healCount--;
                Health_txt.Text = ctrl.adventurer.Health.ToString();
                lblHealCount.Text = healCount.ToString();
                foreach (Item i in ctrl.adventurer.inventory)
                {
                    if (i.Type == PowerUp.Heal)
                    {
                        ctrl.adventurer.inventory.Remove(i);
                        break;
                    }
                }
            }
            else
            {
                string badBuff = @"You don't have any health powerups!";
                MessageBoxButton exit = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Exclamation;
                MessageBox.Show(badBuff, "Oops", exit, icon);
            }
            Arena.Focus();
        }

        
    }
}
