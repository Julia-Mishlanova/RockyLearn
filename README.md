# 1. Введение (Introduction):
Приложение за создание которого я взялась - Rocky интернет-магазин в котором пользователи могут выбирать товары и оставлять свои запросы. Администраторы могут создавать и управлять запросами от клиентов, обращать их в заказы.
Этот pet-проект создавался с целью ознакомиться с базовыми возможностями asp .NET core и EFC.

# 2. Описание проекта (Project Description):
Есть фильтрация товаров по категории. Можно посмотреть детали товара, добавить в корзину, после добавления корзина заполняется и если вы вернетесь к товару, попытаетесь его добавить, то здесь уже не будет такой возможности, взамен можно будет удалить товар из корзины.
В правом верхнем углу вы можете увидеть register / login. Вы можете зарегистрировать новую учетную запись администратора или обычного клиента или войти в уже созданный аккаунт.
#### Customer Role
После авторизации можно попасть в корзину и взглянуть на список выбранных нами товаров, далее вы можете удалить товар если хотите или подтвердить запрос. И после отправки запроса вы получите письмо на адрес электронной почты. И это те функции которые доступны пользователю. 
#### Admin Role
По умолчанию, при инициализации базы данных, вместе с ролями ADMIN и CUSTOMER создается аккаунт админа:
admin@gmail.com Admin123*
Мы не можем зарегистрировать нового администратора будучи авторизованным как пользователь. Однако благодаря аккаунту админа по умолчанию мы можем от него создавать новые учетные записи в роли админа.

В роли админа вам будет доступна новая вкладка - Content Management в которых есть category в которых можно выполять базовые CRUD операции, которые имеются и у типа товара и у товара. 
Раздел Товара имеет более сложную архитектуру по сравнению с другими разделами, потому что здесь гораздо больше полей, здесь так же отображается текстовое поле с использованием стилей. 
Присутсвует валидация полей ввода.

Админ при добавлении товара в корзину, может указывать количество товара. Затем, если мы зайдем в корзину то мы увидим количество в квадратных футах и общую стоимость - эти функции будут доступны только для админа.
Функция оформления заказа работает так, что если кто-то зайдет в магазин, допустим клиент, выбрав некий товар, то админ создаст для него заказ. Админ заполнит все реквизиты клиента и клиент введет номер карты, здесь я использую платежный сервис Braintree для обработки кредитных карт.
После оформления заказа с карты клиента будет списана отплата. 
И если мы перейдем в Order/Inquiry Management, внутрь списка заказов, то админ может просмотреть заказы, сделать изменения и обновить статус заказа. Можно сортировать заказы с помощью тонкой настроки в Syncfusion.

# 3. Технические детали (Technical Details):
Эти технологии и инструменты собираются вместе, чтобы создать мощное и функциональное веб-приложение.
#### Веб-приложение
- ASP.NET Core: Веб-приложение написано на платформе ASP.NET Core
#### Работа с базой данных и ORM
- SSMS (SQL Server Management Studio): Инструмент для управления базами данных Microsoft SQL Server.
- Entity Framework Core: ORM-фреймворк для взаимодействия с базой данных.
- Миграции в EFC: Инструмент, позволяющий управлять изменениями в базе данных и обновлять ее согласно изменениям в модели данных.
#### Безопасность и валидация
- Валидация в ASP.NET Core: Инструмент, который позволяет проверять и обеспечивать правильность введенных пользователем данных, что помогает предотвратить ошибки и повысить безопасность приложения.
- Razor Identity: Фреймворк для аутентификации и авторизации пользователей.
- UserSecret: Механизм хранения конфиденциальных данных в коде, таких как секретные ключи и пароли, в безопасном месте во время разработки приложения.
#### Отправка электронных писем
- SmtpServer: Инструмент для отправки электронных писем. Gmail SMTP - один из популярных почтовых серверов для этой цели.
#### Платежные транзакции
- Braintree: Платежная платформа, которая обеспечивает безопасные и надежные платежные транзакции.
#### Хранение данных и сессии
- Session в ASP.NET Core: Инструмент для хранения пользовательских данных, когда пользователь находится в веб-приложении.
- TempData: Временное хранилище данных, которое позволяет передавать сообщения и информацию между запросами.
#### Паттерны проектирования
- Паттерн Репозиторий: для абстрагирования доступа к данным и управления данными в приложении. Для избежания прямого использования dbcontext в каждом контроллере. Если я захочу изменить класс мне не придется менять ссылки во всех контроллерах
- IoC (Inversion of Control): позволяет управлять зависимостями и внедрять их в вашем приложении. удобно когда изменения настроек сервисов можно производить в одном месте и это класс стартап
- MVC (Model-View-Controller): разделяет приложение на модели, представления и контроллеры для легкости сопровождения и разработки.
- ViewModel: используется для представления данных в форме, удобной для визуализации и взаимодействия с пользователем.
- DI (Dependency Injection): позволяют писать слабосвязный код.
- Code-First: Подход в Entity Framework Core для определения структуры БД через код C# и автоматического создания/обновления БД на основе этого кода.
#### Скрипты и Стили
- SweetAlert: библиотека для создания стилизованных диалоговых окон и предупреждений.
- Syncfusion: предоставляет множество готовых элементов управления и инструментов для упрощения разработки.
- Font Awesome: предоставляет иконки и стили для веб-приложений, что делает их более привлекательными и интуитивно понятными.
- Toastr - для создания стилизованных уведомлений.
- Summernote: WYSIWYG-редактор, позволяет пользователям легко форматировать и создавать текстовое содержимое для описания товара.

# 4. Уроки и достижения (Lessons Learned and Achievements):
Работая над созданием приложения я сталкивалась с некоторыми трудностями, ярким примером я считаю поиск альтернатив для некоторых закрытых сервисов. Нужно было заменить сервис отправки электронных писем и дополнительно прикрутить UserSecret.
Так же я впервые знакомилась с паттернами проектирования, разделяя ответственность и сепарируя свой код на более независимые библиотеки классов. 

Однако я применила знания и опыт в следующих проектах в полной мере, на момент окончания работы над приложением я уже точно знала что нужно внедрять свой проект для получения задуманного результата.