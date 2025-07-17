namespace Client 
{
    struct SendShootEvent 
    {
        public int WeaponIndex;
        public string SenderEntityKey;
        public string TargetEntityKey; 
    }
}