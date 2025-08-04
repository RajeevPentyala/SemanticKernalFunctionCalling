using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Ollama;
using System.ComponentModel;
using System.Text.Json;

/// <summary>
/// Single Agent Demo - All agent logic, plugins, and tools in one organized class
/// </summary>
public class SingleAgentDemo
{
    public static async Task Run()
    {
        Console.WriteLine("🤖 Simple Single Agent Demo");
        Console.WriteLine("══════════════════════════");
        Console.WriteLine("This agent has:");
        Console.WriteLine("📊 Math Tools: Add, Multiply, Divide, Percentage");
        Console.WriteLine("⏰ Time Tools: Current time, Day of week, Month/Year");
        Console.WriteLine("😂 Joke Tools: Fresh clean jokes from API");
        Console.WriteLine("💭 AI Knowledge: General chat and assistance");
        Console.WriteLine();

        // Create the AI kernel with Ollama connection
        var builder = Kernel.CreateBuilder();
        builder.AddOllamaChatCompletion(
            modelId: "llama3.2:1b",
            endpoint: new Uri("http://localhost:11434")
        );
        var kernel = builder.Build();

        // Register plugins - each plugin groups related tools together
        kernel.Plugins.AddFromType<MathTools>("Math");
        kernel.Plugins.AddFromType<TimeTools>("Time");
        kernel.Plugins.AddFromType<JokeTools>("Joke");

        Console.WriteLine("✅ Single Agent ready!");
        Console.WriteLine("📝 Try asking:");
        Console.WriteLine("   • What's 15 + 25?");
        Console.WriteLine("   • What time is it?");
        Console.WriteLine("   • Tell me a joke");
        Console.WriteLine("   • Calculate 50% of 200");
        Console.WriteLine("   • What day is today?");
        Console.WriteLine("   • Type 'exit' to quit");
        Console.WriteLine();

        // Interactive chat loop
        while (true)
        {
            Console.Write("You: ");
            string? input = Console.ReadLine();
            
            if (string.IsNullOrEmpty(input) || input.ToLower() == "exit")
            {
                Console.WriteLine("👋 Goodbye!");
                break;
            }
            
            try
            {
                Console.Write("🤖 Agent: ");
                
                // Configure function calling behavior
                var settings = new OllamaPromptExecutionSettings
                {
                    FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(),
                    Temperature = 0.1f
                };
                var arguments = new KernelArguments(settings);
                
                var response = await kernel.InvokePromptAsync(input, arguments);
                Console.WriteLine(response.GetValue<string>());
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
                Console.WriteLine("💡 Make sure Ollama is running with llama3.2:1b");
                Console.WriteLine();
            }
        }
    }
}

/// <summary>
/// Math operations plugin - provides basic calculation capabilities
/// </summary>
public class MathTools
{
    [KernelFunction, Description("Add two numbers together (example: 5 + 3, what is 10 plus 15)")]
    public double Add(double a, double b)
    {
        Console.WriteLine($"🔧 [MATH TOOL] Adding {a} + {b}");
        return a + b;
    }
    
    [KernelFunction, Description("Multiply two numbers together (example: 5 * 3, 20 times 200, what is 10 multiplied by 15)")]
    public double Multiply(double a, double b)
    {
        Console.WriteLine($"🔧 [MATH TOOL] Multiplying {a} × {b}");
        return a * b;
    }
    
    [KernelFunction, Description("Divide two specific numbers provided by the user")]
    public double Divide(double a, double b)
    {
        Console.WriteLine($"🔧 [MATH TOOL] Dividing {a} ÷ {b}");
        return b != 0 ? a / b : 0;
    }
    
    [KernelFunction, Description("Calculate what percentage one number is of another, based on user's specific numbers")]
    public double Percentage(double number, double percent)
    {
        Console.WriteLine($"🔧 [MATH TOOL] Calculating {percent}% of {number}");
        return number * (percent / 100);
    }
}

/// <summary>
/// Time operations plugin - provides date and time information
/// </summary>
public class TimeTools
{
    [KernelFunction, Description("Get current date and time when user asks what time it is or for current date")]
    public string GetCurrentTime()
    {
        Console.WriteLine("🔧 [TIME TOOL] Getting current time");
        return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
    
    [KernelFunction, Description("Get current day of the week when user asks what day it is")]
    public string GetDayOfWeek()
    {
        Console.WriteLine("🔧 [TIME TOOL] Getting day of week");
        return DateTime.Now.DayOfWeek.ToString();
    }
    
    [KernelFunction, Description("Get current month and year when user asks for current month or year")]
    public string GetMonthYear()
    {
        Console.WriteLine("🔧 [TIME TOOL] Getting month and year");
        return DateTime.Now.ToString("MMMM yyyy");
    }
}

/// <summary>
/// External API integration plugin - demonstrates real-time web API calls
/// </summary>
public class JokeTools
{
    private static readonly HttpClient httpClient = new HttpClient();
    
    [KernelFunction, Description("Tell a clean family-friendly joke when user asks for a joke, humor, or something funny")]
    public async Task<string> GetRandomJoke()
    {
        Console.WriteLine("🔧 [JOKE TOOL] Fetching fresh clean joke from API");
        
        try
        {
            var response = await httpClient.GetStringAsync("https://v2.jokeapi.dev/joke/Any?blacklistFlags=nsfw,political,racist,sexist,explicit&type=single");
            var jokeData = JsonSerializer.Deserialize<JsonElement>(response);
            
            if (jokeData.TryGetProperty("joke", out var jokeElement))
            {
                var joke = jokeElement.GetString() ?? "Sorry, couldn't get the joke content!";
                return $"Here's a clean joke from the API: {joke}";
            }
            else
            {
                return "Sorry, the joke API returned an unexpected format!";
            }
        }
        catch (Exception ex)
        {
            return $"Sorry, couldn't fetch a joke right now. Error: {ex.Message}";
        }
    }
}
