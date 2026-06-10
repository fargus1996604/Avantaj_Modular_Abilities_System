using System.Collections.Generic;
using Gameplay.Core.Abilities;
using NUnit.Framework;

namespace Gameplay.Tests.Editor
{
    public class ActiveAbilityTests
    {
        private class TestTickableAction : ITickableAction
        {
            private readonly float _duration;
            private float _elapsedTime;

            public bool IsExecuted { get; private set; }
            public bool IsFinished { get; private set; }

            public TestTickableAction(float duration)
            {
                _duration = duration;
            }

            public void Execute(IAbilityContext context)
            {
                IsExecuted = true;
            }

            public bool Tick(IAbilityContext context, float deltaTime)
            {
                _elapsedTime += deltaTime;
                if (_elapsedTime >= _duration)
                {
                    IsFinished = true;
                    return true;
                }
                return false;
            }
        }

        [Test]
        public void ActiveAbility_Should_Execute_All_Actions_On_Start()
        {
            var context = new DefaultContext();
            var action1 = new TestTickableAction(1f);
            var action2 = new TestTickableAction(2f);
            var actions = new List<IAbilityAction> { action1, action2 };

            var ability = new ActiveAbility(context, actions);

            Assert.IsTrue(action1.IsExecuted);
            Assert.IsTrue(action2.IsExecuted);
            Assert.IsFalse(ability.IsFinished);
        }

        [Test]
        public void ActiveAbility_Should_Tick_Actions_In_Parallel()
        {
            var context = new DefaultContext();
            var shortAction = new TestTickableAction(1f);
            var longAction = new TestTickableAction(3f);
            var actions = new List<IAbilityAction> { shortAction, longAction };

            var ability = new ActiveAbility(context, actions);

            ability.Update(1.5f);

            Assert.IsTrue(shortAction.IsFinished);
            Assert.IsFalse(longAction.IsFinished);
            Assert.IsFalse(ability.IsFinished);

            ability.Update(2.0f);

            Assert.IsTrue(longAction.IsFinished);
            Assert.IsTrue(ability.IsFinished);
        }
    }
}
