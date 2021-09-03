using System;
using System.Collections.Generic;
using Assets.Scripts.Utilities.Saving.Saver;
using UnityEngine;

namespace Assets.Scripts.Utilities.Saving
{
    public class SaveController : MonoBehaviour
    {
        private static IAsyncSaver Saver;

        private static HashSet<ISaveable> Saveables;


        private void Awake()
        {
            Saveables = new HashSet<ISaveable>();

            Saver = new JsonSaver();
        }

        private void Start()
        {
            var saveables = UnityObjectHelper.FindObjectsOfType<ISaveable>();

            foreach (var saveable in saveables) Saveables.Add(saveable);
        }


        public static void AddSaveable(ISaveable saveable) => Saveables.Add(saveable);


        public async void OnSave()
        {
            var gameData = new GameData();

            foreach (var saveable in Saveables)
            {
                gameData.ItemDatas.Add(saveable.GetItemData());
            }


            Debug.Log("Saving save data...");
            try
            {
                await Saver.SaveAsync(gameData);

                Debug.Log("Saved");
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        public async void OnLoad()
        {
            var gameData = new GameData();
            Debug.Log($"Loading save data...");
            try
            {
                gameData = await Saver.LoadAsync();

                Debug.Log("Loaded");
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
            finally
            {
                foreach (var itemData in gameData.ItemDatas)
                {
                    foreach (var saveable in Saveables)
                    {
                        if (itemData.Id == saveable.Id) saveable.SetItemData(itemData);
                    }
                }
            }
        }
    }
}
