# StoryTracker 🎲

[![NET](https://img.shields.io/badge/.NET-8.0-512bd4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Docker](https://img.shields.io/badge/Docker-enabled-2496ed?logo=docker)](https://www.docker.com/)
[![AI](https://img.shields.io/badge/Powered%20by-Gemini%203.1%20Flash-orange?logo=googlegemini)](https://ai.google.dev/)

**StoryTracker** — это бэкенд-сервис для автоматизации работы Мастера Подземелий (Dungeon Master) в настольных ролевых играх (D&D 5e / Foundry VTT). Сервис использует продвинутые модели ИИ для генерации глубоких, сюжетно связанных NPC и автоматически упаковывает их реальным игровым лутом из локальных компендиумов.

---

## ✨ Ключевые фичи

* **Умная генерация NPC:** Нейросеть генерирует не просто сухие статы, а полноценную личность — зацепки для игроков (Hooks), тайны, биографию, характер и Journal Entries.
* **Синхронизация с компендиумами (Foundry VTT / база Лаару):** ИИ не выдумывает предметы из головы. Вместо этого он генерирует валидные теги (`Type`, `Rarity`), по которым бэкенд вытаскивает честные игровые объекты из локального дампа (включая Active Effects, переводы Babele и пути к иконкам).
* **Строгая типизация через JSON Schema:** Использование рекурсивных схем (`ResponseSchema`) гарантирует 100% стабильный JSON от Gemini API без галлюцинаций и сломанной структуры.

---

## 🛠️ Стек технологий

* **Backend:** C# .NET 8, ASP.NET Core Web API
* **AI Integration:** Google Gemini API (семейство моделей `gemini-3.1`) c использованием Structured Outputs
* **Data & Tools:** System.Text.Json (рекурсивная сериализация схем), Docker

---

## 🚀 Быстрый запуск

### 1. Клонирование репозитория
```bash
git clone https://github.com/DorogaSmerti/AiGeneratorForDND.git
cd StoryTracker
```

### 2. Настройка конфигурации (Local Development)
Все локальные настройки и ключи авторизации вынесены в `appsettings.json`. Добавьте туда секцию для работы с ИИ:

```json
{
  "GeminiApi": {
    "ApiKey": "YOUR_GEMINI_API_KEY_HERE"
  }
}
```

### 3. Запуск приложения
```bash
dotnet restore
dotnet run --project StoryTracker.WebApi
```

---

## 📊 Пример сгенерированного ответа (Выходной JSON)

Сервис обогащает ответ от ИИ реальными данными из файлов компендиумов DnD5e:

```json
{
  "name": "Loren",
  "race": "Человек",
  "class": "Wizard",
  "hookOrSecret": "Лорен украл карту, ведущую к гробнице забытого мага...",
  "inventoryTags": [
    { "type": "equipment", "rarity": "common" },
    { "type": "magic", "rarity": "uncommon" }
  ],
  "equipment": [
    {
      "_id": "GHpAGx25Pj5cxf64",
      "name": "Побег омелы / Sprig of Mistletoe",
      "type": "weapon",
      "img": "icons/consumables/plants/fern-lady-green.webp",
      "system": { "description": { "value": "<p>Живая веточка омелы...</p>" } }
    }
  ]
}
```
