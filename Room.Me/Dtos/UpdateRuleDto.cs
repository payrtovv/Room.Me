namespace Room.Me.Dtos
{
    public class UpdateRuleDto
    {
        //La seleccionada a cambiar
        public String RuleName { get; set; }
        //la habitacion de la que es
        public int Roomid { get; set; }
        //El nuevo nombre
        public String NewRuleName { get; set; }
        //El estado de esta (si se permite o no )
        public bool Value { get; set; }
    }
}
