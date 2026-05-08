namespace Word.API.Contracts.WordExplanations;

public class WordExplanationResponse
{
    public int WordId { get; set; }
    public string EnglishWord { get; set; } = string.Empty;
    public string WhatIs { get; set; } = string.Empty;
    public string Meaning { get; set; } = string.Empty;
    public string Translations { get; set; } = string.Empty;
    public string Usage { get; set; } = string.Empty;
    public string Hint { get; set; } = string.Empty;
    public IReadOnlyCollection<WordExampleResponse> Examples { get; set; } = [];
}
