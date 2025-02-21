using UnityEngine;
using System.Collections.Generic;

public class PlayerPositionSaver : MonoBehaviour
{
    private Dictionary<string, Vector3> startPositions = new Dictionary<string, Vector3>
    {
        { "Bedroom", new Vector3(-2.5f, -2.9f, 0f) },
        { "hall", new Vector3(6f, -3f, 0f) },
        { "school", new Vector3(-0.5f, -3.4f, 0f) }
    };

    // Zapisz pozycję gracza dla danej sceny
    public void SavePosition(string sceneName, Vector3 position)
    {
        PlayerPrefs.SetFloat(sceneName + "_PlayerX", position.x);
        PlayerPrefs.SetFloat(sceneName + "_PlayerY", position.y);
        PlayerPrefs.SetFloat(sceneName + "_PlayerZ", position.z);
        PlayerPrefs.Save();
        Debug.Log($"Position saved for {sceneName}: {position}");
    }

    // Załaduj pozycję gracza dla danej sceny
    public Vector3 LoadPosition(string sceneName)
    {
        string keyX = sceneName + "_PlayerX";
        string keyY = sceneName + "_PlayerY";
        string keyZ = sceneName + "_PlayerZ";

        if (PlayerPrefs.HasKey(keyX) && PlayerPrefs.HasKey(keyY) && PlayerPrefs.HasKey(keyZ))
        {
            Vector3 loadedPosition = new Vector3(
                PlayerPrefs.GetFloat(keyX),
                PlayerPrefs.GetFloat(keyY),
                PlayerPrefs.GetFloat(keyZ)
            );
            Debug.Log($"Loaded saved position for {sceneName}: {loadedPosition}");
            return loadedPosition;
        }
        else if (startPositions.ContainsKey(sceneName))
        {
            Vector3 defaultPosition = startPositions[sceneName];
            Debug.Log($"No saved position for {sceneName}. Using default: {defaultPosition}");
            return defaultPosition;
        }

        return Vector3.zero;
    }

    // Ustaw pozycję gracza po starcie sceny
    private void Start()
    {
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        transform.position = LoadPosition(sceneName);
    }

    // Przykład zapisywania pozycji przy wychodzeniu z gry
    private void OnApplicationQuit()
    {
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        SavePosition(sceneName, transform.position);
    }
}
