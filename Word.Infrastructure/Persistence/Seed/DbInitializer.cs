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
        var categories = new List<Category>
        {
            new("Этиштер", "Негизги англис тилиндеги этиштер."),
            new("Негизги сөздөр", "Көп колдонулган негизги сөздөр."),
            new("Саякат", "Саякат жана жол жүрүү үчүн сөздөр."),
            new("Жемиштер", "Мөмө-жемиштердин аталыштары.")
        };

        var existingCategoryNames = await context.Categories
            .Select(x => x.Name)
            .ToListAsync(cancellationToken);

        var existingNames = new HashSet<string>(existingCategoryNames, StringComparer.OrdinalIgnoreCase);

        var missingCategories = categories
            .Where(x => !existingNames.Contains(x.Name))
            .ToList();

        if (missingCategories.Count == 0)
        {
            return;
        }

        await context.Categories.AddRangeAsync(missingCategories, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    private static async Task SeedWordsAsync(
        AppDbContext context,
        CancellationToken cancellationToken)
    {
        var wordsByCategory = new Dictionary<string, IReadOnlyCollection<(string EnglishWord, string KyrgyzWord)>>(
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

        var categories = await context.Categories
            .Where(x => categoryNames.Contains(x.Name))
            .ToDictionaryAsync(x => x.Name, x => x.Id, StringComparer.OrdinalIgnoreCase, cancellationToken);

        if (categories.Count == 0)
        {
            return;
        }

        var existingWords = await context.WordEntities
            .Select(x => new { x.CategoryId, x.EnglishWord })
            .ToListAsync(cancellationToken);

        var existingWordKeys = new HashSet<string>(
            existingWords.Select(x => CreateWordKey(x.CategoryId, x.EnglishWord)),
            StringComparer.OrdinalIgnoreCase);

        var wordsToAdd = new List<WordEntity>();

        foreach (var category in wordsByCategory)
        {
            if (!categories.TryGetValue(category.Key, out var categoryId))
            {
                continue;
            }

            foreach (var word in category.Value)
            {
                var wordKey = CreateWordKey(categoryId, word.EnglishWord);

                if (existingWordKeys.Contains(wordKey))
                {
                    continue;
                }

                wordsToAdd.Add(new WordEntity(word.EnglishWord, word.KyrgyzWord, categoryId));
                existingWordKeys.Add(wordKey);
            }
        }

        if (wordsToAdd.Count == 0)
        {
            return;
        }

        await context.WordEntities.AddRangeAsync(wordsToAdd, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    private static string CreateWordKey(int categoryId, string englishWord)
    {
        return $"{categoryId}:{englishWord.Trim()}";
    }
}
