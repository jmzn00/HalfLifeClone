using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-99)]
public class Pool : MonoBehaviour
{
    [SerializeField] private int poolSize = 10;
    [SerializeField] private BulletTrail bulletTrail;
    private Queue<BulletTrail> trailPool = new Queue<BulletTrail>();
    private void Awake()
    {
        if(GameServices.Pool != this)
            GameServices.Pool = this;
    }
    private void OnDisable()
    {
        if (GameServices.Pool == this)
            GameServices.Pool = null;
    }

    private void Start()
    {
        PopulatePools();
    }
    private void PopulatePools() 
    {
        for (int i = 0; i < poolSize; i++)
        {
            if (!bulletTrail) return;

            BulletTrail bt = Instantiate(bulletTrail, transform);
            bt.gameObject.SetActive(false);
            trailPool.Enqueue(bt);
        }
    }
    #region BulletTrail   
    public void SpawnTrail(Vector3 start, Vector3 end)
    {
        BulletTrail trail = trailPool.Count > 0 ? trailPool.Dequeue() : CreateNewTrail();
        trail.Init(start, end, 5f);
    }
    public void DespawnTrail(BulletTrail trail)
    {
        trail.gameObject.SetActive(false);
        trailPool.Enqueue(trail);
    }
    private BulletTrail CreateNewTrail()
    {
        BulletTrail trail = Instantiate(bulletTrail, transform);
        trail.gameObject.SetActive(false);
        return trail;
    }
    #endregion

}
