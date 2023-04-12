using System.Collections;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;

namespace CollectionViewIsBroken;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();


		var list = new TempList();
		var collectionView = new CollectionViewEx();
		collectionView.ItemTemplate = new DataTemplate(() => new LabelTemplate());
		//collectionView.ItemsSource = new List<string>() { "a", "b", "c" };
		collectionView.ItemsSource = list;
		collectionView.SelectionMode = SelectionMode.Multiple;

		Content = collectionView;
	}

	class CollectionViewEx : CollectionView
	{
		protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);
		}
	}

	class LabelTemplate : Label
	{
		public LabelTemplate()
		{
			SetBinding(TextProperty, new Binding("."));

			VisualStateManager.SetVisualStateGroups(this,
				new VisualStateGroupList
				{
					new VisualStateGroup()
					{
						Name = nameof(VisualStateManager.CommonStates),
						States =
						{
							new VisualState
							{
								Name = VisualStateManager.CommonStates.Selected,
								Setters =
								{
									new Setter
									{
										Property = IsInSelectedStateProperty,
										Value = true
									}
								}
							},
							new VisualState
							{
								Name = VisualStateManager.CommonStates.Normal,
								Setters =
								{
									new Setter
									{
										Property = IsInSelectedStateProperty,
										Value = false
									}
								}
							}
						}
					}
				});
		}

		public static BindableProperty IsInSelectedStateProperty { get; } = BindableProperty.Create(nameof(IsInSelectedState),
			   returnType: typeof(bool), declaringType: typeof(LabelTemplate), defaultValue: false, propertyChanged: IsSelectedPropChanged);

		bool IsInSelectedState
		{
			set => (this as BindableObject)?.SetValue(IsInSelectedStateProperty, value);
			get => (this as BindableObject)?.GetValue(IsInSelectedStateProperty) is true;
		}

		private static void IsSelectedPropChanged(BindableObject bindable, object oldValue, object newValue)
		{
			// Put a breakpoint here to observe where are we called from
		}
	}

	class TempList : IList<string>, INotifyCollectionChanged
	{
		public string this[int index] { get => $"{index}"; set => throw new NotImplementedException(); }

		public int Count => 3;

		public bool IsReadOnly => true;

        public event NotifyCollectionChangedEventHandler CollectionChanged { add {} remove {} }

        public void Add(string item) => throw new NotImplementedException();

		public void Clear() => throw new NotImplementedException();

		public bool Contains(string item)
		{
			if (int.TryParse(item.ToString(), out int i) && i > 0 && i < Count)
				return true;
			return false;
		}

		public void CopyTo(string[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}

		public IEnumerator<string> GetEnumerator()
		{
			for (int i = 0; i < Count; i++)
				yield return $"{i}";
		}

		public int IndexOf(string item)
		{
			if (int.TryParse(item, out int i) && i > 0 && i < Count)
				return i;
			return -1;
		}

		public void Insert(int index, string item) => throw new NotImplementedException();

		public bool Remove(string item) => throw new NotImplementedException();

		public void RemoveAt(int index) => throw new NotImplementedException();

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}


