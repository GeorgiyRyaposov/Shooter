using UnityEngine;

namespace Assets.Game.Scripts.Core.Common
{
    public interface IContextObserver
    {
        void Refresh(int propertyId);
        void Attach(IContext context);
        void Detach();
    }

    public abstract class ContextView : MonoBehaviour, IContextObserver
    {
        private IContext _context;
        public IContext Context => _context;

        public void Attach(IContext context)
        {
            _context = context;
            _context?.Attach(this);

            Initialize();
        }
        public virtual void Detach()
        {
            _context?.Detach(this);
        }

        public virtual void Initialize() { }


        public abstract void Refresh(int propertyId);
    }
}