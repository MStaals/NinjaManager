namespace NinjaManagerProg5.Models
{
    public class NinjaEquipment
    {
        public int NinjaId { get; set; }
        public Ninja Ninja { get; set; }         

        public int EquipmentId { get; set; }
        public Equipment Equipment { get; set; }  
    }
}