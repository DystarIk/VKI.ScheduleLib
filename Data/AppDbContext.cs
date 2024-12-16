using Microsoft.EntityFrameworkCore;
using VKI.ScheduleLib.Models;

namespace VKI.ScheduleLib.Data;

/// <summary>
/// Контекст базы данных для работы с расписанием колледжа.
/// </summary>
public class AppDbContext : DbContext
{
    /// <summary>
    /// Набор данных для колледжей.
    /// </summary>
    public DbSet<College> Colleges { get; set; }

    /// <summary>
    /// Набор данных для курсов.
    /// </summary>
    public DbSet<Course> Courses { get; set; }

    /// <summary>
    /// Набор данных для групп.
    /// </summary>
    public DbSet<Group> Groups { get; set; }

    /// <summary>
    /// Набор данных для расписаний.
    /// </summary>
    public DbSet<Schedule> Schedules { get; set; }

    /// <summary>
    /// Набор данных для предметов.
    /// </summary>
    public DbSet<Subject> Subjects { get; set; }

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="AppDbContext"/>.
    /// </summary>
    public AppDbContext()
    {
        Colleges = Set<College>();
        Courses = Set<Course>();
        Groups = Set<Group>();
        Schedules = Set<Schedule>();
        Subjects = Set<Subject>();
    }

    /// <summary>
    /// Загружает данные о колледже из базы данных.
    /// </summary>
    /// <returns>Объект <see cref="College"/>, содержащий данные о колледже.</returns>
    public College LoadCollege()
    {
        return new(Courses.
            Include(c => c.Groups).
            ThenInclude(g => g.Schedule).
            ThenInclude(s => s.Subjects).
            ToList());
    }

    /// <summary>
    /// Настраивает параметры подключения к базе данных.
    /// </summary>
    /// <param name="optionsBuilder">Построитель параметров контекста базы данных.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source = Assets/Database/College.db");
        optionsBuilder.EnableSensitiveDataLogging();
    }
}