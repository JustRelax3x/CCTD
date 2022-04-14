using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.U2D;

public class ClassEffectSelector : MonoBehaviour
{
    [SerializeField]
    private Button[] _effectButton;
    [SerializeField]
    private TextMeshProUGUI[] _descriptions;
    [SerializeField]
    private SpriteAtlas _effectsSprites;

    private Queue<CardClass> _effectClass;

    private readonly ClassEffectFactory _effectFactory = new ClassEffectFactory();

    private IClassEffect[] _classEffects;

    private ClassEffectHandler _classEffectHandler = new ClassEffectHandler();

    private CardClass _cardClass;

    public void InitClassEffects(Queue<CardClass> cardClasses)
    {
        _effectClass = cardClasses;
        _classEffects = new IClassEffect[_effectButton.Length];
        ShowEffect();
        
    }

    private void ShowEffect()
    {
        if (_effectClass.Count == 0)
        {
            gameObject.SetActive(false);
            return;
        }
        _cardClass = _effectClass.Dequeue();
        int length = _effectButton.Length;
        for (int i=0; i < length; i++)
        {
            _classEffects[i] = _effectFactory.GetClassEffect(_cardClass, i);
            AddListener(_effectButton[i], i);
            //_descriptions[i].text = _devotions[i].Description();
            //_devotionButton[i].GetComponent<Image>().sprite = _devotionSprites.GetSprite(_devotions[i].GetImageName());
        }
        
        gameObject.SetActive(true);
    }
    private void AddListener(Button b, int value)
    {
        b.onClick.AddListener(() => EffectChosen(value));
    }

    private void EffectChosen(int i)
    {
        _classEffectHandler.ActivateEffect(_cardClass, _classEffects[i]);
        ShowEffect();
    }


}

