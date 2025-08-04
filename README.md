# Semantic Kernel Function Calling Demo

A practical demonstration of AI function calling using Microsoft Semantic Kernel with local LLM (Llama 3.2:1b) via Ollama.

## 🎯 What This Demo Shows

This project demonstrates how a single AI agent can intelligently choose and execute different tools based on user input:

- **📊 Math Tools**: Add, multiply, divide, percentage calculations
- **⏰ Time Tools**: Current time, day of week, month/year
- **😂 Joke Tools**: Live programming jokes from external API
- **🤖 Smart Decision Making**: AI automatically selects appropriate tools

## 🛠️ Technology Stack

- **Microsoft Semantic Kernel 1.61.0** - AI orchestration framework
- **Ollama with Llama 3.2:1b** - Local LLM (only 1.3GB!)
- **.NET 9.0** - Modern C# platform
- **JokeAPI** - External REST API integration

## 🚀 Quick Start

### Prerequisites

1. **Install Ollama**: Download from [ollama.ai](https://ollama.ai)
2. **Pull the model**: 
   ```bash
   ollama pull llama3.2:1b
   ```
3. **Ensure Ollama is running**: 
   ```bash
   ollama serve
   ```

### Running the Demo

1. **Clone the repository**:
   ```bash
   git clone https://github.com/RajeevPentyala/SemanticKernalFunctionCalling.git
   cd SemanticKernalFunctionCalling
   ```

2. **Run the application**:
   ```bash
   dotnet run
   ```

3. **Try these examples**:
   - `What's 15 + 25?`
   - `What time is it?`
   - `Tell me a joke`
   - `Calculate 50% of 200`
   - `What day is today?`

## 🔧 How Function Calling Works

### Plugin Architecture
Each **plugin is a C# class** that groups related tools:
```csharp
/// <summary>
/// Math operations plugin - provides basic calculation capabilities
/// </summary>
public class MathTools
{
    [KernelFunction, Description("Add two numbers")]
    public double Add(double a, double b) => a + b;
}
```

### Tool Selection
Each **tool is a C# method** with a description that the AI uses for selection:
```csharp
[KernelFunction, Description("Divide two numbers")]
public double Divide(double a, double b) => b != 0 ? a / b : 0;
```

### The Magic
When you ask "What's 10 divided by 2?", the AI:
1. Reads all tool descriptions
2. Matches "Divide two numbers" to your request
3. Calls `Divide(10, 2)`
4. Returns the result

## 🧩 Project Structure

```
SingleAgent Demo/
├── Program.cs              # Entry point
├── SingleAgentDemo.cs      # Main demo logic and all plugins
├── SingleAgent Demo.csproj # Project configuration
└── README.md              # This file
```

## 🔍 Key Features Demonstrated

### 1. **Function Choice Behavior**
```csharp
var settings = new OllamaPromptExecutionSettings
{
    FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
};
```

### 2. **Plugin Registration**
```csharp
kernel.Plugins.AddFromType<MathTools>("Math");
kernel.Plugins.AddFromType<TimeTools>("Time");
kernel.Plugins.AddFromType<JokeTools>("Joke");
```

### 3. **External API Integration**
```csharp
[KernelFunction, Description("Get a random programming joke from JokeAPI")]
public async Task<string> GetRandomJoke()
{
    var response = await httpClient.GetStringAsync("https://v2.jokeapi.dev/joke/Programming?type=single");
    // Process and return joke
}
```

## 🎓 Learning Outcomes

After running this demo, you'll understand:

- How to set up Semantic Kernel with local LLMs
- The difference between plugins (classes) and tools (methods)
- How function descriptions guide AI decision-making
- Real-world API integration patterns
- Why model selection matters for function calling

## ⚠️ Important Notes

### Model Compatibility
**Not all LLMs support function calling!** This demo specifically uses Llama 3.2:1b because:
- ✅ Has function calling support
- ✅ Only 1.3GB in size
- ✅ Runs efficiently locally

We initially tried Gemma 3:1b but it lacks function calling capabilities.

### Content Filtering
External APIs may return content that triggers LLM safety filters. This is normal security behavior - the AI reviews external content before sharing it.

## 🌟 Real-World Applications

This pattern scales to production scenarios:
- **Customer Service**: Order lookup, inventory checks
- **Development Tools**: Code analysis, documentation generation
- **Business Intelligence**: Real-time data queries, reporting
- **Personal Assistants**: Calendar management, task automation

## 📚 Additional Resources

- [Microsoft Semantic Kernel Documentation](https://learn.microsoft.com/en-us/semantic-kernel/)
- [Function Calling Guide](https://learn.microsoft.com/en-us/semantic-kernel/concepts/function-calling/)
- [Creating Plugins](https://learn.microsoft.com/en-us/semantic-kernel/concepts/plugins/)
- [Ollama Documentation](https://ollama.ai)

## 🤝 Contributing

Feel free to:
- Open issues for questions or improvements
- Submit pull requests for enhancements
- Share your own function calling experiments

## 📄 License

This project is open source and available under the [MIT License](LICENSE).

---

**Built with ❤️ using Microsoft Semantic Kernel**
