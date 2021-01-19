using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.Domain.Components
{
    public class ExplosionRangeVisual : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField] private MeshRenderer _meshRenderer;
#pragma warning restore 0649

        public void Show(Vector3 position, float size)
        {
            transform.position = position;
            transform.localScale = new Vector3(size, size, size);

            _meshRenderer.material.DOFade(0, 0.5f)
                .SetEase(Ease.OutSine)
                .OnComplete(() =>
                {
                    Destroy(gameObject);
                });
        }

        public class Factory : PlaceholderFactory<ExplosionRangeVisual>
        {
        }
    }
}
