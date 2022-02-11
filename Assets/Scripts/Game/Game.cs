using Assets.Scripts.Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SpellsVisualPresenter))]
[RequireComponent(typeof(BuffsController))]
public class Game : MonoBehaviour
{
    public const float AttackInterval = 10f;

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
    private DefenderHud _defenderHud;

    [SerializeField]
    private HandUI _handUI;

    [SerializeField]
    private GameObject _gg; //поменять
    
    [SerializeField]
    private GameObject _pause;

    [Space]
    [Header("Cards")]

    private CardManager _cardManager = new CardManager();

    [SerializeField]
    private PlayerDeck _playerDeck;

    private GameScenario.State _activeScenario;

    private HandManager _buildManager = new HandManager();
    
    private BuffsController _buffsController; 
    
    private SpellsVisualPresenter _spellsVisualizer;

    [Space]
    [Header("Stats")]

    [SerializeField, Range(1, 100)]
    private int _startingPlayerHealth = 20;

    private bool _scenarioInProcess = false;

    [SerializeField, Range(1, 100)]
    private int _startingPlayerMoney = 10;
    
    [SerializeField, Range(0, 30)]
    private int _prepareTime = 10;

    [SerializeField]
    private float _moneyIncreasingDelay = 1f;

    private int _playerMoney;

    private int PlayerMoney
    {
        get => _playerMoney;
        set
        {
            _playerMoney = Mathf.Max(0, value);
            _defenderHud.UpdatePlayerMoney(_playerMoney);
        }
    }

    private int _playerHealth;

    private int PlayerHealth
    {
        get => _playerHealth;
        set
        {
            _playerHealth = Mathf.Max(0, value);
            _defenderHud.UpdatePlayerHealth(_playerHealth, _startingPlayerHealth);
        }
    }

    private GameBehaviorCollection _enemies = new GameBehaviorCollection();
    private GameBehaviorCollection _nonEnemies = new GameBehaviorCollection();

    private static Game _instance;

  

    private void OnEnable()
    {
        _instance = this;
    }

    private void Start()
    {
        _buffsController = GetComponent<BuffsController>(); 
        _spellsVisualizer = GetComponent<SpellsVisualPresenter>();
        _cardManager.Initialize(_playerDeck);
        _buildManager.Initialize(_board, _cardManager, _handUI);
        _board.Initialize(_contentFactory,_buffsController,_buildManager.CheckingTouch);
      
        BeginNewGame();
    }

    private void Update()
    {
        if (_scenarioInProcess)
        {
            var (currentWave, wavesCount) = _activeScenario.GetWaves();
            _defenderHud.UpdateScenarioWaves(currentWave, wavesCount);
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
        _gg.SetActive(true); 
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
        _gg.SetActive(false); 
        _pause.SetActive(false); 
        _scenarioInProcess = false;
        if (_prepareRoutine != null)
        {
            StopCoroutine(_prepareRoutine);
        }
        if (_moneyIncreaser != null)
        {
            StopCoroutine(_moneyIncreaser);
        }
        _enemies.Clear();
        _nonEnemies.Clear();
        _board.Clear();
        _buildManager.Clear();
        PlayerHealth = _startingPlayerHealth;
        PlayerMoney = _startingPlayerMoney;
        _prepareRoutine = StartCoroutine(PrepareRoutine());
        _defenderHud.PrepareTime(_prepareTime);
        _spellsVisualizer.StopAll();
        Time.timeScale = 1f;
    }

    public static void EnemyReachedDestination()
    {
        _instance.PlayerHealth--;
    }

    public static Enemy GetHighestHpEnemy()
    {
        return _instance._enemies.GetHighestHp()?.GetComponent<Enemy>();
    }

    public static Enemy GetRandomEnemy()
    {
        return _instance._enemies.GetRandom()?.GetComponent<Enemy>();
    }

    public static GameBehaviorCollection GetEnemies()
    {
        return _instance._enemies;
    }
    public static bool TrySpendMoney(int value)
    {
        if (_instance.PlayerMoney < value) return false;
        _instance.PlayerMoney -= value;
        return true;
    }

    public void OnPauseClicked(bool isPaused)
    {
        _pause.SetActive(!isPaused);
        Time.timeScale = isPaused ? 1f : 0f;
    }

    public void Menu()
    {
        StopAllCoroutines();
        _enemies.Clear();
        _nonEnemies.Clear();
        _board.Clear();
        _buildManager.Clear();
        Time.timeScale = 1f;
        SceneManager.LoadScene(Constants.MainMenuSceneNumber);
    }

    private Coroutine _prepareRoutine;
    
    private Coroutine _moneyIncreaser;

    private IEnumerator IncreaseMoney()
    {
        while (_scenarioInProcess)
        {
            yield return new WaitForSeconds(_moneyIncreasingDelay);
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
    private IEnumerator PrepareRoutine()
    {
        yield return new WaitForSeconds(_prepareTime);
        _activeScenario = _scenario.Begin();
        _defenderHud.PrepareTime(-1);
        _scenarioInProcess = true;
        _moneyIncreaser = StartCoroutine(IncreaseMoney());
    }
}
