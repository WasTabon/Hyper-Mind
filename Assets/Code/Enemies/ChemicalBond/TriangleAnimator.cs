using DG.Tweening;
using UnityEngine;

public class TriangleAnimator : MonoBehaviour
{
     [SerializeField] private float _scaleTime = 1f;
    [SerializeField] private float _triangleTime = 1f;
    [SerializeField] private Ease _scaleEase;
    [SerializeField] private Transform[] _bonds;

    private DrawChemicalBond _drawChemicalBond;
    private Sequence _sequence;

    public float _animationDuration = 2f;
    public Ease _easeType = Ease.OutQuad;

    private void Start()
    {
        _sequence = DOTween.Sequence();
        _drawChemicalBond = GetComponent<DrawChemicalBond>();

        SetAllToZeroSizeAndPosition();

        _sequence.AppendCallback(SetAppear);
        _sequence.AppendInterval(_scaleTime);
        _sequence.AppendCallback(SetTriangle);
        _sequence.AppendCallback(AnimateTriangle);
        _sequence.Play();
    }

    private void SetAppear()
    {
        Vector3 _vectorOne = new Vector3(1, 1, 1);

        foreach (Transform bond in _bonds)
        {
            _sequence.Join(bond.DOScale(_vectorOne, _scaleTime).SetEase(_scaleEase));
        }
    }

    private void SetTriangle()
    {
        Debug.Log("123");

        float triangleHeight = 2f;
        float triangleBase = 2f;

        Vector3 topBondPosition = Vector3.up * triangleHeight / 2f;

        Vector3 bottomLeftBondPosition = new Vector3(-triangleBase / 2f, -triangleHeight / 2f, 0f);
        Vector3 bottomRightBondPosition = new Vector3(triangleBase / 2f, -triangleHeight / 2f, 0f);

        for (int i = 0; i < _bonds.Length; i++)
        {
            switch (i)
            {
                case 0:
                    _sequence.Join(_bonds[i].DOMove(topBondPosition, _triangleTime).SetEase(_scaleEase));
                    break;
                case 1:
                    _sequence.Join(_bonds[i].DOMove(bottomLeftBondPosition, _triangleTime).SetEase(_scaleEase));
                    break;
                case 2:
                    _sequence.Join(_bonds[i].DOMove(bottomRightBondPosition, _triangleTime).SetEase(_scaleEase));
                    break;
            }
        }
    }

    private void AnimateTriangle()
    {
        Vector3[] trianglePositions = {
            Vector3.zero,  // Начальная позиция треугольника (в центре экрана)
            Vector3.right + Vector3.up,  // Позиция после превращения в треугольник
            Vector3.left + Vector3.down,  // Позиция после перемещения
            Vector3.down * 5f  // Позиция за экраном
        };

        transform.position = trianglePositions[0];
        _sequence.Append(transform.DOScale(Vector3.one, _animationDuration).SetEase(_easeType));

        _sequence.Join(transform.DOMove(trianglePositions[1], _animationDuration).SetEase(_easeType));
        _sequence.Join(_bonds[1].DOMove(trianglePositions[1], _animationDuration).SetEase(_easeType));

        _sequence.Append(_bonds[1].DOMove(trianglePositions[2], _animationDuration).SetEase(_easeType));
        _sequence.Join(transform.DOMove(trianglePositions[3], _animationDuration).SetEase(_easeType));

        _sequence.AppendCallback(() =>
        {
            gameObject.SetActive(false);
        });
    }

    private void SetAllToZeroSizeAndPosition()
    {
        transform.position = Vector3.zero;
        foreach (Transform bond in _bonds)
        {
            bond.localScale = Vector3.zero;
            bond.localPosition = Vector3.zero;
        }
    }
}
