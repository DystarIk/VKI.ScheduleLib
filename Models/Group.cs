namespace VKI.ScheduleLib.Models;

/// <summary>
/// Представляет группу в колледже.
/// </summary>
public class Group
{
    /// <summary>
    /// Уникальный идентификатор группы.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Название группы.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Расписание группы.
    /// </summary>
    public Schedule Schedule { get; set; }

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="Group"/> с заданным названием.
    /// </summary>
    /// <param name="name">Название группы.</param>
    public Group(string name) : this() => Name = name;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="Group"/>.
    /// </summary>
    public Group()
    {
        Name = string.Empty;
        Schedule = new();
    }

    /// <summary>
    /// Возвращает строковое представление группы.
    /// </summary>
    /// <returns>Строка, содержащая название группы.</returns>
    public override string? ToString()
    {
        return $"Группа {Name}";
    }
}