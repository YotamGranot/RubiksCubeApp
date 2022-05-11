using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Telephony;
using Android.Views;
using Android.Widget;

[assembly: UsesPermission(
  Manifest.Permission.ReadPhoneState)]
[assembly: UsesPermission(
  Manifest.Permission.ProcessOutgoingCalls)]
namespace RubiksCubeApp
{
    [BroadcastReceiver(Enabled = true)]
    [IntentFilter(new[] { TelephonyManager.ActionPhoneStateChanged, Intent.ActionNewOutgoingCall })]
    public class CallReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Extras != null)
            {

                string state = intent.GetStringExtra(TelephonyManager.ExtraState); //get the state of the phone
                bool stopped = false;
                if (state == TelephonyManager.ExtraStateRinging)//if the ohone is ringing stop the timer.
                {
                    if (RubikActivity.timer.Enabled)
                    {
                        RubikActivity.timer.Stop();
                        stopped = true;
                    }
                }
                else if (state == TelephonyManager.ExtraStateOffhook)
                {
                    // incoming call answer
                }
                else if (state == TelephonyManager.ExtraStateIdle)
                {
                    if (stopped) //start the timer again.
                    {
                        RubikActivity.timer.Start();
                    }
                }
            }
        }
    }
}