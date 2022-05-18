using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using UnityEngine.UI;
using System;

public class DatabaseManager : MonoBehaviour
{
    public Text Distancia;
    public Text Coins;

    public Text HightScore;
    public Text NameText;

    int CoinsDataBase = 0;
    int CoinsInGame = 0;

    int DistanceDataBase = 0;
    int DistanceInGame = 0;

    private string userID;
    private DatabaseReference dbReference;

    public TimerScript timerscript;

    private string UserGoogleEmail = "kkkMhallmza@gmagil.com"; // EMAIL DE GOOGLE FALTA CONFIGURAR
    private string UserGoogleUID = "352151231254ewr";  // ID DE GOOGLE FALTA CONFIGURAR

    void Start()
    {    
        FirebaseDatabase database = FirebaseDatabase.GetInstance("https://final-project-406f7-default-rtdb.firebaseio.com");
        //userID = SystemInfo.deviceUniqueIdentifier;
        userID = UserGoogleUID;

        dbReference = FirebaseDatabase.DefaultInstance.RootReference;

        Debug.Log("Start");
        SearchUserExist();

    }

    public void SearchUserExist()
    {
        StartCoroutine(GetUser((string nameDB) =>
        {
            // si existeix el correu electronic 

            if (UserGoogleUID == nameDB.ToString())
            {
                NameText.text = "Exist " + nameDB;

            }
            // si no existeic, crear usuari nou
            else
            {
                NameText.text = "No Exist " + nameDB;
                CreateUser();
            }

        }));
    }

    // Crear Usuari nomes un cop i si userid no existeix falta funcinalitat
    public void CreateUser()
    {
        User newUser = new User(DistanceInGame, CoinsInGame, UserGoogleEmail, userID);
        string json = JsonUtility.ToJson(newUser);

        dbReference.Child("users").Child(userID).SetRawJsonValueAsync(json);
    }

    public IEnumerator GetUser(Action<string> onCallback)
    {
        var userNameData = dbReference.Child("users").Child(userID).Child("UID_Google").GetValueAsync();

        yield return new WaitUntil(predicate: () => userNameData.IsCompleted);

        if (userNameData != null)
        {
            DataSnapshot snapshot = userNameData.Result;

            onCallback.Invoke(snapshot.Value.ToString());
        }
    }


    public IEnumerator GetDistance(Action<int> onCallback)
    {
        var userDistanceData = dbReference.Child("users").Child(userID).Child("distancia").GetValueAsync();

        yield return new WaitUntil(predicate: () => userDistanceData.IsCompleted);

        if (userDistanceData != null)
        {
            DataSnapshot snapshot = userDistanceData.Result;

            onCallback.Invoke(int.Parse(snapshot.Value.ToString()));
        }
    }

    public IEnumerator GetGold(Action<int> onCallback)
    {
        var userCoinData = dbReference.Child("users").Child(userID).Child("coins").GetValueAsync();

        yield return new WaitUntil(predicate: () => userCoinData.IsCompleted);

        if (userCoinData != null)
        {
            DataSnapshot snapshot = userCoinData.Result;

            onCallback.Invoke(int.Parse(snapshot.Value.ToString()));
        }
    }



    // Update Database 
    public void UpdateAllData()
    {

        CoinsInGame = int.Parse(Coins.text); // Diners en la partida

        // GET COINS from DB 
        StartCoroutine(GetGold((int gold) =>
        {
            CoinsDataBase = gold + CoinsInGame; // els diners de DB + els diners recollits en la partida

            dbReference.Child("users").Child(userID).Child("coins").SetValueAsync(CoinsDataBase);

        }));

        StartCoroutine(GetDistance((int distancia) =>
        {
            // la puntuacio que tenim guardat en BD
            DistanceDataBase = distancia;

            // la puntuacio que anem fent durant la partida que agafem del script TimeScript
            DistanceInGame = timerscript.dbTimeCompare;


            if (DistanceDataBase < DistanceInGame)
            {
                //Update en BD si la puntuacio que hem fet es mes gran que el que tenim guardat
                dbReference.Child("users").Child(userID).Child("distancia").SetValueAsync(DistanceInGame);

                // Canvas HighScore
                HightScore.text = "New HighScore: " + DistanceInGame.ToString() + "m";

            }

            else
            {
                // Canvas HighScore
                HightScore.text = "HighScore: " + DistanceDataBase.ToString() + "m";
            }

        }));

    }



}

