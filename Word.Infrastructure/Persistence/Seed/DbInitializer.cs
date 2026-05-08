using Microsoft.EntityFrameworkCore;
using Word.Domain.Entities;

namespace Word.Infrastructure.Persistence.Seed;

public static class DbInitializer
{
    public static async Task InitializeAsync(
        AppDbContext context,
        CancellationToken cancellationToken = default)
    {
        await context.Database.MigrateAsync(cancellationToken);
        await SeedCategoriesAsync(context, cancellationToken);
        await SeedWordsAsync(context, cancellationToken);
    }

    private static async Task SeedCategoriesAsync(
        AppDbContext context,
        CancellationToken cancellationToken)
    {
        var categories = new[]
        {
            new
            {
                ImageUrl = "/images/categories/verbs.png",
                KyrgyzName = "Этиштер",
                KyrgyzDescription = "Негизги англис тилиндеги этиштер.",
                RussianName = "Глаголы",
                RussianDescription = "Основные английские глаголы."
            },
            new
            {
                ImageUrl = "/images/categories/basic-words.png",
                KyrgyzName = "Негизги сөздөр",
                KyrgyzDescription = "Көп колдонулган негизги сөздөр.",
                RussianName = "Базовые слова",
                RussianDescription = "Часто используемые базовые слова."
            },
            new
            {
                ImageUrl = "/images/categories/travel.png",
                KyrgyzName = "Саякат",
                KyrgyzDescription = "Саякат жана жол жүрүү үчүн сөздөр.",
                RussianName = "Путешествия",
                RussianDescription = "Слова для путешествий и дороги."
            },
            new
            {
                ImageUrl = "/images/categories/fruits.png",
                KyrgyzName = "Жемиштер",
                KyrgyzDescription = "Мөмө-жемиштердин аталыштары.",
                RussianName = "Фрукты",
                RussianDescription = "Названия фруктов."
            }
        };

        var existingCategories = await context.Categories
            .Include(x => x.Translations)
            .ToListAsync(cancellationToken);

        var existingCategoriesByKyrgyzName = existingCategories
            .Select(category => new
            {
                Category = category,
                KyrgyzTranslation = category.Translations.FirstOrDefault(x =>
                    string.Equals(x.LanguageCode, "ky", StringComparison.OrdinalIgnoreCase))
            })
            .Where(x => x.KyrgyzTranslation is not null)
            .ToDictionary(x => x.KyrgyzTranslation!.Name, x => x.Category, StringComparer.OrdinalIgnoreCase);

        var hasChanges = false;

        foreach (var categoryDefinition in categories)
        {
            if (!existingCategoriesByKyrgyzName.TryGetValue(categoryDefinition.KyrgyzName, out var category))
            {
                category = new Category(categoryDefinition.ImageUrl);
                category.AddOrUpdateTranslation("ky", categoryDefinition.KyrgyzName, categoryDefinition.KyrgyzDescription);
                category.AddOrUpdateTranslation("ru", categoryDefinition.RussianName, categoryDefinition.RussianDescription);
                await context.Categories.AddAsync(category, cancellationToken);
                hasChanges = true;
                continue;
            }

            if (!string.Equals(category.ImageUrl, categoryDefinition.ImageUrl, StringComparison.Ordinal))
            {
                category.UpdateImageUrl(categoryDefinition.ImageUrl);
                hasChanges = true;
            }

            category.AddOrUpdateTranslation("ky", categoryDefinition.KyrgyzName, categoryDefinition.KyrgyzDescription);
            category.AddOrUpdateTranslation("ru", categoryDefinition.RussianName, categoryDefinition.RussianDescription);
            hasChanges = true;
        }

        if (hasChanges)
            await context.SaveChangesAsync(cancellationToken);
    }

