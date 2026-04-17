using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Word.Application.Abstractions.Services;
using Word.Application.DTOs.AI;
using Word.Infrastructure.Options;

namespace Word.Infrastructure.Services;

public class GeminiTextGenerationClient : IAiTextGenerationClient
{
    private readonly HttpClient _httpClient;
    private readonly GeminiOptions _options;

    public GeminiTextGenerationClient(
        HttpClient httpClient,
        IOptions<GeminiOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<string> GenerateAnswerAsync(
        IReadOnlyCollection<string> words,
        IReadOnlyCollection<AiMessageDto> messages,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_options.ApiKey))
            throw new Exception("Gemini ApiKey не настроен.");

        if (string.IsNullOrWhiteSpace(_options.Model))
            throw new Exception("Gemini Model не настроен.");

        if (string.IsNullOrWhiteSpace(_options.BaseUrl))
            throw new Exception("Gemini BaseUrl не настроен.");

        var selectedWordsText = string.Join(", ", words);

        var request = new GeminiGenerateContentRequest
        {
            SystemInstruction = new GeminiSystemInstruction
            {
                Parts =
                [
                    new GeminiPart
                    {
                        Text =
                            $"""
                            Ты помощник для изучения английских слов.
                            Отвечай только по этим словам: {selectedWordsText}.

                            Твои правила:
                            - объясняй просто и понятно
                            - отвечай на русском языке
                            - если пользователь спрашивает не по этим словам, мягко верни его к ним
                            - можешь объяснять значение, перевод, использование, примеры и как запомнить слово
                            """
                    }
                ]
            },
            Contents = BuildContents(messages)
        };

        var url = $"{_options.BaseUrl.TrimEnd('/')}/v1beta/models/{_options.Model}:generateContent?key={_options.ApiKey}";

        using var response = await _httpClient.PostAsJsonAsync(url, request, cancellationToken);

        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Ошибка Gemini API: {responseContent}");

        var geminiResponse = JsonSerializer.Deserialize<GeminiGenerateContentResponse>(
            responseContent,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        var answer = geminiResponse?
            .Candidates?
            .FirstOrDefault()?
            .Content?
            .Parts?
            .FirstOrDefault()?
            .Text;

        if (string.IsNullOrWhiteSpace(answer))
            throw new Exception("Gemini не вернул текст ответа.");

        return answer;
    }

    private static List<GeminiContent> BuildContents(IReadOnlyCollection<AiMessageDto> messages)
    {
        var contents = messages
            .Where(x => !string.IsNullOrWhiteSpace(x.Content))
            .Select(x => new GeminiContent
            {
                Role = MapRole(x.Role),
                Parts =
                [
                    new GeminiPart
                    {
                        Text = x.Content
                    }
                ]
            })
            .ToList();

        if (contents.Count == 0)
        {
            contents.Add(new GeminiContent
            {
                Role = "user",
                Parts =
                [
                    new GeminiPart
                    {
                        Text = "Объясни выбранные слова простым языком."
                    }
                ]
            });
        }

        return contents;
    }

    private static string MapRole(string role)
    {
        return role?.Trim().ToLowerInvariant() switch
        {
            "assistant" => "model",
            _ => "user"
        };
    }

    private sealed class GeminiGenerateContentRequest
    {
        public GeminiSystemInstruction SystemInstruction { get; set; } = new();
        public List<GeminiContent> Contents { get; set; } = [];
    }

    private sealed class GeminiSystemInstruction
    {
        public List<GeminiPart> Parts { get; set; } = [];
    }

    private sealed class GeminiContent
    {
        public string Role { get; set; } = string.Empty;
        public List<GeminiPart> Parts { get; set; } = [];
    }

    private sealed class GeminiPart
    {
        public string Text { get; set; } = string.Empty;
    }

    private sealed class GeminiGenerateContentResponse
    {
        public List<GeminiCandidate>? Candidates { get; set; }
    }

    private sealed class GeminiCandidate
    {
        public GeminiContentResponse? Content { get; set; }
    }

    private sealed class GeminiContentResponse
    {
        public List<GeminiPartResponse>? Parts { get; set; }
    }

    private sealed class GeminiPartResponse
    {
        public string? Text { get; set; }
    }
}
