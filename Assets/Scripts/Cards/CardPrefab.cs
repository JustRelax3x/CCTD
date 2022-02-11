using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardPrefab : MonoBehaviour
{
    [SerializeField]
    private Card _card;
    public Card Card {
        get => _card;
        set {
            _card = value;
            UpdateInfo();
        }
    }
    [SerializeField]
    private TextMeshProUGUI _damage;
    [SerializeField]
    private TextMeshProUGUI _range;
    [SerializeField]
    private TextMeshProUGUI _cost;
    [SerializeField]
    private Image _sprite;

    private void OnEnable()
    {
        UpdateInfo();
    }

    private void UpdateInfo()
    {
        _damage.text = _card.Damage.ToString();
        _range.text = _card.Range.ToString();
        _cost.text = _card.Cost.ToString();
        _sprite.sprite = _card.Sprite;
    }
}
