package com.example.oleksandrkliushta.testapplication.activities;

import android.content.DialogInterface;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.support.design.widget.FloatingActionButton;
import android.support.v7.app.AlertDialog;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.view.ContextThemeWrapper;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.util.Log;
import android.view.View;
import android.widget.DatePicker;
import android.widget.EditText;
import android.widget.TimePicker;
import android.widget.Toast;

import com.android.volley.AuthFailureError;
import com.android.volley.DefaultRetryPolicy;
import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonArrayRequest;
import com.android.volley.toolbox.JsonObjectRequest;
import com.android.volley.toolbox.Volley;
import com.example.oleksandrkliushta.testapplication.CustomOnShowListener;
import com.example.oleksandrkliushta.testapplication.model.Appointment;
import com.example.oleksandrkliushta.testapplication.AppointmentAdapter;
import com.example.oleksandrkliushta.testapplication.R;
import com.example.oleksandrkliushta.testapplication.model.Constants;
import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import org.jose4j.jwt.JwtClaims;
import org.jose4j.jwt.MalformedClaimException;
import org.jose4j.jwt.consumer.InvalidJwtException;
import org.jose4j.jwt.consumer.JwtConsumer;
import org.jose4j.jwt.consumer.JwtConsumerBuilder;
import org.json.JSONArray;
import org.json.JSONObject;

