
public class DevotionHandler
{
    private static bool[] _devotionsStatus = new bool[sizeof(CardClass)];

    private static IDevotion[] _currentDevotions = new IDevotion[sizeof(CardClass)];

    public void ActivateDevotion(CardClass cardClass, IDevotion devotion)
    {
        _devotionsStatus[(int)cardClass] = true;
        _currentDevotions[(int)cardClass] = devotion;
    }

    public bool DevotionBuffStatus(CardClass cardClass, IDevotion devotion)
    {
        int num = (int)cardClass;
        if (!_devotionsStatus[num] || _currentDevotions[num] != devotion) return false;
        return _currentDevotions[num].BuffStatus();
    }

}

