using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.Domain.Components
{
    public class ExplosionRangeVisual : MonoBehaviour, IPoolable<Vector3, float, IMemoryPool>
    {
#pragma warning disable 0649
        [SerializeField] private MeshRenderer _meshRenderer;
#pragma warning restore 0649

        private IMemoryPool _pool;

        public void OnDespawned()
        {
            _pool = null;
        }

        public void OnSpawned(Vector3 position, float size, IMemoryPool pool)
        {
            transform.position = position;
            transform.localScale = new Vector3(size, size, size);
            _pool = pool;

            //reset color
            var color = _meshRenderer.material.color;
            color.a = 1f;
            _meshRenderer.material.color = color;

            _meshRenderer.material.DOFade(0, 0.5f)
                .SetEase(Ease.OutSine)
                .OnComplete(() =>
                {
                    _pool.Despawn(this);
                });
        }

        public class Factory : PlaceholderFactory<Vector3, float, ExplosionRangeVisual>
        {
        }
    }
}
