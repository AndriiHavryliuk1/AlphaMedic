package com.example.oleksandrkliushta.testapplication;

import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageButton;
import android.widget.TextView;

import com.example.oleksandrkliushta.testapplication.model.Appointment;

import java.util.List;

/**
 * Created by oleksandr.kliushta on 9/8/2016.
 */
public class AppointmentAdapter extends
        RecyclerView.Adapter<AppointmentAdapter.AppointmentCardViewHolder> {

    private List<Appointment> mDataset;
    /*
    * Provide a suitable constructor (depends on the kind of dataset)
    */
    public AppointmentAdapter(List<Appointment> myDataset) {
        // Here should be http request for appointment
        mDataset = myDataset;
    }

    /*
    * Create new views (invoked by the layout manager)
    */
    @Override
    public AppointmentCardViewHolder onCreateViewHolder(ViewGroup parent,int viewType) {
        // create a new view
        View cardView = LayoutInflater.from(parent.getContext())
                .inflate(R.layout.appointment_card, parent, false);
        // set the view's size, margins, paddings and layout parameters
        AppointmentCardViewHolder vh = new AppointmentCardViewHolder(cardView);

        return vh;
    }

    @Override
    public void onBindViewHolder(AppointmentCardViewHolder holder, int position) {
        holder.bindViewData(mDataset,position);
    }

    /**
    * Return the size of your dataset (invoked by the layout manager)
    */
    @Override
    public int getItemCount() {
        return mDataset.size();
    }

    /** Provide a reference to the views for each data item
    * Complex data items may need more than one view per item, and
    * you provide access to all the views for a data item in a view holder
    */
    public class AppointmentCardViewHolder extends RecyclerView.ViewHolder {
        // each data item is just a string in this case
        public TextView doctorTextView;
        public TextView dateTextView;
        public TextView durationTextView;
        public TextView stateTextView;

        public AppointmentCardViewHolder(View itemView) {
            super(itemView);

            doctorTextView = (TextView) itemView.findViewById(R.id.doctorName);
            dateTextView = (TextView) itemView.findViewById(R.id.dateTime);
            durationTextView = (TextView) itemView.findViewById(R.id.duration);
            stateTextView = (TextView) itemView.findViewById(R.id.state);
        }

        public void bindViewData(List<Appointment> dataset, int position){
            stateTextView.setText(dataset.get(position).State.toString());
            durationTextView.setText(dataset.get(position).Duration);
            dateTextView.setText(dataset.get(position).Date);
            String doctorFullName = dataset.get(position).Name + " " +
                    dataset.get(position).Surname;
            doctorTextView.setText(doctorFullName);
        }
    }
}
