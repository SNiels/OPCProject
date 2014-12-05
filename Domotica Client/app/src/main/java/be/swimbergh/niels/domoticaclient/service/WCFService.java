package be.swimbergh.niels.domoticaclient.service;

import android.app.IntentService;
import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.os.ResultReceiver;

import java.util.ArrayList;

import be.swimbergh.niels.domoticaclient.data.WCFApi;
import be.swimbergh.niels.domoticaclient.model.WCFNode;

/**
 * An {@link IntentService} subclass for handling asynchronous task requests in
 * a service on a separate handler thread.
 * <p/>
 * helper methods.
 */
public class WCFService extends IntentService {
    public static final String EXTRA_WCFNODES = "WCFNodes";

    public static final String ACTION_GET_NODES = "be.swimbergh.niels.domoticaclient.service.action.GET_NODES";

    public static final String EXTRA_RESULT_RECEIVER = "be.swimbergh.niels.domoticaclient.service.extra.ResultReceiver";

    public static void startActionGetNodes(Context context, ResultReceiver resultReceiver) {
        Intent intent = new Intent(context, WCFService.class);
        intent.setAction(ACTION_GET_NODES);
        intent.putExtra(EXTRA_RESULT_RECEIVER, resultReceiver);
        context.startService(intent);
    }

    public WCFService() {
        super("WCFService");
    }

    @Override
    protected void onHandleIntent(Intent intent) {
        if (intent != null) {
            final String action = intent.getAction();
            if (ACTION_GET_NODES.equals(action)) {
                final ResultReceiver resultReceiver = intent.getParcelableExtra(EXTRA_RESULT_RECEIVER);
                handleActionGetNodes(resultReceiver);
            }
        }
    }
    private void handleActionGetNodes(ResultReceiver resultReceiver) {
        ArrayList<WCFNode> nodes = WCFApi.getWCFNodes();
        Bundle b = new Bundle();
        b.putParcelableArrayList(EXTRA_WCFNODES,nodes);
        resultReceiver.send(0,b);
    }
}
