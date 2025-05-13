using UnityEngine;

namespace LethalMuseum.Helpers;

internal static class Audio
{
    public static void PlayUI(AudioClip? clip)
    {
        if (clip == null)
            return;
        
        HUDManager.Instance.UIAudio?.PlayOneShot(clip);
    }
}