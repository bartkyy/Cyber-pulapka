using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    // Czas opóźnienia w sekundach przed przejściem do nowej sceny
    public float delayTime = 3f;

    // Nazwa sceny, do której ma zostać przeniesiony gracz
    public string sceneToLoad;

    void Start()
    {
        // Rozpocznij korutynę, która przeniesie gracza do nowej sceny po określonym czasie
        StartCoroutine(TransitionToSceneAfterDelay());
    }

    IEnumerator TransitionToSceneAfterDelay()
    {
        // Czekaj przez określoną liczbę sekund
        yield return new WaitForSeconds(delayTime);

        // Przeładuj scenę
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogError("Brak nazwy sceny do załadowania! Przypisz nazwę sceny w Inspectorze.");
        }
    }
}
