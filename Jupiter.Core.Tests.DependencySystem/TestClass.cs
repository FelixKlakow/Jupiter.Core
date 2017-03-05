using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jupiter.Tests.DependencySystem
{
    public sealed class TestClass : DependencyObject
    {
        public static DependencyProperty<Single> TestProperty = DependencyProperty.Create<TestClass, Single>(p => p.Test, 1, validation:(s,v) => v >= 0, coerceValue:(s,v) => s.TestCoerceToggle ? Math.Max(10f, v) : v);

        public Single Test
        {
            get { return GetValue(TestProperty); }
            set { SetValue(TestProperty, value); }
        }

        public static DependencyProperty<Boolean> TestCoerceToggleProperty = DependencyProperty.Create<TestClass, Boolean>(p => p.TestCoerceToggle, propertyChanged:(s,a) => s.CoerceValue(TestProperty));

        public Boolean TestCoerceToggle
        {
            get { return GetValue(TestCoerceToggleProperty); }
            set { SetValue(TestCoerceToggleProperty, value); }
        }

        public static DependencyPropertyKey<UInt32> ReadonlyTestPropertyKey = DependencyProperty.CreateReadonly<TestClass, UInt32>(p => p.ReadonlyTest);
        public static DependencyProperty<UInt32> ReadonlyTestProperty = ReadonlyTestPropertyKey.Property;

        public UInt32 ReadonlyTest
        {
            get { return GetValue(ReadonlyTestProperty); }
            private set { SetValue(ReadonlyTestPropertyKey, value); }
        }

    }

    public class TestAttachmentClass
    {
        public static DependencyProperty<Int32> IntegerAttachmentProperty = DependencyProperty.CreateAttachment<TestAttachmentClass, IDependencyObject, Int32>("IntegerAttachment");

        public static DependencyPropertyKey<Int32> IntegerReadonlyAttachmentPropertyKey = DependencyProperty.CreateReadonlyAttachment<TestAttachmentClass, IDependencyObject, Int32>("IntegerReadonlyAttachment");
        public static DependencyProperty<Int32> IntegerReadonlyAttachmentProperty = IntegerReadonlyAttachmentPropertyKey.Property;
    }
}
