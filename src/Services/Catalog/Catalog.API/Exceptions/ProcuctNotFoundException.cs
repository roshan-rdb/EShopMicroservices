namespace Catalog.API.Exceptions
{
    public class ProcuctNotFoundException : Exception
    {
        public ProcuctNotFoundException() : base("Product not found!")
        {
            
        }
    }
}
