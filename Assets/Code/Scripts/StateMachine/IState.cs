﻿namespace StateMachine
{
    public interface IState
    {
        public void OnEnter();
        public void Update();
        public void OnExit();
        public void Dispose();
    }
}