# Запросы к списку полов

----
[Описание системы](../index.md) ➞ [Документация API](index.md) ➞ Запросы к списку полов

----

## Методы API

*Графа "ответ" в таблицах ниже не упоминает возврат кода состояния HTTP.*
<br/><br/>

### Запрос названия пола

**Доступ:** требуется аутентификация.

|	Адрес метода				|	Тип запроса			|	Параметры			| Ответ
|	:----:						|	:----:				|	:----:				| :----:
| **/api/genders/get**			|	GET					| id					| JSON:<br/>gender_name

**Значения параметров:**
* `id` : уникальный ID пола.

Метод возвращает `gender_name` - название пола по запрошенному ID.
<br/><br/>

### Запрос списка полов

**Доступ:** требуется аутентификация.

|	Адрес метода				|	Тип запроса			|	Параметры			| Ответ
|	:----:						|	:----:				|	:----:				| :----:
| **/api/genders/list**			|	GET					| —						| JSON:<br/>genders list

Метод вернёт список всех полов.

Возвращаемые данные - `genders list` - содержат значения полей `Id` и `Name` каждого пола.
<br/><br/>

### Запрос ID пола

**Доступ:** требуется аутентификация.

|	Адрес метода				|	Тип запроса			|	Параметры			| Ответ
|	:----:						|	:----:				|	:----:				| :----:
| **/api/genders/getid**		|	GET					| name					| JSON:<br/>gender_id

**Значения параметров:**
* `name` : название пола.

Метод возвращает `gender_id` - уникальный ID пола по его названию.
<br/><br/>
