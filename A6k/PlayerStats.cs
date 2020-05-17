using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Text;

namespace A6k
{
    

    class PlayerStats
    {
        public int whiteMana;
        public int blueMana;
        public int redMana;
        public int blackMana;
        public int greenMana;


        public int maxblueMana;
        public int maxredMana;
        public int maxblackMana;
        public int maxgreenMana;
        public int maxwhiteMana;

        private int life;
        public int maxlife;
        public void addLife(int amount)
        {
            life += amount;
            StatsChanged?.Invoke(this, new StatChangeArgs(StatType.Life, life, amount, maxlife));
            Console.WriteLine(life);
        }
        public void setLife(int currentAmount, int maxAmount)
        {
            life = currentAmount;
            maxlife = maxAmount;
            StatsChanged?.Invoke(this, new StatChangeArgs(StatType.Life, life, 0, maxlife));

        }

        public event EventHandler<StatChangeArgs> StatsChanged;

        

        
    }

    public enum StatType
    {
        Life,
        WhiteMana,
        BlueMana,
        RedMana,
        BlackMana,
        GreenMana
    }

    public class StatChangeArgs : EventArgs
    {

        public StatChangeArgs(StatType statType, int currentValue, int changeAmount, int maxValue)
        {
            this.statType = statType;
            this.currentValue = currentValue;
            this.changeAmount = changeAmount;
            this.maxValue = maxValue;
        }

        public StatType statType { get; set; }
        public int currentValue { get; set; }

        public int changeAmount { get; set; }
        public int maxValue { get; set; }
    }
}
