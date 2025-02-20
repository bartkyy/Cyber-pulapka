using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayerPositionSaver : MonoBehaviour
{
    private Dictionary<string, Vector3> scenePositions = new Dictionary<string, Vector3>
    {
        { "bedroom", new Vector3(1, 1, 0) },  // Pozycja startowa w bedroom
        { "hall", new Vector3(3, 0, 0) },     // Pozycja startowa w hall
        { "school", new Vector3(-2, 2, 0) }   // Pozycja startowa w school
    };

    public Vector3 GetStartPosition()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (scenePositions.ContainsKey(sceneName))
        {
            return scenePositions[sceneName];
        }
        return Vector3.zero; // Domyślna pozycja, jeśli scena nie jest w słowniku
    }

    public void SavePosition(Vector3 position)
    {
        string sceneName = SceneManager.GetActiveScene().name;
        scenePositions[sceneName] = position;
    }
}
