# WeatherApp - Погодное веб-приложение

Погодное веб-приложение на .NET Framework с использованием ASP.NET MVC, отображающее текущую погоду, почасовой прогноз и прогноз на 3 дня для города Москва.

## 🌟 Возможности

- **Текущая погода**: Температура, ощущается как, влажность, ветер, давление, видимость
- **Почасовой прогноз**: Оставшиеся часы текущего дня и все часы следующего дня
- **Прогноз на 3 дня**: Максимальная/минимальная температура, условия, вероятность дождя, восход/закат
- **Автообновление**: Данные обновляются автоматически каждые 10 минут
- **Адаптивный дизайн**: Работает на всех устройствах
- **Обработка ошибок**: Показ загрузки и ошибок с кнопкой повторного запроса

## 🛠 Технологии

- **.NET Framework 4.8**
- **ASP.NET MVC 5**
- **C#**
- **Razor Views**
- **HTML5 / CSS3**
- **jQuery 3.4.1**
- **Bootstrap 3.4.1**
- **Newtonsoft.Json**
- **WeatherAPI.com**
- **UTF-8 Encoding** (полная поддержка русского языка)

## 📁 Структура проекта

```
WeatherApp/
├── App_Start/
│   ├── FilterConfig.cs         # Глобальные фильтры (UTF-8)
│   └── RouteConfig.cs          # Конфигурация маршрутизации
├── Content/
│   ├── css/
│   │   └── weather.css         # Стили приложения
│   └── js/
│       └── weather.js          # JavaScript функционал
├── Controllers/
│   └── WeatherController.cs    # Контроллер погоды
├── Filters/
│   └── EncodingFilter.cs       # Фильтр кодировки UTF-8
├── Helper/
│   └── MapperService.cs       # Маппинг моделей
├── Models/
│   └── WeatherModels.cs        # Модели данных
├── Services/
│   └── WeatherHttpCLient.cs    # Синглетон HttpClient
│   └── WeatherService.cs       # Сервис для работы с API
├── Views/
│   ├── Shared/
│   │   └── _Layout.cshtml      # Основной layout
│   ├── Weather/
│   │   └── Index.cshtml        # Главное представление
│   ├── _ViewStart.cshtml       # Стартовая страница
│   └── Web.config              # Конфигурация Views
├── Properties/
│   └── AssemblyInfo.cs         # Информация о сборке
├── Global.asax                 # Глобальное приложение
├── Global.asax.cs              # Код глобального приложения
├── Web.config                  # Конфигурация приложения
├── packages.config             # NuGet пакеты
├── WeatherApp.csproj           # Файл проекта
└── README.md                   # Этот файл
```

## 🚀 Установка и запуск

### Предварительные требования

- Visual Studio 2019 или 2022
- .NET Framework 4.8
- Подключение к интернету

### Шаги установки

1. **Клонируйте или скачайте проект**
   ```bash
   git clone <repository-url>
   cd WeatherApp
   ```

2. **Откройте проект в Visual Studio**
   - Откройте файл `WeatherApp.csproj` или `WeatherApp.sln`

3. **Восстановите NuGet пакеты**
   - В Visual Studio: `Tools` → `NuGet Package Manager` → `Manage NuGet Packages for Solution`
   - Или через Package Manager Console:
   ```powershell
   Update-Package -reinstall
   ```

4. **Запустите приложение**
   - Нажмите `F5` или `Ctrl+F5`
   - Приложение откроется по адресу `http://localhost:5000/`

## 🔧 Конфигурация

### API Ключ

API ключ уже встроен в код (`WeatherService.cs`):
```csharp
private const string API_KEY = "fa8b3df74d4042b9aa7135114252304";
```

### Геолокация

Город фиксирован на Москву (координаты в `WeatherController.cs`):
```csharp
private const double MOSCOW_LAT = 55.7558;
private const double MOSCOW_LON = 37.6173;
```

### Кодировка UTF-8

Приложение настроено для корректного отображения русского текста:
- `Web.config` - настройки globalization с UTF-8
- `Global.asax.cs` - установка кодировки для запросов и ответов
- `EncodingFilter.cs` - фильтр для принудительной установки UTF-8
- Все представления используют `<meta charset="utf-8" />`

## 📱 API Endpoints

Приложение использует следующие endpoints WeatherAPI:

- **Текущая погода**:
  ```
  http://api.weatherapi.com/v1/current.json?key=API_KEY&q=LAT,LON&lang=ru
  ```

- **Прогноз**:
  ```
  http://api.weatherapi.com/v1/forecast.json?key=API_KEY&q=LAT,LON&days=3&lang=ru
  ```

## 🎨 Дизайн

Приложение использует **Bootstrap 3.4.1** для UI с кастомными стилями:
- Градиентный анимированный фон
- Bootstrap Panels для карточек
- Bootstrap Tables для табличных данных
- Bootstrap Grid System для адаптивной верстки
- Bootstrap Glyphicons для иконок
- Плавные анимации переходов
- Адаптивный дизайн для всех устройств

## 🔄 Обновление данных

- **Ручное обновление**: Кнопка "Обновить" в шапке
- **Автообновление**: Каждые 10 минут
- **Горячая клавиша**: `Ctrl+R`

## ⚠️ Обработка ошибок

Приложение обрабатывает следующие ошибки:
- Отсутствие интернет-соединения
- Таймаут запроса
- Ошибки сервера API
- Некорректные данные

## 📝 Лицензия

Этот проект создан для демонстрационных целей.

## 🙏 Благодарности

- [WeatherAPI.com](https://www.weatherapi.com/) - за предоставление API
- [Google Fonts](https://fonts.google.com/) - за шрифт Inter
