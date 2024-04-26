namespace EventHub.Models
{
    public class ViewModelBase
    {
		public virtual Task InitializeAsync(object navigationData)
		{
			return Task.FromResult(false);
		}
	}
}
