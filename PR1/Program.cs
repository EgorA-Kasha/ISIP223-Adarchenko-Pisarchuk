using System;
using System.Collections.Generic;
using System.Linq;

namespace UniversityManagementSystem
{
    // абстрактный класс для человека
    public abstract class Person
    {
        private static int nextId = 1;
        private int id;
        private string fName;
        private string lName;
        private string mName;
        private int age;

        public int Id => id;

        public string FirstName
        {
            get => fName;
            set => fName = value;
        }

        public string LastName
        {
            get => lName;
            set => lName = value;
        }

        public string MiddleName
        {
            get => mName;
            set => mName = value;
        }

        public int Age
        {
            get => age;
            set => age = value;
        }

        public string FullName => string.IsNullOrEmpty(mName) ? $"{lName} {fName}" : $"{lName} {fName} {mName}";

        public Person(string fName, string lName, int age, string mName = null)
        {
            if (string.IsNullOrWhiteSpace(fName))
            {
                throw new ArgumentException("Имя не может быть пустым.");
            }
            if (string.IsNullOrWhiteSpace(lName))
            {
                throw new ArgumentException("Фамилия не может быть пустой.");
            }
            if (age < 18 || age > 100)
            {
                throw new ArgumentException("Возраст должен быть от 18 до 100 лет.");
            }
            id = nextId++;
            this.fName = fName;
            this.lName = lName;
            this.age = age;
            this.mName = mName;
        }

        // абстрактный метод для отображения информации
        public abstract void DisplayInfo();
    }

    public class Course
    {
        private string name;
        private Teacher teacher;
        private List<Student> students;

        public string Name
        {
            get => name;
            set => name = value;
        }

        public Teacher Teacher
        {
            get => teacher;
            set => teacher = value;
        }

        public List<Student> Students => students;

        public Course(string name)
        {
            this.name = name;
            students = new List<Student>();
        }

        // добавления студентов на курс (не более 20 студентов)
        public bool AddStudent(Student student)
        {
            if (students.Count < 20 && !students.Contains(student))
            {
                students.Add(student);
                return true;
            }
            return false;
        }

        // назначение преподавателя на курс (не более 1 преподавателя)
        public bool AssignTeacher(Teacher teacher)
        {
            if (this.teacher == null)
            {
                this.teacher = teacher;
                return true;
            }
            return false;
        }

        //отображение информации о курсе
        public void DisplayInfo()
        {
            Console.WriteLine($"Курс: {name}");
            Console.WriteLine($"Преподаватель: {(teacher != null ? teacher.FullName : "Не назначен")}");
            Console.WriteLine($"Количество студентов: {students.Count}/20");
            if (students.Count > 0)
            {
                Console.WriteLine("Студенты:");
                foreach (var student in students)
                {
                    Console.WriteLine($"- {student.FullName}");
                }
            }
            else
            {
                Console.WriteLine("Студенты: Нет записанных студентов\n");
            }
        }
    }

    public class Student : Person
    {
        private List<Course> courses;
        private Dictionary<Course, List<int>> grades;

        public List<Course> Courses => courses;

        public Dictionary<Course, List<int>> Grades => grades;

        public double AverageGrade
        {
            get
            {
                if (grades.Values.Sum(list => list.Count) == 0) return 0;
                // 21 считается как 1 при расчёте среднего
                return grades.Values.SelectMany(list => list.Select(g => g == 21 ? 1 : g)).Average();
            }
        }

        public Student(string fName, string lName, int age, string mName = null)
            : base(fName, lName, age, mName)
        {
            courses = new List<Course>();
            grades = new Dictionary<Course, List<int>>();
        }

        // запись студента на курс
        public bool Enroll(Course course)
        {
            if (!courses.Contains(course) && course.AddStudent(this))
            {
                courses.Add(course);
                grades[course] = new List<int>();
                return true;
            }
            return false;
        }

        // добавление оценки по курсу
        public void AddGrade(Course course, int grade)
        {
            if (grades.ContainsKey(course))
            {
                grades[course].Add(grade);
            }
        }

        // переопределенный метод для студентов
        public override void DisplayInfo()
        {
            Console.WriteLine($"ID: {Id}, Имя: {FullName}, Возраст: {Age}, Тип: Студент, Средний балл: {AverageGrade:F2}");
            if (grades.Any())
            {
                Console.WriteLine("Оценки по курсам:");
                foreach (var kvp in grades)
                {
                    string gradesStr = kvp.Value.Any() ? string.Join(", ", kvp.Value) : "Нет оценок";
                    Console.WriteLine($"- {kvp.Key.Name}: {gradesStr}");
                }
            }
            Console.WriteLine();
        }
    }

