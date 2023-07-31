namespace Rocky.Models.ViewModels
{
    public class DetailsVM
    {
        public DetailsVM()
        {
            
        }

        public Product Product { get; set; }
        public bool ExistInCart { get; set; }
    }
}
