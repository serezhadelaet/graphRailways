using UnityEngine;

namespace Train.States
{
    public abstract class BaseState : MonoBehaviour
    {
        protected StateMachine Machine;

        public void Init(StateMachine stateMachine)
        {
            Machine = stateMachine;
        }
        
        public abstract void Enter();
        public virtual void Exit() { }
    }
}