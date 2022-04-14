using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
    public class DefenderHud : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _wavesValue; 
    [SerializeField]
    private TextMeshProUGUI _waves;
    [SerializeField]
    private TextMeshProUGUI _playerHealth;
    [SerializeField]
    private TextMeshProUGUI _playerMoney;
    [SerializeField]
    private Slider _hpBar;
    [SerializeField]
    private Button _quitButton;

    private float _prepareTimer;
    private int _currentWave=-1;


    private void Awake()
    {
        //_quitButton.onClick.AddListener(OnQuitButtonClicked);
    }
    public void UpdatePlayerHealth(float currentHp, float maxHp)
    {
        _playerHealth.text = $"{(int)(currentHp / maxHp * 100)}%";
        _hpBar.value = currentHp / maxHp;
    }
    public void UpdatePlayerMoney(int money)
    {
        _playerMoney.text = money.ToString();
    }

    public void UpdateScenarioWaves(int currentWave, int wavesCount)
    {
        if (_currentWave == currentWave) return;
        _wavesValue.text = $"{currentWave}/{wavesCount}";
        _currentWave = currentWave;
    }

    public void PrepareTime(float prepareTime)
    {
        if (prepareTime > 0)
        {
            _waves.text = "Time:"; //TODO localization
            _prepareTimer = prepareTime;
            StartCoroutine(PrepareTimer());
        }
        else
        {
            StopCoroutine(PrepareTimer());
            _waves.text = "WAVE:"; //TODO localization
        }
    }

    private void OnQuitButtonClicked()
    {
        //OnPauseClicked(true);
        //var isConfirmed = await AlertPopup.Instance.AwaitForDecision("Are you sure to quit?");
        //OnPauseClicked(false);
        //if (isConfirmed)
    }
    
    private IEnumerator PrepareTimer()
    {
        while (_prepareTimer > 0)
        {
            _wavesValue.text = _prepareTimer--.ToString();
            yield return new WaitForSeconds(1f);
        }
    }
}