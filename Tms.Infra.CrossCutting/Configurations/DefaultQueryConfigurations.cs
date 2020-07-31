namespace Tms.Infra.CrossCutting.Configurations
{
    public class DefaultQueryConfigurations
    {
        public int LimitForSelectTop { get; private set; }
        public bool SelectTopDescending { get; private set; }
    }
}