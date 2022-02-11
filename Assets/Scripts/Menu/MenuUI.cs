using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [SerializeField]
    private MenuScreenPresenter _presenter;
    [SerializeField]
    private Button[] _screenButtons;
    [SerializeField]
    private SpriteAtlas _atlas;
    [SerializeField]
    private Image _vibration;
    [SerializeField]
    private Image _volume;

    private void AddListener(Button b, int value)
    {
        b.onClick.AddListener(() => _presenter.ChangeScreen(value));
    }

    private void Start()
    {
        for(int i=0; i<_screenButtons.Length; i++)
        {
            AddListener(_screenButtons[i], i);
        }
    }


    public void OnVolumeClicked()
    {
        //Player.Volume = !Player.Volume;
        //ChangeVolumeSprite(Player.Volume);
    }

    public void OnVibroClicked()
    {
        //Player.Vibration = !Player.Vibration;
        //ChangeVibrationSprite(Player.Vibration);
    }

    public void OnLanguageClicked()
    {
        //int n = Player.Language;
        //Player.Language = n == Player.MAXLanguage - 1 ? 0 : n+1;
        //Assets.SimpleLocalization.LocalizationManager.ChangeLanguage(Player.Language);
    }

    private void UpdateChangeableUI(bool VibroIsActive, bool VolumeIsActive)
    {
        ChangeVibrationSprite(VibroIsActive);
        ChangeVolumeSprite(VolumeIsActive);
    }

    private void ChangeVibrationSprite(bool VibroIsActive) => _vibration.sprite = VibroIsActive ? _atlas.GetSprite("haptic") : _atlas.GetSprite("phone");
    private void ChangeVolumeSprite(bool VolumeIsActive) => _volume.sprite = VolumeIsActive ? _atlas.GetSprite("audioOn") : _atlas.GetSprite("audioOff");


}
