# Запросы к базе рас

----
[Описание системы](../index.md) ➞ [Документация API](index.md) ➞ Запросы к базе рас

----

## База рас

База рас представляет собой набор карточек данных о расах. Модель этой структуры данных описана в сведениях о [карточке расы](../datadoc/race_card.md).

----

## Методы API

*Графа "ответ" в таблицах ниже не упоминает возврат кода состояния HTTP.*
*Знак вопроса после имени параметра означает, что данный параметр необязателен для осуществления запроса.*
<br/><br/>

### Запрос данных о социальных формированиях

**Доступ:** требуется аутентификация.

|	Адрес метода				|	Тип запроса			|	Параметры			| Ответ
|	:----:						|	:----:				|	:----:				| :----:
| **/api/races/get**			|	GET					| id					| JSON:<br/>race data

**Значения параметров:**
* `id` : уникальный ID расы.

Метод возвращает `race data` - набор данных о расе по запрошенному ID. Эти данные содержат значения полей, описанных в [карточке расы](../datadoc/race_card.md) и помеченных как выходные *(Out)*. Текущий метод возвращает все возможные данные о расе.
<br/><br/>

### Запрос списка рас

**Доступ:** требуется аутентификация.

|	Адрес метода				|	Тип запроса			|	Параметры			| Ответ
|	:----:						|	:----:				|	:----:				| :----:
| **/api/races/list**			|	GET					| count?<br/>from?		| JSON:<br/>races list

**Значения параметров:**
* `count` : количество рас, которые вернёт запрос;
* `from` : начиная с какой по счёту расы запрос вернёт список *(счёт определяется реальным размером получаемого списка, а не значением ID расы)*.

Метод вернёт список `count` рас и/или начиная с `from`-ой, если соответствующие параметры указаны. В противном случае вернёт список всех рас.

Возвращаемые данные - `races list` - содержат значения полей `Id` и `Name` каждой расы.
<br/><br/>

### Поиск расы по названию

**Доступ:** требуется аутентификация.

|	Адрес метода				|	Тип запроса			|	Параметры			| Ответ
|	:----:						|	:----:				|	:----:				| :----:
| **/api/races/find**			|	GET					| name					| JSON:<br/>races list

**Значения параметров:**
* `name` : искомая часть названия расы.

Метод вернёт список всех рас, названия которых содержат указываемую при запросе искомую подстроку `name`.

Возвращаемые данные - `races list` - содержат значения полей `Id` и `Name` каждой расы.
<br/><br/>

### Добавление новой расы

**Доступ:** для редакторов.

|	Адрес метода				|	Тип запроса			|	Параметры			| Ответ
|	:----:						|	:----:				|	:----:				| :----:
| **/api/races/list**			|	POST				| race_data				| —

**Значения параметров:**
* `socform_data` : значения полей, описанных в [карточке расы](../datadoc/race_card.md) и помеченных как входные *(In)*.

Для успешного добавления расы должны быть указаны все обязательные поля.
<br/><br/>

### Изменение данных расы

**Доступ:** для редакторов.

|	Адрес метода				|	Тип запроса			|	Параметры			| Ответ
|	:----:						|	:----:				|	:----:				| :----:
| **/api/races/change**			|	POST				| race_data			| —

**Значения параметров:**
* `race_data` : значения полей, описанных в [карточке расы](../datadoc/race_card.md) и помеченных как входные *(In)*, а также значение поля `Id` расы, для которой осуществляется запрос изменения данных.

Для успешного изменения данных расы требуется указать корректный ID изменяемой расы и корректные значения данных изменяемых полей. Изменены могут быть любые из доступных к изменению (помеченных как входные в [карточке расы](../datadoc/race_card.md)) полей.
<br/><br/>

### Удаление расы

**Доступ:** для редакторов.

|	Адрес метода				|	Тип запроса			|	Параметры			| Ответ
|	:----:						|	:----:				|	:----:				| :----:
| **/api/races/remove**			|	GET					| id					| —

**Значения параметров:**
* `id` : уникальный ID расы.

Для успешного удаления требуется указать корректный ID удаляемой расы.
<br/><br/>

### Запрос списка отсортированных рас

**Доступ:** требуется аутентификация.

|	Адрес метода				|	Тип запроса			|	Параметры			| Ответ
|	:----:						|	:----:				|	:----:				| :----:
| **/api/races/sortedlist**		|	GET					| count?<br/>from?		| JSON:<br/>races list

**Значения параметров:**
* `count` : количество рас, которые вернёт запрос;
* `from` : начиная с какой по счёту расы запрос вернёт список *(счёт определяется реальным размером получаемого списка, а не значением ID расы)*.

Метод вернёт список `count` рас и/или начиная с `from`-ой, если соответствующие параметры указаны. В противном случае вернёт список всех рас.

Список рас будет отсортирован по названию в алфавитном порядке *(сортировка осуществляется до применения параметров запроса: отбор количества и начальной для результата расы происходит уже на отсортированном списке)*.

Возвращаемые данные - `races list` - содержат значения полей `Id` и `Name` каждой расы.
<br/><br/>
