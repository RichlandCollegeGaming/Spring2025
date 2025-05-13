using System.Collections;
using UnityEngine;
using Cinemachine;

public class EnemyChecker : MonoBehaviour
{
    [Header("Set References")]
    public GameObject targetObject;          // The object to turn off/on
    public GameObject virtualCameraObject;   // The Cinemachine Virtual Camera GameObject
    public Animator animator;                // Animator to play the trigger animation
    public string triggerName;               // The trigger parameter to activate the animation

    [Header("Timing Settings")]
    public float delayBeforeAnimation = 1f;
    public float delayAfterAnimation = 1f;

    private bool hasTriggered = false;

    void Update()
    {
        if (!hasTriggered && GameObject.FindGameObjectsWithTag("Enemy").Length <= 0)
        {
            hasTriggered = true;
            StartCoroutine(PlaySequence());
        }
    }

    private IEnumerator PlaySequence()
    {
        // Turn on virtual camera and turn off the target object
        virtualCameraObject.SetActive(true);
        targetObject.SetActive(false);

        // Wait before starting animation
        yield return new WaitForSeconds(delayBeforeAnimation);

        // Set the trigger to start the animation
        animator.SetTrigger(triggerName);

        // Wait until the Animator enters any state after trigger
        yield return new WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f
        );

        // Wait for the animation to finish
        yield return new WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f &&
            !animator.IsInTransition(0)
        );

        // Turn off virtual camera
        virtualCameraObject.SetActive(false);

        // Wait again
        yield return new WaitForSeconds(delayAfterAnimation);

        // Re-enable the target object
        targetObject.SetActive(true);
    }
}