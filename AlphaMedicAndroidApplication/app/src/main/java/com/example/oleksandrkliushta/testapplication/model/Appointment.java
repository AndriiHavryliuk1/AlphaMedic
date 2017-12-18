package com.example.oleksandrkliushta.testapplication.model;

import java.sql.Time;
import java.util.Date;


/**
 * Created by oleksandr.kliushta on 9/8/2016.
 */
public class Appointment
{
    public int AppointmentId;
    public AppointmentState State;
    public int DoctorId;
    public int PatientId;
    public String Surname;
    public String Name;
    public String Description;

    public String Date;
    public String Duration;

    public Appointment() {}

    public Appointment(String dateTime, String description) {
        this.Date = dateTime;
        this.Description = description;
        this.State = AppointmentState.Unconfirmed;
    }

    public enum AppointmentState
    {
        Unconfirmed,
        Accepted,
    }
}



