using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;

namespace Interview_Exercise_Tests
{
    [TestFixture]
    public abstract class TestFixtureBase
    {
        protected MockRepository mocks;

        private Exception _actualException;
        private bool _actualExceptionInspected;

        protected Exception ActualException
        {
            get
            {
                _actualExceptionInspected = true;
                return _actualException;
            }
            set
            {
                _actualExceptionInspected = false;
                _actualException = value;
            }
        }

        [TestFixtureSetUp]
        public virtual void RunOnceBeforeAny()
        {
            mocks = new MockRepository();

            // Arrange
            try
            {
                EstablishContext();
                Arrange();
            }
            catch (Exception ex)
            {
                var handled = HandleArrangeException(ex);
                if (!handled)
                    throw;
            }
            // Stop recording
            mocks.ReplayAll();

            //Act
            try
            {
                // Allow execution of code just prior to behavior execution
                BeforeBehaviorExecution();

                // Execute the behavior
                try
                {
                    ExecuteBehavior();
                    Act();
                }
                catch (Exception ex)
                {
                    ActualException = ex;
                }
            }
            finally
            {
                // Allow cleanup surrounding behavior execution, prior to final cleanup
                AfterBehaviorExecution();
            }
        }

        [TestFixtureTearDown]
        public virtual void RunOnceAfterAll()
        {
            // Make sure all objects are now in replay mode
            mocks.ReplayAll();

            // Make sure all defined mocks are satisfied
            mocks.VerifyAll();

            // Make sure exception was inspected.
            if (_actualException != null && !_actualExceptionInspected)
            {
                throw new AssertionException($"The exception of type '{_actualException.GetType() .Name}' was not inspected by the test:\r\n {_actualException}.");
            }
        }

        protected virtual void Arrange()
        {
        }

        protected virtual void EstablishContext()
        {
        }

        protected virtual void BeforeBehaviorExecution()
        {
        }

        protected virtual void AfterBehaviorExecution()
        {
        }

        /// <summary>
        /// Executes the code to be tested.
        /// </summary>
        protected virtual void Act()
        {
        }

        protected virtual void ExecuteBehavior()
        {
        }

        protected T Stub<T>() where T : class
        {
            return MockRepository.GenerateStub<T>();
        }

        protected T Mock<T>() where T : class
        {
            return MockRepository.GenerateMock<T>();
        }

        protected virtual bool HandleArrangeException(Exception ex)
        {
            return false;
        }

        protected void AssertAll(params Action[] asserts)
        {
            var errorMessages = new List<Exception>();
            foreach (var assert in asserts)
            {
                try
                {
                    assert.Invoke();
                }
                catch (Exception ex)
                {
                    errorMessages.Add(ex);
                }
            }

            if (errorMessages.Any())
            {
                var separator = $"{Environment.NewLine}{Environment.NewLine}";
                string errorMessageString = string.Join(separator, errorMessages);

                Assert.Fail("The following conditions failed:{0}{1}", Environment.NewLine, errorMessageString);
            }
        }
    }
}