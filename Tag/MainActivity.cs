using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Android.Views.InputMethods;

namespace Tag
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.content_main);

            //Button for log in
            Button logIn = FindViewById<Button>(Resource.Id.logInButton);
            logIn.Click += LogInOnClick;

            //Button for new user
            Button addNewUser = FindViewById<Button>(Resource.Id.CreateNewUser);
            addNewUser.Click += CreateUser;
        }


        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void LogInOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View)sender;

            GetClientCredentials();
            HideKeyboard();

            try
            {
                if (Server.CheckLogIn(Client.Username, Client.Password))
                {
                    GoToActivity(typeof(GameActivity));
                }
                else
                {
                    Snackbar.Make(view, "Failed to log in - wrong username and password!", Snackbar.LengthLong).Show();
                }
            }
            catch(Exception e) {
                Snackbar.Make(view, "Error! " + e.Message, Snackbar.LengthLong).Show();
            }
        }

        private void CreateUser(object sender, EventArgs eventArgs) {

            View view = (View)sender;

            GetClientCredentials();
            HideKeyboard();

            if (Client.Username == "" || Client.Password == "")
            {
                Snackbar.Make(view, "Username and password can't be blank!", Snackbar.LengthShort).Show();
                return;
            }
            else
            {
                try
                {
                    if (Server.AddUser(Client.Username, Client.Password))
                    {
                        Snackbar.Make(view, "Created new user " + Client.Username + " successfully!", Snackbar.LengthLong).Show();
                    }
                    else
                    {
                        Snackbar.Make(view, "Failed to create new user! There is user with provided name.", Snackbar.LengthLong).Show();
                    }
                }
                catch (Exception e)
                {
                    Snackbar.Make(view, "Error! " + e.Message, Snackbar.LengthLong).Show();
                }

            }
        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public void GoToActivity(Type myActivity)
        {
            StartActivity(myActivity);
        }

        public void HideKeyboard() {
            InputMethodManager inputManager = (InputMethodManager)GetSystemService(InputMethodService);
            if (CurrentFocus != null)
                inputManager.HideSoftInputFromWindow(CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);
        }

        public void GetClientCredentials() {
            Client.Username = FindViewById<EditText>(Resource.Id.TextUsername).Text;
            Client.Password = FindViewById<EditText>(Resource.Id.TextPassword).Text;
        }
    }
}

