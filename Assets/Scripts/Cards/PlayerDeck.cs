using UnityEngine;

[CreateAssetMenu(menuName ="PlayerDeck")]
public class PlayerDeck : ScriptableObject
{
    public Card[] Deck = new Card[Constants.DeckSize];    
}
