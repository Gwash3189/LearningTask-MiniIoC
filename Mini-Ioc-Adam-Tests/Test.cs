using System;
using NUnit.Framework;
using MiniIocAdam;
using System.Collections.Generic;
using System.Linq;

namespace MiniIocAdamTests
{
	[TestFixture]
	public class IOCTests
	{
		public interface ITestInterface{}
		public class CustomeTestClass: ITestInterface{}


		[Test]
		public void Register_GenericTypesWithNoId_WillbeAddedToTheList()
		{
			var c = new IoCContainer();

			c.Register<ITestInterface, CustomeTestClass> ();

			Assert.AreEqual (c.RegisteredObjects.Count, 1);
			Assert.IsInstanceOfType(typeof(IocRegister),c.RegisteredObjects.First());
			Assert.AreEqual(c.RegisteredObjects.First().FromType, typeof(ITestInterface));
			Assert.AreEqual(c.RegisteredObjects.First().ToType, typeof(CustomeTestClass));
			Assert.AreEqual(c.RegisteredObjects.First().Id, String.Empty);

		}

		[Test]
		public void Register_GenericTypesWithId_WillbeAddedToTheList()
		{
			var c = new IoCContainer();
			var Id = "Testing";

			c.Register<ITestInterface, CustomeTestClass> (Id);

			Assert.AreEqual (c.RegisteredObjects.Count, 1);
			Assert.IsInstanceOfType(typeof(IocRegister),c.RegisteredObjects.First());
			Assert.AreEqual(c.RegisteredObjects.First().FromType, typeof(ITestInterface));
			Assert.AreEqual(c.RegisteredObjects.First().ToType, typeof(CustomeTestClass));
			Assert.AreEqual(c.RegisteredObjects.First().Id, Id);

		}

		[Test]
		public void Register_WithTypesAndNoId_WillbeAddedToTheList()
		{
			var c = new IoCContainer();

			c.Register(typeof(ITestInterface), typeof(CustomeTestClass));

			Assert.AreEqual (c.RegisteredObjects.Count, 1);
			Assert.IsInstanceOfType(typeof(IocRegister),c.RegisteredObjects.First());
			Assert.AreEqual(c.RegisteredObjects.First().FromType, typeof(ITestInterface));
			Assert.AreEqual(c.RegisteredObjects.First().ToType, typeof(CustomeTestClass));

		}

		[Test]
		public void Register_WithTypesAndId_WillbeAddedToTheList()
		{
			var c = new IoCContainer();
			var Id = "Testing";

			c.Register(typeof(ITestInterface), typeof(CustomeTestClass), Id);

			Assert.AreEqual (c.RegisteredObjects.Count, 1);
			Assert.IsInstanceOfType(typeof(IocRegister),c.RegisteredObjects.First());
			Assert.AreEqual(c.RegisteredObjects.First().FromType, typeof(ITestInterface));
			Assert.AreEqual(c.RegisteredObjects.First().ToType, typeof(CustomeTestClass));
			Assert.AreEqual(c.RegisteredObjects.First().Id, Id);
		}

		[Test]
		public void Register_WithGenericTypesAndDelegatAndNoId_WillbeAddedToTheList()
		{
			var c = new IoCContainer();

			c.Register<ITestInterface, CustomeTestClass>(() => {return new CustomeTestClass();});

			Assert.AreEqual (c.RegisteredObjects.Count, 1);
			Assert.IsInstanceOfType(typeof(IocRegister),c.RegisteredObjects.First());
			Assert.AreEqual(c.RegisteredObjects.First().FromType, typeof(ITestInterface));
			Assert.AreEqual(c.RegisteredObjects.First().ToType, typeof(CustomeTestClass));

		}

		[Test]
		public void Register_WithGenericTypesAndDelegatAndId_WillBeAddedToTheList()
		{
			var c = new IoCContainer();
			var Id = "Testing";

			c.Register<ITestInterface, CustomeTestClass>(() => {return new CustomeTestClass();}, Id);

			Assert.AreEqual (c.RegisteredObjects.Count, 1);
			Assert.IsInstanceOfType(typeof(IocRegister),c.RegisteredObjects.First());
			Assert.AreEqual(c.RegisteredObjects.First().FromType, typeof(ITestInterface));
			Assert.AreEqual(c.RegisteredObjects.First().ToType, typeof(CustomeTestClass));
			Assert.AreEqual(c.RegisteredObjects.First().Id, Id);

		}

		[Test]
		[ExpectedException( "System.Exception" )]
		public void Register_WithGenericTypesAndDelegatAndIdCalledTwice_WillThrowAnException()
		{
			var c = new IoCContainer();
			var Id = "Testing";

			c.Register<ITestInterface, CustomeTestClass> (() => {
				return new CustomeTestClass ();
			}, Id);

			c.Register<ITestInterface, CustomeTestClass> (() => {
				return new CustomeTestClass ();
			}, Id);

		}

		[Test]
		[ExpectedException( "System.Exception" )]
		public void Register_WithGenericTypesAndDelegatAndNoIdCalledTwice_WillThrowAnException()
		{
			var c = new IoCContainer();

			c.Register<ITestInterface, CustomeTestClass> (() => {
				return new CustomeTestClass ();
			});

			c.Register<ITestInterface, CustomeTestClass> (() => {
				return new CustomeTestClass ();
			});

		}

		[Test]
		[ExpectedException( "System.Exception" )]
		public void Register_WithGenericTypesAndNoDelegatAndNoIdCalledTwice_WillThrowAnException()
		{
			var c = new IoCContainer();

			c.Register<ITestInterface, CustomeTestClass> ();

			c.Register<ITestInterface, CustomeTestClass> ();

		}



		[Test]
		public void IsRegistered_WithId_ReturnsTrue()
		{
			var c = new IoCContainer();
			var Id = "Testing";
			c.Register<ITestInterface, CustomeTestClass> (Id);
			Assert.AreEqual (c.IsRegistered (Id), true);

		}

		[Test]
		public void IsRegistered_WithNoIdAndTypes_ReturnsTrue()
		{
			var c = new IoCContainer();
			c.Register<ITestInterface, CustomeTestClass> ();
			Assert.AreEqual (c.IsRegistered<ITestInterface, CustomeTestClass>(), true);

		}

		[Test]
		public void IsRegistered_WithIdAndTypes_ReturnsTrue()
		{
			var c = new IoCContainer();
			var Id = "Testing";
			c.Register<ITestInterface, CustomeTestClass> (Id);
			Assert.AreEqual (c.IsRegistered<ITestInterface, CustomeTestClass>(Id), true);

		}

		[Test]
		public void IsRegistered_WithIdAndToType_ReturnsTrue()
		{
			var c = new IoCContainer();
			var Id = "Testing";
			c.Register<ITestInterface, CustomeTestClass> (Id);
			Assert.AreEqual (c.IsRegistered(typeof(CustomeTestClass), Id), true);

		}

		[Test]
		public void Resolve_WithNoIdAndFromType_ReturnsObject()
		{
			var c = new IoCContainer();
			c.Register<ITestInterface, CustomeTestClass> ();
			Assert.IsInstanceOfType (typeof(ITestInterface), c.Resolve<ITestInterface>());
		}

		[Test]
		public void Resolve_WithIdAndFromType_ReturnsObject()
		{
			var c = new IoCContainer();
			var Id = "Testing";
			c.Register<ITestInterface, CustomeTestClass> (Id);
			Assert.IsInstanceOfType (typeof(ITestInterface), c.Resolve<ITestInterface>(Id));
		}


	}
}

