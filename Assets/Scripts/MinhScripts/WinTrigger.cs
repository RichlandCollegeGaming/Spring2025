using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class WinTrigger : MonoBehaviour
{
    public GameObject winScreenUI;
    public float fadeDuration = 1f;
    public PlayerInput playerInput; // Reference to PlayerInput component

    private CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = winScreenUI.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(ShowWinScreen());
        }
    }

    System.Collections.IEnumerator ShowWinScreen()
    {
        float t = 0f;

        // Disable player input
        playerInput.enabled = false;

        // Unlock and show mouse cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Freeze the game
        Time.timeScale = 0f;

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        // Fade in UI using unscaled time (since time is frozen)
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    public void LoadLevel2()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level 2");
    }

    public void LoadLevel3()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level3");
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    public void QuitGame()
    {
        Time.timeScale = 1f;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
