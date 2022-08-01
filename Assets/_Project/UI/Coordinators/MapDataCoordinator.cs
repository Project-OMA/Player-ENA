using System.Collections.Generic;
using System.Threading.Tasks;
using ENA.Maps;
using ENA.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ENA.UI
{
    public class MapDataCoordinator: UICoordinator
    {
        #region Variables
        MapService mapService;
        [Header("References")]
        [SerializeField] MapInfoDisplay mapInfoDisplay;
        [SerializeField] UIList mapList;
        [Header("Data")]
        MapData[] loadedData;
        MapData currentMap;
        #endregion
        #region UICoordinator Implementation
        public override void Setup()
        {
            mapService = manager.Get<MapService>();
        }
        #endregion
        #region Methods
        public Task<MapData[]> GetMapsFor(ENAProfile profile)
        {
            if (profile != null)
                return mapService.FetchMaps(profile);
            else 
                return Task.FromResult(new MapData[0]);
        }

        public void Load()
        {
            PlayerPrefs.SetString(OptionsPlayer.LoadedMapKey, currentMap.FolderPath);
            UAP_AccessibilityManager.Say(mapInfoDisplay.LoadingMessage, false);
            SceneManager.LoadSceneAsync(BuildIndex.GameplayScene);
        }

        public void ShowMapInfo(int index)
        {
            currentMap = loadedData[index];
            manager.Push(mapInfoDisplay);
            mapInfoDisplay.SetMapData(currentMap);
        }

        private void Sync(MapData[] data, UIList list)
        {
            list.Resize(data.Length);

            var cells = new List<MapDataDisplay>();
            cells.AddRange(list.GetComponentsInChildren<MapDataDisplay>());

            var updateAmount = Mathf.Min(data.Length, cells.Count);
            for (int i = 0; i < updateAmount; i++) {
                var cell = cells[i];
                cell.SetMapData(data[i]);
                cell.SetBind(ShowMapInfo);
            }
        }

        public async void SyncData(ENAProfile profile)
        {
            var data = await GetMapsFor(profile);
            loadedData = data;
            Sync(loadedData, mapList);
        }
        #endregion
    }
}