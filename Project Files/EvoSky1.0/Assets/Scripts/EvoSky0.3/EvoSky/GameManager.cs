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

public class GameManager : MonoBehaviour
{
    #region Variables
    /// The State of the game as a whole - TODO: have the game start at Menu
    public GameStates currentState_ = GameStates.Menu;

    /// The current game mode being played
    public AbsGameMode gameMode_ = null;

    #endregion
    #region Methods
    private void Start()
    {
        // TODO: for testing
        ToGamePlay();

    }

    private void FixedUpdate()
    {
        // Depending on the games current state
        // TITLE
        // Menu
        // ModeSelect
        // Gameplay

            
    }

    // STATE SWITCHING FUNCTIONS
    private void ToMenu()
    {
        // Switch the gamestate
        currentState_ = GameStates.Menu;
    }

    private void ToModeSelect()
    {
        // Switch the gamestate
        currentState_ = GameStates.ModeSelect;
    }

    private void ToGamePlay()
    {
        // Switch the gamestate
        currentState_ = GameStates.Gameplay;

        // TODO: copy creating in huplayer
        //gameMode_ = new Endless();
        //ScriptableObject.CreateInstance("Endless");
    }
    #endregion
}

public enum GameStates
{
    Splash,
    Title,
    Menu,
    ModeSelect,
    Gameplay

}