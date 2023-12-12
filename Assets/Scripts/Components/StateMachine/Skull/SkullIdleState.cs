using UnityEngine;

namespace StateMachine.Slave
{
    public class SkullIdleState : SkullBaseState
    {
        private static Vector2[] _directions = new[]
        {
            Vector2.up, new Vector2(1, 1), Vector2.right, new Vector2(1, -1), Vector2.down, new Vector2(-1, -1),
            Vector2.left, new Vector2(-1, 1),
        };

        private const float HoveringTime = 0.1f;

        private int _directionIdx = 0;
        private bool _isHover;
        private bool _isForward = false;
        private float _stayTime;
        private float _hoverTime;

        private bool _shouldPosition;


        public SkullIdleState(SkullStateMachine sm) : base(sm)
        {
        }

        public override ESkullState Key() => ESkullState.Idle;


        public override ESkullState NextState()
        {
            ESkullState newState = base.NextState();
            if (newState != Key()) { return newState; }

            if (_shouldPosition) { return ESkullState.Move; }

            return newState;
        }

        public override void Update()
        {
            base.Update();
            if (CanAction) { return; }

            Hover();
            UpdateHoverState();
            MoveToTargetIfCanChase();
        }

        private void MoveToTargetIfCanChase()
        {
            if (StateMachine.Target != null && StateMachine.IsEquipped)
            {
                _shouldPosition = true;
            }
        }

        private void Hover()
        {
            int dir = _isForward ? 1 : -1;
            Vector2 direction = _isHover ? _directions[_directionIdx] * dir : Vector2.zero;
            float deg = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if (_isHover)
            {
                StateMachine.SetDirection(direction.normalized);
            }
            else
            {
                StateMachine.SetDirection(Vector2.zero);
            }

            StateMachine.Rotate(deg);
        }

        private void UpdateHoverState()
        {
            if (_isHover)
            {
                _hoverTime += Time.deltaTime;
                if (_hoverTime >= HoveringTime)
                {
                    _hoverTime = 0f;
                    _isHover = false;
                }
            }
            else
            {
                _stayTime += Time.deltaTime;
                if (_stayTime >= 1f)
                {
                    _stayTime = 0f;
                    _isHover = true;
                    _isForward = !_isForward;
                    if (_isForward)
                    {
                        _directionIdx = (_directionIdx + 1) % _directions.Length;
                    }
                }
            }
        }

        public override void Enter()
        {
            base.Enter();
            _hoverTime = 0f;
            _stayTime = 0f;
            _directionIdx = 0;
            _shouldPosition = false;
            CanAction = false;
            _isHover = false;
            _isForward = false;
        }

        public override void Exit()
        {
            base.Exit();
            StateMachine.SetDirection(Vector2.zero);
        }
    }
}