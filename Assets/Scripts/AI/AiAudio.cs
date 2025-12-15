using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.AI
{
    public abstract class AiAudio : ScriptableObject
    {
        public int audioPriority = 0;
        public abstract void OnEnter(EnemyAi controller);
        public abstract void OnExit(EnemyAi controller);
        public abstract void UpdateAudio(EnemyAi controller);
    }
}