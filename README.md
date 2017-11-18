# Jupiter.Core
Open-Source dependency property system


## Upcomming features:

- Animations
- Property-Bindings
- Serialization-Deserialization

## Samples

Dependency-Object with two properties where one of them is read-only.

```C#
public class Sample : DependencyObject
{
    public static readonly DependencyProperty<String> NameProperty = DependencyProperty.Create<Sample, String>(p => p.Name);
    static readonly DependencyPropertyKey<DateTime> TimeOfCreationPropertyKey =  DependencyProperty.CreateReadonly<Sample, DateTime>(p => p.TimeOfCreation);
    public static readonly DependencyProperty TimeOfCreationProperty = TimeOfCreationPropertyKey.Property;
    
    public String Name
    {
        get => GetValue(NameProperty);
        set => SetValue(NameProperty, value);
    }
    
    public String TimeOfCreation
    {
        get => GetValue(eOfCreationProperty);
        private set => SetValue(eOfCreationPropertyKey, value);
    }
    
    public Sample() => TimeOfCreation = DateTime.Now;
}
```


You can use the dependency property system in existing projects which already provide base classes without modifying those. To do so you've to implement the IDependencyObject interface.

```C#
public abstract class MvvmDependencyObject : MvvmBase, IDependencyObject
{
    readonly DependencyObjectContainer _DependencyObjects;
    
    public MvvmDependencyObject() => _DependencyObjects = new DependencyObjectContainer(this);
    
    #region IDependencyObject Implementation
    // Implement IDependencyObject here
    #endregion
}
```

Author:
Felix Klakow
