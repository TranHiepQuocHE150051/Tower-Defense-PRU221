using UnityEngine;
/// <summary>
/// Performs mobile device vibration
/// </summary>
public static class Vibrator
{
#if UNITY_ANDROID && !UNITY_EDITOR
    public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#else
    public static AndroidJavaClass unityPlayer;
    public static AndroidJavaObject currentActivity;
    public static AndroidJavaObject vibrator;
#endif

    /// <summary>
    /// Vibrates an Android device for the specified length of time in milliseconds (1/1000th of a second).
    /// IOS devices will perform a standard device vibration.
    /// </summary>
    /// <param name="milliseconds"></param>
    public static void Vibrate(long milliseconds)
    {
        if (IsAndroid())
            vibrator.Call("vibrate", milliseconds);
        else
            Handheld.Vibrate();
    }

    /// <summary>
    /// Vibrates an Android device for the length of time specified.
    /// IOS devices will perform a standard device vibration.
    /// </summary>
    /// <param name="vibration"></param>
    public static void Vibrate(Vibration vibration)
    {
        if (IsAndroid())
            vibrator.Call("vibrate", (int)vibration);
        else
            Handheld.Vibrate();
    }

    /// <summary>
    /// Cancels any active vibration.
    /// </summary>
    public static void Cancel()
    {
        if (IsAndroid())
            vibrator.Call("cancel");
    }

    /// <summary>
    /// Returns a boolean value of whether the device is Android or not.
    /// </summary>
    /// <returns></returns>
    public static bool IsAndroid()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
	    return true;
#else
        return false;
#endif
    }
}

// ---------------------------------------------------------------------- //
// Comp-3 Interactive                                                     //
// ---------------------------------------------------------------------- //
// Follow us on Instagram, Facebook and Twitter for bite-sized Unity tips //
// and YouTube for full length, in-depth tutorials!                       //
// ---------------------------------------------------------------------- //