# PP-Backend
| **API**                                                | **Описание**                         | **Текст запроса**                | **Текст ответа**    |
|--------------------------------------------------------|--------------------------------------|----------------------------------|---------------------|
| `GET /api/Account/token?username=admin&password=admin` | Получение токена                     | Нет                              | `Token`             |
| `POST /api/Account/new`                                | Создание пользователя                | `UserRegistrationModel`          | Нет                 |
| `GET /api/Account/info`                                | Получение информации о пользователе  | `Token`                          | `UserInfo`          |
| `POST /api/Account/join_group/{groupId}`               | Добавление  пользователя в группу    | `Token`                          | Нет                 |
| `DELETE /api/Account/leave_group/{groupId}`            | Удаление пользователя из группы      | `Token`                          | Нет                 |
| `GET /api/Account/get_groups`                          | Получение id всех групп пользователя | `Token`                          | `List<int>`         |
| `POST /api/Account/complete_test/{testId}`             | Завершить тест с testId              | `Token`,`List<string>`           | Нет                 |
| `POST /api/Account/add_test/{testId}`                  | Добавить пользователю тест           | `Token`                          | Нет                 |
| `DELETE /api/Account/delete_test/{testId}`             | Удалить тест у  пользователя         | `Token`                          | Нет                 |
| `POST /api/Test`                                       | Создание нового теста                | `TestRegistrationModel`          | Нет                 |
| `GET /api/Test/{id}`                                   | Получение теста по id                | Нет                              | `Test`              |
| `GET /api/Test/questions/{id}`                         | Получение вопросов теста по id       | Нет                              | `List<Question>`    |
| `POST /api/Group`                                      | Создание новой группы                | `Token`,`GroupRegistrationModel` | Id созданной группы |
| `GET /api/Group/{id}`                                  | Получение группы по id               | Нет                              | `Group`             |
| `POST /api/Group/add_test/{groupId}/{testId}`          | Добавить группе тест                 | Нет                              | Нет                 |
| `DELETE /api/Group/delete_test/{groupId}/{testId}`     | Удалить тест у группы                | Нет                              | Нет                 |


# `Token`
В местах где требуется токен нужно добавить запросу новый заголовок(header)
```json
"Authorization" : "Bearer " + Token
```

# `User`
```json
[
  {
    "id": "userId",
    "name": "username",
    "password": "userpassword",
    "role":"Teacher/Student",
    "managedGroups": [],
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
  "password": "userpassword",
  "role":"Teacher/Student"
}
```

# `UserInfo`
```json
{
  "name": "admin",
  "groups": [
    11111
  ],
  "complitedTests": {
    "testId": 0.5
  },
  "role":"Teacher/Student"
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

# `Group`
```json
{
  "id": 111,
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

# `GroupRegistrationModel`
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