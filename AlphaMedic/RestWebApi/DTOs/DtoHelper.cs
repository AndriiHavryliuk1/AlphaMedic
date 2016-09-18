using RestWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RestWebApi.DTOs
{
    public static class DtoHelper
    {
        public static UserDto GetUserDto(User user) {
            return new UserDto
            {
                Id = user.UserId,
                Name = user.UserName,
                Surname = user.UserSurname
            };
        }

        public static ProcedureDto GetProcedureDto(Procedure proc) {
            return new ProcedureDto
            {
                Id = proc.Id,
                Name = proc.Name,
                DoctorFullName = proc.Doctor.User.UserName + " " + proc.Doctor.User.UserSurname,
                DoctorId = proc.DoctorId
            };
        }



        public static readonly Expression<Func<Patient, PatientDto>> AsPatientDto =
            x => new PatientDto
            {
                CurrentProcedure = x.Procedures.FirstOrDefault().Name,
                DoctorId = x.Procedures.FirstOrDefault().DoctorId,
                DoctorFullName = x.Procedures.FirstOrDefault().Doctor.User.UserName+" "+ x.Procedures.FirstOrDefault().Doctor.User.UserSurname,
                PatientId =x.PatientId,
                PatientFullName = x.User.UserName+ " "+ x.User.UserSurname
            };


        public static DoctorDetailsDto GetDoctorDetailsDto(Doctor doc)
        {
            return new DoctorDetailsDto
            {
                DoctorId = doc.DoctorId,
                DoctorFullName = doc.User.UserName + " " + doc.User.UserSurname,
                Phone = doc.User.UserPhone,
                Degree = doc.Degree,
                Education = doc.Education,
             //   DepartmentName = doc.Department.DepartmentName
            };
        }

        public static DepartmentDto GetDepartmentDto(Department dep) {
            return new DepartmentDto
            {
                DepartmentId = dep.DepartmentId.ToString(),
                DepartmentName = dep.DepartmentName,
                DepartmentDescription = dep.DepartmentDescription
            };
        }

        public static DepartmentDetailsDto GetDepartmentDetailsDto(Department dep) {
            return new DepartmentDetailsDto
            {
                DepartmentId = dep.DepartmentId,
                DepartmentName = dep.DepartmentName,
                DepartmentDescription = dep.DepartmentDescription,
                DepartmentHead = GetUserDto(dep.DepartmentHead.User),
            };
        }

        public static readonly Expression<Func<Department, DepartmentDto>> AsDepartmentDto =
           x => new DepartmentDto
           {
               DepartmentId = x.DepartmentId.ToString(),
               DepartmentName = x.DepartmentName,
               DepartmentDescription = x.DepartmentDescription
           };

        public static readonly Expression<Func<User, UserDto>> AsUserDto =
            x => new UserDto
            {
                Id = x.UserId,
                Name = x.UserName,
                Surname = x.UserSurname
            };

        public static readonly Expression<Func<Department, DepartmentDetailsDto>> AsDepartmentDetailsDto =
            x => new DepartmentDetailsDto
            {
                DepartmentId = x.DepartmentId,
                DepartmentName = x.DepartmentName,
                DepartmentDescription = x.DepartmentDescription,
                DepartmentHead = GetUserDto(x.DepartmentHead.User),
            };

        public static readonly Expression<Func<Feedback, FeedbackDto>> AsFeedbackDto =
            x => new FeedbackDto
            {
                FeedbackId = x.FeedbackId,
                UserId = x.UserId,
                UserFullName = x.User.UserName + " " + x.User.UserSurname,
                FeedbackDescription = x.FeedbackDescription
            };

        public static readonly Expression<Func<Doctor, DoctorDto>> AsDoctorDto =
            x => new DoctorDto
            {
                DoctorId = x.DoctorId,
                Degree = x.Degree,
                Education = x.Education,
                DoctorFullName = x.User.UserName + " " + x.User.UserSurname
            };

        public static readonly Expression<Func<Doctor, DoctorDetailsDto>> AsDoctorDetailsDto =
            x => new DoctorDetailsDto
            {
                DoctorId = x.DoctorId,
                DoctorFullName = x.User.UserName + " " + x.User.UserSurname,
                Phone = x.User.UserPhone,
                Degree = x.Degree,
                Education = x.Education,
              //  DepartmentName = x.Department.DepartmentName
            };
    }
}