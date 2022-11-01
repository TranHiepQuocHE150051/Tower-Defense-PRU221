// THIS SCRIPT IS NOT NECESSARY FOR THE VIBRATION FUNSTIONS AND CAN BE REMOVED FROM YOUR PROJECT IF YOU WISH

using System.Collections;
using UnityEngine;

public class VibrationTest : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(VibrateBlock());
    }

    private IEnumerator VibrateBlock()
    {
        // This coroutine will fire if this script is placed on an object in the project.
        // Build the game with this script active and it will test all vibration functions on your device.

        // Each vibration will fire and then wait a total of 5 seconds before moving on to the next vibration.

        Vibrator.Vibrate(Vibration.SHORT);  // 100 ms
        yield return new WaitForSeconds(5);

        Vibrator.Vibrate(Vibration.MEDIUM); // 250 ms
        yield return new WaitForSeconds(5);

        Vibrator.Vibrate(Vibration.LONG);   // 500 ms
        yield return new WaitForSeconds(5);

        Vibrator.Vibrate(Vibration.VERY_LONG);  // 1000 ms (1 second)
        yield return new WaitForSeconds(5);

        Vibrator.Vibrate(135);  // 135 ms
        yield return new WaitForSeconds(5);

        Vibrator.Vibrate(650);  // 650 ms
        yield return new WaitForSeconds(5);

        Vibrator.Vibrate(2000); // 2000 ms (2 Seconds)
        yield return new WaitForSeconds(5);

        Vibrator.Vibrate(2000); // 2000 ms again but this time Cancel() is called half a second into the vibration call. Cancelling it after 500ms
        yield return new WaitForSeconds(0.5f);
        Vibrator.Cancel();
    }
}
