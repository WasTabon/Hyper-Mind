using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IKillPlayer
{
    protected float LifeTime;
    protected float FadeTime;

    private float _transparency = 1f;
    protected float _startTime;

    private Collider2D _collider2D;
    protected Material _enemyMaterial;

    protected virtual void Start()
    {
        _enemyMaterial = GetComponent<SpriteRenderer>().material;
        _collider2D = GetComponent<Collider2D>();
        
        _startTime = Time.time;
        StartCoroutine(Fade());
    }

    protected virtual void Update()
    {
        ControllCollider();
    }

    protected IEnumerator Fade()
    {
        while (_transparency > 0f)
        {
            ChangeFade(1, 0);
            yield return new WaitForSeconds(0f);
        }
        
        yield return new WaitForSeconds(LifeTime - FadeTime);

        _transparency = 0f;
        _startTime = Time.time;
        
        while (_transparency < 1f)
        {
            ChangeFade(0, 1);
            yield return null;
        }
    } 
    
    private void ChangeFade(float from, float to)
    {
        float t = (Time.time - _startTime) / FadeTime;
        _transparency = Mathf.Lerp(from, to, t);
        _enemyMaterial.SetFloat("_GhostTransparency", _transparency);
        _enemyMaterial.SetFloat("_GhostBlend", _transparency);
    }

    private void ControllCollider()
    {
        switch (_enemyMaterial.GetFloat("_GhostTransparency"))
        {
            case 0:
                _collider2D.enabled = true;
                break;
            case 1:
                _collider2D.enabled = false;
                break;
        }
    }

    public void KillPlayer()
    {
        GameObject.FindWithTag("Player").SetActive(false);
    }
}