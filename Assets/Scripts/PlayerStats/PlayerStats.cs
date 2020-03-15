using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : Singleton<PlayerStats>
{
    public float Hunger { get; set; }
    public int GoodBoyPoints { get; set; }
}
