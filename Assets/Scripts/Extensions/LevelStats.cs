using UnityEngine;
[CreateAssetMenu(menuName = "Levels/LevelStats")]
public class LevelStats : ScriptableObject
{
    public int StartingPlayerHealth { get; private set; } = 20;
    public int StartingPlayerMoney { get; private set; } = 10;
    public float PrepareTime { get; private set; } = 5f;
    public float MoneyIncreasingDelay { get; private set; } = 3f;

    public void SetUpLevelStats(LevelStartMode levelStartMode)
    { 
        StartingPlayerHealth = 20;
        switch (levelStartMode)
        {
            case LevelStartMode.Default:
                {
                    StartingPlayerMoney = 10;
                    PrepareTime = 5f;
                    MoneyIncreasingDelay = 3f;
                }
                break;
            case LevelStartMode.NoGold:
                {
                    StartingPlayerMoney = 0;
                    PrepareTime = 5f;
                    MoneyIncreasingDelay = 2f;
                }
                break;
            case LevelStartMode.ExtraGold:
                {
                    StartingPlayerMoney = 15;
                    PrepareTime = 10f;
                    MoneyIncreasingDelay = 3f;
                }
                break;
        }
    }

    public enum LevelStartMode
    {
        Default,
        NoGold, 
        ExtraGold,
    }
}
