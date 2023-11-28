using UnityEngine;

public class DrawChemicalBond : MonoBehaviour, IKillPlayer
{
    [SerializeField] private Transform[] _bonds;

    private Vector2[] _edgePoints;
    
    [HideInInspector] public LineRenderer lineRenderer;
    [HideInInspector] public EdgeCollider2D edgeCollider;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        edgeCollider = gameObject.AddComponent<EdgeCollider2D>();

        _edgePoints = new Vector2[_bonds.Length];
        
        lineRenderer.positionCount = _bonds.Length;
        
        
    }
    
    void Update()
    {
        for (int i = 0; i < _bonds.Length; i++)
        {
            lineRenderer.SetPosition(i, _bonds[i].position);
            _edgePoints[i] = _bonds[i].localPosition;
        }
    
        edgeCollider.points = _edgePoints;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        GameObject.FindWithTag("Player").SetActive(false);
    }
    
    public void KillPlayer()
    {
        GameObject.FindWithTag("Player").SetActive(false);
    }
}
