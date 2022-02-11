using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckBuilder : MonoBehaviour
{
    [SerializeField]
    private CardPrefab[] _playerDeckPrefab = new CardPrefab[Constants.DeckSize];
    private int _deckActiveCards = 0;
    private bool _removeMode;
    private PlayerDeck _playerDeck;

    private Stack<int> _freeSlots = new Stack<int>();

    private LevelSelector _levelSelector;
    public void Initialize(LevelSelector levelSelector, PlayerDeck deck)
    {
        _levelSelector = levelSelector;
        _playerDeck = deck;
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
            _deckActiveCards++;
           
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

    public void OnBackButtonClicked()
    {
        _levelSelector.SetDeckLength(_deckActiveCards);
    }
        
}
