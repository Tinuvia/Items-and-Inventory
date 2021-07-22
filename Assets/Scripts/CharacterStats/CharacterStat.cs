using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Tinuvia.CharacterStats
{
    [Serializable]
    public class CharacterStat
    {
        public float BaseValue;


        public virtual float Value
        {
            get
            {
                if (isDirty || BaseValue != lastBaseValue) //only recalculate if changes have been made
                {
                    lastBaseValue = BaseValue;
                    _value = CalculateFinalValue();
                    isDirty = false;
                }
                return _value;
            }
        }

        protected bool isDirty = true;
        protected float _value;
        protected float lastBaseValue = float.MinValue;


        protected readonly List<StatModifier> statModifiers;
        // readonly means we can only modify this when we are in constructor of the class or in the declaration itself
        // can add and remove elements in the list itself
        public readonly ReadOnlyCollection<StatModifier> StatModifiers; // we don't want to change which reference this points to

        public CharacterStat()
        // must have a parameter-less constructor that initializes both Char stats coll, or we get null reference
        {
            statModifiers = new List<StatModifier>();
            StatModifiers = statModifiers.AsReadOnly();
        }

        public CharacterStat(float baseValue) : this()
        {
            BaseValue = baseValue;
        }

        public virtual void AddModifier(StatModifier mod)
        {
            isDirty = true;
            statModifiers.Add(mod);
            statModifiers.Sort(CompareModifierOrder);
        }

        protected virtual int CompareModifierOrder(StatModifier a, StatModifier b)
        // ensures that the stat modifiers are sorted flats first and percentages last
        {
            if (a.Order < b.Order)
                return -1;
            else if (a.Order > b.Order)
                return 1;
            return 0; // if (a.order == b.order)

        }

        public virtual bool RemoveModifier(StatModifier mod)
        {
            if (statModifiers.Remove(mod))
            {
                isDirty = true;
                return true;
            }
            return false;
        }

        public virtual bool RemoveAllModifiersFromSource(object source)
        // traversing in reverse so that elements aren't shuffled during removal
        {
            bool didRemove = false;

            for (int i = statModifiers.Count - 1; i >= 0; i--)
            {
                if (statModifiers[i].Source == source)
                {
                    isDirty = true;
                    didRemove = true;
                    statModifiers.RemoveAt(i);
                }
            }

            return didRemove;
        }

        protected virtual float CalculateFinalValue()
        {
            float finalValue = BaseValue;
            float sumPercentAdd = 0;

            for (int i = 0; i < statModifiers.Count; i++)
            {
                StatModifier mod = statModifiers[i];

                if (mod.Type == StatModType.Flat)
                {
                    finalValue += mod.Value;
                }
                else if (mod.Type == StatModType.PercentAdd)
                {
                    sumPercentAdd += mod.Value;
                    if (i + 1 >= statModifiers.Count || statModifiers[i + 1].Type != StatModType.PercentAdd)
                    {
                        finalValue *= 1 + sumPercentAdd;
                        sumPercentAdd = 0;
                    }
                }

                else if (mod.Type == StatModType.PercentMult)
                {
                    finalValue *= 1 + mod.Value; // our modifiers will be constructed as 0.1 (i.e 10%)
                }
            }

            // rounding to 4 digits for enough precision to circumvent float conversion errors
            return (float)Math.Round(finalValue, 4);
        }
    }
}
