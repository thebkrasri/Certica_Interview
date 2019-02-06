using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;

namespace InterviewExercise.Tests
{
    [TestFixture]
    public abstract class TestFixtureBase
    {
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

        [OneTimeSetUp]
        public virtual void RunOnceBeforeAny()
        {
            //Arrange
            try
            {
                Arrange();
            }
            catch (Exception ex)
            {
                var handled = HandleArrangeException(ex);
                if (!handled) throw;
            }

            //Act
            try
            {
                Act();
            }
            catch (Exception ex)
            {
                ActualException = ex;
            }
        }

        [OneTimeTearDown]
        public virtual void RunOnceAfterAll()
        {
            // Make sure exception was inspected.
            if (_actualException != null && !_actualExceptionInspected)
                throw new AssertionException(
                    $"The exception of type '{_actualException.GetType().Name}' was not inspected by the test:\r\n {_actualException}.");
        }

        protected virtual void Arrange()
        {
        }

        /// <summary>
        /// Executes the code to be tested.
        /// </summary>
        protected virtual void Act()
        {
        }

        protected Mock<T> Mock<T>() where T : class => new Mock<T>();

        protected virtual bool HandleArrangeException(Exception ex) => false;

        public void AssertAll(params Action[] asserts)
        {
            var errorMessages = new List<Exception>();

            foreach (var assert in asserts)
                try
                {
                    assert.Invoke();
                }
                catch (Exception exc)
                {
                    errorMessages.Add(exc);
                }

            if (!errorMessages.Any()) return;

            string errorMessageString = string.Join($"{Environment.NewLine}{Environment.NewLine}", errorMessages);

            Assert.Fail($"The following conditions failed:{Environment.NewLine}{errorMessageString}");
        }
    }
}