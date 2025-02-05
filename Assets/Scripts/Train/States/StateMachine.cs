using System;
using System.Collections.Generic;
using UnityEngine;

namespace Train.States
{
    [RequireComponent(typeof(ITrain))]
    public class StateMachine : MonoBehaviour
    {
        [SerializeField] private List<BaseState> _availableStates;

        public ITrain Train { get; private set; }
        private Dictionary<Type, BaseState> _states = new ();
        private BaseState _currentState;

        private void Awake()
        {
            Train = GetComponent<ITrain>();
        }

        private void Start()
        {
            foreach (var s in _availableStates)
            {
                _states[s.GetType()] = s;
                s.Init(this);
            }
            SetState<MoveToMineState>();
        }

        public void SetState<T>() where T : BaseState
        {
            if (!_states.TryGetValue(typeof(T), out var state))
                return;
            _currentState?.Exit();
            _currentState = state;
            _currentState.Enter();
        }
    }
}