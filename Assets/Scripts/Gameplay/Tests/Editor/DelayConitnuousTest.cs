using NUnit.Framework;
using System.Collections.Generic;
using Gameplay.Abilities.Data;
using Gameplay.Core.Abilities;
using UnityEngine;

namespace Gameplay.Tests.Editor
{
    public class MockDelayedAction : DelayedActionBase
    {
        public bool HasStarted { get; private set; }

        public MockDelayedAction(PlayTimeData playTime) : base(playTime) { }

        protected override void OnStart(IAbilityContext context)
        {
            HasStarted = true;
        }
    }

    public class MockContinuousAction : ContinuousDelayedActionBase
    {
        public bool HasStarted { get; private set; }
        public bool HasEnded { get; private set; }
        public int TickCount { get; private set; }

        public MockContinuousAction(PlayTimeData playTime, DurationType durationType, float duration) 
            : base(playTime, durationType, duration) { }

        protected override void OnContinuousStart(IAbilityContext context)
        {
            HasStarted = true;
        }

        protected override void OnTick(IAbilityContext context, float deltaTime)
        {
            TickCount++;
        }

        protected override void OnContinuousEnd(IAbilityContext context)
        {
            HasEnded = true;
        }
    }

    [TestFixture]
    public class DelayedActionsTests
    {
        private IAbilityContext _context;

        [SetUp]
        public void Setup()
        {
            _context = new DefaultContext();
        }

        [Test]
        public void DelayedAction_With_PlayTimeStart_Should_Execute_Immediately()
        {
            var playTime = new PlayTimeData { Type = PlayTimeType.Start, Duration = 0f };
            var action = new MockDelayedAction(playTime);

            action.Execute(_context);

            Assert.IsTrue(action.HasStarted, "Action with Start type must execute immediately.");
        }

        [Test]
        public void DelayedAction_With_Delay_Should_Not_Start_Until_Time_Passes()
        {
            var playTime = new PlayTimeData { Type = PlayTimeType.Delay, Duration = 1.5f };
            var action = new MockDelayedAction(playTime);

            action.Execute(_context);
            Assert.IsFalse(action.HasStarted, "Action should not start immediately during Execute if it has a delay.");

            bool isFinished = action.Tick(_context, 1.0f);
            Assert.IsFalse(action.HasStarted, "Action should remain idle while delay time has not expired.");
            Assert.IsFalse(isFinished, "Action should not be marked as finished while waiting for delay.");

            isFinished = action.Tick(_context, 0.6f);
            Assert.IsTrue(action.HasStarted, "Action must trigger OnStart after delay duration ends.");
            Assert.IsTrue(isFinished, "Instant action must return true immediately on the frame it starts.");
        }

        [Test]
        public void ContinuousAction_InstantType_Should_Start_And_End_In_Same_Frame_After_Delay()
        {
            var playTime = new PlayTimeData { Type = PlayTimeType.Delay, Duration = 1.0f };
            var action = new MockContinuousAction(playTime, DurationType.Instant, duration: 0f);

            action.Execute(_context);
            bool isFinished = action.Tick(_context, 1.1f);

            Assert.IsTrue(action.HasStarted, "Action should execute after delay.");
            Assert.IsTrue(action.HasEnded, "Instant duration type must trigger OnContinuousEnd in the same frame.");
            Assert.IsTrue(isFinished, "Tick must return true to indicate the action lifecycle is complete.");
        }

        [Test]
        public void ContinuousAction_ContinuousType_Should_Tick_And_End_Only_After_Duration()
        {
            var playTime = new PlayTimeData { Type = PlayTimeType.Delay, Duration = 0.5f };
            var action = new MockContinuousAction(playTime, DurationType.Continuous, duration: 2.0f);

            action.Execute(_context);

            bool isFinished = action.Tick(_context, 0.3f);
            Assert.IsFalse(action.HasStarted);
            Assert.AreEqual(0, action.TickCount, "OnTick must not be called during the delay phase.");

            isFinished = action.Tick(_context, 0.3f);
            Assert.IsTrue(action.HasStarted, "OnContinuousStart must trigger when delay ends.");
            Assert.IsFalse(action.HasEnded, "OnContinuousEnd must not trigger immediately at initialization.");
            Assert.AreEqual(0, action.TickCount, "OnTick should wait for the subsequent execution frames.");
            Assert.IsFalse(isFinished, "Action shouldn't finish right as the delay ends.");

            action.Tick(_context, 0.5f);
            action.Tick(_context, 0.5f);
            Assert.AreEqual(2, action.TickCount, "OnTick should increment cleanly on sequence frames inside duration phase.");
            Assert.IsFalse(action.HasEnded, "Action should not end yet since running duration is not exceeded.");

            isFinished = action.Tick(_context, 1.1f);
            
            Assert.IsTrue(action.HasEnded, "OnContinuousEnd must be called after running duration time expires.");
            Assert.IsTrue(isFinished, "Tick must return true to cleanly garbage collect and finish the active action.");
        }
    }
}
