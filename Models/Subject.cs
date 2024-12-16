namespace VKI.ScheduleLib.Models;

/// <summary>
/// Представляет предмет в расписании.
/// </summary>
public class Subject
{
    /// <summary>
    /// Уникальный идентификатор предмета.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Название предмета.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Номер аудитории, где проводится предмет.
    /// </summary>
    public string RoomNumber { get; set; }

    /// <summary>
    /// Имя преподавателя, ведущего предмет.
    /// </summary>
    public string TeacherName { get; set; }

    /// <summary>
    /// Номер пары, на которой проводится предмет.
    /// </summary>
    public string LessonNumber { get; set; }

    /// <summary>
    /// День недели, в который проводится предмет.
    /// </summary>
    public DayOfWeek DayOfWeek { get; set; }

    /// <summary>
    /// Список расписаний, в которых упоминается предмет.
    /// </summary>
    public List<Schedule> Schedules { get; set; }

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="Subject"/> с заданным названием.
    /// </summary>
    /// <param name="name">Название предмета.</param>
    public Subject(string name) : this() => Name = name;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="Subject"/>.
    /// </summary>
    public Subject()
    {
        Name = string.Empty;
        RoomNumber = string.Empty;
        TeacherName = string.Empty;
        LessonNumber = string.Empty;
        DayOfWeek = DayOfWeek.Monday;
        Schedules = new();
    }

    /// <summary>
    /// Возвращает строковое представление предмета.
    /// </summary>
    /// <returns>Строка, содержащая название предмета.</returns>
    public override string? ToString()
    {
        return Name;
    }

    /// <summary>
    /// Проверяет, совпадает ли текущий день недели с днем недели предмета.
    /// </summary>
    /// <returns>True, если текущий день недели совпадает с днем недели предмета; иначе False.</returns>
    public bool IsDayMatch()
    {
        return DateTime.Now.DayOfWeek == DayOfWeek;
    }

    /// <summary>
    /// Проверяет, проводится ли сейчас пара по предмету.
    /// </summary>
    /// <returns>True, если текущее время находится в пределах пары; иначе False.</returns>
    public bool IsTimeInLesson()
    {
        if (!IsDayMatch()) return false;

        var now = DateTime.Now;

        List<(TimeSpan Start, TimeSpan End)> lessonTimes = new()
        {
            (new TimeSpan(9, 0, 0), new TimeSpan(10, 35, 0)),   // 1 пара 9:00  - 10:35
            (new TimeSpan(10, 35, 0), new TimeSpan(12, 20, 0)), // 2 пара 10:45 - 12:20
            (new TimeSpan(12, 20, 0), new TimeSpan(14, 35, 0)), // 3 пара 13:00 - 14:35
            (new TimeSpan(14, 35, 0), new TimeSpan(16, 20, 0)), // 4 пара 14:45 - 16:20
            (new TimeSpan(16, 20, 0), new TimeSpan(18, 5, 0))   // 5 пара 16:30 - 18:05
        };

        int numLesson = Convert.ToInt32(LessonNumber) - 1;
        if (now.TimeOfDay >= lessonTimes[numLesson].Start && now.TimeOfDay <= lessonTimes[numLesson].End)
        {
            return true;
        }

        return false;
    }
}