using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{
    [SerializeField]
    private DefenderHud _defenderHud;
    [SerializeField]
    private GameObject _gg;
    [SerializeField]
    private GameObject _pause;
    [SerializeField]
    private TextMeshProUGUI _hintNoGold;
    private Animation _animNoGold;

    private void Start()
    {
        _animNoGold = _hintNoGold.gameObject.GetComponent<Animation>();
    }
    public void UpdateMoney(int value)
    {
        _defenderHud.UpdatePlayerMoney(value);
    }
    public void UpdateHealth(int playerHealth, int startingHealth)
    {
        _defenderHud.UpdatePlayerHealth(playerHealth, startingHealth);
    }
    public void UpdateScenarioWaves(int currentWave,int wavesCount)
    {
        _defenderHud.UpdateScenarioWaves(currentWave, wavesCount);
    }
    public void SetGGScreen(bool flag)
    {
        _gg.SetActive(flag);
    }
    public void SetPauseScreen(bool flag)
    {
        _pause.SetActive(flag);
    }
    public void PrepareNewGame(int prepareTime)
    {
        SetGGScreen(false);
        SetPauseScreen(false);
        _defenderHud.PrepareTime(prepareTime);
    }
    public void DisablePreparePhase()
    {
        _defenderHud.PrepareTime(-1);
    }
    public void ActivateNoGoldHint()
    {
        if (_animNoGold.isPlaying) return;
        _hintNoGold.enabled = true;
        _animNoGold.Play();
    }
}

