using System;
using EventBus;
using GameplayStates;

namespace Events
{
    public struct OnGameplayStateChanged : IEvent
    {
        public readonly Type OldState;
        public readonly Type NewState;
        
        public OnGameplayStateChanged(BaseState oldState, BaseState newState)
        {
            OldState = oldState?.GetType();
            NewState = newState.GetType();
        }
    }
}