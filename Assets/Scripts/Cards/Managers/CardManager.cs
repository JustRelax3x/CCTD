using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager
{
    private List<Card> _orirginalDeck = new List<Card>();
    private List<Card> _drop = new List<Card>();
    private Stack<Card> _shuffledDeck = new Stack<Card>();
    private Card[] _hand = new Card[4];
    public int MaxCardsAmount => _orirginalDeck.Count;

    public void Initialize(PlayerDeck playerDeck)
    {
        int size = playerDeck.Deck.Length;
        _orirginalDeck.Clear();
        for (int i = 0; i < size; i++)
        {
            _orirginalDeck.Add(playerDeck.Deck[i]);
        }
    }

    private void Shuffle(ref List<Card> cards)
    {
        int shuffleAmount = cards.Count - 1;
        int index;
        Card temp;
        while (shuffleAmount > 0)
        {
            index = Random.Range(0, cards.Count);
            temp = cards[index];
            cards[index] = cards[shuffleAmount];
            cards[shuffleAmount] = temp;
            shuffleAmount--;
        }
    }

    public Card GetHandCard(int slot) 
    {
        if (slot < 0 || slot >= _hand.Length) return null;
        return _hand[slot];
    }

    public void CardUsed(int slot)
    {
        if (_hand[slot].Rarity != CardRarity.Legendary)
        {
            _drop.Add(_hand[slot]);
        }
        DealCard(slot);

        if (_shuffledDeck.Count == 0)
        {
            Shuffle(ref _drop);
            PushStack(ref _drop);
        }
    }
    private void FillHand()
    {
        for (int i = 0; i < _hand.Length; i++)
        {
            DealCard(i); 
        }
    }

    private void PushStack(ref List<Card> cards)
    {
        _shuffledDeck.Clear();
        foreach (Card card in cards)
        {
            _shuffledDeck.Push(card);
        }
        cards.Clear();
    }

    private void DealCard(int slotNumber)
    {
        if (slotNumber < 0 || slotNumber >= _hand.Length) return;
        _hand[slotNumber] = _shuffledDeck.Pop();
    }

    public void Clear()
    {
        _drop.Clear();
        _drop.AddRange(_orirginalDeck);
        Shuffle(ref _drop);
        PushStack(ref _drop);
        FillHand();
    }
}
