using UnityEngine;
using UnityEngine.Playables;

public class IntroTrigger : MonoBehaviour
{
    [SerializeField] PlayableDirector playableDirector;
    bool unlocked;
    private void OnTriggerEnter(Collider other)
    {
        if(unlocked)
            return;
        playableDirector.Play();
            
        unlocked = true;
    }
}
