﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

/*
 * GameController.cs contains the GameController class
 * Detecting endgame
 * Adding items to inventory upon an enemy death
 * Also contains an AttackResult enum, a Difficulty enum, and a Direction enum
 * Also contains the Load/Save methods where all Serialization and Deserialization meet
 */

namespace TowerOfTerror.Model
{
    // Indicates whether the entity was hit, missed, killed, or dropped
    public enum AttackResult { Hit, Miss, Kill }

    // Sets the difficulty of the level
    public enum Difficulty { Easy, Medium, Hard }

    // Gives the direction an entity is facing or moving
    public enum Direction { Up, Down, Left, Right }


    // Manages game logic
    class GameController : ISerializable
    {
        // Retrieves floor number for the current floor
        public int CurrentFloor { get; set; }
        // Contains a list of all floors in the game
        public List<Level> Floors { get; set; }
        // Manages the difficulty setting picked in the combo box
        public Difficulty Setting { get; set; }
        // Detects cheat mode
        public bool Cheating { get; set; }
        // A level object storing information of the current floor
        public Level currentFloor;
        // Instance of the character that is passed to every game
        public Character adventurer;
        // Store a game's score
        public int Score { get; set; }
        // Random seed for determining item drop
        public static Random rand = new Random();
        
        // Instantiate a gamecontroller
        public GameController()
        {
            this.Floors = new List<Level>();
            this.Setting = Difficulty.Easy;
            this.adventurer = new Character();
        }

        // Adds levels to the list of levels
        // Sets currentFloor to the first level to start
        public void BuildTower()
        {
            Level.ResetNum(); // keeps level 0 at level 0
            this.Floors.Add(new Level(LevelType.Basic));
            this.Floors.Add(new Level(LevelType.Basic));
            this.Floors.Add(new Level(LevelType.Basic));
            this.Floors.Add(new Level(LevelType.Final));
            this.currentFloor = Floors[0];
        }

        // Move to next level
        // If on last level (final), don't move forward
        public void MoveForward()
        {
            if (currentFloor.Type == LevelType.Basic)
            {
                currentFloor = Floors[CurrentFloor + 1];
                CurrentFloor += 1;
                adventurer.Position = new Point(245, 240); 
            }
        }

        // Populate each level in this.Floors with enemies
        // Also set the adventurer's health depending on difficulty
        public void Setup()
        {
            // Adjust character health according to difficulty
            if (!this.Cheating)
            {
                switch (this.Setting)
                {
                    case Difficulty.Easy:
                        this.adventurer.Health = 100;
                        break;
                    case Difficulty.Medium:
                        this.adventurer.Health = 80;
                        break;
                    case Difficulty.Hard:
                        this.adventurer.Health = 60;
                        break;
                    default:
                        this.adventurer.Health = 100;
                        break;
                }
            }
            this.currentFloor = this.Floors[0]; // for now just load the main level
            foreach (Level lev in this.Floors)
            {
                lev.FillEnemies(lev);
                lev.PlaceEnemies();
            }
        }

        // Does the logic for when the player attacks
        // Detects enemy end of life
        // Also, if the enemy drops an item, adds a new Item object to the inventory
        public void PlayerAttack(Entity character, Entity enemy)
        {
            character.Attack(enemy);
            if (enemy.Health == 0)
            {
                enemy.Status = Life.Dead;
                if (enemy is Enemy)
                {
                    Enemy en = (Enemy)enemy;
                    if (en.DropsItem())
                    {
                        Item it = new Item();
                        it.Type = it.WhichItem();
                        adventurer.inventory.Add(it);
                    }
                }
            }
        }

        // Does Enemy attack logic
        // Detects endLife for the Character
        public void EnemyAttack(Enemy enemy)
        {
            if (!this.Cheating)
            {
                enemy.Attack(adventurer);
                if (adventurer.Health == 0)
                {
                    adventurer.Status = Life.Dead;
                }
            }
        }

        /// <summary>
        /// Moves the character in the direction.
        /// </summary>
        /// <param name="character"></param>
        /// <param name="direction"></param>
        public void UpdatePlayerPosition(Character character, Direction direction)
        {
            character.Move(direction);
        }

        /// <summary>
        /// Randomly moves the enemy.
        /// </summary>
        /// <param name="enemy"></param>
        public void UpdateEnemyPostition(Enemy enemy)
        {
            Direction dir;
            switch (rand.Next(1, 5))
            {
                case 1:
                    dir = Direction.Up;
                    enemy.Facing = Direction.Up;
                    break;
                case 2:
                    dir = Direction.Down;
                    enemy.Facing = Direction.Down;
                    break;
                case 3:
                    dir = Direction.Left;
                    enemy.Facing = Direction.Left;
                    break;
                case 4:
                    dir = Direction.Right;
                    enemy.Facing = Direction.Right;
                    break;
                default:
                    dir = Direction.Up;
                    enemy.Facing = Direction.Up;
                    break;
            }
            enemy.Move(dir);
        }

