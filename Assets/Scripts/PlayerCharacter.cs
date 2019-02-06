using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class PlayerCharacter : Creature
{
    public Inventory Inventory { get; private set; }
    
    
    protected override void Start()
    {
        base.Start();
        Messenger<int, int>.Broadcast(GameEvent.HEALTH_UPDATED, HP, maxHP);
        Managers.Managers.Player.Player = this;
        Inventory = new Inventory();
        
    }

    public override void ChangeHealth(int value)
    {
        base.ChangeHealth(value);
        Messenger<int, int>.Broadcast(GameEvent.HEALTH_UPDATED, HP, maxHP);
    }
    

}
