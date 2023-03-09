using System;
using System.Collections.Generic;

namespace ECT
{
    public struct ECTAction : IAction
    {
        public ECTAction(params Action[] callbacks) => Callbacks = new(callbacks);

        HashSet<Action> Callbacks { get; }

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
            foreach (Action callback in Callbacks)
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