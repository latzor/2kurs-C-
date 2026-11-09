using System;

namespace Очень_крутая_5_лаба__сам_делал_100__
{
    public abstract class Ingredient
    {
        public string Name;
        public double Calories;
        public int Uglevodi;
        public int Belki;

        public Ingredient(string name, double calories, int uglevodi, int belki)
        {
            this.Name = name;
            this.Calories = calories;
            this.Uglevodi = uglevodi;
            this.Belki = belki;
        }

        public abstract string GetInfo();
        public abstract string ShortInf();

        public (double calories, int uglevody, int belki) GetInfoForSort()
        {
            return (Calories, Uglevodi, Belki);
        }
    }

    public class Vegetable : Ingredient
    {
        public Vegetable(string name, double calories, int uglevodi, int belki)
            : base(name, calories, uglevodi, belki) { }

        public override string GetInfo()
        {
            return $"{Name,-15} | {"Овощ",-12} | Калории: {Calories,6:F1} | Белки: {Belki,4} | Углеводы: {Uglevodi,4}";
        }

        public override string ShortInf()
        {
            return $"{Name,-15} - {Calories,6:F1} ккал/100г";
        }
    }

    public class OtherIngredient : Ingredient
    {
        public string Category;

        public OtherIngredient(string name, double calories, int uglevodi, int belki, string category)
            : base(name, calories, uglevodi, belki)
        {
            this.Category = category;
        }

        public override string GetInfo()
        {
            return $"{Name,-15} | {Category,-12} | Калории: {Calories,6:F1} | Белки: {Belki,4} | Углеводы: {Uglevodi,4}";
        }

        public override string ShortInf()
        {
            return $"{Name,-15} [{Category}] - {Calories,6:F1} ккал/100г";
        }
    }
}
