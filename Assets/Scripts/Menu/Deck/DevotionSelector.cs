using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.U2D;

public class DevotionSelector : MonoBehaviour
{
    [SerializeField]
    private Button[] _devotionButton;
    [SerializeField]
    private TextMeshProUGUI[] _descriptions;
    [SerializeField]
    private SpriteAtlas _devotionSprites;

    private Queue<CardClass> _devotionClass;

    private readonly DevotionFactory _devotionFactory = new DevotionFactory();

    private IDevotion[] _devotions;

    public void InitDevotions(Queue<CardClass> cardClasses)
    {
        _devotionClass = cardClasses;
        ShowDevotions();
    }

    private void ShowDevotions()
    {
        if (_devotionClass.Count == 0)
        {
            gameObject.SetActive(false);
            return;
        }
        CardClass cardClass = _devotionClass.Dequeue();
        int length = _devotionButton.Length;
        for (int i=0; i < length; i++)
        {
            _devotions[i] = _devotionFactory.GetDevotions(cardClass, i);
            _descriptions[i].text = _devotions[i].Description();
            AddListener(_devotionButton[i], i);
            _devotionButton[i].GetComponent<Image>().sprite = _devotionSprites.GetSprite(_devotions[i].GetImageName());
        }
        
        gameObject.SetActive(true);
    }
    private void AddListener(Button b, int value)
    {
        b.onClick.AddListener(() => DevotionChosen(value));
    }

    private void DevotionChosen(int i)
    {
        //
    }


}

