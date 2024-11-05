namespace NinjaManagerProg5.Models
{
    public class Equipment
    {
        public Equipment()
        {
            
        }

        public Equipment(int id, string name, string category, int strength, int intelligence, int agility,
            int goldValue)
        {
            this.Id = id;
            this.Name = name;
            this.Category = category;
            this.Strength = strength;
            this.Intelligence = intelligence;
            this.Agility = agility;
            this.GoldValue = goldValue;
            
        }
        public int Id { get; set; }
        public string Name { get; set; }       
        public string Category { get; set; }
        public int Strength { get; set; }       
        public int Intelligence { get; set; }   
        public int Agility { get; set; }        
        public int GoldValue { get; set; }      

        public ICollection<NinjaEquipment> NinjaEquipments { get; set; }
    }
}