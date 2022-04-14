using Assets.Scripts.Cards;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SpellsVisualPresenter))]
[RequireComponent(typeof(BuffsHandler))]
public class GameController : MonoBehaviour
{

    [SerializeField]
    private GameBoard _board;

    [SerializeField]
    private GameTileContentFactory _contentFactory;

    [SerializeField]
    private WarFactory _warFactory;

    [SerializeField]
    private GameScenario _scenario;

    [Space]
    [Header("UI")]

    [SerializeField]
    private UIManager _uiManager;

    [SerializeField]
    private HandUI _handUI;

    [Space]
    [Header("Cards")]

    private CardManager _cardManager = new CardManager();

    [SerializeField]
    private PlayerDeck _playerDeck;

    private GameScenario.State _activeScenario;

    private HandManager _handManager = new HandManager();
    
    private BuffsHandler _buffsController; 
    
    private SpellsVisualPresenter _spellsVisualizer;

    [Space]
    [Header("Stats")]

    [SerializeField]
    private LevelStats _levelStats;

    private int _playerMoney;
    private bool _scenarioInProcess = false;
    private int PlayerMoney
    {
        get => _playerMoney;
        set
        {
            _playerMoney = Mathf.Max(0, value);
            _uiManager.UpdateMoney(_playerMoney);
        }
    }

    private int _playerHealth;
    private System.Action _hpChanged;

    private int PlayerHealth
    {
        get => _playerHealth;
        set
        {
            _playerHealth = Mathf.Max(0, value);
            _uiManager.UpdateHealth(_playerHealth, _levelStats.StartingPlayerHealth);
            _hpChanged?.Invoke();
        }
    }

    private GameBehaviorCollection _enemies = new GameBehaviorCollection();
    private GameBehaviorCollection _nonEnemies = new GameBehaviorCollection();

    private static GameController _instance;

  

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        _buffsController = GetComponent<BuffsHandler>(); 
        _spellsVisualizer = GetComponent<SpellsVisualPresenter>();
        _cardManager.Initialize(_playerDeck);
        _handManager.Initialize(_board, _cardManager, _handUI);
        _board.Initialize(_contentFactory,_buffsController,_handManager.CheckingTouch);
        _waitMoneyIncreasingDelay = new WaitForSeconds(_levelStats.MoneyIncreasingDelay);
        _waitPrepareTime = new WaitForSeconds(_levelStats.PrepareTime);
        BeginNewGame();
    }

    private void Update()
    {
        if (_scenarioInProcess)
        {
            var (currentWave, wavesCount) = _activeScenario.GetWaves();
            _uiManager.UpdateScenarioWaves(currentWave, wavesCount);
            if (PlayerHealth <= 0)
            {
                EndGame();
            }

            if (!_activeScenario.Progress() && _enemies.IsEmpty)
            {
                EndGame();
            }
        }

        _enemies.GameUpdate();
        Physics.SyncTransforms();
        _board.GameUpdate();
        _nonEnemies.GameUpdate();
    }

    private void EndGame()
    {
        if (_moneyIncreaser != null)
        {
            StopCoroutine(_moneyIncreaser);
        }
        _uiManager.SetGGScreen(true); 
        Time.timeScale = 0f;
    }

    public static void SpawnEnemy(EnemyFactory enemyFactory, EnemyType type)
    {
        GameTile spawn = _instance._board.GetSpawnPoint(Random.Range(0, _instance._board.SpawnPointCount));
        Enemy enemy = enemyFactory.Get(type);
        enemy.SpawnOn(spawn);
        _instance._enemies.Add(enemy);
    }


    public void BeginNewGame()
    {
        _scenarioInProcess = false;
        if (_prepareRoutine != null)
        {
            StopCoroutine(_prepareRoutine);
        }
        if (_moneyIncreaser != null)
        {
            StopCoroutine(_moneyIncreaser);
        }
        Clear();
        PlayerHealth = _levelStats.StartingPlayerHealth;
        PlayerMoney = _levelStats.StartingPlayerMoney;
        _prepareRoutine = StartCoroutine(PrepareRoutine());
        _uiManager.PrepareNewGame(_levelStats.PrepareTime);
        Time.timeScale = 1f;
    }

    public static void EnemyReachedDestination()
    {
        _instance.PlayerHealth--;
    }

    public static Enemy GetHighestHpEnemy()
    {
        return _instance._enemies.GetHighestHp().GetComponent<Enemy>();
    }

    public static Enemy GetRandomEnemy()
    {
        return _instance._enemies.GetRandom().GetComponent<Enemy>();
    }

    public static GameBehaviorCollection GetEnemies()
    {
        return _instance._enemies;
    }

    public static float GetHpProcent()
    {
        return _instance._playerHealth / _instance._levelStats.StartingPlayerHealth;
    }
    public static void SubscribeOnHpChanged(System.Action action)
    {
        _instance._hpChanged += action;
    }

    public static bool TrySpendMoney(int value)
    {
        if (_instance.PlayerMoney < value) {
            _instance._uiManager.ActivateNoGoldHint();
            return false;
        }
        _instance.PlayerMoney -= value;
        return true;
    }

    public void OnPauseClicked(bool isPaused)
    {
        _uiManager.SetPauseScreen(!isPaused);
        Time.timeScale = isPaused ? 1f : 0f;
    }

    public void Menu()
    {
        StopAllCoroutines();
        Clear();
        Time.timeScale = 1f;
        SceneManager.LoadScene(Constants.MainMenuSceneNumber);
    }

    private void Clear()
    {
        _enemies.Clear();
        _nonEnemies.Clear();
        _board.Clear();
        _handManager.Clear();
        _spellsVisualizer.StopAll();
    }

    private Coroutine _prepareRoutine;
    
    private Coroutine _moneyIncreaser;

    private WaitForSeconds _waitMoneyIncreasingDelay;

    private IEnumerator IncreaseMoney()
    {
        while (_scenarioInProcess)
        {
            yield return _waitMoneyIncreasingDelay;
            PlayerMoney++;
        }
    }
    public static void StartSpellCoroutine(IEnumerator enumerator)
    {
        _instance._spellsVisualizer.StartSpellCoroutine(enumerator);
    }

    public static void VisualizeSpell(Spell spell, Transform transform)
    {
        _instance._spellsVisualizer.VisualizeSpell(spell, transform);
    }

    private WaitForSeconds _waitPrepareTime;
    private IEnumerator PrepareRoutine()
    {
        yield return _waitPrepareTime;
        _activeScenario = _scenario.Begin();
        _uiManager.DisablePreparePhase();
        _scenarioInProcess = true;
        _moneyIncreaser = StartCoroutine(IncreaseMoney());
    }
}
