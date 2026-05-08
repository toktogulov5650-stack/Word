namespace Word.Domain.Constants;

public static class DomainConstraints
{
    public const int LanguageCodeMaxLength = 10;

    public const int CategoryNameMaxLength = 100;
    public const int CategoryDescriptionMaxLength = 200;
    public const int CategoryImageUrlMaxLength = 500;

    public const int EnglishWordMaxLength = 100;
    public const int WordTranslationTextMaxLength = 100;

    public const int WordExplanationWhatIsMaxLength = 500;
    public const int WordExplanationMeaningMaxLength = 1000;
    public const int WordExplanationTranslationsMaxLength = 1000;
    public const int WordExplanationUsageMaxLength = 1000;
    public const int WordExplanationExampleMaxLength = 500;
    public const int WordExplanationHintMaxLength = 500;
}
