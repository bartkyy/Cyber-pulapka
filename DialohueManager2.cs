using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogueManager2 : MonoBehaviour
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
        playerName = PlayerPrefs.GetString("PlayerName", "Nieznajomy");
        LoadDialogue("dialog2");
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
            characterNameText.text = line.character.Replace("{playerName}", playerName);
            dialogueText.text = line.text.Replace("{playerName}", playerName);

            if (!string.IsNullOrEmpty(line.sprite))
            {
                Sprite newSprite = Resources.Load<Sprite>("Sprites/" + line.sprite);
                if (newSprite != null) characterImage.sprite = newSprite;
            }
        }
        else
        {
            EndDialogue();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
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
        SceneManager.LoadScene(nextSceneName);
    }
}
