# Запросы к базе персонажей

----
[Описание системы](../index.md) ➞ [Документация API](index.md) ➞ Запросы к базе персонажей

----

## База персонажей

База персонажей представляет собой набор карточек данных о персонажах. Модель этой структуры данных описана в сведениях о [карточке персонажа](../datadoc/char_card.md).

----

## Методы API

*Графа "ответ" в таблицах ниже не упоминает возврат кода состояния HTTP.*
*Знак вопроса после имени параметра означает, что данный параметр необязателен для осуществления запроса.*
<br/><br/>

### Запрос данных о персонаже

**Доступ:** требуется аутентификация.

|	Адрес метода				|	Тип запроса			|	Параметры			| Ответ
|	:----:						|	:----:				|	:----:				| :----:
| **/api/characters/get**		|	GET					| id					| JSON:<br/>character data

**Значения параметров:**
* `id` : уникальный ID персонажа.

Метод возвращает `character data` - набор данных о персонаже по запрошенному ID. Эти данные содержат значения полей, описанных в [карточке персонажа](../datadoc/char_card.md) и помеченных как выходные *(Out)*. Текущий метод возвращает все возможные данные о персонаже.
<br/><br/>

### Запрос списка персонажей

**Доступ:** требуется аутентификация.

|	Адрес метода				|	Тип запроса			|	Параметры			| Ответ
|	:----:						|	:----:				|	:----:				| :----:
| **/api/characters/list**		|	GET					| count?<br/>from?		| JSON:<br/>characters list

**Значения параметров:**
* `count` : количество персонажей, которые вернёт запрос;
* `from` : начиная с какого по счёту персонажа запрос вернёт список *(счёт определяется реальным размером получаемого списка, а не значением ID персонажа)*.

Метод вернёт список `count` персонажей и/или начиная с `from`-ого, если соответствующие параметры указаны. В противном случае вернёт список всех персонажей.

Возвращаемые данные - `characters list` - содержат значения полей `Id` и `Name` каждого персонажа.
<br/><br/>

### Поиск персонажа по названию

**Доступ:** требуется аутентификация.

|	Адрес метода				|	Тип запроса			|	Параметры			| Ответ
|	:----:						|	:----:				|	:----:				| :----:
| **/api/characters/find**		|	GET					| name					| JSON:<br/>characters list

**Значения параметров:**
* `name` : искомая часть имени персонажа.

Метод вернёт список всех персонажей, имена (в том числе альтернативные - то есть поля `Name` и `AltNames`) которых содержат указываемую при запросе искомую подстроку `name`.

Возвращаемые данные - `characters list` - содержат значения полей `Id` и `Name` каждого персонажа.
<br/><br/>

### Добавление нового персонажа

**Доступ:** для редакторов.

|	Адрес метода				|	Тип запроса			|	Параметры			| Ответ
|	:----:						|	:----:				|	:----:				| :----:
| **/api/characters/add**		|	POST				| character_data		| —

**Значения параметров:**
* `character_data` : значения полей, описанных в [карточке персонажа](../datadoc/char_card.md) и помеченных как входные *(In)*.

Для успешного добавления персонажа должны быть указаны все обязательные поля.
<br/><br/>

### Изменение данных персонажа

**Доступ:** для редакторов.

|	Адрес метода				|	Тип запроса			|	Параметры			| Ответ
|	:----:						|	:----:				|	:----:				| :----:
| **/api/characters/change**	|	POST				| character_data		| —

**Значения параметров:**
* `character_data` : значения полей, описанных в [карточке персонажа](../datadoc/char_card.md) и помеченных как входные *(In)*, а также значение поля `Id` персонажа, для которой осуществляется запрос изменения данных.

Для успешного изменения данных персонажа требуется указать корректный ID изменяемого персонажа и корректные значения данных изменяемых полей. Изменены могут быть любые из доступных к изменению (помеченных как входные в [карточке персонажа](../datadoc/char_card.md)) полей.
<br/><br/>

### Удаление персонажа

**Доступ:** для редакторов.

|	Адрес метода				|	Тип запроса			|	Параметры			| Ответ
|	:----:						|	:----:				|	:----:				| :----:
| **/api/characters/remove**	|	GET					| id					| —

**Значения параметров:**
* `id` : уникальный ID персонажа.

Для успешного удаления требуется указать корректный ID удаляемого персонажа.
<br/><br/>

### Запрос списка отсортированных персонажей

**Доступ:** требуется аутентификация.

|	Адрес метода				|	Тип запроса			|	Параметры			| Ответ
|	:----:						|	:----:				|	:----:				| :----:
| **/api/characters/sortedlist**|	GET					| count?<br/>from?		| JSON:<br/>characters list

**Значения параметров:**
* `count` : количество персонажей, которые вернёт запрос;
* `from` : начиная с какого по счёту персонажа запрос вернёт список *(счёт определяется реальным размером получаемого списка, а не значением ID персонажа)*.

Метод вернёт список `count` персонажей и/или начиная с `from`-ого, если соответствующие параметры указаны. В противном случае вернёт список всех персонажей.

Список персонажей будет отсортирован по названию в алфавитном порядке *(сортировка осуществляется до применения параметров запроса: отбор количества и начального для результата персонажа происходит уже на отсортированном списке)*.

Возвращаемые данные - `characters list` - содержат значения полей `Id` и `Name` каждого персонажа.
<br/><br/>
