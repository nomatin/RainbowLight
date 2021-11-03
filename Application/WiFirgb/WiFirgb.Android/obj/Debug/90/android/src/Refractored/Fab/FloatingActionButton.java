package Refractored.Fab;


public class FloatingActionButton
	extends android.widget.ImageButton
	implements
		mono.android.IGCUserPeer,
		android.view.ViewTreeObserver.OnPreDrawListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onMeasure:(II)V:GetOnMeasure_IIHandler\n" +
			"n_onPreDraw:()Z:GetOnPreDrawHandler:Android.Views.ViewTreeObserver/IOnPreDrawListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("Refractored.Fab.FloatingActionButton, Refractored.Fab", FloatingActionButton.class, __md_methods);
	}


	public FloatingActionButton (android.content.Context p0)
	{
		super (p0);
		if (getClass () == FloatingActionButton.class)
			mono.android.TypeManager.Activate ("Refractored.Fab.FloatingActionButton, Refractored.Fab", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public FloatingActionButton (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == FloatingActionButton.class)
			mono.android.TypeManager.Activate ("Refractored.Fab.FloatingActionButton, Refractored.Fab", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public FloatingActionButton (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == FloatingActionButton.class)
			mono.android.TypeManager.Activate ("Refractored.Fab.FloatingActionButton, Refractored.Fab", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public FloatingActionButton (android.content.Context p0, android.util.AttributeSet p1, int p2, int p3)
	{
		super (p0, p1, p2, p3);
		if (getClass () == FloatingActionButton.class)
			mono.android.TypeManager.Activate ("Refractored.Fab.FloatingActionButton, Refractored.Fab", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2, p3 });
	}


	public void onMeasure (int p0, int p1)
	{
		n_onMeasure (p0, p1);
	}

	private native void n_onMeasure (int p0, int p1);


	public boolean onPreDraw ()
	{
		return n_onPreDraw ();
	}

	private native boolean n_onPreDraw ();

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
