using Gameplay.Entities;
using NUnit.Framework;
using UnityEngine;

namespace Gameplay.Tests.Editor
{
    [TestFixture]
    public class HealthComponentTests
    {
        private HealthComponent _healthComponent;

        [SetUp]
        public void Setup()
        {
            var go = new GameObject("TestCharacter");
            _healthComponent = go.AddComponent<HealthComponent>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_healthComponent.gameObject);
        }

        [Test]
        public void TakeDamage_ShouldDecreaseHealth_And_NotGoBelowZero()
        {
            _healthComponent.TakeDamage(30);
            _healthComponent.TakeDamage(150);

            Assert.IsTrue(_healthComponent.CurrentHealth == 0);
        }
        
        [Test]
        public void Heal_ShouldIncreaseHealth_And_NotGoAbove100()
        {
            _healthComponent.Heal(300);
            Assert.IsTrue(_healthComponent.CurrentHealth == 100);
        }
    }
}
