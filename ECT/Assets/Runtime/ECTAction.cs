using System;
using System.Collections.Generic;
using UnityEngine;

namespace ECT
{
    public class ECTAction
    {
        private List<Action> callbacks;

        public List<Action> Callbacks
        {
            get
            {
                callbacks ??= new();
                return callbacks;
            }
        }

        public bool Contains(Action function) => Callbacks.Contains(function);

        public void Subscribe(Action function)
        {
            if (Contains(function)) return;
            Callbacks.Add(function);
        }

        public void Unsubscribe(Action function)
        {
            if (!Contains(function)) return;
            Callbacks.Remove(function);
        }

        public void Execute()
        {
            foreach (var callback in Callbacks)
            {
                callback?.Invoke();
            }
        }
    }

    public interface IAction
    {
        public void Execute();
    }
}