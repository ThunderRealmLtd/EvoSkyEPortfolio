////////////////////////////////////////////////////
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

public abstract class AbsUpgrade : MonoBehaviour
{
    #region Variables
    /// what does the upgrade do
    public string description_;

    /// Has the upgrade been chosen
    public bool chosen_ = false;
    #endregion
    #region Methods
    #endregion
}

#region SubClasses
public class FireRate : AbsUpgrade
{
}
#endregion