    public class Teacher : Person
    {
        private List<Course> courses;

        public List<Course> Courses => courses;

        public Teacher(string fName, string lName, int age, string mName = null)
            : base(fName, lName, age, mName)
        {
            courses = new List<Course>();
        }

        public bool Assign(Course course)
        {
            if (!courses.Contains(course) && course.AssignTeacher(this))
            {
                courses.Add(course);
                return true;
            }
            return false;
        }

        // переопределенный метод для преподавателей
        public override void DisplayInfo()
        {
            Console.WriteLine($"ID: {Id}, Имя: {FullName}, Возраст: {Age}, Тип: Преподаватель, Количество курсов: {courses.Count}");
            Console.WriteLine();
        }
    }

    public class University
    {
        private List<Student> students;
        private List<Teacher> teachers;
        private List<Course> courses;

        public List<Student> Students => students;

        public List<Teacher> Teachers => teachers;

        public List<Course> Courses => courses;

        public University()
        {
            students = new List<Student>();
            teachers = new List<Teacher>();
            courses = new List<Course>();
        }

        public void AddStudent(Student student)
        {
            students.Add(student);
        }

        public void AddTeacher(Teacher teacher)
        {
            teachers.Add(teacher);
        }

        public void AddCourse(Course course)
        {
            courses.Add(course);
        }

        public void DisplayAllStudents()
        {
            Console.WriteLine("Студенты:");
            foreach (var student in students)
            {
                student.DisplayInfo();
            }
        }

        public void DisplayAllTeachers()
        {
            Console.WriteLine("Список преподавателей:");
            foreach (var teacher in Teachers)
            {
                string coursesStr = teacher.Courses.Any()
                    ? string.Join(", ", teacher.Courses.Select(c => c.Name))
                    : "Нет";
                Console.WriteLine($"Преподаватель: {teacher.FullName}, Возраст: {teacher.Age}, Отчество: {teacher.MiddleName ?? "Не указано"}, Назначенные курсы: {coursesStr}");
            }
        }

        public void DisplayAllCourses()
        {
            Console.WriteLine("Курсы:");
            foreach (var course in courses)
            {
                course.DisplayInfo();
                Console.WriteLine();
            }
        }
    }

    class Program
    {
        private static University university = new University();

        // добавление студента
        static void AddStudent()
        {
            Console.Write("Введите фамилию студента: ");
            string lNameS = Console.ReadLine();
            Console.Write("Введите имя студента: ");
            string fNameS = Console.ReadLine();
            Console.Write("Введите отчество студента (или оставьте пустым): ");
            string mNameS = Console.ReadLine();
            mNameS = string.IsNullOrEmpty(mNameS) ? null : mNameS;
            Console.Write("Введите возраст студента: ");
            if (!int.TryParse(Console.ReadLine(), out int ageS))
            {
                Console.WriteLine("Некорректный возраст. Возраст должен быть числом.\n");
                return;
            }
            try
            {
                Student student = new Student(fNameS, lNameS, ageS, mNameS);
                university.AddStudent(student);
                Console.WriteLine("Студент добавлен!\n");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}\n");
            }
        }

