using UnityEngine;

namespace LethalMuseum.Helpers;

/// <summary>
/// Helper to play audio
/// </summary>
internal static class Audio
{
    /// <summary>
    /// Plays the given <see cref="AudioClip"/> once
    /// </summary>
    public static void PlayUI(AudioClip? clip)
    {
        if (clip == null)
            return;
        
        HUDManager.Instance.UIAudio?.PlayOneShot(clip);
    }
}