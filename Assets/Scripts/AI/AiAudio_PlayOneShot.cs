using System.Collections;
using UnityEngine;

namespace Assets.Scripts.AI
{
    [CreateAssetMenu(menuName = "Ai/Audio/PlayOneShot")]
    public class AiAudio_PlayOneShot : AiAudio
    {
        public AudioClip startClip;
        public AudioClip endClip;
        public override void OnEnter(EnemyAi controller)
        {
            if (startClip == null) return;
            var request = new AudioRequest
                (
                    startClip,
                    audioPriority,
                    controller.transform.position,
                    1f
                );       
            GameServices.AudioManager.SendRequest(request);
        }
        public override void OnExit(EnemyAi controller)
        {
            if (endClip == null) return;
            var request = new AudioRequest
                (
                    endClip,
                    audioPriority,
                    controller.transform.position,
                    1f
                );
            GameServices.AudioManager.SendRequest(request);
        }
        public override void UpdateAudio(EnemyAi controller)
        {
            
        }
    }
}