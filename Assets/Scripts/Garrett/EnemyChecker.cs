using System.Collections;
using UnityEngine;

public class EnemyChecker : MonoBehaviour
{
    public GameObject[] objectsToToggle;         // Objects to turn on/off
    public float displayDuration = 5f;           // How long everything stays on
    public float respawnExtraTime = 5f;          // Extra time to keep respawnObject off
    public GameObject animationObject;           // Object with animation trigger
    public string animationTrigger = "Play";     // Animator trigger name
    public GameObject respawnObject;             // Respawn object to control separately

    private bool eventTriggered = false;

    void Update()
    {
        int enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (enemyCount <= 0 && !eventTriggered)
        {
            eventTriggered = true;
            StartCoroutine(HandleVictorySequence());
        }
    }

    IEnumerator HandleVictorySequence()
    {
        // 1. Turn on all objects, including respawn
        foreach (GameObject obj in objectsToToggle)
        {
            obj.SetActive(true);
        }

        if (respawnObject != null)
        {
            respawnObject.SetActive(true);
        }

        // 2. Play animation
        if (animationObject != null)
        {
            Animator anim = animationObject.GetComponent<Animator>();
            if (anim != null)
            {
                anim.SetTrigger(animationTrigger);
            }
        }

        // 3. Wait for display duration
        yield return new WaitForSeconds(displayDuration);

        // 4. Turn off everything at the same time
        foreach (GameObject obj in objectsToToggle)
        {
            obj.SetActive(false);
        }

        if (respawnObject != null)
        {
            respawnObject.SetActive(false);

            // 5. Wait extra time, then turn it back on
            yield return new WaitForSeconds(respawnExtraTime);
            respawnObject.SetActive(true);
        }
    }
}
