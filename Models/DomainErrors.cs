namespace StoryTracker.Models;

public static class DomainErrors
{
    public static class Gpt
    {
        public static readonly Error InvalidRequest = new("Gpt.InvalidRequest", "Входящий запрос не может быть пустым.");
        public static readonly Error GenerationFailed = new("Gpt.GenerationFailed", "Не удалось сгенерировать ответ. Нейросеть вернула пустоту.");
        public static readonly Error ApiError = new("Gpt.ApiError", "Ошибка при обращении к API Gemini.");
        public static readonly Error ParseError = new("Gpt.ParseError", "Не удалось распарсить JSON ответ от нейросети.");
    }
}