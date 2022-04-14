using Assets.Scripts.Cards;

public class HandManager 
{
    private const int HandSize = 4;
 
    private GameBoard _gameBoard;
    private HandUI _handUI;
    private Card _card;
    private State _currentState;

    private int _slot;

    private int _maxCardsAmount;
    private int _deckAmount;
    private int _usedLegendaryCards;

    private CardManager _cardManager;

    public void Initialize(GameBoard gameBoard, CardManager cardManager, HandUI handUI)
    {
        _gameBoard = gameBoard;
        _cardManager = cardManager;
        _handUI = handUI;
        _handUI.Initialize(OnRemoveButtonClicked);
        _handUI.CardSelected += OnCardSelected;
        _currentState = State.Null;
        _maxCardsAmount = _cardManager.MaxCardsAmount;
        _deckAmount = _maxCardsAmount;
        _usedLegendaryCards = 0;
    }
    public void OnCardSelected(int slot)
    {
        if (_slot != -1) _handUI.CardTouched(_slot);

        if (_slot == slot)
        {
            _slot = -1;
            _currentState = State.Null;
            return;
                       
        }
        _card = _cardManager.GetHandCard(slot);
        _slot = slot;
        _handUI.CardTouched(_slot);
        if (_card.Content >= GameTileContentType.LaserTower)
        {
            _currentState = State.Building;
            return;
        }
        if (_card.Content == GameTileContentType.Spell) {
            _currentState = State.Casting;
            return;
        }
        _currentState = State.Null;

    }

    public void CheckingTouch(GameTile tile)
    {
        if (_currentState == State.Null) return;
        bool usedCard = false;
        switch (_currentState)
        {
            case State.Building:
                usedCard = _gameBoard.TryBuild(tile, _card);
                break;
            case State.Casting:
                usedCard = _gameBoard.TryCastSpell(tile, _card);
                break;
            case State.Removing:
                _gameBoard.DestroyBuilding(tile);
                _currentState = State.Null;
                break;
        }
        if (usedCard)
        {
            if (_cardManager.GetHandCard(_slot).Rarity == CardRarity.Legendary)
            {
                _usedLegendaryCards++;
            }
            _cardManager.CardUsed(_slot);
            _handUI.GiveCard(_slot);
            CardUpdate(_cardManager.GetHandCard(_slot), _slot);
            _slot = -1;
            _currentState = State.Null;
        }
    }

    private void CardUpdate(Card card, int slot)
    {
        _deckAmount--;
        if (_deckAmount == 0) _deckAmount = _maxCardsAmount - HandSize - _usedLegendaryCards;
        _handUI.CardUIUpdate(card, slot, _deckAmount,_maxCardsAmount - _deckAmount - HandSize -_usedLegendaryCards);
    }

    public void Clear()
    {   
        _cardManager.Clear();
        _card = null;
        _currentState = State.Null;
        _usedLegendaryCards = 0;
        _handUI.ResetUI();
        _deckAmount = _maxCardsAmount;
        for (int i = 0; i < HandSize; i++)
        {
            CardUpdate(_cardManager.GetHandCard(i), i);
        }
        _slot = -1;
    }
    private void OnDestroy()
    {
        _handUI.CardSelected -= OnCardSelected;
    }

    public void OnRemoveButtonClicked()
    {
        _currentState = State.Removing;
    }
    private enum State
    {
        Null,
        Building,
        Casting,
        Removing,
    }
}
