using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerNameManager : MonoBehaviour
{
    public TMP_InputField playerNameInputField;
    public Button submitButton;
    public TMP_Text playerNameDisplayText;
    public TMP_Text confirmationText;
    public GameObject confirmationPanel;
    public Button confirmButton;
    public Button editButton;
    public TMP_Text errorText;

    private string playerName;
    private HashSet<string> badWords = new HashSet<string>();

    void Start()
    {
        submitButton.onClick.AddListener(OnSubmit);
        confirmButton.onClick.AddListener(ConfirmName);
        editButton.onClick.AddListener(EditName);
        LoadBadWords();
        confirmationPanel.SetActive(false);
        errorText.text = "";
    }

    void OnSubmit()
    {
        playerName = playerNameInputField.text;

        if (ContainsBadWords(playerName))
        {
            errorText.text = "Twój nick zawiera zabronione słowa. Proszę, wybierz inny.";
            return;
        }

        confirmationText.text = "Czy Twój nick to: " + playerName + "?";
        confirmationPanel.SetActive(true);
    }

    void ConfirmName()
    {
        playerNameDisplayText.text = "Witaj, " + playerName + "!";
        PlayerPrefs.SetString("PlayerName", playerName); // Zapisanie nazwy gracza
        PlayerPrefs.Save(); // Zapisanie zmian
        confirmationPanel.SetActive(false);
        StartCoroutine(DelayedSceneTransition(1.5f)); // Opóźnienie 1.5 sekundy
    }

    void EditName()
    {
        confirmationPanel.SetActive(false);
        playerNameInputField.text = "";
        playerNameInputField.Select();
    }

    void LoadBadWords()
    {
        TextAsset badWordsFile = Resources.Load<TextAsset>("badwords");
        if (badWordsFile != null)
        {
            string[] lines = badWordsFile.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                badWords.Add(line.Trim().ToLower());
            }
        }
        else
        {
            Debug.LogError("Nie znaleziono pliku badwords.txt w folderze Resources!");
        }
    }

    bool ContainsBadWords(string name)
    {
        foreach (string badWord in badWords)
        {
            if (name.ToLower().Contains(badWord))
            {
                return true;
            }
        }
        return false;
    }

    IEnumerator DelayedSceneTransition(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
