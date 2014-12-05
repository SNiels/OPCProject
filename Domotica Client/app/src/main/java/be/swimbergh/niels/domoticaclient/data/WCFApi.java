package be.swimbergh.niels.domoticaclient.data;

import android.util.Log;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;
import java.util.ArrayList;

import be.swimbergh.niels.domoticaclient.model.WCFNode;

/**
 * Created by Niels on 5/12/2014.
 */
public final class WCFApi {
    public static final String APIURL = "http://172.30.26.150:9000/";
    private static final String TAG = WCFApi.class.getSimpleName();

    private static String  getJSON(String url){
        InputStream is = null;
        String json = "";
        try {
            URL u = new URL(url);
            HttpURLConnection con = (HttpURLConnection)(u).openConnection();
            con.setRequestMethod("GET");
            con.connect();
            is = con.getInputStream();
            BufferedReader reader = new BufferedReader(new InputStreamReader(is));
            StringBuilder stringBuilder = new StringBuilder();
            String line = null;
            while((line = reader.readLine())!=null){
                stringBuilder.append(line);
            }
            json = stringBuilder.toString();
        }catch(Exception ex){
            Log.e(TAG,ex.getMessage());
        }finally {
            if(is!=null){
                try {
                    is.close();
                } catch (IOException e) {
                    e.printStackTrace();
                }
            }
        }
        return json;
    }

    public static ArrayList<WCFNode> getWCFNodes(){
        ArrayList<WCFNode> nodes = null;
        try{
            String url = APIURL + "GetWCFNodes";
            String json = getJSON(url);
            JSONArray jarr = new JSONArray(json);
            nodes = new ArrayList<WCFNode>();
            for(int i = 0; i < jarr.length();i++){
                try{
                    JSONObject jsonObject = jarr.getJSONObject(i);
                    String itemId = jsonObject.getString("ItemId");
                    WCFNode n = new WCFNode(itemId);
                    nodes.add(n);
                }catch(JSONException e){}
            }
        } catch (JSONException e) {
            e.printStackTrace();
        }
        return nodes;
    }
}
