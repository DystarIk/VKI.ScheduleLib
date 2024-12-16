namespace VKI.ScheduleLib.Models;

/// <summary>
/// Представляет расписание группы.
/// </summary>
public class Schedule
{
    /// <summary>
    /// Уникальный идентификатор расписания.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Дата начала расписания.
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Список предметов, связанных с расписанием.
    /// </summary>
    public List<Subject> Subjects { get; set; }

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="Schedule"/> с заданным идентификатором.
    /// </summary>
    /// <param name="id">Идентификатор расписания.</param>
    public Schedule(int id) : this() => Id = id;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="Schedule"/>.
    /// </summary>
    public Schedule() => Subjects = new();

    /// <summary>
    /// Возвращает строковое представление расписания.
    /// </summary>
    /// <returns>Строка, содержащая идентификатор расписания.</returns>
    public override string? ToString()
    {
        return $"Расписание {Id}";
    }
}