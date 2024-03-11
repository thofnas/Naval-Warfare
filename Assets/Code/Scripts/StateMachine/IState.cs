namespace StateMachine
{
    public interface IState
    {
        public void OnCreated();
        public void OnEnter();
        public void Update();
        public void FixedUpdate();
        public void OnExit();
    }
}