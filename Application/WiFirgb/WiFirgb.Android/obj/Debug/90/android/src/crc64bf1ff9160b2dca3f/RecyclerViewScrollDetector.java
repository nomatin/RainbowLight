package crc64bf1ff9160b2dca3f;


public abstract class RecyclerViewScrollDetector
	extends android.support.v7.widget.RecyclerView.OnScrollListener
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onScrolled:(Landroid/support/v7/widget/RecyclerView;II)V:GetOnScrolled_Landroid_support_v7_widget_RecyclerView_IIHandler\n" +
			"n_onScrollStateChanged:(Landroid/support/v7/widget/RecyclerView;I)V:GetOnScrollStateChanged_Landroid_support_v7_widget_RecyclerView_IHandler\n" +
			"";
		mono.android.Runtime.register ("Refractored.Fab.RecyclerViewScrollDetector, Refractored.Fab", RecyclerViewScrollDetector.class, __md_methods);
	}


	public RecyclerViewScrollDetector ()
	{
		super ();
		if (getClass () == RecyclerViewScrollDetector.class)
			mono.android.TypeManager.Activate ("Refractored.Fab.RecyclerViewScrollDetector, Refractored.Fab", "", this, new java.lang.Object[] {  });
	}


	public void onScrolled (android.support.v7.widget.RecyclerView p0, int p1, int p2)
	{
		n_onScrolled (p0, p1, p2);
	}

	private native void n_onScrolled (android.support.v7.widget.RecyclerView p0, int p1, int p2);


	public void onScrollStateChanged (android.support.v7.widget.RecyclerView p0, int p1)
	{
		n_onScrollStateChanged (p0, p1);
	}

	private native void n_onScrollStateChanged (android.support.v7.widget.RecyclerView p0, int p1);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
