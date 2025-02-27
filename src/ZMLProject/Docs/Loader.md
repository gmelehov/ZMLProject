## Loader
Загрузчик содержимого компонента.

###### ExtJS-класс:
```javascript
ZML.data.Loader
```

###### Формат ссылки на компонент:
```javascript
GLOBAL.APP.Loader.LoaderId
```

###### Синтаксис определения:
```xml
<Loader id="LoaderId"
        url="SomeUrl"
        mode="html|data|markdown|component"
        replaceContent="true|false"
        onbeforeload="ActionOnBeforeLoad"
        onafterload="ActionOnAfterLoad"
        descr="Описание компонента" />
```

##### Атрибуты
###### `id`
Обязательный атрибут.
Идентификатор загрузчика.

###### `url`
Обязательный атрибут.
Адрес ресурса-поставщика данных для загрузчика. 

###### `mode`
Обязательный атрибут.
Формат загружаемого содержимого. Варианты:
- `html` - содержимое в виде html-разметки (может включать в себя определения скриптов и стилей)
- `data`
- `markdown` - содержимое в виде markdown-разметки (автоматически компилируемое после получения в формат html)
- `component` - содержимое в виде extjs-определения дочерних компонентов

