# Ролевой доступ и управление пользователями

----
[Описание системы](../index.md) ➞ [Документация API](index.md) ➞ Ролевой доступ и управление пользователями

----

## Ролевой доступ

В PlrAPI реализованы различные политики доступа к методам API для различных пользовательских ролей. Установлены 4 роли (в порядке возрастания прав):
* пользователь (`user`);
* редактор (`editor`);
* администратор (`admin`);
* суперадминистратор (`superadmin`).

Доступ к большинству методов PlrAPI требует наличия как минимум роли "пользователь", что можно считать равносильным требованию аутентификации в API.

*Так как права ролей прямо иерархичны, требование прав, например, "для редакторов" означает требование к наличию роли редактора и выше: редактора, администратора или суперадминистратора.*

#### Пользователь (user)

Пользователи данной роли имеют доступ к методам запроса данных и не имеют прав на осуществление изменений в карточках данных (т.е. их добавление, редактирование или удаление).

#### Редактор (editor)

Пользователи данной роли имеют доступ к большинству методов API: к запросам на получение, добавление, изменение и удаление карточек данных.

#### Администратор (admin)

Пользователи данной роли, помимо прав редакторов, имеют возможность изменения роли других пользователей в том случае, когда такие пользователи не являются администраторами или суперадминистратором.

#### Суперадминистратор (superadmin)

К данной роли относится единственный создаваемый по умолчанию пользователь системы. Он обладает всеми правами администратора, а также имеет возможность изменения ролей администраторов и выдачи данных ролей.

Роль пользователя-суперадминистратора не может быть изменена. Данная роль не может быть назначена кому-либо.

----

## Методы API управления пользователями

*Графа "ответ" в таблицах ниже не упоминает возврат кода состояния HTTP.*  
*Знак вопроса после имени параметра означает, что данный параметр необязателен для осуществления запроса.*
<br/><br/>

### Список пользователей

**Доступ:** для администраторов.

|	Адрес метода					|	Тип запроса			|	Параметры			| Ответ
|	:----:							|	:----:				|	:----:				| :----:
| **/api/accounts/getuserslist**	|	GET					| count?<br />from?		| JSON:<br />список пользователей

**Значения параметров:**
* `count` : количество пользователей, которых вернёт запрос;
* `from` : начиная с какого по счёту пользователя запрос вернёт список *(порядок пользователей определён их ID)*.

Метод вернёт список `count` пользователей, начиная с `from`-ого, если параметры указаны. В противном случае, вернёт список всех пользователей системы.
<br/><br/>

### Изменение роли пользователя

**Доступ:** для администраторов или суперадминистраторов.

|	Адрес метода					|	Тип запроса			|	Параметры			| Ответ
|	:----:							|	:----:				|	:----:				| :----:
| **/api/accounts/getuserslist**	|	GET					| userId<br />role		| —

**Значения параметров:**
* `userId` : ID пользователя, для которого будет осуществлена попытка смены роли;
* `role` : новая роль пользователя *(заданная в текстовом виде: `user`, `editor`, `admin` или `superadmin`)*.

**Особенности работы метода:**
* Роль пользователя с ролью `superadmin` не может быть изменена.
* Роль `superadmin` не может быть назначена.
* Менять роль пользователей с ролью `admin` может только пользователь с ролью `superadmin`.

<br/><br/>