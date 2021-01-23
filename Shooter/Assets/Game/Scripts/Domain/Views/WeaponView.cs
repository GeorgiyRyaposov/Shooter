using UnityEngine;

namespace Assets.Game.Scripts.Domain.Views
{
    public class WeaponView : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField] private Camera _camera;
        [SerializeField] private Animator _animator;
#pragma warning restore 0649

        private int _animatorShoot;

        private void Awake()
        {
            _animatorShoot = Animator.StringToHash("Shoot");
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Fire()
        {
            _animator.SetTrigger(_animatorShoot);
        }

        public Ray GetFireRay()
        {
            return new Ray(_camera.transform.position, _camera.transform.forward);
        }
    }
}
