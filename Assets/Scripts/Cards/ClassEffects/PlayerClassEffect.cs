using UnityEngine;
[CreateAssetMenu(menuName = "Player/PlayerClassEffects")]
public class PlayerClassEffect : ScriptableObject
{
    public IClassEffect[] ClassEffects = new IClassEffect[sizeof(CardClass)];
}

