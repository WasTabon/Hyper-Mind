using DG.Tweening;
using UnityEngine;

public class ControllChemicalBonds : MonoBehaviour
{
    [SerializeField] private float _scaleTime = 1f;
    [SerializeField] private float _LifeTime = 50f;
    
    [SerializeField] private Ease _scaleEase;

    [SerializeField] private Transform[] _bonds;

    private DrawChemicalBond _drawChemicalBond; 
        
    private void Start()
    {
        _drawChemicalBond = GetComponent<DrawChemicalBond>();
        
        SetAllToZeroSizeAndPosition();
        
        Appear();
        
        //AppearDisappear();
    }

    #region Controll Start Appear And Line And End Disappear And Line

    private void AppearDisappear()
        {
            Sequence sequenceAppear = DOTween.Sequence();
            Vector3 _vectorOne = new Vector3(1, 1, 1);
    
            foreach (Transform bond in _bonds)
            {
                sequenceAppear.Join(bond.DOScale(_vectorOne, _scaleTime).SetEase(_scaleEase));
            }

            sequenceAppear.AppendInterval(_LifeTime - _scaleTime);
            
            sequenceAppear.OnComplete(() => { 
                foreach (Transform bond in _bonds)
                    bond.DOScale(Vector3.zero, _scaleTime).SetEase(_scaleEase);
            });
            
            sequenceAppear.Append(LineToZero());
    
            sequenceAppear.OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }
    
    private void Appear()
    {
        Sequence sequenceAppear = DOTween.Sequence();
        Vector3 _vectorOne = new Vector3(1, 1, 1);
    
        foreach (Transform bond in _bonds)
        {
            sequenceAppear.Join(bond.DOScale(_vectorOne, _scaleTime).SetEase(_scaleEase));
        }
    }
    
        private void SetAllToZeroSizeAndPosition()
        {
            gameObject.transform.position = Vector3.zero;
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
        
    #endregion

    #region Abilites

    private void ArrangeBondsInTriangle()
    {
        Debug.Log("123");
        
        // Рассчитываем координаты для треугольника
        float triangleHeight = 2f; // Высота треугольника
        float triangleBase = 2f; // Основание треугольника

        Vector3 topBondPosition = Vector3.up * triangleHeight / 2f; // Позиция верхней связи

        // Позиции для нижних связей
        Vector3 bottomLeftBondPosition = new Vector3(-triangleBase / 2f, -triangleHeight / 2f, 0f);
        Vector3 bottomRightBondPosition = new Vector3(triangleBase / 2f, -triangleHeight / 2f, 0f);

        // Используем DOTween для перемещения связей к вычисленным позициям
        _bonds[0].DOMove(topBondPosition, _scaleTime).SetEase(_scaleEase);
        _bonds[1].DOMove(bottomLeftBondPosition, _scaleTime).SetEase(_scaleEase);
        _bonds[2].DOMove(bottomRightBondPosition, _scaleTime).SetEase(_scaleEase);
    }

    #endregion
    
}
