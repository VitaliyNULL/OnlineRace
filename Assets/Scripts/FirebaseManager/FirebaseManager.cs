using System;
using System.Collections;
using System.Linq;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;
using UnityEngine.Events;
using VitaliyNULL.MenuSceneUI.LoginAndRegistration;

namespace VitaliyNULL.FirebaseManager
{
    public class FirebaseManager : MonoBehaviour
    {
        #region Private Fields

        private readonly string _passwordKey = "PASSWORD";
        private readonly string _emailKey = "EMAIL";
        private bool _isSavingData = false;

        //Firebase variables
        private FirebaseAuth _auth;
        private FirebaseUser _user;
        private DatabaseReference _dataBaseReference;
        private bool _isInitialized;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            DontDestroyOnLoad(this);
        }

        #endregion

        #region Public Methods

        public void InitializeFirebase()
        {
            _auth = FirebaseAuth.DefaultInstance;
            _dataBaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        }

        public void AutoLogin(string email, string password, UnityAction openMainMenu)
        {
            StartCoroutine(WaitForAutoLogin(email, password, openMainMenu));
        }

        //Function for the login button
        public void LoginButton(EmailInput emailInput, PasswordInput passwordInput, WarningUI warningUI,
            UnityAction openMainMenu)
        {
            //Call the login coroutine passing the email and password
            StartCoroutine(WaitForLogin(emailInput, passwordInput, warningUI, openMainMenu));
        }

        //Function for the register button
        public void RegisterButton(EmailInput emailInput, PasswordInput passwordInput,
            ConfirmPasswordInput confirmPasswordInput, WarningUI warningUI, UnityAction openMainMenu)
        {
            //Call the register coroutine passing the email, password, and username
            StartCoroutine(WaitForRegister(emailInput, passwordInput, confirmPasswordInput, warningUI, openMainMenu));
        }

        public void ExitAccount(UnityAction openLoginWindow)
        {
            _auth.SignOut();
            PlayerPrefs.DeleteAll();
            //TODO : Reset rating
            //TODO: Open login window
            openLoginWindow.Invoke();
        }

        public void SaveData()
        {
            if (!_isSavingData)
            {
                _isSavingData = true;
                StartCoroutine(UpdateUsernameAuth(_auth.CurrentUser.DisplayName));
                StartCoroutine(UpdateUsernameDatabase(_auth.CurrentUser.DisplayName));
                //TODO: Get rating
                // StartCoroutine(UpdateBestScore(ScoreManager.Instance.GetBestScore()));
            }
        }

        public void RegisterNewAccount()
        {
            _auth.SignOut();
            // ScoreManager.Instance.ResetBestScoreForNewCustomer();
            //TODO : reset rating for new customer
            // UIManager.Instance.OpenRegistrationWindow();
            //TODO: Open Registration
        }

        public void SignInExistingAccount()
        {
            _auth.SignOut();
            // ScoreManager.Instance.ResetBestScoreForNewCustomer();
            //TODO : reset rating for new customer
            // UIManager.Instance.OpenAuthWindow();
            //TODO: Open Login window
        }

        #endregion


        #region Coroutines

