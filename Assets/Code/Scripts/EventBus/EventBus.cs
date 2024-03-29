using System.Collections.Generic;
using System.Linq;

namespace EventBus
{
    public static class EventBus<T> where T : IEvent
    {
        private static readonly HashSet<IEventBinding<T>> bindings = new();

        public static void Register(EventBinding<T> binding) => bindings.Add(binding);

        public static void Deregister(EventBinding<T> binding) => bindings.Remove(binding);

        public static void Invoke(T @event)
        {
            foreach (IEventBinding<T> binding in bindings.ToList())
            {
                binding.OnEvent.Invoke(@event);
                binding.OnEventNoArgs.Invoke();
            }
        }

        private static void Clear() =>
            //        Debug.Log($"Clearing {typeof(T).Name} bindings");
            bindings.Clear();
    }
}