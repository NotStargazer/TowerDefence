using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Wave
{
    public int smallUnits;
    public int largeUnits;
    public int TotalEnemies { get { return smallUnits + largeUnits; } }
}
