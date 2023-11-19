using System;
using UnityEngine;

public class DrawChemicalBond : MonoBehaviour
{
    [SerializeField] private Transform[] _bonds;

    private Vector2[] _edgePoints;
    
    private LineRenderer lineRenderer;
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
            _edgePoints[i] = _bonds[i].position;
        }
    
        edgeCollider.points = _edgePoints;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log(col.gameObject.name);
    }
}
