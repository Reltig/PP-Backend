# PP-Backend
| **API**                        | **Описание**                   | **Текст запроса**         | **Текст ответа** |
|--------------------------------|--------------------------------|---------------------------|------------------|
| `POST /api/Users`              | Создание нового пользователя   | Имя пользователя и пароль | Нет              |
| `GET /api/Users/{id}`          | Получение пользователя по id   | Нет                       | Пользователь     |
| `POST /api/Test`               | Создание нового теста          | `List<Question>`            | Нет              |
| `GET /api/Test/{id}`           | Получение теста по id          | Нет                       | Тест             |
| `GET /api/Test/questions/{id}` | Получение вопросов теста по id | Нет                       | Вопросы теста    |

# `List<Question>`
```json
[
  {
    "text": "firstQuestionText",
    "rightAnswer": "firstAnswer",
    "possibleAnswers": [
      "firstPossibleAnswer",
      "secondPossibleAnswer"
    ]
  },
  {
    "text": "secondQuestionText",
    "rightAnswer": "secondAnswer",
    "possibleAnswers": []
  }
]
```