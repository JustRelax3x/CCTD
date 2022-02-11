using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

namespace Assets.Scripts.Cards
{
    public class HandUI : MonoBehaviour
    {
        private const int HandSize = 4;
        [SerializeField]
        private TextMeshProUGUI[] _damageValueText = new TextMeshProUGUI[HandSize];
        [SerializeField]
        private TextMeshProUGUI[] _rangeValueText = new TextMeshProUGUI[HandSize];
        [SerializeField]
        private TextMeshProUGUI[] _costValueText = new TextMeshProUGUI[HandSize];
        [SerializeField]
        private Image[] _cardSprites = new Image[HandSize];
        [SerializeField]
        private Animator[] _cardAnimation = new Animator[HandSize];
        [SerializeField]
        private TextMeshProUGUI[] _deckAndDrop = new TextMeshProUGUI[2];
        [SerializeField]
        private Button _removeButton;


        public event UnityAction<int> CardSelected;

        public void Initialize(UnityAction RemoveAction)
        {
            _removeButton.onClick.AddListener(RemoveAction);
        }
        public void Select(int slot)
        {
            CardSelected?.Invoke(slot);
        }

        public void CardTouched(int slot)
        {
            _cardAnimation[slot].SetTrigger("CardTouched");
        }

        public void GiveCard(int slot)
        {
            _cardAnimation[slot].SetTrigger("GiveCard");
        }

        public void CardUIUpdate(Card card, int slot, int deckAmount, int dropAmount)
        {
            //if (card.Content == GameTileContentType.Spell) return;
            _damageValueText[slot].text = card.Damage.ToString();
            _rangeValueText[slot].text = card.Range.ToString();
            _costValueText[slot].text = card.Cost.ToString();
            _cardSprites[slot].sprite = card.Sprite;
            _deckAndDrop[0].text = deckAmount.ToString();
            if (dropAmount < 0) dropAmount = 0;
            _deckAndDrop[1].text = dropAmount.ToString();
        }

        public void ResetUI()
        {
            foreach (var v in _cardAnimation) v.Play("Default");
            SetAnimValue(0);
            GetComponent<Animation>().Play();
        }

        public void SetAnimValue(int x)
        {
            bool flag = x == 1;
            foreach (var v in _cardAnimation)
            {
                v.enabled = flag;
            }
        }
    }
}
