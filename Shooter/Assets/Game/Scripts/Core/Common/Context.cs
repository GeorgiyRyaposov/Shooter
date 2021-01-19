using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.Scripts.Core.Common
{
    public interface IContext
    {
        void Attach(IContextObserver observer);
        void Detach(IContextObserver observer);
        void Notify(int propertyId);
    }

    [Serializable]
    public abstract class Context : IContext, ISerializationCallbackReceiver
    {
        public uint Id;

        [NonSerialized] private readonly List<IContextObserver> _observers = new List<IContextObserver>();

        public void Attach(IContextObserver view)
        {
            _observers.Remove(view);
            _observers.Add(view);
        }
        public void Detach(IContextObserver view)
        {
            _observers.Remove(view);
        }

        public void Notify(int propertyId)
        {
            for (int i = _observers.Count - 1; i >= 0; i--)
            {
                _observers[i].Refresh(propertyId);
            }
        }

        public void Clear()
        {
            _observers.Clear();
        }

        public void OnBeforeSerialize()
        {
            OnBeforeSerializeInternal();
        }
        public void OnAfterDeserialize()
        {
            OnAfterDeserializeInternal();
        }

        protected virtual void OnBeforeSerializeInternal()
        {
        }
        protected virtual void OnAfterDeserializeInternal()
        {
        }
    }

    [Serializable]
    public class ScriptableContext<T> : Context where T : ScriptableObject
    {
        [SerializeReference] public T Model;

        public ScriptableContext(T model)
        {
            Model = model;
        }

        public ScriptableContext()
        {
        }
    }

    [Serializable]
    public class Context<T> : Context where T : new()
    {
        [NonSerialized] public T Model;

        public Context()
        {
        }

        public Context(T model)
        {
            Model = model;
        }
    }
}
