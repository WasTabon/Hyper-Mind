using UnityEngine;

public class ChemicalBond : Enemy
{
    public float _FadeTime = 1f;
    public float _lifeTime = 50f;
    
    protected override void Start()
    {
        LifeTime = _lifeTime;
        FadeTime = _FadeTime;
        
        base.Start();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        KillPlayer();
    }
}