        /// <summary>
        /// Does the logic for how an enemy tracks a character.
        /// </summary>
        /// <param name="enemy"></param>
        /// <param name="player"></param>
        public void EnemyTrack(Enemy enemy, Entity player)
        {
            double xdist = enemy.Position.X - player.Position.X;
            double ydist = enemy.Position.Y - player.Position.Y;
            if(Math.Abs(xdist) >= Math.Abs(ydist))
            {
                if(xdist >= 0)
                {
                    enemy.Move(Direction.Left);
                    enemy.Facing = Direction.Left;
                }
                else
                {
                    enemy.Move(Direction.Right);
                    enemy.Facing = Direction.Right;
                }
            }
            else
            {
                if(ydist >= 0)
                {
                    enemy.Move(Direction.Down);
                    enemy.Facing = Direction.Down;
                }
                else
                {
                    enemy.Move(Direction.Up);
                    enemy.Facing = Direction.Up;
                }
            }
        }

        // Returns true if the final level has been beaten
        public bool IsGameWon()
        {
            int lastFloor = this.Floors.Count - 1;
            return this.Floors[lastFloor].LevelComplete();
        }

        // Returns true if the character has lost all his health.
        public bool IsGameOver()
        {
            return this.adventurer.Health == 0;
        }

        /// <summary>
        /// Creates and returns a list of stringified state data for this GameController.
        /// List begins with a header ("GameController") in index 0, and then serializes each property in turn
        /// </summary>
        /// <returns>list of game state information to be saved</returns>
        public List<string> Serialize()
        {
            List<string> data = new List<string>();
            data.Add("GameController");
            data.Add(CurrentFloor.ToString());
            data.Add(Setting.ToString());
            data.Add(Score.ToString());

            return data;
        }

        /// <summary>
        /// Finds and extracts the correct information from the array prepared by Load()
        /// Converts each string to the correct data and updates its state
        /// </summary>
        /// <param name="savedData">Array of string data extracted by Load() from file</param>
        public void Deserialize(string[] savedData)
        {
            CurrentFloor = Convert.ToInt32(savedData[0]);  
            string difficulty = savedData[1];
            Score = Convert.ToInt32(savedData[2]);
            switch (difficulty)
            {
                case "Easy":
                    Setting = Difficulty.Easy;
                    break;
                case "Medium":
                    Setting = Difficulty.Medium;
                    break;
                case "Hard":
                    Setting = Difficulty.Hard;
                    break;
            }
        }

        /// <summary>
        /// Creates a new file containing state data
        /// name and filepath are selected by user
        /// </summary>
        /// <param name="filename">path provided by Windows dialog box</param>
        public async void Save(string filename) // need to take path as param
        {
            List<List<string>> allSavedData = new List<List<string>>();

            using (StreamWriter writer = new StreamWriter(filename))  
            {
                // start the file off with a "header" that will tell Load that this is a valid file
                await writer.WriteAsync("ToTSave");

                // collect all data to be saved
                allSavedData.Add(Serialize());
                allSavedData.Add(Floors[CurrentFloor].Serialize());
                foreach (Enemy enemy in Floors[CurrentFloor].Enemies)
                {
                    allSavedData.Add(enemy.Serialize());
                }
                allSavedData.Add(adventurer.Serialize());

                // write to file 
                foreach (List<string> classData in allSavedData)
                {
                    foreach (string property in classData)
                    {
                        await writer.WriteAsync("," + property);
                    }
                }
            } // using
        } // Save

        /// <summary>
        /// Loads a previous in-progess game by launching ToT and updating the state to match the file's information
        /// TODO: exception handling
        /// </summary>
        /// <param name="dotDat">User-selected ToT.dat file from dialog box</param>
        public async void Load(string dotDat)
        {
            string file;

            using (StreamReader reader = new StreamReader(dotDat))
            {
                file = await reader.ReadToEndAsync();
            }

            string[] gameData = file.Split(',');

            // check for valid save file 
            if (gameData[0] == "ToTSave")
            {
                string[] gcData = new string[3];
                int gcIndex = Array.IndexOf(gameData, "GameController");
                Array.Copy(gameData, (gcIndex + 1), gcData, 0, 3);
                Deserialize(gcData);

                string[] levelData = new string[2];
                int levelIndex = Array.IndexOf(gameData, "Level");
                Array.Copy(gameData, (levelIndex + 1), levelData, 0, 2);
                Floors[CurrentFloor].Deserialize(levelData);

                int iteration = 0;
                string[] enemyData = new string[8];

                foreach (Enemy enemy in Floors[CurrentFloor].Enemies)
                {
                    int enemyIndex = Array.IndexOf(gameData, "Enemy");
                    if (iteration == 0)
                    {
                        Array.Copy(gameData, (enemyIndex + 1), enemyData, 0, 8);
                    }
                    else
                    {
                        int newIndex = Array.IndexOf(gameData, "Enemy", (enemyIndex + iteration + (8 * iteration)));
                        Array.Copy(gameData, (newIndex + 1), enemyData, 0, 8);
                    }
                    ++iteration;
                    enemy.Deserialize(enemyData);
                }
                
                int characterIndex = Array.IndexOf(gameData, "Character");
                string[] characterData = new string[gameData.Length - characterIndex - 1];
                Array.Copy(gameData, (characterIndex + 1), characterData, 0, gameData.Length - characterIndex - 1);
                adventurer.Deserialize(characterData);
            }
            else
            {
                // launch new game instead... Still can't get the event handler to catch exceptions...
            }
        }
    }
}
