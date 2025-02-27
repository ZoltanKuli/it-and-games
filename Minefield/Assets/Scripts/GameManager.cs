﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField]
    private MainCamera mainCamera;

    [SerializeField]
    private UIManager uIManager;

    [SerializeField]
    private InputManager inputManager;

    [SerializeField]
    private WorldManager worldManager;

    private bool isRunning;
    private bool isPaused;

    [SerializeField]
    private int playersBalance;
    [SerializeField]
    private int minimumPlayersBalance;

    private Entrance entrance;

    [SerializeField]
    private int initialNPCSpawnDelaySeconds;

    [SerializeField]
    private int entranceCost;

    [SerializeField]
    private int maxNumberOfNPCSEnteringAtATimeMinimum;
    [SerializeField]
    private int maxNumberOfNPCSEnteringAtATimeMaximum;
    private int maxNumberOfNPCSEnteringAtATime;
    [SerializeField]
    private int minimumSecondsBetweenNPCSNotEntering;
    [SerializeField]
    private int maximumSecondsBetweenNPCSNotEntering;
    private DateTime newNPCsNotBeingAbleToEnterUntilTime;
    private int npcsEnteredSinceLastNewNPCsNotBeingAbleToEnterUntilTime;
    [SerializeField]
    private int npcSpawnChance;

    [SerializeField]
    public int cleanerStationCostPerGameCycle;
    [SerializeField]
    public int mechanicStationCostPerGameCycle;
    [SerializeField]
    private int secondsBetweenCycles;
    private DateTime nextCycleTime;

    private List<NPC> npcs;
    private float averageNPCSatisfaction;
    private float averageNPCThirst;
    private float averageNPCHunger;
    [SerializeField]
    private float minimumAverageNPCSatisfactionOfNPCsSpawn;
    private int maximumNPCNumber;
    [SerializeField]
    private GameObject stevePrefab;
    [SerializeField]
    private float npcDistancePrecision;
    [SerializeField]
    private int npcDefaultSatisfactionMinimum;
    [SerializeField]
    private int npcDefaultSatisfactionMaximum;
    [SerializeField]
    private int npcMinimumSatisfactionOfStayingMinimum;
    [SerializeField]
    private int npcMinimumSatisfactionOfStayingMaximum;
    [SerializeField]
    private int npcDefaultThirstMinimum;
    [SerializeField]
    private int npcDefaultThirstMaximum;
    [SerializeField]
    private int npcMaximumThirstOfNotNeedingToDrinkMinimum;
    [SerializeField]
    private int npcMaximumThirstOfNotNeedingToDrinkMaximum;
    [SerializeField]
    private int npcDefaultHungerMinimum;
    [SerializeField]
    private int npcDefaultHungerMaximum;
    [SerializeField]
    private int npcMaximumHungerOfNotNeedingToEatMinimum;
    [SerializeField]
    private int npcMaximumHungerOfNotNeedingToEatMaximum;
    [SerializeField]
    private float npcMinimumSpeed;
    [SerializeField]
    private float npcMaximumSpeed;
    [SerializeField]
    private float npcRotationSpeedMultiplier;
    [SerializeField]
    private int secondsUntilThirstGrowthMinimum;
    [SerializeField]
    private int secondsUntilThirstGrowthMaximum;
    [SerializeField]
    private int thirstGrowthAmountMinimum;
    [SerializeField]
    private int thirstGrowthAmountMaximum;
    [SerializeField]
    private int secondsUntilHungerGrowthMinimum;
    [SerializeField]
    private int secondsUntilHungerGrowthMaximum;
    [SerializeField]
    private int hungerGrowthAmountMinimum;
    [SerializeField]
    private int hungerGrowthAmountMaximum;
    [SerializeField]
    private float loweringDistanceOnInvisibility;
    [SerializeField]
    private int thirstDecreaseDissatisfactionAmountMinimum;
    [SerializeField]
    private int thirstDecreaseDissatisfactionAmountMaximum;
    [SerializeField]
    private int hungerDecreaseDissatisfactionAmountMinimum;
    [SerializeField]
    private int hungerDecreaseDissatisfactionAmountMaximum;
    [SerializeField]
    private int minimumSecondsUntilGarbageDisposal;
    [SerializeField]
    private int maximumSecondsUntilGarbageDisposal;
    [SerializeField]
    private int garbageDecreaseDissatisfactionAmountMinimum;
    [SerializeField]
    private int garbageDecreaseDissatisfactionAmountMaximum;
    [SerializeField]
    private float npcSatisfactionDivisorWhenInStructure;

    private System.Random random;

    /// <summary>
    /// Fill up the menus with their actions.
    /// </summary>
    private void Start() {
        uIManager.AssignMethodToOnBuildRoadAction(BuildNewRoad);
        uIManager.AssignMethodToOnDestroyAction(Destroy);

        uIManager.AssingMethodToOnCafeBuildAction(BuildNewCafe);
        uIManager.AssingMethodToOnCafeRestaurantBuildAction(BuildNewCafeRestaurant);

        uIManager.AssingMethodToOnHotdogCarBuildAction(BuildNewHotdogCar);
        uIManager.AssingMethodToOnKfcRestaurantBuildAction(BuildNewKfcRestaurant);
        uIManager.AssingMethodToOnOlivegardenRestaurantBuildAction(BuildNewOlivegardensRestaurant);
        uIManager.AssingMethodToOnTaverneRestaurantBuildAction(BuildNewTaverneRestaurant);

        uIManager.AssingMethodToOnCircusTentBuildAction(BuildNewCircusTent);
        uIManager.AssingMethodToOnLondonEyeBuildAction(BuildNewLondonEye);
        uIManager.AssingMethodToOnMerryGoRoundBuildAction(BuildNewMerryGoRound);
        uIManager.AssingMethodToOnRollerCoasterBuildAction(BuildNewRollerCoaster);

        uIManager.AssingMethodToOnParkBasicBuildAction(BuildNewParkBasic);
        uIManager.AssingMethodToOnParkFountainBuildAction(BuildNewParkFountain);
        uIManager.AssingMethodToOnParkHelicopterBuildAction(BuildNewParkHelicopter);

        uIManager.AssingMethodToOnGarbageCanBuildAction(BuildNewGarbageCan);

        uIManager.AssingMethodToCleanerStationBuildAction(BuildNewCleanerStation);
        uIManager.AssingMethodToMechanicStationBuildAction(BuildNewMechanicStation);

        entrance = worldManager.GetEntrance();

        mainCamera.transform.position = new Vector3(entrance.GetOrigoPosition().x,
            mainCamera.transform.position.y, entrance.GetOrigoPosition().z);

        npcs = new List<NPC>();

        averageNPCSatisfaction = 100;
        averageNPCThirst = 100;
        averageNPCHunger = 100;

        maximumNPCNumber = worldManager.GetMaximumNPCNumberBasedOnWorldMatrixSize();

        nextCycleTime = DateTime.UtcNow.AddSeconds(secondsBetweenCycles);

        random = new System.Random();

        maxNumberOfNPCSEnteringAtATime = random.Next(maxNumberOfNPCSEnteringAtATimeMinimum, maxNumberOfNPCSEnteringAtATimeMaximum);
        newNPCsNotBeingAbleToEnterUntilTime = DateTime.UtcNow;
        npcsEnteredSinceLastNewNPCsNotBeingAbleToEnterUntilTime = 0;

        isRunning = true;
        isPaused = false;
    }

    /// <summary>
    /// Reset the Mouse click to the BuildNewCafe Action.
    /// </summary>
    private void BuildNewCafe() {
        ResetMouseActionsAndAssignMethodToOnMouseClickAction(worldManager.BuildNewCafe);
    }

    /// <summary>
    /// Reset the Mouse click to the BuildNewCafeRestaurant Action.
    /// </summary>
    private void BuildNewCafeRestaurant() {
        ResetMouseActionsAndAssignMethodToOnMouseClickAction(worldManager.BuildNewCafeRestaurant);
    }

    /// <summary>
    /// Reset the Mouse click to the BuildNewHotdogCar Action.
    /// </summary>
    private void BuildNewHotdogCar() {
        ResetMouseActionsAndAssignMethodToOnMouseClickAction(worldManager.BuildNewHotdogCar);
    }

    /// <summary>
    /// Reset the Mouse click to the BuildNewKFC Action.
    /// </summary>
    private void BuildNewKfcRestaurant() {
        ResetMouseActionsAndAssignMethodToOnMouseClickAction(worldManager.BuildNewKFC);
    }

    /// <summary>
    /// Reset the Mouse click to the BuildNewOlivegardensRestaurant Action.
    /// </summary>
    private void BuildNewOlivegardensRestaurant() {
        ResetMouseActionsAndAssignMethodToOnMouseClickAction(worldManager.BuildNewOlivegardensRestaurant);
    }

    /// <summary>
    /// Reset the Mouse click to the BuildNewTaverneRestaurant Action.
    /// </summary>
    private void BuildNewTaverneRestaurant() {
        ResetMouseActionsAndAssignMethodToOnMouseClickAction(worldManager.BuildNewTaverneRestaurant);
    }

    /// <summary>
    /// Reset the Mouse click to the BuildNewRoad Action.
    /// </summary>
    private void BuildNewRoad() {
        ResetMouseActionsAndAssignMethodToOnMouseClickAction(worldManager.BuildNewRoad);
    }

    /// <summary>
    /// Reset the Mouse click to the BuildNewCircusTent Action.
    /// </summary>
    private void BuildNewCircusTent() {
        ResetMouseActionsAndAssignMethodToOnMouseClickAction(worldManager.BuildNewCircusTent);
    }

    /// <summary>
    /// Reset the Mouse click to the BuildNewLondonEye Action.
    /// </summary>
    private void BuildNewLondonEye() {
        ResetMouseActionsAndAssignMethodToOnMouseClickAction(worldManager.BuildNewLondonEye);
    }

    /// <summary>
    /// Reset the Mouse click to the BuildNewMerryGoRound Action.
    /// </summary>
    private void BuildNewMerryGoRound() {
        ResetMouseActionsAndAssignMethodToOnMouseClickAction(worldManager.BuildNewMerryGoRound);
    }

    /// <summary>
    /// Reset the Mouse click to the BuildNewRollerCoaster Action.
    /// </summary>
    private void BuildNewRollerCoaster() {
        ResetMouseActionsAndAssignMethodToOnMouseClickAction(worldManager.BuildNewRollerCoaster);
    }

    /// <summary>
    /// Reset the Mouse click to the BuildNewBasicPark Action.
    /// </summary>
    private void BuildNewParkBasic() {
        ResetMouseActionsAndAssignMethodToOnMouseClickAction(worldManager.BuildNewBasicPark);
    }

    /// <summary>
    /// Reset the Mouse click to the BuildNewFountainPark Action.
    /// </summary>
    private void BuildNewParkFountain() {
        ResetMouseActionsAndAssignMethodToOnMouseClickAction(worldManager.BuildNewFountainPark);
    }

    /// <summary>
    /// Reset the Mouse click to the BuildNewHelicopterPark Action.
    /// </summary>
    private void BuildNewParkHelicopter() {
        ResetMouseActionsAndAssignMethodToOnMouseClickAction(worldManager.BuildNewHelicopterPark);
    }

    /// <summary>
    /// Reset the Mouse click to the BuildNewGarbageCan Action.
    /// </summary>
    private void BuildNewGarbageCan() {
        ResetMouseActionsAndAssignMethodToOnMouseClickAction(worldManager.BuildNewGarbageCan);
    }

    /// <summary>
    /// Reset the Mouse click to the BuildNewCleanerStation Action.
    /// </summary>
    private void BuildNewCleanerStation() {
        ResetMouseActionsAndAssignMethodToOnMouseClickAction(worldManager.BuildNewCleanerStation);
    }

    /// <summary>
    /// Reset the Mouse click to the BuildNewMechanicStation Action.
    /// </summary>
    private void BuildNewMechanicStation() {
        ResetMouseActionsAndAssignMethodToOnMouseClickAction(worldManager.BuildNewMechanicStation);
    }

    /// <summary>
    /// Reset the Mouse click to the Destroy Action.
    /// </summary>
    private void Destroy() {
        ResetMouseActionsAndAssignMethodToOnMouseClickAction(worldManager.Destroy);
    }

    /// <summary>
    /// Reset the Mouse click to the Action what is given via the parameter.
    /// </summary>
    private void ResetMouseActionsAndAssignMethodToOnMouseClickAction(Action<Vector3Int> action) {
        inputManager.ResetMouseActions();
        inputManager.AssignMethodToOnMouseClickAction(action);
    }

    /// <summary>
    /// Update.
    /// </summary>
    private void Update() {
        if (isPaused) {
            return;
        }
        
        mainCamera.UpdateCameraPosition(inputManager.GetCameraMovementDirection());
        mainCamera.UpdateCameraZoomAndRotation(inputManager.GetCameraZoomDirection(), inputManager.GetCameraRotationDirection());

        if (isRunning) {
            SpawnNPC();

            UpdateNPCS();

            UpdateAverageNPCSatisfaction();
            UpdateAverageNPCThirst();
            UpdateAverageNPCHunnger();
            UpdatePlayersBalanceFromNPCSMoneyOwed();

            worldManager.UpdateCleanersAndMechanics();
            WithdrawCrewPayementsFromPlayersBalance();

            uIManager.UpdatePlayerMoneyText(GetPlayersBalance());
            uIManager.UpdateVisitorCountText(npcs.Count);
            uIManager.UpdateStatisfactionSlider(averageNPCSatisfaction);
            uIManager.UpdateFoodSlider(averageNPCHunger);
            uIManager.UpdateThirstSlider(averageNPCThirst);

            Debug.Log("NPC Number:" + npcs.Count
                + "; Player's Balance: " + playersBalance
                + "; Average Satisfaction: " + averageNPCSatisfaction
                + "; Average Thirst: " + averageNPCThirst
                + "; Average Hunger: " + averageNPCHunger);

            StopGameIfPlayersBalanceGoesInTheRedUnderTheMinimumBalanceAmount();
        }
    }

    /// <summary>
    /// Update average npc satisfaction.
    /// </summary>
    private void UpdateAverageNPCSatisfaction() {
        averageNPCSatisfaction = 0;

        if (0 < npcs.Count) {
            foreach (NPC npc in npcs) {
                averageNPCSatisfaction += npc.GetSatisfaction();
            }

            averageNPCSatisfaction /= npcs.Count;
        }
    }

    /// <summary>
    /// Update average npc thirst.
    /// </summary>
    private void UpdateAverageNPCThirst() {
        averageNPCThirst = 0;

        if (0 < npcs.Count) {
            foreach (NPC npc in npcs) {
                averageNPCThirst += npc.GetThirst();
            }

            averageNPCThirst /= npcs.Count;
        }
    }

    /// <summary>
    /// Update average npc hunger.
    /// </summary>
    private void UpdateAverageNPCHunnger() {
        averageNPCHunger = 0;

        if (0 < npcs.Count) {
            foreach (NPC npc in npcs) {
                averageNPCHunger += npc.GetHunger();
            }

            averageNPCHunger /= npcs.Count;
        }
    }

    /// <summary>
    /// Update player's balance from npcs money owed.
    /// </summary>
    private void UpdatePlayersBalanceFromNPCSMoneyOwed() {
        foreach (NPC npc in npcs) {
            IncreaseOrDecreasePlayersBalance(npc.GetMoneyOwed());
            npc.IncreaseOrDecreaseMoneyOwed(-npc.GetMoneyOwed());
        }
    }

    /// <summary>
    /// Spawn npc.
    /// </summary>
    private void SpawnNPC() {
        if (npcs.Count == 0 || (newNPCsNotBeingAbleToEnterUntilTime <= DateTime.UtcNow
            && minimumAverageNPCSatisfactionOfNPCsSpawn <= averageNPCSatisfaction
            && npcs.Count < maximumNPCNumber
            && random.Next(npcSpawnChance) == 0)) {
            npcs.Add(new NPC(stevePrefab, entrance.GetOrigoPosition(), worldManager, npcDistancePrecision,
            random.Next(npcDefaultSatisfactionMinimum, npcDefaultSatisfactionMaximum),
            random.Next(npcMinimumSatisfactionOfStayingMinimum, npcMinimumSatisfactionOfStayingMaximum),
            random.Next(npcDefaultThirstMinimum, npcDefaultThirstMaximum),
            random.Next(npcMaximumThirstOfNotNeedingToDrinkMinimum, npcMaximumThirstOfNotNeedingToDrinkMaximum),
            random.Next(npcDefaultHungerMinimum, npcDefaultHungerMaximum),
            random.Next(npcMaximumHungerOfNotNeedingToEatMinimum, npcMaximumHungerOfNotNeedingToEatMaximum),
            npcMinimumSpeed, npcMaximumSpeed, npcRotationSpeedMultiplier,
            random.Next(secondsUntilThirstGrowthMinimum, secondsUntilThirstGrowthMaximum),
            random.Next(thirstGrowthAmountMinimum, thirstGrowthAmountMaximum),
            random.Next(secondsUntilHungerGrowthMinimum, secondsUntilHungerGrowthMaximum),
            random.Next(hungerGrowthAmountMinimum, hungerGrowthAmountMaximum),
            loweringDistanceOnInvisibility,
            random.Next(thirstDecreaseDissatisfactionAmountMinimum, thirstDecreaseDissatisfactionAmountMaximum),
            random.Next(hungerDecreaseDissatisfactionAmountMinimum, hungerDecreaseDissatisfactionAmountMaximum),
            minimumSecondsUntilGarbageDisposal, maximumSecondsUntilGarbageDisposal,
            random.Next(garbageDecreaseDissatisfactionAmountMinimum, garbageDecreaseDissatisfactionAmountMaximum),
            npcSatisfactionDivisorWhenInStructure));

            IncreaseOrDecreasePlayersBalance(entranceCost);

            npcsEnteredSinceLastNewNPCsNotBeingAbleToEnterUntilTime++;

            if (npcs.Count == 1) {
                newNPCsNotBeingAbleToEnterUntilTime = DateTime.UtcNow.AddSeconds(initialNPCSpawnDelaySeconds);
            }

            if (maxNumberOfNPCSEnteringAtATime <= npcsEnteredSinceLastNewNPCsNotBeingAbleToEnterUntilTime) {
                maxNumberOfNPCSEnteringAtATime = random.Next(maxNumberOfNPCSEnteringAtATimeMinimum, maxNumberOfNPCSEnteringAtATimeMaximum);
                newNPCsNotBeingAbleToEnterUntilTime = DateTime.UtcNow.AddSeconds(random.Next(minimumSecondsBetweenNPCSNotEntering,
                    maximumSecondsBetweenNPCSNotEntering));
                npcsEnteredSinceLastNewNPCsNotBeingAbleToEnterUntilTime = 0;
            }
        }
    }

    /// <summary>
    /// Update npcs.
    /// </summary>
    private void UpdateNPCS() {
        for (int i = 0; i < npcs.Count; i++) {
            NPC npc = npcs[i];
            if (!npc.IsDestroyed()) {
                npc.Update();
            } else {
                npcs.RemoveAt(i);
                i--;
            }
        }
    }

    /// <summary>
    /// Inncrease or decrease player's balance.
    /// </summary>
    public void IncreaseOrDecreasePlayersBalance(int increaseOrDecreaseAmount) {
        playersBalance += increaseOrDecreaseAmount;
    }

    /// <summary>
    /// Get player's balance.
    /// </summary>
    public int GetPlayersBalance() {
        return playersBalance;
    }

    /// <summary>
    /// Withdraw crew payements from players balance.
    /// </summary>
    private void WithdrawCrewPayementsFromPlayersBalance() {
        if (nextCycleTime <= DateTime.UtcNow) {
            playersBalance -= worldManager.GetNumberOfCleanerStations() * cleanerStationCostPerGameCycle;
            playersBalance -= worldManager.GetNumberOfMechanicStations() * mechanicStationCostPerGameCycle;

            nextCycleTime = DateTime.UtcNow.AddSeconds(secondsBetweenCycles);
        }
    }

    /// <summary>
    /// Stop game if players balance goes in the red under the minimum balance amount.
    /// </summary>
    private void StopGameIfPlayersBalanceGoesInTheRedUnderTheMinimumBalanceAmount() {
        if (playersBalance < minimumPlayersBalance && npcs.Count == 0) {
            isRunning = false;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            inputManager.ResetMouseActions();
            uIManager.OpenGameOverPanelWhenLosing();
        }
    }


    /// <summary>
    /// Pause game.
    /// </summary>
    public void PauseGame() {
        isPaused = true;
    }

    /// <summary>
    /// Continue game.
    /// </summary>
    public void ContinueGame() {
        isPaused = false;
    }
}