    private static async Task SeedWordsAsync(
        AppDbContext context,
        CancellationToken cancellationToken)
    {
        var wordsByCategory = new Dictionary<string, IReadOnlyCollection<(string EnglishWord, string KyrgyzText)>>(
            StringComparer.OrdinalIgnoreCase)
        {
            ["Этиштер"] =
            [
                ("go", "баруу"),
                ("come", "келүү"),
                ("eat", "жөө"),
                ("drink", "ичүү"),
                ("sleep", "уктоо"),
                ("read", "окуу"),
                ("write", "жазуу"),
                ("speak", "сүйлөө"),
                ("listen", "угуу"),
                ("see", "көрүү"),
                ("open", "ачуу"),
                ("close", "жабуу"),
                ("work", "иштөө"),
                ("study", "окуу"),
                ("play", "ойноо"),
                ("walk", "жөө басуу"),
                ("run", "чуркоо"),
                ("buy", "сатып алуу"),
                ("help", "жардам берүү"),
                ("learn", "үйрөнүү")
            ],
            ["Негизги сөздөр"] =
            [
                ("hello", "салам"),
                ("goodbye", "кош бол"),
                ("yes", "ооба"),
                ("no", "жок"),
                ("please", "сураныч"),
                ("thanks", "рахмат"),
                ("sorry", "кечиресиз"),
                ("man", "эркек"),
                ("woman", "аял"),
                ("child", "бала"),
                ("friend", "дос"),
                ("house", "үй"),
                ("water", "суу"),
                ("food", "тамак"),
                ("day", "күн"),
                ("night", "түн"),
                ("time", "убакыт"),
                ("name", "ат"),
                ("city", "шаар"),
                ("school", "мектеп")
            ],
            ["Саякат"] =
            [
                ("airport", "аба майдан"),
                ("passport", "паспорт"),
                ("ticket", "билет"),
                ("hotel", "мейманкана"),
                ("room", "бөлмө"),
                ("flight", "учуу"),
                ("plane", "учак"),
                ("train", "поезд"),
                ("bus", "автобус"),
                ("taxi", "такси"),
                ("station", "бекет"),
                ("map", "карта"),
                ("road", "жол"),
                ("bag", "сумка"),
                ("suitcase", "чемодан"),
                ("border", "чек ара"),
                ("visa", "виза"),
                ("booking", "брондоо"),
                ("tourist", "саякатчы"),
                ("guide", "жол көрсөтүүчү")
            ],
            ["Жемиштер"] =
            [
                ("apple", "алма"),
                ("banana", "банан"),
                ("orange", "апельсин"),
                ("grape", "жүзүм"),
                ("pear", "алмурут"),
                ("peach", "шабдалы"),
                ("plum", "кара өрүк"),
                ("lemon", "лимон"),
                ("melon", "коон"),
                ("watermelon", "дарбыз"),
                ("cherry", "алча"),
                ("strawberry", "кулпунай"),
                ("raspberry", "малина"),
                ("pomegranate", "анар"),
                ("apricot", "өрүк"),
                ("pineapple", "ананас"),
                ("mango", "манго"),
                ("kiwi", "киви"),
                ("coconut", "кокос"),
                ("fig", "инжир")
            ]
        };

        var categoryNames = wordsByCategory.Keys.ToList();

        var categories = await context.CategoryTranslations
            .Where(x => x.LanguageCode == "ky" && categoryNames.Contains(x.Name))
            .Select(x => new { x.Name, x.CategoryId })
            .ToDictionaryAsync(x => x.Name, x => x.CategoryId, StringComparer.OrdinalIgnoreCase, cancellationToken);

        if (categories.Count == 0)
            return;

        var existingWords = await context.WordEntities
            .Include(x => x.WordTranslations)
            .ToListAsync(cancellationToken);

        var existingWordsByKey = existingWords.ToDictionary(
            x => CreateWordKey(x.CategoryId, x.EnglishWord),
            StringComparer.OrdinalIgnoreCase);

        var hasChanges = false;

        foreach (var category in wordsByCategory)
        {
            if (!categories.TryGetValue(category.Key, out var categoryId))
                continue;

            foreach (var word in category.Value)
            {
                var wordKey = CreateWordKey(categoryId, word.EnglishWord);

                if (!existingWordsByKey.TryGetValue(wordKey, out var existingWord))
                {
                    existingWord = new WordEntity(word.EnglishWord, categoryId);
                    existingWord.AddOrUpdateTranslation("ky", word.KyrgyzText);
                    await context.WordEntities.AddAsync(existingWord, cancellationToken);
                    existingWordsByKey[wordKey] = existingWord;
                    hasChanges = true;
                    continue;
                }

                existingWord.AddOrUpdateTranslation("ky", word.KyrgyzText);
                hasChanges = true;
            }
        }

        if (hasChanges)
            await context.SaveChangesAsync(cancellationToken);
    }

    private static string CreateWordKey(int categoryId, string englishWord)
    {
        return $"{categoryId}:{englishWord.Trim()}";
    }
}
