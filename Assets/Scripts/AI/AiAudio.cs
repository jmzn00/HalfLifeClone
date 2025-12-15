using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.AI
{
    [CreateAssetMenu(menuName = "Ai/Audio/AiAudio")]
    public class AiAudio : ScriptableObject
    {
        [Header("Audio")]
        public List<AudioClip> audioClips;
        public Vector2 delayRange = new Vector2(2, 4);
        public int audioPriority = 2;

        public void UpdateAudio(EnemyAi controller) 
        {
            if (audioClips == null || audioClips.Count == 0)
                return;

            controller.ClipTimer += Time.deltaTime;

            if (controller.ClipTimer >= controller.NextDelay)
            {
                controller.ClipTimer = 0f;
                controller.NextDelay = Random.Range(delayRange.x, delayRange.y);

                var clip = audioClips[Random.Range(0, audioClips.Count)];

                var request = new AudioRequest
                    (
                    clip,
                    audioPriority,
                    controller.transform.position,
                    1f
                    );
                GameServices.AudioManager.SendRequest(request);
            }
        }
    }
}