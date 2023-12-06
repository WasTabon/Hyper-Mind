using DG.Tweening;
using UnityEngine;

public class ControllChemicalBonds : MonoBehaviour
{
    [SerializeField] private float _scaleTime = 1f;
    [SerializeField] private float _LifeTime = 50f;
    [SerializeField] private float _triangleTime;
    
    [SerializeField] private Ease _scaleEase;

    [SerializeField] private Transform[] _bonds;

    private Sequence _sequenceAppear;
    private Sequence _sequenceTriangle;
    
    private DrawChemicalBond _drawChemicalBond; 
    
    public float _animationDuration = 2f;
    public Ease _easeType = Ease.OutQuad;
        
    private void Start()
    {
        _sequenceAppear = DOTween.Sequence();
        _sequenceTriangle = DOTween.Sequence();
        
        _drawChemicalBond = GetComponent<DrawChemicalBond>();
        
        SetAllToZeroSizeAndPosition();
        
        SetAppear();
        SetTriangle();
        AnimateTriangle();
        _sequenceAppear.AppendInterval(_scaleTime);

        _sequenceAppear.OnComplete(() =>
        {
            _sequenceTriangle.Play();
        });

        _sequenceAppear.Play();
    }

    private void SetAppear()
    {
        Vector3 _vectorOne = new Vector3(1, 1, 1);
    
        foreach (Transform bond in _bonds)
        {
            _sequenceAppear.Join(bond.DOScale(_vectorOne, _scaleTime).SetEase(_scaleEase));
        }
    }

    private void SetDisappear()
    {
        foreach (Transform bond in _bonds)
        {
            _sequenceAppear.Join(bond.DOScale(Vector3.zero, _scaleTime).SetEase(_scaleEase));
        }
        _sequenceAppear.Append(LineToZero());
        _sequenceAppear.OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
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
                    _sequenceTriangle.Join(_bonds[i].DOMove(topBondPosition, _triangleTime).SetEase(_scaleEase));
                    break;
                case 1:
                    _sequenceTriangle.Join(_bonds[i].DOMove(bottomLeftBondPosition, _triangleTime).SetEase(_scaleEase));
                    break;
                case 2:
                    _sequenceTriangle.Join(_bonds[i].DOMove(bottomRightBondPosition, _triangleTime).SetEase(_scaleEase));
                    break;
            }
        }
    }
    private void AnimateTriangle()
    {
        // Начальные позиции для треугольника и связей
        Vector3[] initialPositions = {
            Vector3.zero,  // Начальная позиция треугольника (в центре экрана)
            Vector3.down * 2f,  // Начальная позиция для связи 1
            Vector3.left * 2f,  // Начальная позиция для связи 2
            Vector3.right * 2f  // Начальная позиция для связи 3
        };

        // Установка начальных позиций
        transform.position = initialPositions[0];
        for (int i = 0; i < _bonds.Length; i++)
        {
            _bonds[i].position = initialPositions[i + 1];
        }

        // Анимация превращения в треугольник
        _sequenceTriangle.Append(transform.DOScale(Vector3.one, _animationDuration).SetEase(_easeType));

        // Анимация перемещения в диагональную линию
        _sequenceTriangle.Append(transform.DOMove(Vector3.right + Vector3.up, _animationDuration).SetEase(_easeType));

        // Анимация движения среднего bond к верхнему правому углу
        _bonds[1].DOMove(Vector3.right + Vector3.up, _animationDuration).SetEase(_easeType)
            .OnComplete(() =>
            {
                // Анимация перемещения среднего bond к нижнему левому углу
                _bonds[1].DOMove(Vector3.left + Vector3.down, _animationDuration).SetEase(_easeType)
                    .OnComplete(() =>
                    {
                        // Анимация выхода за экран
                        transform.DOMove(Vector3.down * 5f, _animationDuration).SetEase(_easeType);
                        for (int i = 0; i < _bonds.Length; i++)
                        {
                            _bonds[i].DOMove(Vector3.down * 5f, _animationDuration).SetEase(_easeType);
                        }
                    });
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
    
    private Tween LineToZero()
    {
        float initialWidth = _drawChemicalBond.lineRenderer.startWidth; // Запоминаем начальную ширину линии
    
        // Используем DOTween для анимации изменения ширины линии до нуля за указанное время
        return DOTween.To(() => initialWidth, x => {
            _drawChemicalBond.lineRenderer.startWidth = x;
            _drawChemicalBond.lineRenderer.endWidth = x;
        }, 0f, _scaleTime).SetUpdate(true).OnComplete(() => {
            // Убеждаемся, что финальная ширина установлена точно в 0 (избегаем погрешностей)
            _drawChemicalBond.lineRenderer.startWidth = 0f;
            _drawChemicalBond.lineRenderer.endWidth = 0f;
        });
    }
}
