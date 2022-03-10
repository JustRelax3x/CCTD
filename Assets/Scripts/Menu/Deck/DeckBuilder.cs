using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckBuilder : MonoBehaviour
{
    [SerializeField]
    private CardPrefab[] _playerDeckPrefab = new CardPrefab[Constants.DeckSize];
    [SerializeField]
    private CardPrefab[] _allCards;
    [SerializeField]
    private PlayerDeck _allGameCards;
    private int _deckActiveCards = 0;
    private bool _removeMode;
    private PlayerDeck _playerDeck;
    private Queue<CardClass> _devotionToBeChosen = new Queue<CardClass>();

    private Stack<int> _freeSlots = new Stack<int>();

    private LevelSelector _levelSelector;
    public void Initialize(LevelSelector levelSelector, PlayerDeck deck)
    {
        _levelSelector = levelSelector;
        _playerDeck = deck;
        InitializeCards();
        _freeSlots.Clear();
        int size = 0;
        while (size < Constants.DeckSize)
        {
            if (_playerDeck.Deck[size] == null) break;
            size++;
        }
        for (int i=0; i < size; i++)
        {
            _playerDeckPrefab[i].Card = _playerDeck.Deck[i];
            _playerDeckPrefab[i].gameObject.SetActive(true);
        }
        _deckActiveCards = size;
        while (size < Constants.DeckSize)
        {
            _freeSlots.Push(size);
            _playerDeckPrefab[size++].gameObject.SetActive(false);
        }
    }
    public void OnCardClicked(CardPrefab cardPrefab)
    {
        if (_deckActiveCards == Constants.DeckSize || _removeMode) return;
        Card card = cardPrefab.Card;
        bool flag = false;
        for (int i=0; i < Constants.DeckSize; i++)
        {
            if (!_playerDeckPrefab[i].gameObject.activeSelf) continue;
            if (_playerDeckPrefab[i].Card.Id == card.Id)
            {
                flag = true;
                break;
            } 
        }
        if (!flag)
        {
            int slot = _freeSlots.Pop();
            _playerDeckPrefab[slot].Card = card;
            _playerDeck.Deck[slot] = card;
            _playerDeckPrefab[slot].gameObject.SetActive(true);
            if (!_devotionToBeChosen.Contains(card.Class))
            _deckActiveCards++;
        }
        if (_deckActiveCards == Constants.DeckSize)
        {
            CardClass cardClass;
            for (int i=0; i < _deckActiveCards; i++)
            {
                cardClass = _playerDeckPrefab[i].Card.Class;
                if (!_devotionToBeChosen.Contains(cardClass))
                {
                    _devotionToBeChosen.Enqueue(cardClass);
                }
            }

        }
    }
    public void OnRemoveButtonClicked(Image image)
    {
        _removeMode = !_removeMode;
        image.color = _removeMode ? Color.white : Color.red;
    }

    public void OnDeckCardClicked(int slot)
    {
        if (_removeMode)
        {
            _playerDeckPrefab[slot].gameObject.SetActive(false);
            _deckActiveCards--;
            _freeSlots.Push(slot);
        }
    }

    private void InitializeCards()
    {
        for (int i=0; i<_allCards.Length; i++) //to do: init only visible + 1 row, if scrolled -> all (more). 
        {
            _allCards[i].Card = _allGameCards.Deck[i];
        }
        if (_allGameCards.Deck.Length != _allCards.Length)
        {
            Debug.LogError("Game Cards length != card prefab length");
        }
    }

    public void OnBackButtonClicked()
    {
        _levelSelector.SetDeckLength(_deckActiveCards);
    }
        
}
