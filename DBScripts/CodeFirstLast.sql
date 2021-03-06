USE [ALPHA_AlphaMedic]
GO
/****** Object:  Table [dbo].[Appointments]    Script Date: 8/8/2016 11:34:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Appointments](
	[AppointmentId] [int] IDENTITY(1,1) NOT NULL,
	[State] [int] NOT NULL,
	[ScheduleId] [int] NULL,
	[ProcedureId] [int] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Date] [datetime] NULL,
	[Duration] [time](7) NULL,
	[Patient_UserId] [int] NULL,
 CONSTRAINT [PK_dbo.Appointments] PRIMARY KEY CLUSTERED 
(
	[AppointmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Complaints]    Script Date: 8/8/2016 11:34:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Complaints](
	[ComplaintId] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Examination_ProcedureId] [int] NULL,
 CONSTRAINT [PK_dbo.Complaints] PRIMARY KEY CLUSTERED 
(
	[ComplaintId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Departments]    Script Date: 8/8/2016 11:34:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Departments](
	[DepartmentId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[URLImage] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Departments] PRIMARY KEY CLUSTERED 
(
	[DepartmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Diagnosis]    Script Date: 8/8/2016 11:34:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Diagnosis](
	[DiagnosisId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Diagnosis] PRIMARY KEY CLUSTERED 
(
	[DiagnosisId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Doctors]    Script Date: 8/8/2016 11:34:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Doctors](
	[UserId] [int] NOT NULL,
	[Degree] [nvarchar](max) NULL,
	[Education] [nvarchar](max) NULL,
	[ScheduleId] [int] NULL,
	[DepartmentId] [int] NOT NULL,
	[DoctorType] [int] NOT NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Doctors] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Employees]    Script Date: 8/8/2016 11:34:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employees](
	[UserId] [int] NOT NULL,
	[EmploymentDate] [datetime] NOT NULL,
	[EmploymentRecordBookNumber] [nvarchar](max) NULL,
	[DismissalDate] [datetime] NULL,
	[EmployeeType] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Employees] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Examinations]    Script Date: 8/8/2016 11:34:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Examinations](
	[ProcedureId] [int] NOT NULL,
	[DiagnosisId] [int] NULL,
 CONSTRAINT [PK_dbo.Examinations] PRIMARY KEY CLUSTERED 
(
	[ProcedureId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Feedbacks]    Script Date: 8/8/2016 11:34:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Feedbacks](
	[FeedbackId] [int] IDENTITY(1,1) NOT NULL,
	[PatientId] [int] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Date] [datetime] NULL,
	[FeedbackObjectId] [int] NOT NULL,
	[Patient_UserId] [int] NULL,
	[Doctor_UserId] [int] NULL,
	[Department_DepartmentId] [int] NULL,
 CONSTRAINT [PK_dbo.Feedbacks] PRIMARY KEY CLUSTERED 
(
	[FeedbackId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MedicalHistories]    Script Date: 8/8/2016 11:34:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MedicalHistories](
	[MedicalHistoryId] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_dbo.MedicalHistories] PRIMARY KEY CLUSTERED 
(
	[MedicalHistoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Medications]    Script Date: 8/8/2016 11:34:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Medications](
	[MedicationId] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Price] [decimal](18, 2) NULL,
 CONSTRAINT [PK_dbo.Medications] PRIMARY KEY CLUSTERED 
(
	[MedicationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MedicationTreatments]    Script Date: 8/8/2016 11:34:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MedicationTreatments](
	[Medication_MedicationId] [int] NOT NULL,
	[Treatment_ProcedureId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.MedicationTreatments] PRIMARY KEY CLUSTERED 
(
	[Medication_MedicationId] ASC,
	[Treatment_ProcedureId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Patients]    Script Date: 8/8/2016 11:34:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Patients](
	[UserId] [int] NOT NULL,
	[BloodGroup] [int] NULL,
	[MedicalHistoryId] [int] NULL,
	[DepartmentId] [int] NULL,
 CONSTRAINT [PK_dbo.Patients] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Procedures]    Script Date: 8/8/2016 11:34:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Procedures](
	[ProcedureId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[MedicalHistoryId] [int] NULL,
	[Date] [datetime] NOT NULL,
	[Price] [decimal](18, 2) NULL,
	[Doctor_UserId] [int] NULL,
 CONSTRAINT [PK_dbo.Procedures] PRIMARY KEY CLUSTERED 
(
	[ProcedureId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Schedules]    Script Date: 8/8/2016 11:34:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Schedules](
	[ScheduleId] [int] IDENTITY(1,1) NOT NULL,
	[StartWorkingTime] [datetime] NOT NULL,
	[FinishWorkingTime] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Schedules] PRIMARY KEY CLUSTERED 
(
	[ScheduleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Treatments]    Script Date: 8/8/2016 11:34:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Treatments](
	[ProcedureId] [int] NOT NULL,
	[DiagnosisId] [int] NOT NULL,
	[Result] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Treatments] PRIMARY KEY CLUSTERED 
(
	[ProcedureId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserClaims]    Script Date: 8/8/2016 11:34:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.UserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 8/8/2016 11:34:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Surname] [nvarchar](max) NULL,
	[Gender] [int] NOT NULL,
	[DateOfBirth] [datetime] NULL,
	[Phone] [nvarchar](max) NOT NULL,
	[Address] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[Password] [nvarchar](max) NULL,
	[Active] [bit] NOT NULL,
	[URLImage] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_dbo.Users] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Vaccionations]    Script Date: 8/8/2016 11:34:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vaccionations](
	[ProcedureId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Vaccionations] PRIMARY KEY CLUSTERED 
(
	[ProcedureId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WarningLabels]    Script Date: 8/8/2016 11:34:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WarningLabels](
	[WarningLabelId] [int] IDENTITY(1,1) NOT NULL,
	[MedicalHistoryId] [int] NOT NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.WarningLabels] PRIMARY KEY CLUSTERED 
(
	[WarningLabelId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Appointments_dbo.Patients_Patient_UserId] FOREIGN KEY([Patient_UserId])
REFERENCES [dbo].[Patients] ([UserId])
GO
ALTER TABLE [dbo].[Appointments] CHECK CONSTRAINT [FK_dbo.Appointments_dbo.Patients_Patient_UserId]
GO
ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Appointments_dbo.Schedules_ScheduleId] FOREIGN KEY([ScheduleId])
REFERENCES [dbo].[Schedules] ([ScheduleId])
GO
ALTER TABLE [dbo].[Appointments] CHECK CONSTRAINT [FK_dbo.Appointments_dbo.Schedules_ScheduleId]
GO
ALTER TABLE [dbo].[Complaints]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Complaints_dbo.Examinations_Examination_ProcedureId] FOREIGN KEY([Examination_ProcedureId])
REFERENCES [dbo].[Examinations] ([ProcedureId])
GO
ALTER TABLE [dbo].[Complaints] CHECK CONSTRAINT [FK_dbo.Complaints_dbo.Examinations_Examination_ProcedureId]
GO
ALTER TABLE [dbo].[Doctors]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Doctors_dbo.Departments_DepartmentId] FOREIGN KEY([DepartmentId])
REFERENCES [dbo].[Departments] ([DepartmentId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Doctors] CHECK CONSTRAINT [FK_dbo.Doctors_dbo.Departments_DepartmentId]
GO
ALTER TABLE [dbo].[Doctors]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Doctors_dbo.Employees_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Employees] ([UserId])
GO
ALTER TABLE [dbo].[Doctors] CHECK CONSTRAINT [FK_dbo.Doctors_dbo.Employees_UserId]
GO
ALTER TABLE [dbo].[Doctors]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Doctors_dbo.Schedules_ScheduleId] FOREIGN KEY([ScheduleId])
REFERENCES [dbo].[Schedules] ([ScheduleId])
GO
ALTER TABLE [dbo].[Doctors] CHECK CONSTRAINT [FK_dbo.Doctors_dbo.Schedules_ScheduleId]
GO
ALTER TABLE [dbo].[Employees]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Employees_dbo.Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Employees] CHECK CONSTRAINT [FK_dbo.Employees_dbo.Users_UserId]
GO
ALTER TABLE [dbo].[Examinations]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Examinations_dbo.Diagnosis_DiagnosisId] FOREIGN KEY([DiagnosisId])
REFERENCES [dbo].[Diagnosis] ([DiagnosisId])
GO
ALTER TABLE [dbo].[Examinations] CHECK CONSTRAINT [FK_dbo.Examinations_dbo.Diagnosis_DiagnosisId]
GO
ALTER TABLE [dbo].[Examinations]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Examinations_dbo.Procedures_ProcedureId] FOREIGN KEY([ProcedureId])
REFERENCES [dbo].[Procedures] ([ProcedureId])
GO
ALTER TABLE [dbo].[Examinations] CHECK CONSTRAINT [FK_dbo.Examinations_dbo.Procedures_ProcedureId]
GO
ALTER TABLE [dbo].[Feedbacks]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Feedbacks_dbo.Departments_Department_DepartmentId] FOREIGN KEY([Department_DepartmentId])
REFERENCES [dbo].[Departments] ([DepartmentId])
GO
ALTER TABLE [dbo].[Feedbacks] CHECK CONSTRAINT [FK_dbo.Feedbacks_dbo.Departments_Department_DepartmentId]
GO
ALTER TABLE [dbo].[Feedbacks]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Feedbacks_dbo.Doctors_Doctor_UserId] FOREIGN KEY([Doctor_UserId])
REFERENCES [dbo].[Doctors] ([UserId])
GO
ALTER TABLE [dbo].[Feedbacks] CHECK CONSTRAINT [FK_dbo.Feedbacks_dbo.Doctors_Doctor_UserId]
GO
ALTER TABLE [dbo].[Feedbacks]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Feedbacks_dbo.Patients_Patient_UserId] FOREIGN KEY([Patient_UserId])
REFERENCES [dbo].[Patients] ([UserId])
GO
ALTER TABLE [dbo].[Feedbacks] CHECK CONSTRAINT [FK_dbo.Feedbacks_dbo.Patients_Patient_UserId]
GO
ALTER TABLE [dbo].[MedicationTreatments]  WITH CHECK ADD  CONSTRAINT [FK_dbo.MedicationTreatments_dbo.Medications_Medication_MedicationId] FOREIGN KEY([Medication_MedicationId])
REFERENCES [dbo].[Medications] ([MedicationId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MedicationTreatments] CHECK CONSTRAINT [FK_dbo.MedicationTreatments_dbo.Medications_Medication_MedicationId]
GO
ALTER TABLE [dbo].[MedicationTreatments]  WITH CHECK ADD  CONSTRAINT [FK_dbo.MedicationTreatments_dbo.Treatments_Treatment_ProcedureId] FOREIGN KEY([Treatment_ProcedureId])
REFERENCES [dbo].[Treatments] ([ProcedureId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MedicationTreatments] CHECK CONSTRAINT [FK_dbo.MedicationTreatments_dbo.Treatments_Treatment_ProcedureId]
GO
ALTER TABLE [dbo].[Patients]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Patients_dbo.Departments_DepartmentId] FOREIGN KEY([DepartmentId])
REFERENCES [dbo].[Departments] ([DepartmentId])
GO
ALTER TABLE [dbo].[Patients] CHECK CONSTRAINT [FK_dbo.Patients_dbo.Departments_DepartmentId]
GO
ALTER TABLE [dbo].[Patients]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Patients_dbo.MedicalHistories_MedicalHistoryId] FOREIGN KEY([MedicalHistoryId])
REFERENCES [dbo].[MedicalHistories] ([MedicalHistoryId])
GO
ALTER TABLE [dbo].[Patients] CHECK CONSTRAINT [FK_dbo.Patients_dbo.MedicalHistories_MedicalHistoryId]
GO
ALTER TABLE [dbo].[Patients]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Patients_dbo.Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Patients] CHECK CONSTRAINT [FK_dbo.Patients_dbo.Users_UserId]
GO
ALTER TABLE [dbo].[Procedures]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Procedures_dbo.Appointments_ProcedureId] FOREIGN KEY([ProcedureId])
REFERENCES [dbo].[Appointments] ([AppointmentId])
GO
ALTER TABLE [dbo].[Procedures] CHECK CONSTRAINT [FK_dbo.Procedures_dbo.Appointments_ProcedureId]
GO
ALTER TABLE [dbo].[Procedures]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Procedures_dbo.Doctors_Doctor_UserId] FOREIGN KEY([Doctor_UserId])
REFERENCES [dbo].[Doctors] ([UserId])
GO
ALTER TABLE [dbo].[Procedures] CHECK CONSTRAINT [FK_dbo.Procedures_dbo.Doctors_Doctor_UserId]
GO
ALTER TABLE [dbo].[Procedures]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Procedures_dbo.MedicalHistories_MedicalHistoryId] FOREIGN KEY([MedicalHistoryId])
REFERENCES [dbo].[MedicalHistories] ([MedicalHistoryId])
GO
ALTER TABLE [dbo].[Procedures] CHECK CONSTRAINT [FK_dbo.Procedures_dbo.MedicalHistories_MedicalHistoryId]
GO
ALTER TABLE [dbo].[Treatments]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Treatments_dbo.Diagnosis_DiagnosisId] FOREIGN KEY([DiagnosisId])
REFERENCES [dbo].[Diagnosis] ([DiagnosisId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Treatments] CHECK CONSTRAINT [FK_dbo.Treatments_dbo.Diagnosis_DiagnosisId]
GO
ALTER TABLE [dbo].[Treatments]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Treatments_dbo.Procedures_ProcedureId] FOREIGN KEY([ProcedureId])
REFERENCES [dbo].[Procedures] ([ProcedureId])
GO
ALTER TABLE [dbo].[Treatments] CHECK CONSTRAINT [FK_dbo.Treatments_dbo.Procedures_ProcedureId]
GO
ALTER TABLE [dbo].[UserClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo.UserClaims_dbo.Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserClaims] CHECK CONSTRAINT [FK_dbo.UserClaims_dbo.Users_UserId]
GO
ALTER TABLE [dbo].[Vaccionations]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Vaccionations_dbo.Procedures_ProcedureId] FOREIGN KEY([ProcedureId])
REFERENCES [dbo].[Procedures] ([ProcedureId])
GO
ALTER TABLE [dbo].[Vaccionations] CHECK CONSTRAINT [FK_dbo.Vaccionations_dbo.Procedures_ProcedureId]
GO
ALTER TABLE [dbo].[WarningLabels]  WITH CHECK ADD  CONSTRAINT [FK_dbo.WarningLabels_dbo.MedicalHistories_MedicalHistoryId] FOREIGN KEY([MedicalHistoryId])
REFERENCES [dbo].[MedicalHistories] ([MedicalHistoryId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[WarningLabels] CHECK CONSTRAINT [FK_dbo.WarningLabels_dbo.MedicalHistories_MedicalHistoryId]
GO
