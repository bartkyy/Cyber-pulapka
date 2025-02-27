using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class DialogueChoice
{
    public string text;
    public int next;
}

[System.Serializable]
public class DialogueLine3
{
    public int id;
    public string character;
    public string sprite;
    public string text;
    public List<DialogueChoice> choices;
}

[System.Serializable]
public class DialogueData3
{
    public string playerName;
    public List<DialogueLine3> dialogue;
}

public class DialogueManager3 : MonoBehaviour
{
    public TMP_Text characterNameText;
    public TMP_Text dialogueText;
    public Image characterImage;
    public GameObject choicePanel;
    public TMP_Text choiceText1;
    public TMP_Text choiceText2;

    private DialogueData3 currentDialogue;
    private int currentLineIndex = 0;
    private string playerName;

    private List<DialogueChoice> currentChoices;
    private bool isChoosing = false;

    void Start()
    {
        playerName = PlayerPrefs.GetString("PlayerName", "Nieznajomy");
        LoadDialogue("dialog3");
        DisplayCurrentLine();
    }

    void LoadDialogue(string fileName)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(fileName);
        if (jsonFile != null)
        {
            currentDialogue = JsonUtility.FromJson<DialogueData3>(jsonFile.text);
        }
        else
        {
            Debug.LogError("Nie znaleziono pliku dialogu: " + fileName);
        }
    }

    void DisplayCurrentLine()
    {
        // Disable choice panel when no choices are present
        choicePanel.SetActive(false);  // Hide choices initially
        isChoosing = false;  // Set isChoosing flag to false

        if (currentDialogue != null && currentLineIndex < currentDialogue.dialogue.Count)
        {
            DialogueLine3 line = currentDialogue.dialogue[currentLineIndex];
            characterNameText.text = line.character.Replace("{playerName}", playerName);
            dialogueText.text = line.text.Replace("{playerName}", playerName);

            if (!string.IsNullOrEmpty(line.sprite))
            {
                Sprite newSprite = Resources.Load<Sprite>("Sprites/" + line.sprite);
                if (newSprite != null)
                {
                    characterImage.sprite = newSprite;
                }
                else
                {
                    Debug.LogWarning("Sprite not found: " + line.sprite);
                    characterImage.sprite = null;
                }
            }

            if (line.choices.Count > 0)
            {
                currentChoices = line.choices;
                ShowChoices();  // Show the choices when available
                return;
            }
        }
        else
        {
            EndDialogue();  // End the dialogue if there are no more lines
        }
    }

    void ShowChoices()
    {
        // Show the choice panel when choices are available
        choicePanel.SetActive(true);
        isChoosing = true;

        // Display the available choices
        if (currentChoices.Count > 0)
        {
            choiceText1.text = "A: " + currentChoices[0].text;
        }
        if (currentChoices.Count > 1)
        {
            choiceText2.text = "B: " + currentChoices[1].text;
        }
    }

    void Update()
    {
        if (isChoosing)
        {
            // Handle key inputs for choices
            if (Input.GetKeyDown(KeyCode.A) && currentChoices.Count > 0)
            {
                SelectChoice(currentChoices[0].next);  // Select the first choice
            }
            else if (Input.GetKeyDown(KeyCode.B) && currentChoices.Count > 1)
            {
                SelectChoice(currentChoices[1].next);  // Select the second choice
            }
        }
        else
        {
            // Handle spacebar to proceed when no choices are present
            if (Input.GetKeyDown(KeyCode.Space))
            {
                currentLineIndex++;  // Move to the next dialogue line
                DisplayCurrentLine();
            }
        }
    }

    void SelectChoice(int nextId)
    {
        // Disable the choice text boxes and choice panel after selection
        choiceText1.gameObject.SetActive(false);  // Disable choiceText1
        choiceText2.gameObject.SetActive(false);  // Disable choiceText2
        choicePanel.SetActive(false);  // Hide the choice panel
        isChoosing = false;  // Reset isChoosing flag

        // Find the next dialogue line based on the selected choice
        currentLineIndex = currentDialogue.dialogue.FindIndex(d => d.id == nextId);
        DisplayCurrentLine();  // Display the next dialogue line
    }

    void EndDialogue()
    {
        // Transition to the next scene
        SceneManager.LoadScene("school");
    }
}
