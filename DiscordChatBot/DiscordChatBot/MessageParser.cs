using System;
using System.Linq;
using Discord;

namespace DiscordChatBot
{
    public class MessageParser
    {
        public static bool MessageStatsWith(string message, string checkingString)
        {
            return message.StartsWith(checkingString);
        }

        public static string RemoveTriggerWord(string message)
        {
            var result = message.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return string.Join(' ', result.Skip(1)) ;
        }

        public static Tuple<string, int> ParseDiceMessage(string message, char[] deliniaters)
        {

        Random rng = new Random();
        string[] splitMessage = message.Split(deliniaters, StringSplitOptions.RemoveEmptyEntries);

            int dice = int.Parse(splitMessage[1]);
            int value = int.Parse(splitMessage[2]);

            string resultNumbers = string.Empty;
            var result = 0;
            for (var i = 0; i < dice; i++)
            {
                int tempResult = rng.Next(1, value);

                if (i == dice - 1)
                {
                    resultNumbers += $"{tempResult}";
                }
                else
                {
                    resultNumbers += $"{tempResult} + ";
                }
                result += tempResult;
            }

            return new Tuple<string, int>(resultNumbers, result);
        }
    }
}