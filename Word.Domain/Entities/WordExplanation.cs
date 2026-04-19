

namespace Word.Domain.Entities;

public class WordExplanation
{
    private WordExplanation()
    {
    }


    public WordExplanation(
        int wordId,
        string whatIs,
        string meaning,
        string translations,
        string usage,
        string example1,
        string example2,
        string example3,
        string hint)
    {
        if (wordId <= 0)
            throw new Exception("WordId должен быть больше 0");

        WordId = wordId;
        WhatIs = whatIs;
        Meaning = meaning;
        Translations = translations;
        Usage = usage;
        Example1 = example1;
        Example2 = example2;
        Example3 = example3;
        Hint = hint;
    }


    public int Id { get; private set; }
    public int WordId { get; private set; }
    public WordEntity Word { get; private set; } = null!;
    public string WhatIs { get; private set; } = string.Empty;
    public string Meaning { get; private set; } = string.Empty;
    public string Translations { get; private set; } = string.Empty;
    public string Usage { get; private set; } = string.Empty;
    public string Example1 { get; private set; } = string.Empty;
    public string Example2 { get; private set; } = string.Empty;
    public string Example3 { get; private set; } = string.Empty;
    public string Hint { get; private set; } = string.Empty;
}
