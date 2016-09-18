package com.example.oleksandrkliushta.testapplication;

import android.content.DialogInterface;
import android.support.v7.app.AlertDialog;
import android.support.v7.view.ContextThemeWrapper;
import android.view.View;
import android.widget.Button;
import com.example.oleksandrkliushta.testapplication.activities.AppointmentsActivity;

/**
 * Created by XPS on 09/11/2016.
 * Listener to open TimePicker Dialog
 */
public class CustomOnShowListener implements DialogInterface.OnShowListener {

    private AppointmentsActivity mContextActivity;
    private DialogInterface.OnClickListener mPickTimeListener;

    @Override
    public void onShow(DialogInterface dialog) {
        if (mContextActivity != null && mPickTimeListener != null) {

            AlertDialog alertDialog = (AlertDialog) dialog;
            Button neutral = alertDialog.getButton(AlertDialog.BUTTON_NEUTRAL);

            neutral.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View view) {
                        AlertDialog.Builder alertBuilder = new AlertDialog.Builder(
                        new ContextThemeWrapper(mContextActivity, R.style.AppTheme));
                        AlertDialog alertDialog = alertBuilder.setMessage("Pick a time").
                        setView(R.layout.dialog_timepicker).
                        setPositiveButton("PICK", mPickTimeListener).
                        setNegativeButton("CANCEL", null)
                        .create();
                        alertDialog.show();
                }
            });
        }
    }

    /**
     * Needs to be called before before using CustomOnShowListener,
     * otherwise errors possible
     *
     * @param contextActivity - context
     * @param pickTimeListener - listener for "Pick" button on TimePicker dialog
     */
    public void prepareShowListener(AppointmentsActivity contextActivity,
                                    DialogInterface.OnClickListener pickTimeListener){
        this.mContextActivity = contextActivity;
        this.mPickTimeListener = pickTimeListener;
    }
}
