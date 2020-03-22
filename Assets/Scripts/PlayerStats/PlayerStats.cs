using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : Singleton<PlayerStats>
{
    public float Hunger { get; set; }
    public int GoodBoyPoints { get; set; }

    private float originalHunger;
    private int originalGoodBoyPoints;

    private void Start()
    {
        originalHunger = Hunger;
        originalGoodBoyPoints = GoodBoyPoints;
    }

    public void Reset()
    {
        Hunger = originalHunger;
        GoodBoyPoints = originalGoodBoyPoints;
    }
}
