using Gameplay.Core;
using Gameplay.Core.Abilities;
using Gameplay.Core.Components;
using Gameplay.Entities;
using NUnit.Framework;
using UnityEngine;

namespace Gameplay.Tests.Editor
{
   [TestFixture]
    public class EntityComponentsTest 
    {
        private BaseCharacterAdapter _characterAdapter;

        [SetUp]
        public void Setup()
        {
            var go = new GameObject("TestCharacter");
            go.AddComponent<CharacterController>(); 
            go.AddComponent<BaseCharacterAdapter>();
            go.AddComponent<CharacterAnimationController>();
            go.AddComponent<MovementComponent>();
            go.AddComponent<HealthComponent>();
            go.AddComponent<SoundComponent>();

            _characterAdapter = go.GetComponent<BaseCharacterAdapter>();
            _characterAdapter.RegisterComponentProviders();
        }

        [TearDown]
        public void TearDown()
        {
            if (_characterAdapter != null && _characterAdapter.gameObject != null)
            {
                Object.DestroyImmediate(_characterAdapter.gameObject);
            }
        }

        [Test]
        public void Entity_Should_Have_ComponentsProvider()
        {
            Assert.IsNotNull(_characterAdapter.GetComponentProvider<IAnimationController>(), "Animation controller provider should be registered.");
            Assert.IsNotNull(_characterAdapter.GetComponentProvider<IMovable>(), "Movement component provider should be registered.");
            Assert.IsNotNull(_characterAdapter.GetComponentProvider<IHealable>(), "Healable component provider should be registered.");
            Assert.IsNotNull(_characterAdapter.GetComponentProvider<IDamageable>(), "Damageable component provider should be registered.");
            Assert.IsNotNull(_characterAdapter.GetComponentProvider<ISoundController>(), "Sound component provider should be registered.");
        }
    }
}