using System;
using System.Collections.Generic;
using System.Linq;

namespace UniversityManagementSystem
{
    // абстрактный базовый класс для людей (абстракция и наследование)
    public abstract class Person
    {
        private static int _nextId = 1; // статический счетчик для ID
        private int _id;
        private string _firstName;
        private string _lastName;
        private string _middleName;
        private int _age;

        public Person(string firstName, string lastName, int age, string middleName = null)
        {
            _id = _nextId++;
            _firstName = firstName;
            _lastName = lastName;
            _age = age;
            _middleName = middleName;
        }

        // инкапсуляция: приватные поля, публичные свойства
        public int Id => _id;
        public string FirstName
        {
            get => _firstName;
            set => _firstName = value;
        }
        public string LastName
        {
            get => _lastName;
            set => _lastName = value;
        }
        public string MiddleName
        {
            get => _middleName;
            set => _middleName = value;
        }
        public int Age
        {
            get => _age;
            set => _age = value;
        }

        // Свойство для полного имени
        public string FullName
        {
            get
            {
                if (string.IsNullOrEmpty(_middleName))
                {
                    return $"{_lastName} {_firstName}";
                }
                else
                {
                    return $"{_lastName} {_firstName} {_middleName}";
                }
            }
        }

        // полиморфизм, абстрактный метод для отображения информации
        public abstract void DisplayInfo();
    }

    // класс студент (наследование)
    public class Student : Person
    {
        private List<Course> _enrolledCourses;

        public Student(string firstName, string lastName, int age, string middleName = null) : base(firstName, lastName, age, middleName)
        {
            _enrolledCourses = new List<Course>();
        }

        public List<Course> EnrolledCourses => _enrolledCourses;

        public void EnrollInCourse(Course course)
        {
            if (!_enrolledCourses.Contains(course))
            {
                _enrolledCourses.Add(course);
                course.AddStudent(this);
            }
        }

        // полиморфизм
        public override void DisplayInfo()
        {
            Console.WriteLine($"ID: {Id}, Имя: {FullName}, Возраст: {Age}, Тип: Студент");
        }
    }

    // класс преподаватель (наследование)
    public class Teacher : Person
    {
        private List<Course> _taughtCourses;

        public Teacher(string firstName, string lastName, int age, string middleName = null) : base(firstName, lastName, age, middleName)
        {
            _taughtCourses = new List<Course>();
        }

        public List<Course> TaughtCourses => _taughtCourses;

        public void AssignToCourse(Course course)
        {
            if (!_taughtCourses.Contains(course))
            {
                _taughtCourses.Add(course);
                course.Teacher = this;
            }
        }

        // полиморфизм
        public override void DisplayInfo()
        {
            Console.WriteLine($"ID: {Id}, Имя: {FullName}, Возраст: {Age}, Тип: Преподаватель");
        }
    }

    // Класс Курс
    public class Course
    {
        private static int _nextId = 1;
        private int _id;
        private string _name;
        private string _description;
        private Teacher _teacher;
        private List<Student> _students;

        public Course(string name, string description)
        {
            _id = _nextId++;
            _name = name;
            _description = description;
            _students = new List<Student>();
        }

        public int Id => _id;
        public string Name
        {
            get => _name;
            set => _name = value;
        }
        public string Description
        {
            get => _description;
            set => _description = value;
        }
        public Teacher Teacher
        {
            get => _teacher;
            set => _teacher = value;
        }
        public List<Student> Students => _students;

