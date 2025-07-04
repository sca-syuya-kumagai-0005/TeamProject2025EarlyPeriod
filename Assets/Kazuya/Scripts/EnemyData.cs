[System.Serializable]
public class EnemyData
{
    public int eyes;
    public int eEye,tEye,nRed,tRed,nBue,tBlue;
    public int rarity;
    public int bonus;

    public EnemyData(int eyes,int eEye ,int tEye,int nRed, int tRed,int nBue,int tBlue, int rarity, int bonus)
    {
        this.eyes = eyes;
        this.eEye = eEye;
        this.tEye = tEye;
        this.nRed = nRed;
        this.tRed = tRed;
        this.nBue = nBue;
        this.tBlue = tBlue;
        this.rarity = rarity;
        this.bonus = bonus;
    }
}
