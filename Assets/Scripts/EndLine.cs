using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLine : MonoBehaviour
{
    [SerializeField] private Inventory inventory;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (inventory != null && inventory.HasItem("Flag"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}