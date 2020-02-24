using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : Singleton<PlayerStats>
{
    public float MovementSpeed { get; set; }
    public float RotationSpeed { get; set; }
    public float Hunger { get; set; }
}
