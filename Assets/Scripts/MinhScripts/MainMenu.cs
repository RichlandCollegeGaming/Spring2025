using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public CanvasGroup LevelSelectionPanel;

    public void ToggleLevelPanel()
    {
        bool isVisible = LevelSelectionPanel.alpha > 0f;

        if (isVisible)
        {
            LevelSelectionPanel.alpha = 0f;
            LevelSelectionPanel.interactable = false;
            LevelSelectionPanel.blocksRaycasts = false;
        }
        else
        {
            LevelSelectionPanel.alpha = 1f;
            LevelSelectionPanel.interactable = true;
            LevelSelectionPanel.blocksRaycasts = true;
        }
    }
    public void StartGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void QuitGame()
    {
        Debug.Log("Quit game!");
        Application.Quit();
    }
}
