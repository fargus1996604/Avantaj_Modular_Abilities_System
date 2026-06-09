using Gameplay.Abilities;
using Gameplay.Core;
using Gameplay.Core.Abilities;
using Gameplay.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.Infrastructure.Input
{
    [RequireComponent(typeof(BaseCharacterAdapter))]
    [RequireComponent(typeof(AbilityCastController))]
    public class PlayerInputProvider : MonoBehaviour
    {
        private BaseCharacterAdapter _characterAdapter;
        protected BaseCharacterAdapter CharacterAdapter => _characterAdapter ??= GetComponent<BaseCharacterAdapter>();

        private AbilityCastController _castController;
        protected AbilityCastController CastController => _castController ??= GetComponent<AbilityCastController>();

        [SerializeField]
        private Camera _mainCamera;

        private PlayerInputActions _inputActions;
        private IEntity _entity;

        private void Awake()
        {
            _inputActions = new PlayerInputActions();
            _entity = CharacterAdapter;
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
            if (CharacterAdapter == null || _entity == null)
                return;

            if (_entity.RestrictionController.HasRestriction(EntityRestrictionType.Input))
            {
                CharacterAdapter.SetMoveAxis(Vector2.zero);
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

                CharacterAdapter.SetMoveAxis(finalMoveAxis);
            }
            else
            {
                CharacterAdapter.SetMoveAxis(Vector2.zero);
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