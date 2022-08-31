using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class UIController : MonoBehaviour
{

    public static UIController instance;

    [SerializeField] TMP_Text bombText, missileText;
    [SerializeField] Image shield, supMachine;
    [SerializeField] CanvasGroup fadeScreen;
    [SerializeField] Button pauseButton, volumeButton, resumeButton;
    [SerializeField] RectTransform pausePanel, gameOverPanel, gameWonPanel;
    [SerializeField] Sprite volumeOn, volumeOff;
    public AudioSource gameMusic, bossMusic;
    public GameObject bossHealthBar;
    Image volumeButtonImage;
    bool isPaused;

    private void Start()
    {
        volumeButtonImage = volumeButton.GetComponent<Image>();
        gameMusic.PlayDelayed(3f);
        instance = this;
        bombText.text = 0.ToString();
        missileText.text = 0.ToString();
        fadeScreen.DOFade(0f, 1.5f);
        InvokeRepeating(nameof(CheckSomeStuff), 2, 2);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused) Pause();
            else Resume();
        }
        bombText.text = PlayerController.instance.bombAmmo.ToString();
        missileText.text = PlayerController.instance.missileAmmo.ToString();
        if (PlayerController.instance.isShieldActive) shield.gameObject.SetActive(true);
        else shield.gameObject.SetActive(false);
        if (PlayerController.instance.isSupActive) supMachine.gameObject.SetActive(true);
        else supMachine.gameObject.SetActive(false);
    }

    public void CheckSomeStuff()
    {
        if (PlayerHealthController.instance.isDead)
        {
            StartCoroutine(GameOver());
        }
        else if (GameManager.instance.isGameWon) StartCoroutine(GameWon());

    }

    IEnumerator GameWon()
    {
        yield return new WaitForSeconds(5f);
        fadeScreen.DOFade(1f, 2f);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(2);
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(1.75f);
        fadeScreen.blocksRaycasts = true;
        gameOverPanel.DOScale(1f, 1f);
        fadeScreen.DOFade(.5f, 1f);
    }

    #region Buttons

    public void Pause()
    {
        isPaused = true;
        StartCoroutine(PauseRoutine());
        Time.timeScale = 0f;
    }

    IEnumerator PauseRoutine()
    {
        SoundEffectController.instance.SoundEffect(0);
        float scale = 0;
        while(scale != 1)
        {
            scale += Time.unscaledDeltaTime;
            scale = Mathf.Clamp01(scale);
            fadeScreen.blocksRaycasts = true;
            pausePanel.localScale = new Vector3(scale, scale, scale);
            fadeScreen.alpha = scale / 2;
            yield return null;
        }
    }

    public void Resume()
    {
        Time.timeScale = 1;
        isPaused = false;
        SoundEffectController.instance.SoundEffect(0);
        fadeScreen.blocksRaycasts = false;
        pausePanel.DOScale(0f, 1f);
        fadeScreen.DOFade(0f, 1f);
    }

    public void VolumeOnOff()
    {
        SoundEffectController.instance.SoundEffect(0);
        if (volumeButtonImage.sprite == volumeOn)
        {
            volumeButtonImage.sprite = volumeOff;
            gameMusic.volume = 0f;
        }
        else if (volumeButtonImage.sprite == volumeOff)
        {
            volumeButtonImage.sprite = volumeOn;
            gameMusic.volume = .5f;
        }
    }

    public void Restart()
    {
        SoundEffectController.instance.SoundEffect(0);
        fadeScreen.blocksRaycasts = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void Menu()
    {
        SoundEffectController.instance.SoundEffect(0);
        StartCoroutine(MenuRoutine());
    }

    IEnumerator MenuRoutine()
    {
        fadeScreen.DOFade(1f, 1.5f);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(0);
    }

    #endregion
}
