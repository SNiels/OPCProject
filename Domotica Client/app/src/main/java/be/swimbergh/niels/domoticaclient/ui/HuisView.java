package be.swimbergh.niels.domoticaclient.ui;

import android.annotation.TargetApi;
import android.content.Context;
import android.os.Build;
import android.util.AttributeSet;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.FrameLayout;
import android.widget.GridView;

import java.util.ArrayList;
import java.util.List;

import be.swimbergh.niels.domoticaclient.R;
import be.swimbergh.niels.domoticaclient.model.WCFNode;


/**
 * TODO: document your custom view class.
 */
public class HuisView extends FrameLayout {
    private GridView mGridVerdiep;
    private ArrayList<WCFNode> mNodes;
    private VerdiepAdapter mAdapter;

    public HuisView(Context context) {
        super(context);
        init();
    }

    public HuisView(Context context, AttributeSet attrs) {
        super(context, attrs);
        init();
    }

    public HuisView(Context context, AttributeSet attrs, int defStyleAttr) {
        super(context, attrs, defStyleAttr);
        init();
    }

    @TargetApi(Build.VERSION_CODES.LOLLIPOP)
    public HuisView(Context context, AttributeSet attrs, int defStyleAttr, int defStyleRes) {
        super(context, attrs, defStyleAttr, defStyleRes);
        init();
    }

    private void init() {
        LayoutInflater inflater = LayoutInflater.from(getContext());
        View v = inflater.inflate(R.layout.huis_view,this,true);
        mGridVerdiep = (GridView) v.findViewById(R.id.gridVerdiep);
    }

    public void setNodes(ArrayList<WCFNode> nodes){
        mNodes = nodes;
        mAdapter = new VerdiepAdapter(getContext(),nodes);
        mGridVerdiep.setAdapter(mAdapter);
    }

    private class VerdiepAdapter extends ArrayAdapter<WCFNode>{

        public VerdiepAdapter(Context context,List<WCFNode> nodes) {
            super(context,0,nodes);
        }

        @Override
        public View getView(int position, View convertView, ViewGroup parent) {
            return new LampView(getContext(),getItem(position));
        }
    }
}
