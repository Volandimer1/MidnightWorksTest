using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelState : IGameState
{
    private GameStateMachine _gameStateMachine;
    private const string LevelSceneName = "LevelScene";
    private const string PlotsBuyButtonsListName = "PlotBuyButtonsList";
    private const string LevelBuildingsSOPath = "LevelScene/Scriptables/BuildingsLists/DefaultLevel1Buildings";
    private const string ChooseBuildingCanvasPath = "LevelScene/UI/ChooseBuildingCanvas";
    private const string UICanvasPath = "LevelScene/UI/UICanvas";
    private const string LevelSFXBundlePath = "AudioBundles/Level/DefaultLevelSounds";
    private const string LevelMusicBundlePath = "AudioBundles/Level/DefaultLevelMusic";
    private const string SpawnersPath = "LevelScene/ItemsSpawners";
    private const string LevelItemsPath = "ScriptableObjects/ItemsColections/LevelItems";
    private const string ItemDropPrefabPath = "LevelScene/ItemPrefab";
    private const string InventoryCanvasPath = "LevelScene/UI/InventoryCanvas";

    private Scene levelScene;
    private Camera _camera;
    private PopUpService _popUpService;
    private AudioClipsBundle _LevelSFXBundle;
    private AudioClipsBundle _LevelMusicBundle;

    public LevelState(GameStateMachine gameStateMachine, PopUpService popUpService)
    {
        _gameStateMachine = gameStateMachine;
        _popUpService = popUpService;
    }

    public async Task EnterState()
    {
        if (await LoadLevelSceneAsync())
        {
            await PrepareNewScene();
        }
    }

    public async Task ExitState()
    {
        AsyncOperation asyncUnLoad = null;
        if (levelScene.isLoaded)
        {
            asyncUnLoad = SceneManager.UnloadSceneAsync(levelScene);
        }

        while (!asyncUnLoad.isDone)
        {
            await Task.Yield();
        }

        Resources.UnloadUnusedAssets();
    }

    private async Task<bool> LoadLevelSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(LevelSceneName, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            await Task.Yield();
        }

        if (SceneManager.GetSceneByName(LevelSceneName).isLoaded)
        {
            levelScene = SceneManager.GetSceneByName(LevelSceneName);
            SceneManager.SetActiveScene(levelScene);
            return true;
        }
        else
        {
            Debug.LogError("Failed to load the LevelScene.");
            return false;
        }
    }

    private async Task PrepareNewScene()
    {
        await Task.Yield();
        _camera = Camera.main;
        PlayerBalance playerBalance = new PlayerBalance();
        PlotsModel plotsModel = new PlotsModel();
        SavingData savingData = new SavingData();
        BuildService buildService = new BuildService();

        BuildingsSO buildingsSO = Resources.Load<BuildingsSO>(LevelBuildingsSOPath);
        if (buildingsSO == null)
        {
            Debug.LogError($"Failed to load buildingsSO from path: {LevelBuildingsSOPath}");
        }
        else
        {
            buildingsSO.Initialize(savingData);
        }

        playerBalance.Initialize(savingData);

        BuildingFactory buildingFactory = new BuildingFactory(buildingsSO);
        buildService.Initialize(plotsModel, buildingFactory);

        savingData.LoadData();
        buildingsSO.LoadFromSavingData(savingData);
        playerBalance.LoadFromSavingData(savingData);

        GameObject PlotsBuyButtonsListObject = GameObject.Find(PlotsBuyButtonsListName);
        if (PlotsBuyButtonsListObject == null)
        {
            Debug.LogError($"Failed to find {PlotsBuyButtonsListObject} in the scene.");
            return;
        }
        List<Transform> children = new List<Transform>();
        Transform parentTransform = PlotsBuyButtonsListObject.transform;
        for (int i = 0; i < parentTransform.childCount; i++)
        {
            Transform child = parentTransform.GetChild(i);
            children.Add(child);
        }

        plotsModel.Initialize(children, savingData);
        plotsModel.LoadFromSavingData(savingData);

        GameObject chooseBuildingCanvas = null;
        GameObject chooseBuildingCanvasPrefab = Resources.Load<GameObject>(ChooseBuildingCanvasPath);
        if (chooseBuildingCanvasPrefab == null)
        {
            Debug.LogError($"Failed to load chooseBuildingCanvas from path: {ChooseBuildingCanvasPath}");
        }
        else
        {
            chooseBuildingCanvas = Object.Instantiate(chooseBuildingCanvasPrefab);
            ChooseBuildingCanvasInitializer chooseBuildingCanvasInitializer = chooseBuildingCanvas.GetComponent<ChooseBuildingCanvasInitializer>();
            chooseBuildingCanvasInitializer.Initialize(buildingsSO, buildService, chooseBuildingCanvas, playerBalance);
            chooseBuildingCanvas.SetActive(false);
        }

        for (int i = 0; i < plotsModel.Plots.Count; i++)
        {
            if (plotsModel.Plots[i].IsBuyButtonAvaliable)
            {
                children[i].gameObject.SetActive(true);
                continue;
            }

            int buildingIndex = plotsModel.Plots[i].BuildingOnPlot;
            if (buildingIndex > -1)
            {
                playerBalance.AddToIncome(buildingsSO.BuildingsList[buildingIndex].MoneyIncome);
                plotsModel.Plots[i].BuildingGameObject = buildingFactory.Get(buildingIndex, plotsModel.Plots[i].SpawnPosition);
            }
        }

        for (int i = 0; i < children.Count; i++)
        {
            PlotBuyButton buyBuildingOnPlotButton = children[i].GetComponentInChildren<PlotBuyButton>();
            buyBuildingOnPlotButton.Initialize(buildService, i, children[i].position, chooseBuildingCanvas);
            plotsModel.AddPlotBuyButton(buyBuildingOnPlotButton);
        }

        _gameStateMachine.Updater.AddUpdatable(playerBalance);

        ProgressionSystem progressionSystem = new ProgressionSystem();
        progressionSystem.Initialize(playerBalance, plotsModel, savingData);
        progressionSystem.LoadFromSavingData(savingData);

        ItemsColectionSO levelItemsSO = Resources.Load<ItemsColectionSO>(LevelItemsPath);
        if (levelItemsSO == null)
        {
            Debug.LogError($"Failed to load levelItemsSO from path: {LevelItemsPath}");
            return;
        }
        levelItemsSO.Initialize(buildingsSO);
        Inventory inventory = new Inventory(levelItemsSO, savingData);
        inventory.LoadFromSavingData(savingData);

        GameObject itemDropPrefab = Resources.Load<GameObject>(ItemDropPrefabPath);
        if (itemDropPrefab == null)
        {
            Debug.LogError($"Failed to load itemDropPrefab from path: {ItemDropPrefabPath}");
            return;
        }
        ItemFactory itemFactory = new ItemFactory(itemDropPrefab, inventory, _camera, levelItemsSO);
        GameObject SpawnersPrefab = Resources.Load<GameObject>(SpawnersPath);
        if (SpawnersPrefab == null)
        {
            Debug.LogError($"Failed to load SpawnersPrefab from path: {SpawnersPath}");
        }
        else
        {
            GameObject spawners = Object.Instantiate(SpawnersPrefab);
            for (int i = 0; i < spawners.transform.childCount; i++)
            {
                Transform spawnerTransform = spawners.transform.GetChild(i);
                ItemSpawner spawner = spawnerTransform.gameObject.GetComponent<ItemSpawner>();
                spawner.Initialize(itemFactory);
            }
        }

        GameObject inventoryCanvas = null;
        CraftingSystem craftingSystem = new CraftingSystem(inventory);
        GameObject inventoryCanvasPrefab = Resources.Load<GameObject>(InventoryCanvasPath);
        if (inventoryCanvasPrefab == null)
        {
            Debug.LogError($"Failed to load inventoryCanvas from path: {InventoryCanvasPath}");
        }
        else
        {
            inventoryCanvas = Object.Instantiate(inventoryCanvasPrefab);
            InventoryCanvasInitializer inventoryCanvasInitializer = inventoryCanvas.GetComponent<InventoryCanvasInitializer>();
            inventoryCanvasInitializer.Initialize(craftingSystem, inventory);
            inventoryCanvas.gameObject.SetActive(false);
        }

        GameObject uiCanvasPrefab = Resources.Load<GameObject>(UICanvasPath);
        if (uiCanvasPrefab == null)
        {
            Debug.LogError($"Failed to load uiCanvas from path: {UICanvasPath}");
        }
        else
        {
            GameObject uiCanvas = Object.Instantiate(uiCanvasPrefab);
            LevelUIInitializer uiCanvasInitializer = uiCanvas.GetComponent<LevelUIInitializer>();
            uiCanvasInitializer.Initialize(playerBalance, _popUpService, _gameStateMachine, inventoryCanvas);
        }

        _LevelSFXBundle = Resources.Load<AudioClipsBundle>(LevelSFXBundlePath);
        if (_LevelSFXBundle == null)
        {
            Debug.LogError($"Failed to load LevelSFXBundle from path: {LevelSFXBundlePath}");
        }
        else
        {
            _gameStateMachine.AudioManager.LoadEnumClipsPairDictionaryFromBundle(_LevelSFXBundle);

        }

        _LevelMusicBundle = Resources.Load<AudioClipsBundle>(LevelMusicBundlePath);
        if (_LevelMusicBundle == null)
        {
            Debug.LogError($"Failed to load LevelMusicBundle from path: {LevelMusicBundlePath}");
        }
        else
        {
            _gameStateMachine.AudioManager.LoadEnumMusicPairDictionaryFromBundle(_LevelMusicBundle);

        }

        _gameStateMachine.AudioManager.PlayMusic(AudioClipsEnum.LevelMusic, true);

        _gameStateMachine.Updater.AddUpdatable(savingData);
    }
}