package be.swimbergh.niels.domoticaclient.ui;

import android.annotation.TargetApi;
import android.content.Context;
import android.os.Build;
import android.util.AttributeSet;
import android.view.LayoutInflater;
import android.view.View;
import android.widget.FrameLayout;
import android.widget.TextView;

import be.swimbergh.niels.domoticaclient.R;
import be.swimbergh.niels.domoticaclient.model.WCFNode;

public class LampView extends FrameLayout {
    private TextView mTxtLamp;
    private WCFNode mNode;

    public LampView(Context context) {
        super(context);
    }
    public LampView(Context context,WCFNode node) {
        super(context);
        mNode=node;
        init(node);
    }

    public LampView(Context context, AttributeSet attrs,WCFNode node) {
        super(context, attrs);
        init(node);
    }

    public LampView(Context context, AttributeSet attrs, int defStyleAttr,WCFNode node) {
        super(context, attrs, defStyleAttr);
        init(node);
    }

    @TargetApi(Build.VERSION_CODES.LOLLIPOP)
    public LampView(Context context, AttributeSet attrs, int defStyleAttr, int defStyleRes,WCFNode node) {
        super(context, attrs, defStyleAttr, defStyleRes);
        init(node);
    }


    public WCFNode getNode(){
        return mNode;
    }

    public void setNode(WCFNode node){
         mNode=node;
        if(mNode!=null){
            mTxtLamp.setText(mNode.getItemId());
        }
    }

    private void init(WCFNode node) {
        LayoutInflater inflater = LayoutInflater.from(getContext());
        View v = inflater.inflate(R.layout.lamp_view,this,true);
        mTxtLamp = (TextView) v.findViewById(R.id.txtLamp);
        setNode(node);
    }
}
