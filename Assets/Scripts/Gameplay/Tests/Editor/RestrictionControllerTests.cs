using Gameplay.Core;
using NUnit.Framework;
using UnityEngine;

namespace Gameplay.Tests.Editor
{
    public class RestrictionControllerTests
    {
        [Test]
        public void RestrictionController_Should_Register_And_Unregister_Restrictions()
        {
            IEntityRestrictionController controller = new DefaultRestrictionController();
            object ownerA = new object();
            object ownerB = new object();

            Assert.IsFalse(controller.HasRestriction(EntityRestrictionType.Input));

            controller.Register(EntityRestrictionType.Input, ownerA);
            Assert.IsTrue(controller.HasRestriction(EntityRestrictionType.Input));

            controller.Register(EntityRestrictionType.Input, ownerB);
            Assert.IsTrue(controller.HasRestriction(EntityRestrictionType.Input));

            controller.Unregister(EntityRestrictionType.Input, ownerA);
            Assert.IsTrue(controller.HasRestriction(EntityRestrictionType.Input));

            controller.Unregister(EntityRestrictionType.Input, ownerB);
            Assert.IsFalse(controller.HasRestriction(EntityRestrictionType.Input));
        }
    }
}
