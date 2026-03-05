using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BeatAnimationTrigger : MonoBehaviour
{
    [SerializeField] private BeatNoteSpawner beatSpawner;
    private Animator animator;
    private string triggerName = "PlayAnim";

    private int currentBeatIndex = 0;
    private List<double> beats;

    IEnumerator Start()
    {
        animator = GetComponent<Animator>();

        yield return null;

        beats = beatSpawner.beatData.beats;
        Debug.Log("Beats cargados: " + beats.Count);
    }

    void Update()
    {
        if (beats == null) return;

        if (!beatSpawner.MusicInstance.isValid())
            return;

        beatSpawner.MusicInstance.getTimelinePosition(out int ms);
        double songTime = ms / 1000.0;

        if (currentBeatIndex < beats.Count && songTime >= beats[currentBeatIndex])
        {
            Debug.Log("trigger anim beat: " + currentBeatIndex);

            animator.SetTrigger(triggerName);
            currentBeatIndex++;
        }
    }
}