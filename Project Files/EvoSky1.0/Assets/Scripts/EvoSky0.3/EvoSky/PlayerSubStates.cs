using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EvoSky
{
    public enum PlayerSubState
    {
        Idle,
        Dodging,
        Boosting,
        Upgrading
    }

    public enum PlayerMainState
    {
        Alive,
        Dead
    }

    public enum PlayerDodgeState
    {
        None,
        Flip, 
        LeftRoll, 
        RightRoll
    }
}