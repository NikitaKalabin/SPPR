using WEB_253503_Kalabin.Domain.Entities;

namespace WEB_253503_Kalabin_Blazor.Wasm.Services;

public interface IBlazorClothesService
{
    // Событие, генерируемое при изменении данных
    event Action DataLoaded;

// Список категорий объектов
    List<Category> Categories { get; set; }

//Список объектов
    List<Clothes> Clothes { get; set; }

// Признак успешного ответа на запрос к Api
    bool Success { get; set; }

// Сообщение об ошибке
    string ErrorMessage { get; set; }

// Количество страниц списка
    int TotalPages { get; set; }

// Номер текущей страницы
    int CurrentPage { get; set; }

// Фильтр по категории
    Category? SelectedCategory { get; set; }

    public Task GetClothesListAsync(int pageNo = 1);

    public Task GetCategoryListAsync();
}