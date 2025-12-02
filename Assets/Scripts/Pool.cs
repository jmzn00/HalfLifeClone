using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class PoolConfig 
{
    public string id;
    public MonoBehaviour prefab;
    public int size;

    [System.NonSerialized]
    public Queue<MonoBehaviour> pool;
}

[DefaultExecutionOrder(-99)]
public class Pool : MonoBehaviour 
{    
    [SerializeField] private int poolSize = 10;
    [SerializeField] private BulletTrail bulletTrail;
    private Queue<BulletTrail> trailPool = new Queue<BulletTrail>();

    //[SerializeField] private DamageText damageText;
    //private Queue<DamageText> damageTextPool = new Queue<DamageText>();

    [SerializeField] private List<PoolConfig> poolConfigs = new List<PoolConfig>();


    // remove id in the future and compare by type to avoid string mismatch
    private Dictionary<string, PoolConfig> poolLookup;

    private const string BulletTrailId = "BulletTrail";
    private const string DamageTextId = "DamageText";
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
        poolLookup = new Dictionary<string, PoolConfig>();

        foreach (var cfg in poolConfigs) 
        {
            if(!cfg.prefab)
                continue;

            cfg.pool = new Queue<MonoBehaviour> (cfg.size);

            for (int i = 0; i < cfg.size; i++) 
            {
                var instance = Instantiate(cfg.prefab, transform);
                instance.gameObject.SetActive(false);
                cfg.pool.Enqueue(instance);
            }
            poolLookup[cfg.id] = cfg;
        }
        /*
        for (int i = 0; i < poolSize; i++)
        {
            if (!bulletTrail) return;

            BulletTrail bt = Instantiate(bulletTrail, transform);
            bt.gameObject.SetActive(false);
            trailPool.Enqueue(bt);
        }
        for (int i = 0; i < poolSize; i++) 
        {
            if (!damageText) return;

            DamageText dt = Instantiate(damageText, transform);
            dt.gameObject.SetActive(false);
            damageTextPool.Enqueue(dt);
        }
        */
    }
    // T is a generic type parameter that can return any type specified
    // here the type is MonoBehaviour
    public T Spawn<T>(string id) where T : MonoBehaviour 
    {
        if(!poolLookup.TryGetValue(id, out var cfg)) 
        {
            Debug.LogError($"Pool with id {id} not found");
            return null;
        }

        MonoBehaviour instance;

        if(cfg.pool.Count > 0) 
        {
            instance = cfg.pool.Dequeue();
        }
        else 
        {
            instance = Instantiate(cfg.prefab, transform);
        }

        instance.gameObject.SetActive(true);
        // cast to specified type
        return (T)instance;
    }
    public void Despawn(string id, MonoBehaviour instance) 
    {
        if(!poolLookup.TryGetValue(id, out var cfg)) 
        {
            Debug.LogError($"Pool with id {id} not found");
            return;
        }

        instance.gameObject.SetActive(false);
        cfg.pool.Enqueue(instance);
    }
    
    #region BulletTrail   
    public void SpawnTrail(Vector3 start, Vector3 end, float speed)
    {
        var trail = Spawn<BulletTrail>(BulletTrailId);
        if (trail == null) return;

        trail.Init(start, end, speed);
        //BulletTrail trail = trailPool.Count > 0 ? trailPool.Dequeue() : CreateNewTrail();
        //trail.Init(start, end, speed);
    }
    public void DespawnTrail(BulletTrail trail)
    {
        Despawn(BulletTrailId, trail);
        //trail.gameObject.SetActive(false);
        //trailPool.Enqueue(trail);
    }
    /*
    private BulletTrail CreateNewTrail()
    {
        BulletTrail trail = Instantiate(bulletTrail, transform);
        trail.gameObject.SetActive(false);
        return trail;
    }
    */
    #endregion
    #region DamageText
    public DamageText SpawnDamageText(Vector3 start, float dmg) 
    {
        var dt = Spawn<DamageText>(DamageTextId);
        if (dt == null) return null;

        dt.UpdateDmg(start, dmg);        
        //DamageText dt = damageTextPool.Count > 0 ? damageTextPool.Dequeue() : CreateNewDamageText();
        //dt.gameObject.SetActive(true);
        //dt.UpdateDmg(start, dmg);
        return dt;
    }
    /*
    private DamageText CreateNewDamageText() 
    {
        DamageText dt = Instantiate(damageText, transform);
        dt.gameObject.SetActive(false);
        return dt;
    }
    */
    #endregion

}
