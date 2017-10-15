# Jupiter.Core
Open-Source dependency property system


## Upcomming features:

- Animations
- Property-Bindings
- Serialization-Deserialization

## Samples

```C#
public class Sample : DependencyObject
{
    public static readonly DependencyProperty<String> NameProperty = DependencyProperty.Create<Sample, String>(p => p.Name);
    
    public String Name
    {
        get => GetValue(NameProperty);
        set => SetValue(NameProperty, value);
    }
}
```


Author:
Felix Klakow
