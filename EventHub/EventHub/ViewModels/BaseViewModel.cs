using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EventHub.ViewModels;

public class BaseViewModel : INotifyPropertyChanged
{
	public event PropertyChangedEventHandler PropertyChanged;

	protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
	{
		var changed = PropertyChanged;
		if (changed == null)
			return;

		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}