const flashcardButton = document.getElementById("flashcard");
const englishWordElement = document.getElementById("english-word");
const translationBlock = document.getElementById("translation-block");
const kyrgyzTranslationsElement = document.getElementById("kyrgyz-translations");
const nextButton = document.getElementById("next-button");
const statusText = document.getElementById("status-text");

let currentWordId = null;
let isLoading = false;

async function loadFlashcard() {
    if (isLoading) {
        return;
    }

    isLoading = true;
    nextButton.disabled = true;
    statusText.textContent = "Loading a random flashcard...";

    try {
        const query = currentWordId === null ? "" : `?excludeWordId=${currentWordId}`;
        const response = await fetch(`/api/flashcards/random${query}`);

        if (!response.ok) {
            throw new Error("Could not load flashcard.");
        }

        const flashcard = await response.json();

        currentWordId = flashcard.wordId;
        englishWordElement.textContent = flashcard.englishWord;
        kyrgyzTranslationsElement.textContent = flashcard.kyrgyzTranslations.join("\n");
        translationBlock.classList.add("hidden");
        statusText.textContent = "Tap the card to reveal the Kyrgyz translation.";
    } catch (error) {
        currentWordId = null;
        englishWordElement.textContent = "No flashcard";
        kyrgyzTranslationsElement.textContent = "";
        translationBlock.classList.add("hidden");
        statusText.textContent = error.message;
    } finally {
        isLoading = false;
        nextButton.disabled = false;
    }
}

flashcardButton.addEventListener("click", () => {
    if (!kyrgyzTranslationsElement.textContent) {
        return;
    }

    translationBlock.classList.toggle("hidden");
    statusText.textContent = translationBlock.classList.contains("hidden")
        ? "Tap the card to reveal the Kyrgyz translation."
        : "Press Next for another random word.";
});

nextButton.addEventListener("click", loadFlashcard);

loadFlashcard();
