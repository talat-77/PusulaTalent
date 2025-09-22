using AutoMapper;
using SchoolManangement.Business.CQRS.Absenteeisms.Commands.CreateAbsenteeism;
using SchoolManangement.Business.CQRS.Courses.Commands.AssignCourseToClass;
using SchoolManangement.Business.CQRS.Courses.Commands.CreateCourse;
using SchoolManangement.Business.CQRS.Note.Commands.CreateNote;
using SchoolManangement.Business.CQRS.SchoolClasses.Commands.CreateSchoolClass;
using SchoolManangement.Business.CQRS.Students.Commands.AssignStudentToClass;
using SchoolManangement.Business.CQRS.Students.Commands.CreateStudent;
using SchoolManangement.Business.CQRS.Teachers.Commands.CreateTeacher;
using SchoolManangement.Business.Dto;
using SchoolManangement.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManangement.Business.MappingProfile
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<CreateAbsenteeismCommand,Absenteeism>().ReverseMap();
            CreateMap<Absenteeism, AbsenteeismDto>().ReverseMap();

            CreateMap<CreateCourseCommand, Course>().ReverseMap();
            CreateMap<CourseDto, Course>().ReverseMap();
            CreateMap<AssignCourseToClassCommand, CourseAssignment>().ReverseMap();

            CreateMap<Note, NoteDto>().ReverseMap();
            CreateMap<CreateNoteCommand, Note>().ReverseMap();

            CreateMap<CreateSchoolClassCommand, SchoolClass>().ReverseMap();
            CreateMap<SchoolClassDto, SchoolClass>().ReverseMap();

            CreateMap<Student, CreateStudentCommand>().ReverseMap();
            CreateMap<StudentDto, Student>().ReverseMap();
            CreateMap<StudentAssignment, AssignStudentToClassCommand>().ReverseMap();

            CreateMap<Teacher, TeacherDto>().ReverseMap();
            CreateMap<CreateTeacherCommand, Teacher>().ReverseMap();

        }

       
    }
}
