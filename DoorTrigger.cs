using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    private bool playerIsNear = false;
    private PlayerPositionSaver playerSaver;

    private void Start()
    {
        playerSaver = FindFirstObjectByType<PlayerPositionSaver>();

        if (playerSaver == null)
        {
            Debug.LogError("Brak PlayerPositionSaver w scenie! Upewnij się, że obiekt istnieje.");
        }
    }

    private void Update()
    {
        if (playerIsNear && Input.GetKeyDown(KeyCode.E))
        {
            if (!string.IsNullOrEmpty(sceneToLoad))
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");

                if (player != null)
                {
                    if (playerSaver != null)
                    {
                        playerSaver.SavePosition(SceneManager.GetActiveScene().name, player.transform.position);
                    }
                    else
                    {
                        Debug.LogWarning("PlayerPositionSaver nie został znaleziony – pozycja gracza nie zostanie zapisana.");
                    }

                    SceneManager.LoadScene(sceneToLoad);
                }
                else
                {
                    Debug.LogError("Nie znaleziono obiektu Player! Upewnij się, że gracz ma poprawny tag.");
                }
            }
            else
            {
                Debug.LogError("sceneToLoad nie jest ustawione! Przypisz nazwę sceny w Inspectorze.");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = false;
        }
    }
}
