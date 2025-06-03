using UnityEngine;
using UnityEngine.Pool;

public class ReturnToPool : MonoBehaviour
{
    public ParticleSystem system;
    public AudioSource audioSource;
    public IObjectPool<ParticleSystem> pool;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var main = system.main;
        main.stopAction = ParticleSystemStopAction.Callback;
        pool = LevelManager.deathParticlePool;
    }

    void OnEnable()
    {
        audioSource.Play();
    }

    void OnParticleSystemStopped()
    {
        pool.Release(system);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
