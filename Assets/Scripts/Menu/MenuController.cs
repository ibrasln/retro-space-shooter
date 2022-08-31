using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

public class MenuController : MonoBehaviour
{
    [SerializeField] TMP_Text gameName, twitterText, itchioText, linkedinText;
    [SerializeField] Image volumeButtonImage, twitterLogo, itchioLogo, linkedinLogo;
    [SerializeField] Sprite volumeOn, volumeOff;
    [SerializeField] Button playButton, gameplayButton, creditsButton, quitButton, musicButton;
    [SerializeField] AudioSource menuMusic;
    [SerializeField] RectTransform creditsPanelScale, gameplayPanelScale, powerUpsPanelScale;
    [SerializeField] CanvasGroup fadeScreenCG;

    private void Start()
    {
        fadeScreenCG.blocksRaycasts = true;
        StartCoroutine(OpenMenuObjects());
    }

    /// <summary>
    /// Opens Menu
    /// </summary>
    /// <returns></returns>
    IEnumerator OpenMenuObjects()
    {
        fadeScreenCG.DOFade(0f, 1f);
        yield return new WaitForSeconds(1.5f);
        menuMusic.Play();
        gameName.GetComponent<CanvasGroup>().DOFade(1f, 2f);

        yield return new WaitForSeconds(2.5f);
        ShowButton(playButton);

        yield return new WaitForSeconds(.3f);
        ShowButton(gameplayButton);

        yield return new WaitForSeconds(.3f);
        ShowButton(creditsButton);

        yield return new WaitForSeconds(.3f);
        ShowButton(quitButton);

        yield return new WaitForSeconds(.3f);
        ShowButton(musicButton);

        yield return new WaitForSeconds(.7f);
        ShowImage(twitterLogo);

        yield return new WaitForSeconds(.3f);
        ShowImage(itchioLogo);

        yield return new WaitForSeconds(.3f);
        ShowImage(linkedinLogo);

        yield return new WaitForSeconds(.7f);
        ShowText(twitterText);

        yield return new WaitForSeconds(.5f);
        ShowText(itchioText);

        yield return new WaitForSeconds(.5f);
        ShowText(linkedinText);
        fadeScreenCG.blocksRaycasts = false;
    }

    #region Menu Buttons
    public void Play()
    {
        StartCoroutine(PlayRoutine());
    }

    IEnumerator PlayRoutine()
    {
        SoundEffectController.instance.SoundEffect(0);
        fadeScreenCG.DOFade(1f, 1.5f);
        fadeScreenCG.blocksRaycasts = true;
        yield return new WaitForSeconds(1.6f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Gameplay()
    {
        SoundEffectController.instance.SoundEffect(0);
        OpenPanel(gameplayPanelScale);
    }

    public void Credits()
    {
        SoundEffectController.instance.SoundEffect(0);
        OpenPanel(creditsPanelScale);
    }

    public void Quit()
    {
        SoundEffectController.instance.SoundEffect(0);
        Application.Quit();
    }

    public void VolumeOnOff()
    {
        SoundEffectController.instance.SoundEffect(0);
        if (volumeButtonImage.sprite == volumeOn)
        {
            volumeButtonImage.sprite = volumeOff;
            menuMusic.volume = 0f;
        }
        else if (volumeButtonImage.sprite == volumeOff)
        {
            volumeButtonImage.sprite = volumeOn;
            menuMusic.volume = .5f;
        }
    }

    public void PowerUps()
    {
        StartCoroutine(PowerUpsRoutine());
    }

    IEnumerator PowerUpsRoutine()
    {
        SoundEffectController.instance.SoundEffect(0);
        gameplayPanelScale.DOScale(0f, .5f);
        yield return new WaitForSeconds(.6f);
        powerUpsPanelScale.DOScale(1f, .7f);
    }

    public void BackOfPowerUpsPanel()
    {
        StartCoroutine(BackOfPowerUpsPanelRoutine());
    }

    IEnumerator BackOfPowerUpsPanelRoutine()
    {
        SoundEffectController.instance.SoundEffect(0);
        powerUpsPanelScale.DOScale(0f, .5f);
        yield return new WaitForSeconds(.6f);
        gameplayPanelScale.DOScale(1f, .7f);
    }

    public void Back(RectTransform panelScale)
    {
        SoundEffectController.instance.SoundEffect(0);
        fadeScreenCG.DOFade(0f, 1f);
        fadeScreenCG.blocksRaycasts = false;
        panelScale.DOScale(0f, 1f);
    }

    #endregion

    public void ShowButton(Button button)
    {
        button.GetComponent<CanvasGroup>().DOFade(1f, .5f);
        button.GetComponent<RectTransform>().DOScale(1f, .5f).SetEase(Ease.OutBack);
    }

    public void ShowImage(Image image)
    {
        image.GetComponent<RectTransform>().DOScale(1f, .3f);
    }

    public void ShowText(TMP_Text _text)
    {
        _text.GetComponent<CanvasGroup>().DOFade(1f, .3f);
    }
    public void OpenPanel(RectTransform panelScale)
    {
        fadeScreenCG.DOFade(.5f, 1f);
        fadeScreenCG.blocksRaycasts = true;
        panelScale.DOScale(1f, 1f);
    }
}
