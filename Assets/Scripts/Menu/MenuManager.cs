using UnityEngine;
using Assets.Scripts;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private LevelSelector _levelSelector;
    [SerializeField]
    private DeckBuilder _deckBuilder;
    [SerializeField]
    private MenuScreenPresenter _screenPresenter;
    [SerializeField]
    private PlayerDeck _deck;

    private int _deckActiveCards = 0;

    //private SaveSystem _save = new SaveSystem();

    private void Start()
    {
        _levelSelector.Initialize(_screenPresenter);
        _deckBuilder.Initialize(_levelSelector, _deck);
        _deckActiveCards = 0;
        while (_deckActiveCards < Constants.DeckSize)
        {
            if (_deck.Deck[_deckActiveCards] == null)
            {
                break;
            }
            _deckActiveCards++;
        }
        _levelSelector.SetDeckLength(_deckActiveCards);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
        
    public void AddCardToDeck(Card card, int slot)
    {
        _deck.Deck[slot] = card;
        _deckActiveCards++;
        _levelSelector.SetDeckLength(_deckActiveCards);
    }

    //private void OnApplicationPause(bool pause)
    //{
    //    if (pause)
    //    {
    //        _energyManager.Recycle();
    //        _save.SaveData();
    //    }
    //    else
    //    {
    //        _save.LoadLastSave();
    //        _energyManager.Initialize(Player.Energy, Player.TIMEToAddEnergy, Player.MAXEnergy, Player.TimeLeftToAddEnergy);
    //    }
    //}
//    private void OnApplicationQuit()
//    {
//        _save.SaveData();
//    }
}
