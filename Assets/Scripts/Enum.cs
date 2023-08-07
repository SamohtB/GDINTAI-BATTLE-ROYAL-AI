using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.AttributeUsage(System.AttributeTargets.Field, Inherited = true)]
public class ReadOnlyAttribute : PropertyAttribute { }

public class Enum
{
    public enum Faction { None = -1, HighElf = 0, DarkElf }
    public enum PowerUpType { None = -1, SpeedUp = 0, SpeedDown, BaseShield, Mines, Chaos }
    public enum Direction { NONE = -1, North = 0, South, East, West }

    public enum ObjectType { Base = 0, Tank, Bullet }
}   
