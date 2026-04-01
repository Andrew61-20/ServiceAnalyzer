# Service Analyzer

Test task for Backend (C#) Developer.

## 📌 Description

The application fetches posts from Reddit, filters them by keywords, and returns structured results.  
Supports Docker запуск, логування та простий UI.

---

## ⚙️ Technologies

- .NET 8
- ASP.NET Core Web API
- HttpClient
- Docker / Docker Compose
- Custom File Logger

---

## 🚀 Run with Docker

docker-compose up --build

UI
Simple HTML UI available at:
http://localhost:8080

📝 Logging

Logs are written to:
logs/out.log

🧪 Features
-- Filtering by title and post body
-- Detecting images in posts
-- Multithreading (async/await)
-- Error handling
-- File output
-- Docker support
________________________________________________________________

Example request:
json
{
  "items": [
    {
      "subreddit": "/r/aww",
      "keywords": ["cat", "dog"]
    }
  ],
  "limit": 10
}

----------------------------

Потенційні проблеми з парсингом HTTP + HTML.

Використання HTTP-запитів та парсингу HTML/JSON може призвести до кількох проблем:

1. Обмеження швидкості / блокування

Веб-сайти (наприклад, Reddit) можуть блокувати запити без належних заголовків.

Рішення:
Використовувати належний User-Agent
Впроваджувати політики повторних спроб
Використовувати офіційні API, коли це можливо

2. Зміни в структурі сторінки

Структура HTML може змінитися → перерви парсера.

Рішення:

Використовувати JSON API замість парсингу HTML
Додавати резервну логіку
Використовувати надійні селектори

3. Нестабільність мережі

Час очікування, проблеми з DNS тощо

Рішення:

Додавати час очікування
Правильна обробка винятків

4. Проблеми з продуктивністю

Кілька запитів можуть уповільнити систему.

Рішення:

Використовувати async/await
Паралельна обробка (Task.WhenAll)
Кешування
