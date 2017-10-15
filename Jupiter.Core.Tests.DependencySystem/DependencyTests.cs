using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Jupiter.Reflection;

namespace Jupiter.Tests.DependencySystem
{
    [TestClass]
    public class DependencyTests
    {
        [TestMethod]
        public void BasicInitialization() => new TestClass();

        [TestMethod]
        public void DefaultValue()
        {
            TestClass test = new TestClass();
            Assert.AreEqual(1, test.Test);
        }

        [TestMethod]
        public void TestValueGetSet()
        {
            TestClass test = new TestClass()
            {
                Test = 5.1f
            };

            Assert.AreEqual(5.1f, test.Test);
        }

        [TestMethod]
        public void CoerceValueTest()
        {
            TestClass test = new TestClass
            {
                TestCoerceToggle = true
            };

            Assert.AreEqual(10f, test.Test);

            test.TestCoerceToggle = false;

            test.Test = 5.1f;
            Assert.AreEqual(5.1f, test.Test);

            test.TestCoerceToggle = true;

            test.ClearValue(TestClass.TestProperty);
            Assert.AreEqual(10f, test.Test);

        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void ValidatePropertyException() => new TestClass().Test = -1f;

        [TestMethod]
        public void PropertyEventsAddGeneric()
        {
            const Single OldValue = 5;
            const Single NewValue = 10;

            TestClass test = new TestClass();
            List<DependencyProperty> eventProperty = new List<DependencyProperty>();

            List<Single> newValueList = new List<Single>();
            List<Single> oldValueList = new List<Single>();

            test.Test = OldValue;
            GenericEventHandler<IDependencyObject, PropertyChangedEventArgs<Single>> handler = (o, p) =>
            {
                eventProperty.Add(p.Property);
                newValueList.Add(p.NewValue);
                oldValueList.Add(p.OldValue);
            };
            test.AddChangeHandler(TestClass.TestProperty, handler);
            test.Test = NewValue;
            // Check also if double events are prevented
            test.Test = NewValue;

            Assert.AreEqual(eventProperty.Count, 1);
            Assert.AreEqual(newValueList[0], NewValue);
            Assert.AreEqual(oldValueList[0], OldValue);
        }
        [TestMethod]
        public void PropertyEventsAddRemoveGeneric()
        {
            const Single OldValue = 5;
            const Single NewValue = 10;

            TestClass test = new TestClass();
            List<DependencyProperty> eventProperty = new List<DependencyProperty>();

            List<Single> newValueList = new List<Single>();
            List<Single> oldValueList = new List<Single>();

            test.Test = OldValue;
            GenericEventHandler<IDependencyObject, PropertyChangedEventArgs<Single>> handler = (o, p) =>
            {
                eventProperty.Add(p.Property);
                newValueList.Add(p.NewValue);
                oldValueList.Add(p.OldValue);
            };
            test.AddChangeHandler(TestClass.TestProperty, handler);
            test.RemoveChangeHandler(TestClass.TestProperty, handler);
            test.Test = NewValue;


            Assert.AreEqual(eventProperty.Count, 0);
        }

        [TestMethod]
        public void PropertyExtension()
        {
            TestClass test = new TestClass();

            Single previousValue = test.Test;
            test.SetValueExtension(TestClass.TestProperty, new TestValueExtension(20));
            Assert.AreEqual(test.Test, 20);
            test.SetValueExtension(TestClass.TestProperty, null);
            Assert.AreEqual(test.Test, previousValue);
        }

        [TestMethod]
        public void PropertyExtensionOverride()
        {
            TestClass test = new TestClass();

            Single previousValue = test.Test;
            test.SetValueExtension(TestClass.TestProperty, new TestValueExtension(20));
            Assert.AreEqual(test.Test, 20);
            test.SetValueExtensionOverride(TestClass.TestProperty, new TestValueExtension(40));
            Assert.AreEqual(test.Test, 40);

            test.SetValueExtension(TestClass.TestProperty, null);
            Assert.AreEqual(test.Test, 40);
            test.SetValueExtensionOverride(TestClass.TestProperty, null);


            Assert.AreEqual(test.Test, previousValue);
        }

        [TestMethod]
        public void AttachmentProperty()
        {
            TestClass test = new TestClass();
            test.SetValue(TestAttachmentClass.IntegerAttachmentProperty, 4);
            Assert.AreEqual(test.GetValue(TestAttachmentClass.IntegerAttachmentProperty), 4);
        }
        [TestMethod]
        public void AttachmentPropertyReadonly()
        {
            TestClass test = new TestClass();
            test.SetValue(TestAttachmentClass.IntegerReadonlyAttachmentPropertyKey, 4);
            Assert.AreEqual(test.GetValue(TestAttachmentClass.IntegerReadonlyAttachmentProperty), 4);
        }
        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void AttachmentPropertyReadonlyException()
        {
            TestClass test = new TestClass();
            test.SetValue(TestAttachmentClass.IntegerReadonlyAttachmentProperty, 4);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void AttachmentPropertyNameException()
        {
            DependencyProperty.CreateAttachment<TestAttachmentClass, IDependencyObject, Int32>("$InvalidName123");
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void PropertyReadonlyException()
        {
            TestClass test = new TestClass();
            test.SetValue(TestClass.ReadonlyTestProperty, 4);
        }

        [TestMethod]
        public void SharedReflectionTest()
        {
            SharedReflectionManager manager = new SharedReflectionManager();
            SharedTypeInfo info = manager.GetInfo<TestClass>();
            IReadOnlyList<SharedPropertyInfo> declaredProperties = info.DeclaredProperties;
            Assert.IsNotNull(declaredProperties.FirstOrDefault(p => p.DependencyProperty == TestClass.ReadonlyTestProperty));
            Assert.IsNotNull(declaredProperties.FirstOrDefault(p => p.DependencyProperty == TestClass.TestCoerceToggleProperty));
            Assert.IsNotNull(declaredProperties.FirstOrDefault(p => p.DependencyProperty == TestClass.TestProperty));
        }
    }
}