import java.lang.reflect.Type;
import java.security.spec.X509EncodedKeySpec;
import java.util.Date;
import java.text.Format;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class AppointmentsActivity extends AppCompatActivity {
    private RecyclerView mRecyclerView;
    AppointmentAdapter adapter;
    RequestQueue requestManager;
    Appointment appointment;

    private Date mAppointmentTime; //stores date from TimePicker dialog
    private ArrayList<Appointment> mAppointmentsList; //appointments list binded to Recycler

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        setContentView(R.layout.activity_appointments);
        getSupportActionBar().setTitle("Future appointments");

        FloatingActionButton fab = (FloatingActionButton) findViewById(R.id.fab);
        fab.setOnClickListener(mFabClickListener);

        // use this setting to improve performance if you know that changes
        // in content do not change the layout size of the RecyclerView

        mRecyclerView = (RecyclerView) findViewById(R.id.my_recycler_view);
        mAppointmentsList = new ArrayList<>();

        requestManager = Volley.newRequestQueue(this);

        String requestURL = Constants.URL_FOR_REST+"api/patients/"+17+"/appointments";
        Response.Listener<JSONArray> jsonListener = new Response.Listener<JSONArray>() {
            @Override
            public void onResponse(JSONArray list) {
                try {
                    Gson gson = new Gson();
                    Type type = new TypeToken<List<Appointment>>(){}.getType();
                    mAppointmentsList = gson.fromJson(list.toString(), type);
                    adapter = new AppointmentAdapter(mAppointmentsList);
                    // specify an adapter (see also next example)
                    mRecyclerView.setAdapter(adapter);
                } catch (Exception e) {
                    e.printStackTrace();
                }
            }
        };

        Response.ErrorListener errorListener = new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                Log.w("Volley Error", error.getMessage());
            }
        };

        JsonArrayRequest fileRequest = new JsonArrayRequest(Request.Method.GET, requestURL, jsonListener,errorListener){

            @Override
            public Map<String, String> getHeaders() throws AuthFailureError {
                HashMap<String, String> headers = new HashMap<String, String>();
                SharedPreferences editor = getSharedPreferences(Constants.MY_PREFS_NAME, MODE_PRIVATE);
                String token = editor.getString("token","null");
                String auth = "Bearer " + token;
                headers.put("Authorization", auth);
                // do not add anything here
                return headers;
            }
        };
        fileRequest.setRetryPolicy(new DefaultRetryPolicy(
                5000,
                DefaultRetryPolicy.DEFAULT_MAX_RETRIES,
                DefaultRetryPolicy.DEFAULT_BACKOFF_MULT));
        requestManager.add(fileRequest);
        // use a linear layout manager
        LinearLayoutManager mLayoutManager = new LinearLayoutManager(this);
        mRecyclerView.setLayoutManager(mLayoutManager);
    }

    /**
     * Called after Float Action Button click. Shows new appoinment dialog
     */
    View.OnClickListener mFabClickListener = new View.OnClickListener() {
        @Override
        public void onClick(View view) {
            final AppointmentsActivity contextActivity = AppointmentsActivity.this;
            mAppointmentTime = null;

            AlertDialog.Builder alertBuilder = new AlertDialog.Builder(
                    new ContextThemeWrapper(contextActivity, R.style.AppTheme));
            AlertDialog alertDialog = alertBuilder.setMessage("New appointment").
                    setView(R.layout.dialog_new_appointment).
                    setPositiveButton("REGISTER", registerClickListener).
                    setNegativeButton("CANCEL", null).
                    setNeutralButton("PICK TIME", null).
                    create();

            CustomOnShowListener dialogShowListener = new CustomOnShowListener();
            dialogShowListener.prepareShowListener(contextActivity, mPickTimeListener);
            alertDialog.setOnShowListener(dialogShowListener);
            alertDialog.show();
        }
    };

    /**
     * Called after clicking "Pick"  on TimePicker Dialog
     */
    DialogInterface.OnClickListener mPickTimeListener = new DialogInterface.OnClickListener() {
        @Override
        public void onClick(DialogInterface dialogInterface, int i) {
            AlertDialog timePickDialog = (AlertDialog) dialogInterface;
            TimePicker timePicker = (TimePicker) timePickDialog.getWindow().
                    findViewById(R.id.timePicker);

            Calendar calendar = Calendar.getInstance();
            calendar.set(Calendar.HOUR_OF_DAY, timePicker.getCurrentHour());
            calendar.set(Calendar.MINUTE, timePicker.getCurrentMinute());
            calendar.clear(Calendar.SECOND); //reset seconds to zero
            mAppointmentTime = calendar.getTime();
        }
    };

    /**
     * Called after "Register" click on new appointment dialog
     * field "appointmentTimeDate" stores date from TimePicker and DatePicker.
     * Creates new appointment and adds it to Recycler.
     */
    DialogInterface.OnClickListener registerClickListener = new DialogInterface.OnClickListener() {
        @Override
        public void onClick(DialogInterface dialogInterface, int i) {
            AlertDialog newAppointentDialog = (AlertDialog) dialogInterface;

            DatePicker datePicker = (DatePicker) newAppointentDialog.getWindow().
                    findViewById(R.id.datePicker);
            Calendar calendar = Calendar.getInstance();
            calendar.set(Calendar.DAY_OF_MONTH, datePicker.getDayOfMonth());
            calendar.set(Calendar.MONTH, datePicker.getMonth());
            calendar.set(Calendar.YEAR,datePicker.getYear()); //reset seconds to zero
            if(mAppointmentTime != null){
                calendar.set(Calendar.HOUR_OF_DAY,mAppointmentTime.getHours());
                calendar.set(Calendar.MINUTE,mAppointmentTime.getMinutes());
            }
            Date appointmentTimeDate = calendar.getTime();

            String stringTimeDate = "";
            if(appointmentTimeDate != null){
                Format formatter  = new SimpleDateFormat("dd/MM/yyyy HH:mm");
                stringTimeDate = formatter.format(appointmentTimeDate.getTime());
            }

            EditText symptomsEditText = (EditText) newAppointentDialog.getWindow().
                    findViewById(R.id.symptomsEditText);

            appointment = new Appointment(stringTimeDate,
                    symptomsEditText.getText().toString());

            String requestURL = Constants.URL_FOR_REST+"api/appointments";
            Response.Listener<JSONObject> jsonListener = new Response.Listener<JSONObject>() {
                @Override
                public void onResponse(JSONObject list) {
                    try {
                        appointment.Name="";
                        appointment.Surname="";
                        mAppointmentsList.add(appointment);
                        mRecyclerView.getAdapter().notifyItemInserted(mAppointmentsList.size()-1);
                        Toast.makeText(getApplicationContext(),"Success! Appointment added",Toast.LENGTH_SHORT).show();
                    } catch (Exception e) {
                        e.printStackTrace();
                    }
                }
            };

            Response.ErrorListener errorListener = new Response.ErrorListener() {
                @Override
                public void onErrorResponse(VolleyError error) {
                    Toast.makeText(getApplicationContext(),"Server erorr! Please try again later",Toast.LENGTH_SHORT).show();
                }
            };
            HashMap<String, String> newAppointment = new HashMap<String, String>();
            /*String publicKeyPEM = "31bf3856ad364e35";

            // decode to its constituent bytes
            BASE64Decoder base64Decoder = new BASE64Decoder();
            byte[] publicKeyBytes = base64Decoder.decodeBuffer(publicKeyPEM);

            // create a key object from the bytes
            X509EncodedKeySpec keySpec = new X509EncodedKeySpec(publicKeyBytes);
            KeyFactory keyFactory = KeyFactory.getInstance("RSA");
            PublicKey publicKey = keyFactory.generatePublic(keySpec);
            JwtConsumer jwtConsumer = new JwtConsumerBuilder()
                    .setRequireExpirationTime()
                    .setVerificationKey()
                    .build();

            // validate and decode the jwt
            try {
                JwtClaims jwtDecoded = jwtConsumer.processToClaims(getSharedPreferences(Constants.MY_PREFS_NAME, MODE_PRIVATE).getString("token","null"));
                String username = jwtDecoded.getStringClaimValue("id");
            }
            catch(InvalidJwtException e) {

            }
            catch (MalformedClaimException e)
            {

            }*/
            newAppointment.put("Description",symptomsEditText.getText().toString());
            newAppointment.put("PatientId","17");
            //newAppointment.put("Date", appointmentTimeDate.toString());

            JsonObjectRequest fileRequest = new JsonObjectRequest(Request.Method.POST, requestURL,new JSONObject(newAppointment) ,jsonListener,errorListener){

                @Override
                public Map<String, String> getHeaders() throws AuthFailureError {
                    HashMap<String, String> headers = new HashMap<String, String>();
                    SharedPreferences editor = getSharedPreferences(Constants.MY_PREFS_NAME, MODE_PRIVATE);
                    String token = editor.getString("token","null");
                    String auth = "Bearer " + token;
                    headers.put("Authorization", auth);
                    // do not add anything here
                    return headers;
                }
            };
            fileRequest.setRetryPolicy(new DefaultRetryPolicy(
                    5000,
                    DefaultRetryPolicy.DEFAULT_MAX_RETRIES,
                    DefaultRetryPolicy.DEFAULT_BACKOFF_MULT));
            requestManager.add(fileRequest);
        }
    };
}
