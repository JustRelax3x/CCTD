using UnityEngine;

public class MenuScreenPresenter : MonoBehaviour
{
    [SerializeField]
    private ScreenAnimator[] _screens;
    [SerializeField]
    private int _currentScreen = 0;
    [SerializeField]
    private int _energyScreen = 1;

    public int EnergyScreen => _energyScreen;

    private bool _isChanging = false;
    
    public void ChangeScreen(int num)
    {
        if (num == _currentScreen || num >= _screens.Length || _isChanging == true) return;
        bool dir;
        dir = num < _currentScreen;
        _isChanging = true;
        _screens[_currentScreen].CloseScreen(dir);
        _screens[num].AnimationDone += Changed;
        _screens[num].OpenScreen(dir);
        _currentScreen = num;
    }

    private void Changed()
    {
        _isChanging = false;
        _screens[_currentScreen].AnimationDone -= Changed;
    }
} 