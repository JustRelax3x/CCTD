public class VoltagePriest : IClassEffect
{
    public const int Damage = 1;
    public const int Time = 5;
    public const CardClass CClass = CardClass.Priest;
    public bool BuffStatus()
    {
        return GameController.GetHpProcent() < 1;
    }
    public string Description()
    {
        throw new System.NotImplementedException();
    }

    public int GetId()
    {
        return 0;
    }

    public string GetImageName()
    {
        throw new System.NotImplementedException();
    }
}
