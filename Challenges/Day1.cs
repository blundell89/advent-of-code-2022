using System.Reflection;
using Xunit.Abstractions;

namespace Challenges;

public class Day1
{
    private readonly ITestOutputHelper _testOutputHelper;

    public Day1(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task TopElfCalorieCounter()
    {
        var currentElf = new Elf(1);
        var highestCalorieCount = 0;
        
        await ReadData(async reader =>
        {
            var line = await reader.ReadLineAsync();
            switch (line)
            {
                case null:
                    return;
                case "":
                {
                    var currentCalorieCount = currentElf.Calories;
                    if (currentCalorieCount > highestCalorieCount)
                        highestCalorieCount = currentCalorieCount;
                    currentElf = new(currentElf.Id + 1);
                    break;
                }
                default:
                    currentElf.AddItem(int.Parse(line));
                    break;
            }
        });
        
        _testOutputHelper.WriteLine($"{nameof(TopElfCalorieCounter)} answer: {highestCalorieCount}");
    }

    [Fact]
    public async Task Top3ElfCalorieCounter()
    {
        var currentElf = new Elf(1);
        int top = 0, second = 0, third = 0;

        await ReadData(async reader =>
        {
            var line = await reader.ReadLineAsync();
            switch (line)
            {
                case null:
                    return;
                case "":
                {
                    var currentCalorieCount = currentElf.Calories;

                    if (currentCalorieCount > top)
                    {
                        third = second;
                        second = top;
                        top = currentCalorieCount;
                    }
                    else if (currentCalorieCount > second)
                    {
                        third = second;
                        second = currentCalorieCount;
                    } else if (currentCalorieCount > third)
                    {
                        third = currentCalorieCount;
                    }
                    currentElf = new(currentElf.Id + 1);
                    break;
                }
                default:
                    currentElf.AddItem(int.Parse(line));
                    break;
            }
        });
        
        _testOutputHelper.WriteLine($"{nameof(TopElfCalorieCounter)} answer: {top + second + third}");
    }

    private record Elf(int Id)
    {
        public int Calories { get; private set; }

        public void AddItem(int calories) => Calories += calories;
    }

    private static async Task ReadData(Func<StreamReader, Task> onReadLine)
    {
        await using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Challenges.Data.1.txt");
        if (stream is null)
            throw new InvalidOperationException("Stream is null");
        using var reader = new StreamReader(stream);

        while (!reader.EndOfStream)
            await onReadLine(reader);
    }
}