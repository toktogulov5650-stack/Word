namespace Word.Application.DTOs.WordExplanations;

public class WordExplanationDto
{
    public int WordId { get; set; }

    public string EnglishWord { get; set; } = string.Empty;

    public string WhatIs { get; set; } = string.Empty;

    public string Meaning { get; set; } = string.Empty;

    public string Translations { get; set; } = string.Empty;

    public string Usage { get; set; } = string.Empty;

    public string Example1 { get; set; } = string.Empty;

    public string Example2 { get; set; } = string.Empty;

    public string Example3 { get; set; } = string.Empty;

    public string Hint { get; set; } = string.Empty;
}
