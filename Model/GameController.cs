﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerOfTerror.Model
{
    // Indicates whether the entity was hit, missed, killed, or dropped
    public enum AttackResult { Hit, Miss, Kill, Drop }

    // Sets the difficulty of the level
    public enum Difficulty { Easy, Medium, Hard }

    // Gives the direction an entity is facing or moving
    public enum Direction { Up, Down, Left, Right }


    // Manages game logic
    class GameController : ISerializable
    {
        public int CurrentFloor { get; set; }
        public List<Level> Floors { get; set; }
        public Difficulty Setting { get; set; }
        public bool Cheating { get; set; }
        public Level currentFloor;
        public Level nextFloor;
        public Character adventurer;

        public static Random rand = new Random();
        
        public GameController()
        {
            this.Floors = new List<Level>();
            this.Setting = Difficulty.Easy;
            this.adventurer = new Character();
        }

        public void BuildTower()
        {
            this.Floors.Add(new Level(LevelType.Basic));
            this.Floors.Add(new Level(LevelType.Basic));
            this.Floors.Add(new Level(LevelType.Basic));
            this.currentFloor = Floors[0];
            this.nextFloor = Floors[1];
        }

        // Populate the Level list with three levels
        public void Setup()
        {
            // Adjust character health according to difficulty
            if (!this.Cheating) {
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
        public void PlayerAttack(Entity character, Entity enemy)
        {
            character.Attack(enemy);
        }

        // Does the logic for when the player moves
        // Moves the enemies in a random direction
        public void UpdatePositions(Character character, Direction direction)
        {
            character.Move(direction);
            foreach (Entity enemy in currentFloor.Enemies)
            {
                //Need to Fix
                int i = rand.Next(1, 5);
                Direction dir;
                switch(i)
                {
                    case 1:
                        dir = Direction.Up;
                        break;
                    case 2:
                        dir = Direction.Down;
                        break;
                    case 3:
                        dir = Direction.Left;
                        break;
                    case 4:
                        dir = Direction.Right;
                        break;
                    default:
                        dir = Direction.Up;
                        break;
                }
                enemy.Move(dir);
            }
        }

        // Returns whether or not the game has ended (player completes final level or dies)
        public bool IsGameOver()
        {
            int lastFloor = this.Floors.Count - 1;
            return this.Floors[lastFloor].LevelComplete();
        }

        // Records the result of an attack and does the corresponding logic
        public AttackResult AttackResult()
        {
            return Model.AttackResult.Hit;
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

            return data;
        }

        /// <summary>
        /// Finds and extracts the correct information from the array prepared by Load()
        /// Converts each string to the correct data and updates its state
        /// </summary>
        /// <param name="savedData">Array of string data extracted by Load() from file</param>
        public void Deserialize(string[] savedData)
        {
            CurrentFloor = Convert.ToInt32(savedData[0]);  // <-- VERY FRAGILE, need to confirm exactly how leveling is going to be handled
            string difficulty = savedData[1];
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
                //allSavedData.Add(currentFloor.Serialize());
                //foreach (Enemy enemy in Floors[CurrentFloor].Enemies)
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

            try
            {
                using (StreamReader reader = new StreamReader(dotDat))
                {
                    file = await reader.ReadToEndAsync();
                }

                string[] gameData = file.Split(',');

                // check for valid save file 
                if (gameData[0] == "ToTSave")
                {
                    string[] gcData = new string[2];
                    int gcIndex = Array.IndexOf(gameData, "GameController");
                    Array.Copy(gameData, (gcIndex + 1), gcData, 0, 2);
                    //GameController gc = new GameController();
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
                            Array.Copy(gameData, (enemyIndex - 1), enemyData, 0, 8);
                        }
                        else
                        {
                            int newIndex = Array.IndexOf(gameData, "Enemy", (enemyIndex + iteration + (8 * iteration)));
                        }
                        ++iteration;
                        enemy.Deserialize(enemyData);
                    }

                    string[] characterData = new string[9];
                    int characterIndex = Array.IndexOf(gameData, "Character");
                    Array.Copy(gameData, (characterIndex - 1), characterData, 0, 9);
                    adventurer.Deserialize(characterData);
                }
                else
                {
                    //is being read line by line after going through the try block... but is not throwing anything...
                    //throw new FileFormatException();
                }
            }
            catch
            {
                //throw new FileNotFoundException(); // is not being caught...
            }


        }
    }
}
