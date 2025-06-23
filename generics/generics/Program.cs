using System;
using System.Collections.Generic;
using generics.Interfaces;

class Person { public int Id; public string Name; }

class Student : Person
{
    public void SubmitWork() => Console.WriteLine($"{Name} submitted work.");
    public void SayName() => Console.WriteLine($"I'm {Name}.");
}

class Teacher : Person
{
    public void GradeStudent(Student s) => Console.WriteLine($"{Name} graded {s.Name}.");
    public void ExpelStudent(Student s) => Console.WriteLine($"{s.Name} expelled by {Name}.");
    public void ShowPresentStudents() => Console.WriteLine($"{Name} shows present students.");
}

class InMemoryRepository<TEntity, TKey> :
    IRepository<TEntity, TKey>,
    IReadOnlyRepository<TEntity, TKey>,
    IWriteRepository<TEntity, TKey>
    where TEntity : class, new()
    where TKey : struct
{
    private Dictionary<TKey, TEntity> _storage = new();

    public void Add(TKey id, TEntity entity) => _storage[id] = entity;
    public TEntity Get(TKey id) => _storage[id];
    public IEnumerable<TEntity> GetAll() => _storage.Values;
    public void Remove(TKey id) => _storage.Remove(id);

    void IWriteRepository<TEntity, TKey>.Add(TEntity entity) => throw new NotImplementedException();
}

class Group
{
    public int Id;
    public string Name;
    private IRepository<Student, int> _students = new InMemoryRepository<Student, int>();

    public void AddStudent(Student s) => _students.Add(s.Id, s);
    public void RemoveStudent(int id) => _students.Remove(id);
    public IEnumerable<Student> GetAllStudents() => _students.GetAll();
    public Student FindStudent(int id) => _students.Get(id);
}

class Faculty
{
    public int Id;
    public string Name;
    private IRepository<Group, int> _groups = new InMemoryRepository<Group, int>();

    public void AddGroup(Group g) => _groups.Add(g.Id, g);
    public void RemoveGroup(int id) => _groups.Remove(id);
    public IEnumerable<Group> GetAllGroups() => _groups.GetAll();
    public Group GetGroup(int id) => _groups.Get(id);

    public void AddStudentToGroup(int groupId, Student s) => _groups.Get(groupId).AddStudent(s);
    public void RemoveStudentFromGroup(int groupId, int studentId) => _groups.Get(groupId).RemoveStudent(studentId);
    public IEnumerable<Student> GetAllStudentsInGroup(int groupId) => _groups.Get(groupId).GetAllStudents();
    public Student FindStudentInGroup(int groupId, int studentId) => _groups.Get(groupId).FindStudent(studentId);
}

class Program
{
    static void Main()
    {
        var fpm = new Faculty { Id = 1, Name = "ФПМ" };
        fpm.AddGroup(new Group { Id = 41, Name = "КП-41" });
        fpm.AddGroup(new Group { Id = 42, Name = "КП-42" });

        var s1 = new Student { Id = 1, Name = "Андрій" };
        var s2 = new Student { Id = 2, Name = "Марія" };

        fpm.AddStudentToGroup(41, s1);
        fpm.AddStudentToGroup(41, s2);

        Console.WriteLine("Студенти в КП-41:");
        foreach (var s in fpm.GetAllStudentsInGroup(41))
            Console.WriteLine($"- {s.Name}");
    }
}
