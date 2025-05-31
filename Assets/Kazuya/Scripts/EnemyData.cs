[System.Serializable]
public class EnemyData
{
    public string Type;
    public int eyes;
    public int rarity;
    public int bonus;

    public EnemyData(string type, int eyes, int rarity, int bonus)
    {
        this.Type = type;
        this.eyes = eyes;
        this.rarity = rarity;
        this.bonus = bonus;
    }
}
