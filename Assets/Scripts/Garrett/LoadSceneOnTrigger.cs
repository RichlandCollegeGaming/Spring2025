using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnTrigger : MonoBehaviour
{
    [Tooltip("Name of the scene to load when the player enters the trigger.")]
    public string sceneNameToLoad;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!string.IsNullOrEmpty(sceneNameToLoad))
            {
                SceneManager.LoadScene(sceneNameToLoad);
            }
            else
            {
                Debug.LogWarning("Scene name to load is not set.");
            }
        }
    }
}
