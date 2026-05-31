using System;
using System.Collections.Generic;
using UnityEngine;
using Things.Game.Configs.Upgrade;
using Things.Game.Models;
using Things.Game.Events;
using Things.Core.Infrastructure;
using Things.Game.Services.Currency;

namespace Things.Game.Services.Upgrade
{
    public class UpgradeService : IUpgradeService
    {
        private readonly Dictionary<string, UpgradeModel> models;
        private readonly UpgradeDatabase database;
        private ICurrencyService currencyService;

        public UpgradeService(UpgradeDatabase database)
        {
            this.database = database;
            models = new Dictionary<string, UpgradeModel>();
        }

        private ICurrencyService CurrencyService
        {
            get
            {
                if (currencyService == null)
                {
                    ServiceLocator.TryGet(out currencyService);
                }
                return currencyService;
            }
        }

        public int GetLevel(string upgradeId)
        {
            return models.TryGetValue(upgradeId, out var model) ? model.CurrentLevel : 0;
        }

        public float GetValue(string upgradeId)
        {
            var def = GetDefinition(upgradeId);
            if (def == null) return 0f;
            
            int level = GetLevel(upgradeId);
            return def.BaseValue + (def.IncrementPerLevel * level);
        }

        public bool TryPurchaseUpgrade(string upgradeId)
        {
            var def = GetDefinition(upgradeId);
            if (def == null) return false;

            int currentLevel = GetLevel(upgradeId);
            if (currentLevel >= def.MaxLevel) return false;

            int cost = Mathf.FloorToInt(def.BaseCost * Mathf.Pow(def.CostMultiplierPerLevel, currentLevel));

            var currencySvc = CurrencyService;
            if (currencySvc != null && currencySvc.HasEnough(def.CostCurrencyId, cost))
            {
                currencySvc.Subtract(def.CostCurrencyId, cost);
                
                if (!models.TryGetValue(upgradeId, out var model))
                {
                    model = new UpgradeModel(upgradeId, 0);
                    model.OnLevelChanged += (level) => OnUpgradeLeveledUp(upgradeId, level);
                    models[upgradeId] = model;
                }
                model.LevelUp();
                return true;
            }
            return false;
        }

        private void OnUpgradeLeveledUp(string upgradeId, int level)
        {
            EventBus.Publish(new UpgradeChangedEvent { UpgradeId = upgradeId, NewLevel = level });
        }

        public UpgradeDefinition GetDefinition(string upgradeId)
        {
            return database != null ? database.GetById(upgradeId) : null;
        }
    }
}
