package be.swimbergh.niels.domoticaclient.model;

import android.os.Parcel;
import android.os.Parcelable;

/**
 * Created by Niels on 5/12/2014.
 */
public class WCFNode implements Parcelable{
    private String mItemId;
    public WCFNode(){}
    public WCFNode(Parcel data){
        this(data.readString());
    }
    public WCFNode(String itemId){
        mItemId=itemId;
    }
    public String getItemId(){
        return mItemId;
    }
    public void setItemId(String itemId){
        mItemId=itemId;
    }
    public String getName(){
        if(mItemId==null)
            return null;
        String[] names = mItemId.split(".");
        if(names.length<1)
            return null;
        return names[names.length-1];
    }

    @Override
    public int describeContents() {
        return 0;
    }

    @Override
    public void writeToParcel(Parcel dest, int flags) {
        dest.writeString(mItemId);
    }
}
