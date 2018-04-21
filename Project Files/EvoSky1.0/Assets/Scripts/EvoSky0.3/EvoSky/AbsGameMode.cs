///////////////////////////////////////////////////
//                                               //
//        Copyright Richard Lucas 2016           //
//                                               //
///////////////////////////////////////////////////


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;

using EvoSky;

public abstract class AbsGameMode : MonoBehaviour
{
    #region Variables
    /// Time limit in seconds
    public float timeLimit_ = 60;

    /// The number of AiPlayers
    public int numOfAi_ = 1;

    /// How difficult the enemies are
    public int dificulty_ = 1;

    // Gameobject for Human Player
    public GameObject HumanPlayer;

    // GameObject for AI players
    public GameObject ComPlayer;
        
    /// List of players - 0 being the HuPlayer
    public List<GameObject> players_;

    /// Array of pickups onScreen
    public PickUp[] pickUps_; 
    #endregion
    #region Methods
    private void Start()
    {
        if (!GameStart())
        {
            Debug.Log("GAME DID NOT START");
            Application.Quit();
        }
    }

    private void FixedUpdate()
    {
        GameUpdate();
    }
    /// Initialise the players and settings
    public bool GameStart()
    {
        // Populate the Players List
        for (int i = 0; i < numOfAi_ + 1; i++)
        {
            // If its the first player into the list
            if (i == 0)
            {
                // Add the HumanPlayer
                players_.Add(HumanPlayer);
            }
            else
            {
                // Add the correct number of AI players
                players_.Add(ComPlayer);
            } 
        }

        // Instantiate the players
        for (int i = 0; i < players_.Count; i++)
        {
            // If its the first palyer into the list
            if (i == 0)
            {
                // Instantiate the HuPlayer
                // TODO: cannot instantiate a player as the camera does not correctly follow the object.
                // the camera moves correctly but the player is not in the centre of the screen
                //Instantiate(players_[i], new Vector3(255.0f, 1.0f, 255.0f), Quaternion.identity);
            }
            else
            {
                // Instantiate the AiPlayers
                    
                // TODO: Find a random position in the playable area

                //Instantiate(players_[i], RandPos), Quaternion.identity);
            }
        }
        return true;
    }

    /// Updates the players
    public void GameUpdate()
    {
        throw new System.NotImplementedException();
    }

    /// Uninitialises the players and settings and returns to the menu
    public void GameEnd()
    {
        throw new System.NotImplementedException();
    }

    /// Has the game been won or lost?
    public bool GameWinLossCheck()
    {
        throw new System.NotImplementedException();
    }

    /// Change the difficulty of teh enemies - their level perhaps then randomise the upgrades of the enemy ships
    public void ChangeDifficulty()
    {
        throw new System.NotImplementedException();
    }
    #endregion
}