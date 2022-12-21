# PP-Backend
| **API**                                           | **Описание**                            | **Текст запроса**        | **Текст ответа** |
|---------------------------------------------------|-----------------------------------------|--------------------------|------------------|
| `GET /api/Account/token?username=admin&password=admin` | Получение токена                        | Нет                      | `Token`          |
| `POST /api/Users`                                 | Создание нового пользователя            | `UserRegistrationModel`  | Нет              |
| `GET /api/Users/{id}`                             | Получение пользователя по id            | Нет                      | `User`           |
| `POST /api/Users/add/{group_id}`                  | Добавление  пользователя в группу       | `Token`                  | Нет              |
| `POST /api/Users/test/evaluate/{testId}`          | Оценка теста                            | `Token`, `List<string>`  | Нет              |
| `POST /api/Users/add-test/{testId}/{userId}`      | Добавить пользователю тест              | Нет                      | Нет              |
| `POST /api/Test`                                  | Создание нового теста                   | `TestRegistrationModel`  | Нет              |
| `GET /api/Test/{id}`                              | Получение теста по id                   | Нет                      | `Test`           |
| `GET /api/Test/questions/{id}`                    | Получение вопросов теста по id          | Нет                      | `List<Question>` |
| `POST /api/Group`                                 | Создание новой группы                   | `GroupRegistrationModel` | Нет              |
| `GET /api/Group/{id}`                             | Получение группы по id                  | Нет                      | `Group`          |
| `DELETE /api/Group/{groupId}/{userId}`            | Удаление пользователя из группы         | Нет                      | Нет              |

# `Token`

# `User`
```json
[
  {
    "id": "userId",
    "name": "username",
    "password": "userpassword",
    "avaibleTestsIdList": [
      "firstAvaibleTestId",
      "secondAvaibleTestId"
    ],
    "complitedTests": {
      "firstCompletedTestId": 0,
      "secondCompletedTestId": 100
    }
  }
]
```

# `UserRegistrationModel`
```json
{
  "name": "username",
  "password": "userpassword"
}
```

# `Test`
```json
{
  "id": "testId",
  "testName": "testName",
  "questionsList": List<Question>
}
```

# `TestRegistrationModel`
```json
{
  "testName": "testName",
  "questionsList": List<Question>
}
```

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

# `Group, GroupRegistrationModel`
```json
{
  "name": "groupName",
  "members" : [
    "firstMemberId",
    "secondMemberId"
  ],
  "tests": [
    "firstTestId",
    "secondTestId"
  ]
}
```