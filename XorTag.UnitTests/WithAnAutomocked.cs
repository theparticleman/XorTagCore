using AutoMoqCore;
using Moq;

namespace XorTag.UnitTests
{
    public abstract class WithAnAutomocked<T>
    {
        public T ClassUnderTest { get; set; }
        private AutoMoqer mocker = new AutoMoqer();

        public WithAnAutomocked()
        {
            ClassUnderTest = mocker.Create<T>();
        }

        protected Mock<TMock> GetMock<TMock>() where TMock : class
        {
            return mocker.GetMock<TMock>();
        }

        protected TAny IsAny<TAny>()
        {
            return It.IsAny<TAny>();
        }
    }
}