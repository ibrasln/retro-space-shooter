using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{

    [SerializeField] TMP_Text _text, thanksText;
    [SerializeField] GameObject credits;
    public string[] sentences;

    void Start()
    {
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        yield return new WaitForSeconds(1f);
        string sentence = sentences[0];
        StartCoroutine(TypeSentence(sentence));
        yield return new WaitForSeconds(6f);
        sentence = sentences[1];
        StartCoroutine(TypeSentence(sentence));
        yield return new WaitForSeconds(4f);
        credits.SetActive(true);
        yield return new WaitForSeconds(22f);
        thanksText.GetComponent<CanvasGroup>().DOFade(1f, 1.5f);
        yield return new WaitForSeconds(2.5f);
        thanksText.GetComponent<CanvasGroup>().DOFade(0f, 1.5f);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(0);

        //SceneManager.LoadScene(0);
    }

    IEnumerator TypeSentence(string sentence)
    {
        CanvasGroup textCG = _text.GetComponent<CanvasGroup>();
        textCG.alpha = 1;
        _text.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            _text.text += letter;
            yield return new WaitForSeconds(.05f);
        }
        yield return new WaitForSeconds(1f);
        textCG.DOFade(0f, 1f);
        yield return new WaitForSeconds(1f);
    }
    
}
