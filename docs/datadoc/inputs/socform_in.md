# Добавление/изменение карточки персонажа

----
[Описание системы](../../index.md) ➞ [Карточка социального формирования](../socform_card.md) ➞ Добавление/изменение карточки
| [PlrAPI: SocForms](../../apidoc/api_socforms.md)

----

В таблице ниже указаны имена и описания полей, которые должны или могут быть заданы для добавления новой карточки данных или изменения существующей.

|Параметр 						|Имя параметра в API	|Описание		|Значение сброса										
|:----: 						|:----:					|:----:			|:----:							
|ID						|Id						|Уникальный ID карточки соц.-форм.	| 
|Название соц.-форм.	|Name					|Название соц.-форм.				| 
|Описание соц.-форм.	|Desc					|Описание соц.-форм.				| Empty string

##### Дополнительная информация:
* `Значение сброса` - это значение, которое соответствует нулевому, отсутствующему значению, которое должно быть записано в карточку данных (для изменения значения на пустое). **Если значение не указано - поле не может быть сброшено.**
* Для полей, которые не передаются в метод API, изменение значений не происходит.
* Поле `Id` обязательно должно быть указано при передаче в метод изменения карточки и не оказывает никакого эффекта при передаче в метод добавления карточки.
