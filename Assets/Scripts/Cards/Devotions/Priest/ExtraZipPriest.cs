
public class ExtraZipPriest : IDevotion
{
    public const EffectType Effect = EffectType.ElectrisityExtraZip;
    public const CardClass CClass = CardClass.Priest;
    public bool BuffStatus()
    {
        return GameController.GetHpProcent() >= 1;
    }

    public string Description()
    {
        throw new System.NotImplementedException();
    }

    public string GetImageName()
    {
        throw new System.NotImplementedException();
    }
}

