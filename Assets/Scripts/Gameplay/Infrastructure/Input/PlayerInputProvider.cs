using Gameplay.Abilities;
using Gameplay.Core;
using Gameplay.Core.Abilities;
using Gameplay.Core.Components;
using Gameplay.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.Infrastructure.Input
{
    [RequireComponent(typeof(IEntity))]
    [RequireComponent(typeof(IMovable))]
    [RequireComponent(typeof(AbilityCastController))]
    public class PlayerInputProvider : MonoBehaviour
    {
        private IEntity _entity;
        protected IEntity Entity => _entity ??= GetComponent<IEntity>();
        
        private IMovable _movable;
        protected IMovable Movable => _movable ??= GetComponent<IMovable>();

        private AbilityCastController _castController;
        protected AbilityCastController CastController => _castController ??= GetComponent<AbilityCastController>();

        [SerializeField]
        private Camera _mainCamera;

        private PlayerInputActions _inputActions;

        private void Awake()
        {
            _inputActions = new PlayerInputActions();
        }

        private void OnEnable()
        {
            _inputActions.Player.Enable();
            _inputActions.Player.Ability.performed += OnAbilityPerformed;
        }

        private void OnDisable()
        {
            _inputActions.Player.Ability.performed -= OnAbilityPerformed;
            _inputActions.Player.Disable();
        }

        private void Update()
        {
            if (Entity == null)
                return;

            if (Entity.RestrictionController.HasRestriction(EntityRestrictionType.Input))
            {
                Movable.SetMoveAxis(Vector2.zero);
                return;
            }

            Vector2 rawInput = _inputActions.Player.Move.ReadValue<Vector2>();
            if (rawInput.sqrMagnitude > 0.01f)
            {
                Vector3 camForward = _mainCamera.transform.forward;
                Vector3 camRight = _mainCamera.transform.right;
                camForward.y = 0f;
                camRight.y = 0f;
                camForward.Normalize();
                camRight.Normalize();

                Vector3 calculatedDirection = camForward * rawInput.y + camRight * rawInput.x;
                Vector2 finalMoveAxis = new Vector2(calculatedDirection.x, calculatedDirection.z);

                Movable.SetMoveAxis(finalMoveAxis);
            }
            else
            {
                Movable.SetMoveAxis(Vector2.zero);
            }
        }

        private void OnAbilityPerformed(InputAction.CallbackContext context)
        {
            if (_entity == null || _entity.RestrictionController.HasRestriction(EntityRestrictionType.Input))
                return;

            if (int.TryParse(context.control.name, out int slot))
            {
                int configIndex = slot - 1;
                CastController.TryCastSlot(configIndex);
            }
        }
    }
}