public class DevotionFactory
{
    public IDevotion GetDevotions(CardClass cardClass, int number)
    {
        switch (cardClass)
        {
            case CardClass.Pyromancer:
                break;
            case CardClass.Warlock:
                break;
            case CardClass.Priest:
                if (number == 0) return new VoltagePriest();
                return null;
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
