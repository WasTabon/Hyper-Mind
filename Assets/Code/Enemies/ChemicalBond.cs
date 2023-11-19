public class ChemicalBond : Enemy
{
    private DrawChemicalBond _drawChemicalBond;
    
    protected override void Start()
    {
        _drawChemicalBond = GetComponentInParent<DrawChemicalBond>();
        
        LifeTime = 50f;
        FadeTime = 1f;
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        switch (_enemyMaterial.GetFloat("_GhostTransparency"))
        {
            case 0:
                _drawChemicalBond.edgeCollider.enabled = true;
                break;
            case 1:
                _drawChemicalBond.edgeCollider.enabled = false;
                break;
        }
    }
}
