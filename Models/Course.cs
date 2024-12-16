namespace VKI.ScheduleLib.Models;

/// <summary>
/// Представляет курс в колледже.
/// </summary>
public class Course
{
    /// <summary>
    /// Уникальный идентификатор курса.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Название курса.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Список групп, связанных с курсом.
    /// </summary>
    public List<Group> Groups { get; set; }

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="Course"/> с заданным названием.
    /// </summary>
    /// <param name="name">Название курса.</param>
    public Course(string name) : this() => Name = name;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="Course"/>.
    /// </summary>
    public Course()
    {
        Name = string.Empty;
        Groups = new();
    }

    /// <summary>
    /// Возвращает строковое представление курса.
    /// </summary>
    /// <returns>Строка, содержащая название курса и количество групп.</returns>
    public override string? ToString()
    {
        return $"{Name}: количество групп: {Groups.Count}";
    }
}