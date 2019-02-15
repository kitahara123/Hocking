using Managers;

public class PlayerCharacter : Creature
{
    public Inventory Inventory { get; private set; }
    
    
    protected override void Start()
    {
        base.Start();
        Messenger<int, int>.Broadcast(GameEvent.HEALTH_UPDATED, HP, maxHP);
        Managers.Managers.Player.Player = this;
        Inventory = new Inventory();
        OnDeath += OnPlayerDeath;

    }

    public override void ChangeHealth(int value)
    {
        base.ChangeHealth(value);
        Messenger<int, int>.Broadcast(GameEvent.HEALTH_UPDATED, HP, maxHP);
        
        if (HP == 0) Messenger.Broadcast(GameEvent.LEVEL_FAILED);
    }
    
    public override void UpdateData(int value, int value1)
    {
        base.UpdateData(value, value1);
        Messenger<int, int>.Broadcast(GameEvent.HEALTH_UPDATED, HP, maxHP);
    }

    private void OnPlayerDeath(Creature player)
    {
        
    }

}
