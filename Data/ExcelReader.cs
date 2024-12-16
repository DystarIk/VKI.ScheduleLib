using ClosedXML.Excel;
using VKI.ScheduleLib.Models;

namespace VKI.ScheduleLib.Data;

/// <summary>
/// Класс для чтения данных из Excel-файлов и загрузки их в базу данных.
/// </summary>
public class ExcelReader
{
    private readonly List<string> _filePaths;
    private readonly AppDbContext _appDb;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="ExcelReader"/> с указанным путем к папке с файлами.
    /// </summary>
    /// <param name="folderPath">Путь к папке, содержащей только Excel-файлы.</param>
    public ExcelReader(string folderPath)
    {
        _appDb = new AppDbContext();
        _appDb.Database.EnsureDeleted();
        _appDb.Database.EnsureCreated();

        _filePaths = Directory.GetFiles(folderPath).ToList();
        if (_filePaths.Count == 0)
            throw new Exception($"В каталоге нету файлов");

        foreach (string filePath in _filePaths)
        {
            if (!File.Exists(filePath))
                throw new Exception($"Файл {filePath} не найден");
            if (Path.GetExtension(filePath) != ".xlsx")
                throw new Exception($"Файл {filePath} не формата xlsx");
        }
    }

    /// <summary>
    /// Читает данные из Excel-файлов и загружает их в базу данных.
    /// </summary>
    public void ReadFromExcel()
    {
        foreach (string filePath in _filePaths)
        {
            using (XLWorkbook workbook = new(filePath))
            {
                var worksheet = workbook.Worksheet(1);

                if (worksheet.Rows().Count() != 65)
                    throw new Exception($"{filePath} Число строк должно быть 65.");
                if (worksheet.Columns().Count() < 3)
                    throw new Exception($"{filePath} Число столбцов должно быть больше 2.");

                string courseName = Path.GetFileNameWithoutExtension(filePath);
                LoadCourse(worksheet, courseName);
            }
        }
        _appDb.SaveChanges();
    }

    /// <summary>
    /// Загружает данные о курсе из Excel-файла.
    /// </summary>
    /// <param name="worksheet">Лист Excel, содержащий данные о курсе.</param>
    /// <param name="courseName">Название курса.</param>
    private void LoadCourse(IXLWorksheet worksheet, string courseName)
    {
        Course course = new Course(courseName);
        for (int col = 3; col <= worksheet.Columns().Count(); col += 2)
        {
            var cell = worksheet.Cell(3, col);
            Group group = new(cell.GetText());
            course.Groups.Add(group);

            _appDb.Groups.Add(group);
            _appDb.Schedules.Add(group.Schedule);
        }
        _appDb.Courses.Add(course);
        LoadSchedules(worksheet, course);
    }

    /// <summary>
    /// Загружает данные о расписании из Excel-файла.
    /// </summary>
    /// <param name="worksheet">Лист Excel, содержащий данные о расписании.</param>
    /// <param name="course">Курс, для которого загружается расписание.</param>
    private void LoadSchedules(IXLWorksheet worksheet, Course course)
    {
        for (int row = 6; row <= worksheet.Rows().Count(); row += 2)
        {
            for (int col = 3; col <= worksheet.Columns().Count();)
            {
                var cell = worksheet.Cell(row, col);
                var range = cell.MergedRange();
                List<int> ids = GetSchedulesId(col, range);
                ids.Reverse();
                col += range.ColumnCount();

                string subjectName = cell.Value.ToString();
                string lessonNumber = worksheet.Cell(row, 2).GetValue<string>();
                Subject subject = LoadSubject(range, subjectName, lessonNumber, row);

                foreach (int id in ids)
                {
                    course.Groups[id].Schedule.Subjects.Add(subject);
                    subject.Schedules.Add(course.Groups[id].Schedule);
                }
            }
        }

        List<int> GetSchedulesId(int col, IXLRange range)
        {
            int colRange = range.ColumnCount();
            List<int> ids = new();
            while (colRange >= 2)
            {
                ids.Add(((col + colRange - 3) / 2) - 1);
                colRange -= 2;
            }
            return ids;
        }
    }

    /// <summary>
    /// Загружает данные о предмете из Excel-файла.
    /// </summary>
    /// <param name="range">Диапазон ячеек, содержащих данные о предмете.</param>
    /// <param name="subjectName">Название предмета.</param>
    /// <param name="lessonNumber">Номер пары.</param>
    /// <param name="row">Номер строки в Excel-файле.</param>
    /// <returns>Объект <see cref="Subject"/>, содержащий данные о предмете.</returns>
    private Subject LoadSubject(IXLRange range, string subjectName, string lessonNumber, int row)
    {
        Subject subject = new(subjectName);
        if (subjectName != "")
        {
            subject.TeacherName = range.FirstCell().CellBelow().Value.ToString();
            subject.RoomNumber = range.LastCell().CellBelow().Value.ToString();
        }
        subject.LessonNumber = lessonNumber;
        subject.DayOfWeek = (DayOfWeek)(((row - 6) / 10) + 1);
        _appDb.Subjects.Add(subject);
        return subject;
    }
}