using VKI.ScheduleLib.Models;

namespace VKI.ScheduleLib.Services;

/// <summary>
/// Класс для поиска предметов в расписании колледжа.
/// </summary>
public class Searcher
{
    /// <summary>
    /// Колледж, в котором выполняется поиск предметов.
    /// </summary>
    public College College { get; private set; }

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="Searcher"/> с указанным колледжем.
    /// </summary>
    /// <param name="college">Колледж, в котором будет выполняться поиск.</param>
    public Searcher(College college) => College = college;

    /// <summary>
    /// Находит предметы по имени группы.
    /// </summary>
    /// <param name="groupName">Имя группы, для которой нужно найти предметы.</param>
    /// <returns>Список предметов, отсортированных по дню недели и номеру пары.</returns>
    public List<Subject> FindSubjectsByGroupName(string? groupName)
    {
        return College.Groups
            .Where(g => g.Name == groupName)
            .SelectMany(g => g.Schedule.Subjects)
            .OrderBy(s => s.DayOfWeek)
            .ThenBy(s => s.LessonNumber)
            .ToList();
    }

    /// <summary>
    /// Находит предметы по имени преподавателя.
    /// </summary>
    /// <param name="teacherName">Имя преподавателя, для которого нужно найти предметы.</param>
    /// <returns>Список предметов, отсортированных по дню недели и номеру пары.</returns>
    public List<Subject> FindSubjectsByTeacherName(string? teacherName)
    {
        return College.Subjects
            .Where(s => s.TeacherName == teacherName)
            .OrderBy(s => s.DayOfWeek)
            .ThenBy(s => s.LessonNumber)
            .ToList();
    }

    /// <summary>
    /// Находит предметы по номеру аудитории.
    /// </summary>
    /// <param name="roomNumber">Номер аудитории, для которой нужно найти предметы.</param>
    /// <returns>Список предметов, отсортированных по дню недели и номеру пары.</returns>
    public List<Subject> FindSubjectsByRoomNumber(string? roomNumber)
    {
        return College.Subjects
            .Where(s => s.RoomNumber == roomNumber)
            .OrderBy(s => s.DayOfWeek)
            .ThenBy(s => s.LessonNumber)
            .ToList();
    }

    /// <summary>
    /// Возвращает номер кабинета, в котором сейчас ведется пара для указанной группы.
    /// </summary>
    /// <param name="group">Группа, для которой нужно найти текущую пару.</param>
    /// <returns>Номер кабинета, если пара сейчас ведется; в противном случае - null.</returns>
    public string? GetCurrentRoomNumberForGroup(Group? group)
    {
        // Получаем текущее время и день недели
        DateTime now = DateTime.Now;
        DayOfWeek currentDay = now.DayOfWeek;

        // Ищем предметы для указанной группы
        var currentSubject = group?.Schedule.Subjects
            .FirstOrDefault(s => s.IsDayMatch() && s.IsTimeInLesson());

        // Возвращаем номер кабинета, если найдена текущая пара
        return currentSubject?.RoomNumber;
    }
}