        static void AddTeacher()
        {
            Console.Write("Введите фамилию преподавателя: ");
            string lNameT = Console.ReadLine();
            Console.Write("Введите имя преподавателя: ");
            string fNameT = Console.ReadLine();
            Console.Write("Введите отчество преподавателя (или оставьте пустым): ");
            string mNameT = Console.ReadLine();
            mNameT = string.IsNullOrEmpty(mNameT) ? null : mNameT;
            Console.Write("Введите возраст преподавателя: ");
            if (!int.TryParse(Console.ReadLine(), out int ageT))
            {
                Console.WriteLine("Некорректный возраст. Возраст должен быть числом.\n");
                return;
            }
            try
            {
                Teacher teacher = new Teacher(fNameT, lNameT, ageT, mNameT);
                university.AddTeacher(teacher);
                Console.WriteLine("Преподаватель добавлен!\n");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}\n");
            }
        }

        static void AddCourse()
        {
            Console.Write("Введите название курса: ");
            string courseName = Console.ReadLine();
            Course course = new Course(courseName);
            university.AddCourse(course);
            Console.WriteLine("Курс добавлен!\n");
        }

        static void DisplayAllStudents()
        {
            university.DisplayAllStudents();
        }

        static void DisplayAllTeachers()
        {
            university.DisplayAllTeachers();
        }

        static void DisplayAllCourses()
        {
            university.DisplayAllCourses();
        }

        // запись студента на курс
        static void EnrollStudent()
        {
            Console.Write("Введите ID студента: ");
            int studentId = int.Parse(Console.ReadLine());
            Student selectedStudent = university.Students.FirstOrDefault(s => s.Id == studentId);
            if (selectedStudent == null)
            {
                Console.WriteLine("Студент с таким ID не найден.\n");
                return;
            }
            Console.Write("Введите название курса: ");
            string courseNameEnroll = Console.ReadLine();
            Course selectedCourseEnroll = university.Courses.FirstOrDefault(c => c.Name == courseNameEnroll);
            if (selectedCourseEnroll == null)
            {
                Console.WriteLine("Курс с таким названием не найден.\n");
                return;
            }
            if (selectedStudent.Enroll(selectedCourseEnroll))
            {
                Console.WriteLine("Студент записан на курс!\n");
            }
            else
            {
                Console.WriteLine("Не удалось записать студента (курс полон или студент уже записан).\n");
            }
        }

        // назначение преподавателя на курс
        static void AssignTeacher()
        {
            Console.Write("Введите ID преподавателя: ");
            int teacherId = int.Parse(Console.ReadLine());
            Teacher selectedTeacher = university.Teachers.FirstOrDefault(t => t.Id == teacherId);
            if (selectedTeacher == null)
            {
                Console.WriteLine("Преподаватель с таким ID не найден.\n");
                return;
            }
            Console.Write("Введите название курса: ");
            string courseNameAssign = Console.ReadLine();
            Course selectedCourseAssign = university.Courses.FirstOrDefault(c => c.Name == courseNameAssign);
            if (selectedCourseAssign == null)
            {
                Console.WriteLine("Курс с таким названием не найден.\n");
                return;
            }
            if (selectedTeacher.Assign(selectedCourseAssign))
            {
                Console.WriteLine("Преподаватель назначен на курс!\n");
            }
            else
            {
                Console.WriteLine("Не удалось назначить преподавателя (преподаватель уже назначен на курс или курс уже имеет преподавателя).\n");
            }
        }

        // добавление оценки студенту по курсу
        static void AddGradeToStudent()
        {
            Console.Write("Введите ID студента: ");
            int studentId = int.Parse(Console.ReadLine());
            Student selectedStudent = university.Students.FirstOrDefault(s => s.Id == studentId);
            if (selectedStudent == null)
            {
                Console.WriteLine("Студент с таким ID не найден.\n");
                return;
            }
            Console.Write("Введите название курса: ");
            string courseName = Console.ReadLine();
            Course selectedCourse = university.Courses.FirstOrDefault(c => c.Name == courseName);
            if (selectedCourse == null)
            {
                Console.WriteLine("Курс с таким названием не найден.\n");
                return;
            }
            if (!selectedStudent.Courses.Contains(selectedCourse))
            {
                Console.WriteLine("Студент не записан на этот курс.\n");
                return;
            }
            Console.Write("Введите оценку (1, 2, 3, 4, 5 или 21): ");
            if (!int.TryParse(Console.ReadLine(), out int grade))
            {
                Console.WriteLine("Некорректная оценка. Оценка должна быть числом.\n");
                return;
            }
            if (!new[] { 1, 2, 3, 4, 5, 21 }.Contains(grade))
            {
                Console.WriteLine("Недопустимая оценка. Допустимы только: 1, 2, 3, 4, 5, 21.\n");
                return;
            }
            selectedStudent.AddGrade(selectedCourse, grade);
            Console.WriteLine("Оценка добавлена!\n");
        }

        static void Main(string[] args)
        {
            bool running = true;

            while (running)
            {
                Console.WriteLine("Управление sKIP:");
                Console.WriteLine("1. Добавить студента");
                Console.WriteLine("2. Добавить преподавателя");
                Console.WriteLine("3. Добавить курс");
                Console.WriteLine("4. Отобразить всех студентов");
                Console.WriteLine("5. Отобразить всех преподавателей");
                Console.WriteLine("6. Отобразить все курсы");
                Console.WriteLine("7. Записать студента на курс");
                Console.WriteLine("8. Назначить преподавателя на курс");
                Console.WriteLine("9. Добавить оценку студенту");
                Console.WriteLine("10. Выход");
                Console.Write("Выберите опцию: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddStudent();
                        break;
                    case "2":
                        AddTeacher();
                        break;
                    case "3":
                        AddCourse();
                        break;
                    case "4":
                        DisplayAllStudents();
                        break;
                    case "5":
                        DisplayAllTeachers();
                        break;
                    case "6":
                        DisplayAllCourses();
                        break;
                    case "7":
                        EnrollStudent();
                        break;
                    case "8":
                        AssignTeacher();
                        break;
                    case "9":
                        AddGradeToStudent();
                        break;
                    case "10":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.\n");
                        break;
                }
            }
        }
    }
}
