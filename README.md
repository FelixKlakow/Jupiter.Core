# Jupiter.Core
Open-Source dependency property system

[![jupiter MyGet Build Status](https://www.myget.org/BuildSource/Badge/jupiter?identifier=5686f341-6cb3-48fa-a0f9-7e9a05d548b3)](https://www.myget.org/)

## Features
- Properties
  - Can be read-only
  - Can be attached to IDependencyObject.
  - Can have a DependencyExtension attached for value manipulation.
  - Can have the following events/handlers
    - Coerce-value: Coerces the value
    - Validation: Validates the value against custom rules
    - Property-changing
    - Property-changed
- Properties only consume memory when the property...
  - is coerced and the value is not matching the default one.
  - has a DependencyExtension assigned.
  - has a non-default value assigned.
  - has a specific change handler registered.
- IDependencyObject allows implementation when you cannot modify the base object.

## Upcomming features:

- Animations
- Property-Bindings
- Serialization-Deserialization

## Samples

Dependency-Object with two properties where one of them is read-only.

```C#
public class Sample : DependencyObject
{
    public static readonly DependencyProperty<String> NameProperty = DependencyProperty.Create<Sample, String>(p => p.Name, String.Empty);
    static readonly DependencyPropertyKey<DateTime> TimeOfCreationPropertyKey =  DependencyProperty.CreateReadonly<Sample, DateTime>(p => p.TimeOfCreation);
    public static readonly DependencyProperty TimeOfCreationProperty = TimeOfCreationPropertyKey.Property;
    
    public String Name
    {
        get => GetValue(NameProperty);
        set => SetValue(NameProperty, value);
    }
    
    public String TimeOfCreation
    {
        get => GetValue(TimeOfCreationProperty);
        private set => SetValue(TimeOfCreationPropertyKey, value);
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
