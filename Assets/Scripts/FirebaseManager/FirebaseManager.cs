using System.Collections;
using System.Linq;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;
using UnityEngine.Events;
using VitaliyNULL.Core;
using VitaliyNULL.MenuSceneUI.LeaderBoard;
using VitaliyNULL.MenuSceneUI.LoginAndRegistration;
using WebSocketSharp;

namespace VitaliyNULL.FirebaseManager
{
    public class FirebaseManager : MonoBehaviour
    {
        #region Private Fields

        private bool _isSavingData = false;

        //Firebase variables
        private FirebaseAuth _auth;
        private FirebaseUser _user;
        private DatabaseReference _dataBaseReference;
        public bool isInitialized;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            if (FindObjectsOfType<FirebaseManager>().Length > 1)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(this);
        }

        #endregion

        #region Public Methods

        public void InitializeFirebase()
        {
            _auth = FirebaseAuth.DefaultInstance;
            _dataBaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        }

        public void AutoLogin(string email, string password, UnityAction openMainMenu, UnityAction errorAuthorization)
        {
            StartCoroutine(WaitForAutoLogin(email, password, openMainMenu, errorAuthorization));
        }

        //Function for the login button
        public void LoginButton(EmailInput emailInput, PasswordInput passwordInput, WarningUI warningUI,
            UnityAction openMainMenu, UnityAction errorAction)
        {
            //Call the login coroutine passing the email and password
            StartCoroutine(WaitForLogin(emailInput, passwordInput, warningUI, openMainMenu, errorAction));
        }

        //Function for the register button
        public void RegisterButton(EmailInput emailInput, PasswordInput passwordInput,
            ConfirmPasswordInput confirmPasswordInput, UsernameInput usernameInput, WarningUI warningUI,
            UnityAction openMainMenu, UnityAction errorAction)
        {
            //Call the register coroutine passing the email, password, and username
            StartCoroutine(WaitForRegister(emailInput, passwordInput, confirmPasswordInput, usernameInput, warningUI,
                openMainMenu, errorAction));
        }

        public void LoadLeaderBoard(LeaderBoardContent leaderBoardContent)
        {
            StartCoroutine(LoadLeaderBoardData(leaderBoardContent));
        }

        public void ExitAccount(UnityAction unityAction)
        {
            _auth.SignOut();
            PlayerPrefs.DeleteAll();
            //TODO : Reset rating
            //TODO: Open login window
            unityAction.Invoke();
        }

        public void SaveUsernameData()
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

        public void SaveRatingData(int rating)
        {
            StartCoroutine(UpdateRatingDatabase(rating));
        }

        public void LoadData()
        {
            StartCoroutine(LoadUserData());
        }

        public void LoadData(UnityAction unityAction)
        {
            StartCoroutine(LoadUserData(unityAction));
        }

        #endregion

        #region Coroutines

