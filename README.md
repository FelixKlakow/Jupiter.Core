# Jupiter.Core
Open-Source dependency property system

[![jupiter MyGet Build Status](https://www.myget.org/BuildSource/Badge/jupiter?identifier=5686f341-6cb3-48fa-a0f9-7e9a05d548b3)](https://www.myget.org/)

## Features
- Properties
  - normal, read-only, attached, attached + read-only
  - Handlers and values all use generics to avoid unnecessary casts
  - Can have a DependencyExtension attached for value manipulation.
  - Supports the following features
    - Value-coercing: Coerces the value of a property
    - Value-validation: Validates the value before changing it
    - Property-changing: Executes code before the value is going to change
    - Property-changed: Executes code when the value has been changed
- Property memory consumption reduced as only non-default values require storing of the actual value.
- IDependencyObject allows implementation when you cannot modify the base object.

## Upcomming features:

- Multithreading-support ( access checks are supported already by inheriting from DependencyObjectContainer/DependencyObject and overwriting "CheckPropertyAccess" )
- Property-Bindings
- Serialization-Deserialization
- Animations

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

Reacting to changes of a dependency-property:

```C#
public void CreateSample()
{
    Sample sample = new Sample();
    sample.AddChangeHandler(Sample.NameProperty, Sample_NameChanged);
    sample.Name = "Jonny"; // Prints "Jonny"
    
    // Remove the change handler
    sample.RemoveChangeHandler(Sample.NameProperty, Sample_NameChanged);
    
    // Add a change handler which reacts to all property changes
    sample.PropertyChanged += Sample_PropertyChanged;
    sample.Name = "Jonny"; // No change -> Nothing happens
    sample.Name = "Dude"; // Prints "Name: Dude"
}

void Sample_NameChanged(IDependencyObject sender, PropertyChangedEventArgs<String> args)
{
    // Called when the property has been changed
    Console.WriteLine(args.NewValue);
}

void Sample_PropertyChanged(IDependencyObject sender, PropertyChangedEventArgs args)
{
    // Called when any property has been changed
    Console.WriteLine($"{args.Property.Name} {args.NewValue}");
}
```

Property-validation ( ensures "Name" is never null ):
```C#
public class ValidationSample : DependencyObject
{
    public static readonly DependencyProperty<String> NameProperty = DependencyProperty.Create<ValidationSample, String>(p => p.Name, String.Empty, validation:(s, v) => v != null);

    public String Name
    {
        get => GetValue(NameProperty);
        set => SetValue(NameProperty, value);
    }
}
```

Property-coercion ( forces the "Age" to be in the range between 0 and 200, the base value still can be different ):
```C#
public class CoerceSample : DependencyObject
{
    public static readonly DependencyProperty<Int32> AgeProperty = DependencyProperty.Create<Sample, String>(p => p.Age, 0, coerceValue:(s, v) => Math.Min(200, Math.Max(0, v)));

    public Int32 Age
    {
        get => GetValue(AgeProperty);
        set => SetValue(AgeProperty, value);
    }
}
```


Author:
Felix Klakow
