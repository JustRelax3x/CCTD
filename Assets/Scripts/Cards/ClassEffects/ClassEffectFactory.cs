public class ClassEffectFactory
{
    private readonly IClassEffect[] _priest = { new VoltagePriest(), new ExtraZipPriest() };
    public IClassEffect GetClassEffect(CardClass cardClass, int number)
    {
        switch (cardClass)
        {
            case CardClass.Pyromancer:
                break;
            case CardClass.Warlock:
                break;
            case CardClass.Priest:
                if (number >= _priest.Length) return _priest[0];
                return _priest[number];
            case CardClass.Wizard:
                break;
            case CardClass.Druid:
                break;
            case CardClass.Neutral:
                break;
        }
        return null;
    }
}