        private IEnumerator WaitForAutoLogin(string email, string password, UnityAction openMainMenu,
            UnityAction errorAuthorization)
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
                    errorAuthorization.Invoke();
                }
            }
            else
            {
                //User is now logged in
                //Now get the result
                _user = loginTask.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})", _user.DisplayName, _user.Email);
                SaveUsernameData();
                StartCoroutine(LoadUserData());

                // StartCoroutine(LoadScoreBoardData());
                //TODO: Open MainMenu
                openMainMenu.Invoke();
            }
        }

        private IEnumerator WaitForLogin(EmailInput emailInput, PasswordInput passwordInput, WarningUI warningUI,
            UnityAction openMainMenu, UnityAction errorAction)
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
                    errorAction.Invoke();
                    warningUI.ChangeWarningText(message);
                }
            }
            else
            {
                //User is now logged in
                //Now get the result
                _user = loginTask.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})", _user.DisplayName, _user.Email);
                SaveUsernameData();
                StartCoroutine(LoadUserData());

                // StartCoroutine(LoadScoreBoardData());
                PlayerPrefs.SetString(ConstKeys.PasswordKey, passwordInput.password);
                PlayerPrefs.SetString(ConstKeys.EmailKey, emailInput.email);
                //TODO: Open MainMenu
                openMainMenu.Invoke();
            }
        }

        private IEnumerator WaitForRegister(EmailInput emailInput, PasswordInput passwordInput,
            ConfirmPasswordInput confirmPasswordInput, UsernameInput usernameInput, WarningUI warningUI,
            UnityAction openMainMenu, UnityAction errorAction)
        {
            if (usernameInput.username.IsNullOrEmpty())
            {
                errorAction.Invoke();
                warningUI.ChangeWarningText("Username is empty");
            }
            else if (passwordInput.password != confirmPasswordInput.confirmPassword)
            {
                //If the password does not match show a warning
                errorAction.Invoke();
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

                        errorAction.Invoke();
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
                        UserProfile userProfile = new UserProfile() { DisplayName = usernameInput.username };
                        var profileTask = _user.UpdateUserProfileAsync(userProfile);
                        //Wait until the task completes
                        yield return new WaitUntil(predicate: () => profileTask.IsCompleted);
                        //Create a user profile and set the username
                        SaveUsernameData();
                        SaveRatingData(0);
                        StartCoroutine(LoadUserData());
                        // StartCoroutine(LoadScoreBoardData());
                        PlayerPrefs.SetString(ConstKeys.PasswordKey, passwordInput.password);
                        PlayerPrefs.SetString(ConstKeys.EmailKey, emailInput.email);
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

        private IEnumerator UpdateRatingDatabase(int rating)
        {
            var databaseTask = _dataBaseReference.Child("users")
                .Child(_user.UserId)
                .Child("rating")
                .SetValueAsync(rating);
            yield return new WaitUntil(predicate: () => databaseTask.IsCompleted);
            if (databaseTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {databaseTask.Exception}");
            }

            PlayerPrefs.SetInt(ConstKeys.Rating, rating);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unityAction">If need to do something when LoadUserData</param>
        /// <returns></returns>
        private IEnumerator LoadUserData(UnityAction unityAction)
        {
            var databaseTask = _dataBaseReference.Child("users").Child(_user.UserId).GetValueAsync();
            yield return new WaitUntil(predicate: () => databaseTask.IsCompleted);
            if (databaseTask.Exception != null)
            {
                Debug.LogWarning($"Failed to register task with {databaseTask.Exception}");
            }
            else if (databaseTask.Result.Value == null)
            {
                //TODO: Set rating

                PlayerPrefs.SetInt(ConstKeys.Rating, 0);
            }
            else
            {
                DataSnapshot dataSnapshot = databaseTask.Result;
                var rating = dataSnapshot.Child("rating").Value;

                //TODO: Set rating
                PlayerPrefs.SetInt(ConstKeys.Rating, int.Parse(rating.ToString()));
            }

            unityAction.Invoke();
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
                //TODO: Set rating

                PlayerPrefs.SetInt(ConstKeys.Rating, 0);
            }
            else
            {
                DataSnapshot dataSnapshot = databaseTask.Result;
                var rating = dataSnapshot.Child("rating").Value;
                //TODO: Set rating
                PlayerPrefs.SetInt(ConstKeys.Rating, int.Parse(rating.ToString()));
            }
        }

        private IEnumerator LoadLeaderBoardData(LeaderBoardContent leaderBoardContent)
        {
            var databaseTask = _dataBaseReference.Child("users").OrderByChild("rating").GetValueAsync();
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
                    string rating = childSnapshot.Child("rating").Value.ToString();
                    //TODO: Add LeaderBoard element
                    // UIManager.Instance.AddScoreBoardItem(username, bestScore);
                    leaderBoardContent.InstantiateLeaderBoardItem(username, rating);
                }
            }
        }

        #endregion
    }
}