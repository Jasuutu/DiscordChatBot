using System;
using DiscordChatBot;
using FluentAssertions;
using NUnit.Framework;

namespace DiscordChatBotTests
{
    [TestFixture]
    public class MessageParserTests
    {
        [Test]
        public void MessageStartWithTest()
        {
            var message = "!test stuff";
            MessageParser.MessageStatsWith(message, "!test").Should().BeTrue();
        }
        [Test]
        public void RemoveTriggerWordTest()
        {
            MessageParser.RemoveTriggerWord("!test hello").Should().Be("hello");
        }

        [Test]
        public void ParseDiceMessageTest()
        {
            var result = MessageParser.ParseDiceMessage("!roll 1d8", new[] {' ', 'd'});
            Convert.ToInt32(result.Item1).Should().BeInRange(1, 8);
            result.Item2.Should().BeInRange(1, 8);
        }
    }
}
