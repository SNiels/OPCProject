package be.swimbergh.niels.domoticaclient.ui;

import android.app.Activity;
import android.os.Bundle;
import android.os.Handler;
import android.os.Looper;
import android.os.Parcelable;
import android.os.ResultReceiver;
import android.view.View;
import android.widget.Toast;

import java.util.ArrayList;

import be.swimbergh.niels.domoticaclient.R;
import be.swimbergh.niels.domoticaclient.model.WCFNode;
import be.swimbergh.niels.domoticaclient.service.WCFService;


public class MainActivity extends Activity {
    private ResultReceiver mResultReceiver = new ResultReceiver(new Handler(Looper.getMainLooper())){
        @Override
        protected void onReceiveResult(int resultCode, Bundle resultData) {
            ArrayList<Parcelable> parcelables = resultData.getParcelableArrayList(WCFService.EXTRA_WCFNODES);
            if(parcelables==null){
                Toast.makeText(MainActivity.this,"Check your network",Toast.LENGTH_LONG).show();
            }else{
                ArrayList<WCFNode> nodes = new ArrayList<WCFNode>();
                for(int i = 0 ; i < parcelables.size();i++){
                    Parcelable p = parcelables.get(i);
                    nodes.add((WCFNode)p);
                }
                mHuisView.setNodes(nodes);
            }

        }
    };
    private HuisView mHuisView;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        mHuisView = (HuisView) findViewById(R.id.hvHuis);
    }

    public void showNodes(View v){
        WCFService.startActionGetNodes(this,mResultReceiver);
    }
}
