using UnityEngine;
using TMPro;
using DG.Tweening;
using Zenject;
using Assets.Game.Scripts.Domain.Signals;
using Assets.Game.Scripts.Domain.Models;

namespace Assets.Game.Scripts.Domain.Components
{
    public class Enemy : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField] private int _startHealth;
        [SerializeField] private TextMeshPro _text;
#pragma warning restore 0649

        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private int _health;

        private void Start()
        {
            _health = _startHealth;
            RefreshHpText();
        }

        public void ApplyDamage(int damage, BulletType bullet)
        {
            _health -= damage;
            
            RefreshHpText();

            if (_health <= 0)
            {
                _signalBus.Fire(new EnemyDown() { KilledBy = bullet });

                Destroy(transform.parent.gameObject);
            }
            else
            {
                transform.DOPunchScale(Vector3.up * 0.3f, 0.2f)
                         .OnComplete(()=> {
                             transform.localScale = Vector3.one;
                    });
            }
        }

        private void RefreshHpText()
        {
            _text.text = $"{_health}";
        }
    }
}
