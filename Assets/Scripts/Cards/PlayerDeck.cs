using UnityEngine;

[CreateAssetMenu(menuName ="Player/PlayerDeck")]
public class PlayerDeck : ScriptableObject
{
    public Card[] Deck = new Card[Constants.DeckSize];    
}
