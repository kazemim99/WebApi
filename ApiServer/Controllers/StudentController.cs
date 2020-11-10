using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiServer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiServer.Controllers
{
    //(Get)http://www.divar.com/api/student
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private SchoolContext _schoolContext;

        public StudentController()
        {
            _schoolContext = new SchoolContext();
        }

        [HttpGet]
        public List<Student> Index()
        {
            var students = _schoolContext.Students.ToList();
            return students;
        }

        [HttpGet("{id}")]
        public Student GetById([FromRoute] int id)
        {
            var student = _schoolContext.Students.First(a => a.Id == id);
            return student;
        }

        [HttpPost]
        public void Post([FromBody] Student student)
        {
            _schoolContext.Students.Add(student);
            _schoolContext.SaveChanges();
        }

        [HttpPut("{id}")]
        public void Put([FromRoute] int id, [FromBody] Student student)
        {
            var dbStudent = _schoolContext.Students.First(a => a.Id == id);
            dbStudent.Name = student.Name;

            //_schoolContext.Entry(dbStudent).State = EntityState.Modified;
            _schoolContext.Update(dbStudent);
            _schoolContext.SaveChanges();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var student = _schoolContext.Students.FirstOrDefault(a => a.Id == id);
            if (student == null)
                return NotFound($"student {id} not found");

            _schoolContext.Students.Remove(student);
            _schoolContext.SaveChanges();
            return Ok();
        }

        [HttpGet("search")]
        public List<Student> Search([FromQuery] string phrase)
        {
            var students = _schoolContext.Students.Where(a => a.Name.Contains(phrase));

            return students.ToList();
        }
    }
}