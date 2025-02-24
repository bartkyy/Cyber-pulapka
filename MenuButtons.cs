using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene"); // Podmień na nazwę swojej sceny z grą
    }

    public void OpenOptions()
    {
        Debug.Log("Opcje otwarte!"); // Tu można dodać panel opcji
    }

    public void ExitGame()
    {
        Debug.Log("Zamykanie gry...");
        Application.Quit(); // Zamyka grę (działa tylko w buildzie, nie w edytorze)
    }
}
