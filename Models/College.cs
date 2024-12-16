namespace VKI.ScheduleLib.Models;

/// <summary>
/// Представляет колледж, включая его курсы, группы, расписания и предметы.
/// </summary>
public class College
{
    /// <summary>
    /// Уникальный идентификатор колледжа.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Название колледжа.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Список курсов, связанных с колледжем.
    /// </summary>
    public List<Course> Courses { get; set; }

    /// <summary>
    /// Список групп, связанных с колледжем.
    /// </summary>
    public List<Group> Groups { get; set; }

    /// <summary>
    /// Список расписаний, связанных с колледжем.
    /// </summary>
    public List<Schedule> Schedules { get; set; }

    /// <summary>
    /// Список предметов, связанных с колледжем.
    /// </summary>
    public List<Subject> Subjects { get; set; }

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="College"/>.
    /// </summary>
    public College()
    {
        Id = 0;
        Name = "ВКИ НГУ";
        Courses = new();
        Groups = new();
        Schedules = new();
        Subjects = new();
    }

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="College"/> с заданным списком курсов.
    /// </summary>
    /// <param name="courses">Список курсов, которые будут добавлены в колледж.</param>
    public College(List<Course> courses) : this()
    {
        foreach (var course in courses)
        {
            Courses.Add(course);
            foreach (var group in course.Groups)
            {
                Groups.Add(group);
                Schedules.Add(group.Schedule);
                foreach (var subject in group.Schedule.Subjects)
                {
                    if (!Subjects.Contains(subject))
                        Subjects.Add(subject);
                }
            }
        }
    }
}