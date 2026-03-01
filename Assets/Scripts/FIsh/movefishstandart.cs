using UnityEngine;
using DG.Tweening;

public class MoveFishStandart : MonoBehaviour
{
    [SerializeField] private float moveDuration = 2f;
    [SerializeField] private float turnDuration = 0.2f;     // сколько времени на поворот
    private Animator fishAnimator;

    private Tween currentTween;

    public SkinnedMeshRenderer SkinnedMeshRenderer;

    void Start()
    {
        fishAnimator = GetComponent<Animator>();
        moveDuration = UnityEngine.Random.Range(moveDuration * 0.7f, moveDuration);
        fishAnimator.speed = moveDuration;
        MoveRight();
        

    }

    void MoveRight()
    {
        if (currentTween != null) currentTween.Kill();

        // Поворачиваем лицом вправо (если нужно)
        transform.DORotate(new Vector3(0, 90, 0), turnDuration).SetEase(Ease.OutQuad);

        currentTween = transform.DOMoveX(5f, moveDuration)
            .SetEase(Ease.InOutSine)
            .OnComplete(MoveLeft);
    }

    void MoveLeft()
    {
        if (currentTween != null) currentTween.Kill();

        // Поворачиваем лицом влево
        transform.DORotate(new Vector3(0, -90, 0), turnDuration).SetEase(Ease.OutQuad);

        currentTween = transform.DOMoveX(-5f, moveDuration)
            .SetEase(Ease.InOutSine)
            .OnComplete(MoveRight);
    }

    void OnDestroy()
    {
        if (currentTween != null) currentTween.Kill();
    }
}