using System;
using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;

namespace Tag
{
    [Activity(Label = "GameActivity")]
    public class GameActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.game_page);

            Button getTag = FindViewById<Button>(Resource.Id.GetTag);
            getTag.Click += CurrentTag;

            //Set current tagUser
            getTag.CallOnClick();

            Spinner spinner = FindViewById<Spinner>(Resource.Id.SelectSpinner);

            //Set list of users for tagging
            ArrayAdapter<string> dataAdapter = new ArrayAdapter<string>(this, Resource.Layout.support_simple_spinner_dropdown_item, Server.GetListOfUsers());
            dataAdapter.SetDropDownViewResource(Resource.Layout.support_simple_spinner_dropdown_item);
            spinner.Adapter = dataAdapter;

            //Button for tagging
            Button setTag = FindViewById<Button>(Resource.Id.SetTag);
            setTag.Click += SetTag;
        }



        private void CurrentTag(object sender, EventArgs eventArgs) {
            View view = (View)sender;

            TextView tagText = FindViewById<TextView>(Resource.Id.Tag);
            tagText.Text = GetCurrentTag();

            TextView timeText = FindViewById<TextView>(Resource.Id.TimeData);
            string time = GetMostRecentTimeTagged();
            string who = WhoTaggedYou(time);
            if (time != "Never")
                timeText.Text = time + " by " + who;
            else
                timeText.Text = time;


            Snackbar.Make(view, "Tag info updated!", Snackbar.LengthShort).Show();
        }

        private string GetCurrentTag() {
            return Server.GetTag();
        }

        private string GetMostRecentTimeTagged()
        {
            return Server.GetLastTagTime();
        }

        private string WhoTaggedYou(string lastTimeTagged) {
            return Server.WhoTaggedYou(lastTimeTagged);
        }




        private void SetTag(object sender, EventArgs eventArgs) {
            View view = (View)sender;

            string nextTag = FindViewById<Spinner>(Resource.Id.SelectSpinner).SelectedItem.ToString();

            string currentTag = GetCurrentTag();
            if (currentTag == Client.Username)
            {
                Server.SetTag(currentTag, nextTag);
                CurrentTag(sender, eventArgs);
                Snackbar.Make(view, "You successfully taged " + nextTag + "!", Snackbar.LengthLong).Show();
            }
            else
            {
                Snackbar.Make(view, "You are not the tag!", Snackbar.LengthLong).Show();
            }
        }




    }
}