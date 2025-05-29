# Unity VS Code Setup Guide

## Проблема
Ошибка: "Cannot activate the 'Unity' extension because it depends on the 'C#' extension from 'Anysphere', which is not installed."

## Решение

### 1. Установленные расширения
Убедитесь, что у вас установлены следующие расширения VS Code:
- `anysphere.csharp` - C# расширение от Anysphere
- `visualstudiotoolsforunity.vstuc` - Официальное расширение Unity

### 2. Требования
- .NET SDK 8.0+ (установлен: 8.0.407)
- Unity 2022.3+ 
- VS Code 1.80+

### 3. Конфигурация проекта

#### .vscode/extensions.json
```json
{
    "recommendations": [
      "visualstudiotoolsforunity.vstuc",
      "anysphere.csharp"
    ]
}
```

#### .vscode/launch.json
```json
{
    "version": "0.2.0",
    "configurations": [
        {
            "name": "Attach to Unity",
            "type": "vstuc",
            "request": "attach"
        }
     ]
}
```

#### .vscode/settings.json
- Настроен для исключения временных файлов Unity
- Указан основной solution файл: `"dotnet.defaultSolution": "TestTD.sln"`

### 4. Проверка работоспособности

#### Компиляция
```bash
dotnet restore TestTD.sln
dotnet build Assembly-CSharp.csproj
```

#### Тестовые файлы
- `Assets/TestScript.cs` - Базовый Unity скрипт с тестами API
- `Assets/TestRunner.cs` - Скрипт для запуска тестов
- `Assets/CameraFollow.cs` - Существующий скрипт камеры
- `Assets/FindHome.cs` - Существующий скрипт поиска

### 5. Функции, которые должны работать

✅ **IntelliSense** - Автодополнение Unity API  
✅ **Syntax Highlighting** - Подсветка синтаксиса C#  
✅ **Error Detection** - Обнаружение ошибок в реальном времени  
✅ **Go to Definition** - Переход к определению  
✅ **Debugging** - Отладка через Unity  
✅ **Code Formatting** - Форматирование кода  

### 6. Устранение неполадок

#### Если расширения не активируются:
1. Перезапустите VS Code
2. Убедитесь, что Unity проект открыт в корневой папке
3. Проверьте, что .sln файл существует
4. Выполните `dotnet restore`

#### Если IntelliSense не работает:
1. Откройте Command Palette (Ctrl+Shift+P)
2. Выполните "C#: Restart Language Server"
3. Убедитесь, что проект скомпилирован без ошибок

#### Если отладка не работает:
1. Убедитесь, что Unity запущен
2. В Unity: Edit → Preferences → External Tools → External Script Editor = VS Code
3. Используйте конфигурацию "Attach to Unity" в VS Code

### 7. Полезные команды

```bash
# Проверка установленных расширений
code --list-extensions

# Установка расширений
code --install-extension anysphere.csharp
code --install-extension visualstudiotoolsforunity.vstuc

# Компиляция проекта
dotnet build Assembly-CSharp.csproj

# Восстановление пакетов
dotnet restore TestTD.sln
```

## Статус проекта
✅ Все расширения установлены  
✅ Проект компилируется без ошибок  
✅ IntelliSense настроен  
✅ Отладка настроена  
✅ Тестовые скрипты созданы  

Проект готов к разработке! 