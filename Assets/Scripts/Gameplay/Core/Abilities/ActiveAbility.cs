using System.Collections.Generic;

namespace Gameplay.Core.Abilities
{
    public class ActiveAbility
    {
        private readonly IReadOnlyCollection<IAbilityAction> _actions;
        private readonly IAbilityContext _context;
        private readonly List<ITickableAction> _tickableActions = new();
        
        public bool IsFinished => _tickableActions.Count == 0;

        public ActiveAbility(IAbilityContext context, IReadOnlyCollection<IAbilityAction> actions)
        {
            _context = context;
            _actions = actions;
            ExecuteAll();
        }

        private void ExecuteAll()
        {
            foreach (var action in _actions)
            {
                action.Execute(_context);
                if (action is ITickableAction tickable)
                {
                    _tickableActions.Add(tickable);
                }
            }
        }

        public void Update(float deltaTime)
        {
            for (int i = _tickableActions.Count - 1; i >= 0; i--)
            {
                bool isActionFinished = _tickableActions[i].Tick(_context, deltaTime);

                if (isActionFinished)
                {
                    _tickableActions.RemoveAt(i);
                }
            }
        }
    }
}