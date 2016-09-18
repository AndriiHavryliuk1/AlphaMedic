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

    public Date DateTime;
    public Time Duration;

    public Appointment() {}

    public Appointment(Date dateTime, String description) {
        this.DateTime = dateTime;
        this.Description = description;
        this.State = AppointmentState.Accepted;
    }

    public enum AppointmentState
    {
        Unconfirmed,
        Accepted,
    }
}



