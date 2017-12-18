package com.example.oleksandrkliushta.testapplication.activities;

import android.content.Intent;
import android.content.SharedPreferences;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

import com.android.volley.AuthFailureError;
import com.android.volley.DefaultRetryPolicy;
import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.StringRequest;
import com.android.volley.toolbox.Volley;
import com.example.oleksandrkliushta.testapplication.R;
import com.example.oleksandrkliushta.testapplication.model.Constants;
import com.google.common.hash.Hashing;

import org.json.JSONObject;

import java.nio.charset.StandardCharsets;
import java.util.HashMap;
import java.util.Map;

public class LoginActivity extends AppCompatActivity {
    TextView email;
    String hashed;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);



        Button button = (Button) findViewById(R.id.LoginButton);
        button.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                String url=Constants.URL_FOR_REST+"Token";
                fetchJsonResponse(url);
            }
        });
    }

    private void fetchJsonResponse(String url){
         email = (TextView) findViewById(R.id.Email);
        TextView password = (TextView) findViewById(R.id.Password);
        String pass = password.getText().toString();
        hashed = Hashing.sha256()
                .hashString(pass, StandardCharsets.UTF_8).toString();


        RequestQueue requestManager = Volley.newRequestQueue(this);

        Response.Listener<String> jsonListener = new Response.Listener<String>() {
            @Override
            public void onResponse(String list) {
                try {
                    JSONObject jObject  = new JSONObject(list);
                    SharedPreferences.Editor editor = getSharedPreferences(Constants.MY_PREFS_NAME, MODE_PRIVATE).edit();
                    String token = jObject.getString("access_token");
                    editor.putString("token", token);
                    editor.apply();



                    startActivity(new Intent(getApplicationContext(), AppointmentsActivity.class));
                } catch (Exception e) {
                    e.printStackTrace();
                }
            }
        };

        Response.ErrorListener errorListener = new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                if (error.networkResponse == null || error.networkResponse.statusCode == 400)
                    Toast.makeText(getApplicationContext(),"Server erorr! Please try again later",Toast.LENGTH_SHORT).show();
                else Toast.makeText(getApplicationContext(),"Wrong Email or Password!",Toast.LENGTH_SHORT).show();
            }
        };

        StringRequest fileRequest = new StringRequest(Request.Method.POST, url, jsonListener,errorListener){
            @Override
            public String getBodyContentType()
            {
                return "application/json";
            }

            @Override
            protected Map<String, String> getParams() throws AuthFailureError {
                Map<String,String> params = new HashMap<>();
                params.put("grant_type","password");
                params.put("username", email.getText().toString());
                params.put("password", hashed);
                params.put("client", "desktop");
                return params;
            }

            @Override
            public Map<String, String> getHeaders() throws AuthFailureError {
                HashMap<String, String> headers = new HashMap<String, String>();
                // do not add anything here
                return headers;
            }
        };
        fileRequest.setRetryPolicy(new DefaultRetryPolicy(
                10000,
                DefaultRetryPolicy.DEFAULT_MAX_RETRIES,
                DefaultRetryPolicy.DEFAULT_BACKOFF_MULT));
        requestManager.add(fileRequest);
    }

}
