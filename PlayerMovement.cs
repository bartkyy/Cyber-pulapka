using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public bool isMoving;
    public Vector2 input;
    private Coroutine moveCoroutine;
    private Animator animator;
    [SerializeField] public LayerMask objects; // Warstwa obiektów kolizyjnych

    private void Awake()
    {
        animator = GetComponent<Animator>();

        // Pobieramy zapisane pozycje lub domyślne startowe pozycje dla danej sceny
        PlayerPositionSaver playerSaver = Object.FindFirstObjectByType<PlayerPositionSaver>();
        if (playerSaver != null)
        {
            transform.position = playerSaver.LoadPosition(SceneManager.GetActiveScene().name);
        }
    }

    private void Update()
    {
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if (input != Vector2.zero)
            {
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);

                input = input.normalized * 0.25f; // Zmniejszenie inputu
                Vector3 targetPos = transform.position + new Vector3(input.x, input.y, 0);

                if (IsWalkable(targetPos)) // Sprawdzamy, czy można tam wejść
                {
                    if (moveCoroutine != null)
                    {
                        StopCoroutine(moveCoroutine);
                    }
                    moveCoroutine = StartCoroutine(Move(targetPos));
                }
            }
        }

        if (input == Vector2.zero && isMoving)
        {
            StopMovement();
        }

        animator.SetBool("isMoving", isMoving);
    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
            {
                StopMovement();
                yield break;
            }

            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
        isMoving = false;
    }

    private void StopMovement()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }
        isMoving = false;
    }

    private bool IsWalkable(Vector3 targetPos)
    {
        return Physics2D.OverlapCircle(targetPos, 0.2f, objects) == null;
    }

    // Zapisujemy pozycję gracza przed zmianą sceny
    public void SavePosition()
    {
        PlayerPrefs.SetFloat("PlayerX", transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", transform.position.y);
        PlayerPrefs.Save();
    }
}
