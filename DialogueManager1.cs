using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;  // Dodajemy tę przestrzeń nazw do korzystania z UI
using System.Collections.Generic;

[System.Serializable]
public class DialogueLine
{
    public int id;
    public string character;
    public string sprite;
    public string text;
}

[System.Serializable]
public class DialogueData
{
    public string playerName;
    public List<DialogueLine> dialogue;
}

public class DialogueManager : MonoBehaviour
{
    public TMP_Text characterNameText;
    public TMP_Text dialogueText;
    public Image characterImage;
    private DialogueData currentDialogue;
    private int currentLineIndex = 0;
    private string playerName;
    public string nextSceneName = "school";

    void Start()
    {
        playerName = PlayerPrefs.GetString("PlayerName", "Nieznajomy"); // Pobieranie nazwy gracza
        LoadDialogue("dialog1");
        DisplayCurrentLine();
    }

    void LoadDialogue(string fileName)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(fileName);
        if (jsonFile != null)
        {
            currentDialogue = JsonUtility.FromJson<DialogueData>(jsonFile.text);
        }
        else
        {
            Debug.LogError("Nie znaleziono pliku dialogu: " + fileName);
        }
    }

    void DisplayCurrentLine()
    {
        if (currentLineIndex < currentDialogue.dialogue.Count)
        {
            DialogueLine line = currentDialogue.dialogue[currentLineIndex];
            string displayedCharacter = line.character.Replace("{playerName}", playerName);
            string displayedText = line.text.Replace("{playerName}", playerName);

            characterNameText.text = displayedCharacter;
            dialogueText.text = displayedText;

            if (!string.IsNullOrEmpty(line.sprite))
            {
                Sprite newSprite = Resources.Load<Sprite>("Sprites/" + line.sprite);
                if (newSprite != null)
                {
                    characterImage.sprite = newSprite;
                }
                else
                {
                    Debug.LogWarning("Nie znaleziono sprite'a: " + line.sprite);
                }
            }
        }
        else
        {
            EndDialogue();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Przejdź do następnej linii po wciśnięciu spacji
        {
            NextDialogue();
        }
    }

    void NextDialogue()
    {
        currentLineIndex++;
        DisplayCurrentLine();
    }

    void EndDialogue()
    {
        Debug.Log("Koniec dialogu. Przenoszenie do innej sceny...");
        SceneManager.LoadScene("school");
    }
}
