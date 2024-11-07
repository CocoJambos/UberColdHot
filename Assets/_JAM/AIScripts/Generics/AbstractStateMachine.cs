using System;
using System.Collections.Generic;
using UnityEngine;

namespace JAM.Patterns.SM
{
    public abstract class AbstractStateMachine
    {
        private Dictionary<Type, IState> _states = new();
        private IState _currentState;
        private IState _previousState;

        public IState CurrentState => _currentState;

        public void RegisterState(IState state)
        {
            _states.Add(state.GetType(), state);
        }

        public void SetState<TState>() where TState : IState
        {
            SetState(_states[typeof(TState)]);
        }

        public void SetToPreviousState()
        {
            SetState(_previousState);
        }

        private void SetState(IState state)
        {
            if (_currentState != null)
            {
                _currentState.ExitState();
                _previousState = _currentState;
            }

            _currentState = state;
            _currentState.EnterState();
        }

        public void UpdateCurrentState(float deltaTime)
        {
            _currentState.UpdateState(deltaTime);
        }
    }
}