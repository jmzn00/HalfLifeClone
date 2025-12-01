using System.Collections;
using UnityEditor;
using UnityEngine;

public class BulletTrail : MonoBehaviour
{
    private Vector3 start;
    private Vector3 end;
    private float speed;
    private float progress;
    private TrailRenderer trail;

    private void Awake()
    {
        trail = GetComponent<TrailRenderer>();
    }
    public void Init(Vector3 startPos, Vector3 endPos, float trailSpeed) 
    {
        start = startPos;
        end = endPos;
        speed = trailSpeed;
        progress = 0f;

        transform.position = start;
        if(trail != null) 
        {
            trail.Clear();
        }
        gameObject.SetActive(true);
    }
    private void Update()
    {
        progress += speed * Time.deltaTime;
        float t = Mathf.Clamp01(progress);

        transform.position = Vector3.Lerp(start, end, t);
        if(t >= 1f) 
        {
            GameServices.Pool.DespawnTrail(this);
        }
    }
}
