public class ClassEffectHandler
{
    private static bool[] _ClassEffectStatus = new bool[sizeof(CardClass)];

    private static IClassEffect[] _currentEffects = new IClassEffect[sizeof(CardClass)];

    public void ActivateEffect(CardClass cardClass, IClassEffect effect)
    {
        _ClassEffectStatus[(int)cardClass] = true;
        _currentEffects[(int)cardClass] = effect;
    }

    public void ActivateEffect(int i, IClassEffect effect)
    {
        _ClassEffectStatus[i] = true;
        _currentEffects[i] = effect;
    }

    public bool EffectBuffStatus(CardClass cardClass, IClassEffect effect)
    {
        int num = (int)cardClass;
        if (!_ClassEffectStatus[num] || _currentEffects[num].GetId() != effect.GetId()) return false;
        return _currentEffects[num].BuffStatus();
    }

    public void GetAllActiveEffects(IClassEffect[] effects)
    {
        if (effects.Length != _currentEffects.Length) return;
        _currentEffects.CopyTo(effects, 0);
    }

}

