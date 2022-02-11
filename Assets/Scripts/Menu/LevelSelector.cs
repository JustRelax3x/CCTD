using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
	[SerializeField]
	private Button[] _levelButtons;

	private MenuScreenPresenter _screenPresenter;

	private int _deckLength;

	public void Initialize(MenuScreenPresenter screenPresenter)
	{
		int levelReached = _levelButtons.Length;
		for (int i = 0; i < levelReached; i++)
		{
			AddListener(_levelButtons[i], i);
		}
		_screenPresenter = screenPresenter;
	}
	private void AddListener(Button b, int value)
	{
		b.onClick.AddListener(() => Select(value));
	}

	public void SetDeckLength(int length)
    {
		_deckLength = length;
    }

	public void Select(int level)
	{
		if (_deckLength != Constants.DeckSize)
        {
			_screenPresenter.ChangeScreen(_screenPresenter.EnergyScreen);
        }
		else
        {
			SceneManager.LoadScene(Constants.FirstLevelSceneNumber + level);
        }
	}
}