        private IEnumerator WaitForAutoLogin(string email, string password, UnityAction openMainMenu)
        {
            //Call the Firebase auth signin function passing the email and password
            var loginTask = _auth.SignInWithEmailAndPasswordAsync(email, password);
            //Wait until the task completes
            yield return new WaitUntil(() => loginTask.IsCompleted);

            if (loginTask.Exception != null)
            {
                //If there are errors handle them
                Debug.LogWarning(message: $"Failed to register task with {loginTask.Exception}");
                FirebaseException firebaseEx = loginTask.Exception.GetBaseException() as FirebaseException;
                Debug.Log(firebaseEx?.Message);
                if (firebaseEx == null)
                {
                    Debug.Log("FirebaseException is null");
                }
                else
                {
                    AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                    string message = "Login Failed!";
                    switch (errorCode)
                    {
                        case AuthError.MissingEmail:
                            message = "Missing Email";
                            break;
                        case AuthError.MissingPassword:
                            message = "Missing Password";
                            break;
                        case AuthError.WrongPassword:
                            message = "Wrong Password";
                            break;
                        case AuthError.InvalidEmail:
                            message = "Invalid Email";
                            break;
                        case AuthError.UserNotFound:
                            message = "Account does not exist";
                            break;
                    }

                    Debug.Log(message);
                }
            }
            else
            {
                //User is now logged in
                //Now get the result
                _user = loginTask.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})", _user.DisplayName, _user.Email);
                StartCoroutine(LoadUserData());
                // StartCoroutine(LoadScoreBoardData());
                //TODO: Open MainMenu
                openMainMenu.Invoke();
            }
        }

        private IEnumerator WaitForLogin(EmailInput emailInput, PasswordInput passwordInput, WarningUI warningUI,
            UnityAction openMainMenu)
        {
            //Call the Firebase auth signin function passing the email and password
            var loginTask = _auth.SignInWithEmailAndPasswordAsync(emailInput.email, passwordInput.password);
            //Wait until the task completes
            yield return new WaitUntil(() => loginTask.IsCompleted);

            if (loginTask.Exception != null)
            {
                //If there are errors handle them
                Debug.LogWarning(message: $"Failed to register task with {loginTask.Exception}");
                FirebaseException firebaseEx = loginTask.Exception.GetBaseException() as FirebaseException;
                Debug.Log(firebaseEx?.Message);
                if (firebaseEx == null)
                {
                    Debug.Log("FirebaseException is null");
                }
                else
                {
                    AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                    string message = "Login Failed!";
                    switch (errorCode)
                    {
                        case AuthError.MissingEmail:
                            message = "Missing Email";
                            break;
                        case AuthError.MissingPassword:
                            message = "Missing Password";
                            break;
                        case AuthError.WrongPassword:
                            message = "Wrong Password";
                            break;
                        case AuthError.InvalidEmail:
                            message = "Invalid Email";
                            break;
                        case AuthError.UserNotFound:
                            message = "Account does not exist";
                            break;
                    }

                    Debug.Log(message);
                    warningUI.ChangeWarningText(message);
                }
            }
            else
            {
                //User is now logged in
                //Now get the result
                _user = loginTask.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})", _user.DisplayName, _user.Email);
                StartCoroutine(LoadUserData());
                // StartCoroutine(LoadScoreBoardData());
                PlayerPrefs.SetString(_passwordKey, passwordInput.password);
                PlayerPrefs.SetString(_emailKey, emailInput.email);
                //TODO: Open MainMenu
                openMainMenu.Invoke();
            }
        }

        private IEnumerator WaitForRegister(EmailInput emailInput, PasswordInput passwordInput,
            ConfirmPasswordInput confirmPasswordInput, WarningUI warningUI, UnityAction openMainMenu)
        {
            if (passwordInput.password != confirmPasswordInput.confirmPassword)
            {
                //If the password does not match show a warning
                warningUI.ChangeWarningText("Password Does Not Match!");
            }
            else
            {
                //Call the Firebase auth signin function passing the email and password
                var registerTask = _auth.CreateUserWithEmailAndPasswordAsync(emailInput.email, passwordInput.password);
                //Wait until the task completes
                yield return new WaitUntil(predicate: () => registerTask.IsCompleted);

                if (registerTask.Exception != null)
                {
                    //If there are errors handle them
                    Debug.LogWarning(message: $"Failed to register task with {registerTask.Exception}");
                    FirebaseException firebaseEx = registerTask.Exception.GetBaseException() as FirebaseException;
                    if (firebaseEx == null)
                    {
                        Debug.Log("FirebaseException is null");
                    }
                    else
                    {
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                        string message = "Register Failed!";
                        switch (errorCode)
                        {
                            case AuthError.MissingEmail:
                                message = "Missing Email";
                                break;
                            case AuthError.MissingPassword:
                                message = "Missing Password";
                                break;
                            case AuthError.WeakPassword:
                                message = "Weak Password";
                                break;
                            case AuthError.EmailAlreadyInUse:
                                message = "Email Already In Use";
                                break;
                        }

                        warningUI.ChangeWarningText(message);
                    }
                }
                else
                {
                    //User has now been created
                    //Now get the result
                    _user = registerTask.Result;

                    if (_user != null)
                    {
                        //Create a user profile and set the username
                        StartCoroutine(LoadUserData());
                        // StartCoroutine(LoadScoreBoardData());
                        PlayerPrefs.SetString(_passwordKey, passwordInput.password);
                        PlayerPrefs.SetString(_emailKey, emailInput.email);
                        //TODO: OpenMainMenu
                        openMainMenu.Invoke();
                        
                    }
                }
            }
        }

        private IEnumerator UpdateUsernameAuth(string username)
        {
            UserProfile profile = new UserProfile() { DisplayName = username };
            Debug.Log(_user);
            var profileTask = _user.UpdateUserProfileAsync(profile);
            yield return new WaitUntil(predicate: () => profileTask.IsCompleted);
            if (profileTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {profileTask.Exception}");
            }
        }

        private IEnumerator UpdateUsernameDatabase(string username)
        {
            var databaseTask = _dataBaseReference.Child("users")
                .Child(_user.UserId)
                .Child("username")
                .SetValueAsync(username);
            yield return new WaitUntil(predicate: () => databaseTask.IsCompleted);
            if (databaseTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {databaseTask.Exception}");
            }

            _isSavingData = false;
        }

        private IEnumerator LoadUserData()
        {
            var databaseTask = _dataBaseReference.Child("users").Child(_user.UserId).GetValueAsync();
            yield return new WaitUntil(predicate: () => databaseTask.IsCompleted);
            if (databaseTask.Exception != null)
            {
                Debug.LogWarning($"Failed to register task with {databaseTask.Exception}");
            }
            else if (databaseTask.Result.Value == null)
            {
                //No Data exist
                //TODO: Set rating
                // ScoreManager.Instance.SetBestScore(0);
            }
            else
            {
                DataSnapshot dataSnapshot = databaseTask.Result;
                var bestScore = dataSnapshot.Child("bestScore").Value;
                //TODO: Set rating
                // ScoreManager.Instance.SetBestScore(int.Parse(bestScore.ToString()));
            }
        }

        private IEnumerator LoadScoreBoardData()
        {
            var databaseTask = _dataBaseReference.Child("users").OrderByChild("bestScore").GetValueAsync();
            yield return new WaitUntil(predicate: () => databaseTask.IsCompleted);
            if (databaseTask.Exception != null)
            {
                Debug.LogWarning($"Failed to register task with{databaseTask.Exception}");
            }
            else
            {
                DataSnapshot dataSnapshot = databaseTask.Result;
                //TODO: Destroy all LeaderBoard elements
                // UIManager.Instance.DestroyAllScoreBoardItems();
                foreach (DataSnapshot childSnapshot in dataSnapshot.Children.Reverse<DataSnapshot>())
                {
                    string username = childSnapshot.Child("username").Value.ToString();
                    int bestScore = int.Parse(childSnapshot.Child("bestScore").Value.ToString());
                    //TODO: Add LeaderBoard element
                    // UIManager.Instance.AddScoreBoardItem(username, bestScore);
                }
            }
        }

        #endregion


        //TODO: Make updating for rating
        // private IEnumerator UpdateBestScore(int bestScore)
        // {
        //     var databaseTask = _dataBaseReference.Child("users").Child(_user.UserId).Child("bestScore")
        //         .SetValueAsync(bestScore);
        //     yield return new WaitUntil(predicate: () => databaseTask.IsCompleted);
        //     if (databaseTask.Exception != null)
        //     {
        //         Debug.LogWarning($"Failed to register task with {databaseTask.Exception}");
        //     }
        //
        //     _isSavingData = false;
        // }
    }
}