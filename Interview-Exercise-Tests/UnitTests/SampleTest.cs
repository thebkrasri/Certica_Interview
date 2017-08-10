using System;
using NUnit.Framework;
using Rhino.Mocks;

namespace Interview_Exercise_Tests.UnitTests
{
    public class SampleTest
    {
        public class When_testing_a_class : TestFixtureBase
        {
            private bool _actualResult;

            protected override void Act()
            {
                _actualResult = new DoSomething().Execute();
            }

            [Test]
            public void Should_executed_with_success()
            {
                Assert.That(_actualResult, Is.True);
            }
        }

        public class When_testing_a_class_with_a_dependency : TestFixtureBase
        {
            private IDependency _suppliedDependency;

            protected override void Arrange()
            {
                _suppliedDependency = Mock<IDependency>();
                _suppliedDependency.Expect(x => x.Execute()).Return(false);
            }

            protected override void Act()
            {
                 var testSubject = new Thing(_suppliedDependency);
                 testSubject.DoTheThing();
            }

            [Test]
            public void Should_have_used_the_dependency()
            {
                _suppliedDependency.AssertWasCalled(x => x.Execute(), y=> y.Repeat.Once());
            }
        }

        public class When_an_exception_is_thrown : TestFixtureBase
        {
            protected override void Act()
            {
                throw new Exception("exception was thrown");
            }

            [Test]
            public void Should_set_actual_exception()
            {
                Assert.That(ActualException, Is.Not.Null);
            }

            [Test]
            public void Should_have_expected_message()
            {
                AssertAll(
                    () => Assert.That(ActualException.Message, Is.Not.Null),
                    () => Assert.That(ActualException.Message, Is.EqualTo("exception was thrown")));
            }
        }

        public interface IDependency
        {
            bool Execute();
        }

        private class DoSomething
        {
            public bool Execute()
            {
                return true;
            }
        }

        private class Thing
        {
            private readonly IDependency _dependency;

            public Thing(IDependency dependency)
            {
                _dependency = dependency;
            }

            public void DoTheThing()
            {
                _dependency.Execute();
            }
        }
    }
}