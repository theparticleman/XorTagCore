using System.Threading;

namespace XorTag.UnitTests.Domain;

public class ActionFrequencyCheckerTests
{
  public class When_actions_are_too_frequent : WithAnAutomocked<ActionFrequencyChecker>
  {
    [Test]
    public void It_should_throw_an_exception()
    {
      var playerId = 42;
      GetMock<ISettings>().Setup(x => x.MaxActionFrequencyMilliseconds).Returns(100);
      ClassUnderTest.CheckFreqency(playerId);
      Thread.Sleep(1);
      Assert.Throws<TooFrequentActionException>(() => ClassUnderTest.CheckFreqency(playerId));
    }
  }

  public class When_actions_are_NOT_too_frequent : WithAnAutomocked<ActionFrequencyChecker>
  {
    [Test]
    public void It_should_NOT_throw_an_exception()
    {
      var playerId = 42;
      GetMock<ISettings>().Setup(x => x.MaxActionFrequencyMilliseconds).Returns(1);
      ClassUnderTest.CheckFreqency(playerId);
      Thread.Sleep(5);
      ClassUnderTest.CheckFreqency(playerId);
    }
  }
}