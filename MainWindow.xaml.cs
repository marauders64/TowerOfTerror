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
        SoundPlayer sp;
        private int playeranimatednum = 1; //Keeps track of the player's animation.
        private int monsteranimnum = 1; //Keeps 

        public MainWindow()
        {
            InitializeComponent();
            sp = new SoundPlayer(TowerOfTerror.Properties.Resources.Thunderstorm2); // Thanks Mr. Meyer
            sp.PlayLooping();
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
            // Determine difficulty setting based on Combo Box
            // Enabling Cheat mode defaults the difficulty to easy

            string diff;
            Difficulty sett;
            btnSaveGame.IsEnabled = true;
            btnCheatMode.IsEnabled = false;

            // Disable some features if cheating
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
            ctrl.Score = 0;


            SetupImages(); // works!
            entities.Add(img_Protagonist, ctrl.adventurer);


            Timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            PlayerTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            Timer.Tick += Timer_Tick;
            PlayerTimer.Tick += PlayerTimer_Tick;
            Timer.Start();
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
                    Height = 40,
                    RenderTransformOrigin = new Point(0.5, 0.5)
                };
                Canvas.SetLeft(img_enemy, en.Position.X);
                Canvas.SetTop(img_enemy, en.Position.Y);
                Arena.Children.Add(img_enemy);
                entities.Add(img_enemy, en);
            }
            Arena.Focus();


            /*Timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            PlayerTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            Timer.Tick += Timer_Tick;
            PlayerTimer.Tick += PlayerTimer_Tick;
            Timer.Start();*/
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

            try
            {
                if (dialog.FileName != "")
                {
                    ctrl.Load(dialog.FileName);

                    // load images
                    img_Protagonist.Visibility = Visibility.Visible;
                    txtPlayerName.Text = Convert.ToString(ctrl.adventurer.Name);
                    Health_txt.Text = Convert.ToString(ctrl.adventurer.Health);

                    foreach (Enemy en in ctrl.currentFloor.Enemies)
                    {
                        Image img_enemy = new Image
                        {
                            Source = new BitmapImage(new Uri("Graphics/chitiniac_idle-1.png", UriKind.Relative)),
                            Height = 40,
                            RenderTransformOrigin = new Point(0.5, 0.5)
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

                    // duplicate code from Start... :(
                    Timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
                    PlayerTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
                    Timer.Tick += Timer_Tick;
                    PlayerTimer.Tick += PlayerTimer_Tick;
                    Timer.Start();
                }


            }
            catch (FileFormatException ex)
            {
                string cantDoThat = @"We're sorry, but the file you selected is not a valid save file, or it has been corrupted. Please choose another file.";
                MessageBoxButton exit = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Information;
                MessageBox.Show(cantDoThat, "Problem Loading Game", exit, icon);
            }


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
        

        /// <summary>
        /// Holds the logic for enemy tracking and moving. Ticks every half second
        /// and all enemies will either attack (if in range), track the player,
        /// or randomnly move.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Timer_Tick(object sender, EventArgs e)
        {
            Character player = ctrl.adventurer;

            foreach (Enemy enemy in ctrl.currentFloor.Enemies)
            {
                if (AttackHits(enemy, player))
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

        /// <summary>
        /// Holds the logic for player movement and animations. Ticks every tenth of 
        /// a second and will move the player in the direction they are facing as 
        /// well as update the animation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PlayerTimer_Tick(object sender, EventArgs e)
        {
            ctrl.adventurer.Move(ctrl.adventurer.Facing);
            int pointnum = img_protag.SourceRect.Width * playeranimatednum;
            while(playeranimatednum > 4)
            {
                playeranimatednum -= 4;
            }

            img_Protagonist.Source = new CroppedBitmap(new BitmapImage(new Uri("pack://application:,,,/Graphics/hero_animate.png")), new Int32Rect(pointnum, 0, 32, 32));
            ++playeranimatednum;
            
            ImageUpdate();
            sp = new SoundPlayer(TowerOfTerror.Properties.Resources.Running);
            sp.Play();
        }

        /// <summary>
        /// Contains logic for player attacking and stops the player timer when a 
        /// key is released, which will end player animation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Arena_cvs_KeyUp(object sender, KeyEventArgs e)
        {
            PlayerTimer.Stop();
            Character player = ctrl.adventurer;
            
            if(e.Key == Key.Space)
            {
                Console.WriteLine("Attacking");
                sp = new SoundPlayer(TowerOfTerror.Properties.Resources.Swish);
                sp.Play();
                foreach (Enemy enemy in ctrl.currentFloor.Enemies)
                {   
                    if(AttackHits(player, enemy))
                    {
                        ctrl.PlayerAttack(player, enemy);
                        sp = new SoundPlayer(TowerOfTerror.Properties.Resources.Crack);
                        sp.Play();
                    }
                }
            }

            ImageUpdate();

        }

        /// <summary>
        /// Determines whether an attacker will hit the victim or not.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="victim"></param>
        /// <returns>true if attack hits else false</returns>
        private bool AttackHits(Entity attacker, Entity victim)
        {
            if (attacker is Character)
            {
                switch (attacker.Facing)
                {
                    case Direction.Down:
                        if ((attacker.Position.Y < victim.Position.Y) && (victim.Position.Y - attacker.Position.Y < 45) && (Math.Abs(attacker.Position.X - victim.Position.X)) <= 25)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    case Direction.Up:
                        if ((attacker.Position.Y > victim.Position.Y) && (attacker.Position.Y - victim.Position.Y < 45) && (Math.Abs(attacker.Position.X - victim.Position.X)) <= 25)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    default:
                        break;
                }
            }
            switch(attacker.Facing)
            {
                case Direction.Up:
                    if((attacker.Position.Y < victim.Position.Y) && (victim.Position.Y - attacker.Position.Y < 45) && (Math.Abs(attacker.Position.X - victim.Position.X)) <= 25)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case Direction.Down:
                    if ((attacker.Position.Y > victim.Position.Y) && (attacker.Position.Y - victim.Position.Y < 45) && (Math.Abs(attacker.Position.X - victim.Position.X)) <= 25)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case Direction.Left:
                    if ((attacker.Position.X > victim.Position.X) && (attacker.Position.X - victim.Position.X < 45) && (Math.Abs(attacker.Position.Y - victim.Position.Y)) <= 25)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case Direction.Right:
                    if ((attacker.Position.X < victim.Position.X) && (victim.Position.X - attacker.Position.X < 45) && (Math.Abs(attacker.Position.Y - victim.Position.Y)) <= 25)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                default:
                    return false;
            }
        }

        /// <summary>
        /// When any of the wasd keys are pressed the characters direction that 
        /// it is facing will change accordingly and the Playertimer will start.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Arena_cvs_KeyDown(object sender, KeyEventArgs e)
        {
            Character player = ctrl.adventurer;


            if (e.Key == Key.W)
            {
                player.Facing = Direction.Up;
                PlayerTimer.Start();
            }
            else if (e.Key == Key.S)
            {
                player.Facing = Direction.Down;
                PlayerTimer.Start();
            }
            else if (e.Key == Key.A)
            {
                player.Facing = Direction.Left;
                PlayerTimer.Start();
            }
            else if (e.Key == Key.D)
            {
                player.Facing = Direction.Right;
                PlayerTimer.Start();
            }
        }

        /// <summary>
        /// Updates the list of dead enemies and also updates the images on
        /// the canvas to represent the objects they belong to. Also determines
        /// whether the level has ended or the game has ended.
        /// </summary>
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
                switch (entity.Facing)
                {
                    case Direction.Up:
                        if (entity is Character)
                        {
                            img.RenderTransform = new RotateTransform(0.0);
                        }
                        else
                        {
                            img.RenderTransform = new RotateTransform(180.0);
                        }
                        break;
                    case Direction.Down:
                        if (entity is Character)
                        {
                            img.RenderTransform = new RotateTransform(180.0);
                        }
                        else
                        {
                            img.RenderTransform = new RotateTransform(0.0);
                        }
                        break;
                    case Direction.Left:
                        img.RenderTransform = new RotateTransform(-90.0);
                        break;
                    case Direction.Right:
                        img.RenderTransform = new RotateTransform(90.0);
                        break;
                    default:
                        img.RenderTransform = new RotateTransform(0.0);
                        break;
                }
                if (entity is Enemy)
                {
                    
                    while (monsteranimnum > 7)
                    {
                        monsteranimnum -= 7;
                    }
                    int pointnum = monsteranimnum * 64;
                    img.Source = new CroppedBitmap(new BitmapImage(new Uri("pack://application:,,,/Graphics/chitiniac-move.png")), new Int32Rect(pointnum, 0, 64, 64));
                    ++monsteranimnum;
                }
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
                ctrl.Score += 50;

                // Beat the game logic
                if (ctrl.IsGameWon())
                {
                    string victoryText;
                    if (!ctrl.Cheating)
                    {
                        victoryText = @"Congratulations! You beat the game!
Your score is " + ctrl.Score;
                        HighScores.Leaderboard.AddHighScore(new HighScore(txtPlayerName.Text, ctrl.Score));
                    }
                    else
                    {
                        victoryText = @"You won! Or did you?
You cheated, so your score is 0.";
                    }
                    MessageBoxButton exit = MessageBoxButton.OK;
                    MessageBoxImage icon = MessageBoxImage.Information;
                    MessageBox.Show(victoryText, "Congrats Dude", exit, icon);
                    Application.Current.Shutdown(); // <-- need to get it synced with MessageBox acknowledgement button
                }

                // Level transition logic
                else if (ctrl.currentFloor.LevelComplete())
                {
                    //Timer.Stop(); //heast: necessary to advance to next level (or level will never stop running)
                    //PlayerTimer.Stop();
                    ctrl.Score += ctrl.adventurer.Health;
                    string goodText = @"Level Complete! Proceed to next level";
                    MessageBoxButton exit = MessageBoxButton.OK;
                    MessageBoxImage icon = MessageBoxImage.Information;
                    MessageBoxResult OK = MessageBox.Show(goodText, "Yay", exit, icon);

                    //heast
                    if (MessageBoxResult.OK == OK && !ctrl.IsGameWon())
                    {
                        ctrl.MoveForward();
                        SetupImages();
                        ctrl.adventurer.Position = new Point(245, 240);
                        img_Protagonist.Visibility = Visibility.Visible;
                        //Timer.Start();
                        //PlayerTimer.Start();
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
            if (!ctrl.Cheating)
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
        }

        /// <summary>
        /// Takes a snapshot of point-in-time game state information and stores it
        /// By heast
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveGame_Click(object sender, RoutedEventArgs e)
        {
            Timer.Stop();
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Data files|*.dat";
            dialog.Title = "Saving Game File:";
            dialog.ShowDialog();

            if (dialog.FileName != "")
            {
                ctrl.Save(dialog.FileName);
            }
            Arena.Focus();
            Timer.Start();
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