        public void AddStudent(Student student)
        {
            if (!_students.Contains(student))
            {
                _students.Add(student);
            }
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"ID: {Id}, Название: {Name}, Описание: {Description}, Преподаватель: {(Teacher != null ? Teacher.FullName : "Не назначен")}");
        }
    }

    // класс для управления университетом
    public class UniversityManager
    {
        private List<Student> _students;
        private List<Teacher> _teachers;
        private List<Course> _courses;

        public UniversityManager()
        {
            _students = new List<Student>();
            _teachers = new List<Teacher>();
            _courses = new List<Course>();
        }

        public void AddStudent(Student student)
        {
            _students.Add(student);
        }

        public void AddTeacher(Teacher teacher)
        {
            _teachers.Add(teacher);
        }

        public void AddCourse(Course course)
        {
            _courses.Add(course);
        }

        public Student GetStudentById(int id)
        {
            return _students.FirstOrDefault(s => s.Id == id);
        }

        public Teacher GetTeacherById(int id)
        {
            return _teachers.FirstOrDefault(t => t.Id == id);
        }

        public Course GetCourseById(int id)
        {
            return _courses.FirstOrDefault(c => c.Id == id);
        }

        public List<Student> GetAllStudents() => _students;
        public List<Teacher> GetAllTeachers() => _teachers;
        public List<Course> GetAllCourses() => _courses;
    }

    class Program
    {
        static void Main(string[] args)
        {
            UniversityManager manager = new UniversityManager();
            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine("=== Система КИП ФИН'ом ===");
                Console.WriteLine("1. Добавить студента");
                Console.WriteLine("2. Просмотреть информацию о студенте");
                Console.WriteLine("3. Записать студента на курс");
                Console.WriteLine("4. Просмотреть курсы студента");
                Console.WriteLine("5. Добавить преподавателя");
                Console.WriteLine("6. Просмотреть информацию о преподавателе");
                Console.WriteLine("7. Назначить преподавателя на курс");
                Console.WriteLine("8. Создать курс");
                Console.WriteLine("9. Просмотреть информацию о курсе");
                Console.WriteLine("10. Просмотреть студентов на курсе");
                Console.WriteLine("11. Вывести всех студентов");
                Console.WriteLine("12. Вывести всех преподавателей");
                Console.WriteLine("13. Вывести все курсы");
                Console.WriteLine("14. Выход");
                Console.Write("Выберите опцию: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddStudent(manager);
                        break;
                    case "2":
                        ViewStudent(manager);
                        break;
                    case "3":
                        EnrollStudentInCourse(manager);
                        break;
                    case "4":
                        ViewStudentCourses(manager);
                        break;
                    case "5":
                        AddTeacher(manager);
                        break;
                    case "6":
                        ViewTeacher(manager);
                        break;
                    case "7":
                        AssignTeacherToCourse(manager);
                        break;
                    case "8":
                        CreateCourse(manager);
                        break;
                    case "9":
                        ViewCourse(manager);
                        break;
                    case "10":
                        ViewCourseStudents(manager);
                        break;
                    case "11":
                        ListAllStudents(manager);
                        break;
                    case "12":
                        ListAllTeachers(manager);
                        break;
                    case "13":
                        ListAllCourses(manager);
                        break;
                    case "14":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор. Нажмите любую клавишу для продолжения...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void AddStudent(UniversityManager manager)
        {
            Console.Write("Введите фамилию: ");
            string lastName = Console.ReadLine();
            Console.Write("Введите имя: ");
            string firstName = Console.ReadLine();
            Console.Write("Введите отчество (если имеется, иначе нажмите Enter): ");
            string middleName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(middleName))
            {
                middleName = null;
            }
            Console.Write("Введите возраст: ");
            int age = int.Parse(Console.ReadLine());

            Student student = new Student(firstName, lastName, age, middleName);
            manager.AddStudent(student);
            Console.WriteLine("Студент добавлен! Нажмите любую клавишу...");
            Console.ReadKey();
        }

        static void ViewStudent(UniversityManager manager)
        {
            Console.Write("Введите ID студента: ");
            int id = int.Parse(Console.ReadLine());
            Student student = manager.GetStudentById(id);
            if (student != null)
            {
                student.DisplayInfo();
            }
            else
            {
                Console.WriteLine("Студент не найден.");
            }
            Console.ReadKey();
        }

        static void EnrollStudentInCourse(UniversityManager manager)
        {
            Console.Write("Введите ID студента: ");
            int studentId = int.Parse(Console.ReadLine());
            Console.Write("Введите ID курса: ");
            int courseId = int.Parse(Console.ReadLine());

            Student student = manager.GetStudentById(studentId);
            Course course = manager.GetCourseById(courseId);
            if (student != null && course != null)
            {
                student.EnrollInCourse(course);
                Console.WriteLine("Студент записан на курс!");
            }
            else
            {
                Console.WriteLine("Студент или курс не найдены.");
            }
            Console.ReadKey();
        }

        static void ViewStudentCourses(UniversityManager manager)
        {
            Console.Write("Введите ID студента: ");
            int id = int.Parse(Console.ReadLine());
            Student student = manager.GetStudentById(id);
            if (student != null)
            {
                Console.WriteLine($"Курсы студента {student.FullName}:");
                foreach (var course in student.EnrolledCourses)
                {
                    Console.WriteLine($"- {course.Name}");
                }
            }
            else
            {
                Console.WriteLine("Студент не найден.");
            }
            Console.ReadKey();
        }

        static void AddTeacher(UniversityManager manager)
        {
            Console.Write("Введите фамилию: ");
            string lastName = Console.ReadLine();
            Console.Write("Введите имя: ");
            string firstName = Console.ReadLine();
            Console.Write("Введите отчество (если имеется, иначе нажмите Enter): ");
            string middleName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(middleName))
            {
                middleName = null;
            }
            Console.Write("Введите возраст: ");
            int age = int.Parse(Console.ReadLine());

            Teacher teacher = new Teacher(firstName, lastName, age, middleName);
            manager.AddTeacher(teacher);
            Console.WriteLine("Преподаватель добавлен! Нажмите любую клавишу...");
            Console.ReadKey();
        }

        static void ViewTeacher(UniversityManager manager)
        {
            Console.Write("Введите ID преподавателя: ");
            int id = int.Parse(Console.ReadLine());
            Teacher teacher = manager.GetTeacherById(id);
            if (teacher != null)
            {
                teacher.DisplayInfo();
            }
            else
            {
                Console.WriteLine("Преподаватель не найден.");
            }
            Console.ReadKey();
        }

        static void AssignTeacherToCourse(UniversityManager manager)
        {
            Console.Write("Введите ID преподавателя: ");
            int teacherId = int.Parse(Console.ReadLine());
            Console.Write("Введите ID курса: ");
            int courseId = int.Parse(Console.ReadLine());

            Teacher teacher = manager.GetTeacherById(teacherId);
            Course course = manager.GetCourseById(courseId);
            if (teacher != null && course != null)
            {
                teacher.AssignToCourse(course);
                Console.WriteLine("Преподаватель назначен на курс!");
            }
            else
            {
                Console.WriteLine("Преподаватель или курс не найдены.");
            }
            Console.ReadKey();
        }

        static void CreateCourse(UniversityManager manager)
        {
            Console.Write("Введите название курса: ");
            string name = Console.ReadLine();
            Console.Write("Введите описание курса: ");
            string description = Console.ReadLine();

            Course course = new Course(name, description);
            manager.AddCourse(course);
            Console.WriteLine("Курс создан! Нажмите любую клавишу...");
            Console.ReadKey();
        }

        static void ViewCourse(UniversityManager manager)
        {
            Console.Write("Введите ID курса: ");
            int id = int.Parse(Console.ReadLine());
            Course course = manager.GetCourseById(id);
            if (course != null)
            {
                course.DisplayInfo();
            }
            else
            {
                Console.WriteLine("Курс не найден.");
            }
            Console.ReadKey();
        }

        static void ViewCourseStudents(UniversityManager manager)
        {
            Console.Write("Введите ID курса: ");
            int id = int.Parse(Console.ReadLine());
            Course course = manager.GetCourseById(id);
            if (course != null)
            {
                Console.WriteLine($"Студенты на курсе {course.Name}:");
                foreach (var student in course.Students)
                {
                    Console.WriteLine($"- {student.FullName}");
                }
            }
            else
            {
                Console.WriteLine("Курс не найден.");
            }
            Console.ReadKey();
        }

        static void ListAllStudents(UniversityManager manager)
        {
            Console.WriteLine("Все студенты:");
            foreach (var student in manager.GetAllStudents())
            {
                student.DisplayInfo();
            }
            Console.ReadKey();
        }

        static void ListAllTeachers(UniversityManager manager)
        {
            Console.WriteLine("Все преподаватели:");
            foreach (var teacher in manager.GetAllTeachers())
            {
                teacher.DisplayInfo();
            }
            Console.ReadKey();
        }

        static void ListAllCourses(UniversityManager manager)
        {
            Console.WriteLine("Все курсы:");
            foreach (var course in manager.GetAllCourses())
            {
                course.DisplayInfo();
            }
            Console.ReadKey();
        }
    }
}
