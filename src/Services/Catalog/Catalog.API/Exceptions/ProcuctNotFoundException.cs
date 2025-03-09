using BuildingBlock.Exceptions;

namespace Catalog.API.Exceptions
{
    public class ProcuctNotFoundException : NotFoundException
    {
        public ProcuctNotFoundException(Guid Id) : base("Product", Id)
        {
            
        }
    }
}
